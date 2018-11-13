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
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.PenAction
{
    public class PolygonAction : IAction
    {
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawTermimationEvent;

        public PolygonAction()
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
        private PolygonGeometry polygonGeometry = new PolygonGeometry();



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
                return polygonGeometry;
            }
        }

        /// <summary>
        /// 获取当前的绘制长度
        /// </summary>
        public double Length
        {
            get
            {
                return KernelProperty.GetDistance(this.polygonGeometry.PPoints[this.polygonGeometry.PPoints.Count - 1], this.polygonGeometry.PPoints[this.polygonGeometry.PPoints.Count - 2]);
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
                    polygonGeometry.PPoints.Remove(future);
                }
                future = p;
                polygonGeometry.PPoints.Add(future);
                if (DrawEraseEvent != null)
                {
                    DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
                this.polygonGeometry.Update();
            }
        }

        /// <summary>
        /// 设置要绘制的点
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            polygonGeometry.PPoints.Add(p);
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
            if (polygonGeometry.PPoints.Count > 1)
            {
                return polygonGeometry.PPoints[polygonGeometry.PPoints.Count - 1];

            }
            return null;
        }
        /// <summary>
        /// 获取上一步的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {

            if (polygonGeometry.PPoints.Count > 1)
            {
                return polygonGeometry.PPoints[polygonGeometry.PPoints.Count - 2];

            }
            return null;
        }
        /// <summary>
        /// 停止绘制
        /// </summary>
        public void Complete()
        {
            if (polygonGeometry.PPoints.Count > 1)
            {
                if (DrawCompleteEvent != null)
                {
                    if (future != null)
                    {
                        polygonGeometry.PPoints.Remove(future);
                        this.polygonGeometry.Update();

                    }
                    Geometry.IsActioning = false;
                    DrawCompleteEvent(new ActionEventArgs(this.Geometry));
                }

            }
            else
            {
                if (future != null)
                {
                    polygonGeometry.PPoints.Remove(future);
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
            polygonGeometry.PPoints.Remove(future);
            polygonGeometry.PPoints.Add(endP);
            this.polygonGeometry.Update();
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
