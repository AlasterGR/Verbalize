using System.Xml;
using Verbalize.Core;

namespace _Verbalize
{
    internal class Handler_File
    {
        /// <summary>  The path of the loaded file.</summary>
        private static string locationLoadedFile = string.Empty;

        /// <summary>  The app's Resources folder from which it will draw some info.</summary>
        readonly public static string folderResources = Path.Combine(Environment.CurrentDirectory, @"Resources\");
        /// <summary>  Change this to the desired file name and extension.</summary>
        public static string voicesSSMLFileName = "Voices";
        /// <summary>  The file in which we store the downloaded Voices list.</summary>
        public static string locationOfVoicesFile = Path.Combine(folderResources, voicesSSMLFileName);
        /// <summary>  Backup voices file. Change this to the desired file name and extension.</summary>
        public static string locationOfVoicesFileBackup = "Voices_[orig].xml";
        /// <summary>  Backup voices file. The file in which we store them.</summary>
        public static string locationFileResponseBackup = Path.Combine(folderResources, locationOfVoicesFileBackup);
        public static void Initialize()
        {
            if (!Directory.Exists(folderResources))
            {
                Directory.CreateDirectory(folderResources);
            }
        }
        public static void SaveText(string outputTextFormat, string _textBoxText)
        {
            if (outputTextFormat == "xml")
            {
                SaveFileDialog saveFileDialog1 = new() { Filter = "XML|*.xml", Title = "Save the text box as a .xml file compliant to the SSML type." };
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string pathFileSelected = saveFileDialog1.FileName;

                    XmlDocument SSMLDocument = Handler_Data.CreateSSML(_textBoxText);
                    SSMLDocument.Save(pathFileSelected);  // Save the XML document to a file
                    locationLoadedFile = pathFileSelected;
                }
            }
            else if (outputTextFormat == "txt")
            {
                SaveFileDialog saveFileDialog1 = new() { Filter = "Text|*.txt", Title = "Save the text box as a plain .txt file." };
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string pathFileSelected = saveFileDialog1.FileName;
                    File.WriteAllText(pathFileSelected, _textBoxText);
                    locationLoadedFile = pathFileSelected;
                }
            }
        }

        /// <summary> Lets the user pick a file, then shows its name and puts its speakable text in the main text box. If the file is SSML, its voice settings are loaded into the app too. </summary>
        /// <param name="label_FileName">The label that shows the loaded file's name.</param>
        /// <param name="activeForm">The app's window, whose title will show the file's name.</param>
        /// <param name="applicationBrandName">The app's name, shown before the file's name in the window title.</param>
        /// <param name="label_FileName_in_menustrip">The label in the menu bar that also shows the file's name.</param>
        /// <param name="mainSingleTextBox">The main text box that receives the file's text.</param>
        public static void Load_Text(Label label_FileName, Form activeForm, string applicationBrandName, Label label_FileName_in_menustrip, System.Windows.Forms.TextBox mainSingleTextBox)
        {
            //  Ask the user to pick a file, and stop if they cancel.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //  Show the chosen file's name in the label and in the window title.
                locationLoadedFile = openFileDialog1.FileName;
                label_FileName.Text = Path.GetFileName(locationLoadedFile);
                activeForm.Text = applicationBrandName + " : " + label_FileName.Text;

                //  Show the file's name in the menu bar as well.
                label_FileName_in_menustrip.Visible = true;
                label_FileName.Visible = true;
                label_FileName_in_menustrip.Text = label_FileName.Text;

                //  Read the file as XML, taking its speakable text and loading its voice settings into the app.
                string fileContents = string.Empty;
                XmlDocument SSMLDocument = new();
                try
                {
                    SSMLDocument.Load(locationLoadedFile);
                    fileContents = SsmlTextExtractor.GetSpeakableText(SSMLDocument);
                    Form1.LoadXMLtoApp(SSMLDocument);
                }
                //  If the file is not XML, take its whole contents as plain text instead.
                catch (XmlException)
                { fileContents = File.ReadAllText(locationLoadedFile); }

                //  Put the text in the main text box.
                mainSingleTextBox.Text = fileContents;
            }
        }

        public static void CreateAudioFileFromTextFile()
        {

            OpenFileDialog openFileDialog1 = new()
            {
                Filter = "XML Files (*.xml)|*.xml|Text Files (*.txt)|*.txt",
                Title = "Select a file to be converted into sound. Chose either a .xml or a .txt file."
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string pathFileSelected = openFileDialog1.FileName;
                string formatOutputSound = Form1.GetOutputFormatFromComboBox(Form1.comboBox_SoundTypes);
                _ = Handler_AudioSynthesis.SynthesizeAudioAsync(pathFileSelected, formatOutputSound, false);  // "_= " is for discarding the result afterwards. Practically suppresses the warning.
            }
        }
        public static void CreateAudioFileFromTextBox()
        {
            string formatOutputSound = Form1.GetOutputFormatFromComboBox(Form1.comboBox_SoundTypes);

            if (formatOutputSound != null && formatOutputSound != "None")
            {
                SaveFileDialog saveFileDialog1 = new() { Filter = "Sound|*." + formatOutputSound, Title = "Save the spoken text as a sound file in your disk." };
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string text = Form1.textBox_Main_Single.Text;
                    XmlDocument SSMLDocument = Handler_Data.CreateSSML(text);
                    string pathFileSelected = saveFileDialog1.FileName;
                    _ = Handler_AudioSynthesis.SynthesizeAudioAsyncFromText(SSMLDocument, pathFileSelected, formatOutputSound, false);
                }
            }
        }

    }
}
