using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.PenAction;
using Albert.DrawingKernel.Geometries.Primitives;

namespace Albert.DrawingKernel.Util
{
    /// <summary>
    /// 命令重启类，用于重启当前的命令
    /// </summary>
    internal class CommandRestart
    {
        private Explorer explorer;
        private IAction lastAction;

        /// <summary>
        /// 构造函数，重启当前的命令
        /// </summary>
        /// <param name="explorer"></param>
        /// <param name="lastAction"></param>
        public CommandRestart(Explorer explorer, IAction lastAction)
        {
            this.explorer = explorer;
            this.lastAction = lastAction;
        }

        /// <summary>
        /// 开始指定的命令
        /// </summary>
        internal void Start()
        {
            if (this.lastAction != null && this.lastAction.Geometry != null)
            {
                dynamic Element = null;
                if (lastAction.Geometry.Element != null) {

                    Element = CopyElement(lastAction.Geometry.Element);
                }
                if (this.lastAction is ArcAction)
                {
                    this.explorer.ArcAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is BeamAction)
                {
                    this.explorer.BeamAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, (lastAction.Geometry as BeamGeometry).Thickness, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is CircleAction)
                {
                    this.explorer.CircleAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is ColumnAction)
                {
                    this.explorer.ColumnAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is EllipseAction)
                {
                    this.explorer.EllipseAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is FloorAction)
                {
                    this.explorer.FloorAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, (lastAction.Geometry as FloorGeometry).Thickness, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is LegwireAction)
                {
                    this.explorer.LegwireAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is LineAction)
                {
                    this.explorer.LineAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is MemberAction)
                {
                    this.explorer.MemberAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is OSBAction)
                {
                    this.explorer.OSBAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is PolygonAction)
                {
                    this.explorer.PolygonAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is PolylineAction)
                {
                    this.explorer.PolylineAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is RectangleAction)
                {
                    this.explorer.RectangleAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is StaffAction)
                {
                    this.explorer.StaffAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is SteelBeamAction)
                {
                    this.explorer.SteelBeamAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, (lastAction.Geometry as SteelBeamGeometry).Thickness, lastAction.Geometry.FillColor, Element);
                }
                else if (this.lastAction is SteelColumnAction)
                {
                    this.explorer.SteelColumnAction(lastAction.Geometry.PenColor, lastAction.Geometry.LineWidth, lastAction.Geometry.FillColor, Element);
                }
                else
                {

                }

            }
        }

        /// <summary>
        /// 用于拷贝一个新的元素对象
        /// </summary>
        /// <param name="dynamic"></param>
        /// <returns></returns>
        private dynamic CopyElement(dynamic dynamic)
        {
            
            return null;
        }
    }
}
