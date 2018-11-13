using Albert.DrawingKernel.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Albert.DrawingKernel.Controls
{
    /// <summary>
    /// 可视化对象，实现了向界面上添加可视化对象的功能，并且能够获取可视化对象的数量
    /// </summary>
    public class ContentControl : Panel
    {

        public ContentControl()
        {

            this.Focusable = true;
        }
        /// <summary>
        /// 当前所有的可见对象
        /// </summary>
        private List<Geometry2D> visuals = new List<Geometry2D>();



        //获取Visual的个数
        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        /// <summary>
        /// 获取界面上所有的可视化对象
        /// </summary>
        public List<Geometry2D> Visuals
        {
            get
            {
                return visuals;
            }
        }

        /// <summary>
        /// 获取Visual
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= this.visuals.Count)
            {

                return null;
            }
            return visuals[index];
        }

        /// <summary>
        /// 添加Visual
        /// </summary>
        /// <param name="visual"></param>
        protected void AddVisual(Geometry2D visual)
        {
            visuals.Add(visual);
            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        /// <summary>
        /// 删除Visual
        /// </summary>
        /// <param name="visual"></param>
        protected void RemoveVisual(Geometry2D visual)
        {
            visuals.Remove(visual);
            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        /// <summary>
        /// 命中测试
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as DrawingVisual;
        }

    }
}
