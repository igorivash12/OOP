using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Shapes1
{
    public class ArcShape : Shape
    {
        public ArcShape(Point start, Point end)
            : base(start, end)
        {
        }

        public override void Draw(Graphics g)
        {
            Rectangle r = NormalizeMinimumSize(GetBoundingRectangle());
            g.DrawArc(Pens.Magenta, r, 0f, 180f);
        }
    }
}
