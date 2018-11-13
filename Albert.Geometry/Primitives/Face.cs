using System;
using System.Collections.Generic;
using System.Linq;
using Albert.Geometry.External;

namespace Albert.Geometry.Primitives
{
    /// <summary>
    /// 代表三维中的一个无限面
    /// </summary>
    /// 
    public class Face
    {
        /// <summary>
        /// 面的参数A
        /// </summary>
        internal double A
        {
            get;
            private set;
        }
        /// <summary>
        /// 面的参数B
        /// </summary>
        internal double B
        {
            get;
            private set;
        }

        /// <summary>
        /// 面的参数C
        /// </summary>
        internal double C
        {
            get;
            private set;
        }

        /// <summary>
        /// 面的参数D
        /// </summary>
        internal double D
        {
            get;
            private set;
        }


        private Vector3D origin;

        /// <summary>
        /// 当前面上一个特征点，可以是面上面的任意一点
        /// </summary>
        public Vector3D Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }


        private Vector3D normal;

        /// <summary>
        /// 面的法线方向
        /// </summary>
        public Vector3D Normal
        {
            get
            {
                return normal;
            }
            set
            {
                normal = value;
            }
        }

        /// <summary>
        /// 初始化一个面
        /// </summary>
        public Face()
        {

            this.origin = Vector3D.Zero;
            this.normal = Vector3D.BasisZ;
            C = 1;
        }

        /// <summary>
        /// 构造一个面
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="normal"></param>
        public Face(Vector3D origin, Vector3D normal)
        {

            this.origin = origin.Normalize();
            this.normal = normal.Normalize();
            A = this.normal.X;
            B = this.normal.Y;
            C = this.normal.Z;
            D = -(A * origin.X + B * origin.Y + C * origin.Z);
        }




        /// <summary>
        /// 创建一个面对象
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static Face Create(Vector3D origin, Vector3D normal)
        {
            Face face = new Face();
            face.origin = origin.Normalize();
            face.normal = normal.Normalize();
            face.A = face.normal.X;
            face.B = face.normal.Y;
            face.C = face.normal.Z;
            face.D = -(face.A * face.origin.X + face.B * face.origin.Y + face.C * face.origin.Z);
            return face;
        }
        /// <summary>
        /// 通过三点确定一个面
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        public static Face Create(Vector3D v1, Vector3D v2, Vector3D v3)
        {
            double x1 = v1.X;
            double y1 = v1.Y;
            double z1 = v1.Z;
            double x2 = v2.X;
            double y2 = v2.Y;
            double z2 = v2.Z;
            double x3 = v3.X;
            double y3 = v3.Y;
            double z3 = v3.Z;
            Face face = new Face();
            face.A = (y2 - y1) * (z3 - z1) - (y3 - y1) * (z2 - z1);
            face.B = (z2 - z1) * (x3 - x1) - (z3 - z1) * (x2 - x1);
            face.C = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);
            face.D = -(face.A * x1 + face.B * y1 + face.C * z1);
            face.normal = Vector3D.Create(face.A, face.B, face.C);
            face.Origin = v1;
            return face;
        }


        /// <summary>
        /// 定义一个面
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static Face Create(List<Line3D> lines)
        {
            Line3D l1 = null;
            Line3D l2 = null;
            Vector3D instert = null;
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = i; j < lines.Count; j++)
                {

                    instert = lines[i].Intersect(lines[j]);
                    if (instert != null)
                    {

                        l1 = lines[i];
                        l2 = lines[j];
                        break;
                    }
                }
                if (instert != null)
                    break;

            }

            if (l1 == null || l2 == null)
            {

                return null;
            }
            else
            {

                //当前面对应的方向
                var face = l1.Direction.Cross(l2.Direction);
                var f = Face.Create(instert, face);
                return f;
            }



        }
        /// <summary>
        /// 判断两个面是否平行
        /// </summary>
        /// <param name="source"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsParallelWith(Face face)
        {
            if (this.normal.IsAlmostEqualTo(face.normal) || this.normal.IsAlmostEqualTo(-face.normal))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断两个面是否共面
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool IsCoplanarity(Face source)
        {

            if (IsParallelWith(source) && D.AreEqual(source.D))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 面和面相交获得一条直线
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Line3D IntersectUnlimitedFace(Face source)
        {
            Vector3D origin1 = this.Origin;
            Vector3D normal1 = this.Normal;//n1
            Vector3D origin2 = source.Origin;
            Vector3D normal2 = source.Normal;//n2

            Vector3D direction = normal1.Cross(normal2).Normalize();
            if (direction.Modulus().AreEqual(0))
            {
                //无交点
                return null;
            }

            double s1, s2, a, b;
            s1 = normal1.Dot(origin1);
            s2 = normal2.Dot(origin2);
            double n1n2dot = normal1.Dot(normal2);
            double n1normsqr = normal1.Dot(normal1);
            double n2normsqr = normal2.Dot(normal2);
            a = (s2 * n1n2dot - s1 * n2normsqr) / (Math.Pow(n1n2dot, 2) - n1normsqr * n2normsqr);
            b = (s1 * n1n2dot - s2 * n1normsqr) / (Math.Pow(n1n2dot, 2) - n1normsqr * n2normsqr);
            Vector3D point = a * normal1 + b * normal2;
            Line3D intersectLine = Line3D.CreateUnbond(point, direction);
            return intersectLine;
        }
    }
}
