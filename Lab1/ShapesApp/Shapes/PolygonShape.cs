using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesApp.Shapes
{
    public class PolygonShape : Shape
    {
        private Point[] points;

        public PolygonShape(Point[] points)
        {
            this.points = points;
        }

        public override void Draw(Graphics g)
        {
            g.DrawPolygon(Pens.Orange, points);
        }
    }
}
