namespace Verbalize.Core
{
    /// <summary> What is under the pointer in the app's own title bar. </summary>
    public enum TitleBarTarget
    {
        /// <summary> The pointer is below the title bar, in the rest of the window. </summary>
        OutsideTitleBar,
        /// <summary> An empty part of the title bar, such as the logo, the file name or the space beside the menu. </summary>
        Empty,
        /// <summary> Something in the title bar that can be clicked, such as a menu or the Help, Minimise or Close button. </summary>
        Control,
        /// <summary> The Maximise / Restore button. </summary>
        MaximizeButton,
    }

    /// <summary> How Windows should treat the point under the pointer. </summary>
    public enum TitleBarArea
    {
        /// <summary> An ordinary part of the window that handles its own clicks. </summary>
        Client,
        /// <summary> The title bar: drag to move, double-click to maximise, right-click for the window menu. </summary>
        Caption,
        /// <summary> The Maximise button, so Windows 11 shows its snap layouts when the pointer rests on it. </summary>
        MaximizeButton,
        /// <summary> The top edge, which resizes the window. </summary>
        Top,
        /// <summary> The top-left corner, which resizes the window. </summary>
        TopLeft,
        /// <summary> The top-right corner, which resizes the window. </summary>
        TopRight,
    }

    /// <summary> Decides what the app's own title bar does at each point, so it behaves like a real Windows title bar. </summary>
    public static class TitleBarHitTest
    {
        /// <summary> Decides how Windows should treat a point in the window. </summary>
        /// <param name="x">The point's distance from the window's left edge.</param>
        /// <param name="y">The point's distance from the window's top edge.</param>
        /// <param name="width">The window's width.</param>
        /// <param name="resizeBorder">How thick the resizing edge is.</param>
        /// <param name="isMaximized">True if the window fills the screen, which has no edges to resize.</param>
        /// <param name="target">What is under the pointer.</param>
        /// <returns>How Windows should treat the point.</returns>
        public static TitleBarArea Decide(int x, int y, int width, int resizeBorder, bool isMaximized, TitleBarTarget target)
        {
            //  Let the top edge resize the window, with wider corners so they are easy to grab, unless it fills the screen.
            if (!isMaximized && y >= 0 && y < resizeBorder)
            {
                if (x < resizeBorder * 2) { return TitleBarArea.TopLeft; }
                if (x >= width - resizeBorder * 2) { return TitleBarArea.TopRight; }
                return TitleBarArea.Top;
            }

            //  Otherwise act on what is under the pointer.
            return target switch
            {
                TitleBarTarget.Empty => TitleBarArea.Caption,
                TitleBarTarget.MaximizeButton => TitleBarArea.MaximizeButton,
                _ => TitleBarArea.Client,
            };
        }
    }
}
