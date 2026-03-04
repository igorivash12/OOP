using Lab2.Shapes1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Factory
{
    // Factory for creating shapes
    public class ShapeFactory
    {
        private Dictionary<string, Func<Point, Point, Shape>> creators
            = new Dictionary<string, Func<Point, Point, Shape>>();

        public void Register(string name, Func<Point, Point, Shape> creator)
        {
            creators[name] = creator;
        }

        public Shape Create(string name, Point start, Point end)
        {
            return creators[name](start, end);
        }

        public IEnumerable<string> GetShapeNames()
        {
            return creators.Keys;
        }
    }
}
