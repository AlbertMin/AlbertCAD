using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Util;
using Albert.DrawingKernel.Events;

namespace Albert.DrawingKernel.PenAction
{
    public class FloorAction : IAction
    {
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawTermimationEvent;

        public FloorAction()
        {

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
        /// 绘制的对象
        /// </summary>
        private FloorGeometry floorGeometry = new FloorGeometry();



        private bool starting = false;
        /// <summary>
        /// 当前是否已经开始绘制
        /// </summary>
        public bool Starting
        {
            get
            {
                return starting;
            }
        }

        /// <summary>
        /// 获取当前的元素
        /// </summary>
        public Geometry2D Geometry
        {
            get
            {
                return floorGeometry;
            }
        }

        /// <summary>
        /// 获取当前的绘制长度
        /// </summary>
        public double Length
        {
            get
            {
                return KernelProperty.GetDistance(this.floorGeometry.PPoints[this.floorGeometry.PPoints.Count - 1], this.floorGeometry.PPoints[this.floorGeometry.PPoints.Count - 2]);
            }

        }
        /// <summary>
        /// 设置橡皮筋效果
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Vector2D p)
        {
            if (!isPausing)
            {
                if (future != null)
                {
                    floorGeometry.PPoints.Remove(future);
                }
                future = p;
                floorGeometry.PPoints.Add(future);
                if (DrawEraseEvent != null)
                {
                    DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
                this.floorGeometry.Update();
            }
        }

        /// <summary>
        /// 设置要绘制的点
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            floorGeometry.PPoints.Add(p);
            if (DrawStartEvent != null && !starting)
            {
                Geometry.IsActioning = true;
                DrawStartEvent(new ActionEventArgs(this.Geometry));
            }
            starting = true;

        }

        /// <summary>
        /// 获取最后绘制的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLastPoint()
        {
            if (floorGeometry.PPoints.Count > 1)
            {
                return floorGeometry.PPoints[floorGeometry.PPoints.Count - 1];

            }
            return null;
        }
        /// <summary>
        /// 获取上一步的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {

            if (floorGeometry.PPoints.Count > 1)
            {
                return floorGeometry.PPoints[floorGeometry.PPoints.Count - 2];

            }
            return null;
        }
        /// <summary>
        /// 停止绘制
        /// </summary>
        public void Complete()
        {
            if (floorGeometry.PPoints.Count > 1)
            {
                if (DrawCompleteEvent != null)
                {
                    if (future != null)
                    {
                        floorGeometry.PPoints.Remove(future);
                        this.floorGeometry.Update();

                    }
                    Geometry.IsActioning = false;
                    DrawCompleteEvent(new ActionEventArgs(this.Geometry));
                }

            }
            else
            {
                if (future != null)
                {
                    floorGeometry.PPoints.Remove(future);
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
        /// 直接指定结果
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
            Vector2D v = (future - GetPreviousPoint()).Normalize();
            var end = v * result;
            var endP = GetPreviousPoint() + end;
            floorGeometry.PPoints.Remove(future);
            floorGeometry.PPoints.Add(endP);
            this.floorGeometry.Update();
            this.isPausing = false;
        }

        /// <summary>
        /// 停止当前的绘制
        /// </summary>
        public void Termimal()
        {
            this.Complete();
        }
        public string GetCommandStatus()
        {
            return "绘制提示字符";
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
