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

    public partial class Form1 : Form
    {
        //private const string Key = "4b3dc697810e47fc845f076f446a62da";
        //private const string Location = "westeurope"; // Azure Speech Service Location
        //static string speechKey = Environment.GetEnvironmentVariable("4b3dc697810e47fc845f076f446a62da");
        //static string speechRegion = Environment.GetEnvironmentVariable("westeurope");
        public static SpeechConfig config = SpeechConfig.FromSubscription("120f1e685b4244d8b1260b5bbc28f9ee", "westeurope");

        public Form1()
        {
            InitializeComponent();
            string speechRegion = "en-US";
            string voice = "GuyNeural";
            string SpeechSynthesisVoiceName = speechRegion + "-" + voice;
            float pitch = 0.00f ;
            float rate = 0.00f;
            int volume = 1;
            config.SpeechSynthesisVoiceName = SpeechSynthesisVoiceName;
            config.SpeechRecognitionLanguage = "en";
            //config.SetProperty();
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
            //var config = SpeechConfig.FromSubscription("120f1e685b4244d8b1260b5bbc28f9ee", "westeurope");
            
            SpeechSynthesizer synthesizer;
            
            //if (bSpeak) { synthesizer = new SpeechSynthesizer(config); }
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
        }

    }
}