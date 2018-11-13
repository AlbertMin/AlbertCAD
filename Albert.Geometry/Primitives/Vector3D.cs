using System;
using System.Collections.Generic;
/// <summary>
/// 当前用于定义一个三维图形点，用于处理空间坐标信息
/// </summary>
namespace Albert.Geometry.Primitives
{

    /// <summary>
    /// 一个空间三维点
    /// </summary>
    public class Vector3D
    {


        /// <summary>
        /// 当前定义的几个固定属性
        /// </summary>
        public static Vector3D Zero
        {
            get
            {
                return new Vector3D(0, 0, 0);
            }
        }
        /// <summary>
        /// 当前的X轴
        /// </summary>
        public static Vector3D BasisX
        {
            get
            {
                return new Vector3D(1, 0, 0);
            }
        }
        /// <summary>
        /// 当前的Y轴
        /// </summary>
        public static Vector3D BasisY
        {
            get
            {
                return new Vector3D(0, 1, 0);
            }
        }


        /// <summary>
        /// 当前的Z轴
        /// </summary>
        public static Vector3D BasisZ
        {
            get
            {
                return new Vector3D(0, 0, 1);
            }
        }

        /// <summary>
        /// 构造函数，初始化一个三维向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// 构造函数，初始化一个三维向量
        /// </summary>
        /// <param name="source"></param>
        public Vector3D(Vector3D source)
        {
            this.x = source.X;
            this.y = source.Y;
            this.z = source.Z;
        }
        /// <summary>
        /// 新建一个零向量
        /// </summary>
        public Vector3D()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }




        /// <summary>
        /// 构造器，创建一个三维向量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3D Create(double x, double y, double z)
        {
            return new Vector3D(x, y, z);
        }


        private double x;
        /// <summary>
        /// 向量的X
        /// </summary>
        /// 

        public double X
        {
            get { return x; }
            set { x = value; }
        }
        private double y;
        /// <summary>
        /// 向量的Y
        /// </summary>

        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        private double z;
        /// <summary>
        /// 向量的Z
        /// </summary>

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        ///<summary>
        /// 向量的模
        /// </summary>
        /// <returns></returns>
        public double Modulus()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
        }

        /// <summary>
        /// 归一化
        /// </summary>
        /// <returns></returns>
        public Vector3D Normalize()
        {
            double mod = Modulus();
            if (mod < Extension.SMALL_NUMBER)
                return new Vector3D(0, 0, 0);
            return new Vector3D(X / mod, Y / mod, Z / mod);
        }

        /// <summary>
        /// 向量到向量之间的距离
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public double Distance(Vector3D source)
        {
            return Math.Sqrt(Math.Pow(X - source.X, 2) + Math.Pow(Y - source.Y, 2) + Math.Pow(Z - source.Z, 2));
        }
        /// <summary>
        /// 向量的点积
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public double Dot(Vector3D source)
        {
            return (X * source.X + Y * source.Y + Z * source.Z);
        }

        /// <summary>
        /// 向量的叉积
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Vector3D Cross(Vector3D source)
        {
            return new Vector3D(Y * source.Z - Z * source.Y, Z * source.X - X * source.Z, X * source.Y - Y * source.X);
        }

        /// <summary>
        /// 向量的减
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3D operator -(Vector3D v)
        {
            return new Vector3D(-v.X, -v.Y, -v.Z);
        }
        /// <summary>
        /// 向量的减
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        /// <summary>
        /// 向量的加
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v2.X + v1.X, v2.Y + v1.Y, v2.Z + v1.Z);
        }
        /// <summary>
        /// 向量的乘
        /// </summary>
        /// <param name="v"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Vector3D operator *(Vector3D v, double k)
        {
            return new Vector3D(v.X * k, v.Y * k, v.Z * k);
        }
        /// <summary>
        /// 向量的乘
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3D operator *(double k, Vector3D v)
        {
            return new Vector3D(v.X * k, v.Y * k, v.Z * k);
        }

        /// <summary>
        /// 向量的除
        /// </summary>
        /// <param name="v"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3D operator /(Vector3D v, double value)
        {
            if (Math.Abs(value) < Extension.SMALL_NUMBER)
                throw new DivideByZeroException();
            return new Vector3D(v.X / value, v.Y / value, v.Z / value);
        }

        /// <summary>
        /// 按向量vector进行偏移
        /// </summary>
        /// <param name="vector">偏移向量</param>
        /// <returns></returns>
        public Vector3D Offset(Vector3D v)
        {
            Vector3D point = this + v;
            return point;

        }
        /// <summary>
        /// 沿direction方向对其偏移offset的距离
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector3D Offset(double offset, Vector3D direction)
        {
            Vector3D point = this + direction.Normalize() * offset;
            return new Vector3D(point.x, point.y, point.z);
        }
        /// <summary>
        /// 移动自身到指定距离
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="direction"></param>
        public void MoveTo(Vector3D v)
        {
            var nv = (this + v);
            this.X = nv.X;
            this.Y = nv.Y;
            this.Z = nv.Z;
        }

        /// <summary>
        /// 移动到指定的距离
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="direction"></param>
        public void MoveTo(double offset, Vector3D direction)
        {
            this.X = (this + direction.Normalize() * offset).X;
            this.Y = (this + direction.Normalize() * offset).Y;
            this.Z = (this + direction.Normalize() * offset).Z;
        }



    }

    /// <summary>
    /// 用于比较两个Vector3D是否相同的位置
    /// </summary>
    public class Vector3DEqualityComparer : IEqualityComparer<Vector3D>
    {
        public bool Equals(Vector3D v1, Vector3D v2)
        {
            return v1.IsAlmostEqualTo(v2);
        }
        public int GetHashCode(Vector3D obj)
        {
            return 0;
        }

    }

    /// <summary>
    /// 用于比较两个Vector3D的x是否相同的位置
    /// </summary>
    public class Vector3DXEqualityComparer : IEqualityComparer<Vector3D>
    {
        public bool Equals(Vector3D v1, Vector3D v2)
        {
            return Math.Abs(v1.X - v2.X) < 1e-6;
        }
        public int GetHashCode(Vector3D obj)
        {
            return 0;
        }

    }
}
