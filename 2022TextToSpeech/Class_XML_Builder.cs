using System.Xml;

namespace _Verbalize
{
    /// <summary> A class for building XML documents. </summary>
    public class XmlBuilder : Interface_XML_Builder
    {
        private readonly XmlDocument _xmlDocument = new();  // 'readonly' since it is not modified after initialization.
        private XmlElement? _rootElement;

        /// <summary> This is the constructor for the XmlBuilder class. It gets called when a new instance of XmlBuilder is created. </summary>
        public XmlBuilder()
        {
            _xmlDocument = new XmlDocument();
        }
        /// <summary> Sets the root element of the XML document to the specified element name. </summary>
        /// <param name="rootElementName">The name of the root element to be created and added to the XML document.</param>
        public void SetRoot(string rootElementName)
        {
            _rootElement = _xmlDocument.CreateElement(rootElementName);
            _xmlDocument.AppendChild(_rootElement);
        }
        /// <summary> Adds a new element with the specified name and value to the root element of the XML document. </summary>
        /// <param name="elementName">The name of the element to be created and added to the root element.</param>
        /// <param name="elementValue">The text value to be assigned to the new element.</param>
        public void AddElement(string elementName, string elementValue)
        {
            if (_rootElement == null) { throw new InvalidOperationException("Root element is not set. Call SetRoot first."); }
            XmlElement newElement = _xmlDocument.CreateElement(elementName);
            newElement.InnerText = elementValue;
            _rootElement.AppendChild(newElement);
        }
        /// <summary> Adds a new attribute with the specified name and value to the specified element within the XML document. </summary>
        /// <param name="elementName">The name of the element to which the attribute will be added.</param>
        /// <param name="attributeName">The name of the attribute to be created and added to the element.</param>
        /// <param name="attributeValue">The value to be assigned to the new attribute.</param>
        public void AddAttribute(string elementName, string attributeName, string attributeValue)
        {
            if (_rootElement == null) { throw new InvalidOperationException("Root element is not set. Call SetRoot first."); }

            if (_rootElement.SelectSingleNode(elementName) is XmlElement element) //Used pattern matching with the 'is' keyword to simplify the null check in AddAttribute method. This reduces the code complexity and makes it more concise.
            {
                XmlAttribute newAttribute = _xmlDocument.CreateAttribute(attributeName);
                newAttribute.Value = attributeValue;
                element.Attributes.Append(newAttribute);
            }
            else { throw new InvalidOperationException($"Element '{elementName}' not found in the document."); }
        }
        /// <summary> Builds and returns the constructed XML document. </summary>
        /// <returns>The constructed <see cref="XmlDocument"/> object.</returns>
        public XmlDocument Build()
        {
            return _xmlDocument;
        }
    }
}