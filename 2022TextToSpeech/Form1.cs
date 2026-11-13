namespace _Verbalize
{
    using _Verbalize.Properties;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
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
        /// <summary> The voice and settings the user had when they last closed the app. </summary>
        private static UserPreferences savedPreferences = UserPreferences.None;
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
        public static Button button_NarrateMainTextBox, button_SaveTextToFile, button_PopulateVoicesAndStylesComboBoxes, button_LoadText, button_ClearTextBoxAndLoadedFile, button_HideGuiMarkup, button_CreateAudioFromTextFile, button_CreateNarrationSoundFile, button_UpdateTextFile, button_ShowHelp, button_MinimizeWindow, button_MaximizeWindow, button_QuitApplication, button_RetrieveAndLoadVoices;
        public static Image image_AttrColumnButton_Expand, image_AttrColumnButton_Recede;
        public static Icon icon_AppLogo;
        /// <summary> The strip above the text box that shows the app's messages. </summary>
        public static MessageBar message_Bar;
        /// <summary> True while the computer is connected to the internet, as last checked. </summary>
        private static bool isOnline = true;
        /// <summary> Checks the internet connection every few seconds, so buttons that need it follow the connection. </summary>
        private readonly System.Windows.Forms.Timer connectionCheckTimer = new() { Interval = 3000 };
        /// <summary> Shows hints next to the pointer when it rests on a button or on a cut-short status. </summary>
        private static readonly ToolTip statusHints = new() { InitialDelay = 500, AutoPopDelay = 10000, ReshowDelay = 100 };
        /// <summary> What the app is busy doing, shown in the status bar. </summary>
        private static AppActivity currentActivity = AppActivity.None;

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

            //  Remember the voice and settings when the window closes.
            FormClosing += Form1_FormClosing;
        }

        /// <summary> When the window closes, remembers the language, voice, style, rate, pitch and volume for next time. </summary>
        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            //  Write the current choices to the preferences file, carrying on quietly if it cannot be written.
            UserPreferences current = new(comboBox_Languages?.SelectedItem?.ToString(), comboBox_Voices?.SelectedItem?.ToString(), style, rate, pitch, volume);
            UserPreferencesStore.Save(Handler_Data.PreferencesFilePath, current);
        }
        /// <summary> When the window first opens, sets up its controls and fills the language, voice and style lists. </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            //  Give the controls their starting values and the window its icon.
            Assign_AbstractEntities_InitialValues();
            this.Icon = icon_AppLogo; // ActiveForm has not yet been instanced with focus (https://stackoverflow.com/questions/23826059/why-this-works-but-form-activeform-throws-nullrefernceexception).

            //  Fill the lists from the downloaded voice list, or the built-in one if there is none, starting at the language the user had last time or their own.
            savedPreferences = UserPreferencesStore.Load(Handler_Data.PreferencesFilePath);
            LoadVoiceListAndRefresh();

            //  Bring back the voice, style, rate, pitch and volume the user had last time.
            RestoreVoiceSettings(savedPreferences);

            //  Explain how to set up the Azure key if it is missing or its settings file is damaged.
            ShowSpeechSetupMessageIfNeeded();

            //  Watch the internet connection and the speech, and offer only the buttons that can work right now.
            StartWatchingConnection();
            Handler_AudioSynthesis.SpeakingChanged += UpdateActionAvailability;
            UpdateActionAvailability();

            //  Give the buttons their hints.
            AddButtonHints();

            //  Make the top row work as the window's title bar.
            SetUpTitleBar();
        }

        /// <summary> Shows a message where it belongs: warnings and errors in the bar above the text box, outcomes such as "Saved…" on the right of the status bar. </summary>
        /// <param name="message">The message to show.</param>
        public static void ShowUserMessage(UserMessage message)
        {
            //  Put messages that need attention in the bar above the text box.
            if (message.NeedsAttention)
            {
                message_Bar.ShowMessage(message);
                return;
            }

            //  Put outcomes on the right of the status bar, with the full text on hover in case it is cut short, and let screen readers announce it.
            Inform_WithUserMessage(message.Text);
            statusHints.SetToolTip(label_user_messages, message.Text);
            AnnounceToScreenReaders(label_user_messages, message.Text);
        }

        /// <summary> Records what the app is busy doing and shows it on the left of the status bar. </summary>
        /// <param name="activity">The new activity, or None when it has finished.</param>
        public static void SetActivity(AppActivity activity)
        {
            //  Remember the activity and refresh the status.
            currentActivity = activity;
            RefreshSystemStatus();
        }

        /// <summary> Marks an activity as finished, unless another one has started since. </summary>
        /// <param name="activity">The activity that has finished.</param>
        public static void EndActivity(AppActivity activity)
        {
            //  Go back to Ready only if this activity is still the current one.
            if (currentActivity == activity) { SetActivity(AppActivity.None); }
        }

        /// <summary> Shows the app's state on the left of the status bar: what it is doing, the connection and the key. </summary>
        private static void RefreshSystemStatus()
        {
            //  Describe the state and show it.
            Inform_WithSystemMessage(SystemStatus.Describe(isOnline, Handler_Data.GetSpeechCredentials().HasKey, currentActivity));
        }

        /// <summary> Asks screen readers to read out a short text about a control. </summary>
        /// <param name="control">The control the text belongs to.</param>
        /// <param name="text">What to read out.</param>
        private static void AnnounceToScreenReaders(Control control, string text)
        {
            //  Announce the text, ignoring systems that do not support announcements.
            try
            {
                control.AccessibilityObject.RaiseAutomationNotification(System.Windows.Forms.Automation.AutomationNotificationKind.Other,
                    System.Windows.Forms.Automation.AutomationNotificationProcessing.MostRecent, text);
            }
            catch (Exception) { }
        }

        /// <summary> Gives the main buttons a short hint that appears next to the pointer. </summary>
        private void AddButtonHints()
        {
            //  Describe what each main button does, in plain words.
            statusHints.SetToolTip(button_LoadText, ControlHints.LoadText);
            statusHints.SetToolTip(button_ClearTextBoxAndLoadedFile, ControlHints.Clear);
            statusHints.SetToolTip(button_RetrieveAndLoadVoices, ControlHints.DownloadVoices);
            statusHints.SetToolTip(button_PopulateVoicesAndStylesComboBoxes, ControlHints.RefreshVoiceLists);
            statusHints.SetToolTip(button_CreateNarrationSoundFile, ControlHints.SaveSound);
            statusHints.SetToolTip(button_SaveTextToFile, ControlHints.SaveText);
            statusHints.SetToolTip(button_CreateAudioFromTextFile, ControlHints.SaveSoundFromFile);
            statusHints.SetToolTip(button_HideGuiMarkup, ControlHints.ToggleSettingsPanel);
            statusHints.SetToolTip(button_UpdateTextFile, ControlHints.ComingLater);
        }

        /// <summary> Tells the user how to set up their Azure key when none is set or the settings file cannot be read. </summary>
        private static void ShowSpeechSetupMessageIfNeeded()
        {
            //  Show the setup message only if there is something to fix.
            UserMessage? setupMessage = UserMessages.SpeechSetup(Handler_Data.GetSpeechCredentials(), Handler_Data.SettingsFilePath);
            if (setupMessage != null) { ShowUserMessage(setupMessage); }
        }

        /// <summary> Checks the internet connection now and then every few seconds, telling the user while they are offline. </summary>
        private void StartWatchingConnection()
        {
            //  Check the connection now, and say so if there is none.
            isOnline = Handler_Connectivity.IsConnectedToInternet();
            if (!isOnline) { ShowUserMessage(UserMessages.Offline()); }

            //  Keep checking every few seconds.
            connectionCheckTimer.Tick += ConnectionCheckTimer_Tick;
            connectionCheckTimer.Start();
        }

        /// <summary> Every few seconds, notices when the computer goes offline or comes back, updating the message and the buttons. </summary>
        private void ConnectionCheckTimer_Tick(object? sender, EventArgs e)
        {
            //  Do nothing unless the connection has changed.
            bool online = Handler_Connectivity.IsConnectedToInternet();
            if (online == isOnline) { return; }
            isOnline = online;

            //  When back online, remove the offline message and bring back the key setup message if it still applies.
            if (online)
            {
                message_Bar.ClearTopic(UserMessages.OfflineTopic);
                if (!message_Bar.Visible) { ShowSpeechSetupMessageIfNeeded(); }
            }
            //  When offline, tell the user what is paused.
            else { ShowUserMessage(UserMessages.Offline()); }

            //  Turn the buttons that need the internet on or off.
            UpdateActionAvailability();
        }

        /// <summary> When the text changes, offers Speak and Export only if there is text to use. </summary>
        private void TextBox_Main_TextChanged(object? sender, EventArgs e)
        {
            //  Withdraw any offer to undo a Clear or Load, since the text has moved on.
            message_Bar.ClearTopic(UserMessages.UndoTopic);

            //  Turn the buttons that need text on or off.
            UpdateActionAvailability();
        }

        /// <summary> Everything a Clear or a Load replaces, kept so Undo can put it back. </summary>
        /// <param name="Text">The text box's text.</param>
        /// <param name="LoadedFilePath">The loaded file's path, as the file handler knows it.</param>
        /// <param name="FormLoadedFilePath">The loaded file's path, as the window knows it.</param>
        /// <param name="FileLabel">The file name shown above the text box.</param>
        /// <param name="FileLabelVisible">True if that file name was shown.</param>
        /// <param name="MenuFileLabel">The file name shown in the menu bar.</param>
        /// <param name="MenuFileLabelVisible">True if that file name was shown.</param>
        /// <param name="WindowTitle">The window's title.</param>
        private sealed record TextSnapshot(string Text, string LoadedFilePath, string FormLoadedFilePath, string FileLabel, bool FileLabelVisible, string MenuFileLabel, bool MenuFileLabelVisible, string WindowTitle);

        /// <summary> Records the text and the loaded file's details, before they are replaced. </summary>
        /// <returns>The snapshot.</returns>
        private static TextSnapshot TakeTextSnapshot()
        {
            //  Copy everything a Clear or a Load changes.
            return new TextSnapshot(textBox_Main_Single.Text, Handler_File.LoadedFilePath, locationLoadedFile,
                label_FileName.Text, label_FileName.Visible, label_FileName_in_menustrip.Text, label_FileName_in_menustrip.Visible,
                textBox_Main_Single.FindForm()?.Text ?? applicationBrandName);
        }

        /// <summary> Puts back the text and the loaded file's details from a snapshot. </summary>
        /// <param name="snapshot">What to put back.</param>
        private static void RestoreTextSnapshot(TextSnapshot snapshot)
        {
            //  Restore the file details first, then the text, so the window matches the text again.
            Handler_File.LoadedFilePath = snapshot.LoadedFilePath;
            locationLoadedFile = snapshot.FormLoadedFilePath;
            label_FileName.Text = snapshot.FileLabel;
            label_FileName.Visible = snapshot.FileLabelVisible;
            label_FileName_in_menustrip.Text = snapshot.MenuFileLabel;
            label_FileName_in_menustrip.Visible = snapshot.MenuFileLabelVisible;
            if (textBox_Main_Single.FindForm() is Form window) { window.Text = snapshot.WindowTitle; }
            textBox_Main_Single.Text = snapshot.Text;

            //  Confirm it in the status bar.
            ShowUserMessage(new UserMessage(MessageSeverity.Success, "Brought back the previous text."));
        }

        /// <summary> Shows a message above the text box with an Undo button that puts back what was replaced, if anything was. </summary>
        /// <param name="message">What happened.</param>
        /// <param name="snapshot">What was there before.</param>
        private static void OfferUndo(UserMessage message, TextSnapshot snapshot)
        {
            //  Offer Undo only when text was actually replaced.
            if (!UserMessages.IsUndoWorthOffering(snapshot.Text)) { return; }

            //  Show the message with its Undo button above the text box, where the user is looking.
            message_Bar.ShowMessage(message, "&Undo", () => RestoreTextSnapshot(snapshot));
        }

        /// <summary> Turns each button and menu item on or off, depending on whether there is text, an internet connection and an Azure key. </summary>
        private void UpdateActionAvailability()
        {
            //  Work out what the app can do right now.
            AppConditions conditions = new(ActionAvailability.HasSpeakableText(textBox_Main_Single.Text), isOnline, Handler_Data.GetSpeechCredentials().HasKey);
            bool canSpeakText = ActionAvailability.CanSpeakText(conditions);
            bool canReachAzure = ActionAvailability.CanReachAzure(conditions);

            //  Make the Speak/Stop button read Stop while speaking, and Speak otherwise.
            SpeakButtonState speakButton = SpeakButton.Describe(Handler_AudioSynthesis.IsSpeaking, conditions);
            button_NarrateMainTextBox.Text = speakButton.Text;
            button_NarrateMainTextBox.Enabled = speakButton.Enabled;
            statusHints.SetToolTip(button_NarrateMainTextBox, speakButton.Hint);

            //  Exporting the text box needs text as well as Azure.
            button_CreateNarrationSoundFile.Enabled = soundToolStripMenuItem.Enabled = canSpeakText;

            //  Exporting a file and downloading voices need only Azure.
            button_CreateAudioFromTextFile.Enabled = batchAudioToolStripMenuItem.Enabled = canReachAzure;
            button_RetrieveAndLoadVoices.Enabled = downloadVoicesListToolStripMenuItem.Enabled = canReachAzure;

            //  Show the connection and key in the status bar too.
            RefreshSystemStatus();
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
            message_Bar = messageBar1;

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
            button_SaveTextToFile = button7;
            button_HideGuiMarkup = button10;
            button_NarrateMainTextBox = button12;
        }
        public void Assign_AbstractEntities_InitialValues()
        {
            label_FileName.Visible = label_FileName_in_menustrip.Visible = false;
            label_FileName.Text = string.Empty;
            //  Start the status bar with the app's state on the left and no outcome on the right yet.
            RefreshSystemStatus();
            label_user_messages.Text = string.Empty;

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
        /// <summary> When the Refresh voices button is clicked, fills the lists from the downloaded voice list. </summary>
        private async void Button_PopulateVoicesAndStylesComboBoxes_Click(object sender, EventArgs e)
        {
            //  Fill the lists, downloading the voice list first if needed.
            await PopulateVoicesAndStylesComboBoxesAsync();
        }
        /// <summary> When the Speak/Stop button is clicked, stops the speech if it is playing, or otherwise speaks the main text box aloud. </summary>
        private async void Button_NarrateMainTextBox_Click(object sender, EventArgs e)
        {
            //  Stop or speak, depending on what the button offers right now.
            await SpeakOrStopAsync();
        }

        /// <summary> Stops the speech if it is playing; otherwise speaks the main text box, but only when that can work. </summary>
        /// <returns>A task that finishes once the speech has stopped, or has ended, been stopped or failed.</returns>
        private static async Task SpeakOrStopAsync()
        {
            //  Stop the speech if it is playing.
            if (Handler_AudioSynthesis.IsSpeaking)
            {
                await MuteSpokenNarrationAsync();
                return;
            }

            //  Otherwise speak, if the Speak button is available.
            if (button_NarrateMainTextBox.Enabled) { await NarrateMainTextBoxAsync(); }
        }

        /// <summary> Handles the keyboard shortcuts before any control sees them: Ctrl+Enter speaks and Esc stops the speech. </summary>
        /// <param name="msg">The keyboard message.</param>
        /// <param name="keyData">The key pressed, with any modifier keys.</param>
        /// <returns>True if the shortcut was handled.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //  Speak with Ctrl+Enter, which would otherwise add a line break to the text.
            if (keyData == (Keys.Control | Keys.Enter) && !Handler_AudioSynthesis.IsSpeaking)
            {
                _ = SpeakOrStopAsync();
                return true;
            }

            //  Stop with Esc while speaking, leaving Esc to work as usual otherwise.
            if (keyData == Keys.Escape && Handler_AudioSynthesis.IsSpeaking)
            {
                _ = MuteSpokenNarrationAsync();
                return true;
            }

            //  Let every other key work as usual.
            return base.ProcessCmdKey(ref msg, keyData);
        }
        /// <summary> When the Download button is clicked, downloads the latest voice list and fills the lists from it. </summary>
        private async void Button_RetrieveAndLoadVoices_Click(object sender, EventArgs e)
        {
            //  Download the voice list and refill the lists.
            await RetrieveAndLoadVoicesAsync();
        }
        /// <summary> Lets the user load a file into the text box, offering Undo if it replaced text. </summary>
        public static void LoadText()
        {
            //  Keep what is there now, then load the file.
            TextSnapshot before = TakeTextSnapshot();
            bool loaded = Handler_File.Load_Text(label_FileName, Form1.ActiveForm, applicationBrandName, label_FileName_in_menustrip, textBox_Main_Single);

            //  Offer to bring back the previous text if a file replaced it.
            if (loaded) { OfferUndo(UserMessages.TextReplacedByFile(Path.GetFileName(Handler_File.LoadedFilePath)), before); }
        }
        #endregion

        #region Button functions.

        /// <summary> Maximises the window, or restores it to the size it had before if it is already maximised. </summary>
        public void MaximizeWindow()
        {
            //  Switch between filling the screen and the earlier size; the button and the View menu follow when the size changes.
            WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
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
            //  Keep what is there now, so it can be brought back.
            TextSnapshot before = TakeTextSnapshot();

            //  Empty the text box and the file name.
            locationLoadedFile = string.Empty;
            textBox_Main_Single.Clear();
            label_FileName.Text = string.Empty;
            label_FileName.Visible = false; // remove this when implement the label's automatic behaviour through events

            //  Offer to bring the text back.
            OfferUndo(UserMessages.TextCleared(), before);
        }
        /// <summary> Stops the speech playing aloud, if any. </summary>
        /// <returns>A task that finishes once the speech has stopped.</returns>
        public static Task MuteSpokenNarrationAsync()
        {
            //  Ask the speech handler to stop speaking.
            return Handler_AudioSynthesis.StopSpeakingAsync();
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
        /// <summary> Speaks the main text box's selected text aloud, or all of it if none is selected. </summary>
        /// <returns>A task that finishes once the speech has ended, been stopped, or the user has been told it failed.</returns>
        public static Task NarrateMainTextBoxAsync()
        {
            //  Ask the speech handler to speak the text box.
            return Handler_AudioSynthesis.SpeakFromTextBoxAsync(textBox_Main_Single);
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
            //  Show the download in the status bar while it runs.
            SetActivity(AppActivity.DownloadingVoices);
            try
            {
                //  Ask Azure for the list of voices in the app's region.
                using HttpClient client = new HttpClient();
                string subscriptionKey = Handler_Data.GetTheSubscriptionKey();
                string serverLocation = Handler_Data.GetTheServerLocation();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string listVoicesLocationURL = "https://" + serverLocation + ".tts.speech.microsoft.com/cognitiveservices/voices/list";
                HttpResponseMessage response = await client.GetAsync(listVoicesLocationURL);

                //  Save the list if Azure sent it, and confirm it to the user.
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Handler_Data.Convert_JSONtoXML_AndSaveToDisk(responseData);
                    ShowUserMessage(UserMessages.VoiceListDownloaded());
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
            //  Clear the download from the status bar however it ended.
            finally
            {
                EndActivity(AppActivity.DownloadingVoices);
            }
        }

        /// <summary> Tells the user that the voice list could not be downloaded, and why. </summary>
        /// <param name="reason">Why the download failed.</param>
        private static void ShowVoiceListDownloadError(string reason)
        {
            //  Show the reason in the message bar.
            ShowUserMessage(UserMessages.VoiceListDownloadFailed(reason));
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
                //  Note the language shown now, or else the one the user had last time, so the list can come back to it.
                string? languageToKeep = comboBox_Languages.SelectedItem?.ToString() ?? savedPreferences.LanguageName;

                //  Replace the Languages list with each language once, selecting the first.
                comboBox_Languages.Items.Clear();
                foreach (string? localeName in VoiceCatalog.GetLocaleNames(VoicesXML))
                {
                    comboBox_Languages.Items.Add(localeName!);
                    comboBox_Languages.SelectedIndex = 0;
                }

                //  Move to the noted language, or else the language of the user's Windows regional format, so they do not start at the top of the list.
                string? startLanguage = StartLanguage.Choose(VoiceCatalog.GetLanguages(VoicesXML), languageToKeep, CultureInfo.CurrentCulture.Name);
                if (startLanguage != null) { comboBox_Languages.SelectedItem = startLanguage; }
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

            //  Keep the language's code in the hidden label and the list's hint, and use it as the speech language.
            if (voicesInLocale.Locale != null)
            {
                this.label6.Text = voicesInLocale.Locale;
                if (comboBox_Languages != null) { statusHints.SetToolTip(comboBox_Languages, ControlHints.LanguageCode(voicesInLocale.Locale)); }
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
            //  Find the chosen voice among the chosen language's voices.
            string selectedDisplayName = comboBox_Voices?.SelectedItem?.ToString() ?? string.Empty;
            string? selectedLocaleName = comboBox_Languages?.SelectedItem?.ToString();
            VoiceIdentity? voice = VoiceCatalog.FindVoiceByDisplayName(VoicesXML, selectedDisplayName, selectedLocaleName);

            //  Use it as the speaking voice and show its local name and gender.
            if (voice != null)
            {
                Handler_AudioSynthesis.SetSpeechSynthesisVoiceName(voice.ShortName);
                label7.Text = " - " + voice.LocalName + " (" + voice.Gender + ")";
            }

            //  List the speaking styles that the chosen voice supports.
            PopulateSingleVoiceStylesComboBox(comboBox_VoiceStyles, comboBox_Voices.SelectedItem?.ToString(), selectedLocaleName);
        }
        #endregion
        /// <summary> Lists the speaking styles of a voice, hiding the styles row if the voice has none. </summary>
        /// <param name="_voiceStylesComboBox">The list to fill with the styles.</param>
        /// <param name="_selectedVoice">The display name of the voice.</param>
        /// <param name="_selectedLocaleName">The voice's language, or null to look in every language.</param>
        private void PopulateSingleVoiceStylesComboBox(ComboBox _voiceStylesComboBox, string? _selectedVoice, string? _selectedLocaleName)
        {
            //  Empty the styles list, including any text showing in it.
            _voiceStylesComboBox.Items.Clear();
            _voiceStylesComboBox.Text = string.Empty;

            //  Look up the voice's styles in the voice list in use and add them to the list.
            foreach (string voiceStyle in VoiceCatalog.GetStyles(VoicesXML, _selectedVoice, _selectedLocaleName))
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

            //  Show the rate, pitch and volume in their controls.
            ShowRatePitchAndVolume();
        }

        /// <summary> Shows the current rate, pitch and volume in their lists, sliders and labels, turning anything unreadable into "default". </summary>
        private static void ShowRatePitchAndVolume()
        {
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

        /// <summary> Brings back the voice, style, rate, pitch and volume the user had last time, skipping any that the lists no longer offer. </summary>
        /// <param name="preferences">The remembered settings.</param>
        private static void RestoreVoiceSettings(UserPreferences preferences)
        {
            //  Select the remembered voice if the chosen language still has it.
            if (preferences.VoiceName != null && comboBox_Voices.Items.Contains(preferences.VoiceName)) { comboBox_Voices.SelectedItem = preferences.VoiceName; }

            //  Select the remembered style if the voice offers it.
            if (preferences.Style != null && comboBox_VoiceStyles.FindStringExact(preferences.Style) != -1) { comboBox_VoiceStyles.SelectedItem = preferences.Style; }

            //  Take the remembered rate, pitch and volume, keeping the volume within the slider's range.
            if (preferences.Rate != null) { rate = preferences.Rate; }
            if (preferences.Pitch != null) { pitch = preferences.Pitch; }
            if (preferences.Volume is int rememberedVolume) { volume = Math.Clamp(rememberedVolume, vScrollBar_volume.Minimum, vScrollBar_volume.Maximum); }

            //  Show them in their controls.
            ShowRatePitchAndVolume();
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
        /// <summary> When "Refresh voices" is chosen in the menu, fills the lists from the downloaded voice list. </summary>
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
        /// <summary> When Help is clicked, shows the keyboard shortcuts and how to set up the Speech key in the message bar. </summary>
        private void Button_ShowHelp_Click(object sender, EventArgs e)
        {
            //  Show the help until the user closes it.
            message_Bar.ShowMessage(UserMessages.Help());
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