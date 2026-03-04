using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesApp.Shapes
{
    public class EllipseShape : Shape
    {
        private int x, y, width, height;

        public EllipseShape(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public override void Draw(Graphics g)
        {
            g.DrawEllipse(Pens.Red, x, y, width, height);
        }
    }
}
