using System.Xml;
using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that voice settings are read correctly from loaded SSML files. </summary>
    public class SsmlSettingsReaderTests
    {
        /// <summary> Loads an XML document from text. </summary>
        /// <param name="xml">The XML text.</param>
        /// <returns>The loaded document.</returns>
        private static XmlDocument Load(string xml)
        {
            //  Parse the text into a document.
            XmlDocument document = new();
            document.LoadXml(xml);
            return document;
        }

        /// <summary> A complete SSML file gives back every one of its settings. </summary>
        [Fact]
        public void Read_ReturnsEverySettingFromACompleteDocument()
        {
            //  Load a document with every setting present.
            XmlDocument document = Load(
                "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\" xml:lang=\"el-GR\">" +
                "<voice name=\"el-GR-AthinaNeural\"><prosody rate=\"slow\" pitch=\"+10Hz\" volume=\"45\">" +
                "<mstts:express-as style=\"cheerful\">Γεια</mstts:express-as></prosody></voice></speak>");

            //  Check each setting.
            Assert.Equal(new SsmlLoadedSettings("el-GR", "el-GR-AthinaNeural", "cheerful", "slow", "+10Hz", 45), SsmlSettingsReader.Read(document));
        }

        /// <summary> A file with no settings at all gets the app's defaults. </summary>
        [Fact]
        public void Read_UsesDefaultsWhenNothingIsStated()
        {
            //  Load a bare SSML document.
            XmlDocument document = Load("<speak xmlns=\"http://www.w3.org/2001/10/synthesis\">Hello</speak>");

            //  Check every value is a default and the volume is left unset.
            Assert.Equal(new SsmlLoadedSettings("en-US", "en-US-JennyNeural", "calm", "default", "default", null), SsmlSettingsReader.Read(document));
        }

        /// <summary> Plain XML that is not SSML gets the app's defaults rather than an error. </summary>
        [Fact]
        public void Read_UsesDefaultsForXmlThatIsNotSsml()
        {
            //  Load XML without the SSML vocabulary, even though its element names look similar.
            XmlDocument document = Load("<speak xml:lang=\"fr-FR\"><voice name=\"fr-FR-DeniseNeural\">Bonjour</voice></speak>");

            //  Check the look-alike values are ignored.
            SsmlLoadedSettings loaded = SsmlSettingsReader.Read(document);
            Assert.Equal("en-US", loaded.Language);
            Assert.Equal("en-US-JennyNeural", loaded.VoiceName);
        }

        /// <summary> A blank language or voice name counts as missing. </summary>
        [Fact]
        public void Read_TreatsBlankLanguageAndVoiceAsMissing()
        {
            //  Load a document whose language and voice name are empty.
            XmlDocument document = Load("<speak xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"\"><voice name=\"\">Hi</voice></speak>");

            //  Check both fall back to their defaults.
            SsmlLoadedSettings loaded = SsmlSettingsReader.Read(document);
            Assert.Equal("en-US", loaded.Language);
            Assert.Equal("en-US-JennyNeural", loaded.VoiceName);
        }

        /// <summary> A volume above 100 or below 0 is brought back into range. </summary>
        [Theory]
        [InlineData("150", 100)]
        [InlineData("-20", 0)]
        [InlineData("+30", 30)]
        [InlineData("75", 75)]
        public void Read_KeepsTheVolumeBetweenZeroAndOneHundred(string volumeText, int expectedVolume)
        {
            //  Load a document with the given volume.
            XmlDocument document = Load($"<speak xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"x\"><prosody volume=\"{volumeText}\">Hi</prosody></voice></speak>");

            //  Check the volume that was read.
            Assert.Equal(expectedVolume, SsmlSettingsReader.Read(document).Volume);
        }

        /// <summary> A volume written as a word, which has no number in it, is left unset. </summary>
        [Fact]
        public void Read_LeavesTheVolumeUnsetWhenItHasNoNumber()
        {
            //  Load a document whose volume is the word "loud".
            XmlDocument document = Load("<speak xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"x\"><prosody volume=\"loud\">Hi</prosody></voice></speak>");

            //  Check no volume was read.
            Assert.Null(SsmlSettingsReader.Read(document).Volume);
        }

        /// <summary> When the text sits directly inside the prosody element, with no style element, the default style is used. </summary>
        [Fact]
        public void Read_UsesTheDefaultStyleWhenProsodyHoldsTextDirectly()
        {
            //  Load a document with no style element inside the prosody element.
            XmlDocument document = Load("<speak xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"x\"><prosody rate=\"fast\">Hi</prosody></voice></speak>");

            //  Check the style is the default and the rate is still read.
            SsmlLoadedSettings loaded = SsmlSettingsReader.Read(document);
            Assert.Equal("calm", loaded.Style);
            Assert.Equal("fast", loaded.Rate);
        }

        /// <summary> Only the first voice and its settings are read when a document has several. </summary>
        [Fact]
        public void Read_UsesTheFirstVoiceWhenThereAreSeveral()
        {
            //  Load a document with two voices.
            XmlDocument document = Load(
                "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\">" +
                "<voice name=\"first\"><prosody rate=\"slow\">One</prosody></voice>" +
                "<voice name=\"second\"><prosody rate=\"fast\">Two</prosody></voice></speak>");

            //  Check the first voice's values are the ones read.
            SsmlLoadedSettings loaded = SsmlSettingsReader.Read(document);
            Assert.Equal("first", loaded.VoiceName);
            Assert.Equal("slow", loaded.Rate);
        }
    }
}
