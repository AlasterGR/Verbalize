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
            var client = Handler_Data.CreateHttpClientWithSubscriptionKey(_subscriptionKey);

            // prepare the string of the URI/URL 
            string uri = Handler_Data.CreateUriOfServerLocationForDataType(_serverLocation, _dataType);
            MessageBox.Show("The url to be used is " + uri +".");
            if(uri == "https://westeurope.tts.speech.microsoft.com/cognitiveservices/voices/list")
            {
                MessageBox.Show("The url is correct.");
            }
            else
            {                
                MessageBox.Show("The url is NOT correct.");
                
            }
            // get and store the response from the server with the URI 
            HttpResponseMessage response = await GetResponseMessageFromClientWithUri(client, uri);

            return response;           
        }
        
        
        public static async Task<HttpResponseMessage> GetResponseMessageFromClientWithUri(HttpClient _client, string _Uri)
        {
            HttpClient client = _client; 
            
            // get and store the response from the server of the URI
            HttpResponseMessage response = await client.GetAsync(_Uri); // In this case, it will be a Json file

            if (response.IsSuccessStatusCode) { return response; }
            else return null;
        }
        
    }
}
