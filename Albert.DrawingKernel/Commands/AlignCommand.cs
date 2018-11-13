using Albert.Geometry.Primitives;
using Albert.Geometry.External;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Selector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Functional;
using Albert.DrawingKernel.Events;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Commands
{
    /// <summary>
    /// 对齐命令，将两个实体进行对齐操作
    /// </summary>
    public class AlignCommand : ICommand
    {

        /// <summary>
        /// 当前的主要事件动作
        /// </summary>
        public event PenAction.DrawHandler DrawStartEvent;

        public event PenAction.DrawHandler DrawEraseEvent;

        public event PenAction.DrawHandler DrawCompleteEvent;

        public event PenAction.DrawHandler DrawTermimationEvent;
        public event RecordCommand RecordCommand;


        private string TipMessage = "启动对齐命令，请选择要对齐的边";
        /// <summary>
        /// 当前临时的图形
        /// </summary>
        private Geometry2D referShape = null;

        /// <summary>
        /// 要移动的目标图形
        /// </summary>
        private Geometry2D tagert = null;

        /// <summary>
        /// 过滤器
        /// </summary>
        private Filter.IPickFilter Filter = null;
        /// <summary>
        /// 目标图形的对齐线
        /// </summary>
        private Line2D targetLine = null;
        /// <summary>
        /// 目标图形的参照线
        /// </summary>
        private Line2D referLine = null;

        /// <summary>
        /// 指定当前的目标图形
        /// </summary>
        /// <param name="shape"></param>
        public void SetIntersectGeometry(IntersectGeometry shape)
        {
            if(shape.GeometryShape!=null&&shape.IntersectPoint.Line!=null){

                if (this.tagert == null)
                {
                    if (this.Filter != null)
                    {
                        //判断是不是过滤的对象
                        if (this.Filter.AllowElement(shape.GeometryShape))
                        {
                            this.tagert = shape.GeometryShape;
                            this.targetLine = shape.IntersectPoint.Line;
                            TipMessage = "请选择参照边";
                        }
                    }
                  
            
                }
                else
                {
                    this.referShape = shape.GeometryShape;
                    this.referLine = shape.IntersectPoint.Line;
                    this.Complete();
                }
            }

        }


        /// <summary>
        /// 当前的命令过滤
        /// </summary>
        /// <param name="f"></param>
        public void SetPickFilter(Filter.IPickFilter pf)
        {
            this.Filter = pf;
        }
        /// <summary>
        /// 对齐功能和其他命令不太一样，记录当前是否指定了参照对象。
        /// </summary>
        /// <returns></returns>
        public Geometries.Geometry2D GetTarget()
        {
            return this.referShape;
        }


        /// <summary>
        /// 指定当前的对齐点
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Albert.Geometry.Primitives.Vector2D p)
        {

            if (DrawStartEvent != null) { 
            

            }

     
        }

        /// <summary>
        /// 对齐尝试
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Albert.Geometry.Primitives.Vector2D p)
        {
            if (DrawEraseEvent != null) { 
            
            }
        }

        /// <summary>
        /// 完成对齐操作
        /// </summary>
        public void Complete()
        {

            this.Termimal();
            ///只有两条线平齐的情况下，才能做偏移操作
            if (targetLine.Direction.IsAlmostEqualTo(referLine.Direction) || targetLine.Direction.IsAlmostEqualTo(-referLine.Direction))
            {
                if (!targetLine.IsAlmostEqualTo(referLine))
                {

                    //计算要偏移的距离
                    Vector2D v = targetLine.Start.ProjectOn(referLine);
                    Vector2D movev = v - targetLine.Start;
                    this.tagert.Move(movev);
                    this.tagert.Update();

                    if (DrawCompleteEvent != null)
                    {
                        DrawCompleteEvent(new ActionEventArgs(tagert));
                    }
                    if (RecordCommand != null)
                    {
                        MoveFunction mf = new MoveFunction(this.tagert);
                        mf.Offset = movev;
                        RecordCommand(this, mf);
                    }
                }
            }
            else
            {

                if (DrawCompleteEvent != null)
                {
                    DrawCompleteEvent(null);
                }
            }

    

        }

        /// <summary>
        /// 终止操作
        /// </summary>
        public void Termimal()
        {
            if (DrawTermimationEvent != null) {

                DrawTermimationEvent(null);
            }
        }

        /// <summary>
        /// 获取最后的点
        /// </summary>
        /// <returns></returns>
        public Albert.Geometry.Primitives.Vector2D GetLastPoint()
        {
            return null;
        }

        /// <summary>
        /// 获取前面的点
        /// </summary>
        /// <returns></returns>
        public Albert.Geometry.Primitives.Vector2D GetPreviousPoint()
        {

            return null;
        }

        /// <summary>
        /// 获取当前的图形信息
        /// </summary>
        public Geometries.Geometry2D Geometry
        {
            get { 
                return this.referShape; 
            }
        }

        /// <summary>
        /// 直接指定结果
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
         
        }

        /// <summary>
        /// 获取当前的操作状态
        /// </summary>
        /// <returns></returns>
        public string GetCommandStatus()
        {
            return this.TipMessage;
        }


        /// <summary>
        /// 暂停绘制动作
        /// </summary>
        public void Pause()
        {

 
        }
    }
}
