using Edamame.Application.Interfaces;
using Edamame.Domain.Entities;
using Edamame.Domain.Interfaces;
using Edamame.Presentation.Interfaces;
using Edamame.Presentation.Models;
using Edamame.Presentation.Services;

namespace Edamame.Presentation.Controllers
{
    public class FormController : IFormController
    {
        private readonly DashboardService _dashboardService;
        private readonly MealUIService _mealUIService;
        private readonly BmiUIService _bmiUIService;
        private readonly ChatUIService _chatUIService;
        private readonly SettingsService _settingsService;

        private List<Meal> _allMeals = new();
        private MealFilterParameters _filterParameters = new();
        private DashboardData? _currentDashboardData;

        public FormController(
            DashboardService dashboardService,
            MealUIService mealUIService,
            BmiUIService bmiUIService,
            ChatUIService chatUIService,
            SettingsService settingsService)
        {
            _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
            _mealUIService = mealUIService ?? throw new ArgumentNullException(nameof(mealUIService));
            _bmiUIService = bmiUIService ?? throw new ArgumentNullException(nameof(bmiUIService));
            _chatUIService = chatUIService ?? throw new ArgumentNullException(nameof(chatUIService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public async Task InitializeAsync()
        {
            await RefreshMealsAsync();
        }

        public double LoadDailyCalorieGoal() => _settingsService.LoadCalorieGoal();

        // meal management

        public async Task<Meal> CreateMealAsync(string name, MealType type, DateTime date, string recipesText)
        {
            var meal = await _mealUIService.CreateMealAsync(name, type, date, recipesText);
            await RefreshMealsAsync();
            return meal;
        }

        public async Task UpdateMealAsync(Meal meal, string name, MealType type, DateTime date, string recipesText)
        {
            await _mealUIService.UpdateMealAsync(meal, name, type, date, recipesText);
            await RefreshMealsAsync();
        }

        public async Task DeleteMealAsync(string mealId)
        {
            await _mealUIService.DeleteMealAsync(mealId);
            await RefreshMealsAsync();
        }

        public async Task RefreshMealsAsync()
        {
            _allMeals = await _mealUIService.GetAllMealsAsync();
        }

        /// returns a copy of the meals list
        public List<Meal> GetAllMeals() => new List<Meal>(_allMeals);

        // dashboard management

        public void SetFilterParameters(MealFilterParameters.FilterType filterType, DateTime selectedDate)
        {
            _filterParameters = new MealFilterParameters
            {
                Type = filterType,
                SelectedDate = selectedDate
            };
        }

        public DashboardData GetDashboardData()
        {
            var filteredMeals = _dashboardService.FilterMeals(_allMeals, _filterParameters);
            _currentDashboardData = _dashboardService.CalculateDashboardData(filteredMeals);
            return _currentDashboardData;
        }

        public List<Meal> GetFilteredMeals()
        {
            return _dashboardService.FilterMeals(_allMeals, _filterParameters);
        }

        public (double Current, double Goal, double Percent) GetCalorieProgress(double dailyGoal)
        {
            if (_currentDashboardData == null)
                GetDashboardData();

            return _dashboardService.GetCalorieProgress(_currentDashboardData?.TotalCalories ?? 0, dailyGoal);
        }

        public void SaveDailyCalorieGoal(double goal)
        {
            _settingsService.SaveCalorieGoal(goal);
        }

        // BMI management

        public BmiUIService.BmiResult? CalculateBmi(string heightText, string weightText)
        {
            return _bmiUIService.CalculateBmi(heightText, weightText);
        }

        // Chat management

        public bool IsChatAvailable => _chatUIService.IsAvailable;

        public string GetChatUnavailableMessage() => _chatUIService.GetUnavailableMessage();

        public async Task<string?> SendChatMessageAsync(string message)
        {
            return await _chatUIService.SendMessageWithRetryAsync(message);
        }

        public string GetChatErrorTitle(Exception ex) => _chatUIService.GetErrorTitle(ex);

        public string GetChatErrorMessage(Exception ex) => _chatUIService.GetErrorMessage(ex);
    }
}
