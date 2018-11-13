using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Albert.DrawingKernel.Events;

namespace Albert.DrawingKernel.PenAction
{
    public class LineAction : IAction
    {

        private string commandMessage = "直线绘制";
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ms"></param>
        public LineAction()
        {

        }
        /// <summary>
        /// 当前绘制开始事件
        /// </summary>
        public event DrawHandler DrawStartEvent;
        /// <summary>
        /// 绘制中事件
        /// </summary>
        public event DrawHandler DrawEraseEvent;
        /// <summary>
        /// 绘制完成事件
        /// </summary>
        public event DrawHandler DrawCompleteEvent;
        /// <summary>
        /// 绘制被终止事件
        /// </summary>
        public event DrawHandler DrawTermimationEvent;

        /// <summary>
        /// 当前的线元素
        /// </summary>
        private LineGeometry line = new LineGeometry();

        /// <summary>
        /// 绘制绘制元素
        /// </summary>
        public Geometry2D Geometry
        {
            get
            {
                return line;
            }
        }

        /// <summary>
        /// 获取长度值
        /// </summary>
        public double Length
        {
            get
            {
                return KernelProperty.GetDistance(this.line.Start, this.line.End);
            }

        }


        /// <summary>
        /// 橡皮筋效果
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Vector2D p)
        {
            if (!isPausing)
            {
                this.line.End = p;
                if (DrawEraseEvent != null)
                {
                    DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
                this.line.Update();
            }


        }

        /// <summary>
        /// 设置当前选中点信息
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D v)
        {
            if (line.Start == null)
            {
                line.Start = v;
                this.commandMessage = "请指定终点";

                if (DrawStartEvent != null)
                {
                    Geometry.IsActioning = true;
                    DrawStartEvent(new ActionEventArgs(this.Geometry));
                }
            }
            else
            {

                line.End = v;
                this.line.Update();
                this.Complete();
            }

        }
        /// <summary>
        /// 停止继续绘制
        /// </summary>
        public void Complete()
        {
            if (line.Start != null && line.End != null)
            {

                if (DrawCompleteEvent != null)
                {
                    commandMessage = string.Empty;
                    Geometry.IsActioning = false;
                    DrawCompleteEvent(new ActionEventArgs(this.line));

                }

            }
            else
            {

                if (DrawTermimationEvent != null)
                {
                    commandMessage = string.Empty;
                    DrawTermimationEvent(new ActionEventArgs(this.line));

                }
            }

        }

        /// <summary>
        /// 获取最后绘制的一个点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLastPoint()
        {
            return line.End;
        }

        /// <summary>
        /// 获取上一步的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {

            return line.Start;
        }

        /// <summary>
        /// 设置当前的结果
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
            var RT = result;
            Vector2D v = (line.End - line.Start).Normalize();
            var end = v * result;
            var endP = line.Start + end;
            line.End = endP;
            this.line.Update();
            this.Complete();
        }

        /// <summary>
        /// 暂停当前的绘制
        /// </summary>
        public void Termimal()
        {
            if (this.DrawTermimationEvent != null)
            {
                this.DrawTermimationEvent(new ActionEventArgs(this.Geometry));
            }

            if (DrawCompleteEvent != null) {

                this.DrawCompleteEvent(null);
            }
        }

        /// <summary>
        /// 获取当前的绘制状态
        /// </summary>
        /// <returns></returns>
        public string GetCommandStatus()
        {
            return commandMessage;
        }


        private bool isPausing = false;
        /// <summary>
        /// 暂停绘制动作
        /// </summary>
        public void Pause()
        {
            isPausing = true;
        }
    }
}
