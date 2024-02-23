using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Verbalize
{
    internal class Handler_UI
    {
        public static TextBox textBox_Main_Single = Form1.textBox_Main_Single;
        public static Button button_SaveTextToFile, button_MuteSpokenNarration, button_PopulateVoicesAndStylesComboBoxes, button_LoadText, button_ClearTextBoxAndLoadedFile, button_HideGuiMarkup, button_CreateAudioFromTextFile, button_CreateNarrationSoundFile, button_UpdateTextFile, button_ShowHelp, button_MinimizeWindow, button_MaximizeWindow, button_QuitApplication, button_RetrieveAndLoadVoices;
        public static ComboBox comboBox_Languages, comboBox_Voices, comboBox_VoiceStyles, comboBox_Rate, comboBox_Pitch, comboBox_SoundTypes, comboBox_TextTypes;

        public static void SaveTextToFile()
        {
            string outputTextFormat = GetOutputTextFormat();
            Handler_File.SaveText(outputTextFormat, textBox_Main_Single.Text);
        }
        public static string GetOutputSoundFormat()
        {
            return comboBox_SoundTypes?.SelectedItem?.ToString() ?? "None";
        }
        public static string GetOutputTextFormat()
        {
            return comboBox_TextTypes?.SelectedItem?.ToString() ?? "None";
        }
    }
}
