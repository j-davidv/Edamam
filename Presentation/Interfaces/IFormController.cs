using Edamame.Domain.Entities;
using Edamame.Presentation.Models;
using Edamame.Presentation.Services;

namespace Edamame.Presentation.Interfaces;

/// interface form controller 
public interface IFormController
{
    Task InitializeAsync();

    double LoadDailyCalorieGoal();

    // meal management
    Task<Meal> CreateMealAsync(string name, MealType type, DateTime date, string recipesText);
    Task UpdateMealAsync(Meal meal, string name, MealType type, DateTime date, string recipesText);
    Task DeleteMealAsync(string mealId);
    Task RefreshMealsAsync();
    List<Meal> GetAllMeals();

    // dashboard management
    void SetFilterParameters(MealFilterParameters.FilterType filterType, DateTime selectedDate);
    DashboardData GetDashboardData();
    List<Meal> GetFilteredMeals();
    (double Current, double Goal, double Percent) GetCalorieProgress(double dailyGoal);
    void SaveDailyCalorieGoal(double goal);

    // BMI management
    BmiUIService.BmiResult? CalculateBmi(string heightText, string weightText);

    // chat management
    bool IsChatAvailable { get; }
    string GetChatUnavailableMessage();
    Task<string?> SendChatMessageAsync(string message);
    string GetChatErrorTitle(Exception ex);
    string GetChatErrorMessage(Exception ex);
}
