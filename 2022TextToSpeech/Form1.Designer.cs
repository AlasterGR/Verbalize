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
            button2 = new Button();
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
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            SuspendLayout();
            // 
            // button2
            // 
            button2.Location = new Point(551, 312);
            button2.Name = "button2";
            button2.Size = new Size(96, 75);
            button2.TabIndex = 1;
            button2.Text = "Close";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.RestoreDirectory = true;
            // 
            // button3
            // 
            button3.Location = new Point(19, 192);
            button3.Name = "button3";
            button3.Size = new Size(96, 75);
            button3.TabIndex = 2;
            button3.Text = "FindFile";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(139, 73);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 3;
            label1.Text = "label1";
            // 
            // button1
            // 
            button1.Location = new Point(19, 283);
            button1.Name = "button1";
            button1.Size = new Size(96, 75);
            button1.TabIndex = 4;
            button1.Text = "Replay";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(20, 370);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(95, 19);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Αποθήκευση";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(139, 370);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(85, 19);
            checkBox2.TabIndex = 6;
            checkBox2.Text = "Εκφώνηση";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(249, 365);
            button4.Margin = new Padding(1);
            button4.Name = "button4";
            button4.Size = new Size(170, 22);
            button4.TabIndex = 7;
            button4.Text = "Convert .txt file to SSML .xml";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(139, 106);
            textBox1.Margin = new Padding(1);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(410, 255);
            textBox1.TabIndex = 8;
            // 
            // button5
            // 
            button5.Location = new Point(19, 106);
            button5.Margin = new Padding(1);
            button5.Name = "button5";
            button5.Size = new Size(96, 22);
            button5.TabIndex = 9;
            button5.Text = "Εκφώνηση";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(19, 140);
            button6.Margin = new Padding(1);
            button6.Name = "button6";
            button6.Size = new Size(96, 35);
            button6.TabIndex = 10;
            button6.Text = "Αποθήκευση ως .txt";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // vScrollBar1
            // 
            vScrollBar1.Location = new Point(551, 235);
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(153, 70);
            vScrollBar1.TabIndex = 11;
            // 
            // vScrollBar2
            // 
            vScrollBar2.Location = new Point(632, 235);
            vScrollBar2.Name = "vScrollBar2";
            vScrollBar2.Size = new Size(153, 70);
            vScrollBar2.TabIndex = 12;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(632, 208);
            label2.Margin = new Padding(1, 0, 1, 0);
            label2.Name = "label2";
            label2.Size = new Size(34, 15);
            label2.TabIndex = 13;
            label2.Text = "Pitch";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(551, 208);
            label3.Margin = new Padding(1, 0, 1, 0);
            label3.Name = "label3";
            label3.Size = new Size(30, 15);
            label3.TabIndex = 14;
            label3.Text = "Rate";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(556, 106);
            label4.Margin = new Padding(1, 0, 1, 0);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 15;
            label4.Text = "Γλώσσα";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(687, 106);
            label5.Margin = new Padding(1, 0, 1, 0);
            label5.Name = "label5";
            label5.Size = new Size(63, 15);
            label5.TabIndex = 16;
            label5.Text = "Αφηγητής";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(558, 125);
            comboBox1.Margin = new Padding(1);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(130, 23);
            comboBox1.TabIndex = 17;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(687, 125);
            comboBox2.Margin = new Padding(1);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(130, 23);
            comboBox2.TabIndex = 18;
            // 
            // button7
            // 
            button7.Location = new Point(897, 130);
            button7.Name = "button7";
            button7.Size = new Size(105, 23);
            button7.TabIndex = 19;
            button7.Text = "JSon -> XML";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new Point(897, 170);
            button8.Name = "button8";
            button8.Size = new Size(105, 23);
            button8.TabIndex = 20;
            button8.Text = "Retrieve Voices";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new Point(897, 204);
            button9.Name = "button9";
            button9.Size = new Size(105, 38);
            button9.TabIndex = 21;
            button9.Text = "Populate the combo boxes";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1056, 609);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
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
            Controls.Add(button2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button2;
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
        private Button button7;
        private Button button8;
        private Button button9;
    }
}