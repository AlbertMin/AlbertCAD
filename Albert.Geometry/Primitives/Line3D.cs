using System;
using System.Collections.Generic;
using Albert.Geometry.External;

namespace Albert.Geometry.Primitives
{

    /// <summary>
    /// 初始化当前一个直线
    /// </summary>
    public class Line3D
    {

        /// <summary>
        /// 构造函数创建
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Line3D(Vector3D start, Vector3D end)
        {
            Start = start;
            End = end;
    
        }

        private Vector3D start;

        public Vector3D Start
        {
            set { start = value; }
            get { return start; }
        }

        private Vector3D end;

        public Vector3D End
        {
            set { end = value; }
            get { return end; }
        }

        private Vector3D origin;
        /// <summary>
        /// 返回当前的原点坐标
        /// </summary>

        public Vector3D Origin
        {
            get
            {
                if (origin == null) {

                    origin = start;
                }
                return origin;
            }
            set { origin = value; }
        }

        private Line3D()
        {

        }
        /// <summary>
        /// 通过两个点创建一条直线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Line3D Create(Vector3D start, Vector3D end)
        {
            return new Line3D(start, end);
        }

        /// <summary>
        /// 通过起点，方向和长度确定一个直线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="direction"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static Line3D Create(Vector3D start, Vector3D direction, double step)
        {
            Vector3D nV = start + step * direction.Normalize();
            return new Line3D(start, nV);
        }

        /// <summary>
        /// 创建一个无端点的直线
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Line3D CreateUnbond(Vector3D origin, Vector3D direction)
        {
            Line3D line = new Line3D();
            line.direction = direction.Normalize();
            line.Origin = origin;
            line.Start = null;
            line.End = null;
            return line;
        }

        /// <summary>
        /// 判断线是不是直线
        /// </summary>

