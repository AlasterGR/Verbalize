using System.Xml;

namespace Verbalize.Core
{
    /// <summary> Builds the SSML documents that tell Azure what to say and how to say it. </summary>
    public static class SsmlDocumentFactory
    {
        /// <summary> The namespace of the core SSML elements. </summary>
        public const string SsmlNamespace = "http://www.w3.org/2001/10/synthesis";
        /// <summary> The namespace of Microsoft's SSML extensions, such as speaking styles. </summary>
        public const string MicrosoftTtsNamespace = "http://www.w3.org/2001/mstts";
        /// <summary> The namespace of the EmotionML extensions. </summary>
        public const string EmotionMLNamespace = "http://www.w3.org/2009/10/emotionml";

        /// <summary> Creates an SSML document that speaks the given text with the given voice settings. </summary>
        /// <param name="text">The text to be spoken.</param>
        /// <param name="settings">The voice, language and prosody to speak the text with.</param>
        /// <returns>The SSML document, with a speak, voice, prosody and express-as element wrapped around the text.</returns>
        /// <remarks> Reference : https://www.w3.org/TR/speech-synthesis/#S3.1.1 and https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-structure </remarks>
        public static XmlDocument Create(string text, SsmlVoiceSettings settings)
        {
            //  Start an empty document with the "speak" element at its top.
            XmlDocument SSMLDocument = new();
            XmlElement speak = SSMLDocument.CreateElement("speak");
            SSMLDocument.AppendChild(speak);

            //  Add the XML declaration line, which SSML requires (https://www.w3.org/TR/speech-synthesis/#S2.1).
            XmlDeclaration xmlDeclaration = SSMLDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            SSMLDocument.InsertBefore(xmlDeclaration, speak);

            //  Mark the document as SSML version 1.0, since Azure does not accept 1.1.
            XmlAttribute version = SSMLDocument.CreateAttribute("version");
            version.Value = "1.0";
            speak.SetAttributeNode(version);

            //  Declare the vocabularies the document uses: standard SSML, Microsoft's extensions and EmotionML.
            XmlAttribute xmlns = SSMLDocument.CreateAttribute("xmlns");
            xmlns.Value = SsmlNamespace;
            speak.SetAttributeNode(xmlns);
            XmlAttribute mstts = SSMLDocument.CreateAttribute("xmlns:mstts");
            mstts.Value = MicrosoftTtsNamespace;
            speak.SetAttributeNode(mstts);
            XmlAttribute emo = SSMLDocument.CreateAttribute("xmlns:emo");
            emo.Value = EmotionMLNamespace;
            speak.SetAttributeNode(emo);

            //  State the language of the speech.
            XmlAttribute lang = SSMLDocument.CreateAttribute("xml:lang");
            lang.Value = settings.Language;
            speak.SetAttributeNode(lang);

            //  Tell Azure to skip any text that is not in a language the voice can speak.
            XmlAttribute onlangfailure = SSMLDocument.CreateAttribute("onlangfailure");
            onlangfailure.Value = "ignoretext ";
            speak.SetAttributeNode(onlangfailure);

            //  Choose which voice will do the speaking.
            XmlElement voice = SSMLDocument.CreateElement("voice");
            voice.SetAttribute("name", settings.VoiceName);

            //  Set how fast, how high and how loud the voice speaks.
            XmlElement prosody = SSMLDocument.CreateElement("prosody");
            prosody.SetAttribute("rate", settings.Rate);
            prosody.SetAttribute("pitch", settings.Pitch);
            prosody.SetAttribute("volume", settings.Volume.ToString());

            //  Set the speaking style, such as calm or cheerful.
            XmlElement express = SSMLDocument.CreateElement("mstts", "express-as", MicrosoftTtsNamespace);
            express.SetAttribute("style", settings.Style);

            //  Nest the elements inside each other: speak, then voice, then prosody, then style.
            prosody.AppendChild(express);
            voice.AppendChild(prosody);
            speak.AppendChild(voice);

            //  Place the text to be spoken in the innermost element.
            express.InnerText = text;
            return SSMLDocument;
        }
    }
}
