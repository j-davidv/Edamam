using LiteDB;

namespace Edamame.Domain.Entities;

public class Recipe : EntityBase
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Recipe name cannot be empty", nameof(value));
            _name = value.Trim();
        }
    }

    public string? Description { get; set; }

    public List<Ingredient> Ingredients { get; set; } = new();

    public Recipe(string name, string? description = null)
    {
        Name = name;  // Triggers validation
        Description = description;
        ValidateState();  
    }

    public Recipe() { }

    /// check complete state of the recipe
    public override void ValidateState()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Recipe must have a valid name");

        // Validate all ingredients
        Ingredients ??= new List<Ingredient>();
        foreach (var ingredient in Ingredients)
        {
            ingredient.ValidateState();
        }
    }

    /// get display name for the recipe
    public override string GetDisplayName()
    {
        return $"{Name} ({Ingredients?.Count ?? 0} ingredients)";
    }

    /// add ingredient
    public virtual void AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
            throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null");
        ingredient.ValidateState();
        Ingredients ??= new List<Ingredient>();
        Ingredients.Add(ingredient);
        MarkAsModified();
    }

    /// add multiple ingredients at once
    public void AddIngredients(IEnumerable<Ingredient> ingredients)
    {
        if (ingredients == null)
            throw new ArgumentNullException(nameof(ingredients));
        
        foreach (var ingredient in ingredients)
        {
            AddIngredient(ingredient);
        }
    }

    /// remove ingredient by ID
    public bool RemoveIngredient(ObjectId ingredientId)
    {
        Ingredients ??= new List<Ingredient>();
        var removed = Ingredients.RemoveAll(i => i.Id == ingredientId) > 0;
        if (removed)
            MarkAsModified();
        return removed;
    }

    /// clear all ingredients
    public virtual void ClearIngredients()
    {
        Ingredients ??= new List<Ingredient>();
        if (Ingredients.Count > 0)
        {
            Ingredients.Clear();
            MarkAsModified();
        }
    }

    public override string ToString() => $"Recipe: {Name} ({Ingredients?.Count ?? 0} ingredients)";
}


