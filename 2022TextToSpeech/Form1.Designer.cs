using _2022TextToSpeech;

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
            button7 = new Button();
            comboBox3 = new ComboBox();
            label8 = new Label();
            button2 = new Button();
            button12 = new Button();
            label10 = new Label();
            label11 = new Label();
            button1 = new Button();
            button4 = new Button();
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
            tableLayoutPanel2 = new TableLayoutPanel();
            label14 = new Label();
            label15 = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel12 = new TableLayoutPanel();
            label12 = new Label();
            button3 = new Button();
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
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel12.SuspendLayout();
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
            bttn3_LoadText.Location = new Point(3, 3);
            bttn3_LoadText.Name = "bttn3_LoadText";
            bttn3_LoadText.Size = new Size(14, 14);
            bttn3_LoadText.TabIndex = 1;
            bttn3_LoadText.UseVisualStyleBackColor = true;
            bttn3_LoadText.Click += Bttn3_LoadText_Click;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(14, 20);
            label1.TabIndex = 0;
            label1.TextChanged += Label1_TextChanged;
            label1.Paint += label1_Paint;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.AliceBlue;
            textBox1.Cursor = Cursors.IBeam;
            textBox1.HideSelection = false;
            textBox1.Location = new Point(3, 23);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(14, 23);
            textBox1.TabIndex = 1;
            // 
            // vScrollBar1
            // 
            vScrollBar1.LargeChange = 1;
            vScrollBar1.Location = new Point(0, 0);
            vScrollBar1.Maximum = 50;
            vScrollBar1.Minimum = -50;
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(17, 80);
            vScrollBar1.TabIndex = 0;
            vScrollBar1.ValueChanged += vScrollBar1_ValueChanged;
            // 
            // vScrollBar2
            // 
            vScrollBar2.LargeChange = 1;
            vScrollBar2.Location = new Point(0, 0);
            vScrollBar2.Maximum = 200;
            vScrollBar2.Minimum = -200;
            vScrollBar2.Name = "vScrollBar2";
            vScrollBar2.Size = new Size(17, 80);
            vScrollBar2.TabIndex = 0;
            vScrollBar2.ValueChanged += vScrollBar2_ValueChanged;
            // 
            // label2
            // 
            label2.Location = new Point(23, 0);
            label2.Name = "label2";
            label2.Size = new Size(14, 20);
            label2.TabIndex = 4;
            // 
            // label3
            // 
            label3.Location = new Point(3, 0);
            label3.Name = "label3";
            label3.Size = new Size(14, 20);
            label3.TabIndex = 3;
            // 
            // label4
            // 
            label4.Location = new Point(3, 0);
            label4.Name = "label4";
            label4.Size = new Size(14, 20);
            label4.TabIndex = 5;
            // 
            // label5
            // 
            label5.Location = new Point(3, 20);
            label5.Name = "label5";
            label5.Size = new Size(14, 20);
            label5.TabIndex = 6;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(23, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(14, 23);
            comboBox1.TabIndex = 2;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(23, 23);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(14, 23);
            comboBox2.TabIndex = 1;
            comboBox2.SelectedIndexChanged += ComboBox2_SelectedIndexChanged;
            // 
            // button8
            // 
            button8.Location = new Point(3, 43);
            button8.Name = "button8";
            button8.Size = new Size(14, 14);
            button8.TabIndex = 2;
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new Point(3, 63);
            button9.Name = "button9";
            button9.Size = new Size(14, 14);
            button9.TabIndex = 3;
            button9.UseVisualStyleBackColor = true;
            button9.Click += Button9_Click;
            // 
            // label6
            // 
            label6.Location = new Point(43, 0);
            label6.Name = "label6";
            label6.Size = new Size(14, 20);
            label6.TabIndex = 3;
            // 
            // label7
            // 
            label7.Location = new Point(43, 20);
            label7.Name = "label7";
            label7.Size = new Size(14, 20);
            label7.TabIndex = 7;
            // 
            // button7
            // 
            button7.BackColor = Color.Transparent;
            button7.Location = new Point(3, 103);
            button7.Name = "button7";
            button7.Size = new Size(75, 14);
            button7.TabIndex = 3;
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(23, 43);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(14, 23);
            comboBox3.TabIndex = 0;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.Location = new Point(3, 40);
            label8.Name = "label8";
            label8.Size = new Size(14, 20);
            label8.TabIndex = 4;
            // 
            // button2
            // 
            button2.Location = new Point(3, 123);
            button2.Name = "button2";
            button2.Size = new Size(75, 14);
            button2.TabIndex = 4;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button12
            // 
            button12.BackColor = Color.Transparent;
            button12.Location = new Point(3, 23);
            button12.Name = "button12";
            button12.Size = new Size(14, 14);
            button12.TabIndex = 1;
            button12.UseVisualStyleBackColor = false;
            button12.Click += Button12_Click;
            // 
            // label10
            // 
            label10.BackColor = Color.Transparent;
            label10.Location = new Point(43, 0);
            label10.Name = "label10";
            label10.Size = new Size(14, 20);
            label10.TabIndex = 0;
            // 
            // label11
            // 
            label11.BackColor = Color.Transparent;
            label11.Location = new Point(43, 20);
            label11.Name = "label11";
            label11.Size = new Size(14, 20);
            label11.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.Location = new Point(3, 23);
            button1.Name = "button1";
            button1.Size = new Size(14, 14);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // button4
            // 
            button4.Location = new Point(3, 83);
            button4.Name = "button4";
            button4.Size = new Size(75, 14);
            button4.TabIndex = 2;
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.BackgroundImage = (Image)resources.GetObject("button5.BackgroundImage");
            button5.Location = new Point(3, 3);
            button5.Name = "button5";
            button5.Size = new Size(75, 14);
            button5.TabIndex = 1;
            button5.UseVisualStyleBackColor = true;
            button5.Click += Button5_Click;
            // 
            // cmbBx_SelectSavedSoundFormat
            // 
            cmbBx_SelectSavedSoundFormat.FormattingEnabled = true;
            cmbBx_SelectSavedSoundFormat.Location = new Point(3, 23);
            cmbBx_SelectSavedSoundFormat.Name = "cmbBx_SelectSavedSoundFormat";
            cmbBx_SelectSavedSoundFormat.Size = new Size(116, 23);
            cmbBx_SelectSavedSoundFormat.TabIndex = 0;
            cmbBx_SelectSavedSoundFormat.SelectedIndexChanged += cmbBx_SelectSavedSoundFormat_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(vScrollBar1);
            panel1.Location = new Point(3, 43);
            panel1.Name = "panel1";
            panel1.Size = new Size(14, 14);
            panel1.TabIndex = 8;
            panel1.Paint += panel_Paint;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(vScrollBar2);
            panel2.Location = new Point(23, 43);
            panel2.Name = "panel2";
            panel2.Size = new Size(14, 14);
            panel2.TabIndex = 7;
            panel2.Paint += panel_Paint;
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(3, 23);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(14, 23);
            comboBox4.TabIndex = 5;
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            // 
            // comboBox5
            // 
            comboBox5.FormattingEnabled = true;
            comboBox5.Location = new Point(23, 23);
            comboBox5.Name = "comboBox5";
            comboBox5.Size = new Size(14, 23);
            comboBox5.TabIndex = 6;
            comboBox5.SelectedIndexChanged += comboBox5_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.Location = new Point(3, 0);
            label9.Name = "label9";
            label9.Size = new Size(100, 20);
            label9.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(192, 255, 192);
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel13, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 3);
            tableLayoutPanel1.Controls.Add(button4, 0, 4);
            tableLayoutPanel1.Controls.Add(button7, 0, 5);
            tableLayoutPanel1.Controls.Add(button2, 0, 6);
            tableLayoutPanel1.Controls.Add(label14, 0, 2);
            tableLayoutPanel1.Controls.Add(label15, 0, 1);
            tableLayoutPanel1.Location = new Point(63, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(128, 14);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel13
            // 
            tableLayoutPanel13.BackColor = Color.FromArgb(255, 224, 192);
            tableLayoutPanel13.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel13.Controls.Add(label13, 0, 1);
            tableLayoutPanel13.Controls.Add(button5, 0, 0);
            tableLayoutPanel13.Location = new Point(3, 3);
            tableLayoutPanel13.Name = "tableLayoutPanel13";
            tableLayoutPanel13.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel13.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel13.Size = new Size(122, 14);
            tableLayoutPanel13.TabIndex = 0;
            // 
            // label13
            // 
            label13.Location = new Point(3, 20);
            label13.Name = "label13";
            label13.Size = new Size(100, 20);
            label13.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.FromArgb(192, 255, 192);
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(cmbBx_SelectSavedSoundFormat, 0, 1);
            tableLayoutPanel2.Controls.Add(label9, 0, 0);
            tableLayoutPanel2.Location = new Point(3, 63);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(122, 14);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // label14
            // 
            label14.BackColor = Color.FromArgb(192, 255, 192);
            label14.Location = new Point(3, 40);
            label14.Name = "label14";
            label14.Size = new Size(100, 20);
            label14.TabIndex = 5;
            // 
            // label15
            // 
            label15.BackColor = Color.White;
            label15.Location = new Point(3, 20);
            label15.Name = "label15";
            label15.Size = new Size(100, 20);
            label15.TabIndex = 6;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Controls.Add(button1, 0, 1);
            tableLayoutPanel3.Controls.Add(bttn3_LoadText, 0, 0);
            tableLayoutPanel3.Controls.Add(button8, 0, 2);
            tableLayoutPanel3.Controls.Add(button9, 0, 3);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel12, 0, 4);
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(14, 14);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel12
            // 
            tableLayoutPanel12.BackColor = Color.FromArgb(255, 192, 192);
            tableLayoutPanel12.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel12.Controls.Add(label12, 0, 1);
            tableLayoutPanel12.Controls.Add(button3, 0, 0);
            tableLayoutPanel12.Location = new Point(3, 83);
            tableLayoutPanel12.Name = "tableLayoutPanel12";
            tableLayoutPanel12.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel12.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel12.Size = new Size(14, 14);
            tableLayoutPanel12.TabIndex = 4;
            // 
            // label12
            // 
            label12.BackColor = Color.Transparent;
            label12.Location = new Point(3, 20);
            label12.Name = "label12";
            label12.Size = new Size(14, 20);
            label12.TabIndex = 0;
            // 
            // button3
            // 
            button3.BackColor = Color.Transparent;
            button3.BackgroundImage = (Image)resources.GetObject("button3.BackgroundImage");
            button3.Cursor = Cursors.Hand;
            button3.Location = new Point(3, 3);
            button3.Name = "button3";
            button3.Size = new Size(14, 14);
            button3.TabIndex = 1;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = Color.White;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Controls.Add(label1, 0, 0);
            tableLayoutPanel4.Controls.Add(textBox1, 0, 1);
            tableLayoutPanel4.Location = new Point(23, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(14, 14);
            tableLayoutPanel4.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = Color.FromArgb(192, 255, 192);
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Controls.Add(label10, 2, 0);
            tableLayoutPanel5.Controls.Add(label11, 2, 1);
            tableLayoutPanel5.Controls.Add(vScrollBar3, 2, 2);
            tableLayoutPanel5.Controls.Add(label3, 0, 0);
            tableLayoutPanel5.Controls.Add(label2, 1, 0);
            tableLayoutPanel5.Controls.Add(comboBox4, 0, 1);
            tableLayoutPanel5.Controls.Add(comboBox5, 1, 1);
            tableLayoutPanel5.Controls.Add(panel2, 1, 2);
            tableLayoutPanel5.Controls.Add(panel1, 0, 2);
            tableLayoutPanel5.Location = new Point(3, 43);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Size = new Size(14, 14);
            tableLayoutPanel5.TabIndex = 2;
            // 
            // vScrollBar3
            // 
            vScrollBar3.LargeChange = 1;
            vScrollBar3.Location = new Point(40, 40);
            vScrollBar3.Name = "vScrollBar3";
            vScrollBar3.Size = new Size(17, 20);
            vScrollBar3.TabIndex = 2;
            vScrollBar3.Value = 100;
            vScrollBar3.ValueChanged += vScrollBar3_ValueChanged;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.BackColor = Color.White;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.Controls.Add(tableLayoutPanel8, 0, 0);
            tableLayoutPanel7.Controls.Add(button12, 0, 1);
            tableLayoutPanel7.Controls.Add(tableLayoutPanel5, 0, 2);
            tableLayoutPanel7.Location = new Point(43, 3);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.Size = new Size(14, 14);
            tableLayoutPanel7.TabIndex = 2;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.BackColor = Color.Transparent;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.Controls.Add(comboBox3, 1, 2);
            tableLayoutPanel8.Controls.Add(comboBox2, 1, 1);
            tableLayoutPanel8.Controls.Add(comboBox1, 1, 0);
            tableLayoutPanel8.Controls.Add(label6, 2, 0);
            tableLayoutPanel8.Controls.Add(label8, 0, 2);
            tableLayoutPanel8.Controls.Add(label4, 0, 0);
            tableLayoutPanel8.Controls.Add(label5, 0, 1);
            tableLayoutPanel8.Controls.Add(label7, 2, 1);
            tableLayoutPanel8.Location = new Point(3, 3);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.Size = new Size(14, 14);
            tableLayoutPanel8.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.Location = new Point(3, 43);
            panel4.Name = "panel4";
            panel4.Size = new Size(194, 54);
            panel4.TabIndex = 2;
            // 
            // tableLayoutPanel10
            // 
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel10.Controls.Add(tableLayoutPanel9, 0, 0);
            tableLayoutPanel10.Controls.Add(tableLayoutPanel11, 0, 1);
            tableLayoutPanel10.Controls.Add(panel4, 0, 2);
            tableLayoutPanel10.Location = new Point(0, 0);
            tableLayoutPanel10.Name = "tableLayoutPanel10";
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel10.Size = new Size(200, 100);
            tableLayoutPanel10.TabIndex = 0;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.BackColor = Color.White;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel9.Controls.Add(panel3, 0, 0);
            tableLayoutPanel9.Location = new Point(3, 3);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel9.Size = new Size(194, 14);
            tableLayoutPanel9.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.BackgroundImage = (Image)resources.GetObject("panel3.BackgroundImage");
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(188, 14);
            panel3.TabIndex = 0;
            // 
            // tableLayoutPanel11
            // 
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel11.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel11.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel11.Controls.Add(tableLayoutPanel4, 1, 0);
            tableLayoutPanel11.Controls.Add(tableLayoutPanel7, 2, 0);
            tableLayoutPanel11.Controls.Add(tableLayoutPanel1, 3, 0);
            tableLayoutPanel11.Location = new Point(3, 23);
            tableLayoutPanel11.Name = "tableLayoutPanel11";
            tableLayoutPanel11.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel11.Size = new Size(194, 14);
            tableLayoutPanel11.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(284, 261);
            Controls.Add(tableLayoutPanel10);
            Name = "Form1";
            Load += Form1_Load;
            Paint += Form1_Paint;
            Resize += Form1_Resize;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel13.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel12.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel10.ResumeLayout(false);
            tableLayoutPanel9.ResumeLayout(false);
            tableLayoutPanel11.ResumeLayout(false);
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
        private Button button7;
        private ComboBox comboBox3;
        private Label label8;
        private Button button2;
        private Button button12;
        private Label label10;
        private Label label11;
        private Button button1;
        private Button button4;
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
        private Button button3;
        private TableLayoutPanel tableLayoutPanel10;
        private TableLayoutPanel tableLayoutPanel11;
        private TableLayoutPanel tableLayoutPanel9;
        private Panel panel3;
        private TableLayoutPanel tableLayoutPanel12;
        private Label label12;
        private TableLayoutPanel tableLayoutPanel13;
        private Label label13;
        private VScrollBar vScrollBar3;
        private Label label14;
        private Label label15;
    }
}