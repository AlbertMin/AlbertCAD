using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Commands;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.Geometries.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.PenAction;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Events;

namespace Albert.DrawingKernel.Assistant
{
    public class SubArcTip : BaseTip<ArcAction>
    {

        /// <summary>
        /// 当前的半圆动作
        /// </summary>
        private ArcAction action = null;

        /// <summary>
        /// 辅助线
        /// </summary>
        private SublineGeometry sublineGeometry = null;

        /// <summary>
        /// 当前的构造函数，初始化当前的绘制
        /// </summary>
        /// <param name="dc"></param>
        public SubArcTip(DrawingControl dc, TextTip tip)
            : base(dc, tip)
        {
            sublineGeometry = new SublineGeometry();
            sublineGeometry.SublineOffset = 0;
            sublineGeometry.ShowSize = true;
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
        public override void Attention(ArcAction a)
        {
            action = a;
            action.DrawStartEvent += A_DrawStartEvent;
            action.DrawEraseEvent += A_DrawEraseEvent;
            action.DrawTermimationEvent += A_DrawTermimationEvent;
            action.DrawCompleteEvent += A_DrawCompleteEvent;
        }

        /// <summary>
        /// 绘制完成
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawCompleteEvent(ActionEventArgs gs)
        {
            this.drawingControl.RemoveTemporaryVisual(sublineGeometry);
            this.Tip.SetHide();
        }

        /// <summary>
        /// 绘制结束
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawTermimationEvent(ActionEventArgs gs)
        {
            this.drawingControl.RemoveTemporaryVisual(sublineGeometry);
            this.Tip.SetHide();
        }

        /// <summary>
        /// 绘制缩放
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawEraseEvent(ActionEventArgs gs)
        {

            ArcGeometry arc = action.Geometry as ArcGeometry;

            if (arc.Start != null && arc.End != null && arc.Central != null)
            {
                sublineGeometry.Start = arc.Start;
                if (arc.Start.Distance(arc.Central) < arc.Start.Distance(arc.End) / 2) {
                    sublineGeometry.End = Line2D.Create(arc.Start,arc.End).MiddlePoint;
                }
                else
                {

                    sublineGeometry.End = arc.Central;
                }
             
                if (sublineGeometry.Start.IsAlmostEqualTo(sublineGeometry.End))
                {
                    sublineGeometry.Opacity = 0;
                }
                else
                {
                    sublineGeometry.Opacity = 1;
                    sublineGeometry.Update();
                }
            }
            else
            {
                sublineGeometry.Start = arc.Start;
                sublineGeometry.End = arc.End;
                if (sublineGeometry.Start.IsAlmostEqualTo(sublineGeometry.End))
                {
                    sublineGeometry.Opacity = 0;
                }
                else
                {
                    sublineGeometry.Opacity = 1;
                    sublineGeometry.Update();
                }

            }
        }

        /// <summary>
        /// 开始绘制
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawStartEvent(ActionEventArgs gs)
        {
            this.drawingControl.AddTemporaryVisual(sublineGeometry);
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

        }
    }
}
