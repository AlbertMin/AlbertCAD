using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Albert.DrawingKernel.Util;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Geometries.Consult
{
    /// <summary>
    /// 当前的中心绘制对象
    /// </summary>
    public class CentralGeometry : Geometry2D
    {

        /// <summary>
        /// 中心绘制对象
        /// </summary>
        public CentralGeometry()
        {
            this.DashStyle = new DashStyle(new double[] { 5, 5 }, 0);
            this.Opacity = 0.5;
            this.LineWidth = 1;
        }

        /// <summary>
        /// 当前线段所记录的点
        /// </summary>
        private List<Vector2D> points = new List<Vector2D>();


        /// <summary>
        /// 获取当前所有的顶点信息
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                return points;

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

        /// <summary>
        /// 绘制当前对象
        /// </summary>
        /// <param name="ms"></param>
        public override void Update()
        {
            List<Vector2D> cpoints = new List<Vector2D>();
            Vector2D p1 = new Vector2D(KernelProperty.MeasureWidth / 2, 0);
            Vector2D p2 = new Vector2D(KernelProperty.MeasureWidth / 2, KernelProperty.MeasureHeight);
            Vector2D p3 = new Vector2D(0, KernelProperty.MeasureHeight / 2);
            Vector2D p4 = new Vector2D(KernelProperty.MeasureWidth, KernelProperty.MeasureHeight / 2);
            cpoints.Add(p1);
            cpoints.Add(p2);
            cpoints.Add(p3);
            cpoints.Add(p4);
            this.Draw(cpoints);
        }

        /// <summary>
        /// 绘制打断的对象
        /// </summary>
        protected override void Draw(List<Vector2D> points)
        {
            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();
            for (int i = 0; i < points.Count - 1; i = i + 2)
            {
                dc.DrawLine(Pen, new Point(points[i].X,points[i].Y), new Point(points[i + 1].X, points[i + 1].Y));
            }
            dc.Close();
        }


    }
}
