using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Albert.DrawingKernel.Events;

namespace Albert.DrawingKernel.PenAction
{
    /// <summary>
    /// 定义一个椭圆的动作
    /// </summary>
    public class EllipseAction : IAction
    {

        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawTermimationEvent;
        public EllipseAction()
        {

        }
        private EllipseGeometry ellipse = new EllipseGeometry();
        public Geometry2D Geometry
        {
            get
            {
                return ellipse;
            }
        }


        public Vector2D GetLastPoint()
        {
            return ellipse.Central;
        }
        public Vector2D GetPreviousPoint()
        {

            return ellipse.Central;
        }

        private bool isConfirmOne = false;
        public bool ConfirmOne
        {
            get { return isConfirmOne; }
            private set
            {
                isConfirmOne = value;
            }
        }

        private double width = 0;
        public double Width
        {

            get
            {

                width = ellipse.Reference.X - ellipse.Central.X;
                return width;
            }
            private set
            {
                width = value;
                ellipse.Reference = new Vector2D(ellipse.Central.X + width, ellipse.Central.Y);
                isConfirmOne = true;
            }
        }

        private double height = 0;
        public double Height
        {

            get
            {

                height = ellipse.Reference.Y - ellipse.Central.Y;
                return height;
            }
            private set
            {

                height = value;
                ellipse.Reference = new Vector2D(ellipse.Central.X + width, ellipse.Central.Y + height);
            }
        }

        //开始执行命令
        public void Erase(Vector2D p)
        {

            if (!isPausing)
            {
                if (isConfirmOne)
                {
                    ellipse.Reference = new Vector2D(ellipse.Central.X + width, p.Y);
                }
                else
                {
                    this.ellipse.Reference = p;
                }
                if (this.DrawEraseEvent != null)
                {
                    DrawEraseEvent(new ActionEventArgs(this.Geometry));
                }
                this.ellipse.Update();
            }
        }


        public void SetPoint(Vector2D p)
        {
            if (this.ellipse.Central != null)
            {
                this.ellipse.Reference = p;
                this.Complete();

            }
            else
            {

                this.ellipse.Central = p;
                if (this.DrawStartEvent != null)
                {
                    Geometry.IsActioning = true;
                    DrawStartEvent(new ActionEventArgs(this.Geometry));
                }
            }
        }

        public void Complete()
        {
            if (this.ellipse.Central != null)
            {

                if (DrawCompleteEvent != null)
                {
                    Geometry.IsActioning = false;
                    DrawCompleteEvent(new ActionEventArgs(this.Geometry));

                }

            }
            else
            {
                if (DrawTermimationEvent != null)
                {
                    DrawTermimationEvent(new ActionEventArgs(this.Geometry));
                }
            }
        }

        public void SetResult(dynamic result)
        {
            if (ConfirmOne)
            {

                Height = result;
                this.ellipse.Update();
                this.Complete();
            }
            else
            {

                Width = result;
                this.ellipse.Update();
                this.isPausing = false;
            }
        }

        public void Termimal()
        {
            if (DrawTermimationEvent != null)
            {
                DrawTermimationEvent(new ActionEventArgs(this.Geometry));
            }
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
