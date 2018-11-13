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
    public class SubRotateCommandTip : BaseTip<RotateCommand>
    {

        private RotateCommand command = null;
        private SublineGeometry sublineHGeometry = null;
        private SublineGeometry sublineVGeometry = null;
        private AngleGeometry angleGeometry = null;
        /// <summary>
        /// 当前的构造函数，初始化当前的绘制
        /// </summary>
        /// <param name="dc"></param>
        public SubRotateCommandTip(DrawingControl dc, TextTip tip)
            : base(dc, tip)
        {
            sublineHGeometry = new SublineGeometry();
            sublineHGeometry.SublineOffset = 0;
            sublineHGeometry.ShowSize = false;
            sublineVGeometry = new SublineGeometry();
            sublineVGeometry.SublineOffset = 0;
            sublineVGeometry.ShowSize = false;
            angleGeometry = new AngleGeometry();
        }

        /// <summary>
        /// 开始关心一个动作
        /// </summary>
        /// <param name="a"></param>
        public override void Attention(RotateCommand a)
        {
            command = a;
            command.DrawStartEvent += A_DrawStartEvent;
            command.DrawEraseEvent += A_DrawEraseEvent;
            command.DrawTermimationEvent += A_DrawTermimationEvent;
            command.DrawCompleteEvent += A_DrawCompleteEvent;
        }

        /// <summary>
        /// 绘制完成
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawCompleteEvent(ActionEventArgs gs)
        {
            this.drawingControl.RemoveTemporaryVisual(sublineHGeometry);
            this.drawingControl.RemoveTemporaryVisual(sublineVGeometry);
            this.drawingControl.RemoveTemporaryVisual(angleGeometry);
        }

        /// <summary>
        /// 绘制被终止
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawTermimationEvent(ActionEventArgs gs)
        {
            this.drawingControl.RemoveTemporaryVisual(sublineHGeometry);
            this.drawingControl.RemoveTemporaryVisual(sublineVGeometry);
            this.drawingControl.RemoveTemporaryVisual(angleGeometry);
        }

        /// <summary>
        /// 进行拖拽转动
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawEraseEvent(ActionEventArgs gs)
        {
            var start = command.Start;
            var end = command.End;
            sublineHGeometry.Start = start;
            sublineHGeometry.End = Vector2D.Create(start.X + 500, start.Y);
            sublineHGeometry.Update();


            sublineVGeometry.Start = start;
            sublineVGeometry.End = start + Line2D.Create(start, end).Direction * 500;
            sublineVGeometry.Update();

            angleGeometry.Start = Vector2D.Create(start.X + 100, start.Y); ;
            angleGeometry.End = start + Line2D.Create(start, end).Direction * 100;
            angleGeometry.Angle = Line2D.Create(start, end).Direction.AngleFrom(Vector2D.BasisX);
            angleGeometry.Update();
    
        }

        /// <summary>
        /// 开始绘制
        /// </summary>
        /// <param name="gs"></param>
        private void A_DrawStartEvent(ActionEventArgs gs)
        {
            this.drawingControl.AddTemporaryVisual(sublineHGeometry);
            this.drawingControl.AddTemporaryVisual(sublineVGeometry);
            this.drawingControl.AddTemporaryVisual(angleGeometry);
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
