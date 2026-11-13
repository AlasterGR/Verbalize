using System.Xml;
using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that XML files are read safely. </summary>
    public sealed class SafeXmlTests : IDisposable
    {
        /// <summary> A temporary folder for the test files, removed after each test. </summary>
        private readonly string folder = Directory.CreateTempSubdirectory("verbalize-tests-").FullName;

        /// <summary> Removes the temporary folder and its files. </summary>
        public void Dispose()
        {
            //  Delete the folder and everything in it.
            Directory.Delete(folder, recursive: true);
        }

        /// <summary> Writes a file into the temporary folder. </summary>
        /// <param name="contents">What the file holds.</param>
        /// <returns>The file's path.</returns>
        private string WriteFile(string contents)
        {
            //  Save the contents and return where they went.
            string path = Path.Combine(folder, "file.xml");
            File.WriteAllText(path, contents);
            return path;
        }

        /// <summary> An ordinary XML file is read as usual. </summary>
        [Fact]
        public void Load_ReadsOrdinaryXml()
        {
            //  Read a small XML file.
            XmlDocument document = SafeXml.Load(WriteFile("<speak><voice name=\"a\">Hi</voice></speak>"));

            //  Check its contents came through.
            Assert.Equal("Hi", document.DocumentElement!.InnerText);
        }

        /// <summary> A file with a document type definition is refused, which stops both endless entity expansion and reading other files. </summary>
        [Fact]
        public void Load_RefusesDocumentTypeDefinitions()
        {
            //  Write a file whose definition would read another file from the disk.
            string path = WriteFile("<!DOCTYPE speak [<!ENTITY secret SYSTEM \"file:///C:/Windows/win.ini\">]><speak>&secret;</speak>");

            //  Check it is refused as XML.
            Assert.Throws<XmlException>(() => SafeXml.Load(path));
        }

        /// <summary> Loading into an existing document replaces what it held. </summary>
        [Fact]
        public void LoadInto_ReplacesTheDocumentsContents()
        {
            //  Fill a document, then load a file into it.
            XmlDocument document = new();
            document.LoadXml("<old/>");
            SafeXml.LoadInto(document, WriteFile("<new/>"));

            //  Check only the file's contents remain.
            Assert.Equal("new", document.DocumentElement!.Name);
        }
    }
}
