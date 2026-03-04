using Lab2.Factory;
using Lab2.Rendering;
using Lab2.Shapes1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Lab2
{
    public partial class frmMain : Form
    {
        // List storing all drawn shapes
        private List<Shape> shapes = new List<Shape>();

        // Factory for creating shapes
        private ShapeFactory factory = new ShapeFactory();

        // Renderer responsible for drawing shapes
        private ShapeRenderer renderer = new ShapeRenderer();

        // Starting mouse point
        private Point startPoint;

        // Preview shape while drawing
        private Shape previewShape = null;

        // Drawing state flag
        private bool isDrawing = false;

        public frmMain()
        {
            InitializeComponent();

            // Register shapes
            factory.Register("Rectangle", (s, e) => new RectangleShape(s, e));
            factory.Register("Circle", (s, e) => new CircleShape(s, e));
            factory.Register("Line", (s, e) => new LineShape(s, e));

            comboBoxShapes.Items.AddRange(factory.GetShapeNames().ToArray());
            comboBoxShapes.SelectedIndex = 0;
        }

        // Mouse pressed
        private void drawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            startPoint = e.Location;
            isDrawing = true;
        }

        // Mouse moving (preview shape)
        private void drawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing)
                return;

            Point currentPoint = e.Location;

            string shapeType = comboBoxShapes.SelectedItem.ToString();

            previewShape = factory.Create(shapeType, startPoint, currentPoint);

            drawingPanel.Invalidate();
        }

        // Mouse released
        private void drawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing)
                return;

            Point endPoint = e.Location;

            string shapeType = comboBoxShapes.SelectedItem.ToString();

            Shape shape = factory.Create(shapeType, startPoint, endPoint);

            shapes.Add(shape);

            previewShape = null;
            isDrawing = false;

            drawingPanel.Invalidate();
        }

        // Drawing all shapes
        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (var shape in shapes)
            {
                renderer.Draw(e.Graphics, shape);
            }

            if (previewShape != null)
            {
                renderer.Draw(e.Graphics, previewShape);
            }
        }

        // Clear canvas
        private void buttonClear_Click(object sender, EventArgs e)
        {
            shapes.Clear();
            drawingPanel.Invalidate();
        }
    }
}