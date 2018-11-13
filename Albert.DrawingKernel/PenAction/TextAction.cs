using Albert.DrawingKernel.PenAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Primitives;
using System.Windows.Input;
using Albert.DrawingKernel.Geometries.Temporary;
using Albert.DrawingKernel.Events;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.PenAction
{
    public class TextAction : IAction
    {

        private TextGeometry text = new TextGeometry();
        public Geometry2D Geometry
        {
            get
            {
                return text;
            }
        }
        private bool starting = false;

        /// <summary>
        /// 是否已经开始
        /// </summary>
        public bool Starting
        {
            get
            {
                return starting;
            }
        }

        /// <summary>
        /// 当前的输入文本信息
        /// </summary>
        public string Text
        {

            get {
                return text.Text;
            }
            set {
                text.Text = value;
            }
        }
        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawTermimationEvent;
        public event DrawHandler DrawStartEvent;

        public void Erase(Vector2D p)
        {
            if (!isPausing) {
                text.Central = p;
            }
            this.text.Update();
       
        }

        public Vector2D GetLastPoint()
        {
            return text.Central;
        }

        /// <summary>
        /// 获取上一步的点
        /// </summary>
        /// <returns></returns>
        public Vector2D GetPreviousPoint()
        {

            return text.Central;
        }
        /// <summary>
        /// 设置绘制点
        /// </summary>
        /// <param name="p"></param>
        public void SetPoint(Vector2D p)
        {
            text.Central = p;
            starting = true;
            this.text.Update();
            if (DrawStartEvent != null) {
                DrawStartEvent(new ActionEventArgs(this.Geometry));
            }

            if (DrawEraseEvent != null)
            {
                DrawEraseEvent(new ActionEventArgs(this.Geometry));
            }
        }

        /// <summary>
        /// 设置当前结果
        /// </summary>
        /// <param name="result"></param>
        public void SetResult(dynamic result)
        {
            var RT = result;
            text.Text = result;
            this.text.Update();
            this.Complete();
        }

        public void Complete()
        {
            if (this.text.Central != null && this.text.Text!= "")
            {
                if (this.DrawCompleteEvent != null)
                {
                    this.DrawCompleteEvent(new ActionEventArgs(Geometry));
                }
            }
            else
            {
                if (this.DrawTermimationEvent != null)
                {
                    this.DrawTermimationEvent(new ActionEventArgs(Geometry));
                }
            }
        }

        public void Termimal()
        {
            if (this.DrawTermimationEvent != null)
            {
                this.DrawTermimationEvent(new ActionEventArgs(this.Geometry));
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
