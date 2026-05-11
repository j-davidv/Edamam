using Edamame.Domain.Entities;
using Edamame.Domain.Interfaces;

namespace Edamame.Application.Services;


/// <summary>
/// Implements IDailyMealAggregator for daily nutritional computation
/// computes meals from LiteDB and calculates daily totals
/// </summary>


public class DailyMealAggregator : IDailyMealAggregator
{
    private readonly IRepository<Meal> _mealRepository;

    public DailyMealAggregator(IRepository<Meal> mealRepository)
    {
        _mealRepository = mealRepository ?? throw new ArgumentNullException(nameof(mealRepository));
    }

    // Gets all meals for a specific date
    public async Task<IEnumerable<Meal>> GetMealsForDateAsync(DateTime date)
    {
        var allMeals = await _mealRepository.GetAllAsync();
        var targetDate = date.Date;

        return allMeals.Where(m => m.MealDate.Date == targetDate).ToList();
    }

    // Calculates daily totals for all nutritional metrics

    public async Task<NutritionalMetric> GetDailyTotalsAsync(DateTime date)
    {
        var mealsForDay = await GetMealsForDateAsync(date);
        
        if (!mealsForDay.Any())
        {
            return new NutritionalMetric
            {
                MealName = $"Daily Total - {date:yyyy-MM-dd}",
                AnalyzedDate = date
            };
        }

        var totals = new NutritionalMetric
        {
            MealName = $"Daily Total - {date:yyyy-MM-dd}",
            AnalyzedDate = date,
            Calories = 0,
            Protein = 0,
            Carbohydrates = 0,
            Fat = 0,
            Sodium = 0,
            Sugar = 0,
            SaturatedFat = 0
        };

        // compiles nutritional data from all meals
        foreach (var meal in mealsForDay)
        {
            if (meal.Nutritionals != null)
            {
                totals.Calories += meal.Nutritionals.Calories;
                totals.Protein += meal.Nutritionals.Protein;
                totals.Carbohydrates += meal.Nutritionals.Carbohydrates;
                totals.Fat += meal.Nutritionals.Fat;
                totals.Sodium += meal.Nutritionals.Sodium;
                totals.Sugar += meal.Nutritionals.Sugar;
                totals.SaturatedFat += meal.Nutritionals.SaturatedFat;
            }
        }

        return totals;
    }

    public async Task<string> GetDailySummaryAsync(DateTime date)
    {
        var dailyTotals = await GetDailyTotalsAsync(date);
        var mealsCount = (await GetMealsForDateAsync(date)).Count();

        return $@"
📅 Date: {date:yyyy-MM-dd}
🍽️ Meals: {mealsCount}

📊 Daily Totals:
  🔥 Calories: {dailyTotals.Calories:F0} kcal
  💪 Protein: {dailyTotals.Protein:F1}g
  🌾 Carbs: {dailyTotals.Carbohydrates:F1}g
  🥑 Fat: {dailyTotals.Fat:F1}g
  🧂 Sodium: {dailyTotals.Sodium:F0}mg
  🍬 Sugar: {dailyTotals.Sugar:F1}g
  ⚠️ Saturated Fat: {dailyTotals.SaturatedFat:F1}g
";
    }

    /// meals for a specific week 
    public async Task<IEnumerable<Meal>> GetMealsForWeekAsync(DateTime date)
    {
        // get mon of the week
        var monday = date.AddDays(-(int)date.DayOfWeek + 1);
        var sunday = monday.AddDays(6);

        var allMeals = await _mealRepository.GetAllAsync();
        return allMeals.Where(m => m.MealDate.Date >= monday.Date && m.MealDate.Date <= sunday.Date).ToList();
    }

    /// calculates weekly totals 
    public async Task<NutritionalMetric> GetWeeklyTotalsAsync(DateTime date)
    {
        var mealsForWeek = await GetMealsForWeekAsync(date);
        var monday = date.AddDays(-(int)date.DayOfWeek + 1);
        var sunday = monday.AddDays(6);

        if (!mealsForWeek.Any())
        {
            return new NutritionalMetric
            {
                MealName = $"Weekly Total - {monday:yyyy-MM-dd} to {sunday:yyyy-MM-dd}",
                AnalyzedDate = date
            };
        }

        var totals = new NutritionalMetric
        {
            MealName = $"Weekly Total - {monday:yyyy-MM-dd} to {sunday:yyyy-MM-dd}",
            AnalyzedDate = date,
            Calories = 0,
            Protein = 0,
            Carbohydrates = 0,
            Fat = 0,
            Sodium = 0,
            Sugar = 0,
            SaturatedFat = 0
        };

        // compile nutritional data from weekly
        foreach (var meal in mealsForWeek)
        {
            if (meal.Nutritionals != null)
            {
                totals.Calories += meal.Nutritionals.Calories;
                totals.Protein += meal.Nutritionals.Protein;
                totals.Carbohydrates += meal.Nutritionals.Carbohydrates;
                totals.Fat += meal.Nutritionals.Fat;
                totals.Sodium += meal.Nutritionals.Sodium;
                totals.Sugar += meal.Nutritionals.Sugar;
                totals.SaturatedFat += meal.Nutritionals.SaturatedFat;
            }
        }

        return totals;
    }

