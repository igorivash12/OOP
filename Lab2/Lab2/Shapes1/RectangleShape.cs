using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Shapes1
{
    // Rectangle shape class
    public class RectangleShape : Shape
    {
        public RectangleShape(Point start, Point end)
            : base(start, end)
        {
        }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(Pens.Black, GetBoundingRectangle());
        }
    }
}
