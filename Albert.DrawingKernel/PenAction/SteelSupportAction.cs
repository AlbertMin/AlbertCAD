using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Geometries;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Events;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.PenAction
{
    /// <summary>
    /// 钢梁支撑的绘制
    /// </summary>
    public class SteelSupportAction : IAction
    {
        private bool isPausing=false;

        /// <summary>
        /// 钢梁支撑图形元素
        /// </summary>
        private SteelSupportGeometry steelSupportGeometry = new SteelSupportGeometry();

        /// <summary>
        /// 返回钢梁支撑图形元素
        /// </summary>
        public Geometry2D Geometry
        {
            get
            {
                return steelSupportGeometry;
            }
        }

        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawTermimationEvent;

        public void Complete()
        {
            if (steelSupportGeometry.Central != null && steelSupportGeometry.Face != null)
            {

                if (DrawCompleteEvent != null)
                {
                    Geometry.IsActioning = false;
                    DrawCompleteEvent(new ActionEventArgs(this.steelSupportGeometry));

                }

            }
            else
            {

                if (DrawTermimationEvent != null)
                {
 
                    DrawTermimationEvent(new ActionEventArgs(this.steelSupportGeometry));

                }
            }
        }

        /// <summary>
        /// 当前的拖拽
        /// </summary>
        /// <param name="p"></param>
        public void Erase(Vector2D p)
        {

            if (!isPausing)
            {
               
                steelSupportGeometry.Face = Line2D.Create(this.steelSupportGeometry.Central, p).Direction;
                if (DrawEraseEvent != null)
                {

                    DrawEraseEvent(new ActionEventArgs(this.Geometry));

                }
                this.steelSupportGeometry.Update();
            }
        }

        public string GetCommandStatus()
        {
            return "";
        }

        public Vector2D GetLastPoint()
        {
            return this.steelSupportGeometry.Central;
        }

        public Vector2D GetPreviousPoint()
        {
            return this.steelSupportGeometry.Central;
        }

        /// <summary>
        /// 终止当前命令
        /// </summary>
        public void Pause()
        {
            isPausing = true;
        }

        /// <summary>
        /// 设置第一个点
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            if (steelSupportGeometry.Central == null)
            {

                steelSupportGeometry.Central = p;
                steelSupportGeometry.Face = Vector2D.Zero;
                if (DrawStartEvent != null)
                {
                    Geometry.IsActioning = true;
                    DrawStartEvent(new ActionEventArgs(this.Geometry));

                }
            }
            else
            {

                steelSupportGeometry.Face = Line2D.Create(this.steelSupportGeometry.Central, p).Direction;
                this.Complete();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
           
        }

        /// <summary>
        /// 终止当前命令
        /// </summary>
        public void Termimal()
        {
            if (DrawTermimationEvent != null)
            {
                DrawTermimationEvent(new ActionEventArgs(this.Geometry));
            }
        }
    }
}
