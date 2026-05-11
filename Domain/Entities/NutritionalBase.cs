namespace Edamame.Domain.Entities;

public abstract class NutritionalBase
{
    private decimal _calories;
    public decimal Calories
    {
        get => _calories;
        set
        {
            if (value < 0)
                throw new ArgumentException("Calories cannot be negative", nameof(value));
            _calories = value;
        }
    }

    private decimal _protein;
    public decimal Protein
    {
        get => _protein;
        set
        {
            if (value < 0)
                throw new ArgumentException("Protein cannot be negative", nameof(value));
            _protein = value;
        }
    }

    private decimal _carbohydrates;
    public decimal Carbohydrates
    {
        get => _carbohydrates;
        set
        {
            if (value < 0)
                throw new ArgumentException("Carbohydrates cannot be negative", nameof(value));
            _carbohydrates = value;
        }
    }

    private decimal _fat;
    public decimal Fat
    {
        get => _fat;
        set
        {
            if (value < 0)
                throw new ArgumentException("Fat cannot be negative", nameof(value));
            _fat = value;
        }
    }

    private decimal _sodium;
    public decimal Sodium
    {
        get => _sodium;
        set
        {
            if (value < 0)
                throw new ArgumentException("Sodium cannot be negative", nameof(value));
            _sodium = value;
        }
    }

    private decimal _sugar;
    public decimal Sugar
    {
        get => _sugar;
        set
        {
            if (value < 0)
                throw new ArgumentException("Sugar cannot be negative", nameof(value));
            _sugar = value;
        }
    }

    private decimal _saturatedFat;
    public decimal SaturatedFat
    {
        get => _saturatedFat;
        set
        {
            if (value < 0)
                throw new ArgumentException("Saturated fat cannot be negative", nameof(value));
            _saturatedFat = value;
        }
    }

    /// method for nutritional state
    /// ensures all fields are non-negative
    public virtual void ValidateState()
    {
        if (Calories < 0) throw new InvalidOperationException("Invalid nutritional state: Calories");
        if (Protein < 0) throw new InvalidOperationException("Invalid nutritional state: Protein");
        if (Carbohydrates < 0) throw new InvalidOperationException("Invalid nutritional state: Carbohydrates");
        if (Fat < 0) throw new InvalidOperationException("Invalid nutritional state: Fat");
        if (Sodium < 0) throw new InvalidOperationException("Invalid nutritional state: Sodium");
        if (Sugar < 0) throw new InvalidOperationException("Invalid nutritional state: Sugar");
        if (SaturatedFat < 0) throw new InvalidOperationException("Invalid nutritional state: SaturatedFat");
    }
    protected virtual void Validate()
    {
        ValidateState();
    }

    /// Check if data is in valid state
    public bool IsValid()
    {
        try
        {
            Validate();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
