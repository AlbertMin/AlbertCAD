using Albert.DrawingKernel.PenAction;
using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Events
{

    /// <summary>
    /// 右手的点击事件
    /// </summary>
    public class RightMouseEventArgs : EventArgs
    {

        /// <summary>
        /// 构造函数，初始化当前的右键点击动作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="v"></param>
        public RightMouseEventArgs(IAction action, Vector2D v) {

            this.eAction = action;
            this.point = v;
        }

        private IAction eAction = null;

        /// <summary>
        /// 当前事件的对象
        /// </summary>
        public IAction EAction
        {
            get
            {
                return eAction;
            }

            set
            {
                eAction = value;
            }
        }

        /// <summary>
        /// 当前事件的点击点
        /// </summary>
        public Vector2D Point
        {
            get
            {
                return point;
            }

            set
            {
                point = value;
            }
        }

        private Vector2D point = null;
    }
}
