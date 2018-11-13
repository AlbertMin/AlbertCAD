using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using Albert.Geometry.Transform;

namespace Albert.DrawingKernel.Geometries.Primitives
{
    /// <summary>
    /// 定义一个标尺
    /// </summary>
    public class StaffGeometry : Geometry2D
    {

        private List<Vector2D> ppoints = null;
        /// <summary>
        /// 当前的图形的起点
        /// </summary>
        public List<Vector2D> PPoints {

            get {
                return ppoints;
            }
            set {
                ppoints = value;
            }
        }

        /// <summary>
        /// 标尺线的距离
        /// </summary>
        public Vector2D Margin
        {
            get;
            set;
        }

        /// <summary>
        /// 返回当前所有直线信息
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
        /// 返回当前所有的顶点信息
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                return PPoints;
            }
        }


        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is StaffGeometry)
            {
        
            }

        }
        /// <summary>
        /// 更新当前的绘制
        /// </summary>
        public override void Update()
        {

            if (PPoints.Count > 1)
            {
                this.Draw(PPoints);
            }
        }
    }
}
