using Lab2.Shapes1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Rendering
{
    // Responsible for drawing shapes
    public class ShapeRenderer
    {
        public void Draw(Graphics g, Shape shape)
        {
            if (shape is RectangleShape)
            {
                DrawRectangle(g, shape);
            }
            else if (shape is CircleShape)
            {
                DrawCircle(g, shape);
            }
            else if (shape is LineShape)
            {
                DrawLine(g, shape);
            }
        }

        private void DrawRectangle(Graphics g, Shape s)
        {
            Rectangle rect = GetRectangle(s);
            g.DrawRectangle(Pens.Black, rect);
        }

        private void DrawCircle(Graphics g, Shape s)
        {
            Rectangle rect = GetRectangle(s);
            g.DrawEllipse(Pens.Blue, rect);
        }

        private void DrawLine(Graphics g, Shape s)
        {
            g.DrawLine(Pens.Red, s.Start, s.End);
        }

        private Rectangle GetRectangle(Shape s)
        {
            return new Rectangle(
                Math.Min(s.Start.X, s.End.X),
                Math.Min(s.Start.Y, s.End.Y),
                Math.Abs(s.End.X - s.Start.X),
                Math.Abs(s.End.Y - s.Start.Y)
            );
        }
    }
}
