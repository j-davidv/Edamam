using System.Text.Json;
using System.Text.Json.Serialization;
using Google.GenAI;
using Google.GenAI.Types;
using Edamame.Domain.Entities;
using Edamame.Domain.Interfaces;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Edamame.Infrastructure.ExternalServices;

public class GeminiNutritionAnalysisService : INutritionAnalysisService
{
    private readonly Client _geminiClient;
    private const string MODEL_NAME = "gemini-2.5-flash";
    private readonly ConcurrentDictionary<string, GeminiNutritionData> _nutritionCache = new();

    public GeminiNutritionAnalysisService(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));

        _geminiClient = new Client(apiKey: apiKey);
    }

    public async Task<NutritionalMetric> AnalyzeMealAsync(Meal meal)
    {
        if (meal == null) throw new ArgumentNullException(nameof(meal));

        var allIngredients = meal.Recipes
            .Where(r => r != null && r.Ingredients != null)
            .SelectMany(r => r.Ingredients)
            .Where(i => i != null && !string.IsNullOrWhiteSpace(i.Name))
            .Select(i => $"{i.Quantity} {i.Unit} {i.Name}")
            .ToList();

        if (allIngredients.Count == 0)
        {
            throw new InvalidOperationException($"Meal '{meal.Name}' has no ingredients to analyze. Please provide at least one ingredient.");
        }

        try
        {
            var nutritionalData = await CallGeminiApiAsync(allIngredients);

            return new NutritionalMetric
            {
                MealName = meal.Name,
                Calories = (decimal)nutritionalData.Calories,
                Protein = (decimal)nutritionalData.Protein,
                Carbohydrates = (decimal)nutritionalData.Carbohydrates,
                Fat = (decimal)nutritionalData.Fat,
                Sodium = (decimal)nutritionalData.Sodium,
                Sugar = (decimal)nutritionalData.Sugar,
                SaturatedFat = (decimal)nutritionalData.SaturatedFat,
                DietaryClassification = DetermineDietaryClassification(nutritionalData),
                DietaryAdvice = GenerateDietaryAdvice(nutritionalData),
                AnalyzedDate = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"HTTP error calling Gemini API: {ex.Message}", ex);
        }
        catch (OperationCanceledException ex)
        {
            throw new InvalidOperationException("Gemini API request timed out. Check your internet connection.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error analyzing meal: {ex.Message}", ex);
        }
    }

    public async Task<NutritionalMetric> AnalyzeRecipeAsync(Recipe recipe)
    {
        if (recipe == null) throw new ArgumentNullException(nameof(recipe));

        if (recipe.Ingredients.Count == 0)
        {
            throw new InvalidOperationException("Recipe has no ingredients to analyze.");
        }

        try
        {
            var ingredients = recipe.Ingredients
                .Select(i => $"{i.Quantity} {i.Unit} {i.Name}")
                .ToList();

            var nutritionalData = await CallGeminiApiAsync(ingredients);

            return new NutritionalMetric
            {
                MealName = recipe.Name,
                Calories = (decimal)nutritionalData.Calories,
                Protein = (decimal)nutritionalData.Protein,
                Carbohydrates = (decimal)nutritionalData.Carbohydrates,
                Fat = (decimal)nutritionalData.Fat,
                Sodium = (decimal)nutritionalData.Sodium,
                Sugar = (decimal)nutritionalData.Sugar,
                SaturatedFat = (decimal)nutritionalData.SaturatedFat,
                DietaryClassification = DetermineDietaryClassification(nutritionalData),
                DietaryAdvice = GenerateDietaryAdvice(nutritionalData),
                AnalyzedDate = DateTime.UtcNow
            };
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"HTTP error calling Gemini API: {ex.Message}", ex);
        }
        catch (OperationCanceledException ex)
        {
            throw new InvalidOperationException("Gemini API request timed out. Check your internet connection.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error analyzing recipe: {ex.Message}", ex);
        }
    }

    public async Task<string> GetDietaryAdviceAsync(Meal meal)
    {
        if (meal == null) throw new ArgumentNullException(nameof(meal));

        if (meal.Nutritionals == null)
        {
            return "No nutritional data available. Analyze the meal first.";
        }

        var advice = new List<string>();

        if (meal.Nutritionals.Calories > 2500)
            advice.Add("This meal is calorie-dense. Consider portion control.");
        else if (meal.Nutritionals.Calories < 300)
            advice.Add("This is a light meal. Great for snacks or side dishes.");

        if (meal.Nutritionals.Protein < 10)
            advice.Add("Consider adding more protein sources for better satiety.");
        else if (meal.Nutritionals.Protein > 50)
            advice.Add("This meal has excellent protein content for muscle support.");

        if (meal.Nutritionals.SaturatedFat > 15)
            advice.Add("Reduce saturated fat intake by choosing leaner options.");

        if (meal.Nutritionals.Sodium > 2000)
            advice.Add("Monitor sodium levels; this meal exceeds daily recommendations.");

        if (advice.Count == 0)
            advice.Add("This meal has balanced nutritional composition.");

        return string.Join(" ", advice);
    }

    private async Task<GeminiNutritionData> CallGeminiApiAsync(List<string> ingredients)
    {
        try
        {
            // ----- FIX 1: canonicalize ingredient lines (parse qty/unit/name) -----
            var normalizedIngredients = ingredients
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i =>
                {
                    var s = i.Trim();
                    var m = Regex.Match(s, @"^\s*(\d+(?:[.,]\d+)?)?\s*([^\s\d]+)?\s*(.*)$");
                    if (m.Success)
                    {
                        var qtyGroup = m.Groups[1].Value;
                        var unitGroup = (m.Groups[2].Value ?? string.Empty).Trim().ToLowerInvariant();
                        var nameGroup = (m.Groups[3].Value ?? string.Empty).Trim().ToLowerInvariant();
                        double qty = 0;
                        if (!string.IsNullOrEmpty(qtyGroup))
                        {
                            var qtyStr = qtyGroup.Replace(",", ".");
                            double.TryParse(qtyStr, NumberStyles.Any, CultureInfo.InvariantCulture, out qty);
                        }
                        if (!string.IsNullOrEmpty(unitGroup))
                        {
                            if (unitGroup.StartsWith("kg")) { qty *= 1000; unitGroup = "g"; }
                            else if (unitGroup.StartsWith("gram") || unitGroup == "g" || unitGroup.StartsWith("gr")) unitGroup = "g";
                            else if (unitGroup.StartsWith("mg")) { qty /= 1000; unitGroup = "g"; }
                        }
                        else
                        {
                            unitGroup = "serving";
                        }
                        var qtyStrNormalized = qty > 0 ? qty.ToString("0.###", CultureInfo.InvariantCulture) : "1";
                        var canonical = $"{qtyStrNormalized} {unitGroup} {nameGroup}".Trim();
                        return Regex.Replace(canonical, @"\s+", " ");
                    }
                    return s.ToLowerInvariant();
                })
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x)
                .ToList();

            var cacheKey = string.Join("|", normalizedIngredients);

            if (_nutritionCache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var ingredientsList = string.Join("\n", normalizedIngredients);

            var prompt = $@"Analyze the following ingredients and calculate the TOTAL nutritional values for the COMBINED meal (sum of all listed ingredients). Do NOT return per-serving values or attempt to infer serving sizes — treat the list as a single combined meal. Ignore any meal name, meal type, date, or other metadata; base your calculations ONLY on the ingredient lines below.
Ingredients:
{ingredientsList}

Return ONLY a valid JSON object with no additional text or formatting (no markdown, no code blocks) containing exactly these fields:
{{
  ""calories"": <number - total calories>,
  ""protein"": <number - grams of protein>,
  ""carbohydrates"": <number - grams of total carbohydrates>,
  ""fat"": <number - grams of total fat>,
  ""sodium"": <number - milligrams of sodium>,
  ""sugar"": <number - grams of total sugar>,
  ""saturatedFat"": <number - grams of saturated fat>
}}

Ensure all values are numbers and non-negative. If a value cannot be determined, use 0. Be deterministic: do not include explanatory text or units — only numeric values for the fields listed above.";

            System.Diagnostics.Debug.WriteLine($"Gemini Request: {prompt}");

            GenerateContentResponse? response = null;
            int maxAttempts = 4;
            int delayMs = 800;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    // ----- FIX 2: Temperature=0 for deterministic outputs -----
                    response = await _geminiClient.Models.GenerateContentAsync(
                        model: MODEL_NAME,
                        contents: prompt,
                        config: new GenerateContentConfig { Temperature = 0f }
                    );
                    break;
                }
                catch (Exception ex) when (IsTransient(ex) && attempt < maxAttempts)
                {
                    System.Diagnostics.Debug.WriteLine($"Gemini transient error (attempt {attempt}): {ex.Message}");
                    await Task.Delay(delayMs + (new Random()).Next(0, 200));
                    delayMs *= 2;
                    continue;
                }
            }

            if (response == null)
                throw new InvalidOperationException("Gemini API returned no response after retries.");

            var responseText = ExtractResponseText(response);

            System.Diagnostics.Debug.WriteLine($"Gemini Response: {responseText}");

            var cleanedResponse = CleanJsonResponse(responseText);

            using (var doc = JsonDocument.Parse(cleanedResponse))
            {
                var root = doc.RootElement;

                var nutritionData = new GeminiNutritionData
                {
                    Calories = ExtractNumericValue(root, "calories"),
                    Protein = ExtractNumericValue(root, "protein"),
                    Carbohydrates = ExtractNumericValue(root, "carbohydrates"),
                    Fat = ExtractNumericValue(root, "fat"),
                    Sodium = ExtractNumericValue(root, "sodium"),
                    Sugar = ExtractNumericValue(root, "sugar"),
                    SaturatedFat = ExtractNumericValue(root, "saturatedFat")
                };

                if (nutritionData.Calories == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"WARNING: No calorie data found in Gemini response. Response: {cleanedResponse}");
                    throw new InvalidOperationException($"Gemini API response missing calorie data. Please check your ingredients are valid (e.g., '200 gram chicken breast').");
                }

                try
                {
                    _nutritionCache.TryAdd(cacheKey, nutritionData);
                }
                catch
                {
                    // ignore caching errors - not critical
                }

                return nutritionData;
            }
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Error parsing Gemini JSON response: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error calling Gemini API: {ex.Message}", ex);
        }
    }

    private static string CleanJsonResponse(string response)
    {
        response = response.Trim();

        if (response.StartsWith("```"))
        {
            var startIndex = response.IndexOf('\n');
            if (startIndex > 0)
            {
                response = response.Substring(startIndex + 1);
            }
            else
            {
                response = response.Substring(3);
            }
        }

        if (response.EndsWith("```"))
        {
            response = response.Substring(0, response.Length - 3);
        }

        return response.Trim();
    }

    private static double ExtractNumericValue(JsonElement root, string propertyName)
    {
        if (root.TryGetProperty(propertyName, out var element))
        {
            return ExtractDouble(element);
        }

        foreach (var property in root.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                return ExtractDouble(property.Value);
            }
        }

        return 0;
    }

    private static bool IsTransient(Exception ex)
    {
        if (ex is System.Net.Http.HttpRequestException) return true;
        var msg = ex.Message?.ToLowerInvariant() ?? string.Empty;
        if (msg.Contains("high demand") || msg.Contains("rate limit") || msg.Contains("429") || msg.Contains("503") || msg.Contains("timeout"))
            return true;
        return false;
    }

    private static string ExtractResponseText(GenerateContentResponse? response)
    {
        var candidates = response?.Candidates;
        if (candidates == null || candidates.Count == 0)
            throw new InvalidOperationException("Gemini API returned no candidates in response.");

        var parts = candidates[0]?.Content?.Parts;
        if (parts == null || parts.Count == 0)
            throw new InvalidOperationException("Gemini API returned no content parts in response.");

        var responseText = parts[0]?.Text;
        if (string.IsNullOrWhiteSpace(responseText))
            throw new InvalidOperationException("Gemini API returned empty response text.");

        return responseText;
    }

    private static double ExtractDouble(JsonElement element)
    {
        try
        {
            if (element.ValueKind == JsonValueKind.Number)
            {
                return element.GetDouble();
            }
            else if (element.ValueKind == JsonValueKind.String)
            {
                var str = element.GetString();
                if (double.TryParse(str, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var result))
                {
                    return result;
                }
            }
        }
        catch
        {
        }

        return 0;
    }

    private static string DetermineDietaryClassification(GeminiNutritionData data)
    {
        var classifications = new List<string>();

        if (data.Protein > 30)
            classifications.Add("High Protein");

        if (data.Calories < 400)
            classifications.Add("Low Calorie");

        if (data.Fat < 10)
            classifications.Add("Low Fat");

        if (data.Carbohydrates > 40)
            classifications.Add("High Carb");

        if (classifications.Count == 0)
            classifications.Add("Balanced");

        return string.Join(", ", classifications);
    }

    private static string GenerateDietaryAdvice(GeminiNutritionData data)
    {
        if (data.Calories > 2000)
            return "High calorie meal - suitable for post-workout recovery or active individuals.";

        if (data.Protein > 40)
            return "Excellent protein content - great for muscle recovery and satiety.";

        if (data.Fat < 5)
            return "Very lean meal - ideal for weight management.";

        if (data.Sodium > 1500)
            return "Consider this sodium-rich meal as occasional rather than daily.";

        return "Well-balanced meal suitable for most dietary goals.";
    }
}

internal class GeminiNutritionData
{
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbohydrates { get; set; }
    public double Fat { get; set; }
    public double Sodium { get; set; }
    public double Sugar { get; set; }
    public double SaturatedFat { get; set; }
}
