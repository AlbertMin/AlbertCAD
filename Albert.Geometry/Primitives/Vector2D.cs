using System;
using System.Collections.Generic;

/// <summary>
/// 当前类用于定义一个二维图形点
/// </summary>
namespace Albert.Geometry.Primitives
{
    /// <summary>
    /// 当前定义了一个二维向量类
    /// </summary>
    public class Vector2D
    {
        private double x;

        public double X
        {
            get { return x; }
            set { x = value; }
        }
        private double y;

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// 当前定义的几个固定属性
        /// </summary>
        public static readonly Vector2D Zero = new Vector2D(0, 0);
        /// <summary>
        /// 当前的X轴
        /// </summary>
        public static readonly Vector2D BasisX = new Vector2D(1, 0);
        /// <summary>
        /// 当前的Y轴
        /// </summary>
        public static readonly Vector2D BasisY = new Vector2D(0, 1);
        /// <summary>
        /// 构建一个二维向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 初始化二维向量
        /// </summary>
        public Vector2D()
        {
            x = 0;
            y = 0;
        }
        /// <summary>
        /// 创建一个二维向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector2D Create(double x, double y)
        {
            return new Vector2D(x, y);
        }
        /// <summary>
        /// 通过其他二维向量，构建新的二维向量
        /// </summary>
        /// <param name="source"></param>
        public Vector2D(Vector2D source)
        {
            x = source.X;
            y = source.Y;
        }


        /// <summary>
        /// 返回当前向量的模
        /// </summary>
        /// <returns></returns>
        public double Modulus()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }
        /// <summary>
        /// 归一化
        /// </summary>
        /// <returns></returns>
        public Vector2D Normalize()
        {
            double mod = Modulus();
            return mod < Extension.SMALL_NUMBER ? new Vector2D() : new Vector2D(X / mod, Y / mod);
        }

        /// <summary>
        /// 两个向量之间的距离
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public double Distance(Vector2D source)
        {
            return Math.Sqrt(Math.Pow(X - source.X, 2) + Math.Pow(Y - source.Y, 2));
        }
        /// <summary>
        /// 向量的点积
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public double Dot(Vector2D source)
        {
            return (X * source.X + Y * source.Y);
        }
        /// <summary>
        /// 向量的叉积
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public double Cross(Vector2D source)
        {
            return X * source.Y - Y * source.X;
        }
        /// <summary>
        /// 向量减
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.X, -v.Y);
        }
        /// <summary>
        ///  向量减
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }
        /// <summary>
        ///  向量加
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v2.X + v1.X, v2.Y + v1.Y);
        }
        /// <summary>
        /// 向量乘
        /// </summary>
        /// <param name="v"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Vector2D operator *(Vector2D v, double k)
        {
            return new Vector2D(v.X * k, v.Y * k);
        }
        /// <summary>
        /// 向量乘
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2D operator *(double k, Vector2D v)
        {
            return new Vector2D(v.X * k, v.Y * k);
        }
        /// <summary>
        /// 向量除
        /// </summary>
        /// <param name="v"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2D operator /(Vector2D v, double value)
        {
            if (Math.Abs(value) < Extension.SMALL_NUMBER)
                throw new DivideByZeroException();
            return new Vector2D(v.X / value, v.Y / value);
        }

        /// <summary>
        /// 判断两个向量是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public bool IsAlmostEqualTo(Vector2D other, double tolerance)
        {
            Vector2D v = other;
            if (v == null) return false;
            if (Math.Abs(x - v.X) < tolerance && Math.Abs(y - v.Y) < tolerance)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断两个向量是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAlmostEqualTo(Vector2D other)
        {
            return IsAlmostEqualTo(other, Extension.SMALL_NUMBER);
        }

        /// <summary>
        /// 向量的夹角，值域为[0,π]
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public double AngleTo(Vector2D vector)
        {
            if (!(Math.Abs(Modulus() * vector.Modulus()) < Extension.SMALL_NUMBER))
            {
                double result = Dot(vector) / (Modulus() * vector.Modulus());
                double r = result > 1 ? 1 : result;
                double rr = r < -1 ? -1 : r;
                return Math.Acos(rr);
            }
            return 0;
        }

        /// <summary>
        /// 向量source逆时针旋转到终点向量的角度，值域为[0 ,2π)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public double AngleFrom(Vector2D source)
        {
            double angle = AngleTo(source);
            if (Math.Abs(angle) < Extension.SMALL_NUMBER)
                return 0;
            if (Cross(source) > 0)
                return 2 * Math.PI - angle;
            return angle;
        }

        /// <summary>
        /// 向量所在直线的夹角，值域为[0,π/2]
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public double AngleWith(Vector2D vector)
        {
            return AngleTo(vector) < Math.PI / 2 ? AngleTo(vector) : Math.PI - AngleTo(vector);
        }


        /// <summary>
        /// 向量的偏移，偏移出一个新的向量
        /// </summary>
        /// <param name="vector2D"></param>
        /// <returns></returns>
        public Vector2D Offset(Vector2D v)
        {
            return this + v;
        }

        /// <summary>
        /// 向量的偏移，偏移出指定的长度
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector2D Offset(double offset, Vector2D direction)
        {
            Vector2D v = this.Offset(offset * direction);
            return v;
        }

        /// <summary>
        /// 将当前点移动到指定位置信息
        /// </summary>
        /// <param name="v"></param>
        public void MoveTo(Vector2D v)
        {
            Vector2D nv = Offset(v);
            this.x = nv.x;
            this.y = nv.y;
        }

        /// <summary>
        /// 将当前点移动到指定的位置信息
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="direction"></param>
        public void MoveTo(double offset, Vector2D direction)
        {
            Vector2D nv = this.Offset(offset * direction);
            this.x = nv.x;
            this.y = nv.y;
        }


        public void Rotate(Vector2D c,double angle){
        
        }
    }


    /// <summary>
    /// 用于比较两个Vector3D是否相同的位置,集合中使用
    /// </summary>
    public class Vector2DEqualityComparer : IEqualityComparer<Vector2D>
    {
        public bool Equals(Vector2D v1, Vector2D v2)
        {
            return v1.IsAlmostEqualTo(v2);
        }
        public int GetHashCode(Vector2D obj)
        {
            return 0;
        }

    }
}