using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace _Verbalize
{
    internal class FileHandling
    {
        /// <summary>  The path of the loaded file.</summary>
        private static string locationLoadedFile = string.Empty;

        /// <summary>  The app's Resources folder from which it will draw some info.</summary>
        readonly public static string folderResources = Path.Combine(Environment.CurrentDirectory, @"Resources\");
        /// <summary>  Change this to the desired file name and extension.</summary>
        public static string voicesSSMLFileName = "Voices";
        /// <summary>  The file in which we store the downloaded Voices list.</summary>
        public static string locationFileResponse = Path.Combine(folderResources, voicesSSMLFileName);
        /// <summary>  Backup voices file. Change this to the desired file name and extension.</summary>
        public static string voicesSSMLFileNameBackup = "Voices_[orig].xml";
        /// <summary>  Backup voices file. The file in which we store them.</summary>
        public static string locationFileResponseBackup = Path.Combine(folderResources, voicesSSMLFileNameBackup);
        public static void SaveText(string outputTextFormat, TextBox _textBox)
        {
            if (outputTextFormat == "xml")
            {
                SaveFileDialog saveFileDialog1 = new() { Filter = "XML|*.xml", Title = "Save the text box as a .xml file compliant to the SSML type." };
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string pathFileSelected = saveFileDialog1.FileName;
                    string text = _textBox.Text;
                    XmlDocument SSMLDocument = DataHandling.CreateSSML(text);
                    SSMLDocument.Save(pathFileSelected);  // Save the XML document to a file
                    //formatOutputSound = SetOutputSoundFormat();
                    //_ = SynthesizeAudioAsync(pathFileSelected, formatOutputSound, false);
                    locationLoadedFile = pathFileSelected;
                    //Form1.label1.Text = Path.GetFileNameWithoutExtension(pathFileSelected); // Show the loaded file's name                
                }
            }
            else if (outputTextFormat == "txt")
            {
                SaveFileDialog saveFileDialog1 = new() { Filter = "Text|*.txt", Title = "Save the text box as a plain .txt file." };
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string pathFileSelected = saveFileDialog1.FileName;
                    string text = _textBox.Text;
                    File.WriteAllText(pathFileSelected, text);
                    //XmlDocument SSMLDocument = CreateSSML(text);
                    //SSMLDocument.Save(pathFileSelected);  // Save the XML document to a file
                    //formatOutputSound = SetOutputSoundFormat();
                    //_ = SynthesizeAudioAsync(pathFileSelected, formatOutputSound, false);
                    locationLoadedFile = pathFileSelected;
                    //label1.Text = Path.GetFileNameWithoutExtension(pathFileSelected); // Show the loaded file's name                
                }
            }
        }

        public static void Load_Text(Label label_FileName, Form activeForm, string applicationBrandName, Label label_FileName_in_menustrip, TextBox mainSingleTextBox)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                locationLoadedFile = openFileDialog1.FileName;
                // make it into an event so that it gets automatically changed
                label_FileName.Text = Path.GetFileName(locationLoadedFile); // Show the loaded file's name                
                //label1.Font = new Font(Label.DefaultFont, FontStyle.Bold);
                activeForm.Text = applicationBrandName + " : " + label_FileName.Text /*+ Path.GetFileNameWithoutExtension(locationLoadedFile) + Path.GetExtension(locationLoadedFile)*/;
                
                label_FileName_in_menustrip.Visible = true;
                label_FileName.Visible = true;

                label_FileName_in_menustrip.Text = label_FileName.Text;
                string fileContents = string.Empty;
                //  Search further for advantages on using a rich text box instead. So far none found.
                #region Parse the selected file's contents and save its speakable text to the textbox ~ it will acquire only the inner texts, should the selected file have an xml format.
                XmlDocument SSMLDocument = new();
                try
                {
                    SSMLDocument.Load(locationLoadedFile);
                    XmlNodeList? nodes = SSMLDocument?.SelectNodes("//text()[normalize-space()]");
                    if (nodes?.Count > 0) { foreach (XmlNode node in nodes) { fileContents = node.InnerText; } }  // might need to put append instead of =, in order to support various voices within the text
                    if (SSMLDocument != null) { Form1.LoadXMLtoApp(SSMLDocument); }
                }
                catch (XmlException)
                { fileContents = File.ReadAllText(locationLoadedFile); }  // File.ReadAllText locks the file
                mainSingleTextBox.Text = fileContents;
                #endregion               
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
                string formatOutputSound = Form1.SetOutputSoundFormat();
                _ = AudioSynthesis.SynthesizeAudioAsync(pathFileSelected, formatOutputSound, false);  // "_= " is for discarding the result afterwards. Practically suppresses the warning.
            }
        }
        public static void CreateAudioFileFromTextBox()
        {
            string formatOutputSound = Form1.SetOutputSoundFormat();

            if (formatOutputSound != null && formatOutputSound != "None")
            {
                SaveFileDialog saveFileDialog1 = new() { Filter = "Sound|*." + formatOutputSound, Title = "Save the spoken text as a sound file in your disk." };
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string text = Form1.textBox_Main_Single.Text;
                    XmlDocument SSMLDocument = DataHandling.CreateSSML(text);
                    string pathFileSelected = saveFileDialog1.FileName;
                    _ = AudioSynthesis.SynthesizeAudioAsyncFromText(SSMLDocument, pathFileSelected, formatOutputSound, false);
                }
            }
        }

    }
}
