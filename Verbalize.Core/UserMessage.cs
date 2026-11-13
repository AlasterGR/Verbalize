namespace Verbalize.Core
{
    /// <summary> How important a message to the user is, which decides how it looks and how long it stays. </summary>
    public enum MessageSeverity
    {
        /// <summary> Neutral information. </summary>
        Information,
        /// <summary> Something finished as the user asked. </summary>
        Success,
        /// <summary> Something limits what the app can do, but nothing has failed. </summary>
        Warning,
        /// <summary> Something the user asked for did not work. </summary>
        Error,
    }

    /// <summary> A message to show the user in the app's message bar instead of a pop-up window. </summary>
    /// <param name="Severity">How important the message is.</param>
    /// <param name="Text">What to tell the user.</param>
    /// <param name="Topic">Names a lasting situation, such as being offline, so its message can be removed when the situation ends; null for a one-off message.</param>
    public sealed record UserMessage(MessageSeverity Severity, string Text, string? Topic = null)
    {
        /// <summary> True if the message should disappear by itself after a while: one-off information or success. Warnings, errors and lasting situations stay until dealt with. </summary>
        public bool ClosesByItself => Topic == null && Severity is MessageSeverity.Information or MessageSeverity.Success;

        /// <summary> True if the message needs the user's attention, so it goes in the bar above the text box. Other messages report an outcome and go in the status bar at the bottom. </summary>
        public bool NeedsAttention => Severity is MessageSeverity.Warning or MessageSeverity.Error;
    }
}
