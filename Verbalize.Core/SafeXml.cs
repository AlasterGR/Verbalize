using System.Xml;

namespace Verbalize.Core
{
    /// <summary> Reads XML files in a way that a crafted file cannot use to hang the app or reach other files. </summary>
    public static class SafeXml
    {
        /// <summary> Reads an XML file into a new document. </summary>
        /// <param name="path">The file to read.</param>
        /// <returns>The document.</returns>
        /// <exception cref="XmlException">The file is not XML, or it contains a document type definition, which is refused.</exception>
        public static XmlDocument Load(string path)
        {
            //  Read the file into a fresh document.
            XmlDocument document = new();
            LoadInto(document, path);
            return document;
        }

        /// <summary> Reads an XML file into an existing document, replacing what it held. </summary>
        /// <param name="document">The document to fill.</param>
        /// <param name="path">The file to read.</param>
        /// <exception cref="XmlException">The file is not XML, or it contains a document type definition, which is refused.</exception>
        public static void LoadInto(XmlDocument document, string path)
        {
            //  Refuse document type definitions, which can expand without end, and never fetch anything the file points to.
            XmlReaderSettings settings = new() { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null };
            document.XmlResolver = null;

            //  Read the file, honouring any character encoding it declares.
            using FileStream stream = File.OpenRead(path);
            using XmlReader reader = XmlReader.Create(stream, settings);
            document.Load(reader);
        }
    }
}
