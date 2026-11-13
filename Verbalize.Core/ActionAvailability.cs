namespace Verbalize.Core
{
    /// <summary> What the app knows about its situation, which decides which actions can be used. </summary>
    /// <param name="HasText">True if the text box holds text worth speaking.</param>
    /// <param name="IsOnline">True if the computer is connected to the internet.</param>
    /// <param name="HasKey">True if an Azure Speech key is set.</param>
    public sealed record AppConditions(bool HasText, bool IsOnline, bool HasKey);

    /// <summary> Decides which actions can be used, so buttons are only offered when they can work. </summary>
    public static class ActionAvailability
    {
        /// <summary> Returns true if a text holds something worth speaking, that is, more than spaces and line breaks. </summary>
        /// <param name="text">The text to check.</param>
        /// <returns>True if there is something to speak.</returns>
        public static bool HasSpeakableText(string? text)
        {
            //  Treat empty and blank text alike.
            return !string.IsNullOrWhiteSpace(text);
        }

        /// <summary> Returns true if Azure can be used at all: the computer is online and a key is set. Used for downloading voices and exporting files. </summary>
        /// <param name="conditions">The app's situation.</param>
        /// <returns>True if Azure can be reached.</returns>
        public static bool CanReachAzure(AppConditions conditions)
        {
            //  Azure needs both a connection and a key.
            return conditions.IsOnline && conditions.HasKey;
        }

        /// <summary> Returns true if the text box can be spoken or saved as sound: there is text, and Azure can be reached. </summary>
        /// <param name="conditions">The app's situation.</param>
        /// <returns>True if the text can be turned into speech.</returns>
        public static bool CanSpeakText(AppConditions conditions)
        {
            //  Speaking needs text as well as Azure.
            return conditions.HasText && CanReachAzure(conditions);
        }
    }
}
