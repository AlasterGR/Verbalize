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

    public partial class Form1 : Form
    {
        //private const string Key = "4b3dc697810e47fc845f076f446a62da";
        //private const string Location = "westeurope"; // Azure Speech Service Location
        //static string speechKey = Environment.GetEnvironmentVariable("4b3dc697810e47fc845f076f446a62da");
        //static string regionService = Environment.GetEnvironmentVariable("westeurope");
        public static string ServerLocation = "westeurope";
        public static SpeechConfig config = SpeechConfig.FromSubscription("120f1e685b4244d8b1260b5bbc28f9ee", ServerLocation);  //This is the single most valuable object of the app, as it holds all the important properties for the speech synthesis
        public static int pitch = 0;
        public static int rate = 0;
        public static string style = "calm";
        public static float volume = 0.0f; //possibly won't work in this version
        public static string folderResources = Path.Combine(Environment.CurrentDirectory, @"Resources\");
        public string selectedLocale;
        public static XmlDocument VoicesXML = new XmlDocument();
        public static string voicesSSMLFileName = "Voices"; // Change this to the desired file name and extension
        public static string locationFileResponse = Path.Combine(folderResources, voicesSSMLFileName);
        public static string voicesSSMLFileNameBackup = "Voices_[orig].xml"; // Change this to the desired file name and extension
        public static string locationFileResponseBackup = Path.Combine(folderResources, voicesSSMLFileNameBackup);
        public static string shortName = "";
        public static string locationLoadedFile = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.label1.Visible = false;
            this.checkBox1.Checked = false;
            this.checkBox2.Checked = true;
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
                    VoicesRetrieve();
                }
            }
            VoicesLoad();
            this.comboBox3.SelectedText = "calm";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                locationLoadedFile = openFileDialog1.FileName;
                this.label1.Text = Path.GetRelativePath(folderResources, locationLoadedFile); // Show the loaded file's location relative to Resources folder
                this.label1.Visible = true;
                string fileContents = File.ReadAllText(locationLoadedFile);
                this.textBox1.Text = fileContents;
                //  Search further for advantages on using a rich text box instead. So far none found.
                #region Parse the selected file's contents and save its speakable text to the textbox ~ it will acquire only the inner texts, should the selected file have an xml format.
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(fileContents);
                    XmlNodeList nodes = doc.SelectNodes("//text()[normalize-space()]");
                    if (nodes.Count > 0)
                    {
                        foreach (XmlNode node in nodes)
                        {
                            fileContents = node.InnerText;
                        }
                    }
                    this.textBox1.Text = fileContents;
                    
                }
                catch (XmlException ex)
                {
                    this.textBox1.Text = fileContents;
                }
                #endregion
            }
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


        private void button5_Click(object sender, EventArgs e)
        {
            ReadText();
        }

        private async void ReadText()
        {
            string textfile = this.label1.Text;
            string text = this.textBox1.Text;
            SpeechSynthesizer synthesizer;
            synthesizer = new SpeechSynthesizer(config);
            await synthesizer.SpeakTextAsync(text);
        }


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


        #region Retrieve the list of voices from speech.microsoft.com and reload the voices into the boxes
        private void button8_Click(object sender, EventArgs e)
        {
            VoicesRetrieve();
            VoicesLoad();
        }
        async void VoicesRetrieve()
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
        #region button for Populating the two voice combo boxes - Will later be done to select automaticaly from Resources\voice
        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                VoicesXML.Load(fileName); // Use Load because LoadXML command produces error
                VoicesLoad();
            }
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
                    this.label6.Text = node.SelectSingleNode("Locale").InnerText;
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
            /*  Save the file as an xml. Remove previous existing extention  */
            xmlDoc.Save(Path.Combine(folderResources, rootName));
        }
        #region This function loads the list of language elements from the SSML Voice file onto the Languages combo box

        private void VoicesLoad()
        {

            
            if (VoicesXML != null) 
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
                        label6.Text = node.SelectSingleNode("Locale").InnerText;
                    }
                    string localName = node.SelectSingleNode("LocalName").InnerText;
                    string gender = node.SelectSingleNode("Gender").InnerText;
                    comboBox1.SelectedIndex = 0;
                }
            }
        }
        #endregion

        private void vScrollBar2_ValueChanged(object sender, EventArgs e)
        {
            pitch = this.vScrollBar2.Value;
            this.label2.Text = "Pitch = " + pitch.ToString();
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            rate = this.vScrollBar1.Value;
            this.label3.Text = "Rate = " + rate.ToString();
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
            //prosody.SetAttribute("volume", (volume).ToString("+#.#;-#.#;-0") + "dB"); //Might not work on this version of SSML
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

        //  Load all the selcted file's properties onto the ui
        private void button11_Click(object sender, EventArgs e)
        {
            XmlDocument SSMLDocument = new XmlDocument();
            SSMLDocument.Load(locationLoadedFile);
            this.label6.Text = SSMLDocument.DocumentElement.GetAttribute("xml:lang");//  Set the Speech Language to whatever is first on the xml file, which is the general one of the entire file
            config.SpeechSynthesisLanguage = SSMLDocument.DocumentElement.GetAttribute("xml:lang");
            //XmlNode node = SSMLDocument.SelectSingleNode("voice");
            //string voiceName = node.InnerText;
            //config.SpeechSynthesisLanguage = SSMLDocument.SelectSingleNode("Locale").InnerText;  
            //config.SpeechSynthesisVoiceName = SSMLDocument.DocumentElement.GetAttribute("xml:lang");


            //XDocument SSMLDocument = new XDocument();
            // config.SpeechSynthesisVoiceName = SSMLDocument.
            //node = SSMLDocument.SelectSingleNode("xml:lang");
            //config.SpeechSynthesisLanguage = node.InnerText;
            //this.comboBox1.SelectedText = ;
            //this.vScrollBar1.Value = Int32.Parse(SSMLDocument.DocumentElement.GetAttribute("rate"));

        }
        //  Speak the selected text from the textbox
        private async void button12_Click(object sender, EventArgs e)
        {
            if (this.textBox1.SelectedText != "")
            {                
                string text = CreateSSML(this.textBox1.SelectedText).OuterXml;  //  We take textBox1's selected text, create a proper SSML xml file, convert it to text and will feed it to the synthesizer
                SpeechSynthesizer synthesizer = new SpeechSynthesizer(config);  //  Initialize out speech synthesizer
                await synthesizer.SpeakSsmlAsync(text);  //  Speak the text
            }
        }
    }
}