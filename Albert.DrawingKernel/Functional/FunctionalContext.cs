using Albert.DrawingKernel.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Functional
{

    public delegate void FunctionalHanlder(BaseFunctional bf);

    /// <summary>
    /// 用于管理当前的动作上下文信息
    /// </summary>
    public class FunctionalContext
    {

        
        /// <summary>
        /// 代表可以撤销的操作
        /// </summary>
        private Stack<dynamic> CanRevokes = new Stack<dynamic>();


        /// <summary>
        /// 定义以及撤销事件和撤销操作的通知
        /// </summary>
        public event FunctionalHanlder FunctionalEvent;

        /// <summary>
        /// 当前的绘制上下文对象
        /// </summary>
        public DrawingControl DrawingControl {

            private set;
            get;
        }
        /// <summary>
        /// 当前的构造函数，初始化当前的绘制动作
        /// </summary>
        /// <param name="dc"></param>
        public FunctionalContext(DrawingControl dc) {

            this.DrawingControl = dc;
        }

        /// <summary>
        /// 添加一个撤销动作
        /// </summary>
        /// <param name="bf"></param>
        public void PushFunctional(BaseFunctional bf)
        {
            //添加一个元素
            this.CanRevokes.Push(bf);
        }

        /// <summary>
        /// 撤销一个动作
        /// </summary>
        public void PopFunctional()
        {
            if (this.CanRevokes.Count > 0)
            {
                dynamic bf = this.CanRevokes.Pop();

                //处理当前的动作
                this.ExcuteFunctional(bf);

                if (FunctionalEvent != null)
                {
                    //把剔除的事务进行处理
                    FunctionalEvent(bf);

                }
            }
        }

        /// <summary>
        /// 清除所有可撤销动作，保留当前所有的操作
        /// </summary>
        public void Clear() {

            this.CanRevokes.Clear();
        }

        /// <summary>
        /// 处理添加动作
        /// </summary>
        /// <param name="add"></param>
        public void ExcuteFunctional(AddFunctional af) {

            this.DrawingControl.RemoveDrawingVisual(af.Target);

        }

        /// <summary>
        /// 处理移除的动作
        /// </summary>
        /// <param name="rf"></param>
        public void ExcuteFunctional(RemoveFunctional rf) {

            this.DrawingControl.AddDrawingVisual(rf.Target);
     
        }

        /// <summary>
        /// 对象的移动操作
        /// </summary>
        /// <param name="mf"></param>
        public void ExcuteFunctional(MoveFunction mf) {
            mf.Target.Move(-mf.Offset);
        }

        /// <summary>
        /// 对象的旋转操作
        /// </summary>
        /// <param name="mf"></param>
        public void ExcuteFunctional(RotateFunctional mf)
        {
            mf.Target.Rolate(mf.RunOnPoint,-mf.Degree);
            mf.Target.Update();
        }
        /// <summary>
        /// 处理不清楚的对象
        /// </summary>
        /// <param name="obj"></param>
        public void ExcuteFunctional(Object obj) {

        }
    }
}
