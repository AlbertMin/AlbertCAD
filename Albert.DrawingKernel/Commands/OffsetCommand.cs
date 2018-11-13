using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Functional;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Selector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Events;

namespace Albert.DrawingKernel.Commands
{
    /// <summary>
    /// 偏移一个相同的形状出来
    /// </summary>
    public class OffsetCommand : ICommand
    {
        public event PenAction.DrawHandler DrawStartEvent;

        public event PenAction.DrawHandler DrawEraseEvent;

        public event PenAction.DrawHandler DrawCompleteEvent;

        public event PenAction.DrawHandler DrawTermimationEvent;
        public event RecordCommand RecordCommand;


        /// <summary>
        /// 过滤器
        /// </summary>
        private Filter.IPickFilter Filter = null;

        /// <summary>
        /// 当前临时的图形
        /// </summary>
        private Geometry2D GeometryShape = null;

        /// <summary>
        /// 要移动的目标图形
        /// </summary>
        private Geometry2D tagert = null;

        /// <summary>
        /// 命令的起点坐标
        /// </summary>
        private Vector2D start = null;

        /// <summary>
        /// 当前命令的起点坐标
        /// </summary>
        public Vector2D Start
        {

            get
            {

                return start;
            }
            set
            {

                start = value;
            }
        }


        private Vector2D end = null;
        /// <summary>
        /// 命令的终点坐标
        /// </summary>
        public Vector2D End
        {

            get
            {

                return end;
            }
            set
            {

                end = value;
            }
        }
        /// <summary>
        /// 指定一个偏移的图形元素
        /// </summary>
        /// <param name="shape"></param>
        public void SetIntersectGeometry(IntersectGeometry shape)
        {
            if (this.Filter != null)
            {
                //判断是不是过滤的对象
                if (this.Filter.AllowElement(shape.GeometryShape))
                {
                    this.tagert = shape.GeometryShape;
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
        /// 获取当前的目标对象
        /// </summary>
        /// <returns></returns>
        public Geometries.Geometry2D GetTarget()
        {
            return tagert;
        }


        /// <summary>
        /// 设置当前的偏移起点
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Albert.Geometry.Primitives.Vector2D p)
        {
            if (GeometryShape == null)
            {
                this.start = p;
                GeometryShape = tagert.Copy(true);
                if (DrawStartEvent != null)
                {
                    this.DrawStartEvent(new ActionEventArgs(GeometryShape));
                    this.GeometryShape.IsActioning = true;
                }
            }
            else
            {

                this.end = p;
                //完成当前的移动
                this.Complete();
            }
        }

        private Vector2D lastmove = Vector2D.Zero;
        /// <summary>
        /// 开始偏移
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Albert.Geometry.Primitives.Vector2D p)
        {
            this.end = p;

            //已经移动的距离
            Vector2D totlemove = this.end - this.start;
            //增量的移动距离
            var incrementmove = totlemove - lastmove;
            //最终移动距离
            lastmove = totlemove;
            //移动当前图形
            this.GeometryShape.Move(incrementmove);
            this.GeometryShape.Update();
            //擦除效果
            if (this.DrawEraseEvent != null)
            {
                this.DrawEraseEvent(new ActionEventArgs(GeometryShape));
            }
        }

        /// <summary>
        /// 完成当前的偏移复制工作
        /// </summary>
        public void Complete()
        {
            //已经移动的距离
            Vector2D totlemove = this.end - this.start;
            //增量的移动距离
            var incrementmove = totlemove - lastmove;
            //最终移动距离
            lastmove = totlemove;
            this.GeometryShape.Move(incrementmove);
            this.GeometryShape.Update();
            if (DrawCompleteEvent != null)
            {
                this.GeometryShape.IsActioning = false;
                this.DrawCompleteEvent(new ActionEventArgs(GeometryShape));
            }

     
        }

        /// <summary>
        /// 终止当前的偏移赋值
        /// </summary>
        public void Termimal()
        {
            if (DrawTermimationEvent != null)
            {
                this.GeometryShape.IsActioning = false;
                DrawTermimationEvent(new ActionEventArgs(GeometryShape));
            }
        }

        /// <summary>
        /// 获取最后第一个点
        /// </summary>
        /// <returns></returns>
        public Albert.Geometry.Primitives.Vector2D GetLastPoint()
        {
            return end;
        }

        /// <summary>
        /// 获取前面那个点
        /// </summary>
        /// <returns></returns>
        public Albert.Geometry.Primitives.Vector2D GetPreviousPoint()
        {
            return start;
        }

        /// <summary>
        /// 返回图形对象
        /// </summary>
        public Geometries.Geometry2D Geometry
        {
            get { return GeometryShape; }
        }

        public void SetResult(dynamic result)
        {
           
        }

        public string GetCommandStatus()
        {
            return "命令提示字符";
        }


        /// <summary>
        /// 暂停绘制动作
        /// </summary>
        public void Pause()
        {


        }
    }
}
