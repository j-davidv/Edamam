namespace Edamame
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var appBackground = Color.FromArgb(247, 248, 250);
            var cardBorder = Color.FromArgb(228, 231, 236);
            var brandDark = Color.FromArgb(28, 62, 49);
            var brandPrimary = Color.FromArgb(52, 168, 83);
            var textPrimary = Color.FromArgb(30, 30, 30);
            var textSecondary = Color.FromArgb(92, 99, 112);

            // Main Layout with TableLayoutPanel
            var mainTableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = appBackground,
                Padding = new Padding(16)
            };
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210));  // Navigation + BMI
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 330));  // Input Form
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));   // Main Content (Dashboard)
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360));  // Chat
            MainTableLayout = mainTableLayout;

            // SIDE NAVIGATION PANEL 
            var sideNavPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(243, 244, 246), // Match main background
                Padding = new Padding(0),
                Margin = new Padding(0, 0, 6, 0),
                AutoScroll = true
            };

            // Navigation Card
            var sideNavCardPanel = new Panel
            {
                Dock = DockStyle.Top,
                BackColor = brandDark,
                Padding = new Padding(16, 18, 16, 18),
                Height = 250
            };

            // Add card styling with rounded corners and subtle shadow
            sideNavCardPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(20, 50, 40), 1))
                using (var brush = new SolidBrush(brandDark))
                {
                    var rect = new Rectangle(0, 0, sideNavCardPanel.Width - 1, sideNavCardPanel.Height - 1);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRoundedRectangle(brush, 0, 0, sideNavCardPanel.Width, sideNavCardPanel.Height, 8);
                    e.Graphics.DrawRoundedRectangle(pen, 0, 0, sideNavCardPanel.Width - 1, sideNavCardPanel.Height - 1, 8);
                }
            };

            // logo 
            var navHeaderPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 15),
                Padding = new Padding(0)
            };

            // Logo PictureBox
            var logoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "logo.png");
            var logoPictureBox = new PictureBox
            {
                Image = File.Exists(logoPath) ? Image.FromFile(logoPath) : null,
                Width = 42,
                Height = 42,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Margin = new Padding(0, 0, 8, 0)
            };

            // App name label
            var navBrandLabel = new Label
            {
                Text = "Edamame",
                Font = new Font("Segoe UI Variable Display", 13, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(0, 7, 0, 0)
            };

            navHeaderPanel.Controls.Add(logoPictureBox);
            navHeaderPanel.Controls.Add(navBrandLabel);

            var navFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = false,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0, 10, 0, 0)
            };

            BtnNavDashboard = CreateNavButton("Dashboard", 0);
            BtnNavMyMeals = CreateNavButton("My Meals", 1);
            BtnNavDailyLog = CreateNavButton("Daily Log", 2);
            BtnNavBmi = CreateNavButton("BMI Calculator", 3);

            navFlowPanel.Controls.Add(BtnNavDashboard);
            navFlowPanel.Controls.Add(BtnNavMyMeals);
            navFlowPanel.Controls.Add(BtnNavDailyLog);

            sideNavCardPanel.Controls.Add(navFlowPanel);
            sideNavCardPanel.Controls.Add(navHeaderPanel);
            sideNavPanel.Controls.Add(sideNavCardPanel);

            // BMI CALCULATOR CARD
            var bmiCardPanel = new Panel
            {
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(16),
                Height = 310,
                Margin = new Padding(0, 10, 0, 0)
            };

            // Add card styling
            bmiCardPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(cardBorder, 1))
                using (var brush = new SolidBrush(Color.White))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRoundedRectangle(brush, 0, 0, bmiCardPanel.Width, bmiCardPanel.Height, 8);
                    e.Graphics.DrawRoundedRectangle(pen, 0, 0, bmiCardPanel.Width - 1, bmiCardPanel.Height - 1, 8);
                }
            };

            var bmiTitleLabel = new Label
            {
                Text = "BMI Calculator",
                Font = new Font("Segoe UI Variable Display", 11, FontStyle.Bold),
                ForeColor = brandDark,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 15),
                Dock = DockStyle.Top
            };

            var heightLabel = new Label
            {
                Text = "Height (cm):",
                Font = new Font("Segoe UI Variable Text", 10),
                ForeColor = textSecondary,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8),
                Dock = DockStyle.Top
            };

            TxtBmiHeight = new TextBox
            {
                Font = new Font("Segoe UI Variable Text", 11),
                BackColor = Color.FromArgb(250, 250, 250),
                ForeColor = textPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Height = 32,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 12),
                PlaceholderText = "e.g., 175"
            };
            TxtBmiHeight.TextChanged += (s, e) => CalculateBMI();

            var weightLabel = new Label
            {
                Text = "Weight (kg):",
                Font = new Font("Segoe UI Variable Text", 10),
                ForeColor = textSecondary,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8),
                Dock = DockStyle.Top
            };

            TxtBmiWeight = new TextBox
            {
                Font = new Font("Segoe UI Variable Text", 11),
                BackColor = Color.FromArgb(250, 250, 250),
                ForeColor = textPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Height = 32,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 12),
                PlaceholderText = "e.g., 70"
            };
            TxtBmiWeight.TextChanged += (s, e) => CalculateBMI();

            var bmiResultLabel = new Label
            {
                Text = "BMI Result:",
                Font = new Font("Segoe UI Variable Text", 10),
                ForeColor = textSecondary,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8),
                Dock = DockStyle.Top
            };

            LblBmiResult = new Label
            {
                Text = "BMI: -",
                Font = new Font("Segoe UI Variable Text", 22, FontStyle.Bold),
                ForeColor = brandPrimary,
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0),
                Dock = DockStyle.Top
            };

            var bmiCategoryLabel = new Label
            {
                Text = "Category: -",
                Font = new Font("Segoe UI Variable Text", 9),
                ForeColor = textSecondary,
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 0),
                Dock = DockStyle.Top
            };
            LblBmiCategory = bmiCategoryLabel;
            BmiCardPanel = bmiCardPanel;

            bmiCardPanel.Controls.Add(LblBmiResult);
            bmiCardPanel.Controls.Add(bmiCategoryLabel);
            bmiCardPanel.Controls.Add(bmiResultLabel);
            bmiCardPanel.Controls.Add(TxtBmiWeight);
            bmiCardPanel.Controls.Add(weightLabel);
            bmiCardPanel.Controls.Add(TxtBmiHeight);
            bmiCardPanel.Controls.Add(heightLabel);
            bmiCardPanel.Controls.Add(bmiTitleLabel);

            // Combine navigation and BMI into first column panel
            var firstColumnPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = appBackground,
                Padding = new Padding(0),
                Margin = new Padding(0, 0, 6, 0),
                AutoScroll = true
            };
            firstColumnPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            firstColumnPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 260));
            firstColumnPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            sideNavPanel.Dock = DockStyle.Fill;
            bmiCardPanel.Dock = DockStyle.Fill;
            bmiCardPanel.Margin = new Padding(0, 10, 0, 0);
            firstColumnPanel.Controls.Add(sideNavPanel, 0, 0);
            firstColumnPanel.Controls.Add(bmiCardPanel, 0, 1);

            // LEFT PANEL - INPUT FORM
            var inputTableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 11,
                AutoSize = false,
                BackColor = Color.White,
                Padding = new Padding(18),
                Margin = new Padding(6, 0, 6, 0)
            };

            // Add card styling with rounded corners and subtle shadow
            var inputCardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = appBackground,
                Padding = new Padding(0)
            };

            inputCardPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(cardBorder, 1))
                using (var brush = new SolidBrush(Color.White))
                {
                    var rect = new Rectangle(0, 0, inputCardPanel.Width - 1, inputCardPanel.Height - 1);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRoundedRectangle(brush, 0, 0, inputCardPanel.Width, inputCardPanel.Height, 8);
                    e.Graphics.DrawRoundedRectangle(pen, 0, 0, inputCardPanel.Width - 1, inputCardPanel.Height - 1, 8);
                }
            };
            inputCardPanel.Controls.Add(inputTableLayout);
            InputCardPanel = inputCardPanel;
            inputTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Header
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Meal Name Label
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Meal Name Input
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Meal Type Label
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Meal Type Input
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Date Label
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Date Input
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Recipes Label
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Recipes Input
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Button
            inputTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Clear Button

            InputPanel = inputTableLayout;

            var headerLabel = new Label
            {
                Text = "Add Meal",
                Font = new Font("Segoe UI Variable Text", 14, FontStyle.Bold),
                ForeColor = brandDark,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 18),
                Dock = DockStyle.Top
            };

            // Meal Name
            var lblMealName = CreateLabel("Meal Name");
            TextBoxMealName = new TextBox
            {
                Font = new Font("Segoe UI Variable Text", 10),
                BackColor = Color.FromArgb(250, 250, 250),
                ForeColor = textPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Height = 28,
                Margin = new Padding(0, 0, 0, 10),
                Dock = DockStyle.Top,
                PlaceholderText = "Enter meal name..."
            };

            // Add rounded corner effect and hover styling
            TextBoxMealName.MouseEnter += (s, e) => TextBoxMealName.BackColor = Color.FromArgb(242, 247, 245);
            TextBoxMealName.MouseLeave += (s, e) => TextBoxMealName.BackColor = Color.FromArgb(250, 250, 250);
            TextBoxMealName.GotFocus += (s, e) => TextBoxMealName.BackColor = Color.FromArgb(240, 248, 246);
            TextBoxMealName.LostFocus += (s, e) => TextBoxMealName.BackColor = Color.FromArgb(250, 250, 250);

            // Meal Type
            var lblMealType = CreateLabel("Meal Type");
            ComboBoxMealType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI Variable Text", 10),
                BackColor = Color.FromArgb(250, 250, 250),
                ForeColor = textPrimary,
                Margin = new Padding(0, 0, 0, 10),
                Height = 28,
                Dock = DockStyle.Top
            };

            // Add hover styling
            ComboBoxMealType.MouseEnter += (s, e) => ComboBoxMealType.BackColor = Color.FromArgb(242, 247, 245);
            ComboBoxMealType.MouseLeave += (s, e) => ComboBoxMealType.BackColor = Color.FromArgb(250, 250, 250);
            ComboBoxMealType.GotFocus += (s, e) => ComboBoxMealType.BackColor = Color.FromArgb(240, 248, 246);
            ComboBoxMealType.LostFocus += (s, e) => ComboBoxMealType.BackColor = Color.FromArgb(250, 250, 250);

            ComboBoxMealType.Items.AddRange(new[] { "Breakfast", "Brunch", "Lunch", "Snack", "Dinner", "Supper" });
            ComboBoxMealType.SelectedIndex = 0;

            // Meal Date
            var lblMealDate = CreateLabel("Date");
            DateTimePickerMeal = new DateTimePicker
            {
                Font = new Font("Segoe UI Variable Text", 10),
                BackColor = Color.FromArgb(250, 250, 250),
                ForeColor = textPrimary,
                Format = DateTimePickerFormat.Short,
                Margin = new Padding(0, 0, 0, 10),
                Value = DateTime.Today,
                Height = 28,
                Dock = DockStyle.Top
            };

            DateTimePickerMeal.MouseEnter += (s, e) => DateTimePickerMeal.BackColor = Color.FromArgb(242, 247, 245);
            DateTimePickerMeal.MouseLeave += (s, e) => DateTimePickerMeal.BackColor = Color.FromArgb(250, 250, 250);
            DateTimePickerMeal.GotFocus += (s, e) => DateTimePickerMeal.BackColor = Color.FromArgb(240, 248, 246);
            DateTimePickerMeal.LostFocus += (s, e) => DateTimePickerMeal.BackColor = Color.FromArgb(250, 250, 250);

            // Recipes/Ingredients - with clear visual section and better proportions
            var recipesLabelPanel = new Panel
            {
                Height = 32,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(241, 247, 244),
                Padding = new Padding(10, 4, 10, 4),
                Margin = new Padding(0, 12, 0, 8),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblRecipes = new Label
            {
                Text = "Recipes / Ingredients",
                Font = new Font("Segoe UI Variable Text", 10, FontStyle.Bold),
                ForeColor = brandDark,
                AutoSize = true,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };
            recipesLabelPanel.Controls.Add(lblRecipes);

            TextBoxRecipes = new TextBox
            {
                Font = new Font("Segoe UI Variable Text", 9),
                BackColor = Color.FromArgb(250, 250, 250),
                ForeColor = textPrimary,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Margin = new Padding(0, 0, 0, 10),
                Height = 340,
                Dock = DockStyle.Top,
                PlaceholderText = "List recipes and ingredients (one per line)...",
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8, 6, 8, 6)
            };

            TextBoxRecipes.MouseEnter += (s, e) => TextBoxRecipes.BackColor = Color.FromArgb(242, 247, 245);
            TextBoxRecipes.MouseLeave += (s, e) => TextBoxRecipes.BackColor = Color.FromArgb(250, 250, 250);
            TextBoxRecipes.GotFocus += (s, e) => TextBoxRecipes.BackColor = Color.FromArgb(240, 248, 246);
            TextBoxRecipes.LostFocus += (s, e) => TextBoxRecipes.BackColor = Color.FromArgb(250, 250, 250);

            // Button
            BtnCreateMeal = CreateBlackButton("Create Meal");
            BtnCreateMeal.Dock = DockStyle.Top;
            BtnCreateMeal.Height = 38;
            BtnCreateMeal.Margin = new Padding(0, 12, 0, 0);

            BtnClearForm = new Button
            {
                Text = "Clear Form",
                BackColor = Color.White,
                ForeColor = Color.FromArgb(84, 91, 104),
                Font = new Font("Segoe UI Variable Display", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Height = 38,
                Margin = new Padding(0, 8, 0, 0),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top
            };
            BtnClearForm.FlatAppearance.BorderSize = 1;
            BtnClearForm.FlatAppearance.BorderColor = Color.FromArgb(208, 214, 222);
            BtnClearForm.FlatAppearance.MouseOverBackColor = Color.FromArgb(246, 248, 251);
            BtnClearForm.FlatAppearance.MouseDownBackColor = Color.FromArgb(237, 240, 244);

            inputTableLayout.Controls.Add(headerLabel, 0, 0);
            inputTableLayout.Controls.Add(lblMealName, 0, 1);
            inputTableLayout.Controls.Add(TextBoxMealName, 0, 2);
            inputTableLayout.Controls.Add(lblMealType, 0, 3);
            inputTableLayout.Controls.Add(ComboBoxMealType, 0, 4);
            inputTableLayout.Controls.Add(lblMealDate, 0, 5);
            inputTableLayout.Controls.Add(DateTimePickerMeal, 0, 6);
            inputTableLayout.Controls.Add(recipesLabelPanel, 0, 7);
            inputTableLayout.Controls.Add(TextBoxRecipes, 0, 8);
            inputTableLayout.Controls.Add(BtnCreateMeal, 0, 9);
            inputTableLayout.Controls.Add(BtnClearForm, 0, 10);

            // CENTER PANEL - MAIN CONTENT
            ContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = appBackground,
                Padding = new Padding(0),
                Margin = new Padding(6, 0, 6, 0)
            };

            var contentCardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = appBackground,
                Padding = new Padding(0)
            };

            contentCardPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(cardBorder, 1))
                using (var brush = new SolidBrush(Color.White))
                {
                    var rect = new Rectangle(0, 0, contentCardPanel.Width - 1, contentCardPanel.Height - 1);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRoundedRectangle(brush, 0, 0, contentCardPanel.Width, contentCardPanel.Height, 8);
                    e.Graphics.DrawRoundedRectangle(pen, 0, 0, contentCardPanel.Width - 1, contentCardPanel.Height - 1, 8);
                }
            };

            var contentTableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                BackColor = Color.White,
                Padding = new Padding(15),
                AutoSize = false
            };
            contentTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            contentTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Header (Dashboard + Button)
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Content (visualizations)

            var dashboardHeaderLabel = new Label
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI Variable Text", 14, FontStyle.Bold),
                ForeColor = brandDark,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 15)
            };

            BtnDailyCalorieIntake = new Button
            {
                Text = "Daily Calorie Intake",
                BackColor = brandPrimary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Variable Text", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Height = 32,
                Margin = new Padding(10, 0, 0, 15),
                AutoSize = true
            };
            BtnDailyCalorieIntake.FlatAppearance.BorderSize = 0;
            BtnDailyCalorieIntake.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 145, 71);
            BtnDailyCalorieIntake.FlatAppearance.MouseDownBackColor = Color.FromArgb(38, 122, 60);
            BtnDailyCalorieIntake.Click += (s, e) => ShowCalorieGoalDialog();

            var contentInnerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(0),
                Margin = new Padding(0, 0, 0, 0)
            };

            ContentInnerPanel = contentInnerPanel;

            contentTableLayout.Controls.Add(dashboardHeaderLabel, 0, 0);
            contentTableLayout.Controls.Add(BtnDailyCalorieIntake, 1, 0);
            contentTableLayout.Controls.Add(contentInnerPanel, 0, 1);
            contentTableLayout.SetColumnSpan(contentInnerPanel, 2);

            contentCardPanel.Controls.Add(contentTableLayout);
            ContentPanel.Controls.Add(contentCardPanel);

            // RIGHT PANEL - GEMINI CHAT
            var chatTableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.White,
                Padding = new Padding(16),
                AutoSize = false,
                Margin = new Padding(6, 0, 0, 0)
            };
            chatTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            chatTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Header
            chatTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Chat history
            chatTableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Input area

            var chatHeaderPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 1,
                RowCount = 2,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };
            chatHeaderPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var chatHeaderLabel = new Label
            {
                Text = "AI Nutrition Coach",
                Font = new Font("Segoe UI Variable Display", 14, FontStyle.Bold),
                ForeColor = brandDark,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 3)
            };

            var chatStatusLabel = new Label
            {
                Text = "Personalized nutrition guidance",
                Font = new Font("Segoe UI Variable Text", 9),
                ForeColor = textSecondary,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            };
            chatHeaderPanel.Controls.Add(chatHeaderLabel, 0, 0);
            chatHeaderPanel.Controls.Add(chatStatusLabel, 0, 1);

            ChatHistoryPanel = new FlowLayoutPanel
            {
                BackColor = Color.FromArgb(246, 248, 250),
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 0, 12)
            };

            TextBoxChatInput = new TextBox
            {
                Font = new Font("Segoe UI Variable Text", 9),
                BackColor = Color.White,
                ForeColor = textPrimary,
                BorderStyle = BorderStyle.None,
                Height = 70,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Margin = new Padding(0),
                Dock = DockStyle.Fill,
                PlaceholderText = "Ask about nutrition..."
            };

            // Add hover effect to input
            TextBoxChatInput.MouseEnter += (s, e) => TextBoxChatInput.BackColor = Color.FromArgb(248, 248, 250);
            TextBoxChatInput.MouseLeave += (s, e) => TextBoxChatInput.BackColor = Color.White;
            TextBoxChatInput.GotFocus += (s, e) => TextBoxChatInput.BackColor = Color.FromArgb(248, 248, 250);
            TextBoxChatInput.LostFocus += (s, e) => TextBoxChatInput.BackColor = Color.White;

            var chatInputPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 104,
                BackColor = appBackground,
                Padding = new Padding(10)
            };

            // Pill-shaped border effect
            chatInputPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                using (var brush = new SolidBrush(Color.White))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRoundedRectangle(brush, 0, 0, chatInputPanel.Width, chatInputPanel.Height, 22);
                    e.Graphics.DrawRoundedRectangle(pen, 0, 0, chatInputPanel.Width - 1, chatInputPanel.Height - 1, 22);
                }
            };

            // Use TableLayoutPanel for better layout control
            var inputLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                AutoSize = false
            };
            inputLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            inputLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            inputLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Create container for textbox with internal padding and rounded corners
            var textBoxContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(0),
                Margin = new Padding(0, 0, 8, 0)
            };
            textBoxContainer.Controls.Add(TextBoxChatInput);

            // Add rounded corners to textbox container
            textBoxContainer.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                using (var brush = new SolidBrush(Color.White))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRoundedRectangle(brush, 0, 0, textBoxContainer.Width, textBoxContainer.Height, 12);
                    e.Graphics.DrawRoundedRectangle(pen, 0, 0, textBoxContainer.Width - 1, textBoxContainer.Height - 1, 12);
                }
            };

            inputLayoutPanel.Controls.Add(textBoxContainer, 0, 0);

            BtnSendMessage = new Button
            {
                Text = "➤",
                Font = new Font("Segoe UI Variable Text", 16, FontStyle.Bold),
                BackColor = brandPrimary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 48,
                Height = 44,
                Cursor = Cursors.Hand,
                Margin = new Padding(0),
                AutoSize = false
            };
            BtnSendMessage.FlatAppearance.BorderSize = 0;
            BtnSendMessage.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 145, 71);
            BtnSendMessage.FlatAppearance.MouseDownBackColor = Color.FromArgb(38, 122, 60);

            // Style button with rounded corners
            BtnSendMessage.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddArc(0, 0, 18, 18, 180, 90);
                    path.AddArc(BtnSendMessage.Width - 19, 0, 18, 18, 270, 90);
                    path.AddArc(BtnSendMessage.Width - 19, BtnSendMessage.Height - 19, 18, 18, 0, 90);
                    path.AddArc(0, BtnSendMessage.Height - 19, 18, 18, 90, 90);
                    path.CloseFigure();
                    BtnSendMessage.Region = new Region(path);
                }
            };

            inputLayoutPanel.Controls.Add(BtnSendMessage, 1, 0);
            chatInputPanel.Controls.Add(inputLayoutPanel);

            chatTableLayout.Controls.Add(chatHeaderPanel, 0, 0);
            chatTableLayout.Controls.Add(ChatHistoryPanel, 0, 1);
            chatTableLayout.Controls.Add(chatInputPanel, 0, 2);

            var chatCardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = appBackground,
                Padding = new Padding(0)
            };

            chatCardPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(cardBorder, 1))
                using (var brush = new SolidBrush(Color.White))
                {
                    var rect = new Rectangle(0, 0, chatCardPanel.Width - 1, chatCardPanel.Height - 1);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillRoundedRectangle(brush, 0, 0, chatCardPanel.Width, chatCardPanel.Height, 8);
                    e.Graphics.DrawRoundedRectangle(pen, 0, 0, chatCardPanel.Width - 1, chatCardPanel.Height - 1, 8);
                }
            };
            chatCardPanel.Controls.Add(chatTableLayout);

            ChatPanel = chatCardPanel;

            // add panels to main table layout (inner 4-column layout)
            mainTableLayout.Controls.Add(firstColumnPanel, 0, 0);
            mainTableLayout.Controls.Add(inputCardPanel, 1, 0);
            mainTableLayout.Controls.Add(ContentPanel, 2, 0);
            mainTableLayout.Controls.Add(chatCardPanel, 3, 0);

            // status bar
            StatusStrip = new StatusStrip
            {
                BackColor = appBackground,
                ForeColor = textSecondary,
                Font = new Font("Segoe UI Variable Text", 9)
            };

            StatusLabel = new ToolStripStatusLabel
            {
                Text = "Ready",
                Spring = true,
                Alignment = ToolStripItemAlignment.Left
            };
            StatusStrip.Items.Add(StatusLabel);

            // form setup
            Controls.Add(mainTableLayout);
            Controls.Add(StatusStrip);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1600, 860);
            MinimumSize = new Size(1180, 760);
            Text = "Edamame - Meal Planner";
            Font = new Font("Segoe UI Variable Text", 9F);
            BackColor = appBackground;
            Load += MainForm_Load;
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI Variable Text", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(58, 65, 78),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 3)
            };
        }

        private Button CreateBlackButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                BackColor = Color.FromArgb(52, 168, 83), // Vibrant green
                ForeColor = Color.White,
                Font = new Font("Segoe UI Variable Display", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Height = 38,
                Margin = new Padding(0),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 145, 71); // Darker green
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(38, 122, 60); // Even darker green
            return btn;
        }

        private Button CreateNavButton(string text, int index)
        {
            var btn = new Button
            {
                Text = text,
                BackColor = Color.FromArgb(31, 71, 55),
                ForeColor = Color.FromArgb(222, 233, 227),
                Font = new Font("Segoe UI Variable Text", 10, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Width = 182,
                Height = 44,
                Margin = new Padding(0, 8, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(44, 92, 74); // Lighter forest green on hover
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(52, 100, 82);

            // Add a tag to track if this is the active button
            btn.Tag = false;

            return btn;
        }

        // designer Controls
        public Panel InputPanel;
        public Panel InputCardPanel;
        public Panel ContentPanel;
        public Panel ContentInnerPanel;
        public Panel ChatPanel;
        public TableLayoutPanel MainTableLayout;
        public TextBox TextBoxMealName;
        public ComboBox ComboBoxMealType;
        public DateTimePicker DateTimePickerMeal;
        public TextBox TextBoxRecipes;
        public Button BtnCreateMeal;
        public Button BtnClearForm;
        public Button BtnNavDashboard;
        public Button BtnNavMyMeals;
        public Button BtnNavDailyLog;
        public Button BtnNavBmi;
        public FlowLayoutPanel ChatHistoryPanel;
        public TextBox TextBoxChatInput;
        public Button BtnSendMessage;
        public TextBox TxtBmiHeight;
        public TextBox TxtBmiWeight;
        public Label LblBmiResult;
        public Label LblBmiCategory;
        public Panel BmiCardPanel;
        public Button BtnDailyCalorieIntake;
        public StatusStrip StatusStrip;
        public ToolStripStatusLabel StatusLabel;
    }
}


