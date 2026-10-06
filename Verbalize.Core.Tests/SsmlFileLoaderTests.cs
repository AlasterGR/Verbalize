using System.Xml;
using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that files chosen for batch export are read as SSML, or wrapped as plain text when they are not XML. </summary>
    public sealed class SsmlFileLoaderTests : IDisposable
    {
        /// <summary> A temporary folder for the test files, removed after each test. </summary>
        private readonly string folder = Directory.CreateTempSubdirectory("verbalize-tests-").FullName;

        /// <summary> Removes the temporary folder and its files. </summary>
        public void Dispose()
        {
            //  Delete the folder and everything in it.
            Directory.Delete(folder, recursive: true);
        }

        /// <summary> Writes a test file with the given contents. </summary>
        /// <param name="name">The file's name.</param>
        /// <param name="contents">The file's contents.</param>
        /// <returns>The file's full path.</returns>
        private string WriteFile(string name, string contents)
        {
            //  Save the contents in the temporary folder.
            string path = Path.Combine(folder, name);
            File.WriteAllText(path, contents);
            return path;
        }

        /// <summary> Stands in for the app's SSML builder, wrapping text in a recognisable element. </summary>
        /// <param name="text">The text to wrap.</param>
        /// <returns>A document holding the text.</returns>
        private static XmlDocument WrapText(string text)
        {
            //  Put the text inside a "wrapped" element.
            XmlDocument document = new();
            XmlElement wrapped = document.CreateElement("wrapped");
            wrapped.InnerText = text;
            document.AppendChild(wrapped);
            return document;
        }

        /// <summary> An SSML file is used as it is. </summary>
        [Fact]
        public void Load_UsesAnSsmlFileAsItIs()
        {
            //  Load an SSML file.
            string path = WriteFile("speech.xml", "<speak xmlns=\"http://www.w3.org/2001/10/synthesis\"><voice name=\"a\">Hi</voice></speak>");
            XmlDocument document = SsmlFileLoader.Load(path, WrapText);

            //  Check the file's own document came back, not a wrapped one.
            Assert.Equal("speak", document.DocumentElement!.LocalName);
        }

        /// <summary> A plain text file is wrapped by the builder the caller provides. </summary>
        [Fact]
        public void Load_WrapsAPlainTextFile()
        {
            //  Load a plain text file.
            string path = WriteFile("speech.txt", "Hello there.\nGeneral Kenobi.");
            XmlDocument document = SsmlFileLoader.Load(path, WrapText);

            //  Check the whole text was wrapped.
            Assert.Equal("wrapped", document.DocumentElement!.Name);
            Assert.Equal("Hello there.\nGeneral Kenobi.", document.DocumentElement.InnerText);
        }

        /// <summary> A text file containing XML-like characters is still treated as text. </summary>
        [Fact]
        public void Load_WrapsTextThatOnlyLooksLikeXml()
        {
            //  Load a text file that starts with a "<" but is not valid XML.
            string path = WriteFile("speech.txt", "<3 you & me");
            XmlDocument document = SsmlFileLoader.Load(path, WrapText);

            //  Check it was wrapped, characters intact.
            Assert.Equal("<3 you & me", document.DocumentElement!.InnerText);
        }

        /// <summary> An empty file is wrapped as empty text. </summary>
        [Fact]
        public void Load_WrapsAnEmptyFile()
        {
            //  Load an empty file, and check it was wrapped as empty text.
            Assert.Equal(string.Empty, SsmlFileLoader.Load(WriteFile("empty.txt", string.Empty), WrapText).DocumentElement!.InnerText);
        }

        /// <summary> A missing file is reported as missing, not mistaken for text. </summary>
        [Fact]
        public void Load_ReportsAMissingFile()
        {
            //  Try to load a file that does not exist, and check the error says so.
            Assert.Throws<FileNotFoundException>(() => SsmlFileLoader.Load(Path.Combine(folder, "missing.txt"), WrapText));
        }
    }
}
