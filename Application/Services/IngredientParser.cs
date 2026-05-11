using System.Text.RegularExpressions;
using Edamame.Domain.Entities;
using Edamame.Application.Interfaces;

namespace Edamame.Application.Services;

public class IngredientParser : IIngredientParser
{
    private readonly Dictionary<string, string> _unitAbbreviations = new(StringComparer.OrdinalIgnoreCase)
    {
        // Weight
        { "kg", "kilogram" },
        { "g", "gram" },
        { "mg", "milligram" },
        { "lb", "pound" },
        { "lbs", "pound" },
        { "oz", "ounce" },
        { "oz.", "ounce" },

        // Volume
        { "ml", "milliliter" },
        { "l", "liter" },
        { "cl", "centiliter" },
        { "cup", "cup" },
        { "tbsp", "tablespoon" },
        { "tsp", "teaspoon" },
        { "fl oz", "fluid ounce" },

        // Others
        { "pcs", "piece" },
        { "pc", "piece" },
        { "pcs.", "piece" },
        { "count", "piece" },
    };

    /// parse single ingredient
    public Ingredient? Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var trimmedInput = input.Trim();

        // pattern 1
        var ingredient = TryParseQuantityFirst(trimmedInput);
        if (ingredient != null)
            return ingredient;

        // pattern 2
        ingredient = TryParseQuantityLast(trimmedInput);
        if (ingredient != null)
            return ingredient;

        // pattern 3
        ingredient = TryParseNameOnly(trimmedInput);
        if (ingredient != null)
            return ingredient;

        return null;
    }

    /// converts fraction string to decimal
    private decimal ParseQuantity(string quantityString)
    {
        if (string.IsNullOrWhiteSpace(quantityString))
            throw new FormatException("Quantity cannot be empty");

        var trimmed = quantityString.Trim();

        // check for mixed fraction
        var mixedPattern = @"^(\d+(?:\.\d+)?)\s+(\d+)/(\d+)$";
        var mixedMatch = Regex.Match(trimmed, mixedPattern);
        if (mixedMatch.Success)
        {
            if (decimal.TryParse(mixedMatch.Groups[1].Value, out var wholePart) &&
                decimal.TryParse(mixedMatch.Groups[2].Value, out var numerator) &&
                decimal.TryParse(mixedMatch.Groups[3].Value, out var denominator) &&
                denominator != 0)
            {
                return wholePart + (numerator / denominator);
            }
        }

        // check for simple fraction
        var fractionPattern = @"^(\d+)/(\d+)$";
        var fractionMatch = Regex.Match(trimmed, fractionPattern);
        if (fractionMatch.Success)
        {
            if (decimal.TryParse(fractionMatch.Groups[1].Value, out var numerator) &&
                decimal.TryParse(fractionMatch.Groups[2].Value, out var denominator) &&
                denominator != 0)
            {
                return numerator / denominator;
            }
        }

        // fll back to standard decimal parsing
        if (decimal.TryParse(trimmed, out var result))
            return result;

        throw new FormatException($"Invalid quantity format: {quantityString}");
    }

    /// parse multiple lines at once
    public List<Ingredient> ParseMultiple(string multiLineText)
    {
        if (string.IsNullOrWhiteSpace(multiLineText))
            return new List<Ingredient>();

        return multiLineText
            .Split('\n')
            .Select(line => Parse(line.Trim()))
            .Where(ingredient => ingredient != null)
            .Cast<Ingredient>()
            .ToList();
    }

    private Ingredient? TryParseQuantityFirst(string input)
    {
        var pattern = @"^(?<quantity>[\d./ ]+?)\s*(?<unit>[a-zA-Z]+\.?)\s+(?<name>.+)$";
        var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

        if (!match.Success)
            return null;

        try
        {
            var quantityString = match.Groups["quantity"].Value.Trim();
            var quantity = ParseQuantity(quantityString);
            var unit = NormalizeUnit(match.Groups["unit"].Value);
            var name = match.Groups["name"].Value.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return null;

            return new Ingredient(name, quantity, unit, quantityString);
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (FormatException)
        {
            return null;
        }
    }

    private Ingredient? TryParseQuantityLast(string input)
    {
        var pattern = @"^(?<name>.+?)\s+(?<quantity>[\d./ ]+?)\s*(?<unit>[a-zA-Z]+\.?)$";
        var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

        if (!match.Success)
            return null;

        try
        {
            var quantityString = match.Groups["quantity"].Value.Trim();
            var quantity = ParseQuantity(quantityString);
            var name = match.Groups["name"].Value.Trim();
            var unit = NormalizeUnit(match.Groups["unit"].Value);

            if (string.IsNullOrWhiteSpace(name))
                return null;

            return new Ingredient(name, quantity, unit, quantityString);
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (FormatException)
        {
            return null;
        }
    }

    private Ingredient? TryParseNameOnly(string input)
    {
        var pattern = @"^(?<quantity>[\d./ ]+?)\s+(?<name>.+)$";
        var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);

        if (!match.Success)
            return null;

        try
        {
            var quantityString = match.Groups["quantity"].Value.Trim();
            var quantity = ParseQuantity(quantityString);
            var name = match.Groups["name"].Value.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return null;

            return new Ingredient(name, quantity, "piece", quantityString);
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (FormatException)
        {
            return null;
        }
    }

    private string NormalizeUnit(string unit)
    {
        if (string.IsNullOrWhiteSpace(unit))
            return "piece";

        var trimmedUnit = unit.Trim().ToLowerInvariant();

        if (_unitAbbreviations.TryGetValue(trimmedUnit, out var fullUnit))
            return fullUnit;

        return trimmedUnit;
    }

    /// check if text looks like an ingredient line
    public bool LooksLikeIngredient(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        // Check if input contains any digit
        return Regex.IsMatch(input, @"\d");
    }
}

