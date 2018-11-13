using Albert.DrawingKernel.PenAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Util;
using Albert.DrawingKernel.Events;

namespace Albert.DrawingKernel.Assistant
{
    public class SubTextTip : BaseTip<TextAction>
    {

        /// <summary>
        /// 当前的动作
        /// </summary>
        private TextAction action = null;

        public SubTextTip(DrawingControl dc, TextTip tip) : base(dc, tip)
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
            this.action.SetResult(value.ToString());
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
        public override void Attention(TextAction a)
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
           
        }

        /// <summary>
        /// 开始绘制
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawStartEvent(ActionEventArgs gs)
        {
            TextGeometry tg = (gs.Geometry2D as TextGeometry);
            var md = KernelProperty.MMToPix(tg.FontSize);
            Vector2D nv = Vector2D.Create(tg.Central.X, tg.Central.Y- md / 2);
            this.Tip.SetText("|", nv, 0);
        }

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
