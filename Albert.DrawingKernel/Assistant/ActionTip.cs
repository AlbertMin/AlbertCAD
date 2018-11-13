using Albert.DrawingKernel.Assistant;
using Albert.DrawingKernel.Commands;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.PenAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Albert.DrawingKernel.Assistant
{
    /// <summary>
    /// 当前的直线提示
    /// </summary>
    public class ActionTip
    {
        /// <summary>
        /// 当前的绘制面板
        /// </summary>
        private DrawingControl DrawingControl = null;

        /// <summary>
        /// 当前的提示框
        /// </summary>
        private TextTip textTip = null;


        /// <summary>
        /// 当前的构造函数
        /// </summary>
        /// <param name="dc"></param>
        public ActionTip(DrawingControl dc)
        {

            DrawingControl = dc;

            textTip = new TextTip();
            textTip.Placement = PlacementMode.RelativePoint;
            textTip.ESCPressed += textTip_ESCPressed;
        }

        /// <summary>
        /// 按下了ESC
        /// </summary>
        /// <param name="value"></param>
        void textTip_ESCPressed(double value)
        {
            this.DrawingControl.Esc();
        }

        /// <summary>
        /// 当前的动作
        /// </summary>
        private IAction action = null;

        /// <summary>
        /// 当前的提示条信息
        /// </summary>
        private dynamic tip = null;

        /// <summary>
        /// 对外的函数信息
        /// </summary>
        /// <param name="action"></param>
        public void SetAction(IAction action)
        {
            this.action = action;
            if (tip != null)
            {
                tip.unAttention();
            }
            switch (action.GetType().Name)
            {
                case "LineAction":
                    SetAction(action as LineAction);
                    break;
                case "RectangleAction":
                    SetAction(action as RectangleAction);
                    break;
                case "PolylineAction":
                    SetAction(action as PolylineAction);
                    break;
                case "PolygonAction":
                    SetAction(action as PolygonAction);
                    break;
                case "EllipseAction":
                    SetAction(action as EllipseAction);
                    break;
                case "CircleAction":
                    SetAction(action as CircleAction);
                    break;
                case "WallAction":
                    SetAction(action as WallAction);
                    break;
                case "BeamAction":
                    SetAction(action as BeamAction);
                    break;
                case "TextAction":
                    SetAction(action as TextAction);
                    break;
                case "LegwireAction":
                    SetAction(action as LegwireAction);
                    break;
                case "ArcAction":
                    SetAction(action as ArcAction);
                    break;
                case "OSBAction":
                    SetAction(action as OSBAction);
                    break;
                case "MoveCommand":
                    this.SetAction(action as MoveCommand);
                    break;
                case "OffsetCommand":
                    this.SetAction(action as OffsetCommand);
                    break;
                case "RotateCommand":
                    this.SetAction(action as RotateCommand);
                    break;
                case "MeasureCommand":
                    this.SetAction(action as MeasureCommand);
                    break;
                case "MirrorCommand":
                    this.SetAction(action as MirrorCommand);
                    break;
                case "ArrayCommand":
                    this.SetAction(action as ArrayCommand);
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 发送键盘
        /// </summary>
        /// <param name="key"></param>
        public void SendKeyBorad(Key key, ModifierKeys m)
        {

            if ((key >= Key.NumPad0 && key <= Key.NumPad9) || (key >= Key.D0 && key <= Key.D9) || key == Key.Back || key == Key.Enter)
            {
                if (m == ModifierKeys.None && tip != null)
                {

                }
            }

        }
        /// <summary>
        /// 设定当前动作
        /// </summary>
        /// <param name="a"></param>
        private void SetAction(LineAction a)
        {
            tip = new SublineTip(DrawingControl, textTip);
            tip.Attention(a);
        }

        private void SetAction(PolylineAction a)
        {
            tip = new SubPolylineTip(DrawingControl, textTip);
            tip.Attention(a);
        }
        private void SetAction(RectangleAction a)
        {
            tip = new SubRectangleTip(DrawingControl, textTip);
            tip.Attention(a);
        }
        private void SetAction(PolygonAction a)
        {
            tip = new SubPolygonTip(DrawingControl, textTip);
            tip.Attention(a);
        }
        private void SetAction(EllipseAction a)
        {
            tip = new SubEllipseTip(DrawingControl, textTip);
            tip.Attention(a);
        }
        private void SetAction(CircleAction a)
        {
            tip = new SubCircleTip(DrawingControl, textTip);
            tip.Attention(a);
        }
        private void SetAction(WallAction a)
        {
            tip = new SubWallTip(DrawingControl, textTip);
            tip.Attention(a);
        }
        private void SetAction(LegwireAction a)
        {
            tip = new SubLegwireTip(DrawingControl, textTip);
            tip.Attention(a);
        }
        private void SetAction(BeamAction a)
        {
            tip = new SubBeamTip(DrawingControl, textTip);
            tip.Attention(a);

        }

        private void SetAction(TextAction a)
        {
            tip = new SubTextTip(DrawingControl, textTip);
            tip.Attention(a);

        }

        private void SetAction(OSBAction a)
        {

        }

        /// <summary>
        /// 绘制圆弧
        /// </summary>
        /// <param name="a"></param>
        public void SetAction(ArcAction a) {

            tip = new SubArcTip(DrawingControl, textTip);
            tip.Attention(a);
        }
        /// <summary>
        /// 一个移动命令
        /// </summary>
        /// <param name="c"></param>
        private void SetAction(MoveCommand c)
        {
            tip = new SubMoveCommandTip(DrawingControl, textTip);
            tip.Attention(c);
        }

        /// <summary>
        /// 一个偏移的命令
        /// </summary>
        /// <param name="c"></param>
        private void SetAction(OffsetCommand c)
        {
            tip = new SubOffsetCommandTip(DrawingControl, textTip);
            tip.Attention(c);
        }
        /// <summary>
        /// 一个转动的命令
        /// </summary>
        /// <param name="c"></param>
        private void SetAction(RotateCommand c)
        {
            tip = new SubRotateCommandTip(DrawingControl, textTip);
            tip.Attention(c);
        }

        /// <summary>
        /// 测量命令
        /// </summary>
        /// <param name="c"></param>
        private void SetAction(MeasureCommand c)
        {
            tip = new SubMeasureTip(DrawingControl, textTip);
            tip.Attention(c);
        }

        /// <summary>
        /// 设置当前的镜像功能
        /// </summary>
        /// <param name="c"></param>
        private void SetAction(MirrorCommand c) {
            tip = new SubMirrorCommandTip(DrawingControl, textTip);
            tip.Attention(c);
        }


        /// <summary>
        /// 设置当前阵列模式
        /// </summary>
        /// <param name="c"></param>
        private void SetAction(ArrayCommand c) {

            tip = new SubArrayCommandTip(DrawingControl, textTip);
            tip.Attention(c);
        }
    }
}
