using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Geometries;

namespace Albert.DrawingKernel.Functional
{
    /// <summary>
    /// 记录物体的移动动作
    /// </summary>
    public class MoveFunction : BaseFunctional
    {
        private Vector2D offset = null;

        public MoveFunction(Geometry2D gs) : base(gs)
        {
        }

        /// <summary>
        /// 当前的便宜
        /// </summary>
        public Vector2D Offset {

            get {
                return offset;
            }
            set {
                offset = value;
            }
        }

   
    }
}
