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
    }
}
