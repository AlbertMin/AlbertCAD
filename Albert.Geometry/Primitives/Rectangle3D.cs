using System;
namespace Albert.Geometry.Primitives
{

    /// <summary>
    /// 代表一个有限的矩形面
    /// </summary>
    /// 
    public class Rectangle3D : Face
    {

        /// <summary>
        /// 矩形的底边
        /// </summary>
        public Line3D Bottom { get; set; }

        /// <summary>
        /// 矩形的顶部
        /// </summary>
        public Line3D Top { get; set; }

        /// <summary>
        /// 矩形的右边
        /// </summary>
        public Line3D Right { get; set; }

        /// <summary>
        /// 矩形的左边
        /// </summary>
        public Line3D Left { get; set; }



    }
}
