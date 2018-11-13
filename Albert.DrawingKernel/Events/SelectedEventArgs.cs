using Albert.DrawingKernel.Selector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Events
{
    public class SelectedEventArgs : EventArgs
    {

        /// <summary>
        /// 当前的图形事件对象
        /// </summary>
        /// <param name="igs"></param>
        public SelectedEventArgs(IntersectGeometry ig)
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
    }
}
