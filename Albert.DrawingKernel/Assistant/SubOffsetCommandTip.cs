using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Commands;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.Geometries.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Events;

namespace Albert.DrawingKernel.Assistant
{
    public class SubOffsetCommandTip : BaseTip<OffsetCommand>
    {

        private OffsetCommand command = null;
        private SublineGeometry sublineHGeometry = null;
        private SublineGeometry sublineVGeometry = null;
        private SublineGeometry sublineJGeometry = null;
        /// <summary>
        /// 当前的构造函数，初始化当前的绘制
        /// </summary>
        /// <param name="dc"></param>
        public SubOffsetCommandTip(DrawingControl dc, TextTip tip)
            : base(dc, tip)
        {
            sublineHGeometry = new SublineGeometry();
            sublineHGeometry.SublineOffset = 0;
            sublineVGeometry = new SublineGeometry();
            sublineVGeometry.SublineOffset = 0;
            sublineJGeometry = new SublineGeometry();
            sublineJGeometry.SublineOffset = 0;
        }

        /// <summary>
        /// 开始关心一个动作
        /// </summary>
        /// <param name="a"></param>
        public override void Attention(OffsetCommand a)
        {
            command = a;
            command.DrawStartEvent += A_DrawStartEvent;
            command.DrawEraseEvent += A_DrawEraseEvent;
            command.DrawTermimationEvent += A_DrawTermimationEvent;
            command.DrawCompleteEvent += A_DrawCompleteEvent;
        }

        private void A_DrawCompleteEvent(ActionEventArgs gs)
        {
            this.drawingControl.RemoveTemporaryVisual(sublineHGeometry);
            this.drawingControl.RemoveTemporaryVisual(sublineVGeometry);
            this.drawingControl.RemoveTemporaryVisual(sublineJGeometry);
        }

        private void A_DrawTermimationEvent(ActionEventArgs gs)
        {
            this.drawingControl.RemoveTemporaryVisual(sublineHGeometry);
            this.drawingControl.RemoveTemporaryVisual(sublineVGeometry);
            this.drawingControl.RemoveTemporaryVisual(sublineJGeometry);
        }

        private void A_DrawEraseEvent(ActionEventArgs gs)
        {
            var start = command.Start;
            var end = command.End;
            sublineHGeometry.Start = start;
            sublineHGeometry.End = Vector2D.Create(end.X, start.Y);
            if (sublineHGeometry.Start.IsAlmostEqualTo(sublineHGeometry.End))
            {
                sublineHGeometry.Opacity = 0;
            }
            else {
                sublineHGeometry.Opacity = 1;
                sublineHGeometry.Update();
            }

            sublineVGeometry.Start = sublineHGeometry.End;
            sublineVGeometry.End = Vector2D.Create(sublineHGeometry.End.X, end.Y);
            if (sublineVGeometry.Start.IsAlmostEqualTo(sublineVGeometry.End))
            {
                sublineVGeometry.Opacity = 0;
            }
            else
            {
                sublineVGeometry.Opacity = 1;
                sublineVGeometry.Update();
            }

            sublineJGeometry.Start = sublineHGeometry.Start;
            sublineJGeometry.End = sublineVGeometry.End;
            if (sublineHGeometry.Opacity == 0 || sublineVGeometry.Opacity == 0)
            {
                sublineJGeometry.Opacity = 0;
            }
            else {
                sublineJGeometry.Opacity = 1;
                sublineJGeometry.Update();
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
            this.drawingControl.AddTemporaryVisual(sublineJGeometry);
        }

        /// <summary>
        /// 取消一个关心动作
        /// </summary>
        public override void unAttention()
        {
            command.DrawStartEvent -= A_DrawStartEvent;
            command.DrawEraseEvent -= A_DrawEraseEvent;
            command.DrawTermimationEvent -= A_DrawTermimationEvent;
            command.DrawCompleteEvent -= A_DrawCompleteEvent;

        }
    }
}
