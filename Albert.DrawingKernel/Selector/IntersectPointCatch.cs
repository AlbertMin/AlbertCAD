using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Albert.DrawingKernel.Util;
using Albert.DrawingKernel.Geometries;
using Albert.Geometry.Primitives;
using Albert.Geometry.External;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.PenAction;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Selector
{
    /// <summary>
    /// 用于捕捉获取
    /// </summary>
    public class IntersectPointCatch
    {


        private IAction action = null;
        /// <summary>
        /// 当前的绘制对象
        /// </summary>
        private DrawingControl DrawingControl
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数，初始化当前兴趣点捕获
        /// </summary>
        public IntersectPointCatch(DrawingControl dc)
        {

            DrawingControl = dc;

        }

        /// <summary>
        /// 用于捕获相关的点
        /// </summary>
        /// <returns></returns>
        public IntersectPoint Catch(Vector2D p, IAction action = null)
        {
            this.action = action;

            var InterestPoint = GetInterestPoint(p);
            if (InterestPoint != null)
            {
                return InterestPoint;
            }
            return null;
        }

        /// <summary>
        /// 获取shift的按下的信息
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public IntersectPoint GetShiftPoint(Vector2D p1, Vector2D p2, IAction action = null)
        {
            IntersectPoint intersectPoint = null;
            this.action = action;

            var dir = p2 - p1;
            var a = Math.Abs(dir.AngleWith(new Vector2D(1, 0))) * 180 / Math.PI;

            if (a > 90)
            {
                a = 180 - a;
            }
            if (a < 45)
            {
                var vector = new Vector2D(p2.X, p1.Y);
                intersectPoint = Catch(vector, this.action);

                if (intersectPoint == null)
                {
                    intersectPoint = new IntersectPoint();
                    intersectPoint.Point = new Vector2D(p2.X, p1.Y);
                    intersectPoint.IntersectPointStyle = 1;
                }
                return intersectPoint;
            }
            else
            {
                var vector = new Vector2D(p1.X, p2.Y);
                intersectPoint = Catch(vector, this.action);
                if (intersectPoint == null)
                {
                    intersectPoint = new IntersectPoint();
                    intersectPoint.Point = new Vector2D(p1.X, p2.Y);
                    intersectPoint.IntersectPointStyle = 1;
                }
                return intersectPoint;
            }
        }


        /// <summary>
        /// 当前界面上所有的兴趣点
        /// </summary>
        private List<Vector2D> canCatchPoints = null;

        /// <summary>
        /// 当前能捕获的图形元素
        /// </summary>
        private List<Geometry2D> canCatchVisuals = null;


        /// <summary>
        /// 获取焦点信息
        /// </summary>
        internal IntersectPoint GetInterestPoint(Vector2D v)
        {
            IntersectPoint ip = null;

            if (this.DrawingControl.IsUpdated)
            {
                canCatchPoints = new List<Vector2D>();
                //获取界面上所有可视化图形
                canCatchVisuals = this.DrawingControl.CatchVisuals();


                if (KernelProperty.CanCatchEndPoint)
                {
                    //获取所有的端点
                    List<Vector2D> endpoints = this.GetEndPoint(canCatchVisuals);
                    //添加所有的端点
                    canCatchPoints.AddRange(endpoints);
                }


                //当前所有的兴趣线
                List<Line2D> InerestLines = this.GetInterestLine(canCatchVisuals);
                if (KernelProperty.CanCatchIntersect)
                {
                    //获取当前的兴趣点
                    List<Vector2D> InerestPoints = this.InterestPoint(InerestLines);

                    canCatchPoints.AddRange(InerestPoints);

                }
                if (KernelProperty.CanCatchCentral)
                {
                    //获取当前中心点
                    List<Vector2D> MiddlePoints = this.MiddlePoint(InerestLines);

                    canCatchPoints.AddRange(MiddlePoints);
                }

                //剔除当前正在绘制的点和绘制的上一个点，不会用于捕捉
                if (this.action.GetLastPoint() != null)
                {
                    canCatchPoints.Remove(this.action.GetLastPoint());
                }
                if (this.action.GetPreviousPoint() != null)
                {
                    canCatchPoints.Remove(this.action.GetPreviousPoint());
                }

                this.DrawingControl.IsUpdated = false;

            }
            //查找最近的点
            if (canCatchPoints != null && canCatchPoints.Count > 0)
            {
                //查找最近的端点和相交点
                Vector2D point = canCatchPoints.OrderBy(x => v.Distance(x)).First();
                //获取了端点
                if (v.Distance(point) < KernelProperty.Tolerance)
                {
                    ip = new IntersectPoint();
                    ip.IntersectPointStyle = 0;
                    ip.Point = Vector2D.Create(point.X, point.Y);
                }
            }
            if (ip == null)
            {
                ip = CatchVisualPoint(canCatchVisuals, v, ip);
            }
            //辅助线捕获，应该在交点有何没有的情况下，都应该获取
            if (ip == null)
            {
                ip = this.RefencePoint(canCatchPoints, v);
            }
            return ip;
        }

        /// <summary>
        /// 图形对象之上的对象捕获
        /// </summary>
        /// <param name="canCatchVisuals"></param>
        /// <param name="v"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        private IntersectPoint CatchVisualPoint(List<Geometry2D> canCatchVisuals, Vector2D v, IntersectPoint ip)
        {
            IntersectPoint tpip = null;
            if (canCatchVisuals != null)
            {
                //查找是否有相交点，优先捕获点
                for (int i = 0; i < canCatchVisuals.Count; i++)
                {
                  
                    if (canCatchVisuals[i] == action.Geometry)
                    {
                        tpip = canCatchVisuals[i].IntersectPoint2(v);
                    }
                    else
                    {
                        tpip = canCatchVisuals[i].IntersectPoint(v);
                    }
                    if (tpip != null)
                    {
                        if (ip == null)
                        {
                            return tpip;
                        }
                        else
                        {
                            if (v.Distance(tpip.Point) < v.Distance(ip.Point))
                            {
                                return tpip;
                            }
                            else
                            {

                                return ip;
                            }

                        }

                    }

                }
            }

            return tpip;
        }

        /// <summary>
        /// 获取引用的点
        /// </summary>
        /// <param name="canCatchPoints"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private IntersectPoint RefencePoint(List<Vector2D> canCatchPoints, Vector2D v)
        {
            IntersectPoint ip = null;
            if (canCatchPoints != null && canCatchPoints.Count > 0)
            {
                Vector2D point1 = canCatchPoints.OrderBy(x => Math.Abs(v.Y - x.Y)).First();

                if (Math.Abs(v.Y - point1.Y) < KernelProperty.Tolerance)
                {
                    if (ip == null)
                    {
                        ip = new IntersectPoint();
                        ip.IntersectPointStyle = 2;
                        ip.Point = Vector2D.Create(v.X, point1.Y);
                        ip.Refences = new List<Line2D>();
                    }
                    else
                    {
                        ip.Point = Vector2D.Create(ip.Point.X, point1.Y);
                    }
                    ip.Refences.Add(Line2D.Create(point1, ip.Point));
                }
                Vector2D point2 = canCatchPoints.OrderBy(x => Math.Abs(v.X - x.X)).First();

                if (Math.Abs(v.X - point2.X) < KernelProperty.Tolerance)
                {
                    if (ip == null)
                    {
                        ip = new IntersectPoint();
                        ip.IntersectPointStyle = 2;
                        ip.Point = Vector2D.Create(point2.X, v.Y);
                        ip.Refences = new List<Line2D>();
                    }
                    else
                    {
                        ip.Point = Vector2D.Create(point2.X, ip.Point.Y);
                    }

                    ip.Refences.Add(Line2D.Create(point2, ip.Point));
                }


            }
            return ip;
        }


        /// <summary>
        /// 获取当前所有的线的中心点
        /// </summary>
        /// <param name="inerestLines"></param>
        /// <returns></returns>
        private List<Vector2D> MiddlePoint(List<Line2D> inerestLines)
        {
            List<Vector2D> ols = new List<Vector2D>();
            if (inerestLines.Count > 0)
            {
                for (int i = 0; i < inerestLines.Count; i++)
                {
                    ols.Add(inerestLines[i].MiddlePoint);
                }
            }
            return ols;
        }

        /// <summary>
        /// 获取当前所有线上的交点
        /// </summary>
        /// <param name="inerestLines"></param>
        /// <returns></returns>
        private List<Vector2D> InterestPoint(List<Line2D> inerestLines)
        {
            List<Vector2D> ols = new List<Vector2D>();
            if (inerestLines.Count > 1)
            {
                for (int i = 0; i < inerestLines.Count; i++)
                {

                    for (int j = i; j < inerestLines.Count; j++)
                    {
                        Vector2D inv = inerestLines[i].Intersect(inerestLines[j]);

                        if (inv != null && !inv.IsEndPoint(inerestLines[i]) && !inv.IsEndPoint(inerestLines[j]))
                        {
                            ols.Add(inv);
                        }
                    }
                }
            }

            return ols;
        }

        /// <summary>
        /// 获取所有的直线信息
        /// </summary>
        /// <param name="gss"></param>
        /// <returns></returns>
        private List<Line2D> GetInterestLine(List<Geometry2D> gss)
        {
            List<Line2D> lines = new List<Line2D>();
            for (int i = 0; i < gss.Count; i++)
            {
                if (gss[i].Lines != null && gss[i].Lines.Count > 0&&gss[i].IsEnabled)
                {
                    lines.AddRange(gss[i].Lines);
                }
            }
            return lines;
        }

        /// <summary>
        /// 获取所有的端点
        /// </summary>
        /// <param name="gss"></param>
        /// <returns></returns>
        private List<Vector2D> GetEndPoint(List<Geometry2D> gss)
        {
            List<Vector2D> EndPoints = new List<Vector2D>();
            //获取所有的顶点
            for (int i = 0; i < gss.Count; i++)
            {
                if (gss[i].Points != null && gss[i].IsEnabled)
                {
                    EndPoints.AddRange(gss[i].Points);
                }
            }

            return EndPoints;
        }


        /// <summary>
        /// 用于比较两个点是否相同
        /// </summary>
        public class PointComparable : IEqualityComparer<Point>
        {

            public bool Equals(Point x, Point y)
            {
                if (Math.Sqrt(Math.Pow(x.X - y.X, 2) + Math.Pow(x.Y - y.Y, 2)) <= 5.0 * KernelProperty.PixelToSize)
                {
                    return true;
                }
                return false;
            }

            public int GetHashCode(Point obj)
            {
                return 1;
            }
        }
    }
}
