using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LEDdriverControlApp
{
    // Flat, fully-colorable replacement for GroupBox as a "channel card"
    // container. GroupBox draws its own caption text and a sunken 3D-style
    // border using fixed OS bevel colors (SystemColors.ControlDark/Light)
    // that cannot be restyled -- on a dark theme that shows up as a pale,
    // uncoordinated outline no matter what BackColor is set. CardPanel is a
    // plain rounded-rect surface instead: the card's title is a separate
    // Label (see Form1.Designer.cs's MakeCardTitle), and the border below is
    // drawn in code using the same brand tokens as everything else.
    //
    // The rounded look is a real clip region (not just a drawn line), so the
    // form's background shows through the corners instead of leaving square
    // corners poking out past a rounded border stroke.
    public class CardPanel : Panel
    {
        public int CornerRadius { get; set; } = 14;
        public Color BorderColor { get; set; } = Color.FromArgb(0x29, 0x3A, 0x3D);
        public int BorderThickness { get; set; } = 1;

        public CardPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                      ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Resize += (s, e) => ApplyRoundedClip();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyRoundedClip();
        }

        private void ApplyRoundedClip()
        {
            if (Width <= 0 || Height <= 0) return;
            using var path = RoundedRectPath(new Rectangle(0, 0, Width, Height), CornerRadius);
            Region = new Region(path);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (BorderThickness <= 0) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float inset = BorderThickness / 2f;
            var rect = new RectangleF(inset, inset, Width - BorderThickness, Height - BorderThickness);
            using var path = RoundedRectPath(rect, CornerRadius);
            using var pen = new Pen(BorderColor, BorderThickness);
            g.DrawPath(pen, path);
        }

        private static GraphicsPath RoundedRectPath(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            float d = radius * 2;
            if (d > r.Height) d = r.Height;
            if (d > r.Width) d = r.Width;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
