using Albert.DrawingKernel.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Functional
{
    public class BaseFunctional
    {

        /// <summary>
        /// 构造函数，初始化当前的基础动作
        /// </summary>
        /// <param name="gs"></param>
        public BaseFunctional(Geometry2D gs) {

            this.target = gs;
        }

        private Geometry2D target = null;
        /// <summary>
        /// 获取当前移动的目标对象
        /// </summary>
        public Geometry2D Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }
    }
}
