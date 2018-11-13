using Albert.Geometry.Primitives;
using System.Collections.Generic;
using Albert.Geometry.External;
using System;

namespace Albert.Geometry.Transform
{
    /// <summary>
    /// 当前类的主要用途：将空间中任意一个面，转换到XY平面上
    /// </summary>
    public class TransformUtil
    {


        /// <summary>
        /// 转换向量
        /// </summary>
        /// <param name="transMatrix"></param>
        /// <param name="direction">方向向量</param>
        public static Vector3D TransformDirection(Matrix4 transMatrix, Vector3D direction)
        {
            if (transMatrix == null || direction == null)
                return null;

            Vector3D start = Vector3D.Zero;
            Vector3D end = direction;

            //开始转换
            Vector3D newStart = transMatrix * start;
            Vector3D newEnd = transMatrix * end;

            Vector3D newDirection = (newEnd - newStart).Normalize();
            return newDirection;
        }

 


        /// <summary>
        /// 将三维坐标，直接平移为平面二维坐标
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2D Projection(Vector3D v)
        {
            return new Vector2D(v.X, v.Y);
        }
        /// <summary>
        /// 将三维坐标，直接投影为到其他平面
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3D Transform(Vector3D v,Matrix4 m)
        {
            Vector3D nv = m * v;
            return nv;
        }


        /// <summary>
        /// 直接将二维坐标转换到指定的空间坐标
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3D Projection(Vector2D v, double z)
        {
            return new Vector3D(v.X, v.Y, z);
        }
        /// <summary>
        /// 将三维线直接投影为二维线段
        /// </summary>
        /// <param name="line3d"></param>
        /// <returns></returns>
        public static Line2D Projection(Line3D line3d)
        {
            Line2D line2d = Line2D.Create(Projection(line3d.Start), Projection(line3d.End));
            return line2d;
        }


        /// <summary>
        /// 将三维线直接投影为二维线段
        /// </summary>
        /// <param name="line3d"></param>
        /// <returns></returns>
        public static List<Line2D> Projection(List<Line3D> line3ds)
        {
            List<Line2D> line2ds = new List<Line2D>();
            line3ds.ForEach(x => {
                line2ds.Add(Line2D.Create(Projection(x.Start), Projection(x.End)));
            });
            return line2ds;
        }

        /// <summary>
        /// 将三维还原到二维平面中
        /// </summary>
        /// <param name="line2ds"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static List<Line3D> Projection(List<Line2D> line2ds,double z)
        {
            List<Line3D> line3ds = new List<Line3D>();
            line2ds.ForEach(x => {
                line3ds.Add(Line3D.Create(Projection(x.Start,z), Projection(x.End,z)));
            });
            return line3ds;
        }
        /// <summary>
        /// 进行投影转换操作
        /// </summary>
        /// <param name="line3D"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Line3D Transform(Line3D line3D, Matrix4 m)
        {
            Line3D nline3D = Line3D.Create(m * line3D.Start, m * line3D.End);
            return nline3D;
        }


        /// <summary>
        /// 坐标系转换
        /// </summary>
        /// <param name="transMatrix">转换矩阵</param>
        /// <param name="line3Ds">待转换的线</param>
        public static List<Line3D> Transform(List<Line3D> line3ds, Matrix4 m)
        {
            List<Line3D> nline3ds = new List<Line3D>();
            line3ds.ForEach(x =>
            {
                nline3ds.Add(Transform(x, m));
            });
            return nline3ds;
        }

        /// <summary>
        /// 将二维线段，向上偏移到三维空间
        /// </summary>
        /// <param name="line2d"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Line3D Projection(Line2D line2d, double z)
        {
            Line3D line3d = Line3D.Create(Projection(line2d.Start, z), Projection(line2d.End, z));
            return line3d;
        }
   
        /// <summary>
        /// 获取一个点的镜像点
        /// </summary>
        /// <param name="v"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static Vector2D Mirror(Vector2D v, Line2D mirror)
        {
            Vector2D p = v.ProjectOn(mirror);
            Line2D le = Line2D.Create(v, p);
            Vector2D re = p.Offset(le.Direction * le.Length);
            return re;
        }

        /// <summary>
        /// 通过一个点，和点上三个对应的三个互相垂直的线，获取一个转换矩阵
        /// </summary>
        /// <returns></returns>
        public static Matrix4 GetMatrix(Vector3D origin, Vector3D axisX, Vector3D axisY, Vector3D axisZ) {

            Matrix4 result = null;

            Vector3D AxisX = axisX.Normalize();
            Vector3D AxisY = axisY.Normalize();
            Vector3D AxisZ = axisZ.Normalize();

            //生成转换矩阵
            Matrix4 m1 = Matrix4.Create(
                1, 0, 0, -origin.X,
                0, 1, 0, -origin.Y,
                0, 0, 1, -origin.Z,
                0, 0, 0, 1
                );

            Matrix4 m2 = Matrix4.Create(
                axisX.X, axisX.Y, axisX.Z, 0,
                axisY.X, axisY.Y, axisY.Z, 0,
                axisZ.X, axisZ.Y, axisZ.Z, 0,
                0, 0, 0, 1
                );

            result = m2 * m1;

            return result;
        }

        /// <summary>
        /// 通过直线获取转换矩阵
        /// </summary>
        /// <param name="searchLines"></param>
        /// <returns></returns>
        public static Matrix4 GetMatrix(List<Line3D> line3ds)
        {
            var origin = line3ds[0].Start;
            //先获取这个面的法线
            Vector3D axisZ = line3ds.GetNormal().Normalize();
            //把当前点所在的直线作为X轴
            Vector3D axisX = line3ds[0].Direction.Normalize();
            //计算出Y轴的向量
            Vector3D axisY = axisX.Cross(axisZ).Normalize();
            //返回转换后的逆矩阵
            return GetMatrix(origin, axisX, axisY, axisZ);
        }


        /// <summary>
        /// 获取一个矩阵的逆矩阵
        /// </summary>
        public static Matrix4 GetInversetMatrix(Matrix4 m)
        {
            Matrix4 cm = m.Clone();
            return cm.SetInverseOf(cm);
        }


 
    }
}
