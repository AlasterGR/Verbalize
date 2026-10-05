using System.Xml;

namespace Verbalize.Core
{
    /// <summary> The voice choices found in a loaded SSML document, with defaults in place of anything missing. </summary>
    /// <param name="Language">The document's language, or "en-US" if it has none.</param>
    /// <param name="VoiceName">The short name of the document's first voice, or "en-US-JennyNeural" if it has none.</param>
    /// <param name="Style">The speaking style, or "calm" if it has none.</param>
    /// <param name="Rate">The speaking rate exactly as written, or "default" if it has none.</param>
    /// <param name="Pitch">The pitch exactly as written, or "default" if it has none.</param>
    /// <param name="Volume">The volume from 0 to 100, or null if it is missing or unreadable.</param>
    public sealed record SsmlLoadedSettings(string Language, string VoiceName, string Style, string Rate, string Pitch, int? Volume);

    /// <summary> Reads the voice choices out of an SSML document, so they can be shown in the app. </summary>
    public static class SsmlSettingsReader
    {
        /// <summary> The language used when a document does not state one. </summary>
        public const string DefaultLanguage = "en-US";
        /// <summary> The voice used when a document does not name one. </summary>
        public const string DefaultVoiceName = "en-US-JennyNeural";
        /// <summary> The speaking style used when a document does not state one. </summary>
        public const string DefaultStyle = "calm";
        /// <summary> The value used for rate or pitch when a document does not state one. </summary>
        public const string DefaultProsodyValue = "default";

        /// <summary> Reads the language, voice, style, rate, pitch and volume from an SSML document. </summary>
        /// <param name="ssmlDocument">The SSML document to read.</param>
        /// <returns>The values found, with defaults in place of anything missing.</returns>
        public static SsmlLoadedSettings Read(XmlDocument ssmlDocument)
        {
            //  Prepare to look up elements that belong to the SSML vocabulary.
            XmlNamespaceManager nsMgr = new(ssmlDocument.NameTable);
            nsMgr.AddNamespace("speak", SsmlDocumentFactory.SsmlNamespace);

            //  Find the language of the whole document, falling back to the default if it is missing or blank.
            XmlNode? speakNode = ssmlDocument.SelectSingleNode("//speak:speak", nsMgr);
            string? value = speakNode?.Attributes?["xml:lang"]?.Value;
            string language = string.IsNullOrEmpty(value) ? DefaultLanguage : value;

            //  Find the first voice in the document, falling back to the default if it is missing or blank.
            XmlNode? voiceNode = ssmlDocument.SelectSingleNode("//speak:voice", nsMgr);
            value = voiceNode?.Attributes?["name"]?.Value;
            string voiceName = string.IsNullOrEmpty(value) ? DefaultVoiceName : value;

            //  Find the first set of rate, pitch and volume choices.
            XmlNode? prosodyNode = ssmlDocument.SelectSingleNode("//speak:prosody", nsMgr);

            //  Read the speaking style from the element just inside it, falling back to the default.
            string style = prosodyNode?.FirstChild?.Attributes?["style"]?.Value ?? DefaultStyle;

            //  Read the rate and pitch exactly as written, falling back to the default.
            string rate = prosodyNode?.Attributes?["rate"]?.Value ?? DefaultProsodyValue;
            string pitch = prosodyNode?.Attributes?["pitch"]?.Value ?? DefaultProsodyValue;

            //  Read the volume only if it is present and holds a readable number.
            int? volume = null;
            if (prosodyNode?.Attributes?["volume"] is { Value: string volumeText } && SsmlValueParser.TryParseVolume(volumeText, out int parsedVolume))
            {
                volume = parsedVolume;
            }

            return new SsmlLoadedSettings(language, voiceName, style, rate, pitch, volume);
        }
    }
}
