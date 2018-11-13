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

/// <summary>
/// 用于定义墙体的绘制
/// </summary>
namespace Albert.DrawingKernel.PenAction
{
    public class WallAction : IAction
    {

        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawTermimationEvent;
        /// <summary>
        /// 构造函数，初始化一个墙体绘制动作
        /// </summary>
        public WallAction() {
         
        }


        /// <summary>
        /// 总是绘制的最后一个墙体
        /// </summary>
        private WallGeometry wallGeometry = new WallGeometry();

        /// <summary>
        /// 获取当前的图形对象
        /// </summary>
        public Geometry2D Geometry
        {

            get
            {
                return wallGeometry;
            }
        }

        /// <summary>
        /// 获取最后绘制点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLastPoint()
        {

            return this.wallGeometry.End;
        }

        /// <summary>
        /// 获取上一步的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {
            return wallGeometry.Start;
        }
        /// <summary>
        /// 获取当前墙的长度
        /// </summary>
        public double Length
        {
            get
            {
                return this.wallGeometry.End.Distance(this.wallGeometry.Start);
            }

        }

        /// <summary>
        /// 指定点开始绘制
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            if (wallGeometry.Start != null)
            {

                wallGeometry.End = p;
                wallGeometry.Update();
                this.Complete();
            }
            else
            {
                wallGeometry.Start = p;
                if (DrawStartEvent != null)
                {
                    Geometry.IsActioning = true;
                    DrawStartEvent(new ActionEventArgs(this.Geometry));
                }

            }

        }
        /// <summary>
        /// 开始缩放绘制
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Vector2D p)
        {
            if (!isPausing)
            {
                wallGeometry.End = p;
                this.wallGeometry.Update();
                if (DrawEraseEvent != null)
                {
                    DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
            }
        }

        /// <summary>
        /// 绘制完成
        /// </summary>
        public void Complete()
        {

            if (wallGeometry.Start != null && wallGeometry.End != null)
            {
                WallGeometry wall2 = new WallGeometry();
                wall2.Start = wallGeometry.End;
                wall2.PenColor = wallGeometry.PenColor;
                wall2.LineWidth = wallGeometry.LineWidth;
                wall2.Thickness = wallGeometry.Thickness;
                this.wallGeometry = wall2;
                if (DrawStartEvent != null)
                {
                    Geometry.IsActioning = true;
                    DrawStartEvent(new ActionEventArgs(this.Geometry));
                }
    


            }
        }

        public void SetResult(dynamic result)
        {
            var RT = result;
            Vector2D v = (wallGeometry.End - wallGeometry.Start).Normalize();
            var end = v * result;
            var endP = wallGeometry.Start + end;
            wallGeometry.End = endP;
            this.wallGeometry.Update();
            this.Complete();
        }

        /// <summary>
        /// 终止当前的绘制
        /// </summary>
        public void Termimal()
        {
            if (DrawTermimationEvent != null)
            {
                DrawTermimationEvent(new ActionEventArgs(this.Geometry));
            }
            if (DrawCompleteEvent != null) { }
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
