using System;
using System.Collections.Generic;

namespace Albert.Geometry.Primitives
{

    /// <summary>
    /// 用于处理一个直线的定义
    /// </summary>
    public class Line2D
    {
        private Vector2D start;
        /// <summary>
        /// 起点
        /// </summary>
        public Vector2D Start
        {
            get
            {
                return start;
            }
            set { start = value; }
        }


        private Vector2D end;
        /// <summary>
        /// 终点
        /// </summary>
        public Vector2D End
        {
            get
            {
                return end;
            }
            set { end = value; }
        }
        /// <summary>
        /// 构造函数，初始化一个直线
        /// </summary>
        public Line2D()
        {
            this.start = Vector2D.Zero;
            this.end = Vector2D.Zero;
        }

        /// <summary>
        /// 通过两点初始化一个直线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Line2D(Vector2D start, Vector2D end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// 通过亮点创建一个直线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Line2D Create(Vector2D start, Vector2D end)
        {
            return new Line2D(start, end);
        }
        /// <summary>
        /// 由起点、方向和长度来构造一条直线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="direction"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Line2D Create(Vector2D start, Vector2D direction, double length)
        {
            Vector2D nV = start + length * direction;
            return new Line2D(start, nV);
        }

        /// <summary>
        /// 单位方向向量
        /// </summary>
        public Vector2D Direction
        {
            get
            {
                Vector2D direction = null;
                if (End != null && Start != null)
                    direction = (End - Start).Normalize();
                return direction;
            }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(Math.Pow((End.X - Start.X), 2) + Math.Pow((End.Y - Start.Y), 2));
            }
        }
        /// <summary>
        /// 创建一个相反方向的线段
        /// </summary>
        /// <returns></returns>
        public Line2D Reversed()
        {
            return Create(End, Start);
        }

    
        /// <summary>
        /// 点到直线的距离 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public double Distance(Vector2D source)
        {
            double space = 0;
            var a = Length;
            var b = source.Distance(Start);
            var c = source.Distance(End);
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
            Vector2D v1 = new Vector2D(source.X, source.Y);
            Line2D l1 = Line2D.Create(new Vector2D(this.Start.X, this.Start.Y), new Vector2D(this.End.X, this.End.Y));
            space = Line2D.Create(v1, v1.ProjectOn(l1)).Length;
            return space;
        }



        /// <summary>
        /// 计算水平直线之间的距离
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public double HDistance(Line2D source)
        {
            //计算开始点在指定线上的投影
            Vector2D p = this.start.ProjectOn(source);
            //计算开始点到投影点之间的距离
            return p.Distance(this.start);
        }

        /// <summary>
        /// 以向量direction来偏移
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Line2D Offset(Vector2D v)
        {
            Vector2D p1 = Start.Offset(v);
            Vector2D p2 = End.Offset(v);
            return Create(p1, p2);
        }
        /// <summary>
        /// 根据指定距离和方向偏移
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Line2D Offset(double offset,Vector2D direction)
        {
            Vector2D p1 = Start.Offset(direction.Normalize() * offset);
            Vector2D p2 = End.Offset(direction.Normalize() * offset);
            return Create(p1, p2);
        }

        /// <summary>
        /// 移动到指定的位置
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public void MoveTo(Vector2D v)
        {
            Vector2D p1 = Start.Offset(v);
            Vector2D p2 = End.Offset(v);
            this.start.X = p1.X;
            this.start.Y = p1.Y;
            this.end.X = p2.X;
            this.end.Y = p2.Y;
        }
        /// <summary>
        /// 根据指定距离和方向偏移
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public void MoveTo(double offset, Vector2D direction)
        {
            this.MoveTo(direction.Normalize() * offset);
        }
        /// <summary>
        /// 直线上某点的坐标
        /// </summary>
        /// <param name="parameter">比例</param>
        /// <returns></returns>
        public Vector2D Evaluate(double parameter)
        {
            return (End - Start) * parameter + Start;
        }
        /// <summary>
        /// 当前直线的终点位置
        /// </summary>
        public Vector2D MiddlePoint
        {
            get { return Evaluate(0.5); }
        }

        public Line3D ToLine3D(int v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 判断一个点是否是昂起直线的端点
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private bool IsEndPoint(Line2D line1, Vector2D source)
        {
            if (source.IsAlmostEqualTo(line1.Start) || source.IsAlmostEqualTo(line1.End))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 两条线段的相交点
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Vector2D Intersect(Line2D target)
        {
            Vector2D a = Start;
            Vector2D b = End;
            Vector2D c = target.Start;
            Vector2D d = target.End;
            var s = Math.Abs((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y));
            if (Math.Abs((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y)) < 1e-9)//Extension.SMALL_NUMBER
            {
                if (a.IsOnTwoLine(this, target) && IsEndPoint(target, a) && !IsEndPoint(this, a))
                {
                    return a;
                }
                if (b.IsOnTwoLine(this, target) && IsEndPoint(target, b) && !IsEndPoint(this, b))
                {
                    return b;
                }
                return null;
            }
            double pX = ((b.X - a.X) * (c.X - d.X) * (c.Y - a.Y) - c.X * (b.X - a.X) * (c.Y - d.Y) + a.X * (b.Y - a.Y) * (c.X - d.X)) / ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y));
            double pY = ((b.Y - a.Y) * (c.Y - d.Y) * (c.X - a.X) - c.Y * (b.Y - a.Y) * (c.X - d.X) + a.Y * (b.X - a.X) * (c.Y - d.Y)) / ((b.X - a.X) * (c.Y - d.Y) - (b.Y - a.Y) * (c.X - d.X));
            if ((pX - a.X) * (pX - b.X) <= 1e-6 && (pX - c.X) * (pX - d.X) <= 1e-6 &&
                (pY - a.Y) * (pY - b.Y) <= 1e-6 && (pY - c.Y) * (pY - d.Y) <= 1e-6)
            {
                return new Vector2D(pX, pY);
            }
            return null;
        }

