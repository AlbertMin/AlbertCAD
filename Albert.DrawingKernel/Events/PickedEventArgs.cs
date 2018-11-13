using Albert.DrawingKernel.Selector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Events
{
    public class PickedEventArgs : EventArgs
    {

        /// <summary>
        /// 当前的图形Pick事件对象
        /// </summary>
        /// <param name="igs"></param>
        public PickedEventArgs(IntersectGeometry ig)
        {
            interestGeometry = ig;
        }

        private IntersectGeometry interestGeometry = null;
        //选中的对象
        public IntersectGeometry InterestGeometry
        {
            get
            {
                return interestGeometry;
            }
            protected set
            {
                interestGeometry = value;
            }
        }

        public PickType PickType = PickType.SetSupport;

    }


    public enum PickType
    {
        /// <summary>
        /// 赋支座属性
        /// </summary>
        SetSupport = 0,
        /// <summary>
        /// 边界偏移
        /// </summary>
        EdgeOffset = 1,
        /// <summary>
        /// 当前的标尺
        /// </summary>
        Staff=2
    }
}
