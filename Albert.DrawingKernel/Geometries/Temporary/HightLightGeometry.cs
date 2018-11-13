using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using System.Windows.Media;
using Albert.DrawingKernel.Util;

namespace Albert.DrawingKernel.Geometries.Temporary
{
    /// <summary>
    /// 图形高亮显示
    /// </summary>
    public class HightLightGeometry : Geometry2D
    {

        public HightLightGeometry(List<Line2D> lines):base() {

            this.lightLines = lines;

        }

        private List<Line2D> lightLines = null;
        /// <summary>
        /// 当前需要高亮的线
        /// </summary>
        public override List<Line2D> Lines
        {
            get {

                return lightLines;
            }
        }

        /// <summary>
        /// 返回当前所需要的点
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 刷新当前的绘制
        /// </summary>
        public override void Update()
        {
            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();  //冻结画笔，这样能加快绘图速度
            this.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);
            this.PenColor = Colors.Red;
            this.LineWidth = 5;
            for (int i = 0; i < lightLines.Count; i++)
            {
                dc.DrawLine(Pen, KernelProperty.MMToPix(lightLines[i].Start), KernelProperty.MMToPix(lightLines[i].End));
            }
            dc.Close();
        }
    }
}
