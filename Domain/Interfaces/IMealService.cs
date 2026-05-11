using Edamame.Domain.Entities;

namespace Edamame.Domain.Interfaces;

public interface IMealService
{
    /// create, analyze metrics
    Task<Meal> CreateAndAnalyzeMealAsync(Meal meal);

    /// get em all meals for a specific date
    Task<IEnumerable<Meal>> GetMealsForDateAsync(DateTime date);

    /// get daily nutritional summary as string
    Task<string> GetDailySummaryAsync(DateTime date);

    /// retrieve a meal by ID
    Task<Meal?> GetMealByIdAsync(string id);

    /// update a meal's information
    Task<bool> UpdateMealAsync(string id, Meal meal);

    /// delete a meal by ID
    Task<bool> DeleteMealAsync(string id);

    /// create and analyze a recipe
    Task<Recipe> CreateAndAnalyzeRecipeAsync(Recipe recipe);
}
