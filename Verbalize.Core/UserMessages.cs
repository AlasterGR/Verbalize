namespace Verbalize.Core
{
    /// <summary> The messages the app shows the user, written in one place so they stay plain and consistent. </summary>
    public static class UserMessages
    {
        /// <summary> The topic of the message shown while the computer is offline. </summary>
        public const string OfflineTopic = "offline";
        /// <summary> The topic of the message shown while the Azure key is missing or its settings file is damaged. </summary>
        public const string SpeechSetupTopic = "speech-setup";

        /// <summary> Says the computer is offline and which actions are paused until it is back online. </summary>
        /// <returns>The offline message.</returns>
        public static UserMessage Offline()
        {
            //  Explain what is paused and that it comes back by itself.
            return new UserMessage(MessageSeverity.Warning,
                "You are offline. Speaking, exporting sound and downloading voices are paused, and come back as soon as the connection returns.",
                OfflineTopic);
        }

        /// <summary> Explains how to set up the Azure key when none is set or the settings file cannot be read. </summary>
        /// <param name="credentials">The key and region found, and any problem with the settings file.</param>
        /// <param name="settingsFilePath">Where the settings file is kept.</param>
        /// <returns>The message, or null when the key is set and the settings file is fine.</returns>
        public static UserMessage? SpeechSetup(SpeechCredentials credentials, string settingsFilePath)
        {
            //  Say nothing when a key is set and the settings file is fine.
            if (credentials.HasKey && credentials.Problem == null) { return null; }

            //  Explain what is turned off and the two ways to set the key, when it is missing.
            string text = credentials.HasKey ? string.Empty
                : "No Azure Speech key is set, so speaking, exporting sound and downloading voices are turned off. "
                + "Set the " + SpeechCredentialsResolver.KeyVariable + " environment variable, or put "
                + "{ \"" + SpeechCredentialsResolver.KeySetting + "\": \"your key\", \"" + SpeechCredentialsResolver.RegionSetting + "\": \"" + credentials.Region + "\" } in "
                + settingsFilePath + ", then restart the app.";

            //  Add the problem found with the settings file, if any.
            if (credentials.Problem != null)
            {
                text += (text.Length > 0 ? Environment.NewLine : string.Empty) + credentials.Problem;
            }
            return new UserMessage(MessageSeverity.Warning, text, SpeechSetupTopic);
        }

        /// <summary> Says the text could not be spoken, and why. </summary>
        /// <param name="reason">Why it failed.</param>
        /// <returns>The error message.</returns>
        public static UserMessage SpeakFailed(string reason)
        {
            //  Lead with what failed, then why.
            return new UserMessage(MessageSeverity.Error, "The text could not be spoken. " + reason);
        }

        /// <summary> Says the sound file could not be created, and why. </summary>
        /// <param name="reason">Why it failed.</param>
        /// <returns>The error message.</returns>
        public static UserMessage SoundFileFailed(string reason)
        {
            //  Lead with what failed, then why.
            return new UserMessage(MessageSeverity.Error, "The sound file could not be created. " + reason);
        }

        /// <summary> Says where the sound file was saved. </summary>
        /// <param name="filePath">The saved sound file.</param>
        /// <returns>The success message.</returns>
        public static UserMessage SoundFileSaved(string filePath)
        {
            //  Name the file so the user knows where to find it.
            return new UserMessage(MessageSeverity.Success, "Saved the sound file " + filePath + ".");
        }

        /// <summary> Asks the user to choose a sound format before exporting. It is a warning because the action they asked for did not happen. </summary>
        /// <returns>The warning message.</returns>
        public static UserMessage ChooseSoundFormat()
        {
            //  Say what to do, since nothing would be saved without a format.
            return new UserMessage(MessageSeverity.Warning, "Choose a sound format (mp3 or wav) first, then export again.");
        }

        /// <summary> Says the voice list could not be downloaded, and why. </summary>
        /// <param name="reason">Why it failed.</param>
        /// <returns>The error message.</returns>
        public static UserMessage VoiceListDownloadFailed(string reason)
        {
            //  Reassure the user the app keeps working, then say why.
            return new UserMessage(MessageSeverity.Error, "The voice list could not be downloaded, so the app keeps using the voices it already has. " + reason);
        }

        /// <summary> Says the latest voice list was downloaded and is in use. </summary>
        /// <returns>The success message.</returns>
        public static UserMessage VoiceListDownloaded()
        {
            //  Confirm the download.
            return new UserMessage(MessageSeverity.Success, "Downloaded the latest voice list.");
        }
    }
}
