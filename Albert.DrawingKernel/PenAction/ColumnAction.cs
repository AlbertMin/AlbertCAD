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
    public class ColumnAction : IAction
    {
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawTermimationEvent;
        public ColumnAction()
        {
    
        }
        private ColumnGeometry columnGeometry = new ColumnGeometry();
        public Geometry2D Geometry
        {
            get
            {
                return columnGeometry;
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
            return columnGeometry.Start;
        }

        public Vector2D GetPreviousPoint()
        {

            return columnGeometry.End;
        }
        public void Erase(Vector2D p)
        {
            if (!isPausing)
            {
                columnGeometry.End = p;
                if (this.DrawEraseEvent != null)
                {
                    this.DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
                this.columnGeometry.Update();
            }
        }

        /// <summary>
        /// 设置绘制点信息
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            if (columnGeometry.Start != null)
            {

                columnGeometry.End = p;
                this.columnGeometry.Update();
                this.Complete();
            }
            else {
                columnGeometry.Start = p;
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
            if (columnGeometry.Start != null && columnGeometry.End != null)
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
            this.columnGeometry.End = Vector2D.Create(this.columnGeometry.Start.X + result,this.columnGeometry.Start.Y);
            this.columnGeometry.Update();
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
