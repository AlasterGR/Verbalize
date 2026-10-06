namespace _Verbalize
{
    using _Verbalize.Properties;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Net.Http; // for the supported languages of the voice    
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml; // For constructing our xml file 
    using Verbalize.Core;

    //using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    /// <summary>
    /// The main Form of the app
    /// </summary>
    public partial class Form1 : Form
    {
        #region The Prosody and assorted elements of speech
        // As per : https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/speech-synthesis-markup-voice. The https://www.w3.org/TR/speech-synthesis11/ is irrelevant so far.
        /// <summary>  Pitch is expressed in 3 ways. Here, for now, we are using just the absolute value from the range [-200, +200]</summary>
        public static string pitch = "default";
        /// <summary>  Rate is expressed in 2 ways, an absolute value (string) and a relative (as a number) one. For now, we will use it only as a number (-50% - +50%), I will incoroprate it as a string later </summary>
        public static string rate = "default";
        /// <summary>  Defaults in 80. </summary>
        public static int volume = 80;
        /// <summary>  The Voice's style. Defaults to "calm" </summary>
        public static string style = "calm";
        //  Integrate the rest of the speech elements, such as pitch contour, pitch range
        #endregion
        /// <summary>  The Voice Language's selected locale.</summary>
        private static string selectedLocale = string.Empty;
        /// <summary>  A file that needs to be used throughout the entire app. Might put it within the VoicesLoad() nethod if possible.</summary>
        private static XmlDocument VoicesXML = new();
        /// <summary>  The "short name" of a Voice.</summary>
        private static string shortName = string.Empty;
        /// <summary>  The path of the loaded file.</summary>
        private static string locationLoadedFile = string.Empty;
        private string formatOutputSound = "mp3";
        //private static bool isSynthSpeaking = false;
        /// <summary> What type of seperator, if any, we want to have on the title</summary>
        //private static string title_fileExtention_seperator = ".";
        /// <summary> The app's name</summary>
        public static string applicationBrandName = "Verbalize";
        /// <summary> The dnamic XML of the document we are handling.</summary>
        public static XmlDocument VirtualSSMLDocument = new XmlDocument(); // the dynamic object which stores the proccessed ssml

        public static TextBox textBox_Main_Single;

        public static bool soundTypeSelectorComboboxOrRadiogroup;

        public static TableLayoutPanel table_LayoutPanel_MainGUIRow;
        public static Label label_FileName, label_FileName_in_menustrip, label_rate, label_pitch, label_system_messages, label_user_messages;
        public static VScrollBar vScrollBar_rate, vScrollBar_pitch, vScrollBar_volume;
        public static ComboBox comboBox_Languages, comboBox_Voices, comboBox_VoiceStyles, comboBox_Rate, comboBox_Pitch, comboBox_SoundTypes, comboBox_TextTypes;

        public static float attributesColumnInitialWidth, attributesColumnCurrentWidth = 451f;
        public static SizeType attributesColumnInitialSizeType;
        public static Size attributesColumnInitialMinimumSize;
        public static Button button_NarrateMainTextBox, button_SaveTextToFile, button_MuteSpokenNarration, button_PopulateVoicesAndStylesComboBoxes, button_LoadText, button_ClearTextBoxAndLoadedFile, button_HideGuiMarkup, button_CreateAudioFromTextFile, button_CreateNarrationSoundFile, button_UpdateTextFile, button_ShowHelp, button_MinimizeWindow, button_MaximizeWindow, button_QuitApplication, button_RetrieveAndLoadVoices;
        public static Image image_AttrColumnButton_Expand, image_AttrColumnButton_Recede;
        public static Icon icon_AppLogo;

        // set up dependency injection. break it apart from initialization
        /// <summary>  The public class of the app's main Form, that is window. </summary>
        public Form1()
        {
            InitializeComponent();

            Assign_AbstractEntities();
            Subscribe_AbstractButtons();
            #region Load the voices file that lists the various speech voices          
            //InitializeVoices();  Let's start with the basic voices first
            #endregion
            Handler_AudioSynthesis.Initialize();
            Handler_Networking.Initialize();
            Handler_Data.Initialize();
            Handler_File.Initialize();
        }
        /// <summary> When the window first opens, sets up its controls and fills the language, voice and style lists. </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            //  Give the controls their starting values and the window its icon.
            Assign_AbstractEntities_InitialValues();
            this.Icon = icon_AppLogo; // ActiveForm has not yet been instanced with focus (https://stackoverflow.com/questions/23826059/why-this-works-but-form-activeform-throws-nullrefernceexception).

            //  Fill the lists from the downloaded voice list, or the built-in one if there is none.
            LoadVoiceListAndRefresh();
        }

        public void Assign_AbstractEntities()
        {
            textBox_Main_Single = textBox1;

            label_FileName = label1;
            label_FileName_in_menustrip = label12;
            label_rate = label3;
            label_pitch = label2;
            label_system_messages = label15;
            label_user_messages = label17;

            comboBox_Languages = comboBox1;
            comboBox_Voices = comboBox2;
            comboBox_VoiceStyles = comboBox3;
            comboBox_Rate = comboBox4;
            comboBox_Pitch = comboBox5;
            comboBox_SoundTypes = comboBox6;
            comboBox_TextTypes = comboBox7;

            vScrollBar_rate = vScrollBar1;
            vScrollBar_pitch = vScrollBar2;
            vScrollBar_volume = vScrollBar3;

            table_LayoutPanel_MainGUIRow = tableLayoutPanel11;

            button_LoadText = bttn3_LoadText;
            button_ClearTextBoxAndLoadedFile = button1;
            button_CreateAudioFromTextFile = button4;
            button_CreateNarrationSoundFile = button6;
            button_UpdateTextFile = button2;
            button_ShowHelp = button14;
            button_MinimizeWindow = button3;
            button_MaximizeWindow = Maximize_btn;
            button_QuitApplication = button13;
            button_RetrieveAndLoadVoices = button8;
            button_PopulateVoicesAndStylesComboBoxes = button9;
            button_MuteSpokenNarration = button5;
            button_SaveTextToFile = button7;
            button_HideGuiMarkup = button10;
            button_NarrateMainTextBox = button12;
        }
        public void Assign_AbstractEntities_InitialValues()
        {
            label_FileName.Visible = label_FileName_in_menustrip.Visible = false;
            label_FileName.Text = string.Empty;
            label_system_messages.Text = "System messages";
            label_user_messages.Text = "User messages";

            image_AttrColumnButton_Expand = (Image)Resources.ResourceManager.GetObject("Triangle_Left");
            image_AttrColumnButton_Recede = (Image)Resources.ResourceManager.GetObject("Triangle_Right");
            icon_AppLogo = (Icon)Resources.ResourceManager.GetObject("Verbalize_logo");

            button_UpdateTextFile.Enabled = button_RetrieveAndLoadVoices.Enabled = button_RetrieveAndLoadVoices.Visible = button_PopulateVoicesAndStylesComboBoxes.Enabled = button_PopulateVoicesAndStylesComboBoxes.Visible = true;

            vScrollBar_volume.Value = volume;
            vScrollBar_pitch.Value = 0;
            vScrollBar_rate.Value = 0;

            button_HideGuiMarkup.BackgroundImage = image_AttrColumnButton_Recede;

            attributesColumnInitialWidth = attributesColumnCurrentWidth;
            attributesColumnInitialSizeType = table_LayoutPanel_MainGUIRow.ColumnStyles[2].SizeType; // store the Attributes columnn's sizetype
            attributesColumnInitialMinimumSize = tableLayoutPanel7.MinimumSize;

            //Choosing how to offer sound type selection
            comboBox_SoundTypes.Enabled = comboBox_SoundTypes.Visible = true;
            comboBox_SoundTypes.SelectedIndex = 0;
            //Choosing how to offer text type selection
            comboBox_TextTypes.Enabled = comboBox_TextTypes.Visible = true;
            comboBox_TextTypes.SelectedIndex = 0;
        }
        public void Subscribe_AbstractButtons()
        {
            button_LoadText.Click += Button_LoadText_Click;
            button_ClearTextBoxAndLoadedFile.Click += Button_ClearTextBoxAndLoadedFile_Click;
            button_CreateAudioFromTextFile.Click += Button_CreateAudioFromTextFile_Click;
            button_CreateNarrationSoundFile.Click += Button_CreateNarrationSoundFile_Click;
            button_UpdateTextFile.Click += Button_UpdateTextFile_Click;
            button_ShowHelp.Click += Button_ShowHelp_Click;
            button_MinimizeWindow.Click += Button_MinimizeWindow_Click;
            button_MaximizeWindow.Click += Button_MaximizeWindow_Click;
            button_QuitApplication.Click += Button_QuitApplication_Click;
            button_RetrieveAndLoadVoices.Click += Button_RetrieveAndLoadVoices_Click;
            button_PopulateVoicesAndStylesComboBoxes.Click += Button_PopulateVoicesAndStylesComboBoxes_Click;
            button_MuteSpokenNarration.Click += Button_MuteSpokenNarration_Click;
            button_SaveTextToFile.Click += Button_SaveTextToFile_Click;
            button_HideGuiMarkup.Click += Button_HideGuiMarkup_Click;
            button_NarrateMainTextBox.Click += Button_NarrateMainTextBox_Click;
        }
        #region Main GUI Buttons -not the menu strip ones. Here are the calls to the functions, same with the menu strip ones
        private void Button_HideGuiMarkup_Click(object sender, EventArgs e)
        {
            HideGuiMarkup();
        }
        private void Button_ClearTextBoxAndLoadedFile_Click(object sender, EventArgs e)
        {
            ClearTextBoxAndLoadedFile();
        }
        private async void Button_MuteSpokenNarration_Click(object sender, EventArgs e)
        {
            MuteSpokenNarration();
        }
        private void Button_SaveTextToFile_Click(object sender, EventArgs e)
        {
            Save_MainTextBoxText_ToFile();
        }
        private void Button_LoadText_Click(object sender, EventArgs e)
        {
            LoadText();
        }
        private void Button_CreateAudioFromTextFile_Click(object sender, EventArgs e) // Export sound from a local file
        {
            CreateAudioFromTextFile();
        }
        /// <summary> The Update button </summary>
        private void Button_UpdateTextFile_Click(object sender, EventArgs e)
        {
            UpdateTextFile();
        }
        private void Button_CreateNarrationSoundFile_Click(object sender, EventArgs e) // Export sound
        {
            CreateNarrationSoundFile();
        }
        /// <summary> When the Populate button is clicked, fills the lists from the downloaded voice list. </summary>
        private async void Button_PopulateVoicesAndStylesComboBoxes_Click(object sender, EventArgs e)
        {
            //  Fill the lists, downloading the voice list first if needed.
            await PopulateVoicesAndStylesComboBoxesAsync();
        }
        private void Button_NarrateMainTextBox_Click(object sender, EventArgs e)
        {
            NarrateMainTextBox();
        }
        /// <summary> When the Download button is clicked, downloads the latest voice list and fills the lists from it. </summary>
        private async void Button_RetrieveAndLoadVoices_Click(object sender, EventArgs e)
        {
            //  Download the voice list and refill the lists.
            await RetrieveAndLoadVoicesAsync();
        }
        public static void LoadText()
        {
            Handler_File.Load_Text(label_FileName, Form1.ActiveForm, applicationBrandName, label_FileName_in_menustrip, textBox_Main_Single);
        }
        #endregion

        #region Button functions.

        /// <summary> This is the function which changes the form fullscreen -- normalscreen </summary>
        public void MaximizeWindow()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                Size = new Size(1278, 718);  //This is the minimum size as set at the editor. Will change this to remember the size while in normal screen and revert to that
                fullScreenToolStripMenuItem.Text = "Full Screen";
            }
            else if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                fullScreenToolStripMenuItem.Text = "Normal Screen";
            }
        }
        public void MinimizeWindow()
        {
            WindowState = FormWindowState.Minimized;
        }
        public static void QuitApplication()
        {
            Application.Exit();
        }
        public static void HideGuiMarkup()
        {
            table_LayoutPanel_MainGUIRow.ColumnStyles[2].SizeType = SizeType.Absolute;
            if (table_LayoutPanel_MainGUIRow.ColumnStyles[2].Width > 0)
            {
                table_LayoutPanel_MainGUIRow.ColumnStyles[2].Width = 0;
                button_HideGuiMarkup.BackgroundImage = image_AttrColumnButton_Expand;
            }
            else
            {
                //MessageBox.Show("attributesColumnInitialWidth = " + attributesColumnInitialWidth);
                table_LayoutPanel_MainGUIRow.ColumnStyles[2].Width = attributesColumnInitialWidth;
                //MessageBox.Show("tableLayoutPanel11.ColumnStyles[2].Width = " + tableLayoutPanel11.ColumnStyles[2].Width);
                table_LayoutPanel_MainGUIRow.ColumnStyles[2].SizeType = attributesColumnInitialSizeType;
                // MessageBox.Show("attributesColumnInitialSizeType : " + attributesColumnInitialSizeType.ToString());
                button_HideGuiMarkup.BackgroundImage = image_AttrColumnButton_Recede;
            }
            attributesColumnCurrentWidth = table_LayoutPanel_MainGUIRow.ColumnStyles[2].Width;
            //MessageBox.Show("attributesColumnCurrentWidth = " + attributesColumnCurrentWidth.ToString());
        }
        /// <summary> This is the method that clears the main TextBox and subsequently the label which presents the file name to the GUI. </summary>
        public static void ClearTextBoxAndLoadedFile()
        {
            locationLoadedFile = string.Empty;
            textBox_Main_Single.Clear();
            label_FileName.Text = string.Empty;
            label_FileName.Visible = false; // remove this when implement the label's automatic behaviour through events
        }
        public static void MuteSpokenNarration()
        {
            Handler_AudioSynthesis.SoundPause();
        }
        public static void CreateAudioFromTextFile()
        {
            Handler_File.CreateAudioFileFromTextFile();
        }
        public static void CreateNarrationSoundFile()
        {
            Handler_File.CreateAudioFileFromTextBox();
        }
        public static void UpdateTextFile()
        {
            //string pathFileSelected = locationLoadedFile;
            //string text = this.mainSingleTextBox.Text;
            //XmlDocument SSMLDocument = DataHandling.CreateSSML(text);
            //SSMLDocument.Save(pathFileSelected);// Save the XML document to a file
            //formatOutputSound = SetOutputSoundFormat();
            //_ = SynthesizeAudioAsync(pathFileSelected, formatOutputSound, false);
        }
        /// <summary> Fills the language, voice and style lists from the downloaded voice list, downloading it first if there is no usable copy. </summary>
        /// <returns>A task that finishes once the lists are filled.</returns>
        public static async Task PopulateVoicesAndStylesComboBoxesAsync()
        {
            //  Download the voice list first if there is no usable downloaded copy.
            if (!VoiceListStore.LoadOrDefault(Handler_File.locationOfVoicesFile).IsDownloadedList)
            {
                await Retrieve_Voices_AndSaveToDisk();
            }

            //  Fill the lists from the downloaded list, or the built-in one if there is still none.
            LoadVoiceListAndRefresh();
        }
        public static void NarrateMainTextBox()
        {
            Handler_AudioSynthesis.SpeakFromTextBox(textBox_Main_Single);
        }
        /// <summary> Downloads the latest voice list from Azure and, if that works, refills the language, voice and style lists from it. </summary>
        /// <returns>A task that finishes once the lists are refilled or the user has been told the download failed.</returns>
        public static async Task RetrieveAndLoadVoicesAsync()
        {
            //  Download the list, and refill the lists only if the download worked.
            if (await Retrieve_Voices_AndSaveToDisk())
            {
                LoadVoiceListAndRefresh();
            }
        }

        /// <summary> Downloads the list of voices from Azure and saves it in the app's Resources folder. </summary>
        /// <returns>True if the list was downloaded and saved; false if it failed, in which case the user has been told why.</returns>
        public static async Task<bool> Retrieve_Voices_AndSaveToDisk()
        {
            try
            {
                //  Ask Azure for the list of voices in the app's region.
                using HttpClient client = new HttpClient();
                string subscriptionKey = Handler_Data.GetTheSubscriptionKey();
                string serverLocation = Handler_Data.GetTheServerLocation();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string listVoicesLocationURL = "https://" + serverLocation + ".tts.speech.microsoft.com/cognitiveservices/voices/list";
                HttpResponseMessage response = await client.GetAsync(listVoicesLocationURL);

                //  Save the list if Azure sent it.
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Handler_Data.Convert_JSONtoXML_AndSaveToDisk(responseData);
                    return true;
                }

                //  Tell the user if Azure refused to send it.
                ShowVoiceListDownloadError($"Azure answered {(int)response.StatusCode} ({response.ReasonPhrase}).");
                return false;
            }
            //  Tell the user if the list could not be fetched or saved, for example without an internet connection.
            catch (Exception exception)
            {
                ShowVoiceListDownloadError(exception.Message);
                return false;
            }
        }

        /// <summary> Tells the user that the voice list could not be downloaded, and why. </summary>
        /// <param name="reason">Why the download failed.</param>
        private static void ShowVoiceListDownloadError(string reason)
        {
            //  Show the reason in an error message.
            MessageBox.Show("The voice list could not be downloaded, so the app will keep using the voices it already has." + Environment.NewLine + Environment.NewLine + reason,
                applicationBrandName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary> Loads the downloaded voice list, or the built-in one if there is none, and refills the language, voice and style lists from it. </summary>
        public static void LoadVoiceListAndRefresh()
        {
            //  Pick the voice list to use, then refill the lists from it.
            VoicesXML = VoiceListStore.LoadOrDefault(Handler_File.locationOfVoicesFile).Voices;
            VoicesLoad();
        }

        /// <summary> Fills the Languages list from the loaded voice list, falling back to the built-in voice list if none has been loaded. </summary>
        public static void VoicesLoad()
        {
            //  Use the loaded voice list if there is one.
            if (VoicesXML.DocumentElement != null)
            {
                //  Replace the Languages list with each language once, selecting the first.
                comboBox_Languages.Items.Clear();
                foreach (string? localeName in VoiceCatalog.GetLocaleNames(VoicesXML))
                {
                    comboBox_Languages.Items.Add(localeName!);
                    comboBox_Languages.SelectedIndex = 0;
                }
            }
            //  Otherwise load the built-in voice list and try again.
            else { VoicesXML.LoadXml(VoicesBasic); VoicesLoad(); }

            //  Preselect the default speaking style.
            comboBox_VoiceStyles.SelectedItem = style;
        }
        public static void Save_MainTextBoxText_ToFile()
        {
            string outputTextFormat = GetOutputFormatFromComboBox(comboBox_TextTypes);
            string sourceText = textBox_Main_Single.Text;
            Handler_File.SaveText(outputTextFormat, sourceText);
        }
        public static string GetOutputFormatFromComboBox(ComboBox _comboBox)
        {
            return _comboBox?.SelectedItem?.ToString() ?? "None";
        }
        private void ShowOrHideVoiceStylesRow(TableLayoutPanel _tableLayoutPanel, int _StylesRow, bool _showOrHide)
        {
            for (int columnIndex = 0; columnIndex < _tableLayoutPanel.ColumnCount; columnIndex++)
            {
                Control control = _tableLayoutPanel.GetControlFromPosition(columnIndex, _StylesRow);
                if (control != null)
                {
                    control.Visible = _showOrHide;
                }
            }
        }
        #endregion
        async void InitializeVoices()  //  See if it should be called from a button
        {
            try
            {
                VoicesXML.Load(Handler_File.locationOfVoicesFile);
            }
            catch (Exception)
            {
                try
                {
                    VoicesXML.Load(Handler_File.locationOfVoicesFileBackup);
                }
                catch
                {
                    Directory.CreateDirectory(Handler_File.folderResources);
                    await Retrieve_Voices_AndSaveToDisk();
                }
            }
        }

        #region Language combo box
        /// <summary> When a language is chosen, lists its voices and sets it as the speech language. </summary>
        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Find the voices that speak the chosen language.
            string selectedLocaleName = comboBox_Languages?.SelectedItem?.ToString() ?? string.Empty;
            VoicesInLocale voicesInLocale = VoiceCatalog.GetVoicesInLocale(VoicesXML, selectedLocaleName);

            //  Show the language's code and use it as the speech language.
            if (voicesInLocale.Locale != null)
            {
                this.label6.Text = voicesInLocale.Locale;
                Handler_AudioSynthesis.SetSpeechSynthesisLanguage(voicesInLocale.Locale);
            }

            //  List the voices and select the first one, if there is one.
            comboBox_Voices.DataSource = voicesInLocale.DisplayNames.ToList();
            try { comboBox_Voices.SelectedIndex = 0; } catch { }
        }
        #endregion
        #region Voice actor combo box
        /// <summary> When a voice is chosen, sets it as the speaking voice, shows its local name and gender, and lists its speaking styles. </summary>
        private void ComboBox_Voices_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Find the chosen voice in the voice list.
            string selectedDisplayName = comboBox_Voices?.SelectedItem?.ToString() ?? string.Empty;
            VoiceIdentity? voice = VoiceCatalog.FindVoiceByDisplayName(VoicesXML, selectedDisplayName);

            //  Use it as the speaking voice and show its local name and gender.
            if (voice != null)
            {
                Handler_AudioSynthesis.SetSpeechSynthesisVoiceName(voice.ShortName);
                label7.Text = " - " + voice.LocalName + " (" + voice.Gender + ")";
            }

            //  List the speaking styles that the chosen voice supports.
            PopulateSingleVoiceStylesComboBox(comboBox_VoiceStyles, comboBox_Voices.SelectedItem?.ToString());
        }
        #endregion
        /// <summary> Lists the speaking styles of a voice, hiding the styles row if the voice has none. </summary>
        /// <param name="_voiceStylesComboBox">The list to fill with the styles.</param>
        /// <param name="_selectedVoice">The display name of the voice.</param>
        private void PopulateSingleVoiceStylesComboBox(ComboBox _voiceStylesComboBox, string? _selectedVoice)
        {
            //  Empty the styles list, including any text showing in it.
            _voiceStylesComboBox.Items.Clear();
            _voiceStylesComboBox.Text = string.Empty;

            //  Look up the voice's styles in the voice list in use and add them to the list.
            foreach (string voiceStyle in VoiceCatalog.GetStyles(VoicesXML, _selectedVoice))
            {
                _voiceStylesComboBox.Items.Add(voiceStyle);
            }

            //  Select the first style if there are any, and show the styles row only in that case.
            int selectedIndex = _voiceStylesComboBox.Items.Count > 0 ? 0 : -1;
            _voiceStylesComboBox.SelectedIndex = selectedIndex;
            ShowOrHideVoiceStylesRow(tableLayoutPanel8, 2, selectedIndex == 0);
        }

        /// <summary> When the pitch slider moves, uses its position as a change in pitch and clears the named pitch choice. </summary>
        private void ScrollBar_Pitch_ValueChanged(object sender, EventArgs e)
        {
            //  Turn the slider position into a pitch change and show it.
            pitch = ProsodyFormatter.FormatRelativePitch(vScrollBar_pitch.Value);
            label_pitch.Text = "Pitch = " + pitch;

            //  Clear the named pitch choice, since the slider now decides the pitch.
            comboBox_Pitch.SelectedItem = null;
        }
        /// <summary> Voice Rate slider </summary>
        private void ScrollBar_Rate_ValueChanged(object sender, EventArgs e)
        {
            rate = vScrollBar_rate.Value.ToString() + "%";
            label_rate.Text = "Rate = " + vScrollBar_rate.Value.ToString() + "%";
            comboBox_Rate.SelectedItem = null;
        }
        /// <summary> Voice Volume slider </summary>
        private void ScrollBar_Volume_ValueChanged(object sender, EventArgs e)
        {
            volume = vScrollBar_volume.Value;
            label11.Text = volume.ToString();
        }
        /// <summary> Voice Rate selection </summary>
        private void ComboBox_Rate_SelectedIndexChanged(object sender, EventArgs e)
        {
            rate = comboBox_Rate?.SelectedItem?.ToString() ?? "Default";
            label_rate.Text = "Rate : " + rate;
        }
        /// <summary> Voice Pitch selection </summary>
        private void ComboBox_Pitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            pitch = comboBox_Pitch?.SelectedItem?.ToString() ?? "Default";
            label_pitch.Text = "Pitch : " + pitch;
        }
        /// <summary> Voice Style selection </summary>
        private void ComboBox_VoiceStyles_SelectedIndexChanged(object sender, EventArgs e)
        { style = comboBox_VoiceStyles?.SelectedItem?.ToString() ?? "calm"; }

        /// <summary> Virtual object of our final XML document. Will be changed dynamically through the app and saved in the disk and loading from a disk's file.</summary>
        static XmlDocument InitializeSSMLDocument()
        {
            #region Creation of the XML document and and its root element (called "Speak")
            XmlDocument SSMLDocument = new();
            XmlElement speak = SSMLDocument.CreateElement("speak");
            SSMLDocument.AppendChild(speak);
            // XML declaration. Mandatory for XML 1.1 and SSML also requires this : https://www.w3.org/TR/speech-synthesis/#S2.1
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
            lang.Value = Handler_AudioSynthesis.GetSpeechSynthesisLanguage();
            speak.SetAttributeNode(lang);
            XmlAttribute onlangfailure = SSMLDocument.CreateAttribute("onlangfailure");
            onlangfailure.Value = "ignoretext ";
            speak.SetAttributeNode(onlangfailure);
            #endregion
            #region creating new Voice node using variables for the attributes. 
            XmlElement voice = SSMLDocument.CreateElement("voice");
            string _voiceAttributeValue = Handler_AudioSynthesis.GetSpeechSynthesisVoiceName();
            voice.SetAttribute("name", _voiceAttributeValue);
            XmlElement prosody = SSMLDocument.CreateElement("prosody");
            prosody.SetAttribute("rate", rate);
            prosody.SetAttribute("pitch", pitch);
            prosody.SetAttribute("volume", (volume).ToString()); //  Might not work on this version of SSML
            XmlElement express = SSMLDocument.CreateElement("mstts", "express-as", "http://www.w3.org/2001/mstts");
            express.SetAttribute("style", style);
            #endregion
            #region Appending the XML elements to their parents
            prosody.AppendChild(express);
            voice.AppendChild(prosody);
            speak.AppendChild(voice);
            #endregion
            express.InnerText = string.Empty;
            return SSMLDocument;
        }
        static XmlDocument AppendSSMLDocument(XmlDocument _VirtualSSMLDocument, string _text)
        {
            XmlDocument SSMLDocument = new();
            #region creating new Voice node using variables for the attributes. 
            XmlElement voice = SSMLDocument.CreateElement("voice");
            string _voiceAttributeValue = Handler_AudioSynthesis.GetSpeechSynthesisLanguage();
            voice.SetAttribute("name", _voiceAttributeValue);
            XmlElement prosody = SSMLDocument.CreateElement("prosody");
            prosody.SetAttribute("rate", rate);
            prosody.SetAttribute("pitch", pitch);
            prosody.SetAttribute("volume", (volume).ToString()); //  Might not work on this version of SSML
            XmlElement express = SSMLDocument.CreateElement("mstts", "express-as", "http://www.w3.org/2001/mstts");
            express.SetAttribute("style", style);
            #endregion
            #region Appending the XML elements to their parents
            prosody.AppendChild(express);
            voice.AppendChild(prosody);
            //speak.AppendChild(voice);
            #endregion
            express.InnerText = string.Empty;
            return SSMLDocument;
        }

        /// <summary> Shows the voice settings of a loaded SSML document in the app, using defaults for anything the document does not state. </summary>
        /// <param name="SSMLDocument">The loaded SSML document.</param>
        public static void LoadXMLtoApp(XmlDocument SSMLDocument)
        {
            //  Read the language, voice, style, rate, pitch and volume from the document.
            SsmlLoadedSettings loaded = SsmlSettingsReader.Read(SSMLDocument);
            string langValue = loaded.Language;
            string nameValue = loaded.VoiceName;

            //  Find the voice's language name and display name in the voice list, falling back to Jenny in US English.
            string localeName = "English (United States)";
            string DisplayName = "Jenny";
            VoiceListing? listing = VoiceCatalog.FindVoiceByShortName(VoicesXML, nameValue);
            if (listing != null)
            {
                localeName = listing.LocaleName;
                DisplayName = listing.DisplayName;
            }

            //  Use the document's style only if the styles list offers it, otherwise use calm.
            string styleSSML = loaded.Style;
            if (comboBox_VoiceStyles.FindStringExact(styleSSML) == -1) { styleSSML = "calm"; }

            //  Take the document's rate, pitch and volume, keeping the current volume if the document's cannot be read.
            rate = loaded.Rate;
            pitch = loaded.Pitch;
            if (loaded.Volume is int loadedVolume) { volume = loadedVolume; }

            //  Speak with the document's voice and in the document's language.
            Handler_AudioSynthesis.SetSpeechSynthesisVoiceName(nameValue);
            Handler_AudioSynthesis.SetSpeechSynthesisLanguage(langValue);

            //  Select the document's language, voice and style in their lists.
            comboBox_Languages.SelectedItem = localeName;
            comboBox_Voices.SelectedItem = DisplayName;
            comboBox_VoiceStyles.SelectedItem = styleSSML;

            //  Show the pitch as a named level, as a slider position, or as "default" if it is neither.
            if (comboBox_Pitch.Items.Contains(pitch))
            {
                comboBox_Pitch.SelectedItem = pitch;
                label_pitch.Text = "Pitch : " + pitch;
            }
            else if (pitch.Contains("Hz") && SsmlValueParser.TryParseSignedInteger(pitch, out int pitchInt))
            {
                vScrollBar_pitch.Value = Math.Clamp(pitchInt, vScrollBar_pitch.Minimum, vScrollBar_pitch.Maximum);
                comboBox_Pitch.SelectedItem = null;
                label_pitch.Text = "Pitch = " + vScrollBar_pitch.Value.ToString();
            }
            else
            {
                pitch = "default";
                comboBox_Pitch.SelectedItem = pitch;
                label_pitch.Text = "Pitch : " + pitch;
            }

            //  Show the rate as a named speed, as a slider position, or as "default" if it is neither.
            if (comboBox_Rate.Items.Contains(rate))
            {
                comboBox_Rate.SelectedItem = rate;
                label_rate.Text = "Rate : " + rate;
            }
            else if (rate.Contains('%') && SsmlValueParser.TryParseSignedInteger(rate, out int rateInt))
            {
                vScrollBar_rate.Value = Math.Clamp(rateInt, vScrollBar_rate.Minimum, vScrollBar_rate.Maximum);
                comboBox_Rate.SelectedItem = null;
                label_rate.Text = "Rate = " + vScrollBar_rate.Value.ToString();
            }
            else
            {
                rate = "default";
                label_rate.Text = "Rate : " + rate;
                comboBox_Rate.SelectedItem = rate;
            }

            //  Move the volume slider to the volume.
            vScrollBar_volume.Value = volume;
        }
        /// <summary> Check if we have loaded a file and have the file name and update button accordingly visible.</summary>
        private void Label_FileName_TextChanged(object sender, EventArgs e)
        {
            //button2.Enabled = !string.IsNullOrEmpty(label_filename.Text);// enable back when complete the code
            if (label_FileName.Text != string.Empty) label_FileName.Visible = true;
        }

        private void ComboBox_SoundTypes_SelectedIndexChanged(object sender, EventArgs e)
        { formatOutputSound = comboBox_SoundTypes?.SelectedItem?.ToString() ?? "None"; }

        #region the vertical ScrollBar's annotations ~ could make it abstract for horizontal scrollbar as well
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            int annotationWidth = SystemInformation.VerticalScrollBarWidth;
            int annotationHeight = (SystemInformation.VerticalScrollBarThumbHeight / 10);
            int barHeight = SystemInformation.VerticalScrollBarThumbHeight;
            System.Windows.Forms.VScrollBar? vScrollBar = null;
            foreach (Control c in panel.Controls)
            {
                if (c is VScrollBar)
                {
                    vScrollBar = (System.Windows.Forms.VScrollBar)c;
                    break;
                }
            }
            int annotationX = vScrollBar.Right;
            int ceiling = vScrollBar.Top + vScrollBar.Margin.Top + barHeight + barHeight / 2;  //  the highest point, within the control, from which the annontations are drawn - will be the 1st one as well             
            int floor = vScrollBar.Bottom - vScrollBar.Margin.Bottom - barHeight - barHeight / 2;
            int stepMath = 12;// the mathematical step between the annontations
            int stepGraphic = (floor - ceiling) / stepMath; // the graphical step between the annontations
            int annotationYOffset = 0; // A small offset so that the lines are drawn at a distance fromt he scrollbar.
            int annotationXOffset = -5; // A small offset so that the lines are always drawn at the middle of the Thumb.
            int stepAnontations = Math.Abs(vScrollBar.Maximum - vScrollBar.Minimum) / stepMath; // this nis how much will be added in the annontations, effectively showcasing the scrollbar's value at their point
            int i = 0;  //  this is our step. There will be 10-increment steps
            int value = vScrollBar.Minimum;
            for (int annotationY = ceiling; annotationY <= floor; annotationY += stepGraphic) //  ceiling is actually a small number
            {
                if (value != 0)
                {
                    e.Graphics.FillRectangle(Brushes.Black, new Rectangle(annotationX, annotationY, annotationWidth, annotationHeight));

                    e.Graphics.DrawString(value.ToString("+#;-#") + " %", Font, Brushes.Black, new Point(annotationX + annotationWidth + annotationYOffset, annotationY - barHeight / 2 + annotationXOffset));  //  Would rather draw the strings of the scrollbar's actual value
                }
                i += stepMath;
                value += stepAnontations;
            }
            e.Graphics.FillRectangle(Brushes.Red, new Rectangle(annotationX, vScrollBar.Height / 2, annotationWidth * 2, annotationHeight));
            e.Graphics.DrawString("0", Font, Brushes.Black, new Point(annotationX + annotationWidth * 2 + annotationYOffset / 2, ((vScrollBar.Height / 2) - barHeight / 2 + annotationXOffset)));
        }
        #endregion

        /// <summary> Draw a light blue rectangle as the text file's title background. </summary>
        private void label_FileName_Paint(object sender, PaintEventArgs e)
        {
            using LinearGradientBrush brush = new(ClientRectangle, Color.LightBlue, Color.White, 0F);
            e.Graphics.FillRectangle(brush, ClientRectangle);
            TextRenderer.DrawText(e.Graphics, label_FileName.Text, label_FileName.Font, label_FileName.ClientRectangle, label_FileName.ForeColor, TextFormatFlags.Default);
        }
        /// <summary> This is in order to make sure the panels are redrawn properly. Invalidate() any other control that is drawn uniquely. </summary>
        public void ReDrawEverything()
        {
            foreach (Control control in Controls)
            {
                panel1.Invalidate();
                panel2.Invalidate();
                control.Invalidate();
                label_FileName = label1;// need to fix this, so that it gets called as the below
                label_FileName.Invalidate();

            }
        }
        private void Form1_Resize(object sender, EventArgs e) { ReDrawEverything(); }
        private void Form1_Paint(object sender, PaintEventArgs e) { /*ReDrawEverything();*/ }

        public static void Inform_WithSystemMessage(string _message)
        {
            label_system_messages.Text = _message;
            label_system_messages.Invalidate();
        }
        public static void Inform_WithUserMessage(string _message)
        {
            label_user_messages.Text = _message;
            label_user_messages.Invalidate();
        }
        #region Menu Bar items

        #region Menu strip buttons
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save_MainTextBoxText_ToFile();
        }
        /// <summary> When "Download voices list" is chosen in the menu, downloads the latest voice list and fills the lists from it. </summary>
        private async void DownloadVoicesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Download the voice list and refill the lists.
            await RetrieveAndLoadVoicesAsync();
        }
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadText();
        }
        private void FullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaximizeWindow();
        }
        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTextFile();
        }
        private void ExportSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNarrationSoundFile();
        }
        private void ExportTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save_MainTextBoxText_ToFile();
        }
        private void ExportBatchAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateAudioFromTextFile();
        }
        private void MenuBarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void AppGUIToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void ColourSchemeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary> When "Populate" is chosen in the menu, fills the lists from the downloaded voice list. </summary>
        private async void PopulateVoicesAndStylesComboBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Fill the lists, downloading the voice list first if needed.
            await PopulateVoicesAndStylesComboBoxesAsync();
        }
        private void PresetLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Window buttons - Help, Minimize, Maximize, Quit
        private void Button_ShowHelp_Click(object sender, EventArgs e)
        {

        }
        private void Button_MinimizeWindow_Click(object sender, EventArgs e)
        {
            MinimizeWindow();
        }
        private void Button_MaximizeWindow_Click(object sender, EventArgs e)
        {
            MaximizeWindow();
        }
        private void Button_QuitApplication_Click(object sender, EventArgs e)
        {
            QuitApplication();
        }
        #endregion

        #endregion

        #region A string in XML format that is to be used, should the app not be able to download the Voices file
        public readonly static string VoicesBasic = DefaultVoices.Xml;
        #endregion

    }

}