        public Vector2D Intersect2(Line2D target)
        {
            Vector2D a = Start;
            Vector2D b = End;
            Vector2D c = target.Start;
            Vector2D d = target.End;
            var s = Math.Abs((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y));
            if (Math.Abs((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y)) < 1e-9)//Extension.SMALL_NUMBER
            {
                if (a.IsOnTwoLine(this, target) && IsEndPoint(target, a) && !IsEndPoint(this, a))
                {
                    return a;
                }
                if (b.IsOnTwoLine(this, target) && IsEndPoint(target, b) && !IsEndPoint(this, b))
                {
                    return b;
                }
                return null;
            }
            double pX = ((b.X - a.X) * (c.X - d.X) * (c.Y - a.Y) - c.X * (b.X - a.X) * (c.Y - d.Y) + a.X * (b.Y - a.Y) * (c.X - d.X)) / ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y));
            double pY = ((b.Y - a.Y) * (c.Y - d.Y) * (c.X - a.X) - c.Y * (b.Y - a.Y) * (c.X - d.X) + a.Y * (b.X - a.X) * (c.Y - d.Y)) / ((b.X - a.X) * (c.Y - d.Y) - (b.Y - a.Y) * (c.X - d.X));
            if ((pX - a.X) * (pX - b.X) <= 1e-6 && (pX - c.X) * (pX - d.X) <= 1e-6 &&
                (pY - a.Y) * (pY - b.Y) <= 1e-6 && (pY - c.Y) * (pY - d.Y) <= 1e-6)
            {
                return new Vector2D(pX, pY);
            }
            return null;
        }
        /// <summary>
        /// 判断两个直线的相交关系
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Vector2D IntersectStraightLine(Line2D target)
        {
            Vector2D a = this.Start;
            Vector2D b = this.End;
            Vector2D c = target.Start;
            Vector2D d = target.End;
            //叉积等0.说明两条线平行
            if (Math.Abs((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y)) < Extension.SMALL_NUMBER)
            {
                return null;
            }
            else
            {
                double pX = ((b.X - a.X) * (c.X - d.X) * (c.Y - a.Y) - c.X * (b.X - a.X) * (c.Y - d.Y) + a.X * (b.Y - a.Y) * (c.X - d.X)) / ((b.Y - a.Y) * (c.X - d.X) - (b.X - a.X) * (c.Y - d.Y));
                double pY = ((b.Y - a.Y) * (c.Y - d.Y) * (c.X - a.X) - c.Y * (b.Y - a.Y) * (c.X - d.X) + a.Y * (b.X - a.X) * (c.Y - d.Y)) / ((b.X - a.X) * (c.Y - d.Y) - (b.Y - a.Y) * (c.X - d.X));
                return new Vector2D(pX, pY);
            }
        }

        /// <summary>
        /// 比较两个线段是否相同
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAlmostEqualTo(Line2D other)
        {
            return IsAlmostEqualTo(other, Extension.SMALL_NUMBER);
        }

        /// <summary>
        /// 比较两个线段是否相同
        /// </summary>
        /// <param name="other"></param>
        /// <param name="toterance">容差范围</param>
        /// <returns></returns>
        public bool IsAlmostEqualTo(Line2D other, double toterance)
        {
            return (Start.IsAlmostEqualTo(other.Start, toterance) && End.IsAlmostEqualTo(other.End, toterance)) || (Start.IsAlmostEqualTo(other.End, toterance) && End.IsAlmostEqualTo(other.Start, toterance));
        }

        /// <summary>
        /// 判断是否与line平行
        /// </summary>
        /// <param name="source"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsParallelWith(Line2D line)
        {
            Vector2D d1 = this.Direction;
            Vector2D d2 = line.Direction;
            double angle = d1.AngleTo(d2);
            return Math.Abs(angle) < Extension.SMALL_NUMBER || Math.Abs(angle - Math.PI) < Extension.SMALL_NUMBER;
        }

        /// <summary>
        /// 判断一个线是否是属于另外一个线的一部分
        /// </summary>
        /// <param name="source"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsPartOf(Line2D source)
        {
            //两个线不相等，当前线的端点都在目标直线之上
            if (!this.IsAlmostEqualTo(source))
            {
                if (this.Start.IsOnLine(source) && this.end.IsOnLine(source))
                {
                    return true;
                }
            }
            return false;
        }



       
    }

    public class Line2DEqualityComparer : IEqualityComparer<Line2D>
    {
        public bool Equals(Line2D l1, Line2D l2)
        {
            return l1.IsAlmostEqualTo(l2);
        }
        public int GetHashCode(Line2D obj)
        {
            return 0;
        }
    }
}