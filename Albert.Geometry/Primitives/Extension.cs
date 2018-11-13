using Albert.Geometry.External;
using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albert.Geometry.Primitives
{
    public static class Extension
    {
        #region 常量
        public const double SMALL_NUMBER = 1e-5;
        public const double BIG_NUMBER = 1e6;
        public const double INCH_MM = 304.8;
        public const double PI_DEG = 180;
        #endregion

        #region 实数相关
        /// <summary>
        /// 扩展方法，用于判断两个double类型是否接近相等
        /// </summary>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool AreEqual(this double source1, double source2, double tolerance= SMALL_NUMBER)
        {
            return Math.Abs(source1 - source2) < tolerance;
        }

        /// <summary>
        /// 用于比较两个double的关系
        /// </summary>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="result"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static int AreCompare(this double source1, double source2, double tolerance = SMALL_NUMBER)
        {
            var d_value = source1 - source2;
            if (Math.Abs(d_value) < tolerance)
            {
                return 0;
            }
            else
            {
                if (d_value > 0)
                {
                    return 1;
                }
                else {

                    return -1;
                }
            }

        }
        /// <summary>
        /// 去掉重复部分
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static List<double> Distinct(this List<double> args)
        {
            List<double> newList = new List<double> { args.First() };
            args.ForEach(x => { if (!newList.Any(y => y.AreEqual(x))) newList.Add(x); });
            return newList;
        }
        #endregion

        #region 单位转换
        /// <summary>
        /// 毫米转换英尺
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double MMtoInch(double value)
        {
            return value / INCH_MM;
        }
        /// <summary>
        /// 英尺转换毫米
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double InchtoMM(double value)
        {
            return value * INCH_MM;
        }
        /// <summary>
        /// 毫米转换英尺
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3D MMtoInch(Vector3D v)
        {
            return new Vector3D(MMtoInch(v.X), MMtoInch(v.Y), MMtoInch(v.Z));
        }
        /// <summary>
        /// 英尺转换毫米
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3D InchtoMM(Vector3D v)
        {
            return new Vector3D(InchtoMM(v.X), InchtoMM(v.Y), InchtoMM(v.Z));
        }

        /// <summary>
        /// 度转换为弧度
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double DegToRad(double deg)
        {
            return (deg / PI_DEG) * Math.PI;
        }

        /// <summary>
        /// 弧度转化为角度
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double RadToDeg(double rad)
        {
            return (PI_DEG / Math.PI) * rad;
        }
        #endregion

        #region Vector2D相关
        /// <summary>
        /// 按1e-6的容差对点的集合进行去重
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static List<Vector2D> DistinctPoint(this List<Vector2D> points)
        {
            var newPoints = new List<Vector2D> { points[0] };
            points.ForEach(x => { if (!newPoints.Any(y => y.IsAlmostEqualTo(x))) newPoints.Add(x); });
            return newPoints;
        }

        /// <summary>
        /// 当前点是否是直线的端点
        /// </summary>
        /// <param name="source"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsEndPoint(this Vector2D source, Line2D line)
        {
            if (source.IsAlmostEqualTo(line.Start) || source.IsAlmostEqualTo(line.End))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否不在region外部（在内部或边上）
        /// </summary>
        /// <param name="point"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        private static bool IsPossiblyInRegion(this Vector2D point, List<Line2D> region)
        {
            int num = region.Count;
            double[] arrayX = new double[num];
            double[] arrayY = new double[num];
            Vector2D tempXY = new Vector2D(0, 0);
            for (int n = 0; n < region.Count(); n++)
            {
                tempXY = region[n].Start;
                arrayX[n] = tempXY.X;
                arrayY[n] = tempXY.Y;
            }
            double testx = point.X;
            double testy = point.Y;
            int i, j, crossings = 0;
            for (i = 0, j = num - 1; i < num; j = i++)
            {
                if (((arrayY[i] > testy) != (arrayY[j] > testy)) &&
                 (testx < (arrayX[j] - arrayX[i]) * (testy - arrayY[i]) / (arrayY[j] - arrayY[i]) + arrayX[i]))
                    crossings++;
            }
            return (crossings % 2 != 0);
        }

        /// <summary>
        /// 判断是否在Line内部，不包括端点
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsInLine(this Vector2D point, Line2D line)
        {
            return point.IsOnLine(line) && !point.IsAlmostEqualTo(line.Start) && !point.IsAlmostEqualTo(line.End);
        }

        /// <summary>
        /// 判断一个点是否完全是region内部（不会在边上）
        /// </summary>
        /// <param name="point"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static bool IsInRegion(this Vector2D point, List<Line2D> region)
        {
            return point.IsPossiblyInRegion(region) && !point.IsOnRegionEdge(region);
        }

        /// <summary>
        /// 判断是否在region的边上  region.Any(item => point.IsOnLine(item));
        /// </summary>
        /// <param name="point"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static bool IsOnRegionEdge(this Vector2D point, List<Line2D> region)
        {
            return region.Any(point.IsOnLine);
        }

        /// <summary>
        /// 判断是否在line上，包括端点
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsOnLine(this Vector2D point, Line2D line)
        {
            return line.Distance(point) < SMALL_NUMBER;
        }
        public static bool IsOnTwoLine(this Vector2D point, Line2D line1, Line2D line2)
        {
            return point.IsOnLine(line1) && point.IsOnLine(line2);
        }
        public static bool IsParallelWith(this Vector2D point, Vector2D source)
        {
            return (Math.Abs(point.AngleWith(source)) < SMALL_NUMBER
                || Math.Abs(point.AngleWith(source) - Math.PI) < SMALL_NUMBER);
        }

        public static bool IsVerticalWith(this Vector2D v1, Vector2D v2)
        {
            return Math.Abs(v1.AngleTo(v2) - Math.PI / 2) < 1e-6;
        }

        /// <summary>
        /// 点投影到直线上的点
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Vector2D ProjectOn(this Vector2D point, Line2D line)
        {
            //算法说明：先过this点做垂直于curve的平面，得到其方程，
            //求出该平面与curve的交点即为投影点
            Vector2D s = line.Start;
            Vector2D d = line.Direction;
            double t = ((point.X - s.X) * d.X + (point.Y - s.Y) * d.Y) / (d.X * d.X + d.Y * d.Y);
            return new Vector2D(s.X + d.X * t, s.Y + d.Y * t);
        }

        public static double DistanceTo(this Vector2D point, Line2D line)
        {
          //  Vector2D projectPoint = point.ProjectOn(line);
            return 0;
        }
        /// <summary>
        /// 点到直线的垂直距离 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static double VerticalDistanceTo(this Vector2D point, Line2D line)
        {
            Vector2D projectPoint = point.ProjectOn(line);
            return point.Distance(projectPoint);
        }
        #endregion

        #region Vector3D相关
        /// <summary>
        /// 比较两个点在默认误差范围(1e-6)内是否相同
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static bool IsAlmostEqualTo(this Vector3D point1, Vector3D point2)
        {
            return point1.IsAlmostEqualTo(point2, SMALL_NUMBER);
        }

        /// <summary>
        /// 比较两个点在误差范围内是否相同
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool IsAlmostEqualTo(this Vector3D point1, Vector3D point2, double tolerance)
        {
            return point1.Distance(point2) < tolerance;
        }

        /// <summary>
        /// 向量的夹角，值域为[0,π]
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double AngleTo(this Vector3D v1, Vector3D v2)
        {
            if ((v1.Modulus() * v2.Modulus()).AreEqual(0)) return 0;
            var cosA = v1.Dot(v2) / (v1.Modulus() * v2.Modulus());
            if (cosA > 1)
                cosA = 1;
            if (cosA < -1)
                cosA = -1;
            return Math.Acos(cosA);
        }

        /// <summary>
        /// 向量所在直线的夹角，值域为[0,π/2]
        /// </summary>
        /// <param name="v"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double AngleWith(this Vector3D v, Vector3D vector)
        {
            var angle = v.AngleTo(vector);
            return angle < Math.PI / 2 ? angle : Math.PI - angle;
        }

        /// <summary>
        ///向量source逆时针到旋转到终点向量的角度（均为平行于水平面的向量），值域为[0 ,2π)（平行于水平面的向量）
        /// </summary>
        /// <param name="v"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double AngleFrom(this Vector3D v, Vector3D source)
        {
            return v.AngleFrom(source, Vector3D.BasisZ);
        }


        /// <summary>
        /// 设定一个参考平面的法向量，向量source逆时针旋转到终点向量的角度，值域为[0 ,2π)
        /// </summary>
        /// <param name="v"></param>
        /// <param name="source"></param>
        /// <param name="refNormal">参考平面的法向量，与视线看过去的方向相反</param>
        /// <returns></returns>
        public static double AngleFrom(this Vector3D v, Vector3D source, Vector3D refNormal)
        {
            var angle = v.AngleTo(source);
            if (angle.AreEqual(0))
                return 0;
            if (v.Cross(source).AngleTo(refNormal) < Math.PI / 2)
                return 2 * Math.PI - angle;
            return angle;
        }

        /// <summary>
        /// 判断是否在line3D上，包括端点
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line3D"></param>
        /// <returns></returns>
        public static bool IsOnLine(this Vector3D point, Line3D line3D)
        {
            return point.Distance(line3D).AreEqual(0);
        }

        /// <summary>
        /// 判断线段是否经过一个点
        /// </summary>
        /// <param name="line"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsPassPoint(this Line3D line, Vector3D v)
        {
            Vector3D dir1 = (line.Start - v).Normalize();
            Vector3D dir2 = (line.End - v).Normalize();

            if (dir1.IsAlmostEqualTo(dir2) || dir1.IsAlmostEqualTo(-dir2))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否在line3D上，不包括端点
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line3D"></param>
        /// <returns></returns>
        public static bool IsInLine(this Vector3D point, Line3D line3D)
        {
            return point.IsOnLine(line3D) && !point.IsAlmostEqualTo(line3D.Start) && !point.IsAlmostEqualTo(line3D.End);
        }
        /// <summary>
        /// 是否同时在两条线上
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static bool IsOnTwoLine(this Vector3D point, Line3D line1, Line3D line2)
        {
            return point.IsOnLine(line1) && point.IsOnLine(line2);
        }


        /// <summary>
        /// 点到线段的最近距离(不一定是垂直距离)
        /// </summary>
        /// <param name="line3D"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double Distance(this Vector3D source, Line3D line3D)
        {
            double space = 0;
            double a, b, c;
            a = line3D.Start.Distance(line3D.End);
            b = line3D.Start.Distance(source);
            c = line3D.End.Distance(source);
            if (c < Extension.SMALL_NUMBER || b < Extension.SMALL_NUMBER)
            {
                return space;
            }
            if (a < Extension.SMALL_NUMBER)
            {
                space = b;
                return space;
            }
            if (c * c >= a * a + b * b)
            {
                space = b;
                return space;
            }
            if (b * b >= a * a + c * c)
            {
                space = c;
                return space;
            }
            Vector3D v1 = new Vector3D(source.X, source.Y, source.Z);
            Line3D l1 = Line3D.Create(new Vector3D(line3D.Start.X, line3D.Start.Y, line3D.Start.Z), new Vector3D(line3D.End.X, line3D.End.Y, line3D.End.Z));
            space = Line3D.Create(v1, v1.ProjectOn(l1)).Length;
            return space;
        }

        /// <summary>
        /// 获取两点之间的水平距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double HorizontalDistanceOfPoints(this Vector3D start, Vector3D end)
        {
            Vector3D v1 = new Vector3D(start.X, start.Y, 0);
            Vector3D v2 = new Vector3D(end.X, end.Y, 0);
            return (v1 - v2).Modulus();
        }
        /// <summary>
        /// 给在同一直线上的点，按照坐标从小到大排序，并按照X、Y、Z的优先级依次排列
        /// </summary>
        /// <param name="sourcePoints"></param>
        /// <returns></returns>
        public static List<Vector3D> OrderByXYZ(this List<Vector3D> sourcePoints)
        {
            Vector3D direction = null;
            if (sourcePoints.Count >= 2)
            {
                for (int i = 1; i < sourcePoints.Count; i++)
                {
                    direction = (sourcePoints[i] - sourcePoints[0]).Normalize();
                    if (!direction.IsAlmostEqualTo(Vector3D.Zero))
                        break;
                }
            }
            if (direction != null && !direction.IsAlmostEqualTo(Vector3D.Zero))
            {
                if (direction.Dot(Vector3D.BasisX).AreEqual(0))
                {
                    if (direction.IsAlmostEqualTo(Vector3D.BasisY) || direction.IsAlmostEqualTo(-Vector3D.BasisY))
                        sourcePoints = sourcePoints.OrderBy(x => x.Y).ToList();
                    else
                        sourcePoints = sourcePoints.OrderBy(x => x.Z).ToList();
                }
                else
                {
                    sourcePoints = sourcePoints.OrderBy(x => x.X).ToList();
                }
            }
            return sourcePoints;
        }

        /// <summary>
        /// 给同一条直线的点，按照线的方向排序
        /// </summary>
        /// <param name="sourcePoints"></param>
        /// <param name="lineDirection"></param>
        /// <returns></returns>
        public static List<Vector3D> OrderByDirection(this List<Vector3D> sourcePoints, Vector3D lineDirection)
        {
            sourcePoints = sourcePoints.OrderByXYZ();
            if (sourcePoints.Count >= 2)
            {
                Vector3D direction = (sourcePoints.Last() - sourcePoints.First()).Normalize();
                if (direction.IsAlmostEqualTo(-lineDirection))
                    sourcePoints.Reverse();
            }
            return sourcePoints;
        }
        /// <summary>
        /// 返回点投影到直线上的点
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="line3D"></param>
        /// <returns></returns>
        public static Vector3D ProjectOn(this Vector3D v1, Line3D line3D)
        {
            //算法说明：先过v1点做垂直于line3D的平面，得到其方程，
            //求出该平面与line3D的交点即为投影点
            var s = line3D.Start;
            var d = line3D.Direction;
            var t = ((v1.X - s.X) * d.X + (v1.Y - s.Y) * d.Y + (v1.Z - s.Z) * d.Z) / (d.X * d.X + d.Y * d.Y + d.Z * d.Z);
            return new Vector3D(s.X + d.X * t, s.Y + d.Y * t, s.Z + d.Z * t);
        }


        /// <summary>
        /// 将点影到xoy屏幕
        /// </summary>
        /// <param name="line">需要投影的线</param>
        /// <returns></returns>
        public static Vector3D ProjectOnXoY(this Vector3D point)
        {
            return new Vector3D(point.X, point.Y, 0);
        }

        #endregion

        #region Line2D相关

        ///// <summary>
        ///// 线段相互打断，分解成最小单元
        ///// </summary>
        ///// <param name="lines"></param>
        ///// <returns></returns>
        //public static List<Line2D> Decompose(this List<Line2D> lines)
        //{
        //    List<Line2D> newLines = new List<Line2D>(lines);
        //    bool b = false;
        //    int i = 0;
        //    while (!b)
        //    {
        //        int num = 0;
        //        Line2D c1 = newLines[i];
        //        for (int j = newLines.Count - 1; j > i; j--)
        //        {
        //            Line2D c2 = newLines[j];
        //            Vector2D point = c1.Intersect(c2);
        //            if (point != null)
        //            {
        //                if (!point.IsEndPoint(c1))
        //                {
        //                    num++;
        //                    newLines.Remove(c1);
        //                    newLines.Add(Line2D.Create(c1.Start, point));
        //                    newLines.Add(Line2D.Create(point, c1.End));
        //                }
        //                if (!point.IsEndPoint(c2))
        //                {
        //                    num++;
        //                    newLines.Remove(c2);
        //                    newLines.Add(Line2D.Create(c2.Start, point));
        //                    newLines.Add(Line2D.Create(point, c2.End));
        //                }
        //            }
        //            if (num != 0)
        //                break;
        //        }
        //        if (num == 0)
        //        {
        //            if (i == newLines.Count - 1)
        //                b = true;
        //            i++;
        //        }
        //        else
        //            i = 0;
        //    }//以上步骤是把多片墙体相交分成单独一段
        //    return newLines;
        //}

        /// <summary>
        /// 线段相互打断，分解成最小单元
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<Line2D> Decompose(this List<Line2D> lines)
        {
            List<Line2D> newLines = new List<Line2D>(lines);
            bool b = false;
            int i = 0;
            while (!b)
            {
                int num = 0;
                Line2D c1 = newLines[i];
                for (int j = newLines.Count - 1; j > i; j--)
                {
                    Line2D c2 = newLines[j];
                    Vector2D point = c1.Intersect(c2);
                    if (point != null)
                    {
                        if (!point.IsEndPoint(c1))
                        {
                            num++;
                            newLines.Remove(c1);
                            newLines.Add(Line2D.Create(c1.Start, point));
                            newLines.Add(Line2D.Create(point, c1.End));
                        }
                        if (!point.IsEndPoint(c2))
                        {
                            num++;
                            newLines.Remove(c2);
                            newLines.Add(Line2D.Create(c2.Start, point));
                            newLines.Add(Line2D.Create(point, c2.End));
                        }
                    }
                    if (num != 0)
                        break;
                }
                if (num == 0)
                {
                    if (i == newLines.Count - 1)
                        b = true;
                    i++;
                }
                else
                    i = 0;
            }//以上步骤是把多片墙体相交分成单独一段
            return newLines;
        }

        /// <summary>
        /// 返回直线被点分割后的线段
        /// </summary>
        /// <param name="line2D"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static List<Line2D> DivideViaPoints(this Line2D line2D, List<Vector2D> points)
        {
            List<Line2D> dividedCurves = new List<Line2D>();
            if (points == null || points.Count == 0 || points.Any(x => !x.IsOnLine(line2D)))
            {
                dividedCurves.Add(line2D);
            }
            else
            {
                List<Vector2D> allPoints = new List<Vector2D> { line2D.Start, line2D.End };
                allPoints.AddRange(points);
                allPoints = allPoints.DistinctPoint();
                allPoints = allPoints.OrderBy(x => x.Distance(line2D.Start)).ToList();
                for (int i = 0; i < allPoints.Count - 1; i++)
                {
                    dividedCurves.Add(Line2D.Create(allPoints[i], allPoints[i + 1]));
                }
            }
            return dividedCurves;
        }
        /// <summary>
        /// 偏移值若为正向外偏移，若为负向内偏移。
        /// </summary>
        /// <param name="startExtend"></param>
        /// <param name="endExtend"></param>
        /// <returns></returns>
        public static Line2D Extend(this Line2D line2D, double startExtend, double endExtend)
        {
            Vector2D newStart = new Vector2D(0, 0);
            Vector2D newEnd = new Vector2D(0, 0);
            if (startExtend > 0)
            {
                newStart = line2D.Start - line2D.Direction * startExtend;
            }
            else
            {
                newStart = line2D.Start + line2D.Direction * Math.Abs(startExtend);
            }
            if (endExtend > 0)
            {
                newEnd = line2D.End + line2D.Direction * endExtend;
            }
            else
            {
                newEnd = line2D.End - line2D.Direction * Math.Abs(endExtend);
            }
            return Line2D.Create(newStart, newEnd);
        }

        /// <summary>
        /// 从逆时针转成顺时针或相反
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<Line2D> Flip(this List<Line2D> lines)
        {
            List<Line2D> newLines = new List<Line2D> { Line2D.Create(lines[0].End, lines[0].Start) };
            for (int i = lines.Count - 1; i > 0; i--)
            {
                newLines.Add(Line2D.Create(lines[i].End, lines[i].Start));
            }
            return newLines;
        }

        /// <summary>
        /// 获得直线的端点，无重复
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<Vector2D> GetEndPoints(this List<Line2D> lines)
        {
            List<Vector2D> newPoints = new List<Vector2D>();
            foreach (var item in lines)
            {
                newPoints.Add(item.Start);
                newPoints.Add(item.End);
            }
            return newPoints.DistinctPoint();
        }

        public static List<Line2D> GetOutline(this List<Line2D> lines, List<Line2D> outLines)
        {
            List<Line2D> removeLines = new List<Line2D>();
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < outLines.Count; j++)
                {
                    GraphicAlgorithm.TwoParallelLines2D two = new GraphicAlgorithm.TwoParallelLines2D(lines[i], outLines[j]);
                    if (two.Relationship == GraphicAlgorithm.TwoParallelLinesRelationship.线段1全在线段2内 ||
                        two.Relationship == GraphicAlgorithm.TwoParallelLinesRelationship.完全相同)
                    {
                        removeLines.Add(lines[i]);
                    }
                }
            }
            return removeLines;
        }


        /// <summary>
        /// 判断是否与line共线
        /// </summary>
        /// <param name="source"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsCollinearWith(this Line2D source, Line2D line)
        {
            if (!source.IsParallelWith(line))
                return false;
            Vector2D point = source.Start.ProjectOn(line);
            return point.Distance(source.Start) < SMALL_NUMBER;
        }

        //判断一个线是否在一个区域之内
        public static bool IsInRegion(this Line2D line, List<Line2D> region)
        {
            //是否具有相交点
            bool isIntersect = region.Any(x => x.Intersect(line) != null);
            //开始点是否在区域内
            bool isStartInRegion = line.Start.IsInRegion(region);
            //结束点是否在区域内
            bool isEndInRegion = line.End.IsInRegion(region);
            //是否开始点在边缘线上
            bool isStartOnRegionEdge = line.Start.IsOnRegionEdge(region);
            //是否结束点在边缘线上
            bool isEndOnRegionEdge = line.End.IsOnRegionEdge(region);
            //是否和边缘线叠加
            bool isSuperposition = line.IsSuperpositionWithRegionEdge(region);
            int intersectN = 0;
            List<Vector2D> intersect = new List<Vector2D>();
            //判断相交的线段
            region.ForEach(x =>
            {
                Vector2D v1 = x.Intersect(line);
                if (v1 != null && intersect.Find(y => y.IsAlmostEqualTo(v1)) == null)
                {
                    intersect.Add(v1);
                }
            });
            intersectN = intersect.Count;
            //都在区域内，没有交点，则在区域内
            if (isStartInRegion && isEndInRegion && !isIntersect)
                return true;
            //都在线上，有两个交点，且不重叠，则在区域内
            if (isStartOnRegionEdge && isEndOnRegionEdge && intersectN == 2 && !isSuperposition)
                return true;
            //有一个在区域内，一个在线上，且不重叠，有一个交点
            if (isStartInRegion && isEndOnRegionEdge && !isSuperposition && intersectN == 1)
                return true;
            if (isStartOnRegionEdge && isEndInRegion && !isSuperposition && intersectN == 1)
                return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static bool IsCounterclockwise(this List<Line2D> lines)
        {
            List<Line2D> newLines = lines.OrderBy(x => x.Start.X).ToList();
            Vector2D convexPoint = newLines.First().Start;
            Vector2D dir1 = (from line in lines where line.End.IsAlmostEqualTo(convexPoint) select line.Direction).FirstOrDefault();
            Vector2D dir2 = newLines.First().Direction;
            return dir1 != null && dir1.Cross(dir2) > SMALL_NUMBER;
        }

        /// <summary>
        /// 完全在区域边内部或跟边相同
        /// </summary>
        /// <param name="line2D"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static bool IsOnRegionEdge(this Line2D line2D, List<Line2D> region)
        {
            bool b = false;
            foreach (var item in region)
            {
                GraphicAlgorithm.TwoParallelLines2D lines = new GraphicAlgorithm.TwoParallelLines2D(line2D, item);
                if (lines.Relationship == GraphicAlgorithm.TwoParallelLinesRelationship.线段1全在线段2内 ||
                    lines.Relationship == GraphicAlgorithm.TwoParallelLinesRelationship.完全相同)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
        /// <summary>
        /// 一部分在region内
        /// </summary>
        /// <param name="line2D"></param>
        /// <param name="region"></param>
        /// <param name="intersections"></param>
        /// <returns>传出与region的交点</returns>
        private static bool IsPartInRegion(this Line2D line2D, List<Line2D> region, out List<Vector2D> intersections)
        {
            intersections = new List<Vector2D>();
            bool isIntersect = region.Any(x => line2D.Intersect(x) != null);
            bool isInRegion = line2D.IsInRegion(region);
            if (isIntersect && isInRegion)
            {
                intersections.AddRange(region.Select(line2D.Intersect).Where(point => point != null));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 一部分在区域边界上
        /// </summary>
        /// <param name="line2D"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static bool IsPartOnRegionEdge(this Line2D line2D, List<Line2D> region)
        {
            foreach (var item in region)
            {
                GraphicAlgorithm.TwoParallelLines2D lines = new GraphicAlgorithm.TwoParallelLines2D(line2D, item);
                return lines.Relationship == GraphicAlgorithm.TwoParallelLinesRelationship.两条线段部分搭接;
            }
            return false;
        }
        /// <summary>
        /// 一部分在region内或边上，一部分在region外，不包含只有一个点在边界上的情况
        /// </summary>
        /// <param name="line2D"></param>
        /// <param name="region"></param>
        /// <param name="innerLine"></param>
        /// <returns>传出region内部的线段</returns>
        public static bool IsPartInRegion(this Line2D line2D, List<Line2D> region, out List<Line2D> innerLine)
        {
            List<Vector2D> intersections;
            innerLine = new List<Line2D>();
            bool isPartIn = false;
            bool b = line2D.IsPartInRegion(region, out intersections);
            if (b)
            {
                List<Line2D> cutedLines = line2D.DivideViaPoints(intersections);
                foreach (var item in cutedLines)
                {
                    if (item.IsInRegion(region))
                    {
                        innerLine.Add(item);
                        isPartIn = true;
                    }
                }
            }
            return isPartIn;
        }
        public static bool IsPartInRegion(this Line2D line2D, List<Line2D> region)
        {
            List<Vector2D> intersections;
            bool isPartIn = false;
            bool b = line2D.IsPartInRegion(region, out intersections);
            if (b)
            {
                List<Line2D> cutedLines = line2D.DivideViaPoints(intersections);
                foreach (var item in cutedLines)
                {
                    if (item.IsInRegion(region))
                    {
                        isPartIn = true;
                    }
                }
            }
            return isPartIn;
        }

        /// <summary>
        /// 是否和当前边缘线叠加
        /// </summary>
        /// <param name="line"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        private static bool IsSuperpositionWithRegionEdge(this Line2D line, List<Line2D> region)
        {
            foreach (var item in region)
            {
                GraphicAlgorithm.TwoParallelLines2D two = new GraphicAlgorithm.TwoParallelLines2D(line, item);
                GraphicAlgorithm.TwoParallelLinesRelationship s = two.Relationship;
                if (s == GraphicAlgorithm.TwoParallelLinesRelationship.两条线段部分搭接 ||
                    s == GraphicAlgorithm.TwoParallelLinesRelationship.完全相同 ||
                    s == GraphicAlgorithm.TwoParallelLinesRelationship.线段2全在线段1内)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 一个线属于例外一个线的一部分，//line和source调换位置，让它符合命名，调用它的代码也做了相应修改，2017-6-9-ByJohnny
        /// </summary>
        /// <param name="source"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsPartOf(this Line2D line, Line2D source)
        {
            if (line.Start.IsOnLine(source) && line.End.IsOnLine(source))
            {

                return true;
            }
            return false;
        }
        public static List<Line2D> MakeCounterclockwise(this List<Line2D> lines)
        {
            List<Line2D> sortedLines = new List<Line2D>();
            //添加所有BaseLine为线的起点
            sortedLines.Add(lines.First());
            Vector2D endPoint = lines.First().End;
            lines.RemoveAt(0);
            GraphicAlgorithm.HuntCurveByStartPoint(lines, endPoint, sortedLines);
            if (!sortedLines.IsCounterclockwise())
                sortedLines = sortedLines.Flip();
            return sortedLines;
        }

        /// <summary>
        /// 两线段的差集
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static List<Line2D> Minus(this Line2D line1, Line2D line2)
        {
            List<Line2D> minusLines = new List<Line2D>();
            GraphicAlgorithm.TwoParallelLines2D two = new GraphicAlgorithm.TwoParallelLines2D(line1, line2);
            GraphicAlgorithm.TwoParallelLinesRelationship re = two.Relationship;
            if (re == GraphicAlgorithm.TwoParallelLinesRelationship.完全相同 ||
                re == GraphicAlgorithm.TwoParallelLinesRelationship.不共线 ||
                re == GraphicAlgorithm.TwoParallelLinesRelationship.不平行 ||
                re == GraphicAlgorithm.TwoParallelLinesRelationship.两条线段端点搭接 ||
                re == GraphicAlgorithm.TwoParallelLinesRelationship.平行不共线 ||
                re == GraphicAlgorithm.TwoParallelLinesRelationship.线段1全在线段2内
                )
                return null;
            else if (re == GraphicAlgorithm.TwoParallelLinesRelationship.线段2全在线段1内)
            {
                if (Math.Abs(line1.Direction.AngleTo(line2.Direction)) < SMALL_NUMBER)
                {
                    Line2D lineTemp1 = Line2D.Create(line1.Start, line2.Start);
                    Line2D lineTemp2 = Line2D.Create(line2.End, line1.End);
                    if (lineTemp1.Length > SMALL_NUMBER)
                        minusLines.Add(lineTemp1);
                    if (lineTemp2.Length > SMALL_NUMBER)
                        minusLines.Add(lineTemp2);
                }
                else
                {
                    Line2D line22 = line2.Reversed();
                    Line2D lineTemp1 = Line2D.Create(line1.Start, line22.Start);
                    Line2D lineTemp2 = Line2D.Create(line22.End, line1.End);
                    if (lineTemp1.Length > SMALL_NUMBER)
                        minusLines.Add(lineTemp1);
                    if (lineTemp2.Length > SMALL_NUMBER)
                        minusLines.Add(lineTemp2);
                }
            }
            else if (re == GraphicAlgorithm.TwoParallelLinesRelationship.两条线段部分搭接)
            {
                if (Math.Abs(line1.Direction.AngleTo(line2.Direction)) < SMALL_NUMBER)
                {
                    if (line2.Start.IsOnLine(line1))
                        minusLines.Add(Line2D.Create(line1.Start, line2.Start));
                    else
                        minusLines.Add(Line2D.Create(line1.End, line2.End));
                }
                else
                {
                    if (line2.Start.IsOnLine(line1))
                        minusLines.Add(Line2D.Create(line1.Start, line2.End));
                    else
                        minusLines.Add(Line2D.Create(line1.End, line2.Start));
                }
            }
            return minusLines;
        }

        /// <summary>
        /// 拷贝一个线
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line2D Copy(this Line2D line)
        {
            Line2D copy = Line2D.Create(Vector2D.Create(line.Start.X, line.Start.Y), Vector2D.Create(line.End.X, line.End.Y));
            return copy;
        }
        /// <summary>
        /// 拷贝所有的线段
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<Line2D> Copy(this List<Line2D> lines)
        {

            List<Line2D> copys = new List<Line2D>();

            lines.ForEach(x =>
            {

                copys.Add(x.Copy());
            });
            return copys;
        }

        /// <summary>
        /// 两个直线之间的距离
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static double DistanceTo(this Line2D line1, Line2D line2) {
            if (line1.IsParallelWith(line2))
            {
                var p = line1.Start.ProjectOn(line2);
                return line1.Start.Distance(p);
            }
            var intersect = line1.IntersectStraightLine(line2);
            return 0;
        }
        #endregion

        #region Line3D相关

        /// <summary>
        /// 求两条直线（有端点的直线）之间的距离
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static double DistanceTo(this Line3D line1, Line3D line2)
        {
            if (line1.IsParallelWith(line2))
            {
                var p = line1.Start.ProjectOn(line2);
                return line1.Start.Distance(p);
            }
            var intersect = line1.IntersectStraightLine(line2);
            if (intersect == null)//肯定为异面直线
            {
                Vector3D normal = line1.Direction.Cross(line2.Direction);
                return Math.Abs((line1.Start - line2.Start).Dot(normal)) / normal.Modulus();
            }
            return 0;
        }




        /// <summary>
        /// 偏移值若为正向外偏移，若为负向内偏移。
        /// </summary>
        /// <param name="line3D"></param>
        /// <param name="startExtend"></param>
        /// <param name="endExtend"></param>
        /// <returns></returns>
        public static Line3D Extend(this Line3D line3D, double startExtend, double endExtend)
        {
            Vector3D newStart;
            Vector3D newEnd;
            if (startExtend >= 0)
            {
                newStart = line3D.Start - line3D.Direction * startExtend;
            }
            else
            {
                newStart = line3D.Start + line3D.Direction * Math.Abs(startExtend);
            }
            if (endExtend >= 0)
            {
                newEnd = line3D.End + line3D.Direction * endExtend;
            }
            else
            {
                newEnd = line3D.End - line3D.Direction * Math.Abs(endExtend);
            }
            return Line3D.Create(newStart, newEnd);
        }


        /// <summary>
        /// 当前线是否是当前线的一部分
        /// </summary>
        /// <param name="source"></param>
        /// <param name="curve2"></param>
        /// <returns></returns>
        public static bool IsPartOf(this Line3D source, Line3D curve2)
        {
            Vector3D startPoint1 = source.Start;
            Vector3D endPoint1 = source.End;
            if (startPoint1.IsOnLine(curve2) && endPoint1.IsOnLine(curve2))
                return true;
            return false;
        }

        /// <summary>
        /// 是否重合
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool CoincidesWith(this Line3D source, Line3D line3D)
        {
            return source.IsPartOf(line3D) && line3D.IsPartOf(source);
        }

        public static List<Line3D> Flip(this List<Line3D> lines)
        {
            List<Line3D> newLines = new List<Line3D> { Line3D.Create(lines[0].End, lines[0].Start) };
            for (int i = lines.Count - 1; i > 0; i--)
            {
                newLines.Add(Line3D.Create(lines[i].End, lines[i].Start));
            }
            return newLines;
        }

        /// <summary>
        /// 计算一个点是否是一个直线的端点
        /// </summary>
        /// <param name="point"></param>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static bool IsEndPoint(this Vector3D point, Line3D curve)
        {
            return point.IsAlmostEqualTo(curve.Start, Extension.SMALL_NUMBER) |
                   point.IsAlmostEqualTo(curve.End, Extension.SMALL_NUMBER);
        }

        /// <summary>
        /// 创建一条新line3d，端点相反
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Line3D CreateReverse(this Line3D line)
        {
            Vector3D start = line.Start;
            Vector3D end = line.End;
            return new Line3D(new Vector3D(end.X, end.Y, end.Z), new Vector3D(start.X, start.Y, start.Z));
        }

        /// <summary>
        /// 点到空间直线的垂足
        /// </summary>
        /// <param name="line3D"></param>
        /// <param name="point">三维空间中的某点</param>
        /// <returns>返回该直线上空间点的垂足</returns>
        public static Vector3D GetPedal(this Line3D line3D, Vector3D point)
        {
            // pedal=point+((point-p0) X unitVector) X unitVectorNe)
            // P-空间中的任意一点；p0-直线上的任意一点；unitVector-直线的单位向量；pedal-垂足
            //该函数和projecton为同一功能
            Vector3D pedal = null;
            Vector3D p0 = line3D.Start;//直线上的任意一点，此时选择直线的起点
            Vector3D unitVector = line3D.Direction.Normalize();
            pedal = point + ((point - p0).Cross(unitVector).Cross(unitVector));
            return pedal;
        }

        /// <summary>
        /// 判断当前线段是否是水平的
        /// </summary>
        /// <param name="line3D"></param>
        /// <returns></returns>
        public static bool IsHorizontal(this Line3D line3D)
        {
            return line3D.End.Z.AreEqual(line3D.Start.Z);
        }

        /// <summary>
        /// 判断是否与line平行
        /// </summary>
        /// <param name="source"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsParallelWith(this Line3D source, Line3D line)
        {
            Vector3D d1 = source.Direction;
            Vector3D d2 = line.Direction;
            double angle = d1.AngleTo(d2);
            return Math.Abs(angle) < SMALL_NUMBER || Math.Abs(angle - Math.PI) < SMALL_NUMBER;
        }

        /// <summary>
        /// 判断是否与line共线
        /// </summary>
        /// <param name="source"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsCollinearWith(this Line3D source, Line3D line)
        {
            if (!source.IsParallelWith(line))
                return false;
            Vector3D point = source.Start.ProjectOn(line);
            return point.Distance(source.Start) < SMALL_NUMBER;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines">原始线段集合</param>
        /// <param name="normal">参考平面的方向</param>
        /// <returns></returns>
        public static bool IsCounterclockwise(this List<Line3D> lines, Vector3D normal)
        {
            List<Line3D> newLines = lines.OrderBy(x => x.Start.X).ToList();
            Vector3D convexPoint = newLines.First().Start;//凸点
            Vector3D dir1 = null; //有问题         
            lines.ForEach(x =>
            {
                if (x.End.IsAlmostEqualTo(convexPoint))
                    dir1 = x.Direction;
            });
            Vector3D dir2 = newLines.First().Direction;
            return dir1.Cross(dir2).Normalize().AngleTo(normal) < Math.PI / 2;
        }

        public static bool IsVertical(this Line3D line3D)
        {
            return line3D.End.X.AreEqual(line3D.Start.X) && line3D.End.Y.AreEqual(line3D.Start.Y);
        }

        public static bool IsIncline(this Line3D line3D)
        {
            return !line3D.IsVertical() && !line3D.IsHorizontal();
        }
        /// <summary>
        /// 将线投影到xoy屏幕
        /// </summary>
        /// <param name="line">需要投影的线</param>
        /// <returns></returns>
        public static Line3D ProjectOnXoY(this Line3D line)
        {
            Vector3D startPoint = new Vector3D(line.Start.X, line.Start.Y, 0);
            Vector3D endPoint = new Vector3D(line.End.X, line.End.Y, 0);
            return new Line3D(startPoint, endPoint);
        }

        /// <summary>
        /// 将线投影到xoy屏幕
        /// </summary>
        /// <param name="lines">需要投影的线</param>
        /// <returns></returns>
        public static List<Line3D> ProjectOnXoY(this List<Line3D> lines)
        {
            return lines.Select(ProjectOnXoY).ToList();
        }
        public static List<Line3D> SortLinesByCounterClockwise(this List<Line3D> originalLines, Vector3D normal)
        {
            List<Line3D> sortedLines = originalLines.SortLinesContinuously();
            if (!sortedLines.IsCounterclockwise(normal))
                sortedLines = sortedLines.Flip();
            return sortedLines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalLines"></param>
        /// <returns></returns>
        public static List<Line3D> SortLinesContinuously(this List<Line3D> originalLines)
        {
            List<Line3D> copiedOriginalLines = new List<Line3D>(originalLines);
            List<Line3D> sortedLines = new List<Line3D>();
            //添加所有BaseLine为线的起点
            sortedLines.Add(copiedOriginalLines.First());
            Vector3D endPoint = copiedOriginalLines.First().End;
            copiedOriginalLines.RemoveAt(0);
            GraphicAlgorithm.HuntCurveByStartPoint(copiedOriginalLines, endPoint, sortedLines);

            return sortedLines;
        }
        /// <summary>
        /// 线段是否连续，首尾相接
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static bool IsContinuous(this List<Line3D> lines)
        {
            bool b = lines.Last().End.IsAlmostEqualTo(lines.First().Start);
            if (b)
            {
                for (int i = 0; i < lines.Count - 1; i++)
                {
                    if (!lines[i].End.IsAlmostEqualTo(lines[i + 1].Start))
                    {
                        b = false;
                        break;
                    }
                }
            }
            return b;
        }
        /// <summary>
        /// 以第i个元素为起点，重新排序
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static void ReSort(this List<Line3D> lines, int i)
        {
            var newLines = new List<Line3D>(lines);
            lines.Clear();
            for (int j = i; j < newLines.Count; j++)
            {
                lines.Add(newLines[j]);
            }
            for (int j = 0; j < i; j++)
            {
                lines.Add(newLines[j]);
            }
        }

        /// <summary>
        /// 当前多线段中，是否包含指定的线段集合中一条线段
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static Line3D Line3DsContainOneOfLine3Ds(this List<Line3D> lines, List<Line3D> compare)
        {

            foreach (Line3D x in lines)
            {
                Line3D line = compare.Find(y => y.IsAlmostEqualTo(x));

                if (line != null)
                {
                    return line;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取多边形原点
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static Vector3D GetOrigin(this List<Line3D> edges)
        {
            Vector3D origin = null;
            //原点
            Vector3D temp = new Vector3D(0, 0, 0);
            foreach (Line3D line in edges)
            {
                temp += line.Start;
                temp += line.End;
            }
            if (edges.Count > 0)
                origin = temp / (edges.Count * 2);
            return origin;
        }


        /// <summary>
        /// 获取规则多边形的中心点
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static Vector3D GetCenter(this List<Line3D> edges)
        {

            double x = 0, y = 0, z = 0;
            edges.ForEach(p =>
            {

                x += p.Start.X;
                x += p.End.X;
                y += p.Start.Y;
                y += p.End.Y;
                z += p.Start.Z;
                z += p.End.Z;
            });
            int count = edges.Count * 2;
            return new Vector3D(x / count, y / count, z / count);
        }
        /// <summary>
        /// 获取当前多边形的法向量
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static Vector3D GetNormal(this List<Line3D> lines)
        {
            Vector3D refNomal = null;
            for (int i = 0; i < lines.Count - 1; i++)
            {
                Vector3D tempNormal = lines[i].Direction.Cross(lines[i + 1].Direction).Normalize();
                if (!tempNormal.IsAlmostEqualTo(Vector3D.Zero))
                {
                    refNomal = tempNormal;
                    break;
                }
            }
            return refNomal;
        }

        /// <summary>
        /// 拷贝一个多边形线段
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<Line3D> Copy(this List<Line3D> lines)
        {

            List<Line3D> mergeLines = new List<Line3D>();
            //产生不相关点
            lines.ForEach(x =>
            {
                mergeLines.Add(Line3D.Create(Vector3D.Create(x.Start.X, x.Start.Y, x.Start.Z), Vector3D.Create(x.End.X, x.End.Y, x.End.Z)));
            });
            return mergeLines;
        }

        /// <summary>
        /// 拷贝一个多边形线段
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static Line3D Copy(this Line3D line)
        {

            Line3D mergeLine = null;
            //产生不相关点
            mergeLine = Line3D.Create(Vector3D.Create(line.Start.X, line.Start.Y, line.Start.Z), Vector3D.Create(line.End.X, line.End.Y, line.End.Z));

            return mergeLine;
        }


        /// <summary>
        /// 将所有的线进行偏移得到新的线段集合
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static List<Line3D> Offset(this List<Line3D> lines, double offset, Vector3D direction)
        {

            List<Line3D> offsetLines = new List<Line3D>();

            //对所有的线段进行偏移
            lines.ForEach(x =>
            {
                offsetLines.Add(x.Offset(offset, direction));
            });
            //返回偏移线段
            return offsetLines;
        }



        #endregion

        #region 面相关

        //获取一个面的法线方向
        public static Vector3D pNormal(this Face f)
        {

            List<Line3D> edges = null;// new List<Line3D>(f.Edges);
            //任意取两个共点的线
            Line3D L1 = null; // f.Edges[0];
            edges.Remove(L1);

            Line3D L2 = edges.Find(x => x.Start.IsAlmostEqualTo(L1.End) || x.End.IsAlmostEqualTo(L1.End));

            Vector3D v1 = L1.Direction;
            Vector3D v2 = L2.Direction;
            Vector3D v3 = v1.Cross(v2);
            //只取向下的向量
            if (v3.Z > 0)
            {

                return -v3;
            }
            return v3;
        }

        /// <summary>
        /// 向指定方向偏移一点距离
        /// </summary>
        /// <param name="f"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Face Offset(this Face f, double offset, Vector3D direction)
        {
            /**
            List<Line3D> edges = new List<Line3D>();
            f.Edges.ForEach(x =>
            {
                edges.Add(x.Offset(offset, direction));
            });
    */
            Face nf = null; // Face.Create(edges);
            return nf;
        }

        #endregion
    }
}
