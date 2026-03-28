using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Shapes1
{
    // Line shape class
    public class LineShape : Shape
    {
        public LineShape(Point start, Point end)
            : base(start, end)
        {
        }

        public override void Draw(Graphics g)
        {
            g.DrawLine(Pens.Red, Start, End);
        }
    }
}
