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

namespace Albert.DrawingKernel.PenAction
{
    public class MemberAction : IAction
    {
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawTermimationEvent;
        /// <summary>
        /// 构造函数，初始化一个墙体绘制动作
        /// </summary>
        public MemberAction() { }

        /// <summary>
        /// 总是绘制的最后一个墙体
        /// </summary>
        private MemberGeometry beam = new MemberGeometry();

        private string commandMessage = "绘制梁，请选择梁的起点";

        /// <summary>
        /// 获取当前的图形对象
        /// </summary>
        public Geometry2D Geometry
        {

            get
            {
                return beam;
            }
        }

        /// <summary>
        /// 获取最后绘制点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLastPoint()
        {

            return this.beam.End;
        }
        public Vector2D GetPreviousPoint()
        {

            return beam.Start;
        }
        /// <summary>
        /// 获取当前墙的长度
        /// </summary>
        public double Length
        {
            get
            {
                return this.beam.End.Distance(this.beam.Start);
            }

        }

        /// <summary>
        /// 指定点开始绘制
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            if (beam.Start != null)
            {

                beam.End = p;
                beam.Update();
                this.Complete();
            }
            else
            {
                beam.Start = p;
                if (DrawStartEvent != null)
                {
                    commandMessage = "绘制梁，请选择梁的终点";
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
                beam.End = p;
                if (DrawEraseEvent != null)
                {
                    commandMessage = "绘制梁，请选择梁的终点";
                    DrawEraseEvent(new ActionEventArgs(this.Geometry));

                }
                this.beam.Update();
            }
        }

        /// <summary>
        /// 绘制完成
        /// </summary>
        public void Complete()
        {

            if (beam.Start != null && beam.End != null)
            {

                if (DrawCompleteEvent != null)
                {
                    commandMessage = string.Empty;
                    Geometry.IsActioning = false;
                    DrawCompleteEvent(new ActionEventArgs(this.beam));
                  
                }

            }
            else {

                if (DrawTermimationEvent != null)
                {
                    commandMessage = string.Empty;
                    DrawTermimationEvent(new ActionEventArgs(this.beam));
                   
                }
            }
           
        }

        public void SetResult(dynamic result)
        {
            var RT = result;
            Vector2D v = (beam.End - beam.Start).Normalize();
            var end = v * result;
            var endP = beam.Start + end;
            beam.End = endP;
            this.beam.Update();
            this.Complete();
        }

        /// <summary>
        /// 终止当前的绘制
        /// </summary>
        public void Termimal()
        {
            if (DrawTermimationEvent != null)
            {
                commandMessage = string.Empty;
                DrawTermimationEvent(new ActionEventArgs(this.Geometry));
            }
          
        }


        public string  GetCommandStatus()
        {
            return commandMessage;
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