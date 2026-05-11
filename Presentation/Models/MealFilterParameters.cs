namespace Edamame.Presentation.Models
{
    // Encapsulation of meal filtering params
    public class MealFilterParameters
    {
        public enum FilterType { Daily, Weekly, Monthly }

        public FilterType Type { get; set; } = FilterType.Daily;
        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public (DateTime StartDate, DateTime EndDate) GetDateRange()
        {
            return Type switch
            {
                FilterType.Daily => (SelectedDate.Date, SelectedDate.Date.AddDays(1).AddSeconds(-1)),
                FilterType.Weekly => GetWeekRange(),
                FilterType.Monthly => GetMonthRange(),
                _ => (SelectedDate.Date, SelectedDate.Date.AddDays(1).AddSeconds(-1))
            };
        }

        private (DateTime, DateTime) GetWeekRange()
        {
            var monday = SelectedDate.AddDays(-(int)SelectedDate.DayOfWeek + 1);
            var sunday = monday.AddDays(6).AddDays(1).AddSeconds(-1);
            return (monday.Date, sunday);
        }

        private (DateTime, DateTime) GetMonthRange()
        {
            var firstDay = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1).AddDays(1).AddSeconds(-1);
            return (firstDay.Date, lastDay);
        }
    }
}
