using System.Xml;
using Verbalize.Core;

namespace _Verbalize
{
    internal class Handler_File
    {
        /// <summary>  The path of the loaded file.</summary>
        private static string locationLoadedFile = string.Empty;

        /// <summary> The path of the loaded file, or empty when none is loaded; settable so Undo can restore it. </summary>
        public static string LoadedFilePath
        {
            get => locationLoadedFile;
            set => locationLoadedFile = value;
        }

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
        /// <returns>True if a file was loaded; false if the user cancelled.</returns>
        public static bool Load_Text(Label label_FileName, Form activeForm, string applicationBrandName, Label label_FileName_in_menustrip, System.Windows.Forms.TextBox mainSingleTextBox)
        {
            //  Ask the user to pick a file, and stop if they cancel.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() != DialogResult.OK) { return false; }
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
                    fileContents = SsmlTextExtractor.GetSpeakableText(SSMLDocument, Environment.NewLine);
                    Form1.LoadXMLtoApp(SSMLDocument);
                }
                //  If the file is not XML, take its whole contents as plain text instead.
                catch (XmlException)
                { fileContents = File.ReadAllText(locationLoadedFile); }

                //  Put the text in the main text box.
                mainSingleTextBox.Text = fileContents;
            }

            //  Report that a file was loaded.
            return true;
        }

        /// <summary> Lets the user pick an SSML or plain text file, then saves its speech as a sound file next to it. </summary>
        public static void CreateAudioFileFromTextFile()
        {
            //  Ask the user to choose a sound format first if none is chosen, since nothing would be saved.
            if (!IsSoundFormatChosen()) { return; }

            //  Ask the user to pick an SSML or text file, and stop if they cancel.
            OpenFileDialog openFileDialog1 = new()
            {
                Filter = "XML Files (*.xml)|*.xml|Text Files (*.txt)|*.txt",
                Title = "Select a file to be converted into sound. Chose either a .xml or a .txt file."
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //  Turn the file into speech in the background, telling the user if it fails.
                string pathFileSelected = openFileDialog1.FileName;
                string formatOutputSound = Form1.GetOutputFormatFromComboBox(Form1.comboBox_SoundTypes);
                string outputFile = Path.ChangeExtension(pathFileSelected, "." + formatOutputSound);
                _ = RunAndReportAsync(() => Handler_AudioSynthesis.SynthesizeAudioAsync(pathFileSelected, formatOutputSound, false), outputFile);
            }
        }

        /// <summary> Lets the user choose where to save, then saves the main text box's speech as a sound file. </summary>
        public static void CreateAudioFileFromTextBox()
        {
            //  Go ahead only if a sound format is chosen, otherwise ask the user to choose one.
            string formatOutputSound = Form1.GetOutputFormatFromComboBox(Form1.comboBox_SoundTypes);
            if (IsSoundFormatChosen())
            {
                //  Ask the user where to save the sound, and stop if they cancel.
                SaveFileDialog saveFileDialog1 = new() { Filter = "Sound|*." + formatOutputSound, Title = "Save the spoken text as a sound file in your disk." };
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //  Turn the text box's text into speech in the background, telling the user if it fails.
                    string text = Form1.textBox_Main_Single.Text;
                    XmlDocument SSMLDocument = Handler_Data.CreateSSML(text);
                    string pathFileSelected = saveFileDialog1.FileName;
                    string outputFile = Path.ChangeExtension(pathFileSelected, "." + formatOutputSound);
                    _ = RunAndReportAsync(() => Handler_AudioSynthesis.SynthesizeAudioAsyncFromText(SSMLDocument, pathFileSelected, formatOutputSound, false), outputFile);
                }
            }
        }

        /// <summary> Returns true if a sound format (mp3 or wav) is chosen, and otherwise asks the user to choose one. </summary>
        /// <returns>True if a sound format is chosen.</returns>
        private static bool IsSoundFormatChosen()
        {
            //  A format is chosen unless the list says "None" or nothing.
            string formatOutputSound = Form1.GetOutputFormatFromComboBox(Form1.comboBox_SoundTypes);
            bool isChosen = formatOutputSound != "None";

            //  Ask the user to choose one if not.
            if (!isChosen) { Form1.ShowUserMessage(UserMessages.ChooseSoundFormat()); }
            return isChosen;
        }

        /// <summary> Runs a sound-file task, then tells the user where the file was saved or why it could not be created. </summary>
        /// <param name="createSoundFile">The task that creates the sound file.</param>
        /// <param name="outputFile">Where the sound file will be saved.</param>
        /// <returns>A task that finishes once the user has been told how it went.</returns>
        private static async Task RunAndReportAsync(Func<Task> createSoundFile, string outputFile)
        {
            //  Show the saving in the status bar, run the task, and say where the file was saved.
            Form1.SetActivity(AppActivity.SavingSound);
            try
            {
                await createSoundFile();
                Form1.ShowUserMessage(UserMessages.SoundFileSaved(outputFile));
            }
            //  Show the reason in the message bar if anything goes wrong.
            catch (Exception exception)
            {
                Form1.ShowUserMessage(UserMessages.SoundFileFailed(exception.Message));
            }
            //  Clear the saving from the status bar however it ended.
            finally
            {
                Form1.EndActivity(AppActivity.SavingSound);
            }
        }

    }
}
