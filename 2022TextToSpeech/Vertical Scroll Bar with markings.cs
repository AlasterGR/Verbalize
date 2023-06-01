using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2022TextToSpeech
{/// <summary> The custom control of a vertical Scrollbar with its values' marked on the side.</summary>
    public partial class Vertical_Scroll_Bar_with_markings : UserControl
    {
        /// <summary> The custom control of a vertical Scrollbar with its values' marked on the side.</summary>
        public Vertical_Scroll_Bar_with_markings()
        {
            InitializeComponent();
        }
        private int _markerValue;
        /// <summary> The marker values.</summary>
        public int MarkerValue
        {
            get { return _markerValue; }
            set
            {
                _markerValue = value;
                Refresh();
            }
        }
        /// <summary> What to do on the OnPaint event.</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (VerticalScrollBarVisible())
            {
                var scrollBar = Controls[0] as VScrollBar;
                if (scrollBar != null)
                {
                    var barHeight = scrollBar.Height;
                    var pos = (int)((float)(Height - barHeight) * (float)MarkerValue / (float)scrollBar.Maximum);
                    var linePen = new Pen(Color.Red, 2);
                    e.Graphics.DrawLine(linePen, 0, pos, Width, pos);
                }
            }
        }

        private bool VerticalScrollBarVisible()
        {
            if (Controls.Count > 0)
            {
                var scrollBar = Controls[0] as VScrollBar;
                if (scrollBar != null)
                {
                    return scrollBar.Visible;
                }
            }
            return false;
        }
    }

}