    /// weekly nutritional summary
    public async Task<string> GetWeeklySummaryAsync(DateTime date)
    {
        var weeklyTotals = await GetWeeklyTotalsAsync(date);
        var mealsCount = (await GetMealsForWeekAsync(date)).Count();
        var monday = date.AddDays(-(int)date.DayOfWeek + 1);
        var sunday = monday.AddDays(6);

        return $@"
📅 Week: {monday:yyyy-MM-dd} to {sunday:yyyy-MM-dd}
🍽️ Meals: {mealsCount}

📊 Weekly Totals:
  🔥 Calories: {weeklyTotals.Calories:F0} kcal
  💪 Protein: {weeklyTotals.Protein:F1}g
  🌾 Carbs: {weeklyTotals.Carbohydrates:F1}g
  🥑 Fat: {weeklyTotals.Fat:F1}g
  🧂 Sodium: {weeklyTotals.Sodium:F0}mg
  🍬 Sugar: {weeklyTotals.Sugar:F1}g
  ⚠️ Saturated Fat: {weeklyTotals.SaturatedFat:F1}g
";
    }

    ///  all meals for a specific month
    public async Task<IEnumerable<Meal>> GetMealsForMonthAsync(DateTime date)
    {
        var firstDay = new DateTime(date.Year, date.Month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        var allMeals = await _mealRepository.GetAllAsync();
        return allMeals.Where(m => m.MealDate.Date >= firstDay.Date && m.MealDate.Date <= lastDay.Date).ToList();
    }

    /// calculate monthly totals
    public async Task<NutritionalMetric> GetMonthlyTotalsAsync(DateTime date)
    {
        var mealsForMonth = await GetMealsForMonthAsync(date);
        var firstDay = new DateTime(date.Year, date.Month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        if (!mealsForMonth.Any())
        {
            return new NutritionalMetric
            {
                MealName = $"Monthly Total - {date:yyyy-MM}",
                AnalyzedDate = date
            };
        }

        var totals = new NutritionalMetric
        {
            MealName = $"Monthly Total - {date:yyyy-MM}",
            AnalyzedDate = date,
            Calories = 0,
            Protein = 0,
            Carbohydrates = 0,
            Fat = 0,
            Sodium = 0,
            Sugar = 0,
            SaturatedFat = 0
        };

        // compile nutritional data in a month
        foreach (var meal in mealsForMonth)
        {
            if (meal.Nutritionals != null)
            {
                totals.Calories += meal.Nutritionals.Calories;
                totals.Protein += meal.Nutritionals.Protein;
                totals.Carbohydrates += meal.Nutritionals.Carbohydrates;
                totals.Fat += meal.Nutritionals.Fat;
                totals.Sodium += meal.Nutritionals.Sodium;
                totals.Sugar += meal.Nutritionals.Sugar;
                totals.SaturatedFat += meal.Nutritionals.SaturatedFat;
            }
        }

        return totals;
    }

    public async Task<string> GetMonthlySummaryAsync(DateTime date)
    {
        var monthlyTotals = await GetMonthlyTotalsAsync(date);
        var mealsCount = (await GetMealsForMonthAsync(date)).Count();

        return $@"
📅 Month: {date:yyyy-MM}
🍽️ Meals: {mealsCount}

📊 Monthly Totals:
  🔥 Calories: {monthlyTotals.Calories:F0} kcal
  💪 Protein: {monthlyTotals.Protein:F1}g
  🌾 Carbs: {monthlyTotals.Carbohydrates:F1}g
  🥑 Fat: {monthlyTotals.Fat:F1}g
  🧂 Sodium: {monthlyTotals.Sodium:F0}mg
  🍬 Sugar: {monthlyTotals.Sugar:F1}g
  ⚠️ Saturated Fat: {monthlyTotals.SaturatedFat:F1}g
";
    }
}
