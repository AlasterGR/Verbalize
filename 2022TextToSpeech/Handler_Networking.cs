using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Verbalize
{
    internal class Handler_Networking
    {
        private static string subscriptionKey = string.Empty;
        private static string serverLocation = string.Empty;
        public static void Initialize()
        {
            subscriptionKey = Handler_Data.GetTheSubscriptionKey();
            serverLocation = Handler_Data.GetTheServerLocation();
        }
        public static async Task RetrieveVoices()  // need to make it wait until it is finished
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string listVoicesLocationURL = "https://" + serverLocation + ".tts.speech.microsoft.com/cognitiveservices/voices/list";
            HttpResponseMessage response = await client.GetAsync(listVoicesLocationURL);
            if (response.IsSuccessStatusCode)
            {
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new(responseStream))
                    {
                        string responseData = reader.ReadToEnd();
                        Handler_Data.SSML_JSONtoXMLConvert(responseData);
                    }
                }
            }
        }
    }
}
