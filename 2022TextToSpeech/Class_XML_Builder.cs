using System.Xml;

namespace _Verbalize
{
    /// <summary> A class for building XML documents. </summary>
    public class XmlBuilder : Interface_XML_Builder
    {
        private readonly XmlDocument _xmlDocument = new();  // 'readonly' since it is not modified after initialization.
        private XmlElement? _rootElement;
        private Dictionary<string, int> _elementCounts = new();

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

            _elementCounts[rootElementName] = 0; // Initialize root element count
            SetUniqueID(_rootElement, "0");
        }

        /// <summary> Adds a new element with the specified name and inner text to the specified parent element of the XML document. </summary>/// <summary> Adds a new element with the specified name, value, and inner text to the specified parent element. </summary>
        /// <param name="parentElement">The parent element to which the new element will be added.</param>
        /// <param name="elementName">The name of the element to be created and added to the root element.</param>
        /// <param name="innerText">The inner text value to be assigned to the new element.</param>
        public void AddElement(XmlElement? parentElement, string elementName, string innerText)
        {
            if (_rootElement == null) { throw new InvalidOperationException("Root element is not set. Call SetRoot first."); }
            if (parentElement == null) { throw new InvalidOperationException("Parent element is not set."); }
            XmlElement newElement = _xmlDocument.CreateElement(elementName);
            newElement.InnerText = innerText;
            parentElement.AppendChild(newElement);

            string parentId = parentElement.GetAttribute("ID");
            string newId = GenerateUniqueID(elementName, parentId);
            SetUniqueID(newElement, newId);
        }

        /// <summary> Adds a new attribute with the specified name and value to the specified element within the XML document. </summary>
        /// <param name="element">The xml element to which the attribute will be added.</param>
        /// <param name="attributeName">The name of the attribute to be created and added to the element.</param>
        /// <param name="attributeValue">The value to be assigned to the new attribute.</param>
        public void AddAttribute(XmlElement element, string attributeName, string attributeValue)
        {
            if (_rootElement == null) { throw new InvalidOperationException("Root element is not set. Call SetRoot first."); }

            if (element != null)
            {
                XmlAttribute newAttribute = _xmlDocument.CreateAttribute(attributeName);
                newAttribute.Value = attributeValue;
                element.Attributes.Append(newAttribute);
            }
            else { throw new InvalidOperationException($"Element not found in the document."); }
        }

        public void SetAttribute(XmlElement element, string attributeName, string attributeValue)
        {
            if (_rootElement == null) { throw new InvalidOperationException("Root element is not set. Call SetRoot first."); }

            if (element != null)
            {
                XmlAttribute newAttribute = _xmlDocument.CreateAttribute(attributeName);
                newAttribute.Value = attributeValue;
                element.Attributes.Append(newAttribute);

            }
            else { throw new InvalidOperationException($"Element not found in the document."); }
        }

        /// <summary> Sets the inner text of the specified element. </summary>
        /// <param name="elementName">The name of the element whose inner text will be set.</param>
        /// <param name="innerText">The text to be set as the inner text of the element.</param>
        public void SetInnerText(string elementName, string innerText)
        {
            if (_rootElement == null) { throw new InvalidOperationException("Root element is not set. Call SetRoot first."); }

            if (_rootElement.SelectSingleNode(elementName) is XmlElement element)
            {
                element.InnerText = innerText;
            }
            else { throw new InvalidOperationException($"Element '{elementName}' not found in the document."); }
        }

        /// <summary> Builds and returns the constructed XML document. </summary>
        /// <returns>The constructed <see cref="XmlDocument"/> object.</returns>
        public XmlDocument Build()
        {
            return _xmlDocument;
        }

        /// <summary> Sets a unique ID attribute for the specified element. </summary>
        /// <param name="element">The element to set the ID attribute on.</param>
        /// <param name="id">The unique ID to be assigned.</param>
        private void SetUniqueID(XmlElement element, string id)
        {
            XmlAttribute idAttribute = _xmlDocument.CreateAttribute("ID");
            idAttribute.Value = id;
            element.Attributes.Append(idAttribute);
        }

        /// <summary> Generates a unique ID for a new element based on its name and parent's ID. </summary>
        /// <param name="elementName">The name of the new element.</param>
        /// <param name="parentId">The ID of the parent element.</param>
        /// <returns>A unique ID for the new element.</returns>
        private string GenerateUniqueID(string elementName, string parentId)
        {
            if (!_elementCounts.ContainsKey(elementName))
            {
                _elementCounts[elementName] = 0;
            }

            _elementCounts[elementName]++;
            return $"{parentId}-{_elementCounts[elementName]}";
        }
    }
}