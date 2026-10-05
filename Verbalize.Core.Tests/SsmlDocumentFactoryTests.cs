using System.Xml;
using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that the SSML sent to Azure is built correctly. </summary>
    public class SsmlDocumentFactoryTests
    {
        /// <summary> Typical settings, matching the app's defaults with Jenny's voice. </summary>
        private static readonly SsmlVoiceSettings JennySettings = new("en-US", "en-US-JennyNeural", "default", "default", 80, "calm");

        /// <summary> The whole document must match, character for character, what the app has always produced. </summary>
        [Fact]
        public void Create_ProducesTheExactExpectedDocument()
        {
            //  Build a document for a short sentence.
            XmlDocument document = SsmlDocumentFactory.Create("Hello world", JennySettings);

            //  Compare it with the known-good output.
            string expected =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\" xmlns:emo=\"http://www.w3.org/2009/10/emotionml\" xml:lang=\"en-US\" onlangfailure=\"ignoretext \">" +
                "<voice name=\"en-US-JennyNeural\">" +
                "<prosody rate=\"default\" pitch=\"default\" volume=\"80\">" +
                "<mstts:express-as style=\"calm\">Hello world</mstts:express-as>" +
                "</prosody></voice></speak>";
            Assert.Equal(expected, document.OuterXml);
        }

        /// <summary> Every chosen setting must end up in the right place. </summary>
        [Fact]
        public void Create_PlacesEachSettingInItsAttribute()
        {
            //  Build a document with every setting changed from its default.
            SsmlVoiceSettings settings = new("el-GR", "el-GR-AthinaNeural", "-20%", "+5Hz", 35, "cheerful");
            XmlDocument document = SsmlDocumentFactory.Create("Καλημέρα", settings);

            //  Check the language, voice, rate, pitch, volume, style and text.
            XmlElement speak = document.DocumentElement!;
            XmlElement voice = (XmlElement)speak.FirstChild!;
            XmlElement prosody = (XmlElement)voice.FirstChild!;
            XmlElement express = (XmlElement)prosody.FirstChild!;
            Assert.Equal("el-GR", speak.GetAttribute("xml:lang"));
            Assert.Equal("el-GR-AthinaNeural", voice.GetAttribute("name"));
            Assert.Equal("-20%", prosody.GetAttribute("rate"));
            Assert.Equal("+5Hz", prosody.GetAttribute("pitch"));
            Assert.Equal("35", prosody.GetAttribute("volume"));
            Assert.Equal("cheerful", express.GetAttribute("style"));
            Assert.Equal("Καλημέρα", express.InnerText);
        }

        /// <summary> Characters with a special meaning in XML must be escaped, not break the document. </summary>
        [Fact]
        public void Create_EscapesSpecialCharactersInTheText()
        {
            //  Build a document whose text contains XML's special characters.
            XmlDocument document = SsmlDocumentFactory.Create("Tom & Jerry <3", JennySettings);

            //  Check the text is escaped in the output and reads back unchanged.
            Assert.Contains("Tom &amp; Jerry &lt;3", document.OuterXml);
            Assert.Equal("Tom & Jerry <3", document.DocumentElement!.InnerText);
        }

        /// <summary> The style element must belong to Microsoft's vocabulary, or Azure ignores it. </summary>
        [Fact]
        public void Create_PutsTheStyleInMicrosoftsNamespace()
        {
            //  Build a document and find its style element.
            XmlDocument document = SsmlDocumentFactory.Create("Hi", JennySettings);
            XmlNode express = document.DocumentElement!.FirstChild!.FirstChild!.FirstChild!;

            //  Check its name and vocabulary.
            Assert.Equal("express-as", express.LocalName);
            Assert.Equal(SsmlDocumentFactory.MicrosoftTtsNamespace, express.NamespaceURI);
        }

        /// <summary> Empty text is allowed and gives an empty style element. </summary>
        [Fact]
        public void Create_AcceptsEmptyText()
        {
            //  Build a document with no text.
            XmlDocument document = SsmlDocumentFactory.Create(string.Empty, JennySettings);

            //  Check the document still has all its elements and no text.
            Assert.Contains("<mstts:express-as style=\"calm\"></mstts:express-as>", document.OuterXml);
        }

        /// <summary> Saving a document and loading it again must give the same settings back. </summary>
        [Fact]
        public void Create_OutputCanBeReadBackBySsmlSettingsReader()
        {
            //  Build a document, save it to text, and load it again.
            SsmlVoiceSettings settings = new("el-GR", "el-GR-NestorasNeural", "+10%", "-5Hz", 60, "sad");
            XmlDocument reloaded = new();
            reloaded.LoadXml(SsmlDocumentFactory.Create("Hi", settings).OuterXml);

            //  Check every setting survives the round trip.
            SsmlLoadedSettings loaded = SsmlSettingsReader.Read(reloaded);
            Assert.Equal(new SsmlLoadedSettings("el-GR", "el-GR-NestorasNeural", "sad", "+10%", "-5Hz", 60), loaded);
        }
    }
}
