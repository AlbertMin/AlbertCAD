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
    /// 偏移一个相同的形状出来
    /// </summary>
    public class RotateCommand : ICommand
    {
        public event PenAction.DrawHandler DrawStartEvent;

        public event PenAction.DrawHandler DrawEraseEvent;

        public event PenAction.DrawHandler DrawCompleteEvent;

        public event PenAction.DrawHandler DrawTermimationEvent;
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
        /// 设置当前的目标图形
        /// </summary>
        /// <param name="shape"></param>
        public void SetIntersectGeometry(IntersectGeometry shape)
        {
            this.tagert = shape.GeometryShape;
        }


        /// <summary>
        /// 设置当前参照点
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

        private double lastAngle = 0;
        /// <summary>
        /// 开始进行转动
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Albert.Geometry.Primitives.Vector2D p)
        {
            this.end = p;

            //已经移动的距离
            Vector2D direction = Line2D.Create(this.start, this.end).Direction;
            //增量的移动距离
            double a = Vector2D.BasisX.AngleFrom(direction);

            double incrementAngle = a - lastAngle;
            //最终移动距离
            lastAngle = a;
            //移动当前图形
            this.GeometryShape.Rolate(start, incrementAngle);
            //擦除效果
            if (this.DrawEraseEvent != null)
            {

                this.DrawEraseEvent(new ActionEventArgs(GeometryShape));
            }
        }

        /// <summary>
        /// 完成当前的绘制工作
        /// </summary>
        public void Complete()
        {
            this.Termimal();
            //已经移动的距离
            Vector2D direction = Line2D.Create(this.start, this.end).Direction;
            //增量的移动距离
            double a = Vector2D.BasisX.AngleFrom(direction);

            this.tagert.Rolate(start, a);
            this.tagert.Update();
            //转动完成事件
            if (DrawCompleteEvent != null)
            {
                this.DrawCompleteEvent(null);
            }

            if (RecordCommand != null) {
                RotateFunctional rf = new RotateFunctional(this.tagert);
                rf.RunOnPoint = start;
                rf.Degree = a;
                RecordCommand(this,rf);
            }
        }

        /// <summary>
        /// 终止当前命令
        /// </summary>
        public void Termimal()
        {
            if (this.DrawTermimationEvent != null)
            {

                this.DrawTermimationEvent(new ActionEventArgs(this.Geometry));
            }
        }

        /// <summary>
        /// 返回最后的点
        /// </summary>
        /// <returns></returns>
        public Albert.Geometry.Primitives.Vector2D GetLastPoint()
        {
            return end;
        }
        /// <summary>
        /// 返回一开始的点
        /// </summary>
        /// <returns></returns>
        public Albert.Geometry.Primitives.Vector2D GetPreviousPoint()
        {
            return start;
        }

        /// <summary>
        /// 指定结果
        /// </summary>
        /// <param name="result"></param>
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

        public void SetPickFilter(IPickFilter f)
        {
            throw new NotImplementedException();
        }
    }
}
