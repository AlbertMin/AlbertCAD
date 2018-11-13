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
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.PenAction
{

    /// <summary>
    /// 当前的会线动作
    /// </summary>
    public class StaffAction : IAction
    {

        private string tipMessage = "直线绘制";
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ms"></param>
        public StaffAction()
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
        private StaffGeometry staffGeometry = new StaffGeometry();

        /// <summary>
        /// 绘制绘制元素
        /// </summary>
        public Geometry2D Geometry
        {
            get
            {
                return staffGeometry;
            }
        }


        /// <summary>
        /// 期望的点
        /// </summary>
        private Vector2D future = null;

        /// <summary>
        /// 获取当前的期望点
        /// </summary>
        public Vector2D Future
        {

            get
            {
                return future;
            }
        }
        /// <summary>
        /// 获取长度值
        /// </summary>
        public double Length
        {
            get
            {
                return 0;
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
                if (future != null)
                {
                    staffGeometry.PPoints.Remove(future);
                }
                future = p;
                staffGeometry.PPoints.Add(future);
                if (DrawEraseEvent != null)
                {
                    DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
                this.staffGeometry.Update();
            }

        }

        /// <summary>
        /// 设置当前选中点信息
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D v)
        {
            if (staffGeometry.PPoints == null)
            {
                staffGeometry.PPoints = new List<Vector2D>();
                staffGeometry.PPoints.Add(v);
                this.tipMessage = "请选择下一点";
                if (DrawStartEvent != null)
                {
                    Geometry.IsActioning = true;
                    DrawStartEvent(new ActionEventArgs(this.Geometry));
                }
            }
            else
            {
                staffGeometry.PPoints.Add(v);
            }

        }
        /// <summary>
        /// 停止继续绘制
        /// </summary>
        public void Complete()
        {
            if (staffGeometry.PPoints.Count > 2)
            {
                if (DrawCompleteEvent != null)
                {
                    if (future != null)
                    {
                        staffGeometry.PPoints.Remove(future);
                        this.staffGeometry.Update();

                    }
                    Geometry.IsActioning = false;
                    DrawCompleteEvent(new ActionEventArgs(this.Geometry));
                }

            }
            else
            {
                if (future != null)
                {
                    staffGeometry.PPoints.Remove(future);
                    future = null;
                    if (DrawTermimationEvent != null)
                    {
                        DrawTermimationEvent(new ActionEventArgs(this.Geometry));
                    }
                }
                else
                {
                    if (DrawTermimationEvent != null)
                    {
                        DrawTermimationEvent(new ActionEventArgs(this.Geometry));
                    }
                }

            }

        }

        /// <summary>
        /// 获取最后绘制的一个点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLastPoint()
        {
            if (staffGeometry.PPoints != null&&staffGeometry.PPoints.Count > 1)
            {
                return staffGeometry.PPoints[staffGeometry.PPoints.Count - 1];

            }
            return null;
        }

        /// <summary>
        /// 获取上一步的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {

            if (staffGeometry.PPoints != null && staffGeometry.PPoints.Count > 1)
            {
                return staffGeometry.PPoints[staffGeometry.PPoints.Count - 2];

            }
            return null;
        }

        /// <summary>
        /// 设置当前的结果
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
          
            this.staffGeometry.Update();
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
