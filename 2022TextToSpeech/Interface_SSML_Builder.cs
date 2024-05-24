using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace _Verbalize
{
    public interface Interface_SSML_Builder
    {
        /// <summary> https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-structure#speak-root-element </summary>
        /// <param name="version"></param>
        /// <param name="xmlns"></param>
        /// <param name="xml_lang"></param>
        void SetSpeakAttributes(string version, string xmlns, string xml_lang);

        void AddVoice(string name, string effect);

        /// <summary>
        /// Use the mstts:silence element to insert pauses before or after text, or between two adjacent sentences.
        /// Silence only works at the beginning or end of input text or at the boundary of two adjacent sentences.
        /// The silence setting is applied to all input text within its enclosing voice element. To reset or change the silence setting again, you must use a new voice element with either the same voice or a different voice.
        /// Usage of the mstts:silence element's attributes are described at https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-structure#add-silence.
        /// </summary>
        /// <param name="type">Required.</param>
        /// <param name="value">Required.</param>
        void AddSilence(string type, string value);

        /// <summary>
        /// Use the empty break element to override the default behavior of breaks or pauses between words. Can be inserted anywhere in the text.
        /// Otherwise the Speech service automatically inserts pauses.
        /// Usage of the break element's attributes are described at https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-structure#add-a-break.
        /// </summary>
        /// <param name="strength">Optional.</param>
        /// <param name="time">Optional.</param>
        void AddBreak(string strength, string time);

        /// <summary>
        /// The p element is used to denote paragraphs. In the absence of this element, the Speech service automatically determines the structure of the SSML document.
        /// https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-structure#specify-paragraphs-and-sentences
        /// </summary>
        void AddParagraph();
        /// <summary>
        /// The s element is used to denote sentences. In the absence of this element, the Speech service automatically determines the structure of the SSML document.
        /// https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-structure#specify-paragraphs-and-sentences
        /// </summary>
        void AddSentence();
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
