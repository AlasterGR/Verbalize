using System.Xml;

namespace _Verbalize
{
    /// <summary> Defines methods for building an XML document. /// </summary>
    public interface Interface_XML_Builder
    {
        /// <summary> Sets the root element of the XML document to the specified element name. </summary>
        /// <param name="rootElementName">The name of the root element to be created and added to the XML document.</param>
        void SetRoot(string rootElementName);

        /// <summary> Adds a new element with the specified name and value to the root element of the XML document. </summary>
        /// <param name="elementName">The name of the element to be created and added to the root element.</param>
        /// <param name="elementValue">The text value to be assigned to the new element.</param>
        void AddElement(string elementName, string elementValue);

        /// <summary> Adds a new attribute with the specified name and value to the specified element within the XML document. </summary>
        /// <param name="elementName">The name of the element to which the attribute will be added.</param>
        /// <param name="attributeName">The name of the attribute to be created and added to the element.</param>
        /// <param name="attributeValue">The value to be assigned to the new attribute.</param>
        void AddAttribute(string elementName, string attributeName, string attributeValue);

        /// <summary> Builds and returns the constructed XML document. </summary>
        /// <returns>The constructed <see cref="XmlDocument"/> object.</returns>
        XmlDocument Build();
    }
}