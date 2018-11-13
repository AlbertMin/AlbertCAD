using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Geometries.Temporary;
using Albert.DrawingKernel.PenAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Events;

namespace Albert.DrawingKernel.Assistant
{
    /// <summary>
    /// 矩形的提示对象
    /// </summary>
    public class SubRectangleTip : BaseTip<RectangleAction>
    {
        private RectangleAction action = null;
        private SublineGeometry sublineHGeometry = null;
        private SublineGeometry sublineVGeometry = null;
        /// <summary>
        /// 当前的构造函数，初始化当前的绘制
        /// </summary>
        /// <param name="dc"></param>
        public SubRectangleTip(DrawingControl dc, TextTip tip) :base(dc,tip)
        {
            sublineHGeometry = new SublineGeometry();
            sublineHGeometry.ShowSize = false;
            sublineVGeometry = new SublineGeometry();

            Tip.TipChanged += tip_TipChanged;
            Tip.TipCompleted += tip_TipCompleted;
        }
        /// <summary>
        /// 提示完成的事件
        /// </summary>
        /// <param name="value"></param>
        void tip_TipCompleted(double value)
        {
            if (this.action.ConfirmOne)
            {
                if ((sublineVGeometry.End.Y - sublineVGeometry.Start.Y) > 0)
                {
                    this.action.SetResult(value);
                }
                else
                {
                    this.action.SetResult(-value);
                }
            }
            else {
                if ((sublineHGeometry.End.X - sublineHGeometry.Start.X) > 0)
                {
                    this.action.SetResult(value);
                }
                else
                {
                    this.action.SetResult(-value);
                }
            }


            if (this.action.ConfirmOne) {
                sublineHGeometry.ShowSize = true;
                sublineVGeometry.ShowSize = false;
                Tip.SetText(sublineVGeometry.TextValue, sublineHGeometry.TextPosition, sublineHGeometry.TextAngle);
            }
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
        public override void Attention(RectangleAction a)
        {
            action = a;
            action.DrawStartEvent += A_DrawStartEvent;
            action.DrawEraseEvent += A_DrawEraseEvent;
            action.DrawTermimationEvent += A_DrawTermimationEvent;
            action.DrawCompleteEvent += A_DrawCompleteEvent;
        }

        private void A_DrawCompleteEvent(ActionEventArgs gs)
        {
            this.drawingControl.RemoveTemporaryVisual(sublineHGeometry);
            this.drawingControl.RemoveTemporaryVisual(sublineVGeometry);
            this.Tip.SetHide();
        }

        private void A_DrawTermimationEvent(ActionEventArgs gs)
        {
            this.drawingControl.RemoveTemporaryVisual(sublineHGeometry);
            this.drawingControl.RemoveTemporaryVisual(sublineVGeometry);
            this.Tip.SetHide();
        }

        private void A_DrawEraseEvent(ActionEventArgs gs)
        {
            RectangleGeometry rectangle = this.action.Geometry as RectangleGeometry;
            var start = rectangle.Start;
            var end = rectangle.End;
            sublineHGeometry.Start = start;
            sublineHGeometry.End = Vector2D.Create(end.X, start.Y);
            sublineHGeometry.Update();

            sublineVGeometry.Start = start;
            sublineVGeometry.End = Vector2D.Create(start.X, end.Y);
            sublineVGeometry.Update();

            if (!action.ConfirmOne)
            {
                Tip.SetText(sublineHGeometry.TextValue, sublineHGeometry.TextPosition, sublineHGeometry.TextAngle);
            }
            else
            {
                Tip.SetText(sublineVGeometry.TextValue, sublineVGeometry.TextPosition, sublineVGeometry.TextAngle);
            }
        }

        /// <summary>
        /// 开始绘制
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawStartEvent(ActionEventArgs gs)
        {
            this.drawingControl.AddTemporaryVisual(sublineHGeometry);
            this.drawingControl.AddTemporaryVisual(sublineVGeometry);
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
