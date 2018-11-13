using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.PenAction;
using Albert.DrawingKernel.Selector;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Events;
using Albert.DrawingKernel.Filter;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Commands
{
    public class ArrayCommand : ICommand
    {

        /// <summary>
        /// 当前临时的图形
        /// </summary>
        private Geometry2D GeometryShape = null;
        /// <summary>
        /// 当前镜像的图形列表
        /// </summary>
        private List<Geometry2D> Geometry2ds = null;
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

        private int number = 0;

        public ArrayCommand(int number)
        {
            this.number = number;
        }

        /// <summary>
        /// 表示整列的数量
        /// </summary>
        public int Number {

            get {

                return number;
            }
            set {

                number = value;
            }
        }
        /// <summary>
        /// 获取要整列的图形元素
        /// </summary>
        public Geometry2D Geometry
        {
            get
            {
                return this.GeometryShape;
            }
        }

        /// <summary>
        /// 当前的一些事件
        /// </summary>
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawTermimationEvent;
        public event RecordCommand RecordCommand;

        public void Complete()
        {

            var mginx = this.end - this.start;

            var totlemove = mginx / number;

            var incrementmove = totlemove - lastmove;

            lastmove = totlemove;
            for (int i = 0; i < number; i++)
            {
                this.Geometry2ds[i].Move(incrementmove * (i + 1));
                this.Geometry2ds[i].Update();
            }
            if (DrawCompleteEvent != null)
            {

                DrawCompleteEvent(new ActionEventArgs(this.GeometryShape));
            }
        }
        private Vector2D lastmove = Vector2D.Zero;
        /// <summary>
        /// 当前的拖拽显示
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Vector2D p)
        {
            if (this.start != null) {
                this.end = p;

                var mginx = this.end - this.start;

                var totlemove = mginx / number;

                var incrementmove = totlemove - lastmove;

                lastmove = totlemove;
                for (int i = 0; i < number; i++)
                {
                    this.Geometry2ds[i].Move(incrementmove * (i + 1));
                    this.Geometry2ds[i].Update();
                }



                if (this.DrawEraseEvent != null)
                {
                    this.DrawEraseEvent(null);
                }
            }
      
        }

        /// <summary>
        /// 获取命令状态
        /// </summary>
        /// <returns></returns>
        public string GetCommandStatus()
        {
            return "";
        }

        public Vector2D GetLastPoint()
        {
            return null;
        }

        public Vector2D GetPreviousPoint()
        {
            return null;
        }

        public Geometry2D GetTarget()
        {
            return tagert;
        }

        public void Pause()
        {
          
        }

        /// <summary>
        /// 设置当前相交图形信息
        /// </summary>
        /// <param name="shape"></param>
        public void SetIntersectGeometry(IntersectGeometry shape)
        {
            this.tagert = shape.GeometryShape;
        }

        /// <summary>
        /// 设置当前起点位置
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            if (GeometryShape == null)
            {
                this.start = p;
                this.end = p;
                this.Geometry2ds = new List<Geometry2D>();
                for (int i = 0; i < number; i++)
                {
                    Geometry2D gs = this.tagert.Copy();
                    this.Geometry2ds.Add(gs);
                }
    

                if (DrawStartEvent != null)
                {
                    this.Geometry2ds.ForEach(x =>
                    {
                        this.GeometryShape = x;
                        this.GeometryShape.IsActioning = true;
                        DrawStartEvent(new ActionEventArgs(x));
                    });
                }
            }
            else
            {
                this.end = p;
                //完成当前的移动
                this.Complete();
            }
        }

        public void SetResult(dynamic result)
        {
          
        }

        public void Termimal()
        {
            if (DrawTermimationEvent != null)
            {
                DrawTermimationEvent(null);
            }
        }

        public void SetPickFilter(IPickFilter f)
        {
            throw new NotImplementedException();
        }
    }
}
