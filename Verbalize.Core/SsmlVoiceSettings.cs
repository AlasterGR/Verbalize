namespace Verbalize.Core
{
    /// <summary> The voice choices that shape how a piece of text is spoken. </summary>
    /// <param name="Language">The language code of the speech, for example "en-US".</param>
    /// <param name="VoiceName">The short name of the voice, for example "en-US-JennyNeural".</param>
    /// <param name="Rate">How fast the voice speaks, for example "default" or "-20%".</param>
    /// <param name="Pitch">How high the voice sounds, for example "default" or "-10Hz".</param>
    /// <param name="Volume">How loud the voice is, from 0 to 100.</param>
    /// <param name="Style">The speaking style, for example "calm" or "cheerful".</param>
    public sealed record SsmlVoiceSettings(string? Language, string? VoiceName, string Rate, string Pitch, int Volume, string Style);
}
