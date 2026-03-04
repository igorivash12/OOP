using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesApp.Shapes
{
    public class CircleShape : Shape
    {
        private int x, y, radius;

        public CircleShape(int x, int y, int radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
        }

        public override void Draw(Graphics g)
        {
            g.DrawEllipse(Pens.Green, x, y, radius * 2, radius * 2);
        }
    }
}
