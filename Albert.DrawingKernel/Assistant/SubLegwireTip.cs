using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.PenAction;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.Geometries.Temporary;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Events;

namespace Albert.DrawingKernel.Assistant
{
    public class SubLegwireTip : BaseTip<LegwireAction>
    {
        /// <summary>
        /// 当前的动作
        /// </summary>
        private LegwireAction action = null;

 
        /// <summary>
        /// 当前的构造函数，初始化当前的绘制
        /// </summary>
        /// <param name="dc"></param>
        public SubLegwireTip(DrawingControl dc, TextTip tip)
            : base(dc, tip)
        {

            Tip.TipChanged += tip_TipChanged;
            Tip.TipCompleted += tip_TipCompleted;
        }

        /// <summary>
        /// 提示完成的事件
        /// </summary>
        /// <param name="value"></param>
        void tip_TipCompleted(double value)
        {
            this.action.SetResult(value);
        }

        /// <summary>
        /// 提示改变事件
        /// </summary>
        /// <param name="value"></param>
        void tip_TipChanged(double value)
        {
            //停止当前的拖拽
            this.action.Pause();
        }

        /// <summary>
        /// 开始关心一个动作
        /// </summary>
        /// <param name="a"></param>
        public override void Attention(LegwireAction a)
        {
            action = a;
            action.DrawStartEvent += A_DrawStartEvent;
            action.DrawEraseEvent += A_DrawEraseEvent;
            action.DrawTermimationEvent += A_DrawTermimationEvent;
            action.DrawCompleteEvent += A_DrawCompleteEvent;
        }

        private void A_DrawCompleteEvent(ActionEventArgs gs)
        {
            this.Tip.SetHide();
        }

        private void A_DrawTermimationEvent(ActionEventArgs gs)
        {
            this.Tip.SetHide();
     
        }

        private void A_DrawEraseEvent(ActionEventArgs gs)
        {
            LegwireGeometry tg = (gs.Geometry2D as LegwireGeometry);
            this.Tip.SetText("|", tg.End, 0);
        }

        /// <summary>
        /// 开始绘制
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawStartEvent(ActionEventArgs gs)
        {
            LegwireGeometry tg = (gs.Geometry2D as LegwireGeometry);
            this.Tip.SetText("|", tg.End, 0);
        }

        /// <summary>
        /// 取消一个关心动作
        /// </summary>
        public override void unAttention()
        {
            action.DrawStartEvent -= A_DrawStartEvent;
            action.DrawEraseEvent -= A_DrawEraseEvent;
            action.DrawTermimationEvent -= A_DrawTermimationEvent;
            action.DrawCompleteEvent -= A_DrawCompleteEvent;
            Tip.TipChanged -= tip_TipChanged;
            Tip.TipCompleted -= tip_TipCompleted;
        }
    }
}
