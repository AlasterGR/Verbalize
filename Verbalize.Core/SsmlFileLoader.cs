using System.Xml;

namespace Verbalize.Core
{
    /// <summary> Reads a file to be spoken, whether it holds SSML or plain text. </summary>
    public static class SsmlFileLoader
    {
        /// <summary> Reads a file as SSML, or, if it is not XML, reads it as plain text and wraps it into SSML. </summary>
        /// <param name="path">The file to read.</param>
        /// <param name="createFromText">Turns plain text into an SSML document, for example with the app's current voice settings.</param>
        /// <returns>The SSML document to be spoken.</returns>
        public static XmlDocument Load(string path, Func<string, XmlDocument> createFromText)
        {
            //  Try to read the file as XML, which also honours any character encoding the file declares.
            XmlDocument document = new();
            try
            {
                document.Load(path);
                return document;
            }
            //  If it is not XML, take its whole contents as plain text and wrap them.
            catch (XmlException)
            {
                return createFromText(File.ReadAllText(path));
            }
        }
    }
}
