using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Util;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Events;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.PenAction
{
    public class PolylineAction : IAction
    {
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawTermimationEvent;

        public PolylineAction()
        {

        }

        /// <summary>
        /// 期望的点
        /// </summary>
        private Vector2D future = null;

        /// <summary>
        /// 获取当前的期望点
        /// </summary>
        public Vector2D Future {

            get {

                return future;
            }
        }
        /// <summary>
        /// 绘制的对象
        /// </summary>
        private PolylineGeometry polylineElement = new PolylineGeometry();




        /// <summary>
        /// 获取当前的元素
        /// </summary>
        public Geometry2D Geometry
        {
            get
            {
                return polylineElement;
            }
        }

        /// <summary>
        /// 获取当前的绘制长度
        /// </summary>
        public double Length
        {
            get
            {
                return KernelProperty.GetDistance(this.polylineElement.PPoints[this.polylineElement.PPoints.Count - 1], this.polylineElement.PPoints[this.polylineElement.PPoints.Count - 2]);
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
                    polylineElement.PPoints.Remove(future);
                }
                future = p;
                polylineElement.PPoints.Add(future);
                if (DrawEraseEvent != null)
                {
                    DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
                this.polylineElement.Update();
            }
        }

        /// <summary>
        /// 设置要绘制的点
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            polylineElement.PPoints.Add(p);
            if (DrawStartEvent != null && !Geometry.IsActioning)
            {
                Geometry.IsActioning = true;
                DrawStartEvent(new ActionEventArgs(this.Geometry));
            }

      
        }

        /// <summary>
        /// 获取上一步的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {

            if (polylineElement.PPoints.Count > 1)
            {
                return polylineElement.PPoints[polylineElement.PPoints.Count - 2];

            }
            return null;
        }
        /// <summary>
        /// 获取最后绘制的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLastPoint()
        {
            if (polylineElement.PPoints.Count > 1)
            {
                return polylineElement.PPoints[polylineElement.PPoints.Count - 1];

            }
            return null;
        }
        /// <summary>
        /// 停止绘制
        /// </summary>
        public void Complete()
        {
            if (polylineElement.PPoints.Count > 2)
            {
                if (DrawCompleteEvent != null)
                {
                    if (future != null)
                    {
                        polylineElement.PPoints.Remove(future);
                        this.polylineElement.Update();

                    }
                    Geometry.IsActioning = false;
                    DrawCompleteEvent(new ActionEventArgs(this.Geometry));
                }

            }
            else
            {
                if (future != null)
                {
                    polylineElement.PPoints.Remove(future);
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
            polylineElement.PPoints.Remove(future);
            polylineElement.PPoints.Add(endP);
            this.polylineElement.Update();
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
