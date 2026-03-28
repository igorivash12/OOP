using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Shapes1
{
    public class TriangleShape : Shape
    {
        public TriangleShape(Point start, Point end)
            : base(start, end)
        {
        }

        public override void Draw(Graphics g)
        {
            Rectangle r = NormalizeMinimumSize(GetBoundingRectangle());
            Point apex = new Point(r.Left + r.Width / 2, r.Top);
            Point bl = new Point(r.Left, r.Bottom);
            Point br = new Point(r.Right, r.Bottom);
            g.DrawPolygon(Pens.DarkGreen, new[] { apex, br, bl });
        }
    }
}
