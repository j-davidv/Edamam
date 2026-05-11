namespace Edamame.Presentation.Models
{
    // DTO containing aggregated dashboard stats
    public class DashboardData
    {
        public int TotalMeals { get; set; }
        public double TotalCalories { get; set; }
        public double TotalProtein { get; set; }
        public double TotalCarbohydrates { get; set; }
        public double TotalFat { get; set; }
        public double TotalSodium { get; set; }
        public double TotalSugar { get; set; }
        public double TotalSaturatedFat { get; set; }

        // percentages for macros
        public double ProteinPercent { get; set; }
        public double CarbsPercent { get; set; }
        public double FatPercent { get; set; }
    }
}
