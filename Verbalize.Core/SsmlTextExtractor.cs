using System.Text;
using System.Xml;

namespace Verbalize.Core
{
    /// <summary> Pulls the words to be spoken out of an XML or SSML document, leaving the markup behind. </summary>
    public static class SsmlTextExtractor
    {
        /// <summary> The SSML elements that hold a separate block of speech, whose text goes on its own line. </summary>
        private static readonly HashSet<string> BlockElementNames = new(StringComparer.Ordinal) { "speak", "voice", "p", "s", "express-as" };

        /// <summary> Returns all the speakable text of a document, with each block of speech on its own line. </summary>
        /// <param name="document">The XML or SSML document to read.</param>
        /// <param name="lineBreak">The line break to put between blocks, for example "\r\n" for a Windows text box.</param>
        /// <returns>The text, or an empty string if there is none.</returns>
        /// <remarks> A block is a voice, paragraph, sentence or speaking style; in XML that is not SSML, each element is its own block. Text within one block is kept as written. </remarks>
        public static string GetSpeakableText(XmlDocument document, string lineBreak)
        {
            //  Find every piece of text in the document that is not just spaces or line breaks.
            List<StringBuilder> blocks = new();
            XmlNode? currentBlock = null;
            XmlNodeList? nodes = document.SelectNodes("//text()[normalize-space()]");

            //  Go through the pieces in order, starting a new block whenever a piece belongs to a different one.
            foreach (XmlNode node in nodes?.Cast<XmlNode>() ?? Enumerable.Empty<XmlNode>())
            {
                XmlNode? block = FindBlock(node);
                if (blocks.Count == 0 || block != currentBlock)
                {
                    blocks.Add(new StringBuilder());
                    currentBlock = block;
                }
                AppendPiece(blocks[^1], node.Value ?? string.Empty);
            }

            //  Put each block on its own line, without the spaces and line breaks around it.
            return string.Join(lineBreak, blocks.Select(block => block.ToString().Trim()));
        }

        /// <summary> Finds the block of speech that a piece of text belongs to. </summary>
        /// <param name="textNode">The piece of text.</param>
        /// <returns>The nearest voice, paragraph, sentence or speaking style around the text, or the element directly around it if there is none.</returns>
        private static XmlNode? FindBlock(XmlNode textNode)
        {
            //  Walk outwards from the text until a block element is found.
            for (XmlNode? ancestor = textNode.ParentNode; ancestor != null; ancestor = ancestor.ParentNode)
            {
                if (ancestor.NodeType == XmlNodeType.Element && BlockElementNames.Contains(ancestor.LocalName))
                {
                    return ancestor;
                }
            }

            //  Fall back to the element directly around the text, for XML that is not SSML.
            return textNode.ParentNode;
        }

        /// <summary> Adds a piece of text to a block, putting a space between two words that would otherwise run together. </summary>
        /// <param name="block">The block's text so far.</param>
        /// <param name="piece">The piece of text to add.</param>
        private static void AppendPiece(StringBuilder block, string piece)
        {
            //  Add a space only when neither side of the join already has one.
            if (block.Length > 0 && !char.IsWhiteSpace(block[^1]) && piece.Length > 0 && !char.IsWhiteSpace(piece[0]))
            {
                block.Append(' ');
            }
            block.Append(piece);
        }
    }
}
