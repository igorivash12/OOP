using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Shapes1
{
    public class RoundedRectangleShape : Shape
    {
        public RoundedRectangleShape(Point start, Point end)
            : base(start, end)
        {
        }

        public override void Draw(Graphics g)
        {
            Rectangle r = NormalizeMinimumSize(GetBoundingRectangle());
            int radius = Math.Min(15, Math.Min(r.Width / 2, r.Height / 2));
            using (GraphicsPath path = CreateRoundedRectPath(r, radius))
            {
                g.DrawPath(Pens.DarkCyan, path);
            }
        }

        private static GraphicsPath CreateRoundedRectPath(Rectangle bounds, int radius)
        {
            int d = Math.Max(1, radius * 2);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180f, 90f);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270f, 90f);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0f, 90f);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90f, 90f);
            path.CloseFigure();
            return path;
        }
    }
}
