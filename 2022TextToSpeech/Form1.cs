using Microsoft.VisualBasic.Devices;

namespace _2022TextToSpeech
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Microsoft.CognitiveServices.Speech;
    using System.Xml; // For constructing our xml file 
    using System.Net.Http; // for the supported languages of the voice
    using Newtonsoft.Json; // for converting the Json files into XML ones. Possibly removable should we only load the XML file I am going to have produced 
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    // for the audio conversion - add an ogg vorbis encoder
    using NAudio.Wave;  // for the audio conversion
    using NAudio.MediaFoundation;
    using System.Drawing.Drawing2D;

    /// <summary>
    /// The main Form of the app
    /// </summary>
    public partial class Form1 : Form
    {
        //static string regionService = Environment.GetEnvironmentVariable("westeurope");
        // If, at some point, MS changes Cognitive Services authorization protocols, https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/rest-text-to-speech provides the methods used
        /// <summary>  Azure Speech Service Location </summary>
        public static string serverLocation = "westeurope";
        readonly static string subscriptionKeyGiannis1 = "4b3dc697810e47fc845f076f446a62da";
        readonly static string subscriptionKeyGiannis2 = "120f1e685b4244d8b1260b5bbc28f9ee";
        readonly static string subscriptionKeyAlex1 = "5521b17037c34b96aa88e1ab83b34fb3";
        readonly static string subscriptionKeyAlex2 = "1491bf9d70da4dedab0f0f375beae896";
        /// <summary>  This is the single most valuable object of the app, as it holds all the important properties for the speech synthesis </summary>
        private static SpeechConfig config = SpeechConfig.FromSubscription(subscriptionKeyGiannis1, serverLocation);
        #region The Prosody and assorted elements of speech
        // As per : https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/speech-synthesis-markup-voice. The https://www.w3.org/TR/speech-synthesis11/ is irrelevant so far.
        /// <summary>  Pitch is expressed in 3 ways. Here, for now, we are using just the absolute value from the range [-200, +200]</summary>
        private static string pitch = "default";
        /// <summary>  Rate is expressed in 2 ways, an absolute value (string) and a relative (as a number) one. For now, we will use it only as a number (-50% - +50%), I will incoroprate it as a string later </summary>
        private static string rate = "default";
        /// <summary>  Defaults in 80. </summary>
        private static int volume = 80;
        /// <summary>  The Voice's style. Defaults to "calm" </summary>
        private static string style = "calm";
        //  Integrate the rest of the speech elements, such as pitch contour, pitch range
        #endregion
        /// <summary>  The app's Resources folder from which it will draw some info.</summary>
        readonly static string folderResources = Path.Combine(Environment.CurrentDirectory, @"Resources\");
        /// <summary>  The Voice Language's selected locale.</summary>
        private static string selectedLocale = string.Empty;
        /// <summary>  A file that needs to be used throughout the entire app. Might put it within the VoicesLoad() nethod if possible.</summary>
        private static XmlDocument VoicesXML = new();
        /// <summary>  Change this to the desired file name and extension.</summary>
        private static string voicesSSMLFileName = "Voices";
        /// <summary>  The file in which we store the downloaded Voices list.</summary>
        private static string locationFileResponse = Path.Combine(folderResources, voicesSSMLFileName);
        /// <summary>  Backup voices file. Change this to the desired file name and extension.</summary>
        private static string voicesSSMLFileNameBackup = "Voices_[orig].xml";
        /// <summary>  Backup voices file. The file in which we store them.</summary>
        private static string locationFileResponseBackup = Path.Combine(folderResources, voicesSSMLFileNameBackup);
        /// <summary>  The "short name" of a Voice.</summary>
        private static string shortName = string.Empty;
        /// <summary>  The path of the loaded file.</summary>
        private static string locationLoadedFile = string.Empty;
        private static Task<SpeechSynthesisResult>? spokenTextSoundResult;
        private static SpeechSynthesizer? synth;
        private string formatOutputSound = "None";
        private static bool isSynthSpeaking = false;
        public static XmlDocument objectXML = new XmlDocument(); // the dynamic object which stores the proccessed ssml

        /// <summary>  The public class of the app's main Form, that is window. </summary>
        public Form1()
        {
            InitializeComponent();
            #region Load the voices file that lists the various speech voices          
            //InitializeVoices();  Let's start with the basic voices first
            #endregion
            label11.Text = string.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Visible = false;
            VoicesLoad();  //  Load the voices onto the combo boxes
            cmbBx_SelectSavedSoundFormat.SelectedIndex = 0;
            button2.Enabled = button8.Enabled = button8.Visible = button9.Enabled = button9.Visible = false;
            vScrollBar3.Value = volume;
        }

        private void Bttn3_LoadText_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                locationLoadedFile = openFileDialog1.FileName;
                label1.Text = Path.GetFileNameWithoutExtension(locationLoadedFile); // Show the loaded file's name
                label1.Visible = true;
                string fileContents = string.Empty;
                //  Search further for advantages on using a rich text box instead. So far none found.
                #region Parse the selected file's contents and save its speakable text to the textbox ~ it will acquire only the inner texts, should the selected file have an xml format.
                XmlDocument SSMLDocument = new();
                try
                {
                    SSMLDocument.Load(locationLoadedFile);
                    XmlNodeList? nodes = SSMLDocument?.SelectNodes("//text()[normalize-space()]");
                    if (nodes?.Count > 0) { foreach (XmlNode node in nodes) { fileContents = node.InnerText; } }  // might need to put append instead of =, in order to support various voices within the text
                    if (SSMLDocument != null) { LoadXMLtoApp(SSMLDocument); }
                }
                catch (XmlException)
                { fileContents = File.ReadAllText(locationLoadedFile); }  // File.ReadAllText locks the file
                this.textBox1.Text = fileContents;
                #endregion               
            }
        }

        private void button4_Click(object sender, EventArgs e) // Produce sound from a local file
        {
            OpenFileDialog openFileDialog1 = new()
            {
                Filter = "XML Files (*.xml)|*.xml|Text Files (*.txt)|*.txt",
                Title = "Select a file to be converted into sound. Chose either a .xml or a .txt file."
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string pathFileSelected = openFileDialog1.FileName;
                formatOutputSound = cmbBx_SelectSavedSoundFormat?.SelectedItem?.ToString() ?? "None";
                _ = SynthesizeAudioAsync(pathFileSelected, formatOutputSound, false);  // "_= " is for discarding the result afterwards. Practically suppresses the warning.
            }

        }
        static async Task SynthesizeAudioAsync(string soundfile, string formatOutputSound, bool _audioOn)
        {
            #region Producing the sound data
            XmlDocument xmlDoc = new();
            xmlDoc.Load(soundfile);
            string ssmlText = xmlDoc.OuterXml;
            soundPause();
            if (!_audioOn) { synth = new SpeechSynthesizer(config, null); } // Note : SpeechSynthesizer(speechConfig, null) gets a result as an in-memory stream
            else { synth = new SpeechSynthesizer(config, null); }
            SpeechSynthesisResult result = await synth.SpeakSsmlAsync(ssmlText);
            #endregion
            #region Saving the sound data to the disk as a specific sound format
            string outputFile = Path.ChangeExtension(soundfile, "." + formatOutputSound);
            using Stream stream = new MemoryStream(result.AudioData);
            if (formatOutputSound != "none" || formatOutputSound != null)
            {
                switch (formatOutputSound)
                {
                    case "mp3":
                        //outputFile = Path.ChangeExtension(outputFile, "."+formatOutputSound);
                        MediaFoundationApi.Startup();
                        var reader = new WaveFileReader(stream);  // Normally public WaveFileReader(Stream inputStream) ought to handle it properly but it does not accept it
                        MediaFoundationEncoder.EncodeToMp3(reader, outputFile);
                        break;

                    case "wav":
                        MediaFoundationApi.Startup();
                        var reader1 = new WaveFileReader(stream);
                        WaveFileWriter.CreateWaveFile(outputFile, reader1);
                        break;

                    case "ogg": // add an ogg vorbis encoder 
                        break;
                    default:
                        break;
                }
            }
            #endregion
        }

        async void InitializeVoices()  //  See if it should be called from a button
        {
            try
            {
                VoicesXML.Load(locationFileResponse);
            }
            catch (Exception)
            {
                try
                {
                    VoicesXML.Load(locationFileResponseBackup);
                }
                catch
                {
                    System.IO.Directory.CreateDirectory(folderResources);
                    await VoicesRetrieve();
                }
            }
        }

        #region Retrieve the list of voices from speech.microsoft.com and reload the voices into the boxes
        private async void button8_Click(object sender, EventArgs e)
        {
            await VoicesRetrieve();
            VoicesLoad();
        }
        async Task VoicesRetrieve()  // need to make it wait until it is finished
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKeyGiannis1);
            string listVoicesLocationURL = "https://" + serverLocation + ".tts.speech.microsoft.com/cognitiveservices/voices/list";
            HttpResponseMessage response = await client.GetAsync(listVoicesLocationURL);
            if (response.IsSuccessStatusCode)
            {
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new(responseStream))
                    {
                        string responseData = reader.ReadToEnd();
                        SSML_JSONtoXMLConvert(responseData);
                    }
                }
            }
        }
        #endregion

        #region This function loads the list of language elements from the SSML Voice file onto the Languages combo box
        private void VoicesLoad()
        {
            if (VoicesXML.DocumentElement != null)  //  To make sure it has been downloaded and loaded
            {
                List<string> uniqueLocales = new();  //  List that will contain only the unique values of Locale, for reference, to populate the combo box with unique elements only
                comboBox1.Items.Clear();
                foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice")) //Expression "//Voice" works as well
                {
                    string? locale = node?.SelectSingleNode("Locale")?.InnerText;
                    if (!uniqueLocales.Contains(locale))
                    {
                        uniqueLocales.Add(locale);
                        string? localeName = node?.SelectSingleNode("LocaleName")?.InnerText;
                        comboBox1.Items.Add(localeName);
                    }
                    string? localName = node?.SelectSingleNode("LocalName")?.InnerText ?? "N/A";
                    string? gender = node?.SelectSingleNode("Gender")?.InnerText ?? "N/A";
                    comboBox1.SelectedIndex = 0;
                }
            }
            else { VoicesXML.LoadXml(VoicesBasic); VoicesLoad(); }
            comboBox3.SelectedItem = style;  //  Have the the default voice style preselected as default
        }
        #endregion

        #region button for Populating the two voice combo boxes from Resources\voice
        private void Button9_Click(object sender, EventArgs e)
        {
            try
            {
                VoicesXML.Load(Path.Combine(locationFileResponse)); // Use Load because LoadXML command produces error
            }
            catch (Exception)
            {
                _ = VoicesRetrieve();
            }
            VoicesLoad();
        }
        #endregion
        #region Language combo box
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> displayNames = new();
            string selectedLocaleName = comboBox1?.SelectedItem?.ToString() ?? string.Empty;  // comboBox1.SelectedItem.ToString() is not a string that can be handled within the next if()

            foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice"))
            {
                string localeName = node?.SelectSingleNode("LocaleName")?.InnerText ?? string.Empty;
                if (localeName == selectedLocaleName)
                {
                    displayNames.Add(node.SelectSingleNode("DisplayName").InnerText);
                    this.label6.Text = node.SelectSingleNode("Locale").InnerText;  //  can probably delete this
                    config.SpeechSynthesisLanguage = node.SelectSingleNode("Locale").InnerText;
                }
            }
            comboBox2.DataSource = displayNames;
            try { comboBox2.SelectedIndex = 0; } catch { } //  This clause is for when pressing to populate the boxes but with the basic Voices loaded
        }
        #endregion
        #region Voice actor combo box
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice"))
            {
                string localName = node?.SelectSingleNode("DisplayName")?.InnerText ?? string.Empty;
                string selectedDisplayName = comboBox2?.SelectedItem?.ToString() ?? string.Empty;
                if (localName == selectedDisplayName)
                {
                    config.SpeechSynthesisVoiceName = node.SelectSingleNode("ShortName").InnerText;  //  We store who our voice actor will be
                    label7.Text = " - " + node.SelectSingleNode("LocalName").InnerText + " (" + node.SelectSingleNode("Gender").InnerText + ")";
                }

            }
        }
        #endregion

        private static void SSML_JSONtoXMLConvert(string TextJSONFile)
        {
            string rootName = voicesSSMLFileName;  //  Change to whatever we need to have as root
            string mainElementsName = "\"Voice\":";
            string textJSONFile = "{ " + mainElementsName + TextJSONFile + "}";
            XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode(textJSONFile, rootName);
            /*  Create an XML declaration and then add it to the xml document  */
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.InsertBefore(xmldecl, xmlDoc.DocumentElement);
            /*  Save the xml file but with no extention */
            xmlDoc.Save(Path.Combine(folderResources, rootName));
        }

        /// <summary> Voice Pitch slider </summary>
        private void vScrollBar2_ValueChanged(object sender, EventArgs e)
        {
            pitch = vScrollBar2.Value.ToString() + "Hz";
            label2.Text = "Pitch = " + pitch;

        }
        /// <summary> Voice Rate slider </summary>
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            //rate = vScrollBar1.Value.ToString() + "%";
            label3.Text = "Rate = " + vScrollBar1.Value.ToString() + "%";
            comboBox4.SelectedItem = null;
        }
        /// <summary> Voice Volume slider </summary>
        private void vScrollBar3_ValueChanged(object sender, EventArgs e)
        {
            volume = vScrollBar3.Value;
            label11.Text = volume.ToString();
        }
        /// <summary> Voice Rate selection </summary>
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            rate = comboBox4?.SelectedItem?.ToString() ?? "Default";
            label3.Text = "Rate : " + rate;
        }
        /// <summary> Voice Pitch selection </summary>
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            pitch = comboBox5?.SelectedItem?.ToString() ?? "Default";
            label2.Text = "Pitch : " + pitch;
        }
        /// <summary> Voice Style selection </summary>
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        { style = comboBox3?.SelectedItem?.ToString() ?? "calm"; }
        /// <summary> The Save button </summary>
        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new() { Filter = "XML|*.xml", Title = "Save the text box as a .xml file compliant to the SSML type." };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string pathFileSelected = saveFileDialog1.FileName;
                string text = textBox1.Text;
                XmlDocument SSMLDocument = CreateSSML(text);
                SSMLDocument.Save(pathFileSelected);  // Save the XML document to a file
                _ = SynthesizeAudioAsync(pathFileSelected, formatOutputSound, false);
                label1.Text = Path.GetFileNameWithoutExtension(pathFileSelected); // Show the loaded file's name
            }
        }

        static XmlDocument CreateSSML(string text)
        {
            #region Creation of the XML document and and its root element
            XmlDocument SSMLDocument = new();
            XmlElement speak = SSMLDocument.CreateElement("speak");
            SSMLDocument.AppendChild(speak);
            #endregion
            #region XML declaration. Mandatory for XML 1.1 and SSML also requires this : https://www.w3.org/TR/speech-synthesis/#S2.1
            XmlDeclaration xmlDeclaration = SSMLDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            SSMLDocument.InsertBefore(xmlDeclaration, speak);
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
        /// <summary> The Save button </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            string pathFileSelected = locationLoadedFile;
            string text = this.textBox1.Text;
            XmlDocument SSMLDocument = CreateSSML(text);
            SSMLDocument.Save(pathFileSelected);// Save the XML document to a file
            _ = SynthesizeAudioAsync(pathFileSelected, formatOutputSound, false);
        }

        //  Speak the selected text from the textbox
        private void Button12_Click(object sender, EventArgs e)
        {
            soundPause();
            synth = new SpeechSynthesizer(config);
            spokenTextSoundResult = synth.SpeakSsmlAsync(CreateSSML(string.IsNullOrEmpty(textBox1.SelectedText) ? textBox1.Text : textBox1.SelectedText).OuterXml); // Create SSML from textbox's either selected text or entire text (whichever is nonempty) and feed it to the syntesizer
        }

        private void LoadXMLtoApp(XmlDocument SSMLDocument) // Load all the markup of the XML onto the u.i.
        {
            #region Initialize every mark to its default values
            string styleSSML = "calm";
            string volumeSSML = "80";
            //string pitchSSML = "default";
            string rateSSML = "default";
            string nameValue = "en-US-JennyNeural";
            string langValue = "en-US";
            string localeName = "English (United States)";
            string DisplayName = "Jenny";
            string? value = string.Empty;
            #endregion

            #region Parse the loaded document and acquire the desired values. Add try-catch...
            XmlNamespaceManager nsMgr = new(SSMLDocument.NameTable);
            nsMgr.AddNamespace("speak", "http://www.w3.org/2001/10/synthesis");
            // Find the general default language of the entire document
            XmlNode? speakNode = SSMLDocument.SelectSingleNode("//speak:speak", nsMgr);
            value = speakNode?.Attributes["xml:lang"]?.Value;
            langValue = string.IsNullOrEmpty(value) ? langValue : value;
            // Find the general default Voice of the entire document...
            XmlNode? voiceNode = SSMLDocument.SelectSingleNode("//speak:voice", nsMgr);
            value = voiceNode?.Attributes["name"]?.Value;
            nameValue = string.IsNullOrEmpty(value) ? nameValue : value; // Select the <voice> element and get its "name" attribute value. Need to extract name value from < voice name = "en-US-JennyNeural" >
            // ... and its the Locale name and Display name
            foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice"))
            {
                if (node.SelectSingleNode("ShortName").InnerText == nameValue)
                {
                    localeName = node.SelectSingleNode("LocaleName").InnerText;
                    DisplayName = node.SelectSingleNode("DisplayName").InnerText;
                }
            }
            // Acquire the various attributes from Prosody such as... 
            XmlNode? prosodyNode = SSMLDocument.SelectSingleNode("//speak:prosody", nsMgr);
            //  ... the voice style
            styleSSML = prosodyNode?.FirstChild?.Attributes["style"]?.Value ?? "calm"; // if it cannot find the attribute, it returns the default "calm" string
            if (comboBox3.FindStringExact(styleSSML) == -1) { styleSSML = "calm"; }// if the style is not an option withinn the combo box, it sets it to the default "calm" string
            //  ... the rate
            rate = rateSSML = prosodyNode?.Attributes["rate"]?.Value ?? "default";
            //  ... the pitch 
            //pitch = pitchSSML = prosodyNode?.Attributes["pitch"]?.Value ?? "default";
            pitch = prosodyNode?.Attributes["pitch"]?.Value ?? "default";
            //  ... the volume

            if (prosodyNode.Attributes["volume"] is { Value: string _value })
            {
                volumeSSML = MyRegex().Replace(_value, "");
                if (int.TryParse(volumeSSML, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int parsedVolume))
                { volume = Math.Clamp(parsedVolume, 0, 100); }
            }
            #endregion

            #region set the u.i. and config elements to the parsed values (which will be the default ones on whatever has failed)

            config.SpeechSynthesisVoiceName = nameValue;
            config.SpeechSynthesisLanguage = langValue; //  Set the Speech Language to whatever is first on the xml file, which is the general one of the entire file

            comboBox1.SelectedItem = localeName;
            comboBox2.SelectedItem = DisplayName;
            comboBox3.SelectedItem = styleSSML;

            if (comboBox5.Items.Contains(pitch))
            {
                comboBox5.SelectedItem = pitch;
                label2.Text = "Pitch : " + pitch;
            }
            else if (pitch.Contains("Hz") && Int32.TryParse(MyRegex().Replace(pitch, string.Empty), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int pitchInt))
            {
                vScrollBar2.Value = Math.Clamp(pitchInt, -200, 200);
                comboBox5.SelectedItem = null;
                label2.Text = "Pitch = " + pitchInt.ToString();
            }
            else
            {
                pitch = "default";
                comboBox5.SelectedItem = null;
                label2.Text = "Pitch : " + pitch;
            }

            if (comboBox4.Items.Contains(rate))
            {
                comboBox4.SelectedItem = rate;
                label3.Text = "Rate : " + rate;
            }
            else if (rate.Contains('%') && Int32.TryParse(MyRegex().Replace(rate, string.Empty), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int rateInt))
            {
                vScrollBar1.Value = Math.Clamp(rateInt, -50, 50);
                label2.Text = "Rate = " + rateInt.ToString();
                comboBox4.SelectedItem = null;
            }
            else
            {
                rate = "default";
                label3.Text = "Rate : " + rate;
                comboBox4.SelectedItem = null;
            }

            vScrollBar3.Value = volume;
            #endregion
        }
        /// <summary> Check if we have loaded a file and have the file name and update button accordingly visible.</summary>
        private void label1_TextChanged(object sender, EventArgs e)
        { button2.Enabled = !string.IsNullOrEmpty(label1.Text); }
        /// <summary> Clears the Textbox and loaded file.</summary>
        private void button1_Click(object sender, EventArgs e)
        {
            locationLoadedFile = string.Empty;
            textBox1.Clear();
            this.label1.Text = string.Empty;
            this.label1.Visible = false;
        }
        /// <summary> The Mute button.</summary>
        private async void button5_Click(object sender, EventArgs e) { soundPause(); }
        /// <summary> Stop the sound if it is already playing.</summary>
        public static void soundPause() { synth?.StopSpeakingAsync(); }

        #region A string in XML format that is to be used, should the app not be able to download the Voices file
        readonly string VoicesBasic = @"<Voices>  
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (el-GR, AthinaNeural)</Name>
                                    <DisplayName>Athina</DisplayName>
                                    <LocalName>Αθηνά</LocalName>
                                    <ShortName>el-GR-AthinaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>el-GR</Locale>
                                    <LocaleName>Greek (Greece)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (el-GR, NestorasNeural)</Name>
                                    <DisplayName>Nestoras</DisplayName>
                                    <LocalName>Νέστορας</LocalName>
                                    <ShortName>el-GR-NestorasNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>el-GR</Locale>
                                    <LocaleName>Greek (Greece)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JennyNeural)</Name>
                                    <DisplayName>Jenny</DisplayName>
                                    <LocalName>Jenny</LocalName>
                                    <ShortName>en-US-JennyNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JennyMultilingualNeural)</Name>
                                    <DisplayName>Jenny Multilingual</DisplayName>
                                    <LocalName>Jenny Multilingual</LocalName>
                                    <ShortName>en-US-JennyMultilingualNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, GuyNeural)</Name>
                                    <DisplayName>Guy</DisplayName>
                                    <LocalName>Guy</LocalName>
                                    <ShortName>en-US-GuyNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sk-SK, LukasNeural)</Name>
                                    <DisplayName>Lukas</DisplayName>
                                    <LocalName>Lukáš</LocalName>
                                    <ShortName>sk-SK-LukasNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>sk-SK</Locale>
                                    <LocaleName>Slovak (Slovakia)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sk-SK, ViktoriaNeural)</Name>
                                    <DisplayName>Viktoria</DisplayName>
                                    <LocalName>Viktória</LocalName>
                                    <ShortName>sk-SK-ViktoriaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>sk-SK</Locale>
                                    <LocaleName>Slovak (Slovakia)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sl-SI, PetraNeural)</Name>
                                    <DisplayName>Petra</DisplayName>
                                    <LocalName>Petra</LocalName>
                                    <ShortName>sl-SI-PetraNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>sl-SI</Locale>
                                    <LocaleName>Slovenian (Slovenia)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sl-SI, RokNeural)</Name>
                                    <DisplayName>Rok</DisplayName>
                                    <LocalName>Rok</LocalName>
                                    <ShortName>sl-SI-RokNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>sl-SI</Locale>
                                    <LocaleName>Slovenian (Slovenia)</LocaleName>
                                  </Voice>
                                </Voices>";
        #endregion


        private void cmbBx_SelectSavedSoundFormat_SelectedIndexChanged(object sender, EventArgs e)
        { formatOutputSound = cmbBx_SelectSavedSoundFormat?.SelectedItem?.ToString() ?? "None"; }

        #region the vertical ScrollBar's annotations ~ could make it abstract for horizontal scrollbar as well
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            int annotationWidth = SystemInformation.VerticalScrollBarWidth;
            int annotationHeight = (SystemInformation.VerticalScrollBarThumbHeight / 10);
            int barHeight = SystemInformation.VerticalScrollBarThumbHeight;
            System.Windows.Forms.VScrollBar? vScrollBar = null;
            foreach (Control c in panel.Controls)
            {
                if (c is VScrollBar)
                {
                    vScrollBar = (System.Windows.Forms.VScrollBar)c;
                    break;
                }
            }
            int annotationX = vScrollBar.Right;
            int ceiling = vScrollBar.Top + vScrollBar.Margin.Top + barHeight + barHeight / 2;  //  the highest point, within the control, from which the annontations are drawn - will be the 1st one as well             
            int floor = vScrollBar.Bottom - vScrollBar.Margin.Bottom - barHeight - barHeight / 2;
            int stepMath = 10;// the mathematical step between the annontations
            int stepGraphic = (floor - ceiling) / stepMath; // the graphical step between the annontations
            int annotationYOffset = 5; // A small offset so that the lines are always drawn at the middle of the Thumb.
            int stepAnontations = Math.Abs(vScrollBar.Maximum - vScrollBar.Minimum) / stepMath; // this nis how much will be added in the annontations, effectively showcasing the scrollbar's value at their point
            int i = 0;  //  this is our step. There will be 10-increment steps
            int value = vScrollBar.Minimum;
            for (int annotationY = ceiling; annotationY <= floor; annotationY += stepGraphic) //  ceiling is actually a small number
            {
                if (value != 0)
                {
                    e.Graphics.FillRectangle(Brushes.Black, new Rectangle(annotationX, annotationY, annotationWidth, annotationHeight));

                    e.Graphics.DrawString(value.ToString("+#;-#"), Font, Brushes.Black, new Point(annotationX + annotationWidth + annotationYOffset, annotationY - barHeight / 2));  //  Would rather draw the strings of the scrollbar's actual value
                }
                i += stepMath;
                value += stepAnontations;
            }
            e.Graphics.FillRectangle(Brushes.Red, new Rectangle(annotationX, vScrollBar.Height / 2, annotationWidth * 2, annotationHeight));
            e.Graphics.DrawString("0", Font, Brushes.Black, new Point(annotationX + annotationWidth * 2 + annotationYOffset / 2, ((vScrollBar.Height / 2) - barHeight / 2)));
        }
        #endregion

        /// <summary> The app's Quit button. </summary>
        private void button3_Click(object sender, EventArgs e) { Application.Exit(); }
        /// <summary> Draw a light blue rectangle as the text file's title background. </summary>
        private void label1_Paint(object sender, PaintEventArgs e)
        {
            using LinearGradientBrush brush = new(ClientRectangle, Color.LightBlue, Color.White, 0F);
            e.Graphics.FillRectangle(brush, ClientRectangle);
            TextRenderer.DrawText(e.Graphics, label1.Text, label1.Font, label1.ClientRectangle, label1.ForeColor, TextFormatFlags.Default);
        }

        /// <summary> This is in order to make sure the panels are redrawn properly. Invalidate() any other control that is drawn uniquely. </summary>
        public void ReDrawEverything()
        {
            foreach (Control control in Controls)
            {
                panel1.Invalidate();
                panel2.Invalidate();
                control.Invalidate();
                label1.Invalidate();
            }
        }
        private void Form1_Resize(object sender, EventArgs e) { ReDrawEverything(); }
        private void Form1_Paint(object sender, PaintEventArgs e) { /*ReDrawEverything();*/ }

        /// <summary> Remove any extra unit from the string, keeping solely the number </summary>
        [GeneratedRegex("[^0-9-+]")]
        private static partial Regex MyRegex();
    }
}