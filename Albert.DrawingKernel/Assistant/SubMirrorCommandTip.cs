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
    public class SubMirrorCommandTip : BaseTip<MirrorCommand>
    {

        private MirrorCommand command = null;
        private SublineGeometry sublineGeometry = null;
        /// <summary>
        /// 当前的构造函数，初始化当前的绘制
        /// </summary>
        /// <param name="dc"></param>
        public SubMirrorCommandTip(DrawingControl dc, TextTip tip)
            : base(dc, tip)
        {
            sublineGeometry = new SublineGeometry();
            sublineGeometry.SublineOffset = 0;
            sublineGeometry.ShowSize = false;

        }

        /// <summary>
        /// 开始关心一个动作
        /// </summary>
        /// <param name="a"></param>
        public override void Attention(MirrorCommand a)
        {
            command = a;
            command.DrawStartEvent += A_DrawStartEvent;
            command.DrawEraseEvent += A_DrawEraseEvent;
            command.DrawTermimationEvent += A_DrawTermimationEvent;
            command.DrawCompleteEvent += A_DrawCompleteEvent;
        }

        private void A_DrawCompleteEvent(ActionEventArgs gs)
        {
            this.drawingControl.RemoveTemporaryVisual(sublineGeometry);
        }

        private void A_DrawTermimationEvent(ActionEventArgs gs)
        {
            this.drawingControl.RemoveTemporaryVisual(sublineGeometry);
        }

        private void A_DrawEraseEvent(ActionEventArgs gs)
        {
            var start = command.Start;
            var end = command.End;
            sublineGeometry.Start = start;
            sublineGeometry.End = end;
            if (sublineGeometry.Start.IsAlmostEqualTo(sublineGeometry.End))
            {
                sublineGeometry.Opacity = 0;
            }
            else {
                sublineGeometry.Opacity = 1;
                sublineGeometry.Update();
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
            command.DrawStartEvent -= A_DrawStartEvent;
            command.DrawEraseEvent -= A_DrawEraseEvent;
            command.DrawTermimationEvent -= A_DrawTermimationEvent;
            command.DrawCompleteEvent -= A_DrawCompleteEvent;

        }
    }
}
