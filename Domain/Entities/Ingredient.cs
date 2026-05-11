using LiteDB;

namespace Edamame.Domain.Entities;

public class Ingredient : EntityBase
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ingredient name cannot be empty or whitespace", nameof(value));
            _name = value.Trim();
        }
    }

    private string _quantityString = string.Empty;
    public string QuantityString
    {
        get => _quantityString;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Quantity string cannot be empty or whitespace", nameof(value));
            _quantityString = value.Trim();
        }
    }

    private decimal _quantity;
    /// decimal calculation
    public decimal Quantity
    {
        get => _quantity;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(value));
            _quantity = value;
        }
    }

    private string _unit = string.Empty;
    public string Unit
    {
        get => _unit;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Unit cannot be empty or whitespace", nameof(value));
            _unit = value.Trim();
        }
    }

    // Constructor for validation on creation
    public Ingredient(string name, decimal quantity, string unit, string? quantityString = null)
    {
        Name = name;      // Triggers validation
        Quantity = quantity;  // Triggers validation
        Unit = unit;      // Triggers validation
        QuantityString = quantityString ?? quantity.ToString(); 
        ValidateState();  
    }

    // parameterless constructor for liteDB 
    public Ingredient() { }

    /// validate the complete state of the ingredient
    public override void ValidateState()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Ingredient must have a valid name");

        if (_quantity <= 0)
            throw new InvalidOperationException("Ingredient quantity must be greater than zero");

        if (string.IsNullOrWhiteSpace(_unit))
            throw new InvalidOperationException("Ingredient must have a valid unit");

        if (string.IsNullOrWhiteSpace(_quantityString))
            throw new InvalidOperationException("Ingredient must have a valid quantity string");
    }

    /// get display name for the ingredient
    public override string GetDisplayName()
    {
        return $"{_quantityString} {_unit} {_name}";
    }

    public override string ToString() => $"{QuantityString} {Unit} {Name}";
}
