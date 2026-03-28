using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Shapes1
{
    public class DiamondShape : Shape
    {
        public DiamondShape(Point start, Point end)
            : base(start, end)
        {
        }

        public override void Draw(Graphics g)
        {
            Rectangle r = GetBoundingRectangle();
            int cx = r.Left + r.Width / 2;
            int cy = r.Top + r.Height / 2;
            Point top = new Point(cx, r.Top);
            Point right = new Point(r.Right, cy);
            Point bottom = new Point(cx, r.Bottom);
            Point left = new Point(r.Left, cy);
            g.DrawPolygon(Pens.Orange, new[] { top, right, bottom, left });
        }
    }
}
