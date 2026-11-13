namespace Verbalize.Core
{
    /// <summary> How the single Speak/Stop button should look and behave. </summary>
    /// <param name="Text">The button's label, with an access key marked by an ampersand.</param>
    /// <param name="Enabled">True if the button can be pressed.</param>
    /// <param name="Hint">The hint shown when the pointer rests on the button.</param>
    /// <param name="Stops">True if pressing the button stops the speech; false if it starts speaking.</param>
    public sealed record SpeakButtonState(string Text, bool Enabled, string Hint, bool Stops);

    /// <summary> Decides whether the Speak/Stop button reads "Speak" or "Stop", and whether it can be pressed. </summary>
    public static class SpeakButton
    {
        /// <summary> Describes the button: "Stop" while speaking, which always works; otherwise "Speak", which works only when the text can be spoken. </summary>
        /// <param name="isSpeaking">True while speech is playing.</param>
        /// <param name="conditions">Whether there is text, a connection and a key.</param>
        /// <returns>How the button should look and behave.</returns>
        public static SpeakButtonState Describe(bool isSpeaking, AppConditions conditions)
        {
            //  While speaking, offer Stop, even if the text was emptied or the connection dropped meanwhile.
            if (isSpeaking) { return new SpeakButtonState("St&op", true, ControlHints.Stop, true); }

            //  Otherwise offer Speak, available only when the text can be spoken.
            return new SpeakButtonState("Sp&eak", ActionAvailability.CanSpeakText(conditions), ControlHints.Speak, false);
        }
    }
}
