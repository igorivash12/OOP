using System.Collections.Generic;
using System.Drawing;
using ShapesApp.Shapes;

namespace ShapesApp
{
    public class ShapeList
    {
        private List<Shape> shapes = new List<Shape>();

        public void Add(Shape shape)
        {
            shapes.Add(shape);
        }

        public void DrawAll(Graphics g)
        {
            foreach (Shape shape in shapes)
            {
                shape.Draw(g); // Полиморфизм
            }
        }
    }
}