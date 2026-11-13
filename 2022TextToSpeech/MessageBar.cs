using Verbalize.Core;

namespace _Verbalize
{
    /// <summary> A strip above the text box that shows the app's messages quietly, in place of pop-up windows with a sound. It hides itself when there is nothing to say. </summary>
    public sealed class MessageBar : UserControl
    {
        /// <summary> How long information and success messages stay before closing by themselves. </summary>
        private static readonly TimeSpan AutoCloseDelay = TimeSpan.FromSeconds(8);
        /// <summary> The smallest height of the bar, enough for one line and the close button. </summary>
        private const int MinimumBarHeight = 34;

        /// <summary> Shows a symbol for the kind of message, so it is not told apart by colour alone. </summary>
        private readonly Label symbolLabel;
        /// <summary> Shows the message's text. </summary>
        private readonly Label messageLabel;
        /// <summary> Lets the user dismiss the message. </summary>
        private readonly Button closeButton;
        /// <summary> Closes information and success messages after a while. </summary>
        private readonly System.Windows.Forms.Timer autoCloseTimer;
        /// <summary> The message on show, or null when the bar is hidden. </summary>
        private UserMessage? currentMessage;

        /// <summary> Builds the bar: a symbol, the message text and a close button, hidden until there is a message. </summary>
        public MessageBar()
        {
            //  Lay out the symbol on the left, the close button on the right, and the text between them.
            Padding = new Padding(8, 4, 4, 4);
            Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            symbolLabel = new Label { Dock = DockStyle.Left, Width = 30, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI Symbol", 13F, FontStyle.Regular, GraphicsUnit.Point), AccessibleRole = AccessibleRole.Graphic };
            closeButton = new Button { Dock = DockStyle.Right, Width = 34, Text = "✕", FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, AccessibleName = "Dismiss message", Font = new Font("Segoe UI Symbol", 10F, FontStyle.Regular, GraphicsUnit.Point) };
            closeButton.FlatAppearance.BorderSize = 0;
            messageLabel = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, AutoSize = false, UseMnemonic = false };
            Controls.Add(messageLabel);
            Controls.Add(symbolLabel);
            Controls.Add(closeButton);

            //  Close the message when the user clicks the close button.
            closeButton.Click += (_, _) => Clear();

            //  Prepare the timer that closes information and success messages.
            autoCloseTimer = new System.Windows.Forms.Timer { Interval = (int)AutoCloseDelay.TotalMilliseconds };
            autoCloseTimer.Tick += (_, _) => Clear();

            //  Present the bar to screen readers as an alert, and start hidden.
            AccessibleRole = AccessibleRole.Alert;
            AccessibleName = "Messages";
            Visible = false;
        }

        /// <summary> Shows a message, replacing any message already on show. </summary>
        /// <param name="message">The message to show.</param>
        public void ShowMessage(UserMessage message)
        {
            //  Remember the message and give the bar the look of its kind.
            currentMessage = message;
            (Color background, Color text, string symbol, string kind) = LookOf(message.Severity);
            BackColor = background;
            messageLabel.ForeColor = symbolLabel.ForeColor = closeButton.ForeColor = text;
            closeButton.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(background, 0.05F);
            symbolLabel.Text = symbol;
            messageLabel.Text = message.Text;
            AccessibleDescription = kind + ": " + message.Text;

            //  Show the bar at the height its text needs.
            Visible = true;
            FitHeightToText();

            //  Close it later if the message closes by itself, otherwise keep it until dismissed.
            autoCloseTimer.Stop();
            if (message.ClosesByItself) { autoCloseTimer.Start(); }

            //  Let screen readers announce the new message.
            AnnounceToScreenReaders(kind + ": " + message.Text);
        }

        /// <summary> Hides the bar and forgets its message. </summary>
        public void Clear()
        {
            //  Stop the timer and hide the bar.
            autoCloseTimer.Stop();
            currentMessage = null;
            Visible = false;
        }

        /// <summary> Hides the bar only if it is showing a message about the given situation, for example once the computer is back online. </summary>
        /// <param name="topic">The situation that has ended.</param>
        public void ClearTopic(string topic)
        {
            //  Leave other messages alone.
            if (currentMessage?.Topic == topic) { Clear(); }
        }

        /// <summary> Keeps the bar tall enough for its text whenever its width changes. </summary>
        /// <param name="e">The event details.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            //  Let the control resize as usual, then refit the height.
            base.OnSizeChanged(e);
            FitHeightToText();
        }

        /// <summary> Frees the timer and fonts along with the bar. </summary>
        /// <param name="disposing">True when called from Dispose.</param>
        protected override void Dispose(bool disposing)
        {
            //  Release the timer and the fonts made for the bar.
            if (disposing)
            {
                autoCloseTimer.Dispose();
                symbolLabel.Font.Dispose();
                closeButton.Font.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> Sets the bar's height to fit its text, wrapped to the space available. </summary>
        private void FitHeightToText()
        {
            //  Nothing to fit while hidden or before the text area has a width.
            if (!Visible || messageLabel.Width <= 0) { return; }

            //  Measure the wrapped text and size the bar to it, never below one line.
            Size textSize = TextRenderer.MeasureText(messageLabel.Text, messageLabel.Font, new Size(messageLabel.Width, int.MaxValue), TextFormatFlags.WordBreak);
            int height = Math.Max(MinimumBarHeight, textSize.Height + Padding.Vertical + 6);
            if (Height != height) { Height = height; }
        }

        /// <summary> Asks screen readers to read out the message. </summary>
        /// <param name="text">What to read out.</param>
        private void AnnounceToScreenReaders(string text)
        {
            //  Announce the message, ignoring systems that do not support announcements.
            try
            {
                AccessibilityObject.RaiseAutomationNotification(System.Windows.Forms.Automation.AutomationNotificationKind.Other,
                    System.Windows.Forms.Automation.AutomationNotificationProcessing.ImportantMostRecent, text);
            }
            catch (Exception) { }
        }

        /// <summary> Returns the colours, symbol and spoken name for a kind of message. Colours are soft backgrounds with dark text, for easy reading. </summary>
        /// <param name="severity">The kind of message.</param>
        /// <returns>The background colour, text colour, symbol and the kind's name.</returns>
        private static (Color Background, Color Text, string Symbol, string Kind) LookOf(MessageSeverity severity)
        {
            //  Pick the look that matches the kind of message.
            return severity switch
            {
                MessageSeverity.Success => (Color.FromArgb(230, 244, 234), Color.FromArgb(19, 61, 31), "✔", "Done"),
                MessageSeverity.Warning => (Color.FromArgb(255, 244, 206), Color.FromArgb(77, 56, 0), "⚠", "Warning"),
                MessageSeverity.Error => (Color.FromArgb(253, 231, 233), Color.FromArgb(110, 22, 28), "✖", "Error"),
                _ => (Color.FromArgb(232, 240, 254), Color.FromArgb(23, 43, 77), "ℹ", "Information"),
            };
        }
    }
}
