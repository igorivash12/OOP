using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Shapes1
{
    // Base abstract class for all shapes
    public abstract class Shape
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        protected Shape(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public abstract void Draw(Graphics g);

        protected Rectangle GetBoundingRectangle()
        {
            return new Rectangle(
                Math.Min(Start.X, End.X),
                Math.Min(Start.Y, End.Y),
                Math.Abs(End.X - Start.X),
                Math.Abs(End.Y - Start.Y)
            );
        }

        protected static Rectangle NormalizeMinimumSize(Rectangle r, int minW = 1, int minH = 1)
        {
            if (r.Width < minW)
                r.Width = minW;
            if (r.Height < minH)
                r.Height = minH;
            return r;
        }
    }
}
