using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool is_paint = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button_Chart_Click(object sender, EventArgs e)
        {
            chart1.Visible = true;
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas.Add("area1");
            chart1.Series.Add("Dependence of accuracy on the radius of the hit zone");
            chart1.Series["Dependence of accuracy on the radius of the hit zone"].ChartArea = "area1";
            chart1.Series["Dependence of accuracy on the radius of the hit zone"].ChartType = SeriesChartType.Spline;
            chart1.Series["Dependence of accuracy on the radius of the hit zone"].Color = Color.Blue;
            chart1.ChartAreas[0].AxisX.Title = "Radius of the hit zone";
            chart1.ChartAreas[0].AxisY.Title = "Accuracy";
            chart1.ChartAreas[0].AxisX.Minimum = 0;

            int step = 1;
            int x1 = 1;
            int y1 = 10;
            Point p1 = new Point(x1, y1);
            int x2 = 510;
            int y2 = 530;
            Point p2 = new Point(x2, y2);

            for (double i = 1; i <= 101; i = i + 5)
            {
                int n = Count_n(p1.X, p1.Y, p2.X, p2.Y, step, i);
                ChartForm(i, n);
            }
        }

        private void button_Move_Click(object sender, EventArgs e)
        {
            chart1.Visible = false;
            is_paint = true;
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (is_paint)
            {
                int step = 1;
                int x1 = 1;
                int y1 = 10;
                int x2 = 110;
                int y2 = 130;
                double eps = 5;
                int n = 5;
                Paint_Points(x1, y1, x2, y2, n, step, eps, e.Graphics);
            }
        }

        void ClearPoint(double X, double Y, Graphics g, double radius)
        {
            Pen pen = new Pen(Color.White);
            g.DrawEllipse(pen, (float)X, (float)Y, (float)radius, (float)radius);
        }
        void DrawPoint(double X, double Y, Graphics g, double radius)
        {
            Pen pen = new Pen(Color.Black);
            g.DrawEllipse(pen, (float)X, (float)Y, (float)radius, (float)radius);
        }
        void ChartForm(double x, double y)
        {
            chart1.Series["Dependence of accuracy on the radius of the hit zone"].Points.AddXY(x, y);

        }

        int Count_n(int x1, int y1, int x2, int y2, int step, double eps)
        {
            int n = 1;
            bool is_finish = false;
            double alpha;
            while ((n < 50) && (is_finish == false))
            {
                int dx = Math.Abs(x2 - x1);
                int dy = Math.Abs(y2 - y1);

                double otn = (double)dy / dx;
                if (otn <= 1)
                {
                    alpha = Atan(n, otn);
                }
                else
                {
                    otn = (double)dx / dy;
                    alpha = Math.PI / 2 - Atan(n, otn);

                }

                double distance = Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2)); ;
                double distance_last = Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
                double x = x1;
                double y = y1;

                while (true)
                {
                    if (distance <= distance_last)
                    {
                        distance_last = Math.Sqrt(Math.Pow((x - x2), 2) + Math.Pow((y - y2), 2));
                        x += step * Cos(n, alpha);
                        y += step * Sin(n, alpha);
                        distance = Math.Sqrt(Math.Pow((x - x2), 2) + Math.Pow((y - y2), 2));
                        if (distance <= eps)
                        {
                            is_finish = true;
                            break;
                        }
                    }
                    else
                    {
                        n++;
                        break;
                    }

                }


            }
            return n;
        }


        void Paint_Points(int x1, int y1, int x2, int y2, int n, int step, double eps, Graphics g)
        {
            double x = x1;
            double y = y1;
            double alpha;
            DrawPoint(x1, y1, g, eps);
            DrawPoint(x2, y2, g, eps);

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);

            double otn = (double)dy / dx;
            if (otn <= 1)
            {
                alpha = Atan(n, otn);
            }
            else
            {
                otn = (double)dx / dy;
                alpha = Math.PI / 2 - Atan(n, otn);

            }

            double distance = Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2)); ;
            double distance_last = Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));


            while (true)
            {
                if (distance <= distance_last)
                {
                    Thread.Sleep(50);
                    ClearPoint(x, y, g, eps);
                    distance_last = Math.Sqrt(Math.Pow((x - x2), 2) + Math.Pow((y - y2), 2));
                    x += step * Cos(n, alpha);
                    y += step * Sin(n, alpha);
                    DrawPoint(x, y, g, eps);
                    distance = Math.Sqrt(Math.Pow((x - x2), 2) + Math.Pow((y - y2), 2));
                    if (distance <= eps)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

            }
        }


        double Atan(int n, double x)
        {
            double result = 0;

            for (int i = 1; i <= n; i++)
            {
                result = result + Math.Pow(-1, (i - 1)) * Math.Pow(x, (2 * i - 1)) / (2 * i - 1);
            }

            return result;
        }



        double Sin(int n, double x)
        {
            double result = 0;
            for (int i = 1; i <= n; i++)
            {
                result = result + Math.Pow(-1, (i - 1)) * Math.Pow(x, (2 * i - 1)) / Factorial(2 * i - 1);
            }
            return result;
        }

        double Cos(int n, double x)
        {
            double result = 0;
            for (int i = 1; i <= n; i++)
            {
                result = result + Math.Pow(-1, (i - 1)) * Math.Pow(x, (2 * i - 2)) / Factorial(2 * i - 2);
            }
            return result;
        }

        double Factorial(double n)
        {
            double factorial = 1;
            for (int i = 1; i <= n; i++)
                factorial = factorial * i;
            return factorial;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.Visible = false;
        }
    }
}
