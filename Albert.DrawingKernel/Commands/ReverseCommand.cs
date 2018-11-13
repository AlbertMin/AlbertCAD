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
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Commands
{
    public class ReverseCommand : ICommand
    {

        private string CommandMessage = "";

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
        /// 完成命令
        /// </summary>
        public void Complete()
        {

            //当前命令操作完成
            if (DrawCompleteEvent != null)
            {
                tagert.UnSelect();
                this.DrawCompleteEvent(new ActionEventArgs(tagert));
            }

        }

        /// <summary>
        /// 拖拽
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Vector2D p)
        {

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
            if (this.Filter != null)
            {
                //判断是不是过滤的对象
                if (this.Filter.AllowElement(shape.GeometryShape))
                {
                    this.tagert = shape.GeometryShape;
                    this.tagert.Revserse();
                    this.tagert.Update();

                    this.Complete();
                }
            }
        }

        /// <summary>
        /// 指定一个命令点
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            
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

        public void SetPickFilter(Filter.IPickFilter pf)
        {

            this.Filter = pf;
        }
   
    }
}