        public bool IsLineUnbond
        {
            get
            {
                if (this.Start == null || this.End == null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 当前直线的方向信息
        /// </summary>
        private Vector3D direction;
        /// <summary>
        /// 直线单位方向
        /// </summary>

        public Vector3D Direction
        {
            get
            {
                if (End != null && Start != null)
                    direction = (End - Start).Normalize();
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        /// <summary>
        /// 获取当前线的长度
        /// </summary>
        public double Length
        {
            get
            {
                if (Start != null && End != null)
                    return Math.Sqrt(Math.Pow((End.X - Start.X), 2) + Math.Pow((End.Y - Start.Y), 2) + Math.Pow((End.Z - Start.Z), 2));
                throw new ArgumentNullException("该直线无端点");
            }
        }

        /// <summary>
        /// 获取之间指定比例的坐标
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Vector3D Evaluate(double parameter)
        {
            return (End - Start) * parameter + Start;
        }

        /// <summary>
        /// 线段和线段相交
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public Vector3D Intersect(Line3D curve)
        {
            double x0 = Start.X, y0 = Start.Y, z0 = Start.Z,
                        x1 = curve.Start.X, y1 = curve.Start.Y, z1 = curve.Start.Z,
                        l0 = Direction.X, m0 = Direction.Y, n0 = Direction.Z,
                        l1 = curve.Direction.X, m1 = curve.Direction.Y, n1 = curve.Direction.Z,
                        deltaLM = l0 * m1 - m0 * l1, deltaMN = m0 * n1 - n0 * m1, deltaLN = l0 * n1 - n0 * l1,
                        t0, t1;

            if (deltaLM.AreEqual(0) && deltaLN.AreEqual(0) && deltaMN.AreEqual(0))
            {
                if (Start.IsOnTwoLine(this, curve) && !this.CoincidesWith(curve))
                {
                    return Start;
                }
                if (End.IsOnTwoLine(this, curve) && !this.CoincidesWith(curve))
                {
                    return End;
                }
                else
                {
                    return null;
                }
            }
            if (!deltaLM.AreEqual(0))
            {
                t0 = (m1 * x1 - l1 * y1 - m1 * x0 + l1 * y0) / deltaLM;
                if (!l1.AreEqual(0))
                    t1 = (x0 + l0 * t0 - x1) / l1;
                else
                    t1 = (y0 + m0 * t0 - y1) / m1;
                double zTry1 = z0 + n0 * t0, zTry2 = z1 + n1 * t1;
                Vector3D intersection = new Vector3D(x0 + l0 * t0, y0 + m0 * t0, z0 + n0 * t0);
                if (zTry1.AreEqual(zTry2) && intersection.IsOnTwoLine(this, curve))
                    return intersection;
                else
                    return null;
            }
            else if (!deltaLN.AreEqual(0))
            {
                t0 = (n1 * x1 - l1 * z1 - n1 * x0 + l1 * z0) / deltaLN;
                if (!l1.AreEqual(0))
                    t1 = (x0 + l0 * t0 - x1) / l1;
                else
                    t1 = (z0 + n0 * t0 - z1) / n1;
                double yTry1 = y0 + m0 * t0, yTry2 = y1 + m1 * t1;
                Vector3D intersection = new Vector3D(x0 + l0 * t0, y0 + m0 * t0, z0 + n0 * t0);
                if (yTry1.AreEqual(yTry2) && intersection.IsOnTwoLine(this, curve))
                    return intersection;
                else
                    return null;
            }

            else
            {
                t0 = (n1 * y1 - m1 * z1 - n1 * y0 + m1 * z0) / deltaMN;
                if (!m1.AreEqual(0))
                    t1 = (y0 + m0 * t0 - y1) / m1;
                else
                    t1 = (z0 + n0 * t0 - z1) / n1;
                double xTry1 = x0 + l0 * t0, xTry2 = x1 + l1 * t1;
                Vector3D intersection = new Vector3D(x0 + l0 * t0, y0 + m0 * t0, z0 + n0 * t0);
                if (xTry1.AreEqual(xTry2) && intersection.IsOnTwoLine(this, curve))
                    return intersection;
                else
                    return null;
            }
        }

        /// <summary>
        /// 创建一个反方向的线
        /// </summary>
        /// <returns></returns>
        public Line3D Reversed()
        {
            return Line3D.Create(this.end, this.start);
        }

        /// <summary>
        /// 直线和直线相交
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public Vector3D IntersectStraightLine(Line3D curve)
        {
            double x0 = Origin.X, y0 = Origin.Y, z0 = Origin.Z,
                        x1 = curve.Origin.X, y1 = curve.Origin.Y, z1 = curve.Origin.Z,
                        l0 = Direction.X, m0 = Direction.Y, n0 = Direction.Z,
                        l1 = curve.Direction.X, m1 = curve.Direction.Y, n1 = curve.Direction.Z,
                        deltaLM = l0 * m1 - m0 * l1, deltaMN = m0 * n1 - n0 * m1, deltaLN = l0 * n1 - n0 * l1,
                        t0, t1;

            if (deltaLM.AreEqual(0) && deltaLN.AreEqual(0) && deltaMN.AreEqual(0))
            {
                return null;
            }
            if (!deltaLM.AreEqual(0))
            {
                t0 = (m1 * x1 - l1 * y1 - m1 * x0 + l1 * y0) / deltaLM;
                if (!l1.AreEqual(0))
                    t1 = (x0 + l0 * t0 - x1) / l1;
                else
                    t1 = (y0 + m0 * t0 - y1) / m1;
                double zTry1 = z0 + n0 * t0, zTry2 = z1 + n1 * t1;
                Vector3D intersection = new Vector3D(x0 + l0 * t0, y0 + m0 * t0, z0 + n0 * t0);
                return intersection;
            }
            else if (!deltaLN.AreEqual(0))
            {
                t0 = (n1 * x1 - l1 * z1 - n1 * x0 + l1 * z0) / deltaLN;
                if (!l1.AreEqual(0))
                    t1 = (x0 + l0 * t0 - x1) / l1;
                else
                    t1 = (z0 + n0 * t0 - z1) / n1;
                double yTry1 = y0 + m0 * t0, yTry2 = y1 + m1 * t1;
                Vector3D intersection = new Vector3D(x0 + l0 * t0, y0 + m0 * t0, z0 + n0 * t0);
                return intersection;
            }

            else
            {
                t0 = (n1 * y1 - m1 * z1 - n1 * y0 + m1 * z0) / deltaMN;
                if (!m1.AreEqual(0))
                    t1 = (y0 + m0 * t0 - y1) / m1;
                else
                    t1 = (z0 + n0 * t0 - z1) / n1;
                double xTry1 = x0 + l0 * t0, xTry2 = x1 + l1 * t1;
                Vector3D intersection = new Vector3D(x0 + l0 * t0, y0 + m0 * t0, z0 + n0 * t0);
                return intersection;
            }
        }

        /// <summary>
        /// 直线和线段相交
        /// </summary>
        /// <param name="limitedCurve">线段</param>
        /// <returns></returns>
        public Vector3D IntersectStraightLine2(Line3D limitedCurve)
        {
            double x0 = Origin.X, y0 = Origin.Y, z0 = Origin.Z,
                        x1 = limitedCurve.Start.X, y1 = limitedCurve.Start.Y, z1 = limitedCurve.Start.Z,
                        l0 = Direction.X, m0 = Direction.Y, n0 = Direction.Z,
                        l1 = limitedCurve.Direction.X, m1 = limitedCurve.Direction.Y, n1 = limitedCurve.Direction.Z,
                        deltaLM = l0 * m1 - m0 * l1, deltaMN = m0 * n1 - n0 * m1, deltaLN = l0 * n1 - n0 * l1,
                        t0, t1;

            if (deltaLM.AreEqual(0) && deltaLN.AreEqual(0) && deltaMN.AreEqual(0))
            {
                return null;
            }
            if (!deltaLM.AreEqual(0))
            {
                t0 = (m1 * x1 - l1 * y1 - m1 * x0 + l1 * y0) / deltaLM;
                if (!l1.AreEqual(0))
                    t1 = (x0 + l0 * t0 - x1) / l1;
                else
                    t1 = (y0 + m0 * t0 - y1) / m1;
                double zTry1 = z0 + n0 * t0, zTry2 = z1 + n1 * t1;
                Vector3D intersection = new Vector3D(x0 + l0 * t0, y0 + m0 * t0, z0 + n0 * t0);
                if (zTry1.AreEqual(zTry2) && intersection.IsOnLine(limitedCurve))
                    return intersection;
                else
                    return null;
            }
            else if (!deltaLN.AreEqual(0))
            {
                t0 = (n1 * x1 - l1 * z1 - n1 * x0 + l1 * z0) / deltaLN;
                if (!l1.AreEqual(0))
                    t1 = (x0 + l0 * t0 - x1) / l1;
                else
                    t1 = (z0 + n0 * t0 - z1) / n1;
                double yTry1 = y0 + m0 * t0, yTry2 = y1 + m1 * t1;
                Vector3D intersection = new Vector3D(x0 + l0 * t0, y0 + m0 * t0, z0 + n0 * t0);
                if (yTry1.AreEqual(yTry2) && intersection.IsOnLine(limitedCurve))
                    return intersection;
                else
                    return null;
            }
            else
            {
                t0 = (n1 * y1 - m1 * z1 - n1 * y0 + m1 * z0) / deltaMN;
                if (!m1.AreEqual(0))
                    t1 = (y0 + m0 * t0 - y1) / m1;
                else
                    t1 = (z0 + n0 * t0 - z1) / n1;
                double xTry1 = x0 + l0 * t0, xTry2 = x1 + l1 * t1;
                Vector3D intersection = new Vector3D(x0 + l0 * t0, y0 + m0 * t0, z0 + n0 * t0);
                if (xTry1.AreEqual(xTry2) && intersection.IsOnLine(limitedCurve))
                    return intersection;
                else
                    return null;
            }
        }
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public Line3D Clone()
        {
            Vector3D s = new Vector3D(Start.X, Start.Y, Start.Z);
            Vector3D e = new Vector3D(End.X, End.Y, End.Z);
            return new Line3D(s, e);
        }

        public bool IsAlmostEqualTo(Line3D other, double toterance)
        {
            return (Start.IsAlmostEqualTo(other.Start, toterance) && End.IsAlmostEqualTo(other.End, toterance)) || (Start.IsAlmostEqualTo(other.End, toterance) && End.IsAlmostEqualTo(other.Start, toterance));
        }

        public bool IsAlmostEqualTo(Line3D other)
        {
            return IsAlmostEqualTo(other, Extension.SMALL_NUMBER);
        }

        public Line3D Offset(double offset, Vector3D vdirection)
        {
            Vector3D startPoint = Start.Offset(offset, vdirection);
            Vector3D endPoint = End.Offset(offset, vdirection);
            return new Line3D(startPoint, endPoint);
        }
        /// <summary>
        /// 将自身平移
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="vdirection"></param>
        public void MoveTo(double offset, Vector3D vdirection)
        {
            Start.MoveTo(offset, vdirection);
            End.MoveTo(offset, vdirection);
        }

        /// <summary>
        /// 两条线是否平行
        /// </summary>
        /// <param name="source"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsParallelWith(Line3D line)
        {
            Vector3D d1 = this.Direction;
            Vector3D d2 = line.Direction;
            double angle = d1.AngleTo(d2);
            return Math.Abs(angle) < Extension.SMALL_NUMBER || Math.Abs(angle - Math.PI) < Extension.SMALL_NUMBER;
        }


        public Line2D ProjectOnXoY()
        {

            return new Line2D(new Vector2D(Start.X,Start.Y),new Vector2D(End.X,End.Y)) ;
        }


    }
    public class Line3DEqualityComparer : IEqualityComparer<Line3D>
    {
        public bool Equals(Line3D l1, Line3D l2)
        {
            return l1.IsAlmostEqualTo(l2);
        }
        public int GetHashCode(Line3D obj)
        {
            return 0;
        }
    }


}
