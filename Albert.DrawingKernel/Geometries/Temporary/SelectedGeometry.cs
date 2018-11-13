using Albert.DrawingKernel.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Selector;
using System.Windows.Media;
using Albert.DrawingKernel.Util;

namespace Albert.DrawingKernel.Geometries.Temporary
{
    public class SelectedGeometry : Geometry2D
    {
        private IntersectGeometry IntersectGeometry = null;

        /// <summary>
        /// 构造函数，初始化当前的图形选择类
        /// </summary>
        /// <param name="ig"></param>
        public SelectedGeometry(IntersectGeometry ig)
        {
            IntersectGeometry = ig;
        }

        
        public override List<Line2D> Lines
        {
            get
            {
                return null;
            }
        }

        public override List<Vector2D> Points
        {
            get
            {
                return null;
            }
        }

        public override void Update()
        {
            this.Draw(null);
        }

        /// <summary>
        /// 绘制当前图形的选择
        /// </summary>
        /// <param name="points"></param>
        protected override void Draw(List<Vector2D> points)
        {
            //冻结画笔，这样能加快绘图速度
            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();
            this.DashStyle = null;
            if (IntersectGeometry.GeometryShape != null)
            {
                //获取当前图形上所有的点
                var npoints = IntersectGeometry.GeometryShape.Points;
                if (npoints != null) {
                    //绘制矩形
                    npoints.ForEach(x =>
                    {
                        drawRect(dc, x);
                    });
                
                }

                if (IntersectGeometry.GeometryShape.Emphasize != null) {
                    IntersectGeometry.GeometryShape.Emphasize.ForEach(x => {
                        this.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);
                        this.PenColor = Colors.DarkGreen;
                        this.Pen.EndLineCap = PenLineCap.Triangle;
                        dc.DrawLine(Pen, KernelProperty.MMToPix(x.Start), KernelProperty.MMToPix(x.End));
                    });

                }
            }
            //获取兴趣的线
            if (IntersectGeometry.IntersectPoint != null && IntersectGeometry.IntersectPoint.Line != null)
            {
                var l = IntersectGeometry.IntersectPoint.Line;
                this.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);
                this.PenColor = Colors.Blue;
                this.Pen.EndLineCap = PenLineCap.Triangle;
                dc.DrawLine(Pen, KernelProperty.MMToPix(l.Start), KernelProperty.MMToPix(l.End));
            }


            dc.Close();
        }

        private void drawRect(DrawingContext dc,Vector2D central) {

            var v1 = new Vector2D(central.X - 5 * KernelProperty.PixelToSize, central.Y - 5 * KernelProperty.PixelToSize);
            var v2 = new Vector2D(central.X + 5 * KernelProperty.PixelToSize, central.Y - 5 * KernelProperty.PixelToSize);
            var v3 = new Vector2D(central.X + 5 * KernelProperty.PixelToSize, central.Y + 5 * KernelProperty.PixelToSize);
            var v4 = new Vector2D(central.X - 5 * KernelProperty.PixelToSize, central.Y + 5 * KernelProperty.PixelToSize);
            dc.DrawLine(Pen, KernelProperty.MMToPix(v1), KernelProperty.MMToPix(v2));
            dc.DrawLine(Pen, KernelProperty.MMToPix(v2), KernelProperty.MMToPix(v3));
            dc.DrawLine(Pen, KernelProperty.MMToPix(v3), KernelProperty.MMToPix(v4));
            dc.DrawLine(Pen, KernelProperty.MMToPix(v4), KernelProperty.MMToPix(v1));
        }
    }
}
