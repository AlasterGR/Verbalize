namespace _2022TextToSpeech
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.CognitiveServices.Speech;
    using Microsoft.CognitiveServices.Speech.Audio;

    public partial class Form1 : Form
    {
        private const string Key = "8ee4528192934fec97d9ac6b03685047";
        private const string Location = "westeurope"; // Azure Speech Service Location
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.label1.Visible = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {               
                this.label1.Text = openFileDialog1.FileName;
                this.label1.Visible = true;
            }

            string textfile = this.label1.Text;
            string text = System.IO.File.ReadAllText(textfile);
            if (textfile.Contains(".txt"))
            {
                Task task = SynthesizeAudioAsyncText(text, textfile.Replace(".txt", ".wav"));
            }
            else if (textfile.Contains(".xml"))
            {
                Task task = SynthesizeAudioAsyncXML(text, textfile.Replace(".xml", ".wav"));
            }
        }         

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        static async Task SynthesizeAudioAsyncText(string txt, string soundfile)
        {
            var config = SpeechConfig.FromSubscription("8ee4528192934fec97d9ac6b03685047", "westeurope");           
            config.SpeechSynthesisLanguage = "el-GR";
            config.SpeechSynthesisVoiceName = "el-GR-AthinaNeural";

            //using AudioConfig audioConfig = AudioConfig.FromWavFileOutput(soundfile);           
            //using SpeechSynthesizer synthesizer = new SpeechSynthesizer(config, audioConfig);

            using SpeechSynthesizer synthesizer = new SpeechSynthesizer(config);

            await synthesizer.SpeakTextAsync(txt);           
            MessageBox.Show("The text was read");
        }

        static async Task SynthesizeAudioAsyncXML(string txt, string soundfile)
        {
            var config = SpeechConfig.FromSubscription("8ee4528192934fec97d9ac6b03685047", "westeurope");
            config.SpeechSynthesisLanguage = "el-GR";
            config.SpeechSynthesisVoiceName = "el-GR-AthinaNeural";

            using AudioConfig audioConfig = AudioConfig.FromWavFileOutput(soundfile);
            using SpeechSynthesizer synthesizer = new SpeechSynthesizer(config, audioConfig);

            //using SpeechSynthesizer synthesizer = new SpeechSynthesizer(config);
          
            await synthesizer.SpeakSsmlAsync(txt);
            MessageBox.Show("The xml was read");
        }




    }
}