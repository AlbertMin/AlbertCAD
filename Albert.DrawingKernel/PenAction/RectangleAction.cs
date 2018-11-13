using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Geometries;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Events;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.PenAction
{
    /// <summary>
    /// 绘制一个矩形的动作
    /// </summary>
    public class RectangleAction : IAction
    {
        /// <summary>
        /// 绘制的主要事件
        /// </summary>
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawTermimationEvent;

        /// <summary>
        /// 矩形的图形对象
        /// </summary>
        private RectangleGeometry rectangle = new RectangleGeometry();

        public Vector2D GetLastPoint()
        {
            return rectangle.End;
        }

        /// <summary>
        /// 获取上一步的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {

            return rectangle.Start;
        }
        /// <summary>
        /// 获取图形对象
        /// </summary>
        public Geometry2D Geometry
        {
            get
            {
                return rectangle;
            }
        }
        /// <summary>
        /// 当前的动作构造函数
        /// </summary>
        public RectangleAction()
        {

        }


        private double width = 0;
        private double height = 0;

        /// <summary>
        /// 矩形的宽度
        /// </summary>
        public double Width {

            get {
                width = rectangle.End.X - rectangle.Start.X;
                return width;
            }
            set {
                width = value;
                rectangle.End = new Vector2D(rectangle.Start.X + width, rectangle.End.Y);
         
                isConfirmOne = true;
            }
        }
        /// <summary>
        /// 矩形的高度
        /// </summary>
        public double Height {

            get {
                height= rectangle.End.Y - rectangle.Start.Y;
                return height;
            }
            set {
                height = value;
                rectangle.End = new Vector2D(rectangle.Start.X + width, rectangle.Start.Y+height);
            }
        }

        private bool isConfirmOne = false;
        /// <summary>
        /// 判断当前是否已经确定了宽度
        /// </summary>
        public bool ConfirmOne
        {
            get { return isConfirmOne; }
            private set
            {
                isConfirmOne = value;
            }
        }

        /// <summary>
        /// 拖拽效果
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Vector2D p)
        {
            if (!isPausing)
            {
                if (isConfirmOne)
                {
                    rectangle.End = new Vector2D(rectangle.Start.X + width, p.Y);
                }
                else
                {
                    rectangle.End = p;
                }

                this.rectangle.Update();
                if (this.DrawEraseEvent != null)
                {
                    this.DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
            }
        }

        /// <summary>
        /// 设置矩形的点位
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
  
            if (rectangle.Start != null)
            {

                rectangle.End = p;
                this.rectangle.Update();
                this.Complete();
            }
            else {
                rectangle.Start = p;
                if (this.DrawStartEvent != null) {
                    Geometry.IsActioning = true;
                    this.DrawStartEvent(new ActionEventArgs(this.Geometry));
                }
            }
         
        }
        /// <summary>
        /// 完成绘制
        /// </summary>
        public void Complete()
        {
            if (rectangle.Start != null && rectangle.End != null)
            {
                if (this.DrawCompleteEvent != null)
                {
                    Geometry.IsActioning = false;
                    this.DrawCompleteEvent(new ActionEventArgs(this.Geometry));
                }
            }
            else
            {
                if (this.DrawTermimationEvent != null)
                {
                    this.DrawTermimationEvent(new ActionEventArgs(this.Geometry));
                }
            }
        }

        /// <summary>
        /// 直接指定绘制结果
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
            if (ConfirmOne)
            {
                Height = result;
                this.rectangle.Update();
                this.Complete();
            }
            else {
                Width = result;
                this.rectangle.Update();
                isPausing = false;
   
            }
        
        }

        /// <summary>
        /// 终止当前的绘制
        /// </summary>
        public void Termimal()
        {
            if (this.DrawTermimationEvent != null)
            {
                this.DrawTermimationEvent(new ActionEventArgs(this.Geometry));
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
