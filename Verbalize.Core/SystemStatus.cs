namespace Verbalize.Core
{
    /// <summary> What the app is busy doing, shown in the status bar. </summary>
    public enum AppActivity
    {
        /// <summary> Nothing; the app is ready. </summary>
        None,
        /// <summary> Speaking the text aloud. </summary>
        Speaking,
        /// <summary> Creating and saving a sound file. </summary>
        SavingSound,
        /// <summary> Downloading the list of voices from Azure. </summary>
        DownloadingVoices,
    }

    /// <summary> Builds the text for the left side of the status bar: what the app is doing and the state it is in. </summary>
    public static class SystemStatus
    {
        /// <summary> The separator placed between the parts of the status. </summary>
        public const string Separator = "  ·  ";

        /// <summary> Describes the app's state, for example "Speaking…  ·  Online  ·  Speech key set". </summary>
        /// <param name="isOnline">True if the computer is connected to the internet.</param>
        /// <param name="hasKey">True if an Azure Speech key is set.</param>
        /// <param name="activity">What the app is busy doing.</param>
        /// <returns>The status text.</returns>
        public static string Describe(bool isOnline, bool hasKey, AppActivity activity)
        {
            //  Start with what the app is doing, since that changes most often.
            string doing = activity switch
            {
                AppActivity.Speaking => "Speaking…",
                AppActivity.SavingSound => "Saving the sound file…",
                AppActivity.DownloadingVoices => "Downloading the voice list…",
                _ => "Ready",
            };

            //  Follow with the connection and the key, which every Azure action needs.
            string connection = isOnline ? "Online" : "Offline";
            string key = hasKey ? "Speech key set" : "No speech key";
            return string.Join(Separator, doing, connection, key);
        }
    }
}
