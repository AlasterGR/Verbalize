using System.Xml;

namespace _Verbalize
{
    /// <summary> Defines methods for building an XML document. /// </summary>
    public interface Interface_XML_Builder
    {
        /// <summary> Sets the root element of the XML document to the specified element name. </summary>
        /// <param name="rootElementName">The name of the root element to be created and added to the XML document.</param>
        void SetRoot(string rootElementName);        

        /// <summary> Adds a new element with the specified name and inner text to the specified parent element of the XML document. </summary>/// <summary> Adds a new element with the specified name, value, and inner text to the specified parent element. </summary>
        /// <param name="parentElement">The parent element to which the new element will be added.</param>
        /// <param name="elementName">The name of the element to be created and added to the root element.</param>
        /// <param name="innerText">The inner text value to be assigned to the new element.</param>
        void AddElement(XmlElement parentElement, string elementName, string innerText);

        /// <summary> Adds a new attribute with the specified name and value to the specified element within the XML document. </summary>
        /// <param name="element">The xml element to which the attribute will be added.</param>
        /// <param name="attributeName">The name of the attribute to be created and added to the element.</param>
        /// <param name="attributeValue">The value to be assigned to the new attribute.</param>
        void AddAttribute(XmlElement element, string attributeName, string attributeValue);

        void SetAttribute(XmlElement element, string attributeName, string attributeValue);

        void AddInnerText(XmlElement element, string innerText);
        /// <summary> Sets the inner text of the specified element. </summary>
        /// <param name="element">The xml element whose inner text will be set.</param>
        /// <param name="innerText">The text to be set as the inner text of the element.</param>
        void SetInnerText(XmlElement element, string innerText);

        /// <summary> Builds and returns the constructed XML document. </summary>
        /// <returns>The constructed <see cref="XmlDocument"/> object.</returns>
        XmlDocument Build();
    }
}