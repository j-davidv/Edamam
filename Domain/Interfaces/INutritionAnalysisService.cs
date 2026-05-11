using Edamame.Domain.Entities;

namespace Edamame.Domain.Interfaces;

/// abstraction for nutrition analysis service
/// defines contract for AI-based nutritional analysis
public interface INutritionAnalysisService
{
    Task<NutritionalMetric> AnalyzeMealAsync(Meal meal);

    Task<NutritionalMetric> AnalyzeRecipeAsync(Recipe recipe);

    /// Gets dietary advice for a meal
    Task<string> GetDietaryAdviceAsync(Meal meal);
}
