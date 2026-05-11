using Edamame.Domain.Entities;

namespace Edamame.Application.Interfaces;

/// interface for ingredient parsing
public interface IIngredientParser
{
    /// parse single line igredients to ingredient object, null if fails
    Ingredient? Parse(string input);

    /// parse multiple lines
    List<Ingredient> ParseMultiple(string multiLineText);
}
