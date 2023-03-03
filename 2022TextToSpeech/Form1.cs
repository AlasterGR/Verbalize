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
    using System.Security.Cryptography.X509Certificates;
    using static System.Net.Mime.MediaTypeNames;
    using static System.Windows.Forms.LinkLabel;

    public partial class Form1 : Form
    {
        //private const string Key = "4b3dc697810e47fc845f076f446a62da";
        //private const string Location = "westeurope"; // Azure Speech Service Location
        //static string speechKey = Environment.GetEnvironmentVariable("4b3dc697810e47fc845f076f446a62da");
        //static string speechRegion = Environment.GetEnvironmentVariable("westeurope");
        public static SpeechConfig config = SpeechConfig.FromSubscription("120f1e685b4244d8b1260b5bbc28f9ee", "westeurope");
        public static string speechRegion = "en-US";
        public static string voice = "JennyNeural";
        public static string SpeechSynthesisVoiceName = speechRegion + "-" + voice;
        public static float pitch = 0.00f;
        public static float rate = 0.00f;
        public static float volume = 0.0f; //possibly won't work in this version

        public Form1()
        {
            InitializeComponent();
            SpeechSynthesisVoiceName = speechRegion + "-" + voice;
            config.SpeechSynthesisVoiceName = SpeechSynthesisVoiceName;
            config.SpeechRecognitionLanguage = "en";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.label1.Visible = false;
            this.checkBox1.Checked = false;
            this.checkBox2.Checked = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string textfile = openFileDialog1.FileName;
                this.label1.Text = textfile;
                this.label1.Visible = true;
                string fileContents = File.ReadAllText(textfile);
                this.textBox1.Text = fileContents;
                //  Search further for advantages on using a rich text box instead. So far none found.
                #region Parse the selected file's contents and save its speakable text to the textbox ~ it will acquire only the inner texts shoud the selcted file have an xml format.
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

                PlaySound();
            }
            
        }
        private void button1_Click_1(object sender, EventArgs e)
        {         
            PlaySound();
        }

        private void PlaySound()
        {
            string textfile = this.label1.Text;
            string text = System.IO.File.ReadAllText(textfile);

            #region descontructing selected file's path, name and type info in order to build abstraction
            string nameFile = Path.GetFileNameWithoutExtension(textfile);
            string typeFile = Path.GetExtension(textfile);
            string typeFileSaved = ".wav"; // need to add option for choosing file type and sound option            
            string pathFileSelected = Path.GetDirectoryName(textfile); // I use Path.GetDirectoryName instead of FileInfo because it directly gets the exact path and doesn�t construct any large objects
            string pathFileSaved = Path.Combine(pathFileSelected, "Recorded");
            string fileSound = Path.Combine(pathFileSaved, nameFile + typeFileSaved);
            #endregion

            bool bSave = this.checkBox1.Checked;
            bool bSpeak = this.checkBox1.Checked;

            Task task = SynthesizeAudioAsync(text, fileSound, bSave, typeFile, bSpeak);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
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
            MessageBox.Show("The file was read");

            // Note : SpeechSynthesizer(speechConfig, null) gets a result as an in-memory stream
        }

        static void CreateXML(string text, string pathFileSelected)
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
            lang.Value = speechRegion;
            speak.SetAttributeNode(lang);
            XmlAttribute onlangfailure = SSMLDocument.CreateAttribute("onlangfailure");
            onlangfailure.Value = "ignoretext ";
            speak.SetAttributeNode(onlangfailure);            
            XmlElement voice = SSMLDocument.CreateElement("voice");
            voice.SetAttribute("name", SpeechSynthesisVoiceName);
            XmlElement prosody = SSMLDocument.CreateElement("prosody");
            prosody.SetAttribute("rate",  (rate*100).ToString() +"%");
            prosody.SetAttribute("pitch", (pitch*100).ToString() + "%"); //  Hz or % ?
            //prosody.SetAttribute("volume", (volume).ToString("+#.#;-#.#;-0") + "dB"); //Might not work on this version of SSML
            XmlElement express = SSMLDocument.CreateElement("mstts", "express-as", "http://www.w3.org/2001/mstts");
            express.SetAttribute("style", "calm");
            #endregion
            #region Appending the XML elements to their parents
            prosody.AppendChild(express);
            voice.AppendChild(prosody);
            speak.AppendChild(voice);
            #endregion
            //  Entering the .txt file's text as the xml's main -inner- text
            express.InnerText = text;
            // Save the XML document to a file           
            SSMLDocument.Save(Path.Combine(pathFileSelected, "output.xml"));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string textfile = openFileDialog1.FileName;
                string text = System.IO.File.ReadAllText(textfile);
                string pathFileSelected = Path.GetDirectoryName(textfile);
                CreateXML(text, pathFileSelected);
            }
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
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text|*.txt";
            saveFileDialog1.Title = "Save the text box as a .txt File";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
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
        }
    }
}