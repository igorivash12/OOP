using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesApp.Shapes
{
    public class RectangleShape : Shape
    {
        private int x, y, width, height;

        public RectangleShape(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(Pens.Blue, x, y, width, height);
        }
    }
}
