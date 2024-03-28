using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Verbalize
{
    internal class Handler_Networking
    {
        // If, at some point, MS changes Cognitive Services authorization protocols, https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/rest-text-to-speech provides the methods used
        private static string subscriptionKey = string.Empty;
        private static string serverLocation = string.Empty;
        private static string voicesListDefaultUriPart = string.Empty;
        public static void Initialize()
        {
            subscriptionKey = Handler_Data.GetTheSubscriptionKey();
            serverLocation = Handler_Data.GetTheServerLocation();
            voicesListDefaultUriPart = Handler_Data.GetTheVoicesListDefaultUriPart();
        }
        public static async Task RetrieveVoices()  // need to make it wait until it is finished
        {
            // create the client object
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            // prepare the string of the URI/URL
            string listVoicesLocationURL = "https://" + serverLocation + ".tts.speech.microsoft.com/cognitiveservices/voices/list";
            // get and store the response from the server of the URI 
            HttpResponseMessage response = await client.GetAsync(listVoicesLocationURL);
            // return the response if it was sucecssful 
            if (response.IsSuccessStatusCode)
            {
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new(responseStream))
                    {
                        string responseData = reader.ReadToEnd();
                        Handler_Data.Convert_JSONtoXML_AndSaveToDisk(responseData);
                    }
                }
            }
        }
        public static async Task<HttpResponseMessage> RetrieveDataFromServer(string _dataType, string _serverLocation, string _subscriptionKey)  // need to make it wait until it is finished
        {
            // Initialize the return data
            string data = string.Empty;

            // create the client object
            HttpClient client = Handler_Data.CreateHttpClientWithSubscriptionKey();

            // prepare the string of the URI/URL 
            string uri = Handler_Data.CreateUriOfServerLocationForDataType(_serverLocation, _dataType);
            string system_message = string.Empty;
            string user_message = string.Empty;
            system_message = "The url to be used is " + uri + ".";
            if(uri == "https://westeurope.tts.speech.microsoft.com/cognitiveservices/voices/list") user_message = "The url is correct.";
            else user_message = "The url is NOT correct.";
            Form1.Inform_WithSystemMessage(system_message);
            Form1.Inform_WithUserMessage(user_message);
            // get and store the response from the server with the URI 
            //HttpResponseMessage response = await GetResponseMessageFromClientWithUri(client, uri);

            Form1.Inform_WithSystemMessage("Getting response");
            Form1.Inform_WithSystemMessage("Awaiting response");
            MessageBox.Show(client.ToString() + client.DefaultRequestHeaders.ToString());
            // get and store the response from the server of the URI
            HttpResponseMessage response = await client.GetAsync(uri); // In this case, it will be a Json file
            if (response.IsSuccessStatusCode)
            {
                Form1.Inform_WithSystemMessage("Response successful");
                //return response;
            }
            else Form1.Inform_WithSystemMessage("Response unsuccessful");

            return response;           
        }
        
        
        public static async Task<HttpResponseMessage> GetResponseMessageFromClientWithUri(HttpClient _client, string _Uri)
        {
            Form1.Inform_WithSystemMessage("Getting response");
            // get and store the response from the server of the URI
            HttpResponseMessage response = await _client.GetAsync(_Uri); // In this case, it will be a Json file
            Form1.Inform_WithSystemMessage("Awaiting response");
            if (response.IsSuccessStatusCode) 
            {
                Form1.Inform_WithSystemMessage("Rresponse successful");
                return response; 
            }
            else return null;
        }
        
    }
}
