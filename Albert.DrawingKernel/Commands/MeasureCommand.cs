using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Geometries.Temporary;
using Albert.DrawingKernel.PenAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Events;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Commands
{
    /// <summary>
    /// 当前启动一个测量命令
    /// </summary>
    public class MeasureCommand :IAction
    {


        public event DrawHandler DrawStartEvent;

        public event DrawHandler DrawEraseEvent;

        public event DrawHandler DrawCompleteEvent;

        public event DrawHandler DrawTermimationEvent;

        private MeasureGeometry measureGeometry = new MeasureGeometry();

        /// <summary>
        /// 指定起点位置
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {

            if (measureGeometry.IsActioning)
            {
                measureGeometry.End = p;
                this.Complete();
            }
            else
            {
                measureGeometry.Start = p;

                if (DrawStartEvent != null)
                {
                    DrawStartEvent(new ActionEventArgs(measureGeometry));
                }
            }
            measureGeometry.IsActioning = true;
        }

        /// <summary>
        /// 拖动绘制当前对象
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Vector2D p)
        {
            if (measureGeometry.IsActioning)
            {
                measureGeometry.End = p;
                measureGeometry.Update();

                if (DrawEraseEvent != null) {

                    DrawEraseEvent(new ActionEventArgs(measureGeometry));
                }
            }
        }

        /// <summary>
        /// 完成当前的绘制操作
        /// </summary>
        public void Complete()
        {
            if (this.DrawCompleteEvent != null)
            {
                this.DrawCompleteEvent(new ActionEventArgs(Geometry));
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

        /// <summary>
        /// 获取最后绘制的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLastPoint()
        {
            return measureGeometry.End;
        }

        /// <summary>
        /// 获取之前绘制的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {
            return measureGeometry.Start;
        }

        /// <summary>
        /// 返回当前的图形元素
        /// </summary>
        public Geometries.Geometry2D Geometry
        {
            get {
                return measureGeometry; 
            }
        }

        /// <summary>
        /// 设置当前的结果
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
            var RT = result;
            Vector2D v = (measureGeometry.End - measureGeometry.Start).Normalize();
            var end = v * result;
            var endP = measureGeometry.Start + end;
            measureGeometry.End = endP;
            this.measureGeometry.Update();
            this.Complete();
        }

        public string GetCommandStatus()
        {
            return "开始进行测量";
        }

        /// <summary>
        /// 暂停绘制
        /// </summary>
        public void Pause()
        {
           
        }
    }
}
