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
using Albert.DrawingKernel.Filter;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Commands
{
    /// <summary>
    /// 当前的移动命令
    /// </summary>
    public class MoveCommand : ICommand
    {

        public MoveCommand() {

            this.TipMessage = "开始进行移动命令，请选择需要移动的图形对象";
        }
        public event PenAction.DrawHandler DrawStartEvent;


        public event PenAction.DrawHandler DrawEraseEvent;

        public event PenAction.DrawHandler DrawCompleteEvent;

        public event PenAction.DrawHandler DrawTermimationEvent;

        //用于记录当前的动作
        public event RecordCommand RecordCommand;

        /// <summary>
        /// 当前临时的图形
        /// </summary>
        private Geometry2D GeometryShape = null;

        /// <summary>
        /// 要移动的目标图形
        /// </summary>
        private Geometry2D tagert = null;

        /// <summary>
        /// 过滤器
        /// </summary>
        private Filter.IPickFilter Filter = null;

        /// <summary>
        /// 获取当前图形的目标图形对象
        /// </summary>
        public Geometry2D GetTarget()
        {
            return tagert;

        }
        /// <summary>
        /// 获取当前图形元素
        /// </summary>
        public Geometries.Geometry2D Geometry
        {
            get { return GeometryShape; }
        }
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
        /// 设置当前命令要处理的图形动作
        /// </summary>
        /// <param name="shape"></param>
        public void SetIntersectGeometry(IntersectGeometry shape)
        {
            this.TipMessage = "选择移动的起点";
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
        public void SetPickFilter(Filter.IPickFilter pf) {

            this.Filter = pf;
        }

        /// <summary>
        /// 设置移动的起点
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Albert.Geometry.Primitives.Vector2D p)
        {
            if (GeometryShape == null)
            {
                this.start = p;
                GeometryShape = tagert.Copy();
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
        /// 移动的拖放
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
            //擦除效果
            if (this.DrawEraseEvent != null)
            {
                this.DrawEraseEvent(new ActionEventArgs(GeometryShape));
            }
        }

        /// <summary>
        /// 完成当前的任务操作
        /// </summary>
        public void Complete()
        {
            this.Termimal();
            //移动距离
            Vector2D moveDistance = this.end - this.start;
            //开始移动目标对象
            this.tagert.Move(moveDistance);
            //更新当前的目标对象
            this.tagert.Update();
            //当前命令操作完成
            if (DrawCompleteEvent != null)
            {
                GeometryShape.UnSelect();
                this.DrawCompleteEvent(null);
            }

            if (RecordCommand != null) {

                MoveFunction mf = new MoveFunction(this.tagert);
                mf.Offset = moveDistance;
                RecordCommand(this,mf);
            }
        }

        /// <summary>
        /// 终止当前的移动操作
        /// </summary>
        public void Termimal()
        {
            if (DrawTermimationEvent != null)
            {
                if (GeometryShape != null)
                {
                    GeometryShape.UnSelect();

                }
                DrawTermimationEvent(new ActionEventArgs(GeometryShape));
            }
        }

        public Albert.Geometry.Primitives.Vector2D GetLastPoint()
        {
            return this.end;
        }

        public Albert.Geometry.Primitives.Vector2D GetPreviousPoint()
        {
            return this.start;
        }



        public void SetResult(dynamic result)
        {

        }

        private string TipMessage = "";
        /// <summary>
        /// 获取当前提示信息
        /// </summary>
        /// <returns></returns>
        public string GetCommandStatus()
        {
            return TipMessage;
        }


        /// <summary>
        /// 暂停绘制动作
        /// </summary>
        public void Pause()
        {


        }
    }
}
