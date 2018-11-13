using Albert.DrawingKernel.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Events
{
    /// <summary>
    /// 当前动作事件
    /// </summary>
    public class ActionEventArgs : EventArgs
    {

        private Geometry2D g = null;

        /// <summary>
        /// 当前时间的图形对象
        /// </summary>
        public Geometry2D Geometry2D
        {
            get
            {
                return g;
            }

            set
            {
                g = value;
            }
        }


        /// <summary>
        /// 构造函数，当前的事件类型
        /// </summary>
        /// <param name="g"></param>
        public ActionEventArgs(Geometry2D g, bool isContinuous = false)
        {

            this.Geometry2D = g;
            this.isContinuous = isContinuous;
        }


        private bool isContinuous = false;

        /// <summary>
        /// 当前事件命令是否是连续的
        /// </summary>
        public bool IsContinuous
        {
            get
            {
                return isContinuous;
            }

            set
            {
                isContinuous = value;
            }
        }

    }
}
