using Lab2.Factory;
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

        // Factory — shape types are loaded from the assembly automatically
        private ShapeFactory factory = new ShapeFactory();

        // Starting mouse point
        private Point startPoint;

        // Preview shape while drawing
        private Shape previewShape = null;

        // Drawing state flag
        private bool isDrawing = false;

        public frmMain()
        {
            InitializeComponent();

            comboBoxShapes.Items.AddRange(factory.GetShapeNames().ToArray());
            if (comboBoxShapes.Items.Count > 0)
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
                shape.Draw(e.Graphics);
            }

            if (previewShape != null)
            {
                previewShape.Draw(e.Graphics);
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