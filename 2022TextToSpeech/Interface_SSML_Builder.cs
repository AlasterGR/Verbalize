using System.Xml;

namespace _Verbalize
{
    public interface Interface_SSML_Builder
    {
        void SetSpeakAttributes(string lang);
        void AddVoice(string name, string lang);
        void AddProsody(string rate, string pitch, string volume, string style);
        XmlDocument Build();
    }
    #region Notes
    /*
     Default values will be :
      lang = "en-US", rate = "default", pitch = "default", volume = "default", style = "default".
    These will be set up in a dedicated way 
     */
    #endregion
}
