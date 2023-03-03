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
            this.button2 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.vScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1574, 997);
            this.button2.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(274, 240);
            this.button2.TabIndex = 1;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(54, 613);
            this.button3.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(274, 240);
            this.button3.TabIndex = 2;
            this.button3.Text = "FindFile";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(398, 233);
            this.label1.Margin = new System.Windows.Forms.Padding(9, 0, 9, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 48);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(54, 907);
            this.button1.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(274, 240);
            this.button1.TabIndex = 4;
            this.button1.Text = "Replay";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(56, 1185);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(272, 52);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Αποθήκευση";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(398, 1185);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(239, 52);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Εκφώνηση";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(712, 1168);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(486, 69);
            this.button4.TabIndex = 7;
            this.button4.Text = "Convert .txt file to SSML .xml";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(398, 339);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(1164, 808);
            this.textBox1.TabIndex = 8;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(54, 339);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(274, 69);
            this.button5.TabIndex = 9;
            this.button5.Text = "Εκφώνηση";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(54, 449);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(274, 113);
            this.button6.TabIndex = 10;
            this.button6.Text = "Αποθήκευση ως .txt";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(1574, 751);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(153, 223);
            this.vScrollBar1.TabIndex = 11;
            // 
            // vScrollBar2
            // 
            this.vScrollBar2.Location = new System.Drawing.Point(1806, 751);
            this.vScrollBar2.Name = "vScrollBar2";
            this.vScrollBar2.Size = new System.Drawing.Size(153, 223);
            this.vScrollBar2.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1806, 666);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 48);
            this.label2.TabIndex = 13;
            this.label2.Text = "Pitch";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1574, 666);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 48);
            this.label3.TabIndex = 14;
            this.label3.Text = "Rate";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1588, 339);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 48);
            this.label4.TabIndex = 15;
            this.label4.Text = "Γλώσσα";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1963, 339);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 48);
            this.label5.TabIndex = 16;
            this.label5.Text = "Αφηγητής";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1594, 399);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(363, 56);
            this.comboBox1.TabIndex = 17;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(1963, 399);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(363, 56);
            this.comboBox2.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(20F, 48F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2359, 1287);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.vScrollBar2);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}