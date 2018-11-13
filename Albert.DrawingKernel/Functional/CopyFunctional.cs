using Albert.DrawingKernel.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Functional
{
    public class CopyFunctional : BaseFunctional
    {
        private Geometry2D newObject = null;

        public CopyFunctional(Geometry2D gs) : base(gs)
        {
        }

        /// <summary>
        /// 当前新添加的对象
        /// </summary>
        public Geometry2D NewObject {

            get {

                return newObject;
            }
            set {
                newObject = value;
            }
        }
    }
}
