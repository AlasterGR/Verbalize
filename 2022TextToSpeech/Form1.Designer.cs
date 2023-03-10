namespace _2022TextToSpeech
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
            button1 = new Button();
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            button4 = new Button();
            textBox1 = new TextBox();
            button5 = new Button();
            button6 = new Button();
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
            button10 = new Button();
            button2 = new Button();
            button11 = new Button();
            button12 = new Button();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.RestoreDirectory = true;
            // 
            // button3
            // 
            button3.Location = new Point(123, 9);
            button3.Name = "button3";
            button3.Size = new Size(126, 35);
            button3.TabIndex = 2;
            button3.Text = "Φόρτωση κειμένου";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(255, 19);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 3;
            label1.Text = "label1";
            // 
            // button1
            // 
            button1.Location = new Point(367, 9);
            button1.Name = "button1";
            button1.Size = new Size(97, 33);
            button1.TabIndex = 4;
            button1.Text = "Εκφώνηση";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(12, 285);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(95, 19);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Αποθήκευση";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(12, 260);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(85, 19);
            checkBox2.TabIndex = 6;
            checkBox2.Text = "Εκφώνηση";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(11, 447);
            button4.Margin = new Padding(1);
            button4.Name = "button4";
            button4.Size = new Size(96, 40);
            button4.TabIndex = 7;
            button4.Text = "Convert .txt file to SSML .xml";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // textBox1
            // 
            textBox1.HideSelection = false;
            textBox1.Location = new Point(123, 48);
            textBox1.Margin = new Padding(1);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(341, 446);
            textBox1.TabIndex = 8;
            // 
            // button5
            // 
            button5.Location = new Point(13, 48);
            button5.Margin = new Padding(1);
            button5.Name = "button5";
            button5.Size = new Size(96, 55);
            button5.TabIndex = 9;
            button5.Text = "Εκφώνηση απλού κειμένου";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(12, 180);
            button6.Margin = new Padding(1);
            button6.Name = "button6";
            button6.Size = new Size(97, 52);
            button6.TabIndex = 10;
            button6.Text = "Αποθήκευση ως .txt";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // vScrollBar1
            // 
            vScrollBar1.LargeChange = 0;
            vScrollBar1.Location = new Point(512, 258);
            vScrollBar1.Maximum = 200;
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(42, 236);
            vScrollBar1.SmallChange = 0;
            vScrollBar1.TabIndex = 11;
            vScrollBar1.Value = 20;
            vScrollBar1.ValueChanged += vScrollBar1_ValueChanged;
            // 
            // vScrollBar2
            // 
            vScrollBar2.LargeChange = 0;
            vScrollBar2.Location = new Point(575, 258);
            vScrollBar2.Maximum = 1000;
            vScrollBar2.Name = "vScrollBar2";
            vScrollBar2.Size = new Size(52, 236);
            vScrollBar2.SmallChange = 0;
            vScrollBar2.TabIndex = 12;
            vScrollBar2.ValueChanged += vScrollBar2_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(575, 243);
            label2.Margin = new Padding(1, 0, 1, 0);
            label2.Name = "label2";
            label2.Size = new Size(34, 15);
            label2.TabIndex = 13;
            label2.Text = "Pitch";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(512, 243);
            label3.Margin = new Padding(1, 0, 1, 0);
            label3.Name = "label3";
            label3.Size = new Size(30, 15);
            label3.TabIndex = 14;
            label3.Text = "Rate";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(474, 48);
            label4.Margin = new Padding(1, 0, 1, 0);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 15;
            label4.Text = "Γλώσσα";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(474, 96);
            label5.Margin = new Padding(1, 0, 1, 0);
            label5.Name = "label5";
            label5.Size = new Size(63, 15);
            label5.TabIndex = 16;
            label5.Text = "Αφηγητής";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(541, 48);
            comboBox1.Margin = new Padding(1);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(243, 23);
            comboBox1.TabIndex = 17;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(541, 94);
            comboBox2.Margin = new Padding(1);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(95, 23);
            comboBox2.TabIndex = 18;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // button8
            // 
            button8.Location = new Point(10, 349);
            button8.Name = "button8";
            button8.Size = new Size(97, 41);
            button8.TabIndex = 20;
            button8.Text = "Download Voices list";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new Point(10, 396);
            button9.Name = "button9";
            button9.Size = new Size(97, 46);
            button9.TabIndex = 21;
            button9.Text = "Populate the combo boxes";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(640, 111);
            label6.Name = "label6";
            label6.Size = new Size(68, 15);
            label6.TabIndex = 22;
            label6.Text = "Short name";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(637, 96);
            label7.Name = "label7";
            label7.Size = new Size(117, 15);
            label7.TabIndex = 23;
            label7.Text = "Local name (Gender)";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // button7
            // 
            button7.Location = new Point(123, 509);
            button7.Margin = new Padding(1);
            button7.Name = "button7";
            button7.Size = new Size(170, 22);
            button7.TabIndex = 24;
            button7.Text = "Save textbox text to SSML .xml";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "advertisement_upbeat", "affectionate", "angry", "assistant", "calm", "chat", "cheerful", "customerservice", "depressed", "disgruntled", "documentary-narration", "embarrassed", "empathetic", "envious", "excited", "fearful", "friendly", "gentle", "hopeful", "lyrical", "narration-professional", "narration-relaxed", "newscast", "newscast-casual", "newscast-formal", "poetry-reading", "sad", "serious", "shouting", "sports_commentary", "sports_commentary_excited", "whispering", "terrified", "unfriendly" });
            comboBox3.Location = new Point(512, 209);
            comboBox3.Margin = new Padding(1);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(130, 23);
            comboBox3.TabIndex = 25;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(474, 212);
            label8.Name = "label8";
            label8.Size = new Size(32, 15);
            label8.TabIndex = 26;
            label8.Text = "Style";
            // 
            // button10
            // 
            button10.Location = new Point(13, 105);
            button10.Margin = new Padding(1);
            button10.Name = "button10";
            button10.Size = new Size(96, 38);
            button10.TabIndex = 27;
            button10.Text = "Εκφώνηση SSML κειμένου";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button2
            // 
            button2.Location = new Point(295, 509);
            button2.Margin = new Padding(1);
            button2.Name = "button2";
            button2.Size = new Size(170, 22);
            button2.TabIndex = 28;
            button2.Text = "Ενημέρωση του αρχείου";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button11
            // 
            button11.Location = new Point(474, 10);
            button11.Name = "button11";
            button11.Size = new Size(249, 33);
            button11.TabIndex = 29;
            button11.Text = "Φόρτωση χαρακτηριστικών από το αρχείο";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // button12
            // 
            button12.Location = new Point(479, 144);
            button12.Name = "button12";
            button12.Size = new Size(200, 23);
            button12.TabIndex = 30;
            button12.Text = "Εκφώνηση επιλεγμένου κειμένου";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 599);
            Controls.Add(button12);
            Controls.Add(button11);
            Controls.Add(button2);
            Controls.Add(button10);
            Controls.Add(label8);
            Controls.Add(comboBox3);
            Controls.Add(button7);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(vScrollBar2);
            Controls.Add(vScrollBar1);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(textBox1);
            Controls.Add(button4);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(button3);
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
        private Button button1;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private Button button4;
        private TextBox textBox1;
        private Button button5;
        private Button button6;
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
        private Button button10;
        private Button button2;
        private Button button11;
        private Button button12;
    }
}