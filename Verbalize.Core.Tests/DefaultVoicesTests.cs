using System.Xml;
using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks the built-in voice list that the app falls back on when it cannot download one. </summary>
    public class DefaultVoicesTests
    {
        /// <summary> Loads the built-in voice list. </summary>
        /// <returns>The built-in voice list as a document.</returns>
        private static XmlDocument LoadDefaultVoices()
        {
            //  Parse the built-in list into a document.
            XmlDocument document = new();
            document.LoadXml(DefaultVoices.Xml);
            return document;
        }

        /// <summary> The built-in list is valid XML with voices in it. </summary>
        [Fact]
        public void Xml_IsAValidVoiceList()
        {
            //  Load the built-in list.
            XmlDocument document = LoadDefaultVoices();

            //  Check it has the expected top element and at least one voice.
            Assert.Equal("Voices", document.DocumentElement!.Name);
            Assert.NotEmpty(document.DocumentElement.SelectNodes("Voice")!);
        }

        /// <summary> Every voice has all the details the app needs, so choosing any of them cannot fail. </summary>
        [Fact]
        public void Xml_EveryVoiceHasTheDetailsTheAppNeeds()
        {
            //  Go through every voice in the built-in list.
            string[] requiredFields = { "DisplayName", "LocalName", "ShortName", "Gender", "Locale", "LocaleName" };
            foreach (XmlNode voice in LoadDefaultVoices().DocumentElement!.SelectNodes("Voice")!)
            {
                //  Check each required detail is present and not blank.
                foreach (string field in requiredFields)
                {
                    string? value = voice.SelectSingleNode(field)?.InnerText;
                    Assert.False(string.IsNullOrWhiteSpace(value), $"A voice is missing its {field}: {voice.OuterXml}");
                }
            }
        }

        /// <summary> The voice the app uses by default is in the built-in list. </summary>
        [Fact]
        public void Xml_ContainsTheDefaultVoice()
        {
            //  Look up the default voice by its Azure name.
            VoiceListing? listing = VoiceCatalog.FindVoiceByShortName(LoadDefaultVoices(), SsmlSettingsReader.DefaultVoiceName);

            //  Check it is there, as Jenny in US English.
            Assert.Equal(new VoiceListing("English (United States)", "Jenny"), listing);
        }

        /// <summary> The built-in list includes Jenny's speaking styles, which the app shows when she is chosen. </summary>
        [Fact]
        public void Xml_IncludesJennysStyles()
        {
            //  Look up Jenny's styles.
            IReadOnlyList<string> styles = VoiceCatalog.GetStyles(LoadDefaultVoices(), "Jenny");

            //  Check some of her known styles are listed.
            Assert.Contains("cheerful", styles);
            Assert.Contains("whispering", styles);
        }
    }
}
