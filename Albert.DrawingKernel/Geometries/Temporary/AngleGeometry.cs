using Albert.Geometry.External;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Geometries.Temporary
{
    /// <summary>
    /// 一个控制角度的图形元素
    /// </summary>
    public class AngleGeometry:Geometry2D
    {

          /// <summary>
        /// 构造函数，定义当前圆形的参照方向和目标方向
        /// </summary>
        /// <param name="ip"></param>
        public AngleGeometry(Vector2D start,Vector2D end)
        {
            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AngleGeometry() { 
        
        }

        private Vector2D start = null;

        //获取参照方向
        public Vector2D Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        private Vector2D end = null;
        //获取当前的目标方向
        public Vector2D End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        private double angle = 0;
        //指定对应的角度
        public double Angle {

            get {
                return angle;
            }
            set {

                angle = value;
            }
        }
        /// <summary>
        /// 刷新当前图形元素
        /// </summary>
        public override void Update()
        {

            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();  //冻结画笔，这样能加快绘图速度
            this.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);
            this.PenColor = Colors.DeepSkyBlue;
            this.Pen.EndLineCap = PenLineCap.Triangle;
            ArcSegment arc = new ArcSegment();
            arc.IsLargeArc = true;
            if (start.AngleFrom(end) >= Math.PI)
            {
                arc.IsLargeArc = false;
            }
            else
            {
                arc.IsLargeArc = true;
            }
            arc.RotationAngle = 0;
            arc.Size = new System.Windows.Size(100, 100);
            arc.Point = KernelProperty.MMToPix(this.end);
            PathGeometry paths = new PathGeometry();
            paths.FillRule = FillRule.EvenOdd;
            PathFigureCollection pfc = new PathFigureCollection();
            PathFigure pf = new PathFigure();
            pfc.Add(pf);
            pf.StartPoint = KernelProperty.MMToPix(this.start);
            pf.Segments.Add(arc);
            paths.Figures = pfc;
            dc.DrawGeometry(Brush, Pen, paths);
            SeText(dc, Line2D.Create(start,end));
            dc.Close();
        }


        /// <summary>
        /// 绘制相关的文字信息
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="startToLine"></param>
        private void SeText(DrawingContext dc, Line2D startToLine)
        {
            Vector2D middle = startToLine.MiddlePoint;
            var md = KernelProperty.MMToPix(middle);
            string text = ((int)Extension.RadToDeg(Angle)).ToString();
            FormattedText ft = new FormattedText(text, new System.Globalization.CultureInfo(0x0804, false), System.Windows.FlowDirection.LeftToRight, new Typeface("微软雅黑"), 14, Brushes.Blue);
            dc.DrawText(ft, md);
        }
        /// <summary>
        /// 当前参照的点
        /// </summary>
        public override List<Vector2D> Points
        {
            get { return null; }
        }

        /// <summary>
        /// 当前参照的线信息
        /// </summary>
        public override List<Line2D> Lines
        {
            get { return null; }
        }
    }
}
