using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Selector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Selector
{
    /// <summary>
    /// 捕获的对象的关系
    /// </summary>
   public class IntersectGeometry
    {
        /// <summary>
        /// 当前的捕捉点
        /// </summary>
        public IntersectPoint IntersectPoint
        {
            get;
            set;
        }
        /// <summary>
        /// 命中的图形对象
        /// </summary>
        public Geometry2D GeometryShape
        {
            get;
            set;
        }
    }
}
