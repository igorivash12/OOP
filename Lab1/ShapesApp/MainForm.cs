using System;
using System.Drawing;
using System.Windows.Forms;
using ShapesApp.Shapes;

namespace ShapesApp
{
    public partial class MainForm : Form
    {
        private ShapeList shapeList = new ShapeList();

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Статическая инициализация фигур
            shapeList.Add(new LineShape(10, 10, 200, 50));
            shapeList.Add(new RectangleShape(50, 80, 150, 100));
            shapeList.Add(new EllipseShape(250, 80, 150, 100));
            shapeList.Add(new CircleShape(450, 80, 50));
            shapeList.Add(new TriangleShape(
                new Point(100, 250),
                new Point(50, 350),
                new Point(150, 350)
            ));
            shapeList.Add(new PolygonShape(new Point[]
            {
                new Point(300, 250),
                new Point(350, 300),
                new Point(320, 350),
                new Point(280, 350),
                new Point(250, 300)
            }));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            shapeList.DrawAll(e.Graphics);
        }
    }
}