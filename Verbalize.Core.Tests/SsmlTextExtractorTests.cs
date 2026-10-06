using System.Xml;
using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that the words to be spoken are taken correctly from loaded files. </summary>
    public class SsmlTextExtractorTests
    {
        /// <summary> The line break used in these tests. </summary>
        private const string LineBreak = "\n";

        /// <summary> Loads an XML document from text, the same way the app loads files. </summary>
        /// <param name="xml">The XML text.</param>
        /// <returns>The loaded document.</returns>
        private static XmlDocument Load(string xml)
        {
            //  Parse the text into a document.
            XmlDocument document = new();
            document.LoadXml(xml);
            return document;
        }

        /// <summary> Takes the speakable text from XML, using a plain line break between blocks. </summary>
        /// <param name="xml">The XML text.</param>
        /// <returns>The speakable text.</returns>
        private static string Extract(string xml)
        {
            //  Load the XML and take its text.
            return SsmlTextExtractor.GetSpeakableText(Load(xml), LineBreak);
        }

        /// <summary> The text is taken without any of the markup around it. </summary>
        [Fact]
        public void GetSpeakableText_ReturnsTheTextWithoutMarkup()
        {
            //  Take the text from a document like the ones the app saves, and check only the words come back.
            Assert.Equal("Hello there", Extract(
                "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\">" +
                "<voice name=\"x\"><prosody><mstts:express-as style=\"calm\">Hello there</mstts:express-as></prosody></voice></speak>"));
        }

        /// <summary> Spaces and line breaks around the text, such as indentation, are removed. </summary>
        [Fact]
        public void GetSpeakableText_RemovesTheSpaceAroundTheText()
        {
            //  Take the text from a neatly indented document, and check the indentation is gone.
            Assert.Equal("Spoken words", Extract("<speak>\n  <voice>\n    Spoken words\n  </voice>\n</speak>"));
        }

        /// <summary> A document with no words gives an empty result. </summary>
        [Fact]
        public void GetSpeakableText_ReturnsEmptyWhenThereIsNoText()
        {
            //  Take the text from a document with no words, and check the result is empty.
            Assert.Equal(string.Empty, Extract("<speak><voice name=\"x\"/></speak>"));
        }

        /// <summary> Every voice's text is kept, each on its own line. This used to keep only the last voice's text. </summary>
        [Fact]
        public void GetSpeakableText_KeepsEveryVoicesText()
        {
            //  Take the text from a document with two voices, and check both parts come back.
            Assert.Equal("First part" + LineBreak + "Second part", Extract(
                "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"a\">First part</voice><voice name=\"b\">Second part</voice></speak>"));
        }

        /// <summary> Paragraphs and sentences each go on their own line. </summary>
        [Fact]
        public void GetSpeakableText_PutsParagraphsAndSentencesOnTheirOwnLines()
        {
            //  Take the text from a paragraph of two sentences followed by a second paragraph.
            string text = Extract(
                "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"a\">" +
                "<p><s>One.</s><s>Two.</s></p><p>Three.</p></voice></speak>");

            //  Check each one is on its own line.
            Assert.Equal("One." + LineBreak + "Two." + LineBreak + "Three.", text);
        }

        /// <summary> Text interrupted by a pause stays on one line, with its own spacing. </summary>
        [Fact]
        public void GetSpeakableText_KeepsTextAroundAPauseTogether()
        {
            //  Take the text from a sentence with a pause in the middle, and check it stays on one line as written.
            Assert.Equal("Wait for it  now", Extract(
                "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"a\">Wait for it <break time=\"500ms\"/> now</voice></speak>"));
        }

        /// <summary> Emphasised words next to each other do not run together. </summary>
        [Fact]
        public void GetSpeakableText_KeepsAdjacentWordsApart()
        {
            //  Take the text from two emphasised words with only markup between them, and check they stay separate words.
            Assert.Equal("very good", Extract(
                "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"a\"><emphasis>very</emphasis> <emphasis>good</emphasis></voice></speak>"));
        }

        /// <summary> Text in different speaking styles goes on separate lines. </summary>
        [Fact]
        public void GetSpeakableText_PutsEachSpeakingStyleOnItsOwnLine()
        {
            //  Take the text from one voice speaking in two styles, and check each style's text is on its own line.
            Assert.Equal("Hooray!" + LineBreak + "Oh no.", Extract(
                "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\"><voice name=\"a\">" +
                "<mstts:express-as style=\"cheerful\">Hooray!</mstts:express-as><mstts:express-as style=\"sad\">Oh no.</mstts:express-as></voice></speak>"));
        }

        /// <summary> In XML that is not SSML, each element's text goes on its own line. </summary>
        [Fact]
        public void GetSpeakableText_PutsEachElementOfOtherXmlOnItsOwnLine()
        {
            //  Take the text from plain XML with two elements, and check each is on its own line.
            Assert.Equal("1" + LineBreak + "2", Extract("<root><a>1</a><b>2</b></root>"));
        }

        /// <summary> The line break the caller asks for is the one used, so Windows text boxes get the line break they need. </summary>
        [Fact]
        public void GetSpeakableText_UsesTheRequestedLineBreak()
        {
            //  Take the text from two voices using a Windows line break, and check it is used between them.
            string text = SsmlTextExtractor.GetSpeakableText(Load("<speak><voice>A</voice><voice>B</voice></speak>"), "\r\n");
            Assert.Equal("A\r\nB", text);
        }
    }
}
