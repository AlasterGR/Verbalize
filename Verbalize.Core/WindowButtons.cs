namespace Verbalize.Core
{
    /// <summary> How the Maximise / Restore button and its menu item should read. </summary>
    /// <param name="Glyph">The symbol on the button, from the Windows icon font.</param>
    /// <param name="Name">The button's name for screen readers, for example "Maximize".</param>
    /// <param name="Hint">The hint shown when the pointer rests on the button.</param>
    /// <param name="MenuText">The matching View menu item's text.</param>
    public sealed record MaximizeButtonState(string Glyph, string Name, string Hint, string MenuText);

    /// <summary> The symbols and names of the title bar's buttons, matching the ones Windows draws. </summary>
    public static class WindowButtons
    {
        /// <summary> The Windows icon font that holds the title bar symbols on Windows 10 and 11. </summary>
        public const string IconFont = "Segoe MDL2 Assets";
        /// <summary> The Help symbol. </summary>
        public const string HelpGlyph = "";
        /// <summary> The Minimise symbol. </summary>
        public const string MinimizeGlyph = "";
        /// <summary> The Close symbol. </summary>
        public const string CloseGlyph = "";

        /// <summary> Describes the Maximise / Restore button: it offers to restore the window when it fills the screen, and to maximise it otherwise. </summary>
        /// <param name="isMaximized">True if the window fills the screen.</param>
        /// <returns>The button's symbol, name, hint and menu text.</returns>
        public static MaximizeButtonState DescribeMaximize(bool isMaximized)
        {
            //  Offer the opposite of the window's current size.
            return isMaximized
                ? new MaximizeButtonState("", "Restore", ControlHints.Restore, "Restore")
                : new MaximizeButtonState("", "Maximize", ControlHints.Maximize, "Maximize");
        }
    }
}
