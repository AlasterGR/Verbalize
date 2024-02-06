using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
