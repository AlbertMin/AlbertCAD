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
    public class PickedGeometry : Geometry2D
    {
        private IntersectGeometry IntersectGeometry = null;

        /// <summary>
        /// 构造函数，初始化当前的图形选择类
        /// </summary>
        /// <param name="ig"></param>
        public PickedGeometry(IntersectGeometry ig)
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
            //获取兴趣的线
            if (IntersectGeometry.IntersectPoint != null && IntersectGeometry.IntersectPoint.Line != null)
            {
                var l = IntersectGeometry.IntersectPoint.Line;
                this.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);
                this.PenColor = Colors.Red;
                this.Pen.EndLineCap = PenLineCap.Triangle;
                dc.DrawLine(Pen, KernelProperty.MMToPix(l.Start), KernelProperty.MMToPix(l.End));
            }


            dc.Close();
        }

      
    }
}
