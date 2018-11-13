using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Primitives;
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
    /// 绘制当前圆形的动作
    /// </summary>
    public class ArcAction : IAction
    {
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawTermimationEvent;
        public ArcAction()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private ArcGeometry arcGeometry = new ArcGeometry();


        public Geometry2D Geometry
        {
            get
            {
                return arcGeometry;
            }
        }


        /// <summary>
        /// 获取当前最后一个点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLastPoint()
        {
            return arcGeometry.Start;
        }

        /// <summary>
        /// 获取上一个点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {
            return arcGeometry.End;
        }

        /// <summary>
        /// 当前的绘制选择
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Vector2D p)
        {
            if (!isPausing)
            {
                if (Tht)
                {
                    arcGeometry.Central = p;

                }
                else
                {
                    arcGeometry.End = p;
             
                }
                if (this.DrawEraseEvent != null)
                {
                    this.DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
                this.arcGeometry.Update();
            }
        }

        private bool Tht = false;

        /// <summary>
        /// 设置绘制点信息
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            if (arcGeometry.Start != null && Tht)
            {
                arcGeometry.Central = p;
                this.arcGeometry.Update();
                this.Complete();
            }
            else if (arcGeometry.Start != null && !Tht)
            {
                arcGeometry.End = p;
                this.arcGeometry.Update();
                Tht = true;
            }
            else
            {
                arcGeometry.Start = p;
                if (DrawStartEvent != null)
                {
                    Geometry.IsActioning = true;
                    this.DrawStartEvent(new ActionEventArgs(this.Geometry));
                }
            }

        }

        /// <summary>
        /// 停止当前的绘制
        /// </summary>
        public void Complete()
        {
            if (arcGeometry.Start != null && arcGeometry.End != null && arcGeometry.Central != null)
            {
                if (this.DrawCompleteEvent != null)
                {
                    DrawCompleteEvent(new ActionEventArgs(this.Geometry));
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

        /// <summary>
        /// 直接指定结果，不重新绘制
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
            this.arcGeometry.End = Vector2D.Create(this.arcGeometry.Start.X + result, this.arcGeometry.Start.Y);
            this.arcGeometry.Update();
            this.Complete();
        }
        /// <summary>
        /// 终止绘制
        /// </summary>
        public void Termimal()
        {
            if (DrawTermimationEvent != null)
            {
                DrawTermimationEvent(new ActionEventArgs(this.Geometry));
            }
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
