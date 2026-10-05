using System.Xml;

namespace Verbalize.Core
{
    /// <summary> Pulls the words to be spoken out of an XML or SSML document, leaving the markup behind. </summary>
    public static class SsmlTextExtractor
    {
        /// <summary> Returns the speakable text of a document. </summary>
        /// <param name="document">The XML or SSML document to read.</param>
        /// <returns>The last piece of non-blank text in the document, or an empty string if there is none.</returns>
        /// <remarks> Only the last piece of text is kept, so a document with several voices or paragraphs loses all but its final part. This matches the app's existing behaviour and is a known limitation. </remarks>
        public static string GetSpeakableText(XmlDocument document)
        {
            //  Find every piece of text in the document that is not just spaces or line breaks.
            string speakableText = string.Empty;
            XmlNodeList? nodes = document.SelectNodes("//text()[normalize-space()]");

            //  Go through the pieces in order, keeping only the last one.
            if (nodes?.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    speakableText = node.InnerText;
                }
            }
            return speakableText;
        }
    }
}
