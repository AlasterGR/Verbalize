using System.Xml;
using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks the lookups that fill the app's language, voice and style lists. </summary>
    public class VoiceCatalogTests
    {
        /// <summary> A small voice list: two US English voices, a Greek voice, and a second Jenny entry to show which match wins. </summary>
        private const string SampleVoicesXml = """
            <Voices>
              <Voice><DisplayName>Jenny</DisplayName><LocalName>Jenny</LocalName><ShortName>en-US-JennyNeural</ShortName><Gender>Female</Gender><Locale>en-US</Locale><LocaleName>English (United States)</LocaleName><StyleList>chat</StyleList><StyleList>sad</StyleList></Voice>
              <Voice><DisplayName>Athina</DisplayName><LocalName>Αθηνά</LocalName><ShortName>el-GR-AthinaNeural</ShortName><Gender>Female</Gender><Locale>el-GR</Locale><LocaleName>Greek (Greece)</LocaleName></Voice>
              <Voice><DisplayName>Guy</DisplayName><LocalName>Guy</LocalName><ShortName>en-US-GuyNeural</ShortName><Gender>Male</Gender><Locale>en-US</Locale><LocaleName>English (United States)</LocaleName></Voice>
              <Voice><DisplayName>Jenny</DisplayName><LocalName>Jenny B</LocalName><ShortName>en-US-JennyBNeural</ShortName><Gender>Female</Gender><Locale>en-US</Locale><LocaleName>English (United States)</LocaleName></Voice>
            </Voices>
            """;

        /// <summary> Loads the sample voice list. </summary>
        /// <returns>The sample voice list as a document.</returns>
        private static XmlDocument LoadSample()
        {
            //  Parse the sample text into a document.
            XmlDocument document = new();
            document.LoadXml(SampleVoicesXml);
            return document;
        }

        /// <summary> Each language appears once, in the order its first voice appears. </summary>
        [Fact]
        public void GetLocaleNames_ListsEachLanguageOnceInOrder()
        {
            //  Look up the languages in the sample list.
            IReadOnlyList<string?> localeNames = VoiceCatalog.GetLocaleNames(LoadSample());

            //  Check US English comes first and appears only once.
            Assert.Equal(new[] { "English (United States)", "Greek (Greece)" }, localeNames);
        }

        /// <summary> An empty, never-loaded voice list gives no languages rather than an error. </summary>
        [Fact]
        public void GetLocaleNames_ReturnsNothingForAnEmptyDocument()
        {
            //  Look up the languages in a document that was never loaded.
            IReadOnlyList<string?> localeNames = VoiceCatalog.GetLocaleNames(new XmlDocument());

            //  Check the list is empty.
            Assert.Empty(localeNames);
        }

        /// <summary> Choosing a language lists all of its voices, in order, with the language's code. </summary>
        [Fact]
        public void GetVoicesInLocale_ListsTheLanguagesVoices()
        {
            //  Look up the US English voices.
            VoicesInLocale result = VoiceCatalog.GetVoicesInLocale(LoadSample(), "English (United States)");

            //  Check the voices and the language code.
            Assert.Equal(new[] { "Jenny", "Guy", "Jenny" }, result.DisplayNames);
            Assert.Equal("en-US", result.Locale);
        }

        /// <summary> An unknown language gives no voices and no code. </summary>
        [Fact]
        public void GetVoicesInLocale_ReturnsNothingForAnUnknownLanguage()
        {
            //  Look up a language that is not in the list.
            VoicesInLocale result = VoiceCatalog.GetVoicesInLocale(LoadSample(), "Klingon");

            //  Check nothing was found.
            Assert.Empty(result.DisplayNames);
            Assert.Null(result.Locale);
        }

        /// <summary> Choosing a voice finds its Azure name, local name and gender. </summary>
        [Fact]
        public void FindVoiceByDisplayName_FindsTheVoice()
        {
            //  Look up Athina.
            VoiceIdentity? voice = VoiceCatalog.FindVoiceByDisplayName(LoadSample(), "Athina");

            //  Check her details.
            Assert.Equal(new VoiceIdentity("el-GR-AthinaNeural", "Αθηνά", "Female"), voice);
        }

        /// <summary> When two voices share a display name, the last one wins. </summary>
        [Fact]
        public void FindVoiceByDisplayName_UsesTheLastMatch()
        {
            //  Look up Jenny, who appears twice.
            VoiceIdentity? voice = VoiceCatalog.FindVoiceByDisplayName(LoadSample(), "Jenny");

            //  Check the second Jenny is the one found.
            Assert.Equal("en-US-JennyBNeural", voice?.ShortName);
        }

        /// <summary> An unknown voice gives no result. </summary>
        [Fact]
        public void FindVoiceByDisplayName_ReturnsNullForAnUnknownVoice()
        {
            //  Look up a voice that is not in the list, and check nothing was found.
            Assert.Null(VoiceCatalog.FindVoiceByDisplayName(LoadSample(), "Nobody"));
        }

        /// <summary> A voice named in a loaded file is found by its Azure name. </summary>
        [Fact]
        public void FindVoiceByShortName_FindsTheVoice()
        {
            //  Look up Guy by his Azure name.
            VoiceListing? listing = VoiceCatalog.FindVoiceByShortName(LoadSample(), "en-US-GuyNeural");

            //  Check his language and display name.
            Assert.Equal(new VoiceListing("English (United States)", "Guy"), listing);
        }

        /// <summary> An unknown Azure name gives no result. </summary>
        [Fact]
        public void FindVoiceByShortName_ReturnsNullForAnUnknownVoice()
        {
            //  Look up a voice that is not in the list, and check nothing was found.
            Assert.Null(VoiceCatalog.FindVoiceByShortName(LoadSample(), "xx-XX-NobodyNeural"));
        }

        /// <summary> A voice's styles are listed in order, taken from the first entry with that name. </summary>
        [Fact]
        public void GetStyles_ListsTheVoicesStyles()
        {
            //  Look up Jenny's styles.
            IReadOnlyList<string> styles = VoiceCatalog.GetStyles(LoadSample(), "Jenny");

            //  Check they come from the first Jenny entry.
            Assert.Equal(new[] { "chat", "sad" }, styles);
        }

        /// <summary> A voice with no styles, an unknown voice, or no voice at all gives an empty list. </summary>
        [Theory]
        [InlineData("Athina")]
        [InlineData("Nobody")]
        [InlineData(null)]
        public void GetStyles_ReturnsNothingWhenThereAreNoStyles(string? displayName)
        {
            //  Look up the styles, and check there are none.
            Assert.Empty(VoiceCatalog.GetStyles(LoadSample(), displayName));
        }
    }
}
