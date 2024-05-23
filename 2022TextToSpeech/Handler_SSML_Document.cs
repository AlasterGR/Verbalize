//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace _Verbalize
//{
//    internal class Handler_SSML_Document
//    {
//        #region Declare all the variables
//        private string rootElement = string.Empty;
//        private string style_default = "calm";
//        private string volume_defaultL = "80";
//        private string rate_default = "default";
//        private string nameValue = "en-US-JennyNeural";
//        private string langValue = "en-US";
//        private string localeName = "English (United States)";
//        private string DisplayName = "Jenny";
//        private string value = string.Empty;
//        #endregion
//        #region Initialize default values
//        //rootElement = "speak";
//        //style_default = "calm";
//        //volume_defaultL = "80";
//        //rate_default = "default";
//        //nameValue = "en-US-JennyNeural";
//        //langValue = "en-US";
//        //localeName = "English (United States)";
//        //DisplayName = "Jenny";
//        //value = string.Empty;
//        //rootElement = "speak";
//        #endregion


//        public static XmlDocument CreateSSML()
//        {
//            #region Creation of the XML document and its root element
//            XmlDocument SSMLDocument = new();
//            XmlElement speak = SSMLDocument.CreateElement(rootElement);
//            SSMLDocument.AppendChild(speak);
//            #endregion

//            #region XML declaration. Mandatory for XML 1.1 and SSML also requires this : https://www.w3.org/TR/speech-synthesis/#S2.1
//            XmlDeclaration xmlDeclaration = SSMLDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
//            SSMLDocument.InsertBefore(xmlDeclaration, speak);
//            #endregion

//            //#region Retrieve needed data from other parts
//            //config = Handler_AudioSynthesis.GetConfig();
//            //pitch = Form1.pitch;
//            //rate = Form1.rate;
//            //volume = Form1.volume;
//            //style = Form1.style;
//            //#endregion

//            #region Assigning the XML's elements and using variables for the attributes. Reference : https://www.w3.org/TR/speech-synthesis/#S3.1.1         
//            XmlAttribute version = SSMLDocument.CreateAttribute("version");  //  For some reason, version cannot be 1.1
//            version.Value = "1.0";
//            speak.SetAttributeNode(version);
//            XmlAttribute xmlns = SSMLDocument.CreateAttribute("xmlns");
//            xmlns.Value = "http://www.w3.org/2001/10/synthesis";
//            speak.SetAttributeNode(xmlns);
//            XmlAttribute mstts = SSMLDocument.CreateAttribute("xmlns:mstts");
//            mstts.Value = "http://www.w3.org/2001/mstts";
//            speak.SetAttributeNode(mstts);
//            XmlAttribute emo = SSMLDocument.CreateAttribute("xmlns:emo");
//            emo.Value = "http://www.w3.org/2009/10/emotionml";
//            speak.SetAttributeNode(emo);
//            XmlAttribute lang = SSMLDocument.CreateAttribute("xml:lang");
//            lang.Value = config.SpeechSynthesisLanguage;
//            speak.SetAttributeNode(lang);
//            XmlAttribute onlangfailure = SSMLDocument.CreateAttribute("onlangfailure");
//            onlangfailure.Value = "ignoretext ";
//            speak.SetAttributeNode(onlangfailure);
//            XmlElement voice = SSMLDocument.CreateElement("voice");
//            voice.SetAttribute("name", config.SpeechSynthesisVoiceName);
//            XmlElement prosody = SSMLDocument.CreateElement("prosody");
//            prosody.SetAttribute("rate", rate);
//            prosody.SetAttribute("pitch", pitch);
//            prosody.SetAttribute("volume", (volume).ToString()); //  Might not work on this version of SSML
//            XmlElement express = SSMLDocument.CreateElement("mstts", "express-as", "http://www.w3.org/2001/mstts");
//            express.SetAttribute("style", style);
//            #endregion
//            #region Appending the XML elements to their parents
//            prosody.AppendChild(express);
//            voice.AppendChild(prosody);
//            speak.AppendChild(voice);
//            #endregion            

//            express.InnerText = string.Empty;
//            return SSMLDocument;
//        }

//    }
//}
