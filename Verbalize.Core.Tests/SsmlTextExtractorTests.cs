using System.Xml;
using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that the words to be spoken are taken correctly from loaded files. </summary>
    public class SsmlTextExtractorTests
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

        /// <summary> The text is taken without any of the markup around it. </summary>
        [Fact]
        public void GetSpeakableText_ReturnsTheTextWithoutMarkup()
        {
            //  Load a document like the ones the app saves.
            XmlDocument document = Load(
                "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\">" +
                "<voice name=\"x\"><prosody><mstts:express-as style=\"calm\">Hello there</mstts:express-as></prosody></voice></speak>");

            //  Check only the words come back.
            Assert.Equal("Hello there", SsmlTextExtractor.GetSpeakableText(document));
        }

        /// <summary> Spaces and line breaks between elements are not counted as text. </summary>
        [Fact]
        public void GetSpeakableText_IgnoresBlankSpaceBetweenElements()
        {
            //  Load a neatly indented document.
            XmlDocument document = new() { PreserveWhitespace = true };
            document.LoadXml("<speak>\n  <voice>\n    Spoken words\n  </voice>\n</speak>");

            //  Check the words come back, with the indentation around them kept as written.
            Assert.Equal("\n    Spoken words\n  ", SsmlTextExtractor.GetSpeakableText(document));
        }

        /// <summary> A document with no words gives an empty result. </summary>
        [Fact]
        public void GetSpeakableText_ReturnsEmptyWhenThereIsNoText()
        {
            //  Load a document with no words in it.
            XmlDocument document = Load("<speak><voice name=\"x\"/></speak>");

            //  Check the result is empty.
            Assert.Equal(string.Empty, SsmlTextExtractor.GetSpeakableText(document));
        }

        /// <summary> Known limitation: with several pieces of text, only the last is kept. This test records the current behaviour, so any change to it is deliberate. </summary>
        [Fact]
        public void GetSpeakableText_KeepsOnlyTheLastPieceOfText()
        {
            //  Load a document with two voices, each saying something.
            XmlDocument document = Load("<speak><voice name=\"a\">First part</voice><voice name=\"b\">Second part</voice></speak>");

            //  Check only the second part comes back.
            Assert.Equal("Second part", SsmlTextExtractor.GetSpeakableText(document));
        }
    }
}
