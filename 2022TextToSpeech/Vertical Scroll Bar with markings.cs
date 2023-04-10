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
{
    public partial class Vertical_Scroll_Bar_with_markings : UserControl
    {
        public Vertical_Scroll_Bar_with_markings()
        {
            InitializeComponent();
        }
        private int _markerValue;
        public int MarkerValue
        {
            get { return _markerValue; }
            set
            {
                _markerValue = value;
                Refresh();
            }
        }

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
