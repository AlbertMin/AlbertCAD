using System.Collections.Generic;
using System.Linq;
using Albert.Geometry.External;

namespace Albert.Geometry.Primitives
{
    /// <summary>
    /// 定义了一个三维的多边形
    /// </summary>
    public class Polygon3D : Face
    {
        /// <summary>
        /// 顶点
        /// </summary>

        public List<Vector3D> Vertexes { get { return GetVertexes(); } set { } }
        /// <summary>
        /// 面积
        /// </summary>

        public double Area { get { return GetArea(); } set { } }

        /// <summary>
        /// 形心
        /// </summary>


        public Vector3D Centroid { get { return GetCentroid(); } set { } }
        /// <summary>
        /// 轮廓线
        /// </summary>

        public List<Line3D> Edges { get; set; }

        /// <summary>
        /// 法向量
        /// </summary>

        public Vector3D Normal { get { return GetNormal(); } set { } }

        /// <summary>
        /// 转换矩阵算法
        /// </summary>

        //    private GlobalToLocalCoordinateAlgorithm tAlgorithm { get { return GetTransform(); } set { } }

        private Polygon2D Plane { get { return ProjectToPlane(); } set { } }
        /// <summary>
        /// lines集合必须为一个平面内的
        /// </summary>
        /// <param name="lines"></param>
        public Polygon3D(List<Line3D> lines)
        {
            List<Line3D> newLines = new List<Line3D>(lines);
            Edges = newLines;
        }

        private List<Vector3D> GetVertexes()
        {
            return Edges.Select(t => t.Start).ToList();
        }

        private Polygon2D ProjectToPlane()
        {
            List<Line2D> lines = Edges.Select(edge => new Line2D(new Vector2D(edge.Start.X, edge.Start.Y), new Vector2D(edge.End.X, edge.End.Y))).ToList();
            return new Polygon2D(lines);
        }

        private double GetArea()
        {
            return Plane.Area;
        }

        private Vector3D GetCentroid()
        {
            Vector3D v = new Vector3D(Plane.Centroid.X, Plane.Centroid.Y, 0);
            // return tAlgorithm.Transform(v, tAlgorithm.GetRETransformMatrix());
            return null;
        }

        private Vector3D GetNormal()
        {
            return Edges[0].Direction.Cross(Edges[1].Direction).Normalize();
        }


    }
}
