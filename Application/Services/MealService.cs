using Edamame.Domain.Entities;
using Edamame.Domain.Interfaces;

namespace Edamame.Application.Services;


public class MealService : IMealService
{
    private readonly IRepository<Meal> _mealRepository;
    private readonly IRepository<Recipe> _recipeRepository;
    private readonly INutritionAnalysisService _nutritionService;
    private readonly IDailyMealAggregator _dailyAggregator;

    public MealService(
        IRepository<Meal> mealRepository,
        IRepository<Recipe> recipeRepository,
        INutritionAnalysisService nutritionService,
        IDailyMealAggregator dailyAggregator)
    {
        _mealRepository = mealRepository ?? throw new ArgumentNullException(nameof(mealRepository));
        _recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
        _nutritionService = nutritionService ?? throw new ArgumentNullException(nameof(nutritionService));
        _dailyAggregator = dailyAggregator ?? throw new ArgumentNullException(nameof(dailyAggregator));
    }

    /// create and analyze meal
    public async Task<Meal> CreateAndAnalyzeMealAsync(Meal meal)
    {
        if (meal == null) throw new ArgumentNullException(nameof(meal));

        // Analyze nutritional content
        meal.Nutritionals = await _nutritionService.AnalyzeMealAsync(meal);

        // Persist to database - LiteDB will automatically assign an ObjectId
        await _mealRepository.CreateAsync(meal);

        return meal;
    }

    /// sget all meals for a specific date
    public async Task<IEnumerable<Meal>> GetMealsForDateAsync(DateTime date)
    {
        return await _dailyAggregator.GetMealsForDateAsync(date);
    }

    /// get the daily nutritional summary
    public async Task<string> GetDailySummaryAsync(DateTime date)
    {
        return await _dailyAggregator.GetDailySummaryAsync(date);
    }

    /// retrieve a meal by ID
    public async Task<Meal?> GetMealByIdAsync(string id)
    {
        return await _mealRepository.GetByIdAsync(id);
    }

    /// update a meal's nutritional information
    public async Task<bool> UpdateMealAsync(string id, Meal meal)
    {
        return await _mealRepository.UpdateAsync(id, meal);
    }

    /// delete a meal by ID
    public async Task<bool> DeleteMealAsync(string id)
    {
        return await _mealRepository.DeleteAsync(id);
    }

    /// create and analyze a recipe
    public async Task<Recipe> CreateAndAnalyzeRecipeAsync(Recipe recipe)
    {
        if (recipe == null) throw new ArgumentNullException(nameof(recipe));

        // LiteDB will automatically assign an ObjectId during Insert
        await _recipeRepository.CreateAsync(recipe);

        return recipe;
    }

    /// get all recipes
    public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
    {
        return await _recipeRepository.GetAllAsync();
    }
}
