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

namespace Albert.DrawingKernel.PenAction
{
    /// <summary>
    /// 绘制当前圆形的动作
    /// </summary>
    public class CircleAction : IAction
    {
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawTermimationEvent;
        public CircleAction()
        {
    
        }
        private CircleGeometry circle = new CircleGeometry();
        public Geometry2D Geometry
        {
            get
            {
                return circle;
            }
        }


        public double Length
        {
            get
            {
                return 0;
            }

        }
        public Vector2D GetLastPoint()
        {
            return circle.Start;
        }

        public Vector2D GetPreviousPoint()
        {

            return circle.End;
        }
        public void Erase(Vector2D p)
        {
            if (!isPausing)
            {
                circle.End = p;
                if (this.DrawEraseEvent != null)
                {
                    this.DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
                this.circle.Update();
            }
        }

        /// <summary>
        /// 设置绘制点信息
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            if (circle.Start != null)
            {

                circle.End = p;
                this.circle.Update();
                this.Complete();
            }
            else {
                circle.Start = p;
                if (DrawStartEvent != null) {
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
            if (circle.Start != null && circle.End != null)
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
            this.circle.End = Vector2D.Create(this.circle.Start.X + result,this.circle.Start.Y);
            this.circle.Update();
            this.Complete();
        }
        /// <summary>
        /// 终止绘制
        /// </summary>
        public void Termimal()
        {
            if (DrawTermimationEvent != null)
            {
                DrawTermimationEvent(new ActionEventArgs(Geometry));
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
