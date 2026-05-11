namespace Edamame.Presentation.Services
{
    /// handles application settings persistence

    public class SettingsService
    {
        private readonly string _settingsPath;
        private const double DefaultCalorieGoal = 2000;

        public SettingsService()
        {
            _settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");
        }

        public double LoadCalorieGoal()
        {
            try
            {
                if (!File.Exists(_settingsPath))
                    return DefaultCalorieGoal;

                var content = File.ReadAllText(_settingsPath);
                if (content.StartsWith("DailyCalorieGoal="))
                {
                    var goalStr = content.Replace("DailyCalorieGoal=", "").Trim();
                    if (double.TryParse(goalStr, out var goal) && goal > 0)
                        return goal;
                }
            }
            catch
            {
                // Log error if needed
            }

            return DefaultCalorieGoal;
        }

        public void SaveCalorieGoal(double goal)
        {
            try
            {
                if (goal <= 0)
                    throw new ArgumentException("Goal must be greater than 0");

                File.WriteAllText(_settingsPath, $"DailyCalorieGoal={goal:F0}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not save settings: {ex.Message}", ex);
            }
        }
    }
}
