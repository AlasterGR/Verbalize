namespace Verbalize.Core
{
    /// <summary> The short hints shown next to the pointer when it rests on a button, kept in one place so the wording stays plain and consistent. </summary>
    public static class ControlHints
    {
        /// <summary> Hint for the Load text button. </summary>
        public const string LoadText = "Open a text or SSML file and put its text in the box.";
        /// <summary> Hint for the Clear button. </summary>
        public const string Clear = "Empty the text box and forget the loaded file.";
        /// <summary> Hint for the Download Voices list button. </summary>
        public const string DownloadVoices = "Get the latest list of voices from Azure. Needs the internet and a Speech key.";
        /// <summary> Hint for the button that refills the language, voice and style lists. </summary>
        public const string RefreshVoiceLists = "Refill the language, voice and style lists, downloading the voice list first if there is none.";
        /// <summary> Hint for the Speak button. </summary>
        public const string Speak = "Read the selected text aloud, or all of it if nothing is selected (needs text, the internet and a Speech key).";
        /// <summary> Hint for the button that stops the speech. </summary>
        public const string Stop = "Stop the speech that is playing.";
        /// <summary> Hint for the button that saves the text box as a sound file. </summary>
        public const string SaveSound = "Save the text as a sound file in the chosen format (needs text, the internet and a Speech key).";
        /// <summary> Hint for the button that saves the text box as a text file. </summary>
        public const string SaveText = "Save the text as a file in the chosen format.";
        /// <summary> Hint for the button that turns a file on disk into a sound file. </summary>
        public const string SaveSoundFromFile = "Choose a text or SSML file and save its speech as a sound file next to it.";
        /// <summary> Hint for the arrow that shows or hides the voice settings panel. </summary>
        public const string ToggleSettingsPanel = "Show or hide the voice settings.";
        /// <summary> Hint for controls that are not built yet. </summary>
        public const string ComingLater = "Coming later.";
    }
}
