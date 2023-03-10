using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjektNG
{
    public partial class GraphicsForm : Form
    {
        public GraphicsForm()
        {
            InitializeComponent();
        }
        private List<Entities.Point> points = new List<Entities.Point>();
        private List<Entities.Line> lines = new List<Entities.Line>();
        private List<Entities.Circle>circles= new List<Entities.Circle>();
        private Vector3 currentPosition;
        private Vector3 firstPoint;
        private int DrawIndex = -1;
        private bool active_drawing = false;
        private int ClickNum = 1;

        private void GraphicsForm_Load(object sender, EventArgs e)
        {

        }

        private void drawing_MouseMove(object sender, MouseEventArgs e)
        {
            currentPosition =PointToCartesian(e.Location);
            label1.Text=string.Format("{0}, {1}",e.Location.X,e.Location.Y); //koordinantni sustav
            drawing.Refresh();
           // label2.Text = string.Format("{0,0:F3}, {1,0:F3}", currentPosition.X, currentPosition.Y);
        }

        //dobili smo screen dpi
        private float DPI
        {
            get
            {
                using (var g = CreateGraphics())
                    return g.DpiX;
            }
        }
        //convert system point to word1 point
        private Vector3 PointToCartesian(Point point)
        {
            return new Vector3(Pixel_to_Mn(point.X), Pixel_to_Mn(drawing.Height-point.Y));
        }
        //pretvorba piksela u milimetre
        private float Pixel_to_Mn(float pixel)
        {
            return pixel * 25.4f / DPI;
        }

        private void drawing_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Left)
            {
                if (active_drawing)
                {
                    switch(DrawIndex)
                    {
                        case 0: //point
                            points.Add(new Entities.Point(currentPosition));
                                break;
                       case 1:// line
                            switch (ClickNum)
                            {
                                case 1:
                                    firstPoint = currentPosition;
                                    points.Add(new Entities.Point(currentPosition));
                                    ClickNum++;
                                    break;
                                case 2:
                                    lines.Add(new Entities.Line(firstPoint, currentPosition));
                                    points.Add(new Entities.Point(currentPosition));
                                    firstPoint =currentPosition;
                                    break;
                            }
                            break;
                       case 2: // circle
                            switch (ClickNum)
                            {
                                case 1:
                                    firstPoint = currentPosition;
                                    ClickNum++;
                                    break;
                                case 2:
                                    double r=firstPoint.DistanceFrom(currentPosition);
                                    circles.Add(new Entities.Circle(firstPoint,r));
                                    ClickNum = 1;
                                    break;  
                            }
                            break;
                    }
                    drawing.Refresh();
                }
            }
        }


        private void drawing_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SetParameters(Pixel_to_Mn(drawing.Height));
            Pen pen=new Pen(Color.Pink,0.1f);
            Pen extpen = new Pen(Color.Blue, 0.1f);
            //Draw all points
            if (points.Count > 0)
            {
                foreach(Entities.Point p in points)
                {
                    e.Graphics.DrawPoint(new Pen(Color.Red, 0.1f), p);
                }
            }
            // Draw all lines 
            if (lines.Count > 0)
            {
                foreach(Entities.Line l in lines)
                {
                    e.Graphics.DrawLine(pen, l);
                }
            }
            // Draw all circle
            if (circles.Count > 0)
            {
                foreach(Entities.Circle c in circles)
                {
                    e.Graphics.DrawCircle(pen, c);
                }
            }
            // Draw line extended
            switch (DrawIndex)
            {
                case 1:
                    if(ClickNum == 2)
                    {
                        Entities.Line line = new Entities.Line(firstPoint, currentPosition);
                        e.Graphics.DrawLine(extpen, line);
                    }
                  
                    break;
                case 2:
                    if (ClickNum == 2)
                    {
                        Entities.Line line = new Entities.Line(firstPoint, currentPosition);
                        e.Graphics.DrawLine(extpen, line);
                        double r=firstPoint.DistanceFrom(currentPosition);
                        Entities.Circle circle = new Entities.Circle(firstPoint,r);
                        e.Graphics.DrawCircle(extpen, circle); 
                    }

                    break;
            }
        }

        private void drawing_Click(object sender, EventArgs e)
        {

        }

        private void pointBtn_Click(object sender, EventArgs e)
        {
            DrawIndex = 0;
            active_drawing = true;
            drawing.Cursor = Cursors.Cross;
        }

        private void lineBtn_Click(object sender, EventArgs e)
        {
            DrawIndex = 1;
            active_drawing = true;
            drawing.Cursor = Cursors.Cross;
        }

        private void circleBtn_Click(object sender, EventArgs e)
        {
            DrawIndex = 2;
            active_drawing = true;
            drawing.Cursor = Cursors.Cross;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
