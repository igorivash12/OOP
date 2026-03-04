using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesApp.Shapes
{
    public class TriangleShape : Shape
    {
        private Point p1, p2, p3;

        public TriangleShape(Point p1, Point p2, Point p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public override void Draw(Graphics g)
        {
            g.DrawPolygon(Pens.Purple, new Point[] { p1, p2, p3 });
        }
    }
}
