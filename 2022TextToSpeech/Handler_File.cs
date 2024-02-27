using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        public static void Load_Text(Label label_FileName, Form activeForm, string applicationBrandName, Label label_FileName_in_menustrip, System.Windows.Forms.TextBox mainSingleTextBox)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                locationLoadedFile = openFileDialog1.FileName;
                // make it into an event so that it gets automatically changed
                label_FileName.Text = Path.GetFileName(locationLoadedFile); // Show the loaded file's name
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
