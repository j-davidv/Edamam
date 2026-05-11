namespace Edamame.Presentation.Interfaces;

public interface IColorTheme
{
    // bg colors
    Color AppBackground { get; }
    Color CardBackground { get; }
    Color PanelBackground { get; }
    Color LightBackground { get; }

    // borders and separators
    Color CardBorder { get; }
    Color LightBorder { get; }

    // branding
    Color BrandDark { get; }
    Color BrandPrimary { get; }
    Color BrandPrimaryHover { get; }
    Color BrandPrimaryPressed { get; }
    Color BrandNavActive { get; }
    Color BrandNavInactive { get; }

    // text colors
    Color TextPrimary { get; }
    Color TextSecondary { get; }
    Color TextMuted { get; }
    Color TextLight { get; }

    // metrics colors
    Color Green { get; }
    Color Blue { get; }
    Color Orange { get; }
    Color Purple { get; }
    Color Red { get; }
    Color LightGreen { get; }

    // chat bubbles
    Color UserBubbleBackground { get; }
    Color AiBubbleBackground { get; }
    Color UserBubbleText { get; }
    Color AiBubbleText { get; }

    // input fields
    Color InputBackground { get; }
    Color InputHoverBackground { get; }
    Color InputFocusBackground { get; }

    // buttons
    Color ButtonSecondaryBackground { get; }
    Color ButtonHoverBackground { get; }
    Color ButtonPressedBackground { get; }
}
