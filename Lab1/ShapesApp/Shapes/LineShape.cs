using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesApp.Shapes
{
    public class LineShape : Shape
    {
        private int x1, y1, x2, y2;

        public LineShape(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public override void Draw(Graphics g)
        {
            g.DrawLine(Pens.Black, x1, y1, x2, y2);
        }
    }
}
