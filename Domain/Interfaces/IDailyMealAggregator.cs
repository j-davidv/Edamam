using Edamame.Domain.Entities;

namespace Edamame.Domain.Interfaces;

/// abstraction for daily meal aggregation

public interface IDailyMealAggregator
{
    Task<IEnumerable<Meal>> GetMealsForDateAsync(DateTime date);

    Task<NutritionalMetric> GetDailyTotalsAsync(DateTime date);

    /// gets daily nutritional summary as a string for UI binding
    Task<string> GetDailySummaryAsync(DateTime date);

    /// meals for specific weeks (mon - fri)
    /// calculation for weekly macros filter
    Task<IEnumerable<Meal>> GetMealsForWeekAsync(DateTime date);

    Task<NutritionalMetric> GetWeeklyTotalsAsync(DateTime date);

    Task<string> GetWeeklySummaryAsync(DateTime date);

    /// get all meals for a specific month
    Task<IEnumerable<Meal>> GetMealsForMonthAsync(DateTime date);

    /// calculation for monthly macros filter
    Task<NutritionalMetric> GetMonthlyTotalsAsync(DateTime date);

    Task<string> GetMonthlySummaryAsync(DateTime date);
}
