using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Drawing.Drawing2D;
using System.Reflection.Metadata;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System;

namespace _Verbalize
{
    public interface Interface_SSML_Builder
    {
        /// <summary> https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-structure#speak-root-element </summary>
        /// <param name="version"></param>
        /// <param name="xmlns"></param>
        /// <param name="xml_lang"></param>
        void SetSpeakAttributes(string version, string xmlns, string xml_lang);

        /// <summary>
        /// At least one voice element must be specified within each SSML speak element.
        /// This element determines the voice that's used for text to speech.
        /// You can include multiple voice elements in a single SSML document.
        /// Each voice element can specify a different voice.You can also use the same voice multiple times with different settings, such as when you change the silence duration between sentences.
        /// /// https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-voice#use-voice-elements
        /// </summary>
        /// <param name="name">The voice used for text to speech output.</param>
        /// <param name="effect">The audio effect processor that's used to optimize the quality of the synthesized speech output for specific scenarios on devices.The following values are supported:
        /// eq_car – Optimize the auditory experience when providing high-fidelity speech in cars, buses, and other enclosed automobiles.
        /// eq_telecomhp8k – Optimize the auditory experience for narrowband speech in telecom or telephone scenarios. You should use a sampling rate of 8 kHz.If the sample rate isn't 8 kHz, the auditory quality of the output speech isn't optimized.
        /// If the value is missing or invalid, this attribute is ignored and no effect is applied.</param>
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

        /// <summary>
        /// You can use the bookmark element in SSML to reference a specific location in the text or tag sequence. 
        /// Then you use the Speech SDK and subscribe to the BookmarkReached event to get the offset of each marker in the audio stream. 
        /// The bookmark element isn't spoken.
        /// https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-structure#bookmark-element
        /// </summary>
        /// <param name="mark">The reference text of the bookmark element.</param>
        void AddBookmark(string mark);

        /// <summary>
        /// A viseme is the visual description of a phoneme in spoken language. 
        /// It defines the position of the face and mouth while a person is speaking. 
        /// You can use the mstts:viseme element in SSML to request viseme output. For more information, see Get facial position with viseme.
        /// The viseme setting is applied to all input text within its enclosing voice element.
        /// To reset or change the viseme setting again, you must use a new voice element with either the same voice or a different voice.
        /// https://learn.microsoft.com/en-us/azure/ai-services/speech-service/speech-synthesis-markup-structure#viseme-element
        /// </summary>
        /// <param name="type">The type of viseme output. redlips_front – lip-sync with viseme ID and audio offset output. FacialExpression – blend shapes output</param>
        void AddViseme(string type);


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
