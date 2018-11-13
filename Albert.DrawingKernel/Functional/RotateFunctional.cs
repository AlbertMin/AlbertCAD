using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Geometries;

namespace Albert.DrawingKernel.Functional
{
    public class RotateFunctional : BaseFunctional
    {
     
        private Vector2D runOnPoint = null;
        /// <summary>
        /// 转动点
        /// </summary>
        public Vector2D RunOnPoint
        {

            get
            {
                return runOnPoint;
            }
            set
            {
                runOnPoint = value;
            }
        }


        private double degree = 0;

        public RotateFunctional(Geometry2D gs) : base(gs)
        {
        }

        /// <summary>
        /// 转动的角度
        /// </summary>
        public double Degree {

            get { return degree; }
            set { degree = value; }
        }
    }
}
