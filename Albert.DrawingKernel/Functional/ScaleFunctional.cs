using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Geometries;

namespace Albert.DrawingKernel.Functional
{
    /// <summary>
    /// 记录物体的缩放动作
    /// </summary>
    public class ScaleFunctional : BaseFunctional
    {
        public ScaleFunctional(Geometry2D gs) : base(gs)
        {
        }
    }
}
