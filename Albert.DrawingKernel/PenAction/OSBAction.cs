using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Albert.Geometry.External;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Util;
using Albert.Geometry.Transform;
using Albert.DrawingKernel.Events;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.PenAction
{
    public class OSBAction : IAction
    {


        public OSBAction()
        {
     
        }

        private bool isruning = false;
        private OSBGeometry osb = null;

        public Geometry2D Geometry
        {
            get
            {
                return osb;
            }
        }

        public event DrawHandler DrawCompleteEvent;
        public event DrawHandler DrawEraseEvent;
        public event DrawHandler DrawStartEvent;
        public event DrawHandler DrawTermimationEvent;

        public void Complete()
        {
            if (osb.LocationPoint != null)
            {
                if (DrawCompleteEvent != null)
                {
                   // AutoTrim(osb, Floor);
                    DrawCompleteEvent.Invoke(new ActionEventArgs(osb));
                }

            }
            else
            {
                if (DrawTermimationEvent != null)
                {
                  //  AutoTrim(osb, Floor);
                    DrawTermimationEvent.Invoke(new ActionEventArgs(osb));
                }
            }

        }
        private void AutoTrim(OSBGeometry osb)
        {
           
        }
        public void Erase(Vector2D p)
        {
            if (!isPausing)
            {

            }
            if (!isruning)
            {
                if (DrawStartEvent != null)
                    DrawStartEvent(new ActionEventArgs(Geometry));
                isruning = true;

            }
            osb.LocationPoint = p;
            if (DrawEraseEvent != null)
                DrawEraseEvent(new ActionEventArgs(Geometry));
            osb.Update();
        }

        public Vector2D GetLastPoint()
        {
            return osb.LocationPoint;
        }

        public void SetPoint(Vector2D p)
        {
            if (osb.LocationPoint != null)
            {
                osb.Update();
                Complete();
            }
        }

        public void SetResult(dynamic result)
        {

        }

        public void Termimal()
        {
            if (DrawTermimationEvent != null)
                DrawTermimationEvent(new ActionEventArgs(Geometry));
        }



        public void Space_OnClick()
        {
            osb.Status++;
            osb.Update();
        }

        public void Ctrl_OnClick()
        {
            osb.MirrorF = -osb.MirrorF;
            osb.Update();
        }

        public Vector2D GetPreviousPoint()
        {
            return osb.LocationPoint;
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
