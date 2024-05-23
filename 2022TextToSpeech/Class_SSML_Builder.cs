using System.Xml;

namespace _Verbalize
{
    public class Class_SSML_Builder : Interface_SSML_Builder
    {
        private Interface_XML_Builder _xmlBuilder;

        public void SsmlBuilder()
        {
            _xmlBuilder = new XmlBuilder();
            _xmlBuilder.SetRoot("speak");
            _xmlBuilder.AddAttribute("speak", "xmlns", "http://www.w3.org/2001/10/synthesis");
        }

        public void SetSpeakAttributes(string lang = "en-US")
        {
            _xmlBuilder.AddAttribute("speak", "xml:lang", lang);
        }

        public void AddVoice(string name, string lang = "en-US")
        {
            _xmlBuilder.AddElement("voice", string.Empty);
            _xmlBuilder.AddAttribute("voice", "name", name);
            //_xmlBuilder.AddAttribute("voice", "xml:lang", lang); //this line might be needless
        }

        public void AddProsody(string rate = "default", string pitch = "default", string volume = "default", string style = "default")
        {
            _xmlBuilder.AddElement("prosody", string.Empty);
            _xmlBuilder.AddAttribute("prosody", "rate", rate);
            _xmlBuilder.AddAttribute("prosody", "pitch", pitch);
            _xmlBuilder.AddAttribute("prosody", "volume", volume);
            _xmlBuilder.AddAttribute("prosody", "style", style);
        }

        public XmlDocument Build()
        {
            return _xmlBuilder.Build();
        }
    }

}
