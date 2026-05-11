using Edamame.Presentation.Helpers;

namespace Edamame.Presentation.Services
{
    ///  logic for BMI calculations and categorization
    public class BmiUIService
    {
        public class BmiResult
        {
            public double Bmi { get; set; }
            public string Category { get; set; } = "";
            public Color CategoryColor { get; set; }
        }

        public BmiResult? CalculateBmi(string heightCmText, string weightKgText)
        {
            if (!double.TryParse(heightCmText, out var height) ||
                !double.TryParse(weightKgText, out var weight) ||
                height <= 0 || weight <= 0)
            {
                return null;
            }

            var heightInMeters = height / 100.0;
            var bmi = weight / (heightInMeters * heightInMeters);

            var (category, color) = GetBmiCategory(bmi);

            return new BmiResult
            {
                Bmi = bmi,
                Category = category,
                CategoryColor = color
            };
        }

        private (string Category, Color Color) GetBmiCategory(double bmi)
        {
            return bmi switch
            {
                < 18.5 => ("Underweight", ColorScheme.Blue),
                < 25 => ("Normal Weight", ColorScheme.Green),
                < 30 => ("Overweight", ColorScheme.Orange),
                _ => ("Obese", ColorScheme.Red)
            };
        }
    }
}
