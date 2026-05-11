using Edamame.Domain.Entities;
using Edamame.Presentation.Models;

namespace Edamame.Presentation.Services
{
    /// logic for dashboard calculations and filtering
    /// Separates data aggregation from UI rendering
    public class DashboardService
    {
        /// Filters meals based on criteria
        public List<Meal> FilterMeals(IEnumerable<Meal> allMeals, MealFilterParameters parameters)
        {
            var (startDate, endDate) = parameters.GetDateRange();
            return allMeals
                .Where(m => m.MealDate.Date >= startDate.Date && m.MealDate.Date <= endDate.Date)
                .ToList();
        }

        /// calculates aggregated dashboard statistics from filtered meals
        public DashboardData CalculateDashboardData(List<Meal> filteredMeals)
        {
            var totalMeals = filteredMeals.Count;
            var totalCalories = (double)(filteredMeals.Sum(m => m.Nutritionals?.Calories ?? 0));
            var totalProtein = (double)(filteredMeals.Sum(m => m.Nutritionals?.Protein ?? 0));
            var totalCarbs = (double)(filteredMeals.Sum(m => m.Nutritionals?.Carbohydrates ?? 0));
            var totalFat = (double)(filteredMeals.Sum(m => m.Nutritionals?.Fat ?? 0));
            var totalSodium = (double)(filteredMeals.Sum(m => m.Nutritionals?.Sodium ?? 0));
            var totalSugar = (double)(filteredMeals.Sum(m => m.Nutritionals?.Sugar ?? 0));
            var totalSaturatedFat = (double)(filteredMeals.Sum(m => m.Nutritionals?.SaturatedFat ?? 0));

            var totalMacros = totalCarbs + totalFat + totalProtein;
            var carbsPercent = totalMacros > 0 ? (totalCarbs / totalMacros) * 100 : 0;
            var fatPercent = totalMacros > 0 ? (totalFat / totalMacros) * 100 : 0;
            var proteinPercent = totalMacros > 0 ? (totalProtein / totalMacros) * 100 : 0;

            return new DashboardData
            {
                TotalMeals = totalMeals,
                TotalCalories = totalCalories,
                TotalProtein = totalProtein,
                TotalCarbohydrates = totalCarbs,
                TotalFat = totalFat,
                TotalSodium = totalSodium,
                TotalSugar = totalSugar,
                TotalSaturatedFat = totalSaturatedFat,
                ProteinPercent = proteinPercent,
                CarbsPercent = carbsPercent,
                FatPercent = fatPercent
            };
        }

        /// Calculates calorie goal progress
        public (double CurrentCalories, double GoalCalories, double ProgressPercent) GetCalorieProgress(
            double currentCalories, double dailyGoal)
        {
            var progressPercent = Math.Min(currentCalories / dailyGoal * 100, 100);
            return (currentCalories, dailyGoal, progressPercent);
        }
    }
}
