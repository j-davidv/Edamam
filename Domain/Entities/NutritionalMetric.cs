namespace Edamame.Domain.Entities;

public class NutritionalMetric : NutritionalBase
{
    private string _mealName = string.Empty;
    public string MealName
    {
        get => _mealName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Meal name cannot be empty", nameof(value));
            _mealName = value.Trim();
        }
    }

    public DateTime AnalyzedDate { get; set; } = DateTime.UtcNow;

    public string? DietaryClassification { get; set; } // e.g., "Vegan", "Gluten-Free"

    public string? DietaryAdvice { get; set; }

    private int _servingSize = 1;
    public int ServingSize
    {
        get => _servingSize;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Serving size must be greater than zero", nameof(value));
            _servingSize = value;
        }
    }

    public NutritionalMetric()
    {
    }

    public NutritionalMetric(string mealName)
    {
        MealName = mealName;
        ValidateState();
    }

    /// metric-specific checks
    public override void ValidateState()
    {
        base.ValidateState();

        if (string.IsNullOrWhiteSpace(MealName))
            throw new InvalidOperationException("Meal name is required for a nutritional metric");

        if (ServingSize <= 0)
            throw new InvalidOperationException("Serving size must be positive");
    }

    public override string ToString()
    {
        return $"{MealName} - {Calories} kcal, Protein: {Protein}g, Carbs: {Carbohydrates}g, Fat: {Fat}g";
    }
}
