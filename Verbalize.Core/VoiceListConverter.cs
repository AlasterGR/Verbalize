using Newtonsoft.Json;
using System.Xml;

namespace Verbalize.Core
{
    /// <summary> Turns the voice list that Azure sends (as JSON) into the XML layout the app works with. </summary>
    public static class VoiceListConverter
    {
        /// <summary> Converts Azure's JSON voice list into an XML document. </summary>
        /// <param name="voicesJson">The voice list as sent by Azure: a JSON array with one object per voice.</param>
        /// <param name="rootElementName">The name of the XML element at the top of the document, for example "Voices".</param>
        /// <returns>An XML document with one "Voice" element per voice, under the named top element.</returns>
        public static XmlDocument ConvertJsonToXml(string voicesJson, string rootElementName)
        {
            //  Wrap the list so that each entry becomes a "Voice" element.
            string mainElementsName = "\"Voice\":";
            string wrappedJson = "{ " + mainElementsName + voicesJson + "}";

            //  Convert the wrapped list into XML under the chosen top element.
            XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode(wrappedJson, rootElementName)!;

            //  Add the standard XML declaration line at the start.
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.InsertBefore(xmldecl, xmlDoc.DocumentElement);
            return xmlDoc;
        }
    }
}
