using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.PenAction;
using Albert.DrawingKernel.Selector;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Functional;
using Albert.DrawingKernel.Events;
using Albert.DrawingKernel.Filter;

namespace Albert.DrawingKernel.Commands
{
    public class MirrorCommand : ICommand
    {

        private string CommandMessage = "";


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
        /// 当前的二维图形对象
        /// </summary>
        public Geometry2D Geometry
        {
            get
            {
                return GeometryShape;
            }
        }

        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawTermimationEvent;
        public event RecordCommand RecordCommand;

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
        /// 完成命令
        /// </summary>
        public void Complete()
        {


            //已经移动的距离
            Line2D mirrorLine = Line2D.Create(this.start, this.end);

            //移动当前图形
            this.GeometryShape.Mirror(this.tagert,mirrorLine);


            //更新当前的目标对象
            this.GeometryShape.Update();
            //当前命令操作完成
            if (DrawCompleteEvent != null)
            {
                GeometryShape.UnSelect();
                this.DrawCompleteEvent(new ActionEventArgs(GeometryShape));
            }

            if (RecordCommand != null)
            {
                CopyFunctional mf = new CopyFunctional(this.tagert);
                RecordCommand(this, mf);
            }
        }

        /// <summary>
        /// 拖拽
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Vector2D p)
        {
            this.end = p;

            //已经移动的距离
            Line2D mirrorLine = Line2D.Create(this.start, this.end);

            //移动当前图形
            this.GeometryShape.Mirror(this.tagert,mirrorLine);

            GeometryShape.Update();
            //擦除效果
            if (this.DrawEraseEvent != null)
            {
                this.DrawEraseEvent(new ActionEventArgs(GeometryShape));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCommandStatus()
        {
            return CommandMessage;
        }

        /// <summary>
        /// 获取最后的一个点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLastPoint()
        {
            return null;
        }

        /// <summary>
        /// 获取最后的一个点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {
            return null;
        }


        /// <summary>
        /// 暂停当前的命令
        /// </summary>
        public void Pause()
        {
       
        }

        /// <summary>
        /// 设置当前相交信息
        /// </summary>
        /// <param name="shape"></param>
        public void SetIntersectGeometry(IntersectGeometry shape)
        {
            this.CommandMessage = "选择移动的起点";
            this.tagert = shape.GeometryShape;
        }

        /// <summary>
        /// 指定一个命令点
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            if (GeometryShape == null)
            {
                this.start = p;
                this.end = p;
                GeometryShape = tagert.Copy();
                if (this.DrawStartEvent != null)
                {

                    if (DrawStartEvent != null)
                    {
                        this.DrawStartEvent(new ActionEventArgs(GeometryShape));
                        this.GeometryShape.IsActioning = true;
                    }
                }
            }
            else
            {
                this.end = p;
                //完成当前的移动
                this.Complete();
            }
        }

        /// <summary>
        /// 直接指定当前命令的结果
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
  
        }

        /// <summary>
        /// 终止当前的命令
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

        public void SetPickFilter(IPickFilter f)
        {
            throw new NotImplementedException();
        }
    }
}
