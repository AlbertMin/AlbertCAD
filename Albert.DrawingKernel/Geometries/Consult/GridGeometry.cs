using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Albert.DrawingKernel.Geometries.Consult
{
    public class GridGeometry : Geometry2D
    {

        public GridGeometry()
        {

            this.DashStyle = new DashStyle(new double[] { 5, 5 }, 0);
        }
        private List<Vector2D> points = new List<Vector2D>();
        /// <summary>
        /// 重绘制当前的网格
        /// </summary>
        /// <param name="ms"></param>
        public override void Update()
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Black);
            brush.Opacity = 0.5;


            double w = KernelProperty.MeasureWidth;
            double h = KernelProperty.MeasureHeight;

            //总共有多少米
            double wmi = w /50;

            double hmi = h /50;

            for (int i = 0; i < wmi; i++)
            {
                Vector2D p1 = new Vector2D(i* 50, 0);
                Vector2D p2 = new Vector2D(i * 50, h);

                points.Add(p1);
                points.Add(p2);
            }
            for (int j = 0; j < hmi; j++)
            {
                Vector2D p1 = new Vector2D(0, j * 50);
                Vector2D p2 = new Vector2D(w, j * 50);

                points.Add(p1);
                points.Add(p2);
            }

            this.Draw(points);
        }

        protected override void Draw(List<Vector2D> points)
        {
            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();  //冻结画笔，这样能加快绘图速度
            for (int i = 0; i < points.Count - 1; i = i + 2)
            {
                dc.DrawLine(Pen, KernelProperty.MMToPix(points[i]), KernelProperty.MMToPix(points[i + 1]));
            }

            dc.Close();
        }

        public override List<Vector2D> Points
        {

            get
            {

                return points.ToList();
            }
        }


        /// <summary>
        /// 获取当前所有直线信息
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {
                List<Line2D> lines = new List<Line2D>();
                return lines;
            }
        }

    }

}
