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
    public class LegwireAction : IAction
    {

        private string tipMessage = "直线绘制";
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ms"></param>
        public LegwireAction()
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
        private LegwireGeometry legwireGeometry = new LegwireGeometry();

        /// <summary>
        /// 绘制绘制元素
        /// </summary>
        public Geometry2D Geometry
        {
            get
            {
                return legwireGeometry;
            }
        }

        /// <summary>
        /// 获取长度值
        /// </summary>
        public double Length
        {
            get
            {
                return KernelProperty.GetDistance(this.legwireGeometry.Start, this.legwireGeometry.End);
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
                this.legwireGeometry.End = p;
                if (DrawEraseEvent != null)
                {
                    DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
                this.legwireGeometry.Update();
            }


        }

        /// <summary>
        /// 设置当前选中点信息
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D v)
        {
            if (legwireGeometry.Start == null)
            {
                legwireGeometry.Start = v;
                legwireGeometry.End = v;
                this.tipMessage = "请指定终点";

                if (DrawStartEvent != null)
                {
                    Geometry.IsActioning = true;
                    DrawStartEvent(new ActionEventArgs(this.Geometry));
                }
            }
            else
            {

                legwireGeometry.End = v;
                this.legwireGeometry.Update();
                this.Complete();
            }

        }
        /// <summary>
        /// 停止继续绘制
        /// </summary>
        public void Complete()
        {
            if (legwireGeometry.Start != null && legwireGeometry.End != null)
            {

                if (DrawCompleteEvent != null)
                {
                    Geometry.IsActioning = false;
                    DrawCompleteEvent(new ActionEventArgs(this.legwireGeometry));

                }
            }

        }

        /// <summary>
        /// 获取最后绘制的一个点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLastPoint()
        {
            return legwireGeometry.End;
        }

        /// <summary>
        /// 获取上一步的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {

            return legwireGeometry.Start;
        }

        /// <summary>
        /// 设置当前的结果
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
            var RT = result;
          
            this.legwireGeometry.Update();
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

            if (DrawCompleteEvent != null)
            {

                this.DrawCompleteEvent(null);
            }
        }

        /// <summary>
        /// 获取当前的绘制状态
        /// </summary>
        /// <returns></returns>
        public string GetCommandStatus()
        {
            return tipMessage;
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
