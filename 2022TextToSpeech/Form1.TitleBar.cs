namespace _Verbalize
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Verbalize.Core;

    /// <summary> The app's own title bar: hides the Windows one and makes the top row move, resize, snap and maximise the window like a real title bar. </summary>
    public partial class Form1
    {
        private const int WM_NCCALCSIZE = 0x0083, WM_NCHITTEST = 0x0084, WM_NCMOUSEMOVE = 0x00A0, WM_NCLBUTTONDOWN = 0x00A1, WM_NCLBUTTONUP = 0x00A2, WM_NCLBUTTONDBLCLK = 0x00A3, WM_NCMOUSELEAVE = 0x02A2;
        private const int HTTRANSPARENT = -1, HTCLIENT = 1, HTCAPTION = 2, HTMAXBUTTON = 9, HTTOP = 12, HTTOPLEFT = 13, HTTOPRIGHT = 14;
        private const uint TME_LEAVE = 0x2, TME_NONCLIENT = 0x10;

        /// <summary> The grey behind a title bar button while the pointer rests on it. </summary>
        private static readonly Color TitleButtonHoverColor = Color.FromArgb(229, 229, 229);
        /// <summary> The red behind the Close button while the pointer rests on it, as in Windows. </summary>
        private static readonly Color CloseButtonHoverColor = Color.FromArgb(196, 43, 28);

        /// <summary> How thick the window's resizing edge is, measured from the frame Windows gives the window. </summary>
        private int resizeBorderThickness = 8;
        /// <summary> True while the pointer rests on the Maximise button. </summary>
        private bool maximizeButtonHovered;
        /// <summary> True between pressing and releasing the mouse on the Maximise button. </summary>
        private bool maximizeButtonPressed;
        /// <summary> The listeners that let the top row's pieces pass the pointer to the window, kept so they stay alive. </summary>
        private readonly List<TitleBarPassThrough> titleBarPassThroughs = new();

        /// <summary> A rectangle as Windows describes it. </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; }

        /// <summary> What Windows sends when it asks where the window's content should go. </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct NCCALCSIZE_PARAMS { public RECT Rect0, Rect1, Rect2; public IntPtr Position; }

        /// <summary> Asks Windows to say when the pointer leaves a part of the window. </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct TRACKMOUSEEVENT { public uint Size, Flags; public IntPtr Window; public uint HoverTime; }

        /// <summary> Tells whether a window is maximised. </summary>
        /// <param name="window">The window.</param>
        /// <returns>True if it is maximised.</returns>
        [DllImport("user32.dll")]
        private static extern bool IsZoomed(IntPtr window);

        /// <summary> Asks Windows to report when the pointer leaves. </summary>
        /// <param name="eventTrack">What to watch.</param>
        /// <returns>True if Windows agreed.</returns>
        [DllImport("user32.dll")]
        private static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT eventTrack);

        /// <summary> Gives the title bar buttons the Windows symbols and names, and makes the top row act as the title bar. </summary>
        private void SetUpTitleBar()
        {
            //  Show the same symbols Windows uses, with names that screen readers can read.
            Font iconFont = new(WindowButtons.IconFont, 10F, FontStyle.Regular, GraphicsUnit.Point);
            StyleTitleBarButton(button_ShowHelp, WindowButtons.HelpGlyph, "Help", ControlHints.Help, iconFont, TitleButtonHoverColor);
            StyleTitleBarButton(button_MinimizeWindow, WindowButtons.MinimizeGlyph, "Minimize", ControlHints.Minimize, iconFont, TitleButtonHoverColor);
            StyleTitleBarButton(button_MaximizeWindow, string.Empty, string.Empty, string.Empty, iconFont, TitleButtonHoverColor);
            StyleTitleBarButton(button_QuitApplication, WindowButtons.CloseGlyph, "Close", ControlHints.Close, iconFont, CloseButtonHoverColor);

            //  Keep the Maximise button and the View menu in step with the window's size, however it changed.
            ShowMaximizeState();
            Resize += (_, _) => ShowMaximizeState();

            //  Let the top row's empty parts and the Maximise button pass the pointer to the window, which treats them as the title bar.
            foreach (Control piece in new Control[] { tableLayoutPanel10, tableLayoutPanel9, panel3, menuStrip1, label12, tableLayoutPanel6, button14, button3, Maximize_btn, button13 })
            {
                titleBarPassThroughs.Add(new TitleBarPassThrough(piece, PassesToTitleBar));
            }
        }

        /// <summary> Makes a title bar button flat, with a symbol, a name for screen readers, a hint and a hover colour. </summary>
        /// <param name="button">The button.</param>
        /// <param name="glyph">The symbol to show.</param>
        /// <param name="name">The name screen readers read.</param>
        /// <param name="hint">The hint shown when the pointer rests on it.</param>
        /// <param name="iconFont">The font that holds the symbol.</param>
        /// <param name="hoverColor">The colour behind it while the pointer rests on it.</param>
        private static void StyleTitleBarButton(Button button, string glyph, string name, string hint, Font iconFont, Color hoverColor)
        {
            //  Make it flat like the Windows title bar buttons.
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = hoverColor;
            button.UseVisualStyleBackColor = false;
            button.BackColor = Color.Transparent;

            //  Give it its symbol, name and hint.
            button.Font = iconFont;
            button.Text = glyph;
            button.AccessibleName = name;
            statusHints.SetToolTip(button, hint);
        }

        /// <summary> Shows Maximise or Restore on the button and in the View menu, to match the window's size. </summary>
        private void ShowMaximizeState()
        {
            //  Offer the opposite of the window's current size.
            MaximizeButtonState state = WindowButtons.DescribeMaximize(WindowState == FormWindowState.Maximized);
            button_MaximizeWindow.Text = state.Glyph;
            button_MaximizeWindow.AccessibleName = state.Name;
            statusHints.SetToolTip(button_MaximizeWindow, state.Hint);
            fullScreenToolStripMenuItem.Text = state.MenuText;
        }

        /// <summary> Handles the Windows messages that make the top row work as the title bar. </summary>
        /// <param name="m">The message from Windows.</param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //  Remove the Windows title bar but keep its edges, so the app's own top row takes its place.
                case WM_NCCALCSIZE when m.WParam != IntPtr.Zero:
                    RemoveWindowsTitleBar(ref m);
                    return;

                //  Tell Windows which parts of the top row move, resize or maximise the window.
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if (m.Result == (IntPtr)HTCLIENT) { m.Result = (IntPtr)ToHitCode(DecideTitleBarArea(PointToClient(PointFromLParam(m.LParam)))); }
                    return;

                //  Light up the Maximise button while the pointer rests on it.
                case WM_NCMOUSEMOVE:
                    SetMaximizeButtonHovered(m.WParam == (IntPtr)HTMAXBUTTON);
                    break;

                //  Dim it again when the pointer leaves.
                case WM_NCMOUSELEAVE:
                    SetMaximizeButtonHovered(false);
                    maximizeButtonPressed = false;
                    break;

                //  Note a press on the Maximise button, instead of letting Windows draw its own button.
                case WM_NCLBUTTONDOWN or WM_NCLBUTTONDBLCLK when m.WParam == (IntPtr)HTMAXBUTTON:
                    maximizeButtonPressed = true;
                    m.Result = IntPtr.Zero;
                    return;

                //  Maximise or restore the window when the press is released on the button.
                case WM_NCLBUTTONUP when m.WParam == (IntPtr)HTMAXBUTTON:
                    if (maximizeButtonPressed) { maximizeButtonPressed = false; MaximizeWindow(); }
                    m.Result = IntPtr.Zero;
                    return;
            }
            base.WndProc(ref m);
        }

        /// <summary> Lets Windows work out the window's edges as usual, then moves the top of the content up to replace the Windows title bar. </summary>
        /// <param name="m">The message that asks where the window's content should go.</param>
        private void RemoveWindowsTitleBar(ref Message m)
        {
            //  Note where the window's top is, then let Windows work out the edges.
            NCCALCSIZE_PARAMS before = Marshal.PtrToStructure<NCCALCSIZE_PARAMS>(m.LParam);
            base.WndProc(ref m);
            NCCALCSIZE_PARAMS after = Marshal.PtrToStructure<NCCALCSIZE_PARAMS>(m.LParam);

            //  Use the side edge's thickness for the top edge's resizing strip.
            int edge = after.Rect0.Left - before.Rect0.Left;
            if (edge > 0) { resizeBorderThickness = edge; }

            //  Start the content at the very top, or just inside the screen when maximised, since Windows then pushes the edges off screen.
            after.Rect0.Top = before.Rect0.Top + (IsZoomed(Handle) ? edge : 0);
            Marshal.StructureToPtr(after, m.LParam, false);
            m.Result = IntPtr.Zero;
        }

        /// <summary> Tells whether the window should treat a point as title bar rather than as an ordinary part of the window. </summary>
        /// <param name="screenPoint">The point on the screen.</param>
        /// <returns>True if the point moves, resizes or maximises the window.</returns>
        private bool PassesToTitleBar(Point screenPoint)
        {
            //  Treat it as title bar unless it is an ordinary part of the window.
            return DecideTitleBarArea(PointToClient(screenPoint)) != TitleBarArea.Client;
        }

        /// <summary> Decides how Windows should treat a point in the window. </summary>
        /// <param name="clientPoint">The point, measured from the window's content corner.</param>
        /// <returns>How Windows should treat it.</returns>
        private TitleBarArea DecideTitleBarArea(Point clientPoint)
        {
            //  Combine where the point is with what is under it.
            return TitleBarHitTest.Decide(clientPoint.X, clientPoint.Y, ClientSize.Width, resizeBorderThickness, IsHandleCreated && IsZoomed(Handle), FindTitleBarTarget(clientPoint));
        }

        /// <summary> Finds what in the top row is under a point. </summary>
        /// <param name="clientPoint">The point, measured from the window's content corner.</param>
        /// <returns>What is under the point.</returns>
        private TitleBarTarget FindTitleBarTarget(Point clientPoint)
        {
            //  Anything below the top row is the rest of the window.
            Rectangle titleRow = RectangleToClient(tableLayoutPanel9.RectangleToScreen(tableLayoutPanel9.ClientRectangle));
            if (!titleRow.Contains(clientPoint)) { return TitleBarTarget.OutsideTitleBar; }

            //  Find the innermost visible piece under the point.
            Point screenPoint = PointToScreen(clientPoint);
            Control deepest = this;
            Control? child;
            while ((child = deepest.GetChildAtPoint(deepest.PointToClient(screenPoint), GetChildAtPointSkip.Invisible)) != null) { deepest = child; }

            //  The Maximise button is special, other buttons and menu items keep their clicks, and the rest moves the window.
            if (deepest == button_MaximizeWindow) { return TitleBarTarget.MaximizeButton; }
            if (deepest is Button) { return TitleBarTarget.Control; }
            if (deepest is MenuStrip menu && menu.GetItemAt(menu.PointToClient(screenPoint)) != null) { return TitleBarTarget.Control; }
            return TitleBarTarget.Empty;
        }

        /// <summary> Lights up or dims the Maximise button, and asks Windows to say when the pointer leaves it. </summary>
        /// <param name="hovered">True if the pointer rests on the button.</param>
        private void SetMaximizeButtonHovered(bool hovered)
        {
            //  Do nothing if the button already looks right.
            if (hovered == maximizeButtonHovered) { return; }
            maximizeButtonHovered = hovered;
            button_MaximizeWindow.BackColor = hovered ? TitleButtonHoverColor : Color.Transparent;

            //  Ask to be told when the pointer leaves, so the button can dim again.
            if (hovered)
            {
                TRACKMOUSEEVENT track = new() { Size = (uint)Marshal.SizeOf<TRACKMOUSEEVENT>(), Flags = TME_LEAVE | TME_NONCLIENT, Window = Handle };
                TrackMouseEvent(ref track);
            }
        }

        /// <summary> Turns a title bar decision into the code Windows expects. </summary>
        /// <param name="area">How the point should be treated.</param>
        /// <returns>The Windows hit-test code.</returns>
        private static int ToHitCode(TitleBarArea area)
        {
            //  Match each decision to its Windows code.
            return area switch
            {
                TitleBarArea.Caption => HTCAPTION,
                TitleBarArea.MaximizeButton => HTMAXBUTTON,
                TitleBarArea.Top => HTTOP,
                TitleBarArea.TopLeft => HTTOPLEFT,
                TitleBarArea.TopRight => HTTOPRIGHT,
                _ => HTCLIENT,
            };
        }

        /// <summary> Reads the screen point packed into a Windows message. </summary>
        /// <param name="lParam">The packed point.</param>
        /// <returns>The point on the screen.</returns>
        private static Point PointFromLParam(IntPtr lParam)
        {
            //  Take the signed low and high halves, which can be negative on a second screen.
            long packed = lParam.ToInt64();
            return new Point(unchecked((short)packed), unchecked((short)(packed >> 16)));
        }

        /// <summary> Listens to one piece of the top row and passes the pointer through to the window wherever the window treats it as title bar. </summary>
        private sealed class TitleBarPassThrough : NativeWindow
        {
            /// <summary> Tells whether a screen point should go to the window. </summary>
            private readonly Func<Point, bool> passesToWindow;

            /// <summary> Starts listening to a piece of the top row, and again whenever Windows recreates it. </summary>
            /// <param name="piece">The piece of the top row.</param>
            /// <param name="passesToWindow">Tells whether a screen point should go to the window.</param>
            public TitleBarPassThrough(Control piece, Func<Point, bool> passesToWindow)
            {
                //  Listen now if the piece is already on screen, and follow it when it is recreated or removed.
                this.passesToWindow = passesToWindow;
                if (piece.IsHandleCreated) { AssignHandle(piece.Handle); }
                piece.HandleCreated += (_, _) => AssignHandle(piece.Handle);
                piece.HandleDestroyed += (_, _) => ReleaseHandle();
            }

            /// <summary> Passes the pointer to the window where it is title bar, and keeps it here otherwise. </summary>
            /// <param name="m">The message from Windows.</param>
            protected override void WndProc(ref Message m)
            {
                //  Hand over points that belong to the title bar.
                if (m.Msg == WM_NCHITTEST && passesToWindow(PointFromLParam(m.LParam)))
                {
                    m.Result = (IntPtr)HTTRANSPARENT;
                    return;
                }
                base.WndProc(ref m);
            }
        }
    }
}
