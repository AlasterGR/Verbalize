using System.Xml;
using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that the voice list sent by Azure is converted into the app's XML layout. </summary>
    public class VoiceListConverterTests
    {
        /// <summary> A small voice list in the same shape Azure sends: one voice with styles and one without. </summary>
        private const string SampleJson = """
            [
              {
                "Name": "Microsoft Server Speech Text to Speech Voice (en-US, JennyNeural)",
                "DisplayName": "Jenny",
                "LocalName": "Jenny",
                "ShortName": "en-US-JennyNeural",
                "Gender": "Female",
                "Locale": "en-US",
                "LocaleName": "English (United States)",
                "StyleList": [ "assistant", "chat", "cheerful" ],
                "SampleRateHertz": "48000",
                "VoiceType": "Neural",
                "Status": "GA",
                "WordsPerMinute": "152"
              },
              {
                "Name": "Microsoft Server Speech Text to Speech Voice (el-GR, AthinaNeural)",
                "DisplayName": "Athina",
                "LocalName": "Αθηνά",
                "ShortName": "el-GR-AthinaNeural",
                "Gender": "Female",
                "Locale": "el-GR",
                "LocaleName": "Greek (Greece)",
                "SampleRateHertz": "24000",
                "VoiceType": "Neural",
                "Status": "GA"
              }
            ]
            """;

        /// <summary> Each voice becomes a "Voice" element under the chosen top element. </summary>
        [Fact]
        public void ConvertJsonToXml_CreatesOneVoiceElementPerVoice()
        {
            //  Convert the sample list.
            XmlDocument document = VoiceListConverter.ConvertJsonToXml(SampleJson, "Voices");

            //  Check the top element and the number of voices.
            Assert.Equal("Voices", document.DocumentElement!.Name);
            Assert.Equal(2, document.DocumentElement.SelectNodes("Voice")!.Count);
        }

        /// <summary> Each field of a voice becomes an element of the same name. </summary>
        [Fact]
        public void ConvertJsonToXml_KeepsEachFieldOfAVoice()
        {
            //  Convert the sample list and take the second voice.
            XmlDocument document = VoiceListConverter.ConvertJsonToXml(SampleJson, "Voices");
            XmlNode athina = document.DocumentElement!.SelectNodes("Voice")![1]!;

            //  Check its fields, including text outside the Latin alphabet.
            Assert.Equal("el-GR-AthinaNeural", athina.SelectSingleNode("ShortName")!.InnerText);
            Assert.Equal("Αθηνά", athina.SelectSingleNode("LocalName")!.InnerText);
            Assert.Equal("Greek (Greece)", athina.SelectSingleNode("LocaleName")!.InnerText);
        }

        /// <summary> A list of styles becomes one "StyleList" element per style. </summary>
        [Fact]
        public void ConvertJsonToXml_TurnsTheStyleListIntoRepeatedElements()
        {
            //  Convert the sample list and take the first voice's styles.
            XmlDocument document = VoiceListConverter.ConvertJsonToXml(SampleJson, "Voices");
            XmlNodeList styles = document.DocumentElement!.SelectSingleNode("Voice")!.SelectNodes("StyleList")!;

            //  Check each style is there, in order.
            Assert.Equal(new[] { "assistant", "chat", "cheerful" }, styles.Cast<XmlNode>().Select(style => style.InnerText));
        }

        /// <summary> The document starts with the standard XML declaration line. </summary>
        [Fact]
        public void ConvertJsonToXml_AddsTheXmlDeclaration()
        {
            //  Convert the sample list.
            XmlDocument document = VoiceListConverter.ConvertJsonToXml(SampleJson, "Voices");

            //  Check the first line is the declaration.
            XmlDeclaration declaration = Assert.IsType<XmlDeclaration>(document.FirstChild);
            Assert.Equal("1.0", declaration.Version);
            Assert.Equal("utf-8", declaration.Encoding);
        }

        /// <summary> The converted list can be used straight away to fill the app's lists. </summary>
        [Fact]
        public void ConvertJsonToXml_OutputWorksWithTheVoiceCatalog()
        {
            //  Convert the sample list.
            XmlDocument document = VoiceListConverter.ConvertJsonToXml(SampleJson, "Voices");

            //  Check the languages and styles can be looked up from it.
            Assert.Equal(new[] { "English (United States)", "Greek (Greece)" }, VoiceCatalog.GetLocaleNames(document));
            Assert.Equal(new[] { "assistant", "chat", "cheerful" }, VoiceCatalog.GetStyles(document, "Jenny"));
        }
    }
}
