namespace _Verbalize
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml; // For constructing our xml file 
    using System.Net.Http; // for the supported languages of the voice    
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Drawing.Drawing2D;
    using _Verbalize.Properties;

    //using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    /// <summary>
    /// The main Form of the app
    /// </summary>
    public partial class Form1 : Form
    {
        //static string regionService = Environment.GetEnvironmentVariable("westeurope");
        // If, at some point, MS changes Cognitive Services authorization protocols, https://learn.microsoft.com/en-us/azure/cognitive-services/speech-service/rest-text-to-speech provides the methods used
        
        
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
        /// <summary>  Backup voices file. Change this to the desired file name and extension.</summary>
        private static string voicesSSMLFileNameBackup = "Voices_[orig].xml";
        /// <summary>  Backup voices file. The file in which we store them.</summary>
        //private static string locationFileResponseBackup = Path.Combine(folderResources, voicesSSMLFileNameBackup);
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
        public static Label label_FileName, label_FileName_in_menustrip, label_rate, label_pitch;
        public static VScrollBar vScrollBar_rate, vScrollBar_pitch, vScrollBar_volume;
        public static ComboBox comboBox_Languages, comboBox_Voices, comboBox_VoiceStyles, comboBox_Rate, comboBox_Pitch, comboBox_SoundTypes, comboBox_TextTypes;

        public static float attributesColumnInitialWidth, attributesColumnCurrentWidth = 451f;
        public static SizeType attributesColumnInitialSizeType;
        public static Size attributesColumnInitialMinimumSize;
        public static Button button_NarrateMainTextBox, button_SaveTextToFile, button_MuteSpokenNarration, button_PopulateVoicesAndStylesComboBoxes, button_LoadText, button_ClearTextBoxAndLoadedFile, button_HideGuiMarkup, button_CreateAudioFromTextFile, button_CreateNarrationSoundFile, button_UpdateTextFile, button_ShowHelp, button_MinimizeWindow, button_MaximizeWindow, button_QuitApplication, button_RetrieveAndLoadVoices;
        public static Image image_AttrColumnButton_Expand, image_AttrColumnButton_Recede;
        public static Icon icon_AppLogo;

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

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Assign_AbstractEntities_InitialValues();
            this.Icon = icon_AppLogo; // The app's main icon. ActiveForm has not yet been instanced with focus (https://stackoverflow.com/questions/23826059/why-this-works-but-form-activeform-throws-nullrefernceexception).
            VoicesLoad();  //  Load the voices onto the combo boxes            
        }

        public void Assign_AbstractEntities()
        {
            textBox_Main_Single = textBox1;

            label_FileName = label1;
            label_FileName_in_menustrip = label12;
            label_rate = label3;
            label_pitch = label2;

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

            image_AttrColumnButton_Expand = (Image)Resources.ResourceManager.GetObject("Triangle_Left");
            image_AttrColumnButton_Recede = (Image)Resources.ResourceManager.GetObject("Triangle_Right");
            icon_AppLogo = (Icon)Resources.ResourceManager.GetObject("Verbalize_logo");

            button_UpdateTextFile.Enabled = button_RetrieveAndLoadVoices.Enabled = button_RetrieveAndLoadVoices.Visible = button_PopulateVoicesAndStylesComboBoxes.Enabled = button_PopulateVoicesAndStylesComboBoxes.Visible = false;

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
        private void Button_PopulateVoicesAndStylesComboBoxes_Click(object sender, EventArgs e)
        {
            PopulateVoicesAndStylesComboBoxes();
        }
        private void Button_NarrateMainTextBox_Click(object sender, EventArgs e)
        {
            NarrateMainTextBox();
        }
        private async void Button_RetrieveAndLoadVoices_Click(object sender, EventArgs e)
        {
            RetrieveAndLoadVoices();
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
        public static void PopulateVoicesAndStylesComboBoxes()
        {
            try
            {
                VoicesXML.Load(Path.Combine(Handler_File.locationFileResponse)); // Use Load because LoadXML command produces error
            }
            catch (Exception)
            {
                _ = VoicesRetrieve();
            }
            VoicesLoad();
        }
        public static void NarrateMainTextBox()
        {
            Handler_AudioSynthesis.SpeakFromTextBox(textBox_Main_Single);
        }
        /// <summary> Retrieve the list of voices from speech.microsoft.com and reload the voices into the boxes </summary>
        public async void RetrieveAndLoadVoices()
        {
            // will need to abstract it further and move into other classes in order to work correctly
            //await VoicesRetrieve();
            //VoicesLoad();
        }
        public static async Task VoicesRetrieve()  // need to make it wait until it is finished
        {
            var client = new HttpClient();
            string subscriptionKey = Handler_Data.GetTheSubscriptionKey();
            string serverLocation = Handler_Data.GetTheServerLocation();
            //older code to deprecate
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string listVoicesLocationURL = "https://" + serverLocation + ".tts.speech.microsoft.com/cognitiveservices/voices/list";
            HttpResponseMessage response = await client.GetAsync(listVoicesLocationURL);
            if (response.IsSuccessStatusCode)
            {
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new(responseStream))
                    {
                        string responseData = reader.ReadToEnd();
                        Handler_Data.SSML_JSONtoXMLConvert(responseData);
                    }
                }
            }
            //newer code to switch to
            //HttpResponseMessage responseMessage = Handler_Networking.RetrieveDataFromServer("VoicesList", serverLocation, subscriptionKey).Result;
            //String responseData = Handler_Data.TransformHttpResponceIntoString(responseMessage).Result;
            //Handler_Data.SSML_JSONtoXMLConvert(responseData);
        }
        /// <summary> This function loads the list of language elements from the SSML Voice file onto the Languages combo box </summary>
        public static void VoicesLoad()
        {
            if (VoicesXML.DocumentElement != null)  //  To make sure it has been downloaded and loaded
            {
                List<string> uniqueLocales = new();  //  List that will contain only the unique values of Locale, for reference, to populate the combo box with unique elements only
                comboBox_Languages.Items.Clear();
                foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice")) //Expression "//Voice" works as well
                {
                    string? locale = node?.SelectSingleNode("Locale")?.InnerText;
                    if (!uniqueLocales.Contains(locale))
                    {
                        uniqueLocales.Add(locale);
                        string? localeName = node?.SelectSingleNode("LocaleName")?.InnerText;
                        comboBox_Languages.Items.Add(localeName);
                    }
                    string? localName = node?.SelectSingleNode("LocalName")?.InnerText ?? "N/A";
                    string? gender = node?.SelectSingleNode("Gender")?.InnerText ?? "N/A";
                    comboBox_Languages.SelectedIndex = 0;
                }
            }
            else { VoicesXML.LoadXml(VoicesBasic); VoicesLoad(); }
            comboBox_VoiceStyles.SelectedItem = style;  //  Have the the default voice style preselected as default
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
                VoicesXML.Load(Handler_File.locationFileResponse);
            }
            catch (Exception)
            {
                try
                {
                    VoicesXML.Load(Handler_File.locationFileResponseBackup);
                }
                catch
                {
                    System.IO.Directory.CreateDirectory(Handler_File.folderResources);
                    await VoicesRetrieve();
                }
            }
        }

        #region Language combo box
        private void LanguagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> displayNames = new();
            string selectedLocaleName = comboBox_Languages?.SelectedItem?.ToString() ?? string.Empty;  // languagesComboBox.SelectedItem.ToString() is not a string that can be handled within the next if()

            foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice"))
            {
                string localeName = node?.SelectSingleNode("LocaleName")?.InnerText ?? string.Empty;
                if (localeName == selectedLocaleName)
                {
                    displayNames.Add(node.SelectSingleNode("DisplayName").InnerText);
                    this.label6.Text = node.SelectSingleNode("Locale").InnerText;  //  can probably delete this
                    string _speechSynthesisLanguage = node.SelectSingleNode("Locale").InnerText;
                    //Handler_AudioSynthesis.config.SpeechSynthesisLanguage = node.SelectSingleNode("Locale").InnerText;
                    Handler_AudioSynthesis.SetSpeechSynthesisLanguage(_speechSynthesisLanguage);
                }
            }
            comboBox_Voices.DataSource = displayNames;
            try { comboBox_Voices.SelectedIndex = 0; } catch { } //  This clause is for when pressing to populate the boxes but with the basic Voices loaded
        }
        #endregion
        #region Voice actor combo box
        private void ComboBox_Voices_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice"))
            {
                string localName = node?.SelectSingleNode("DisplayName")?.InnerText ?? string.Empty;
                string selectedDisplayName = comboBox_Voices?.SelectedItem?.ToString() ?? string.Empty;
                if (localName == selectedDisplayName)
                {
                    string _SpeechSynthesisVoiceName = node.SelectSingleNode("ShortName").InnerText;  //  We store who our voice actor will be
                    Handler_AudioSynthesis.SetSpeechSynthesisVoiceName(_SpeechSynthesisVoiceName);
                    label7.Text = " - " + node.SelectSingleNode("LocalName").InnerText + " (" + node.SelectSingleNode("Gender").InnerText + ")";
                }
            }
            PopulateSingleVoiceStylesComboBox(comboBox_VoiceStyles, comboBox_Voices.SelectedItem?.ToString()); // enter the combobox which holds the selected Voice's styles
        }
        #endregion
        private void PopulateSingleVoiceStylesComboBox(ComboBox _voiceStylesComboBox, string _selectedVoice)
        {
            // Clear the existing items in voiceStylesComboBox
            _voiceStylesComboBox.Items.Clear();
            _voiceStylesComboBox.Text = string.Empty; // needed in order to have the list appear visibly clear regardless of the fact we will be hiding it
            // Find the corresponding node in the VoicesBasic XML
            XmlDocument voicesBasicXml = new XmlDocument();
            voicesBasicXml.LoadXml(VoicesBasic);
            XmlNode voiceNode = voicesBasicXml.SelectSingleNode($"//Voice[DisplayName='{_selectedVoice}']");

            if (voiceNode != null)
            {
                // Get the StyleList elements for the selected voice
                XmlNodeList styleListNodes = voiceNode.SelectNodes("StyleList");

                // Populate voiceStylesComboBox with the styles
                foreach (XmlNode styleNode in styleListNodes)
                {
                    string style = styleNode.InnerText;
                    _voiceStylesComboBox.Items.Add(style);
                }
            }
            //How to handle the populating
            int selectedIndex = _voiceStylesComboBox.Items.Count > 0 ? 0 : -1; // select proper index for the combo box
            _voiceStylesComboBox.SelectedIndex = selectedIndex; // set the index on it
            ShowOrHideVoiceStylesRow(tableLayoutPanel8, 2, selectedIndex == 0); // show or hide the row which has the styles
        }
        
        /// <summary> Voice Pitch slider </summary>
        private void ScrollBar_Pitch_ValueChanged(object sender, EventArgs e)
        {
            pitch = vScrollBar_pitch.Value.ToString() + "Hz";
            label_pitch.Text = "Pitch = " + pitch;
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

        public static void LoadXMLtoApp(XmlDocument SSMLDocument) // Load all the markup of the XML onto the u.i.
        {
            #region Initialize every mark to its default values
            string styleSSML = "calm";
            string volumeSSML = "80";
            //string pitchSSML = "default";
            string rateSSML = "default";
            string nameValue = "en-US-JennyNeural";
            string langValue = "en-US";
            string localeName = "English (United States)";
            string DisplayName = "Jenny";
            string? value = string.Empty;
            #endregion

            #region Parse the loaded document and acquire the desired values. Add try-catch...
            XmlNamespaceManager nsMgr = new(SSMLDocument.NameTable);
            nsMgr.AddNamespace("speak", "http://www.w3.org/2001/10/synthesis");
            // Find the general default language of the entire document
            XmlNode? speakNode = SSMLDocument.SelectSingleNode("//speak:speak", nsMgr);
            value = speakNode?.Attributes["xml:lang"]?.Value;
            langValue = string.IsNullOrEmpty(value) ? langValue : value;
            // Find the general default Voice of the entire document...
            XmlNode? voiceNode = SSMLDocument.SelectSingleNode("//speak:voice", nsMgr);
            value = voiceNode?.Attributes["name"]?.Value;
            nameValue = string.IsNullOrEmpty(value) ? nameValue : value; // Select the <voice> element and get its "name" attribute value. Need to extract name value from < voice name = "en-US-JennyNeural" >
            // ... and its the Locale name and Display name
            foreach (XmlNode node in VoicesXML.DocumentElement.SelectNodes("Voice"))
            {
                if (node.SelectSingleNode("ShortName").InnerText == nameValue)
                {
                    localeName = node.SelectSingleNode("LocaleName").InnerText;
                    DisplayName = node.SelectSingleNode("DisplayName").InnerText;
                }
            }
            // Acquire the various attributes from Prosody such as... 
            XmlNode? prosodyNode = SSMLDocument.SelectSingleNode("//speak:prosody", nsMgr);
            //  ... the voice style
            styleSSML = prosodyNode?.FirstChild?.Attributes["style"]?.Value ?? "calm"; // if it cannot find the attribute, it returns the default "calm" string
            if (comboBox_VoiceStyles.FindStringExact(styleSSML) == -1) { styleSSML = "calm"; }// if the style is not an option withinn the combo box, it sets it to the default "calm" string
            //  ... the rate
            rate = rateSSML = prosodyNode?.Attributes["rate"]?.Value ?? "default";
            //  ... the pitch 
            //pitch = pitchSSML = prosodyNode?.Attributes["pitch"]?.Value ?? "default";
            pitch = prosodyNode?.Attributes["pitch"]?.Value ?? "default";
            //  ... the volume

            if (prosodyNode.Attributes["volume"] is { Value: string _value })
            {
                volumeSSML = MyRegex().Replace(_value, "");
                if (int.TryParse(volumeSSML, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int parsedVolume))
                { volume = Math.Clamp(parsedVolume, 0, 100); }
            }
            #endregion

            #region set the u.i. and config elements to the parsed values (which will be the default ones on whatever has failed)

            Handler_AudioSynthesis.SetSpeechSynthesisVoiceName(nameValue) ;
            Handler_AudioSynthesis.SetSpeechSynthesisLanguage(langValue); //  Set the Speech Language to whatever is first on the xml file, which is the general one of the entire file

            comboBox_Languages.SelectedItem = localeName;
            comboBox_Voices.SelectedItem = DisplayName;
            comboBox_VoiceStyles.SelectedItem = styleSSML;

            if (comboBox_Pitch.Items.Contains(pitch))
            {
                comboBox_Pitch.SelectedItem = pitch;
                label_pitch.Text = "Pitch : " + pitch;
            }
            else if (pitch.Contains("Hz") && Int32.TryParse(MyRegex().Replace(pitch, string.Empty), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int pitchInt))
            {
                //vScrollBar_pitch.Value = Math.Clamp(pitchInt, -30, 30); // try with scrollbar maxes - mins
                vScrollBar_pitch.Value = Math.Clamp(pitchInt, vScrollBar_pitch.Minimum, vScrollBar_pitch.Maximum);
                comboBox_Pitch.SelectedItem = null;
                //label_pitch.Text = "Pitch = " + pitchInt.ToString();
                label_pitch.Text = "Pitch = " + vScrollBar_pitch.Value.ToString();
            }
            else
            {
                pitch = "default";
                comboBox_Pitch.SelectedItem = pitch;
                label_pitch.Text = "Pitch : " + pitch;
            }

            if (comboBox_Rate.Items.Contains(rate))
            {
                comboBox_Rate.SelectedItem = rate;
                label_rate.Text = "Rate : " + rate;
            }
            else if (rate.Contains('%') && Int32.TryParse(MyRegex().Replace(rate, string.Empty), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int rateInt))
            {
                //vScrollBar_rate.Value = Math.Clamp(rateInt, -30, 30);
                vScrollBar_rate.Value = Math.Clamp(rateInt, vScrollBar_rate.Minimum, vScrollBar_rate.Maximum);
                comboBox_Rate.SelectedItem = null;
                //label3.Text = "Rate = " + rateInt.ToString();
                label_rate.Text = "Rate = " + vScrollBar_rate.Value.ToString();
            }
            else
            {
                rate = "default";
                label_rate.Text = "Rate : " + rate;
                comboBox_Rate.SelectedItem = rate;
            }

            vScrollBar_volume.Value = volume;
            #endregion
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
        /// <summary> Remove any extra unit from the string, keeping solely the number </summary>
        [GeneratedRegex("[^0-9-+]")]
        private static partial Regex MyRegex();        

        #region Menu Bar items

        #region Menu strip buttons
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save_MainTextBoxText_ToFile();
        }
        private void DownloadVoicesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RetrieveAndLoadVoices();
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
        private void PopulateVoicesAndStylesComboBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopulateVoicesAndStylesComboBoxes();
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
        public readonly static string VoicesBasic = @"<Voices>  
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (el-GR, AthinaNeural)</Name>
                                    <DisplayName>Athina</DisplayName>
                                    <LocalName>Αθηνά</LocalName>
                                    <ShortName>el-GR-AthinaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>el-GR</Locale>
                                    <LocaleName>Greek (Greece)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (el-GR, NestorasNeural)</Name>
                                    <DisplayName>Nestoras</DisplayName>
                                    <LocalName>Νέστορας</LocalName>
                                    <ShortName>el-GR-NestorasNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>el-GR</Locale>
                                    <LocaleName>Greek (Greece)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JennyNeural)</Name>
                                    <DisplayName>Jenny</DisplayName>
                                    <LocalName>Jenny</LocalName>
                                    <ShortName>en-US-JennyNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>assistant</StyleList>
                                    <StyleList>chat</StyleList>
                                    <StyleList>customerservice</StyleList>
                                    <StyleList>newscast</StyleList>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>152</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JennyMultilingualNeural)</Name>
                                    <DisplayName>Jenny Multilingual</DisplayName>
                                    <LocalName>Jenny Multilingual</LocalName>
                                    <ShortName>en-US-JennyMultilingualNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SecondaryLocaleList>de-DE</SecondaryLocaleList>
                                    <SecondaryLocaleList>en-AU</SecondaryLocaleList>
                                    <SecondaryLocaleList>en-CA</SecondaryLocaleList>
                                    <SecondaryLocaleList>en-GB</SecondaryLocaleList>
                                    <SecondaryLocaleList>es-ES</SecondaryLocaleList>
                                    <SecondaryLocaleList>es-MX</SecondaryLocaleList>
                                    <SecondaryLocaleList>fr-CA</SecondaryLocaleList>
                                    <SecondaryLocaleList>fr-FR</SecondaryLocaleList>
                                    <SecondaryLocaleList>it-IT</SecondaryLocaleList>
                                    <SecondaryLocaleList>ja-JP</SecondaryLocaleList>
                                    <SecondaryLocaleList>ko-KR</SecondaryLocaleList>
                                    <SecondaryLocaleList>pt-BR</SecondaryLocaleList>
                                    <SecondaryLocaleList>zh-CN</SecondaryLocaleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>190</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, GuyNeural)</Name>
                                    <DisplayName>Guy</DisplayName>
                                    <LocalName>Guy</LocalName>
                                    <ShortName>en-US-GuyNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>newscast</StyleList>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>215</WordsPerMinute>
                                  </Voice>
                                <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AmberNeural)</Name>
                                    <DisplayName>Amber</DisplayName>
                                    <LocalName>Amber</LocalName>
                                    <ShortName>en-US-AmberNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>152</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AnaNeural)</Name>
                                    <DisplayName>Ana</DisplayName>
                                    <LocalName>Ana</LocalName>
                                    <ShortName>en-US-AnaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>135</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AriaNeural)</Name>
                                    <DisplayName>Aria</DisplayName>
                                    <LocalName>Aria</LocalName>
                                    <ShortName>en-US-AriaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>chat</StyleList>
                                    <StyleList>customerservice</StyleList>
                                    <StyleList>narration-professional</StyleList>
                                    <StyleList>newscast-casual</StyleList>
                                    <StyleList>newscast-formal</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>empathetic</StyleList>
                                    <StyleList>angry</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>150</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AshleyNeural)</Name>
                                    <DisplayName>Ashley</DisplayName>
                                    <LocalName>Ashley</LocalName>
                                    <ShortName>en-US-AshleyNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>149</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, BrandonNeural)</Name>
                                    <DisplayName>Brandon</DisplayName>
                                    <LocalName>Brandon</LocalName>
                                    <ShortName>en-US-BrandonNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>156</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, ChristopherNeural)</Name>
                                    <DisplayName>Christopher</DisplayName>
                                    <LocalName>Christopher</LocalName>
                                    <ShortName>en-US-ChristopherNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>149</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, CoraNeural)</Name>
                                    <DisplayName>Cora</DisplayName>
                                    <LocalName>Cora</LocalName>
                                    <ShortName>en-US-CoraNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>146</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, DavisNeural)</Name>
                                    <DisplayName>Davis</DisplayName>
                                    <LocalName>Davis</LocalName>
                                    <ShortName>en-US-DavisNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>chat</StyleList>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>154</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, ElizabethNeural)</Name>
                                    <DisplayName>Elizabeth</DisplayName>
                                    <LocalName>Elizabeth</LocalName>
                                    <ShortName>en-US-ElizabethNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>152</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, EricNeural)</Name>
                                    <DisplayName>Eric</DisplayName>
                                    <LocalName>Eric</LocalName>
                                    <ShortName>en-US-EricNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>147</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JacobNeural)</Name>
                                    <DisplayName>Jacob</DisplayName>
                                    <LocalName>Jacob</LocalName>
                                    <ShortName>en-US-JacobNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>154</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JaneNeural)</Name>
                                    <DisplayName>Jane</DisplayName>
                                    <LocalName>Jane</LocalName>
                                    <ShortName>en-US-JaneNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>154</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JasonNeural)</Name>
                                    <DisplayName>Jason</DisplayName>
                                    <LocalName>Jason</LocalName>
                                    <ShortName>en-US-JasonNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>156</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, MichelleNeural)</Name>
                                    <DisplayName>Michelle</DisplayName>
                                    <LocalName>Michelle</LocalName>
                                    <ShortName>en-US-MichelleNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>154</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, MonicaNeural)</Name>
                                    <DisplayName>Monica</DisplayName>
                                    <LocalName>Monica</LocalName>
                                    <ShortName>en-US-MonicaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>145</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, NancyNeural)</Name>
                                    <DisplayName>Nancy</DisplayName>
                                    <LocalName>Nancy</LocalName>
                                    <ShortName>en-US-NancyNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>149</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, SaraNeural)</Name>
                                    <DisplayName>Sara</DisplayName>
                                    <LocalName>Sara</LocalName>
                                    <ShortName>en-US-SaraNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>157</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, SteffanNeural)</Name>
                                    <DisplayName>Steffan</DisplayName>
                                    <LocalName>Steffan</LocalName>
                                    <ShortName>en-US-SteffanNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>154</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, TonyNeural)</Name>
                                    <DisplayName>Tony</DisplayName>
                                    <LocalName>Tony</LocalName>
                                    <ShortName>en-US-TonyNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>156</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AIGenerate1Neural)</Name>
                                    <DisplayName>AIGenerate1</DisplayName>
                                    <LocalName>AIGenerate1</LocalName>
                                    <ShortName>en-US-AIGenerate1Neural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>Preview</Status>
                                    <WordsPerMinute>135</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AIGenerate2Neural)</Name>
                                    <DisplayName>AIGenerate2</DisplayName>
                                    <LocalName>AIGenerate2</LocalName>
                                    <ShortName>en-US-AIGenerate2Neural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>Preview</Status>
                                    <WordsPerMinute>140</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, RogerNeural)</Name>
                                    <DisplayName>Roger</DisplayName>
                                    <LocalName>Roger</LocalName>
                                    <ShortName>en-US-RogerNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>Preview</Status>
                                    <WordsPerMinute>143</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sk-SK, LukasNeural)</Name>
                                    <DisplayName>Lukas</DisplayName>
                                    <LocalName>Lukáš</LocalName>
                                    <ShortName>sk-SK-LukasNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>sk-SK</Locale>
                                    <LocaleName>Slovak (Slovakia)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sk-SK, ViktoriaNeural)</Name>
                                    <DisplayName>Viktoria</DisplayName>
                                    <LocalName>Viktória</LocalName>
                                    <ShortName>sk-SK-ViktoriaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>sk-SK</Locale>
                                    <LocaleName>Slovak (Slovakia)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sl-SI, PetraNeural)</Name>
                                    <DisplayName>Petra</DisplayName>
                                    <LocalName>Petra</LocalName>
                                    <ShortName>sl-SI-PetraNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>sl-SI</Locale>
                                    <LocaleName>Slovenian (Slovenia)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sl-SI, RokNeural)</Name>
                                    <DisplayName>Rok</DisplayName>
                                    <LocalName>Rok</LocalName>
                                    <ShortName>sl-SI-RokNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>sl-SI</Locale>
                                    <LocaleName>Slovenian (Slovenia)</LocaleName>
                                  </Voice>
                                </Voices>";
        #endregion

    }
}