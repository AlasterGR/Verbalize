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
            openFileDialog1 = new OpenFileDialog();
            button3 = new Button();
            label1 = new Label();
            checkBox2 = new CheckBox();
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
            button7 = new Button();
            comboBox3 = new ComboBox();
            label8 = new Label();
            button2 = new Button();
            button12 = new Button();
            hScrollBar1 = new HScrollBar();
            label10 = new Label();
            label11 = new Label();
            button1 = new Button();
            button4 = new Button();
            button5 = new Button();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.RestoreDirectory = true;
            // 
            // button3
            // 
            button3.Location = new Point(12, 12);
            button3.Name = "button3";
            button3.Size = new Size(126, 35);
            button3.TabIndex = 2;
            button3.Text = "Φόρτωση κειμένου";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.Font = new Font("Calibri", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(236, 20);
            label1.Name = "label1";
            label1.Size = new Size(194, 26);
            label1.TabIndex = 3;
            label1.Text = "Selected File address";
            label1.TextChanged += label1_TextChanged;
            label1.DoubleClick += label1_DoubleClick;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(12, 82);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(107, 19);
            checkBox2.TabIndex = 6;
            checkBox2.Text = "Πλήρης Σίγαση";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Cursor = Cursors.IBeam;
            textBox1.HideSelection = false;
            textBox1.Location = new Point(149, 53);
            textBox1.Margin = new Padding(1);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(361, 333);
            textBox1.TabIndex = 8;
            // 
            // vScrollBar1
            // 
            vScrollBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            vScrollBar1.LargeChange = 5;
            vScrollBar1.Location = new Point(606, 193);
            vScrollBar1.Maximum = 50;
            vScrollBar1.Minimum = -50;
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(61, 224);
            vScrollBar1.TabIndex = 11;
            vScrollBar1.ValueChanged += vScrollBar1_ValueChanged;
            // 
            // vScrollBar2
            // 
            vScrollBar2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            vScrollBar2.Location = new Point(793, 193);
            vScrollBar2.Maximum = 200;
            vScrollBar2.Minimum = -200;
            vScrollBar2.Name = "vScrollBar2";
            vScrollBar2.Size = new Size(62, 229);
            vScrollBar2.TabIndex = 12;
            vScrollBar2.ValueChanged += vScrollBar2_ValueChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(803, 178);
            label2.Margin = new Padding(1, 0, 1, 0);
            label2.Name = "label2";
            label2.Size = new Size(34, 15);
            label2.TabIndex = 13;
            label2.Text = "Pitch";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label3.Location = new Point(606, 178);
            label3.Margin = new Padding(1, 0, 1, 0);
            label3.Name = "label3";
            label3.Size = new Size(30, 15);
            label3.TabIndex = 14;
            label3.Text = "Rate";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(540, 12);
            label4.Margin = new Padding(1, 0, 1, 0);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 15;
            label4.Text = "Γλώσσα";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(540, 37);
            label5.Margin = new Padding(1, 0, 1, 0);
            label5.Name = "label5";
            label5.Size = new Size(63, 15);
            label5.TabIndex = 16;
            label5.Text = "Αφηγητής";
            // 
            // comboBox1
            // 
            comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(622, 9);
            comboBox1.Margin = new Padding(1);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(95, 23);
            comboBox1.TabIndex = 17;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // comboBox2
            // 
            comboBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(622, 34);
            comboBox2.Margin = new Padding(1);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(95, 23);
            comboBox2.TabIndex = 18;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // button8
            // 
            button8.Location = new Point(12, 107);
            button8.Name = "button8";
            button8.Size = new Size(97, 41);
            button8.TabIndex = 20;
            button8.Text = "Download Voices list";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new Point(12, 154);
            button9.Name = "button9";
            button9.Size = new Size(97, 46);
            button9.TabIndex = 21;
            button9.Text = "Populate the combo boxes";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(729, 40);
            label6.Name = "label6";
            label6.Size = new Size(66, 15);
            label6.TabIndex = 22;
            label6.Text = "Locale PCB";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(729, 12);
            label7.Name = "label7";
            label7.Size = new Size(117, 15);
            label7.TabIndex = 23;
            label7.Text = "Local name (Gender)";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // button7
            // 
            button7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button7.Location = new Point(147, 408);
            button7.Margin = new Padding(1);
            button7.Name = "button7";
            button7.Size = new Size(170, 56);
            button7.TabIndex = 24;
            button7.Text = "Save textbox text to SSML .xml  - και παραγωγή αρχείου ήχου";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // comboBox3
            // 
            comboBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "advertisement_upbeat", "affectionate", "angry", "assistant", "calm", "chat", "cheerful", "customerservice", "depressed", "disgruntled", "documentary-narration", "embarrassed", "empathetic", "envious", "excited", "fearful", "friendly", "gentle", "hopeful", "lyrical", "narration-professional", "narration-relaxed", "newscast", "newscast-casual", "newscast-formal", "poetry-reading", "sad", "serious", "shouting", "sports_commentary", "sports_commentary_excited", "whispering", "terrified", "unfriendly" });
            comboBox3.Location = new Point(578, 92);
            comboBox3.Margin = new Padding(1);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(130, 23);
            comboBox3.TabIndex = 25;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(542, 100);
            label8.Name = "label8";
            label8.Size = new Size(32, 15);
            label8.TabIndex = 26;
            label8.Text = "Style";
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.Location = new Point(357, 408);
            button2.Margin = new Padding(1);
            button2.Name = "button2";
            button2.Size = new Size(153, 56);
            button2.TabIndex = 28;
            button2.Text = "Ενημέρωση του αρχείου  - και παραγωγή αρχείου ήχου";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button12
            // 
            button12.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button12.Location = new Point(542, 65);
            button12.Name = "button12";
            button12.Size = new Size(200, 23);
            button12.TabIndex = 30;
            button12.Text = "Εκφώνηση επιλεγμένου κειμένου";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // hScrollBar1
            // 
            hScrollBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            hScrollBar1.Location = new Point(606, 435);
            hScrollBar1.Name = "hScrollBar1";
            hScrollBar1.Size = new Size(179, 30);
            hScrollBar1.TabIndex = 32;
            hScrollBar1.ValueChanged += hScrollBar1_ValueChanged;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label10.AutoSize = true;
            label10.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(514, 435);
            label10.Name = "label10";
            label10.Size = new Size(77, 23);
            label10.TabIndex = 33;
            label10.Text = "Volume :";
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label11.AutoSize = true;
            label11.Font = new Font("Calibri", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(788, 435);
            label11.Name = "label11";
            label11.Size = new Size(67, 23);
            label11.TabIndex = 34;
            label11.Text = "volume";
            // 
            // button1
            // 
            button1.Location = new Point(12, 53);
            button1.Name = "button1";
            button1.Size = new Size(97, 23);
            button1.TabIndex = 35;
            button1.Text = "Εκκαθάριση";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button4
            // 
            button4.Location = new Point(12, 206);
            button4.Name = "button4";
            button4.Size = new Size(95, 91);
            button4.TabIndex = 36;
            button4.Text = "Παραγωγή αρχείου ήχου από αρχείο";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(29, 323);
            button5.Name = "button5";
            button5.Size = new Size(75, 49);
            button5.TabIndex = 37;
            button5.Text = ".wav file to .mp3";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(867, 474);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(button9);
            Controls.Add(button7);
            Controls.Add(checkBox2);
            Controls.Add(button2);
            Controls.Add(button8);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(label11);
            Controls.Add(label6);
            Controls.Add(comboBox1);
            Controls.Add(label3);
            Controls.Add(label7);
            Controls.Add(comboBox3);
            Controls.Add(vScrollBar2);
            Controls.Add(comboBox2);
            Controls.Add(vScrollBar1);
            Controls.Add(label4);
            Controls.Add(label8);
            Controls.Add(button12);
            Controls.Add(label10);
            Controls.Add(label5);
            Controls.Add(hScrollBar1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private OpenFileDialog openFileDialog1;
        private Button button3;
        private Label label1;
        private CheckBox checkBox2;
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
        private Button button7;
        private ComboBox comboBox3;
        private Label label8;
        private Button button2;
        private Button button12;
        private HScrollBar hScrollBar1;
        private Label label10;
        private Label label11;
        private Button button1;
        private Button button4;
        private Button button5;
    }
}