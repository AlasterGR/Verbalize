using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Verbalize
{
    internal class UIHandling
    {
        public TableLayoutPanel soundRadioGroupParentPanel = Form1.soundRadioGroupParentPanel;
        public TableLayoutPanel textRadioGroupParentPanel = Form1.textRadioGroupParentPanel;
        public TextBox mainSingleTextBox = Form1.mainSingleTextBox;

        public void btn_ExportText()
        {
            string outputTextFormat = SetOutputTextFormat();
            FileHandling.SaveText(outputTextFormat, mainSingleTextBox);
        }
        private string SetOutputTextFormat()
        {
            foreach (Control control in textRadioGroupParentPanel.Controls)
            {
                if (control is System.Windows.Forms.RadioButton radioButton && radioButton.Checked)
                {
                    return radioButton.Text;
                }
            }
            return "None"; // Return null if no radio button is selected in the group
        }
    }
}
