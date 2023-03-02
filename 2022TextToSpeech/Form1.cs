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
        public static float pitch = -0.10f;
        public static float rate = -0.10f;

        public Form1()
        {
            InitializeComponent();
            speechRegion = "en-US";
            voice = "JennyNeural";
            SpeechSynthesisVoiceName = speechRegion + "-" + voice;
            float pitch = -0.10f ;
            float rate = -0.10f;
            int volume = 1;
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
                this.label1.Text = openFileDialog1.FileName;
                this.label1.Visible = true;
            }
            PlaySound();

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
            string pathFileSelected = Path.GetDirectoryName(textfile); // I use Path.GetDirectoryName instead of FileInfo because it directly gets the exact path and doesn’t construct any large objects
            string pathFileSaved = Path.Combine(pathFileSelected, "Recorded");
            string fileSound = Path.Combine(pathFileSaved, nameFile + typeFileSaved);
            #endregion

            bool bSave = this.checkBox1.Checked;
            bool bSpeak = this.checkBox1.Checked;

            Task task = SynthesizeAudioAsync(text, fileSound, bSave, typeFile, bSpeak);
            /*
            if (typeFile.Equals(".txt") ) // should probably create a function to turn it into a proper xml file
            { Task task = SynthesizeAudioAsyncText(text, fileSound, bSave); }
            else if (typeFile.Equals(".xml")) { Task task = SynthesizeAudioAsyncXML(text, fileSound, bSave); } 
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        static async Task SynthesizeAudioAsyncText(string txt, string soundfile, bool bSave)
        {
            var config = SpeechConfig.FromSubscription("120f1e685b4244d8b1260b5bbc28f9ee", "westeurope");           
            //config.SpeechSynthesisLanguage = "el-GR";
            //config.SpeechSynthesisVoiceName = "el-GR-AthinaNeural";
            SpeechSynthesizer synthesizer;

            if (bSave)
            {
                using AudioConfig audioConfig = AudioConfig.FromWavFileOutput(soundfile);
                synthesizer = new SpeechSynthesizer(config, audioConfig);
            }
            else
            {
                synthesizer = new SpeechSynthesizer(config);
            }

            await synthesizer.SpeakTextAsync(txt);           
            MessageBox.Show("The text was read");
        }
        static async Task SynthesizeAudioAsyncXML(string txt, string soundfile, bool bSave)
        { 
            var config = SpeechConfig.FromSubscription("120f1e685b4244d8b1260b5bbc28f9ee", "westeurope");
            SpeechSynthesizer synthesizer;

            if (bSave)
            {
                using AudioConfig audioConfig = AudioConfig.FromWavFileOutput(soundfile);
                synthesizer = new SpeechSynthesizer(config, audioConfig);
            }
            else
            {
                synthesizer = new SpeechSynthesizer(config);
            }          
            await synthesizer.SpeakSsmlAsync(txt);
            MessageBox.Show("The xml was read");
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
            XmlDocument SSMLDocument = new XmlDocument();
            #region xml declaration is mandatory for XML 1.1
            //XmlDeclaration xmlDeclaration = SSMLDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement speak = SSMLDocument.CreateElement("speak");
            //SSMLDocument.InsertBefore(xmlDeclaration, speak);
            #endregion
            #region assigning the XML's strings and parameters and attributes on variables
            /*                        
             */            
            XmlAttribute version = SSMLDocument.CreateAttribute("version");
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
            XmlElement voice = SSMLDocument.CreateElement("voice");
            voice.SetAttribute("name", SpeechSynthesisVoiceName);
            XmlElement prosody = SSMLDocument.CreateElement("prosody");
            prosody.SetAttribute("rate",  (rate*100).ToString() +"%");
            prosody.SetAttribute("pitch", (pitch*100).ToString() + "%");
            XmlElement express = SSMLDocument.CreateElement("mstts:express-as");
            express.SetAttribute("style", "calm");

            #endregion
            express.InnerText = text;
            // Append the voice style element to the prosody element
            prosody.AppendChild(express);
            // Append the prosody element to the voice element
            voice.AppendChild(prosody);
            // Append the voice element to the speak element
            speak.AppendChild(voice);            
            // Add the <speak> element to the XML document
            SSMLDocument.AppendChild(speak);
            // Add the namespace attribute to the root element - shouldn;t have to but seems proper - never use "namespace"
            XmlNamespaceManager ns = new XmlNamespaceManager(SSMLDocument.NameTable);
            ns.AddNamespace("ssml", "http://www.w3.org/2001/10/synthesis");
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
    }
}