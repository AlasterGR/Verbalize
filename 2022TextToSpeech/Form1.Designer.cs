﻿namespace _2022TextToSpeech
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            openFileDialog1 = new OpenFileDialog();
            bttn3_LoadText = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            vScrollBar1 = new VScrollBar();
            vScrollBar2 = new VScrollBar();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            button8 = new Button();
            button9 = new Button();
            label6 = new Label();
            label7 = new Label();
            comboBox3 = new ComboBox();
            label8 = new Label();
            button2 = new Button();
            button12 = new Button();
            label10 = new Label();
            label11 = new Label();
            button1 = new Button();
            button5 = new Button();
            cmbBx_SelectSavedSoundFormat = new ComboBox();
            panel1 = new Panel();
            panel2 = new Panel();
            comboBox4 = new ComboBox();
            comboBox5 = new ComboBox();
            label9 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel13 = new TableLayoutPanel();
            label13 = new Label();
            label14 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel15 = new TableLayoutPanel();
            radioButton4 = new RadioButton();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            button6 = new Button();
            tableLayoutPanel16 = new TableLayoutPanel();
            label16 = new Label();
            tableLayoutPanel14 = new TableLayoutPanel();
            radioButton3 = new RadioButton();
            radioButton5 = new RadioButton();
            radioButton6 = new RadioButton();
            button7 = new Button();
            button4 = new Button();
            panel5 = new Panel();
            button10 = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            tableLayoutPanel5 = new TableLayoutPanel();
            vScrollBar3 = new VScrollBar();
            tableLayoutPanel7 = new TableLayoutPanel();
            tableLayoutPanel8 = new TableLayoutPanel();
            panel4 = new Panel();
            tableLayoutPanel10 = new TableLayoutPanel();
            tableLayoutPanel9 = new TableLayoutPanel();
            panel3 = new Panel();
            tableLayoutPanel11 = new TableLayoutPanel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel13.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel15.SuspendLayout();
            tableLayoutPanel16.SuspendLayout();
            tableLayoutPanel14.SuspendLayout();
            panel5.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            tableLayoutPanel10.SuspendLayout();
            tableLayoutPanel9.SuspendLayout();
            tableLayoutPanel11.SuspendLayout();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.RestoreDirectory = true;
            // 
            // bttn3_LoadText
            // 
            bttn3_LoadText.Dock = DockStyle.Fill;
            bttn3_LoadText.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            bttn3_LoadText.Location = new Point(10, 8);
            bttn3_LoadText.Margin = new Padding(5, 4, 5, 4);
            bttn3_LoadText.Name = "bttn3_LoadText";
            bttn3_LoadText.Size = new Size(71, 58);
            bttn3_LoadText.TabIndex = 2;
            bttn3_LoadText.Text = "&Load text";
            bttn3_LoadText.UseVisualStyleBackColor = true;
            bttn3_LoadText.Click += Bttn3_LoadText_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Calibri", 27.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ImageAlign = ContentAlignment.MiddleLeft;
            label1.Location = new Point(5, 4);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(308, 45);
            label1.TabIndex = 3;
            label1.Text = "Selected File name";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            label1.TextChanged += Label1_TextChanged;
            label1.Paint += label1_Paint;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.AliceBlue;
            textBox1.Cursor = Cursors.IBeam;
            textBox1.Dock = DockStyle.Fill;
            textBox1.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.HideSelection = false;
            textBox1.Location = new Point(5, 55);
            textBox1.Margin = new Padding(0);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(519, 504);
            textBox1.TabIndex = 8;
            // 
            // vScrollBar1
            // 
            vScrollBar1.Dock = DockStyle.Left;
            vScrollBar1.LargeChange = 1;
            vScrollBar1.Location = new Point(0, 0);
            vScrollBar1.Maximum = 30;
            vScrollBar1.Minimum = -30;
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(86, 238);
            vScrollBar1.TabIndex = 11;
            vScrollBar1.ValueChanged += vScrollBar1_ValueChanged;
            // 
            // vScrollBar2
            // 
            vScrollBar2.Dock = DockStyle.Left;
            vScrollBar2.LargeChange = 1;
            vScrollBar2.Location = new Point(0, 0);
            vScrollBar2.Maximum = 30;
            vScrollBar2.Minimum = -30;
            vScrollBar2.Name = "vScrollBar2";
            vScrollBar2.Size = new Size(86, 238);
            vScrollBar2.TabIndex = 12;
            vScrollBar2.ValueChanged += vScrollBar2_ValueChanged;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(172, 4);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(167, 29);
            label2.TabIndex = 13;
            label2.Text = "Pitch";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(5, 4);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(167, 29);
            label3.TabIndex = 14;
            label3.Text = "Rate";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(5, 4);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(72, 49);
            label4.TabIndex = 15;
            label4.Text = "Language";
            label4.TextAlign = ContentAlignment.TopCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(10, 53);
            label5.Margin = new Padding(5, 0, 5, 0);
            label5.Name = "label5";
            label5.Size = new Size(62, 49);
            label5.TabIndex = 16;
            label5.Text = "Voice";
            label5.TextAlign = ContentAlignment.TopCenter;
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(77, 4);
            comboBox1.Margin = new Padding(0);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(191, 27);
            comboBox1.TabIndex = 17;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            // 
            // comboBox2
            // 
            comboBox2.Dock = DockStyle.Fill;
            comboBox2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(77, 53);
            comboBox2.Margin = new Padding(0);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(191, 27);
            comboBox2.TabIndex = 18;
            comboBox2.SelectedIndexChanged += ComboBox2_SelectedIndexChanged;
            // 
            // button8
            // 
            button8.Dock = DockStyle.Fill;
            button8.Location = new Point(10, 140);
            button8.Margin = new Padding(5, 4, 5, 4);
            button8.Name = "button8";
            button8.Size = new Size(71, 137);
            button8.TabIndex = 20;
            button8.Text = "Download Voices list";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Dock = DockStyle.Fill;
            button9.Location = new Point(10, 285);
            button9.Margin = new Padding(5, 4, 5, 4);
            button9.Name = "button9";
            button9.Size = new Size(71, 137);
            button9.TabIndex = 21;
            button9.Text = "Populate the combo boxes";
            button9.UseVisualStyleBackColor = true;
            button9.Click += Button9_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(279, 4);
            label6.Margin = new Padding(11, 0, 11, 0);
            label6.MaximumSize = new Size(347, 0);
            label6.Name = "label6";
            label6.Size = new Size(138, 49);
            label6.TabIndex = 22;
            label6.Text = "Locale";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(279, 53);
            label7.Margin = new Padding(11, 0, 11, 0);
            label7.MaximumSize = new Size(347, 0);
            label7.Name = "label7";
            label7.Size = new Size(138, 49);
            label7.TabIndex = 23;
            label7.Text = "Local name\r\n(Gender)";
            // 
            // comboBox3
            // 
            comboBox3.Dock = DockStyle.Fill;
            comboBox3.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "advertisement_upbeat", "affectionate", "angry", "assistant", "calm", "chat", "cheerful", "customerservice", "depressed", "disgruntled", "documentary-narration", "embarrassed", "empathetic", "envious", "excited", "fearful", "friendly", "gentle", "hopeful", "lyrical", "narration-professional", "narration-relaxed", "newscast", "newscast-casual", "newscast-formal", "poetry-reading", "sad", "serious", "shouting", "sports_commentary", "sports_commentary_excited", "whispering", "terrified", "unfriendly" });
            comboBox3.Location = new Point(77, 102);
            comboBox3.Margin = new Padding(0);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(191, 27);
            comboBox3.TabIndex = 25;
            comboBox3.SelectedIndexChanged += voiceStylesComboBox_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Fill;
            label8.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(16, 102);
            label8.Margin = new Padding(11, 0, 11, 0);
            label8.Name = "label8";
            label8.Size = new Size(50, 51);
            label8.TabIndex = 26;
            label8.Text = "Style";
            label8.TextAlign = ContentAlignment.TopCenter;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button2.ImeMode = ImeMode.NoControl;
            button2.Location = new Point(5, 523);
            button2.Margin = new Padding(5, 4, 5, 4);
            button2.Name = "button2";
            button2.Size = new Size(433, 36);
            button2.TabIndex = 28;
            button2.Text = "&Update this file";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Button2_Click;
            // 
            // button12
            // 
            button12.BackColor = Color.Transparent;
            button12.Dock = DockStyle.Fill;
            button12.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button12.Location = new Point(8, 72);
            button12.Margin = new Padding(3, 4, 3, 4);
            button12.Name = "button12";
            button12.Padding = new Padding(3, 4, 3, 4);
            button12.Size = new Size(105, 37);
            button12.TabIndex = 30;
            button12.Text = "Sp&eak";
            button12.UseVisualStyleBackColor = false;
            button12.Click += Button12_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.Dock = DockStyle.Fill;
            label10.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(339, 4);
            label10.Margin = new Padding(0);
            label10.Name = "label10";
            label10.Size = new Size(89, 29);
            label10.TabIndex = 33;
            label10.Text = "Volume";
            label10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.Transparent;
            label11.Dock = DockStyle.Fill;
            label11.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(339, 33);
            label11.Margin = new Padding(0);
            label11.Name = "label11";
            label11.Size = new Size(89, 29);
            label11.TabIndex = 34;
            label11.Text = "100";
            label11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Dock = DockStyle.Fill;
            button1.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(10, 74);
            button1.Margin = new Padding(5, 4, 5, 4);
            button1.Name = "button1";
            button1.Size = new Size(71, 58);
            button1.TabIndex = 35;
            button1.Text = "&Clear";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // button5
            // 
            button5.BackgroundImage = (Image)resources.GetObject("button5.BackgroundImage");
            button5.BackgroundImageLayout = ImageLayout.Zoom;
            button5.Dock = DockStyle.Fill;
            button5.Location = new Point(5, 4);
            button5.Margin = new Padding(5, 4, 5, 4);
            button5.Name = "button5";
            button5.Size = new Size(91, 20);
            button5.TabIndex = 37;
            button5.Text = " ";
            button5.UseVisualStyleBackColor = true;
            button5.Click += Button5_Click;
            // 
            // cmbBx_SelectSavedSoundFormat
            // 
            cmbBx_SelectSavedSoundFormat.Dock = DockStyle.Top;
            cmbBx_SelectSavedSoundFormat.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            cmbBx_SelectSavedSoundFormat.FormattingEnabled = true;
            cmbBx_SelectSavedSoundFormat.Items.AddRange(new object[] { "mp3", "wav", "None" });
            cmbBx_SelectSavedSoundFormat.Location = new Point(10, 37);
            cmbBx_SelectSavedSoundFormat.Margin = new Padding(5, 4, 5, 4);
            cmbBx_SelectSavedSoundFormat.Name = "cmbBx_SelectSavedSoundFormat";
            cmbBx_SelectSavedSoundFormat.Size = new Size(91, 31);
            cmbBx_SelectSavedSoundFormat.TabIndex = 39;
            cmbBx_SelectSavedSoundFormat.SelectedIndexChanged += soundtypesComboBox_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(vScrollBar1);
            panel1.Dock = DockStyle.Fill;
            panel1.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            panel1.Location = new Point(5, 62);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(167, 238);
            panel1.TabIndex = 40;
            panel1.Paint += panel_Paint;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(vScrollBar2);
            panel2.Dock = DockStyle.Fill;
            panel2.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            panel2.Location = new Point(172, 62);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(167, 238);
            panel2.TabIndex = 41;
            panel2.Paint += panel_Paint;
            // 
            // comboBox4
            // 
            comboBox4.Dock = DockStyle.Fill;
            comboBox4.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "x-slow", "slow", "medium", "fast", "x-fast", "default" });
            comboBox4.Location = new Point(12, 33);
            comboBox4.Margin = new Padding(7, 0, 7, 0);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(153, 27);
            comboBox4.TabIndex = 42;
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            // 
            // comboBox5
            // 
            comboBox5.Dock = DockStyle.Fill;
            comboBox5.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox5.FormattingEnabled = true;
            comboBox5.Items.AddRange(new object[] { "x-low", "low", "medium", "high", "x-high", "default" });
            comboBox5.Location = new Point(179, 33);
            comboBox5.Margin = new Padding(7, 0, 7, 0);
            comboBox5.Name = "comboBox5";
            comboBox5.Size = new Size(153, 27);
            comboBox5.TabIndex = 43;
            comboBox5.SelectedIndexChanged += comboBox5_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = DockStyle.Fill;
            label9.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(10, 8);
            label9.Margin = new Padding(5, 4, 5, 4);
            label9.Name = "label9";
            label9.Size = new Size(91, 21);
            label9.TabIndex = 44;
            label9.Text = "Soundfile type :";
            label9.TextAlign = ContentAlignment.BottomCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(192, 255, 192);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel13, 0, 0);
            tableLayoutPanel1.Controls.Add(button12, 0, 1);
            tableLayoutPanel1.Controls.Add(label14, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 3);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel16, 0, 4);
            tableLayoutPanel1.Controls.Add(button4, 0, 5);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(1131, 4);
            tableLayoutPanel1.Margin = new Padding(5, 4, 5, 4);
            tableLayoutPanel1.MinimumSize = new Size(121, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(5, 4, 5, 4);
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 207F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 175F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 13F));
            tableLayoutPanel1.Size = new Size(121, 563);
            tableLayoutPanel1.TabIndex = 45;
            // 
            // tableLayoutPanel13
            // 
            tableLayoutPanel13.BackColor = Color.FromArgb(255, 224, 192);
            tableLayoutPanel13.ColumnCount = 1;
            tableLayoutPanel13.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel13.Controls.Add(label13, 0, 1);
            tableLayoutPanel13.Controls.Add(button5, 0, 0);
            tableLayoutPanel13.Dock = DockStyle.Fill;
            tableLayoutPanel13.Location = new Point(10, 8);
            tableLayoutPanel13.Margin = new Padding(5, 4, 5, 4);
            tableLayoutPanel13.Name = "tableLayoutPanel13";
            tableLayoutPanel13.RowCount = 2;
            tableLayoutPanel13.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel13.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel13.Size = new Size(101, 56);
            tableLayoutPanel13.TabIndex = 1;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Dock = DockStyle.Fill;
            label13.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label13.ImeMode = ImeMode.NoControl;
            label13.Location = new Point(5, 32);
            label13.Margin = new Padding(5, 4, 5, 4);
            label13.Name = "label13";
            label13.RightToLeft = RightToLeft.No;
            label13.Size = new Size(91, 20);
            label13.TabIndex = 38;
            label13.Text = "&Mute";
            label13.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.BackColor = Color.FromArgb(192, 255, 192);
            label14.Dock = DockStyle.Fill;
            label14.Font = new Font("Calibri", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label14.ImeMode = ImeMode.NoControl;
            label14.Location = new Point(10, 117);
            label14.Margin = new Padding(5, 4, 5, 4);
            label14.Name = "label14";
            label14.Size = new Size(101, 1);
            label14.TabIndex = 47;
            label14.Text = "Export spoken text";
            label14.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.FromArgb(192, 255, 192);
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(cmbBx_SelectSavedSoundFormat, 0, 1);
            tableLayoutPanel2.Controls.Add(label9, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel15, 0, 2);
            tableLayoutPanel2.Controls.Add(button6, 0, 3);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(5, 121);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.Padding = new Padding(5, 4, 5, 4);
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 14.5728645F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 18.5929642F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 44.7236176F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 21.60804F));
            tableLayoutPanel2.Size = new Size(111, 207);
            tableLayoutPanel2.TabIndex = 46;
            // 
            // tableLayoutPanel15
            // 
            tableLayoutPanel15.ColumnCount = 1;
            tableLayoutPanel15.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel15.Controls.Add(radioButton4, 0, 2);
            tableLayoutPanel15.Controls.Add(radioButton1, 0, 0);
            tableLayoutPanel15.Controls.Add(radioButton2, 0, 1);
            tableLayoutPanel15.Dock = DockStyle.Fill;
            tableLayoutPanel15.Location = new Point(8, 74);
            tableLayoutPanel15.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel15.Name = "tableLayoutPanel15";
            tableLayoutPanel15.RowCount = 3;
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel15.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel15.Size = new Size(95, 81);
            tableLayoutPanel15.TabIndex = 50;
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(3, 58);
            radioButton4.Margin = new Padding(3, 4, 3, 4);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(61, 19);
            radioButton4.TabIndex = 48;
            radioButton4.TabStop = true;
            radioButton4.Text = "None";
            radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(3, 4);
            radioButton1.Margin = new Padding(3, 4, 3, 4);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(55, 19);
            radioButton1.TabIndex = 46;
            radioButton1.TabStop = true;
            radioButton1.Text = "mp3";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(3, 31);
            radioButton2.Margin = new Padding(3, 4, 3, 4);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(53, 19);
            radioButton2.TabIndex = 47;
            radioButton2.TabStop = true;
            radioButton2.Text = "wav";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Dock = DockStyle.Fill;
            button6.Font = new Font("Calibri", 10F, FontStyle.Regular, GraphicsUnit.Point);
            button6.Location = new Point(8, 163);
            button6.Margin = new Padding(3, 4, 3, 4);
            button6.Name = "button6";
            button6.Size = new Size(95, 36);
            button6.TabIndex = 51;
            button6.Text = "Export Sound";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // tableLayoutPanel16
            // 
            tableLayoutPanel16.ColumnCount = 1;
            tableLayoutPanel16.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel16.Controls.Add(label16, 0, 0);
            tableLayoutPanel16.Controls.Add(tableLayoutPanel14, 0, 1);
            tableLayoutPanel16.Controls.Add(button7, 0, 2);
            tableLayoutPanel16.Dock = DockStyle.Fill;
            tableLayoutPanel16.Location = new Point(8, 332);
            tableLayoutPanel16.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel16.Name = "tableLayoutPanel16";
            tableLayoutPanel16.RowCount = 3;
            tableLayoutPanel16.RowStyles.Add(new RowStyle(SizeType.Percent, 17.5531921F));
            tableLayoutPanel16.RowStyles.Add(new RowStyle(SizeType.Percent, 54.7872353F));
            tableLayoutPanel16.RowStyles.Add(new RowStyle(SizeType.Percent, 26.88172F));
            tableLayoutPanel16.Size = new Size(105, 167);
            tableLayoutPanel16.TabIndex = 50;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Dock = DockStyle.Fill;
            label16.Font = new Font("Calibri", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label16.Location = new Point(3, 4);
            label16.Margin = new Padding(3, 4, 3, 4);
            label16.Name = "label16";
            label16.Padding = new Padding(3, 4, 3, 4);
            label16.Size = new Size(99, 21);
            label16.TabIndex = 0;
            label16.Text = "Textfile type :";
            // 
            // tableLayoutPanel14
            // 
            tableLayoutPanel14.ColumnCount = 1;
            tableLayoutPanel14.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel14.Controls.Add(radioButton3, 0, 0);
            tableLayoutPanel14.Controls.Add(radioButton5, 0, 1);
            tableLayoutPanel14.Controls.Add(radioButton6, 0, 2);
            tableLayoutPanel14.Dock = DockStyle.Fill;
            tableLayoutPanel14.Location = new Point(3, 33);
            tableLayoutPanel14.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel14.Name = "tableLayoutPanel14";
            tableLayoutPanel14.RowCount = 3;
            tableLayoutPanel14.RowStyles.Add(new RowStyle(SizeType.Percent, 35.5555573F));
            tableLayoutPanel14.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel14.RowStyles.Add(new RowStyle(SizeType.Percent, 31.11111F));
            tableLayoutPanel14.Size = new Size(99, 84);
            tableLayoutPanel14.TabIndex = 49;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(3, 4);
            radioButton3.Margin = new Padding(3, 4, 3, 4);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(50, 21);
            radioButton3.TabIndex = 0;
            radioButton3.TabStop = true;
            radioButton3.Text = "xml";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            radioButton5.AutoSize = true;
            radioButton5.Location = new Point(3, 33);
            radioButton5.Margin = new Padding(3, 4, 3, 4);
            radioButton5.Name = "radioButton5";
            radioButton5.Size = new Size(44, 20);
            radioButton5.TabIndex = 1;
            radioButton5.TabStop = true;
            radioButton5.Text = "txt";
            radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            radioButton6.AutoSize = true;
            radioButton6.Location = new Point(3, 61);
            radioButton6.Margin = new Padding(3, 4, 3, 4);
            radioButton6.Name = "radioButton6";
            radioButton6.Size = new Size(61, 19);
            radioButton6.TabIndex = 2;
            radioButton6.TabStop = true;
            radioButton6.Text = "None";
            radioButton6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.BackColor = Color.Transparent;
            button7.Dock = DockStyle.Fill;
            button7.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button7.ImeMode = ImeMode.NoControl;
            button7.Location = new Point(5, 125);
            button7.Margin = new Padding(5, 4, 5, 4);
            button7.Name = "button7";
            button7.Size = new Size(95, 38);
            button7.TabIndex = 24;
            button7.Text = "Export Text";
            button7.UseVisualStyleBackColor = false;
            button7.Click += Button7_Click;
            // 
            // button4
            // 
            button4.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button4.ImeMode = ImeMode.NoControl;
            button4.Location = new Point(10, 507);
            button4.Margin = new Padding(5, 4, 5, 4);
            button4.Name = "button4";
            button4.Size = new Size(101, 48);
            button4.TabIndex = 36;
            button4.Text = "Export audio from a local file";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // panel5
            // 
            panel5.BackColor = Color.White;
            panel5.Controls.Add(button10);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(1088, 4);
            panel5.Margin = new Padding(3, 4, 3, 4);
            panel5.Name = "panel5";
            panel5.Padding = new Padding(1);
            panel5.Size = new Size(35, 563);
            panel5.TabIndex = 51;
            // 
            // button10
            // 
            button10.BackColor = Color.FromArgb(82, 198, 222);
            button10.BackgroundImage = (Image)resources.GetObject("button10.BackgroundImage");
            button10.BackgroundImageLayout = ImageLayout.Zoom;
            button10.Dock = DockStyle.Fill;
            button10.Location = new Point(1, 1);
            button10.Margin = new Padding(3, 4, 3, 4);
            button10.Name = "button10";
            button10.Padding = new Padding(3, 4, 3, 4);
            button10.Size = new Size(33, 561);
            button10.TabIndex = 0;
            button10.TextAlign = ContentAlignment.TopCenter;
            button10.UseVisualStyleBackColor = false;
            button10.Click += Button10_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(button1, 0, 1);
            tableLayoutPanel3.Controls.Add(bttn3_LoadText, 0, 0);
            tableLayoutPanel3.Controls.Add(button8, 0, 2);
            tableLayoutPanel3.Controls.Add(button9, 0, 3);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(5, 4);
            tableLayoutPanel3.Margin = new Padding(5, 4, 5, 4);
            tableLayoutPanel3.MaximumSize = new Size(297, 0);
            tableLayoutPanel3.MinimumSize = new Size(65, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.Padding = new Padding(5, 4, 5, 4);
            tableLayoutPanel3.RowCount = 5;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
            tableLayoutPanel3.Size = new Size(91, 563);
            tableLayoutPanel3.TabIndex = 47;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.AutoSize = true;
            tableLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel4.BackColor = Color.White;
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(label1, 0, 0);
            tableLayoutPanel4.Controls.Add(textBox1, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(106, 4);
            tableLayoutPanel4.Margin = new Padding(5, 4, 5, 4);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.Padding = new Padding(5, 4, 5, 4);
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 51F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(529, 563);
            tableLayoutPanel4.TabIndex = 48;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = Color.FromArgb(192, 255, 192);
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 89F));
            tableLayoutPanel5.Controls.Add(label10, 2, 0);
            tableLayoutPanel5.Controls.Add(label11, 2, 1);
            tableLayoutPanel5.Controls.Add(vScrollBar3, 2, 2);
            tableLayoutPanel5.Controls.Add(label3, 0, 0);
            tableLayoutPanel5.Controls.Add(label2, 1, 0);
            tableLayoutPanel5.Controls.Add(comboBox4, 0, 1);
            tableLayoutPanel5.Controls.Add(comboBox5, 1, 1);
            tableLayoutPanel5.Controls.Add(panel2, 1, 2);
            tableLayoutPanel5.Controls.Add(panel1, 0, 2);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(5, 211);
            tableLayoutPanel5.Margin = new Padding(5, 4, 5, 4);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.Padding = new Padding(5, 4, 5, 4);
            tableLayoutPanel5.RowCount = 3;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(433, 304);
            tableLayoutPanel5.TabIndex = 49;
            // 
            // vScrollBar3
            // 
            vScrollBar3.Dock = DockStyle.Fill;
            vScrollBar3.LargeChange = 1;
            vScrollBar3.Location = new Point(339, 62);
            vScrollBar3.Name = "vScrollBar3";
            vScrollBar3.Size = new Size(89, 238);
            vScrollBar3.TabIndex = 44;
            vScrollBar3.Value = 100;
            vScrollBar3.ValueChanged += vScrollBar3_ValueChanged;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.AutoSize = true;
            tableLayoutPanel7.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel7.BackColor = Color.White;
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 443F));
            tableLayoutPanel7.Controls.Add(tableLayoutPanel8, 0, 0);
            tableLayoutPanel7.Controls.Add(tableLayoutPanel5, 0, 2);
            tableLayoutPanel7.Controls.Add(button2, 0, 3);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(645, 4);
            tableLayoutPanel7.Margin = new Padding(5, 4, 5, 4);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 4;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 165F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableLayoutPanel7.Size = new Size(435, 563);
            tableLayoutPanel7.TabIndex = 51;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.BackColor = Color.Transparent;
            tableLayoutPanel8.ColumnCount = 3;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 72F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tableLayoutPanel8.Controls.Add(comboBox3, 1, 2);
            tableLayoutPanel8.Controls.Add(comboBox2, 1, 1);
            tableLayoutPanel8.Controls.Add(comboBox1, 1, 0);
            tableLayoutPanel8.Controls.Add(label6, 2, 0);
            tableLayoutPanel8.Controls.Add(label5, 0, 1);
            tableLayoutPanel8.Controls.Add(label7, 2, 1);
            tableLayoutPanel8.Controls.Add(label8, 0, 2);
            tableLayoutPanel8.Controls.Add(label4, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(5, 4);
            tableLayoutPanel8.Margin = new Padding(5, 4, 5, 4);
            tableLayoutPanel8.MinimumSize = new Size(400, 0);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.Padding = new Padding(5, 4, 5, 4);
            tableLayoutPanel8.RowCount = 3;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel8.Size = new Size(433, 157);
            tableLayoutPanel8.TabIndex = 52;
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.BackgroundImageLayout = ImageLayout.Stretch;
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(11, 643);
            panel4.Margin = new Padding(11, 13, 11, 13);
            panel4.Name = "panel4";
            panel4.Size = new Size(1242, 25);
            panel4.TabIndex = 47;
            // 
            // tableLayoutPanel10
            // 
            tableLayoutPanel10.ColumnCount = 1;
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel10.Controls.Add(tableLayoutPanel9, 0, 0);
            tableLayoutPanel10.Controls.Add(tableLayoutPanel11, 0, 1);
            tableLayoutPanel10.Controls.Add(panel4, 0, 2);
            tableLayoutPanel10.Dock = DockStyle.Fill;
            tableLayoutPanel10.Location = new Point(0, 0);
            tableLayoutPanel10.Margin = new Padding(0);
            tableLayoutPanel10.Name = "tableLayoutPanel10";
            tableLayoutPanel10.RowCount = 3;
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Absolute, 51F));
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Absolute, 51F));
            tableLayoutPanel10.Size = new Size(1264, 681);
            tableLayoutPanel10.TabIndex = 0;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.BackColor = Color.White;
            tableLayoutPanel9.BackgroundImageLayout = ImageLayout.Zoom;
            tableLayoutPanel9.ColumnCount = 2;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 67F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.Controls.Add(panel3, 0, 0);
            tableLayoutPanel9.Dock = DockStyle.Fill;
            tableLayoutPanel9.Location = new Point(0, 0);
            tableLayoutPanel9.Margin = new Padding(0);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 1;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.Size = new Size(1264, 51);
            tableLayoutPanel9.TabIndex = 54;
            // 
            // panel3
            // 
            panel3.BackgroundImageLayout = ImageLayout.Zoom;
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(0);
            panel3.Name = "panel3";
            panel3.Size = new Size(67, 51);
            panel3.TabIndex = 0;
            // 
            // tableLayoutPanel11
            // 
            tableLayoutPanel11.AutoSize = true;
            tableLayoutPanel11.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel11.ColumnCount = 5;
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 101F));
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 445F));
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 41F));
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 128F));
            tableLayoutPanel11.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel11.Controls.Add(tableLayoutPanel4, 1, 0);
            tableLayoutPanel11.Controls.Add(tableLayoutPanel7, 2, 0);
            tableLayoutPanel11.Controls.Add(panel5, 3, 0);
            tableLayoutPanel11.Controls.Add(tableLayoutPanel1, 4, 0);
            tableLayoutPanel11.Dock = DockStyle.Fill;
            tableLayoutPanel11.Location = new Point(5, 55);
            tableLayoutPanel11.Margin = new Padding(5, 4, 5, 4);
            tableLayoutPanel11.Name = "tableLayoutPanel11";
            tableLayoutPanel11.RowCount = 1;
            tableLayoutPanel11.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel11.Size = new Size(1254, 571);
            tableLayoutPanel11.TabIndex = 53;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.White;
            ClientSize = new Size(1264, 681);
            Controls.Add(tableLayoutPanel10);
            Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point);
            HelpButton = true;
            Margin = new Padding(11, 13, 11, 13);
            MinimumSize = new Size(1278, 718);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Verbalize";
            Load += Form1_Load;
            Paint += Form1_Paint;
            Resize += Form1_Resize;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel13.ResumeLayout(false);
            tableLayoutPanel13.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel15.ResumeLayout(false);
            tableLayoutPanel15.PerformLayout();
            tableLayoutPanel16.ResumeLayout(false);
            tableLayoutPanel16.PerformLayout();
            tableLayoutPanel14.ResumeLayout(false);
            tableLayoutPanel14.PerformLayout();
            panel5.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel8.PerformLayout();
            tableLayoutPanel10.ResumeLayout(false);
            tableLayoutPanel10.PerformLayout();
            tableLayoutPanel9.ResumeLayout(false);
            tableLayoutPanel11.ResumeLayout(false);
            tableLayoutPanel11.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private OpenFileDialog openFileDialog1;
        private Button bttn3_LoadText;
        private Label label1;
        private TextBox textBox1;
        private VScrollBar vScrollBar1;
        private VScrollBar vScrollBar2;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Button button8;
        private Button button9;
        private Label label6;
        private Label label7;
        private ComboBox comboBox3;
        private Label label8;
        private Button button2;
        private Button button12;
        private Label label10;
        private Label label11;
        private Button button1;
        private Button button5;
        private ComboBox cmbBx_SelectSavedSoundFormat;
        private Panel panel1;
        private Panel panel2;
        private ComboBox comboBox4;
        private ComboBox comboBox5;
        private Label label9;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel7;
        private TableLayoutPanel tableLayoutPanel8;
        private Panel panel4;
        private TableLayoutPanel tableLayoutPanel10;
        private TableLayoutPanel tableLayoutPanel11;
        private TableLayoutPanel tableLayoutPanel9;
        private Panel panel3;
        private TableLayoutPanel tableLayoutPanel13;
        private Label label13;
        private VScrollBar vScrollBar3;
        private Label label14;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton4;
        private TableLayoutPanel tableLayoutPanel15;
        private Button button6;
        private TableLayoutPanel tableLayoutPanel16;
        private Label label16;
        private TableLayoutPanel tableLayoutPanel14;
        private RadioButton radioButton3;
        private RadioButton radioButton5;
        private RadioButton radioButton6;
        private Button button7;
        private Button button4;
        private Panel panel5;
        private Button button10;
    }
}