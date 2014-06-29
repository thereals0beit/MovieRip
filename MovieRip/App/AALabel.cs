using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MovieRip.App
{
    class AALabel : System.Windows.Forms.Label
    {
        private System.Drawing.Text.TextRenderingHint _textRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

        public System.Drawing.Text.TextRenderingHint TextRenderingHint
        {
            get { return _textRenderingHint; }
            set { _textRenderingHint = value; }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = _textRenderingHint;

            base.OnPaint(e);
        }
    }
}
