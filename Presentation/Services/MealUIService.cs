using Edamame.Application.Interfaces;
using Edamame.Application.Services;
using Edamame.Domain.Entities;
using Edamame.Domain.Interfaces;
using System.Text;

namespace Edamame.Presentation.Services
{

    public class MealUIService
    {
        private readonly IMealService _mealService;
        private readonly IRepository<Meal> _mealRepository;
        private readonly IIngredientParser _ingredientParser;

        public MealUIService(
            IMealService mealService,
            IRepository<Meal> mealRepository,
            IIngredientParser ingredientParser)
        {
            _mealService = mealService ?? throw new ArgumentNullException(nameof(mealService));
            _mealRepository = mealRepository ?? throw new ArgumentNullException(nameof(mealRepository));
            _ingredientParser = ingredientParser ?? throw new ArgumentNullException(nameof(ingredientParser));
        }

        /// Validates and creates a meal from UI inputs
        public async Task<Meal> CreateMealAsync(string mealName, MealType mealType, DateTime mealDate, string recipesText)
        {
            if (string.IsNullOrWhiteSpace(mealName))
                throw new ArgumentException("Meal name cannot be empty");

            if (string.IsNullOrWhiteSpace(recipesText))
                throw new ArgumentException("Please enter recipes and ingredients");

            var ingredients = ParseIngredients(recipesText);
            if (ingredients.Count == 0)
                throw new ArgumentException(GetIngredientParsingHelpText());

            // Create recipe using Add method
            var recipe = new Recipe(name: "Main Recipe", description: null);
            recipe.AddIngredients(ingredients);

            // Create meal using constructor validation
            var meal = new Meal(name: mealName, type: mealType, mealDate: mealDate);
            meal.AddRecipe(recipe);

            return await _mealService.CreateAndAnalyzeMealAsync(meal);
        }

        /// Updates an existing meal
        public async Task<Meal> UpdateMealAsync(Meal meal, string mealName, MealType mealType, DateTime mealDate, string recipesText)
        {
            meal.Name = mealName;  // Triggers validation
            meal.Type = mealType;  // Triggers validation
            meal.MealDate = mealDate;

            var ingredients = ParseIngredients(recipesText);
            if (ingredients.Count > 0)
            {
                meal.ClearRecipes();  // Clear existing recipes
                
                var recipe = new Recipe(name: "Main Recipe");
                recipe.AddIngredients(ingredients);
                meal.AddRecipe(recipe);
            }

            await _mealRepository.UpdateAsync(meal.Id.ToString(), meal);
            return meal;
        }

        /// Deletes a meal by ID
        public async Task DeleteMealAsync(string mealId)
        {
            await _mealRepository.DeleteAsync(mealId);
        }

        /// Gets all meals from repository
        public async Task<List<Meal>> GetAllMealsAsync()
        {
            var meals = await _mealRepository.GetAllAsync();
            return meals.ToList();
        }

        /// parses ingredient text into ingredient objects
        private List<Ingredient> ParseIngredients(string recipesText)
        {
            if (string.IsNullOrWhiteSpace(recipesText))
                return new List<Ingredient>();

            var ingredients = new List<Ingredient>();
            var lines = recipesText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine)) continue;

                try
                {
                    var parsed = _ingredientParser.Parse(trimmedLine);
                    if (parsed != null)
                    {
                        ingredients.Add(parsed);
                    }
                }
                catch (ArgumentException)
                {
                    // skip ingredients that fail validation
                    continue;
                }
            }

            return ingredients;
        }

        private string GetIngredientParsingHelpText() =>
            "Please enter at least one ingredient. Examples:\n" +
            "• 1kg chicken breast\n" +
            "• chicken breast 1kg\n" +
            "• 2 cups flour\n" +
            "• 1/2 cup dry rolled oats\n" +
            "• 1/3 cup sugar\n" +
            "• 3/4 cup milk\n" +
            "• 1 1/2 tablespoons\n" +
            "• 500ml water\n" +
            "• 3 apples";
    }
}

