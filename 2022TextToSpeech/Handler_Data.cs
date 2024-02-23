using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;
using NAudio.MediaFoundation;
using NAudio.Wave;
using _Verbalize.Properties;

namespace _Verbalize
{
    internal class Handler_Data
    {
        public static SpeechConfig config ;
        public static string pitch = string.Empty;
        /// <summary>  Rate is expressed in 2 ways, an absolute value (string) and a relative (as a number) one. For now, we will use it only as a number (-50% - +50%), I will incoroprate it as a string later </summary>
        public static string rate = string.Empty;
        /// <summary>  Defaults in 80. </summary>
        public static int volume = 0;
        /// <summary>  The Voice's style. Defaults to "calm" </summary>
        public static string style = string.Empty;
        public static void Initialize()
        {
            pitch = Form1.pitch;
            rate = Form1.rate;
            volume = Form1.volume;
            style = Form1.style;
        }
        public static string GetTheSubscriptionKey()
        {
            string subscriptionKey = (string)Resources.ResourceManager.GetObject("subscriptionKey1");
            return subscriptionKey; 
        }
        public static string GetTheServerLocation()
        {
            string serverLocation = (string)Resources.ResourceManager.GetObject("serverLocation");
            return serverLocation;
        }
        public static XmlDocument CreateSSML(string text)
        {
            #region Creation of the XML document and its root element
            XmlDocument SSMLDocument = new();
            XmlElement speak = SSMLDocument.CreateElement("speak");
            SSMLDocument.AppendChild(speak);
            #endregion

            #region XML declaration. Mandatory for XML 1.1 and SSML also requires this : https://www.w3.org/TR/speech-synthesis/#S2.1
            XmlDeclaration xmlDeclaration = SSMLDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            SSMLDocument.InsertBefore(xmlDeclaration, speak);
            #endregion

            #region Retrieve needed data from other parts
            config = Handler_AudioSynthesis.GetConfig();
            pitch = Form1.pitch;
            rate = Form1.rate;
            volume = Form1.volume;
            style = Form1.style;
            #endregion

            #region Assigning the XML's elements and using variables for the attributes. Reference : https://www.w3.org/TR/speech-synthesis/#S3.1.1         
            XmlAttribute version = SSMLDocument.CreateAttribute("version");  //  For some reason, version cannot be 1.1
            version.Value = "1.0";
            speak.SetAttributeNode(version);
            XmlAttribute xmlns = SSMLDocument.CreateAttribute("xmlns");
            xmlns.Value = "http://www.w3.org/2001/10/synthesis";
            speak.SetAttributeNode(xmlns);
            XmlAttribute mstts = SSMLDocument.CreateAttribute("xmlns:mstts");
            mstts.Value = "http://www.w3.org/2001/mstts";
            speak.SetAttributeNode(mstts);
            XmlAttribute emo = SSMLDocument.CreateAttribute("xmlns:emo");
            emo.Value = "http://www.w3.org/2009/10/emotionml";
            speak.SetAttributeNode(emo);
            XmlAttribute lang = SSMLDocument.CreateAttribute("xml:lang");
            lang.Value = config.SpeechSynthesisLanguage;
            speak.SetAttributeNode(lang);
            XmlAttribute onlangfailure = SSMLDocument.CreateAttribute("onlangfailure");
            onlangfailure.Value = "ignoretext ";
            speak.SetAttributeNode(onlangfailure);
            XmlElement voice = SSMLDocument.CreateElement("voice");
            voice.SetAttribute("name", config.SpeechSynthesisVoiceName);
            XmlElement prosody = SSMLDocument.CreateElement("prosody");
            prosody.SetAttribute("rate", rate);
            prosody.SetAttribute("pitch", pitch);
            prosody.SetAttribute("volume", (volume).ToString()); //  Might not work on this version of SSML
            XmlElement express = SSMLDocument.CreateElement("mstts", "express-as", "http://www.w3.org/2001/mstts");
            express.SetAttribute("style", style);
            #endregion
            #region Appending the XML elements to their parents
            prosody.AppendChild(express);
            voice.AppendChild(prosody);
            speak.AppendChild(voice);
            #endregion
            //  Entering the .txt file's text as the xml's main -inner- text
            express.InnerText = text;
            return SSMLDocument;
        }

        public static void SSML_JSONtoXMLConvert(string TextJSONFile)
        {
            string rootName = Handler_File.voicesSSMLFileName;  //  Change to whatever we need to have as root
            string mainElementsName = "\"Voice\":";
            string textJSONFile = "{ " + mainElementsName + TextJSONFile + "}";
            XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode(textJSONFile, rootName);
            /*  Create an XML declaration and then add it to the xml document  */
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.InsertBefore(xmldecl, xmlDoc.DocumentElement);
            /*  Save the xml file but with no extention */
            xmlDoc.Save(Path.Combine(Handler_File.folderResources, rootName));
        }

        
    }
}
