using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using System.Windows.Media;
using Albert.DrawingKernel.Selector;
using Albert.Geometry.Transform;
using Albert.DrawingKernel.Util;

namespace Albert.DrawingKernel.Geometries.Primitives
{
    /// <summary>
    /// 绘制一个圆弧
    /// </summary>
    public class ArcGeometry : Geometry2D
    {


        private Vector2D central = null;

        /// <summary>
        /// 当前圆弧的中心坐标
        /// </summary>
        public Vector2D Central
        {
            get
            {
                return central;
            }
            set
            {
                central = value;
            }
        }


        private Vector2D start = null;

        /// <summary>
        /// 当前的圆弧的一个端点
        /// </summary>
        public Vector2D Start {

            get {

                return start;
            }
            set {
                start = value;
            }
        }

        private Vector2D end = null;
        /// <summary>
        /// 当前圆弧的另外一个端点
        /// </summary>
        public Vector2D End {

            get
            {
                return end;
            }
            set
            {
                end = value;
            }

        }

        /// <summary>
        /// 获取圆弧的所有的线
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前圆弧的点
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                List<Vector2D> vs = new List<Vector2D>();
                if (this.start != null)
                {
                    vs.Add(this.start);
                }
                if (this.end != null)
                {
                    vs.Add(this.end);
                }
                return vs;
            }
        }

        /// <summary>
        /// 刷新显示指定的图形元素
        /// </summary>
        public override void Update()
        {
            if (Start != null && Central != null && End == null)
            {
                this.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);
                DrawingContext dc = this.RenderOpen();
                Pen.Freeze();  //冻结画笔，这样能加快绘图速度
                List<Vector2D> vs = new List<Vector2D>() { Start, Central };
                this.Draw(vs);

            }
            else if (Start != null && Central != null && End != null)
            {
                //冻结画笔，这样能加快绘图速度
                DrawingContext dc = this.RenderOpen();
                Pen.Freeze();
                ArcSegment arc = new ArcSegment();
                var len = this.Central.Distance(this.start);
                arc.IsLargeArc = false;
                arc.Size = new System.Windows.Size(KernelProperty.MMToPix(len), KernelProperty.MMToPix(len));
                arc.Point = KernelProperty.MMToPix(this.end);
                PathGeometry paths = new PathGeometry();
                PathFigureCollection pfc = new PathFigureCollection();
                PathFigure pf = new PathFigure();
                pfc.Add(pf);
                pf.StartPoint = KernelProperty.MMToPix(this.start);
                pf.Segments.Add(arc);
                paths.Figures = pfc;
                dc.DrawGeometry(Brush, Pen, paths);
                dc.Close();
            }
        }


        /// <summary>
        /// 捕获圆形上的点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override IntersectPoint IntersectPoint(Vector2D v)
        {

            return null;
        }

        /// <summary>
        /// 不允许重合出现，所有假如绘制当前图形，则相交点总为空
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override IntersectPoint IntersectPoint2(Vector2D v)
        {
            return null;
        }

        /// <summary>
        /// 图形镜像
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mirrorLine"></param>
        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is BeamGeometry)
            {
                ArcGeometry arc = (target as ArcGeometry);
                this.start = TransformUtil.Mirror(arc.start, mirrorLine);
                this.end = TransformUtil.Mirror(arc.end, mirrorLine);
                this.central = TransformUtil.Mirror(arc.central, mirrorLine);
            }

        }
    }
}
