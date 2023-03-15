namespace _2022TextToSpeech
{
    using System;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Threading.Tasks;
    using System.Windows.Forms.VisualStyles;
    using Microsoft.CognitiveServices.Speech;
    using Microsoft.CognitiveServices.Speech.Audio;
    using System.Xml; // For constructing our xml file 
    using System.Xml.Linq; // For the queries towards MS in order to populate the languages and voices lists
    using System.Security.Cryptography.X509Certificates;
    using static System.Net.Mime.MediaTypeNames;
    using static System.Windows.Forms.LinkLabel;
    using System.Net.Http; // for the supported languages of the voice
    using System.Net.Http.Headers;  // for the supported languages of the voice
    using System.Net.Security;
    using Newtonsoft.Json; // for converting the Json files into XML ones. Possibly removable should we only load the XML file I am going to have produced
    using System.Text.Json;// for converting the Json files into XML ones. Possibly removable should we only load the XML file I am going to have produced
    using Properties;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Net.Http.Json;
    using System.Text;
    using System.Linq; // for <group by> in the comboboxes
    using System.Runtime.CompilerServices;
    using System.Configuration;
    using System.Net;
    using static System.Runtime.InteropServices.JavaScript.JSType;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public partial class Form1 : Form
    {
        //private const string Key = "4b3dc697810e47fc845f076f446a62da";
        //private const string Location = "westeurope"; // Azure Speech Service Location
        //static string speechKey = Environment.GetEnvironmentVariable("4b3dc697810e47fc845f076f446a62da");
        //static string regionService = Environment.GetEnvironmentVariable("westeurope");
        public static string ServerLocation = "westeurope";
        public static SpeechConfig config = SpeechConfig.FromSubscription("120f1e685b4244d8b1260b5bbc28f9ee", ServerLocation);  //This is the single most valuable object of the app, as it holds all the important properties for the speech synthesis
        #region The Prosody and assorted elements of speech
        // As per : https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/speech-synthesis-markup-voice. The https://www.w3.org/TR/speech-synthesis11/ is irrelevant so far.
        public static int pitch = 0;  //  Pitch is expressed in 3 ways. Here, for now, we are using just the absolute value from the range [-200, +200]
        public static int rate = 0;  // Rate is expressed in 2 ways, an absolute value (string) and a relative (as a number) one. For now, we will use it only as a number (-50% - +50%), I will incoroprate it as a string later
        public static int volume = 100; // Defaults in 100.
        public static string style = "calm";
        //  Integrate the rest of the speech elements, such as pitch contour, pitch range
        #endregion
        public static string folderResources = Path.Combine(Environment.CurrentDirectory, @"Resources\");
        public string selectedLocale;
        public static XmlDocument VoicesXML = new XmlDocument();  //  A file that needs to be used throughout the entire app. Might put it within the VoicesLoad() nethod if possible
        public static string voicesSSMLFileName = "Voices"; // Change this to the desired file name and extension
        public static string locationFileResponse = Path.Combine(folderResources, voicesSSMLFileName);
        public static string voicesSSMLFileNameBackup = "Voices_[orig].xml"; // Change this to the desired file name and extension
        public static string locationFileResponseBackup = Path.Combine(folderResources, voicesSSMLFileNameBackup);
        public static string shortName = "";
        public static string locationLoadedFile = string.Empty;
        Task<SpeechSynthesisResult> sound1;

        

        public Form1()
        {
            InitializeComponent();
            #region Load the voices file that lists the various speech voices          
            InitializeVoices();
            #endregion
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.label1.Visible = false;
            this.checkBox2.Checked = true;
            VoicesLoad();  //  Load the voices onto the combo boxes
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                locationLoadedFile = openFileDialog1.FileName;
                this.label1.Text = Path.GetFileNameWithoutExtension(locationLoadedFile); // Show the loaded file's name
                this.label1.Visible = true;
                string fileContents = string.Empty;
                //  Search further for advantages on using a rich text box instead. So far none found.
                #region Parse the selected file's contents and save its speakable text to the textbox ~ it will acquire only the inner texts, should the selected file have an xml format.
                XmlDocument SSMLDocument = new XmlDocument();
                try
                {
                    SSMLDocument.Load(locationLoadedFile);
                    XmlNodeList nodes = SSMLDocument.SelectNodes("//text()[normalize-space()]");
                    if (nodes.Count > 0)
                    {
                        foreach (XmlNode node in nodes)
                        {
                            fileContents = node.InnerText; // might need to put append instead of =, in order to support various voices within the text
                        }
                    }
                    loadXMLtoApp(SSMLDocument);
                }
                catch (XmlException ex)
                {
                    fileContents = File.ReadAllText(locationLoadedFile); ;
                }
                this.textBox1.Text = fileContents;
                #endregion               
            }
        }

        static async Task SynthesizeAudioAsync(string txt, string soundfile, bool bSave, string filetype, bool bSpeak)
        {
            SpeechSynthesizer synthesizer;
            //if (bSpeak) { synthesizer = new SpeechSynthesizer(config); }  // Option of whether to speak the text
            if (bSave)
            {
                using AudioConfig audioConfig = AudioConfig.FromWavFileOutput(soundfile);
                synthesizer = new SpeechSynthesizer(config, audioConfig);
            }
            else
            {
                synthesizer = new SpeechSynthesizer(config);
            }
            if (filetype.Equals(".txt"))
            { await synthesizer.SpeakTextAsync(txt); }
            else if (filetype.Equals(".xml"))
            { await synthesizer.SpeakSsmlAsync(txt); }
            //MessageBox.Show("The file was read");
            // Note : SpeechSynthesizer(speechConfig, null) gets a result as an in-memory stream
        }


        async void InitializeVoices()
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
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "120f1e685b4244d8b1260b5bbc28f9ee");
            string listVoicesLocationURL = "https://" + ServerLocation + ".tts.speech.microsoft.com/cognitiveservices/voices/list";
            HttpResponseMessage response = await client.GetAsync(listVoicesLocationURL);
            if (response.IsSuccessStatusCode)
            {
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
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
                List<string> uniqueLocales = new List<string>();  //  List that will contain only the unique values of Locale, for reference, to populate the combo box with unique elements only
                foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice")) //Expression "//Voice" works as well
                {
                    string locale = node.SelectSingleNode("Locale").InnerText;
                    if (!uniqueLocales.Contains(locale))
                    {
                        uniqueLocales.Add(locale);
                        string localeName = node.SelectSingleNode("LocaleName").InnerText;
                        comboBox1.Items.Add(localeName);
                    }
                    string localName = node.SelectSingleNode("LocalName").InnerText;
                    string gender = node.SelectSingleNode("Gender").InnerText;
                    comboBox1.SelectedIndex = 0;
                }
            }
            else { VoicesXML.LoadXml(VoicesBasic); VoicesLoad(); }
            this.comboBox3.SelectedItem = style;  //  Have the the default voice style preselected as default
        }
        #endregion

        #region button for Populating the two voice combo boxes from Resources\voice
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                VoicesXML.Load(Path.Combine(locationFileResponse)); // Use Load because LoadXML command produces error
            }
            catch (Exception ex)
            {
                VoicesRetrieve();
            }
            VoicesLoad();
        }
        #endregion
        #region Language combo box
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> displayNames = new List<string>();
            string selectedLocaleName = this.comboBox1.SelectedItem.ToString();  // this.comboBox1.SelectedItem.ToString() is not a string that can be handled within the next if()

            foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice"))
            {
                string localeName = node.SelectSingleNode("LocaleName").InnerText;
                if (localeName == selectedLocaleName)
                {
                    displayNames.Add(node.SelectSingleNode("DisplayName").InnerText);
                    this.label6.Text = node.SelectSingleNode("Locale").InnerText;  //  can probably delete this
                    config.SpeechSynthesisLanguage = node.SelectSingleNode("Locale").InnerText;
                }
            }
            comboBox2.DataSource = displayNames;
            comboBox2.SelectedIndex = 0;
        }
        #endregion
        #region Voice actor combo box
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDisplayName = this.comboBox2.SelectedItem.ToString();
            foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice"))
            {
                string localName = node.SelectSingleNode("DisplayName").InnerText;
                if (localName == selectedDisplayName)
                {
                    config.SpeechSynthesisVoiceName = node.SelectSingleNode("ShortName").InnerText;  //  We store who our voice actor will be
                    this.label7.Text = " - " + node.SelectSingleNode("LocalName").InnerText + " (" + node.SelectSingleNode("Gender").InnerText + ")";                    
                }
                
            }
        }
        #endregion

        private void SSML_JSONtoXMLConvert(string TextJSONFile)
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


        private void vScrollBar2_ValueChanged(object sender, EventArgs e)
        {  // This handles integers only
            pitch = this.vScrollBar2.Value;
            this.label2.Text = "Pitch = " + pitch.ToString();
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {  // This handles integers only 
            rate = this.vScrollBar1.Value;
            this.label3.Text = "Rate = " + rate.ToString();
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {  // This handles integers only 
            volume = this.hScrollBar1.Value;
            this.label11.Text = volume.ToString();
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            style = this.comboBox3.SelectedItem.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML|*.xml";
            saveFileDialog1.Title = "Save the text box as a .xml file compliant to the SSML type.";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    string pathFileSelected = saveFileDialog1.FileName;
                    string text = this.textBox1.Text;
                    XmlDocument SSMLDocument = CreateSSML(text);
                    // Save the XML document to a file         
                    SSMLDocument.Save(pathFileSelected);
                }
            }
        }
        static XmlDocument CreateSSML(string text)
        {
            #region Creation of the XML document and and its root element
            XmlDocument SSMLDocument = new XmlDocument();
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
            prosody.SetAttribute("rate", rate.ToString() + "%");
            prosody.SetAttribute("pitch", pitch.ToString() + "Hz"); //  Hz or % ?
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (locationLoadedFile == string.Empty)
            {
                //this.button2.Enabled = false;
            }
            else
            {
                string pathFileSelected = locationLoadedFile;
                string text = this.textBox1.Text;
                XmlDocument SSMLDocument = CreateSSML(text);
                // Save the XML document to a file         
                SSMLDocument.Save(pathFileSelected);
            }

        }

        //  Speak the selected text from the textbox
        private async void button12_Click(object sender, EventArgs e)
        {
            //if (sound1 != null) { sound1.stat; }
            string text = string.Empty;  // Initialize the text input for the synthesizer
            if (this.textBox1.SelectedText != "") { text = CreateSSML(this.textBox1.SelectedText).OuterXml; } //  If there is text selected within the textbox's text, feed that into the synthesizer
            else { text = CreateSSML(this.textBox1.Text).OuterXml; } //  if there is no text selected, feed the entire textbox's text into the synthesizer.
            //  Initialize out speech synthesizer
            //await synthesizer.SpeakSsmlAsync(text);  //  Speak the text

            //sound1.
            SpeechSynthesizer synthesizer = new SpeechSynthesizer(config);
            sound1 = synthesizer.SpeakSsmlAsync(text);
        }

        private void loadXMLtoApp(XmlDocument SSMLDocument) // Load all the markup of the XML onto the u.i.
        {
            #region Initialize every mark to its default values
            string styleSSML = "calm";
            string volumeSSML = "100";
            string pitchSSML = "0";
            string rateSSML = "0";
            string nameValue = "en-US-JennyNeural";
            string langValue = "en-US";

            string localeName = "English (United States)";
            string DisplayName = "Jenny";
            #endregion
            #region Parse the loaded document and acquire the desired values. Add try-catch...
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(SSMLDocument.NameTable);
            nsMgr.AddNamespace("speak", "http://www.w3.org/2001/10/synthesis");

            // Find the general default language of the entire document
            XmlNode speakNode = SSMLDocument.SelectSingleNode("//speak:speak", nsMgr);
            langValue = speakNode.Attributes["xml:lang"].Value;
            // Find the general default Voice of the entire document...
            XmlNode voiceNode = SSMLDocument.SelectSingleNode("//speak:voice", nsMgr);
            nameValue = voiceNode.Attributes["name"].Value;// Select the <voice> element and get its "name" attribute value. Need to extract name value from < voice name = "en-US-JennyNeural" >
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
            XmlNode prosodyNode = SSMLDocument.SelectSingleNode("//speak:prosody", nsMgr);
            //  ... the voice style
            styleSSML = prosodyNode.FirstChild.Attributes["style"]?.Value ?? "calm"; // if it cannot find the attribute, it returns the default "calm" string
            if (comboBox3.FindStringExact(styleSSML) == -1) { styleSSML = "calm"; }// if the style is not an option withinn the combo box, it sets it to the default "calm" string

            //  ... the rate
            try
            {
                if (prosodyNode.Attributes["rate"] != null) { rateSSML = prosodyNode.Attributes["rate"].Value; } // For some reason, this cannot be handled by this generic try-catch                
                rateSSML = Regex.Replace(rateSSML, "[^0-9-+]", "");
                rate = Int32.Parse(rateSSML, NumberStyles.AllowLeadingSign);
                if (rate > 50) { rate = 50; }  // replace with more abstract bounds
                else if (rate < -50) { rate = -50; }
            }
            catch { rateSSML = "0"; rate = 0; }

            //  ... the pitch 
            try
            {
                if (prosodyNode.Attributes["pitch"] != null) { pitchSSML = prosodyNode.Attributes["pitch"].Value; } // For some reason, this cannot be handled by this generic try-catch                
                pitchSSML = Regex.Replace(pitchSSML, "[^0-9-+]", "");
                pitch = Int32.Parse(pitchSSML, NumberStyles.AllowLeadingSign);
                if (pitch > 200) { pitch = 200; }  // replace with more abstract bounds
                else if (pitch < -200) { pitch = -200; }
            }
            catch { pitchSSML = "0"; pitch = 0; }

            //  ... the volume
            try
            {
                if (prosodyNode.Attributes["volume"] != null) { volumeSSML = prosodyNode.Attributes["volume"].Value; } // For some reason, this cannot be handled by this generic try-catch
                volumeSSML = Regex.Replace(volumeSSML, "[^0-9-+]", "");  // remove any extra unit from the string, keeping solely the number
                volume = Int32.Parse(volumeSSML, NumberStyles.AllowLeadingSign);
                if (volume > 100) { volume = 100; }  // replace with more abstract bounds
                else if (volume < 0) { volume = 0; }
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is FormatException) { volumeSSML = "80"; volume = 80; }
            #endregion
            #region set the u.i. and config elements to the parsed values (which will be the default ones fow whatever has failed)

            config.SpeechSynthesisVoiceName = nameValue;
            config.SpeechSynthesisLanguage = langValue; //  Set the Speech Language to whatever is first on the xml file, which is the general one of the entire file

            this.comboBox1.SelectedItem = localeName;
            this.comboBox2.SelectedItem = DisplayName;

            this.comboBox3.SelectedItem = styleSSML;
            this.vScrollBar1.Value = rate;  //  make a save clause to keep any value between [-50, +50], trasnforming out-of-bounds values to bounds
            this.vScrollBar2.Value = pitch;  //  make a save clause to keep any value between bounds, trasnforming out-of-bounds values to bounds
            this.hScrollBar1.Value = volume;  //  make a save clause to keep any value between [0, +100], trasnforming out-of-bounds values to bounds
            #endregion
        }

        #region deprecated code to be deleted once everything sorts out
        /* Quite Possibly unneeded - it's a button to transform a .txt file into a proper ssml .xml but we now load it first and save it afterwards
        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string text = System.IO.File.ReadAllText(openFileDialog1.FileName);
                string xmlFile = Path.GetFileNameWithoutExtension(openFileDialog1.FileName) + ".xml";
                string pathFileSelected = Path.Combine(Path.GetDirectoryName(openFileDialog1.FileName), xmlFile);
                XmlDocument SSMLDocument = CreateSSML(text);
                // Save the XML document to a file         
                SSMLDocument.Save(pathFileSelected);
            }
        }*/
        /*
        private void button6_Click(object sender, EventArgs e)
        {
            #region Saves the written text as a txt file
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text|*.txt";
            saveFileDialog1.Title = "Save the text box as a .txt File";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string pathFileSelected = saveFileDialog1.FileName;
                if (saveFileDialog1.FileName != "")
                {
                    using (StreamWriter writer = new StreamWriter(pathFileSelected))
                    {
                        writer.Write(textBox1.Text);
                    }
                }
            }
            #endregion
           
        }        
        private async void button10_Click(object sender, EventArgs e)
        {
            string text = this.textBox1.Text;
            XmlDocument SSMLDocument = CreateSSML(text);
            text = SSMLDocument.OuterXml;
            //MessageBox.Show(text);
            SpeechSynthesizer synthesizer;
            synthesizer = new SpeechSynthesizer(config);
            await synthesizer.SpeakSsmlAsync(SSMLDocument.OuterXml);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            ReadText();
        }
        private async void ReadText()
        {
            string text = this.textBox1.Text;
            SpeechSynthesizer synthesizer;
            synthesizer = new SpeechSynthesizer(config);
            await synthesizer.SpeakTextAsync(text);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            PlaySound();
        }
        private void PlaySound()
        {
            string text = System.IO.File.ReadAllText(locationLoadedFile);

            #region descontructing selected file's path, name and type info in order to build abstraction
            string nameFile = Path.GetFileNameWithoutExtension(locationLoadedFile);
            string typeFile = Path.GetExtension(locationLoadedFile);
            string typeFileSaved = ".wav"; // need to add option for choosing file type and sound option            
            string pathFileSelected = Path.GetDirectoryName(locationLoadedFile); // I use Path.GetDirectoryName instead of FileInfo because it directly gets the exact path and doesn’t construct any large objects
            string pathFileSaved = Path.Combine(pathFileSelected, "Recorded");
            string fileSound = Path.Combine(pathFileSaved, nameFile + typeFileSaved);
            #endregion

            bool bSave = this.checkBox1.Checked;
            bool bSpeak = this.checkBox1.Checked;

            Task task = SynthesizeAudioAsync(text, fileSound, bSave, typeFile, bSpeak);
        }
        */
        #endregion

        private void label1_TextChanged(object sender, EventArgs e) // Keep out save as button available only if we are making a new document
        {
            if (label1.Text != string.Empty) { this.button7.Enabled = false; this.button2.Enabled = true; }
            else { this.button7.Enabled = true; this.button2.Enabled = false; }
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            this.label1.Text = string.Empty;
            this.label1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.label1.Text = string.Empty;
            this.label1.Visible = false;
        }
        //  A string in XML format that is to be used should the app not be able to download the Voices file
        string VoicesBasic = @"<Voices>  
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
    }
}