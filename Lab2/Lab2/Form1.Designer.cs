using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab2
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        private ComboBox comboBoxShapes;
        private Panel drawingPanel;
        private Button buttonClear;
        private Label label1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxShapes = new ComboBox();
            this.drawingPanel = new Panel();
            this.buttonClear = new Button();
            this.label1 = new Label();

            this.SuspendLayout();

            // Label
            label1.Text = "Shape:";
            label1.Location = new Point(10, 15);
            label1.AutoSize = true;

            // ComboBox
            comboBoxShapes.Location = new Point(60, 10);
            comboBoxShapes.Width = 150;

            // Clear button
            buttonClear.Text = "Clear";
            buttonClear.Location = new Point(230, 10);
            buttonClear.Click += new EventHandler(buttonClear_Click);

            // Drawing panel
            drawingPanel.Location = new Point(10, 50);
            drawingPanel.Size = new Size(760, 400);
            drawingPanel.BorderStyle = BorderStyle.FixedSingle;
            drawingPanel.BackColor = Color.White;

            drawingPanel.MouseDown += new MouseEventHandler(drawingPanel_MouseDown);
            drawingPanel.MouseMove += new MouseEventHandler(drawingPanel_MouseMove);
            drawingPanel.MouseUp += new MouseEventHandler(drawingPanel_MouseUp);
            drawingPanel.Paint += new PaintEventHandler(drawingPanel_Paint);

            // Form
            this.ClientSize = new Size(800, 470);
            this.Controls.Add(label1);
            this.Controls.Add(comboBoxShapes);
            this.Controls.Add(buttonClear);
            this.Controls.Add(drawingPanel);

            this.Text = "Primitive Graphic Editor";

            this.ResumeLayout(false);
        }
    }
}