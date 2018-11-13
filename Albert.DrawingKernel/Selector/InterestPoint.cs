using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Selector
{
    /// <summary>
    /// 捕获相交的点对象
    /// </summary>
    public class IntersectPoint
    {
        /// <summary>
        /// 当前的捕捉点
        /// </summary>
        public Vector2D Point
        {
            get;
            set;
        }

        /// <summary>
        /// 捕获点相关联的线对象
        /// </summary>
        public Line2D Line
        {

            get;
            set;
        }

        /// <summary>
        /// 当前相交点的参照线对象
        /// </summary>
        public List<Line2D> Refences
        {

            get;
            set;
        }
        /// <summary>
        /// 捕捉点的样式
        /// </summary>
        public int IntersectPointStyle
        {
            get;
            set;
        }

    }
}
