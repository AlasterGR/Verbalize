using _Verbalize.Properties;
using Microsoft.CognitiveServices.Speech;
using System.Xml;
using Verbalize.Core;

namespace _Verbalize
{
    internal class Handler_Data
    {
        public static SpeechConfig config;
        public static string pitch = string.Empty;
        /// <summary>  Rate is expressed in 2 ways, an absolute value (string) and a relative (as a number) one. For now, we will use it only as a number (-50% - +50%), I will incoroprate it as a string later </summary>
        public static string rate = string.Empty;
        /// <summary>  Defaults in 80. </summary>
        public static int volume = 0;
        /// <summary>  The Voice's style. Defaults to "calm" </summary>
        public static string style = string.Empty;
        /// <summary>  A file that needs to be used throughout the entire app. Might put it within the VoicesLoad() nethod if possible.</summary>
        private static XmlDocument VoicesXML = new();
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
        /// <summary> Creates an SSML document that speaks the given text with the voice settings currently chosen in the app. </summary>
        /// <param name="text">The text to be spoken.</param>
        /// <returns>The SSML document, ready to be spoken or saved.</returns>
        public static XmlDocument CreateSSML(string text)
        {
            //  Collect the voice, language and prosody currently chosen in the app.
            config = Handler_AudioSynthesis.GetConfig();
            pitch = Form1.pitch;
            rate = Form1.rate;
            volume = Form1.volume;
            style = Form1.style;

            //  Build the SSML document around the text with those choices.
            SsmlVoiceSettings settings = new(config.SpeechSynthesisLanguage, config.SpeechSynthesisVoiceName, rate, pitch, volume, style);
            return SsmlDocumentFactory.Create(text, settings);
        }

        /// <summary> Converts the voice list downloaded from Azure from JSON into XML and saves it in the app's Resources folder. </summary>
        /// <param name="TextJSONFile">The voice list as sent by Azure.</param>
        public static void Convert_JSONtoXML_AndSaveToDisk(string TextJSONFile)
        {
            //  Convert the list into XML, using the voices file's name for the top element.
            string rootName = Handler_File.voicesSSMLFileName;
            XmlDocument xmlDoc = VoiceListConverter.ConvertJsonToXml(TextJSONFile, rootName);

            //  Save the list in the Resources folder, under a file name with no extension.
            xmlDoc.Save(Path.Combine(Handler_File.folderResources, rootName));
        }

        public static string GetTheVoicesListDefaultUriPart()
        {
            string voicesListRetrieveUriPartDefault = (string)Resources.ResourceManager.GetObject("voicesListRetrieveUriPartDefault");

            return voicesListRetrieveUriPartDefault;
        }
        public static HttpClient CreateHttpClientWithSubscriptionKey()
        {
            // create the client object
            var client = new HttpClient();
            string subscriptionKey = GetTheSubscriptionKey();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            return client;
        }
        public static string CreateUriOfServerLocationForDataType(string _serverLocation, string _dataType)
        {
            string uri = string.Empty;
            //string listVoicesLocationURL = "https://" + _serverLocation + voicesListDefaultUriPart;
            switch (_dataType)
            {
                case "VoicesList":
                    uri = "https://" + _serverLocation + GetTheVoicesListDefaultUriPart();
                    break;
                default:
                    break;
            }
            return uri;
        }
        public static async Task<string> TransformHttpResponceIntoString(HttpResponseMessage _message)
        {
            string message = string.Empty;
            // turn the response into string data
            if (_message != null)
            {
                using (Stream responseStream = await _message.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new(responseStream))
                    {
                        message = reader.ReadToEnd();
                    }
                }
            }
            return message;
            //SSML_JSONtoXMLConvert(responseData);
        }

    }
}
