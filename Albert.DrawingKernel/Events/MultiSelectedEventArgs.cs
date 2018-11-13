using Albert.DrawingKernel.Selector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Albert.DrawingKernel.Events
{

    /// <summary>
    /// 图形事件参数
    /// </summary>
    public class MultiSelectedEventArgs : EventArgs
    {

        /// <summary>
        /// 当前的图形事件对象
        /// </summary>
        /// <param name="igs"></param>
        public MultiSelectedEventArgs(List<IntersectGeometry> igs)
        {
            interestGeometrys = igs;
            CanDeleted = true;
        }


        private List<IntersectGeometry> interestGeometrys = null;
        //选中的对象
        public List<IntersectGeometry> InterestGeometrys
        {
            get
            {
                return interestGeometrys;
            }
            protected set
            {
                interestGeometrys = value;
            }
        }

        public bool CanDeleted { get;  set; }
    }
}
