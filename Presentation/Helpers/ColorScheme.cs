using Edamame.Presentation.Interfaces;

namespace Edamame.Presentation.Helpers
{
    /// <summary>
    /// ✅ Refactored - Default color theme implementation
    /// - Implements IColorTheme for dependency injection
    /// - Enables dynamic theming, dark mode, user customization
    /// - Can be extended or replaced with alternative themes
    /// </summary>
    public class DefaultColorTheme : IColorTheme
    {
        // Background colors
        public Color AppBackground => Color.FromArgb(247, 248, 250);
        public Color CardBackground => Color.White;
        public Color PanelBackground => Color.FromArgb(250, 250, 250);
        public Color LightBackground => Color.FromArgb(243, 244, 246);

        // Borders and separators
        public Color CardBorder => Color.FromArgb(228, 231, 236);
        public Color LightBorder => Color.FromArgb(208, 214, 222);

        // Branding
        public Color BrandDark => Color.FromArgb(28, 62, 49);
        public Color BrandPrimary => Color.FromArgb(52, 168, 83);
        public Color BrandPrimaryHover => Color.FromArgb(45, 145, 71);
        public Color BrandPrimaryPressed => Color.FromArgb(38, 122, 60);
        public Color BrandNavActive => Color.FromArgb(52, 104, 86);
        public Color BrandNavInactive => Color.FromArgb(31, 71, 55);

        // Text colors
        public Color TextPrimary => Color.FromArgb(30, 30, 30);
        public Color TextSecondary => Color.FromArgb(92, 99, 112);
        public Color TextMuted => Color.FromArgb(150, 150, 150);
        public Color TextLight => Color.FromArgb(200, 220, 210);

        // Metrics colors
        public Color Green => Color.FromArgb(52, 168, 83);
        public Color Blue => Color.FromArgb(51, 150, 243);
        public Color Orange => Color.FromArgb(255, 152, 0);
        public Color Purple => Color.FromArgb(156, 39, 176);
        public Color Red => Color.FromArgb(244, 67, 54);
        public Color LightGreen => Color.FromArgb(76, 175, 80);

        // Chat bubbles
        public Color UserBubbleBackground => Color.FromArgb(52, 168, 83);
        public Color AiBubbleBackground => Color.FromArgb(220, 240, 230);
        public Color UserBubbleText => Color.White;
        public Color AiBubbleText => Color.FromArgb(33, 33, 33);

        // Input fields
        public Color InputBackground => Color.FromArgb(250, 250, 250);
        public Color InputHoverBackground => Color.FromArgb(242, 247, 245);
        public Color InputFocusBackground => Color.FromArgb(240, 248, 246);

        // Buttons
        public Color ButtonSecondaryBackground => Color.FromArgb(200, 200, 200);
        public Color ButtonHoverBackground => Color.FromArgb(246, 248, 251);
        public Color ButtonPressedBackground => Color.FromArgb(237, 240, 244);
    }

    /// <summary>
    /// 🔁 Static accessor for backward compatibility
    /// Provides access to default theme without DI (for legacy code)
    /// </summary>
    public static class ColorScheme
    {
        private static readonly IColorTheme _defaultTheme = new DefaultColorTheme();

        public static Color AppBackground => _defaultTheme.AppBackground;
        public static Color CardBackground => _defaultTheme.CardBackground;
        public static Color PanelBackground => _defaultTheme.PanelBackground;
        public static Color LightBackground => _defaultTheme.LightBackground;
        public static Color CardBorder => _defaultTheme.CardBorder;
        public static Color LightBorder => _defaultTheme.LightBorder;
        public static Color BrandDark => _defaultTheme.BrandDark;
        public static Color BrandPrimary => _defaultTheme.BrandPrimary;
        public static Color BrandPrimaryHover => _defaultTheme.BrandPrimaryHover;
        public static Color BrandPrimaryPressed => _defaultTheme.BrandPrimaryPressed;
        public static Color BrandNavActive => _defaultTheme.BrandNavActive;
        public static Color BrandNavInactive => _defaultTheme.BrandNavInactive;
        public static Color TextPrimary => _defaultTheme.TextPrimary;
        public static Color TextSecondary => _defaultTheme.TextSecondary;
        public static Color TextMuted => _defaultTheme.TextMuted;
        public static Color TextLight => _defaultTheme.TextLight;
        public static Color Green => _defaultTheme.Green;
        public static Color Blue => _defaultTheme.Blue;
        public static Color Orange => _defaultTheme.Orange;
        public static Color Purple => _defaultTheme.Purple;
        public static Color Red => _defaultTheme.Red;
        public static Color LightGreen => _defaultTheme.LightGreen;
        public static Color UserBubbleBackground => _defaultTheme.UserBubbleBackground;
        public static Color AiBubbleBackground => _defaultTheme.AiBubbleBackground;
        public static Color UserBubbleText => _defaultTheme.UserBubbleText;
        public static Color AiBubbleText => _defaultTheme.AiBubbleText;
        public static Color InputBackground => _defaultTheme.InputBackground;
        public static Color InputHoverBackground => _defaultTheme.InputHoverBackground;
        public static Color InputFocusBackground => _defaultTheme.InputFocusBackground;
        public static Color ButtonSecondaryBackground => _defaultTheme.ButtonSecondaryBackground;
        public static Color ButtonHoverBackground => _defaultTheme.ButtonHoverBackground;
        public static Color ButtonPressedBackground => _defaultTheme.ButtonPressedBackground;
    }
}

