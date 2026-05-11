using Edamame.Presentation.Helpers;

namespace Edamame.Presentation.Services
{
    /// <summary>
    /// Centralizes UI component creation and styling
    /// Reduces code duplication in Form1
    /// </summary>
    public class UIRenderingService
    {
        public enum StatCardSize { Small, Medium, Large }

        public Panel CreateStatCard(string label, string value, Color accentColor,
            string unit = "", bool hasProgress = false, double currentCalories = 0,
            double dailyGoal = 2000, StatCardSize size = StatCardSize.Medium)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorScheme.CardBackground,
                Margin = new Padding(15)
            };

            // Rounded corners
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(ColorScheme.CardBorder, 1))
                using (var brush = new SolidBrush(ColorScheme.CardBackground))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRoundedRectangle(brush, 0, 0, card.Width, card.Height, 12);
                    e.Graphics.DrawRoundedRectangle(pen, 0, 0, card.Width - 1, card.Height - 1, 12);
                }
            };

            var accentBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 4,
                BackColor = accentColor
            };

            var (labelFontSize, valueFontSize) = GetFontSizes(size);

            var labelControl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI Variable Text", labelFontSize),
                ForeColor = ColorScheme.TextMuted,
                AutoSize = true,
                Margin = new Padding(15, 15, 15, 0),
                Dock = DockStyle.Top
            };

            var valueControl = new Label
            {
                Text = value,
                Font = new Font("Segoe UI Variable Text", valueFontSize, FontStyle.Bold),
                ForeColor = accentColor,
                AutoSize = true,
                Margin = new Padding(15, 10, 15, 0),
                Dock = DockStyle.Top
            };

            var unitControl = new Label
            {
                Text = unit,
                Font = new Font("Segoe UI Variable Text", 9),
                ForeColor = ColorScheme.TextMuted,
                AutoSize = true,
                Margin = new Padding(15, 5, 15, 0),
                Dock = DockStyle.Top
            };

            card.Controls.Add(accentBar);

            if (hasProgress)
            {
                var progressPercent = Math.Min(currentCalories / dailyGoal * 100, 100);

                var progressLabel = new Label
                {
                    Text = $"Daily Goal: {currentCalories:F0} / {dailyGoal:F0}",
                    Font = new Font("Segoe UI Variable Text", 8),
                    ForeColor = ColorScheme.TextMuted,
                    AutoSize = true,
                    Margin = new Padding(15, 8, 15, 2),
                    Dock = DockStyle.Top
                };

                var progressBar = new ProgressBar
                {
                    Value = (int)progressPercent,
                    Maximum = 100,
                    Dock = DockStyle.Top,
                    Height = 6,
                    Margin = new Padding(15, 0, 15, 8)
                };
                progressBar.ForeColor = accentColor;

                card.Controls.Add(progressLabel);
                card.Controls.Add(progressBar);
            }

            card.Controls.Add(unitControl);
            card.Controls.Add(valueControl);
            card.Controls.Add(labelControl);

            return card;
        }

        private (int LabelSize, int ValueSize) GetFontSizes(StatCardSize size) => size switch
        {
            StatCardSize.Large => (13, 48),
            StatCardSize.Medium => (11, 36),
            StatCardSize.Small => (10, 28),
            _ => (11, 36)
        };

        public Button CreateNavButton(string text, int index)
        {
            var btn = new Button
            {
                Text = text,
                BackColor = ColorScheme.BrandNavInactive,
                ForeColor = ColorScheme.TextLight,
                Font = new Font("Segoe UI Variable Display", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Height = 40,
                Dock = DockStyle.Top,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 0, 8),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        public Button CreateBlackButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Variable Display", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Height = 38,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }
    }
}


