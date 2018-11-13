using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.Geometry.Primitives
{

    /// <summary>
    /// 当前定义了一个圆形
    /// </summary>
    public class Circle2D
    {
        /// <summary>
        /// 当前的圆形中心
        /// </summary>
        public Vector2D Central
        {

            get; set;
        }

        /// <summary>
        /// 当前圆形的半径
        /// </summary>
        public double Radius
        {

            get; set;
        }
        /// <summary>
        /// 初始化一个圆形
        /// </summary>
        public Circle2D()
        {

            this.Central = Vector2D.Zero;
            this.Radius = 0;
        }
        /// <summary>
        /// 构造函数，初始化一个圆形
        /// </summary>
        /// <param name="central"></param>
        /// <param name="radius"></param>
        public Circle2D(Vector2D central, double radius) {

            this.Central = central;
            this.Radius = radius;
        }

        /// <summary>
        /// 构建一个圆形
        /// </summary>
        /// <param name="central"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public Circle2D Create(Vector2D central, double radius) {
            return new Circle2D(central, radius);
        }

        /// <summary>
        /// 创建一个圆形，通过两个坐标点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Circle2D Create(Vector2D start, Vector2D end) {
            var radius = end.Distance(start);
            return new Circle2D(start, radius);
        }
        /// <summary>
        /// 当前用于比较两个圆形是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAlmostEqualTo(Circle2D other) {

            if (Central.IsAlmostEqualTo(other.Central) && Radius.AreEqual(other.Radius)) {

                return true;
            }
            return false;
        }



        /// <summary>
        /// 两个圆形的相交
        /// </summary>
        /// <param name="circle2D"></param>
        /// <returns></returns>
        public List<Vector2D> Intersect(Circle2D source)
        {
            List<Vector2D> intersects = null;
            //假如两个圆形重合,或者一个圆在另外一个内部，则返回不会有交点
            if (this.IsAlmostEqualTo(source))
            {
                return intersects;
            }
            //获取圆点之间的距离
            var c_d = source.Central.Distance(source.Central);
            //获取半径的总长度
            var r_d = this.Radius + source.Radius;
            //获取两个半径的差值
            var r_ans = Math.Abs(this.Radius-source.Radius);
            //假如圆点之间的距离，要大于半径，则说明相离
            if (c_d.AreCompare(r_d) == 1)
            {
                return intersects;
            }
            //两个原点的距离要小于半径的差值，说明一定是包含
            else if (c_d.AreCompare(r_ans) == -1)
            {
                return intersects;
            }
            else {

                double a, b, c, p, q, r,cos,sin;

                a = 2 * this.Radius * (this.Central.X - source.Central.X);
                b = 2 * this.Radius * (this.Central.Y - source.Central.Y);
                c = Math.Pow(source.Radius, 2) - Math.Pow(this.Radius, 2) - Math.Pow(this.Central.X - source.Central.X, 2) - Math.Pow(this.Central.Y - source.Central.Y, 2);
                p = Math.Pow(a, 2) + Math.Pow(b, 2);
                q = -2 * a * c;
                r= Math.Pow(c, 2) - Math.Pow(b, 2);

                //说明是相切,那么只有一个解
                if (r_d.AreCompare(c_d) == 0 || c_d.AreCompare(r_ans) == 0)
                {

                    cos = -q / p / 2;
                    sin = Math.Sqrt(1 - Math.Pow(cos, 2));
                    Vector2D intersect = new Vector2D();
                    intersect.X = this.Radius * cos + this.Central.X;
                    intersect.Y = this.Radius * sin + this.Central.Y;
                    intersects = new List<Vector2D>();
                    //由于sin可能有正负两种情况，则先计算的交点是否正确
                    if (source.Central.Distance(intersect).AreCompare(source.Radius) == 0)
                    {
                        intersects.Add(intersect);
                    }
                    else
                    {
                        intersect.Y = -this.Radius * sin + this.Central.Y;
                        intersects.Add(intersect);
                    }
                    return intersects;
                }
                //说明两个多边形相交
                else {
                    intersects = new List<Vector2D>();
                    cos = (Math.Sqrt(Math.Pow(q, 2) - 4 * p * r) - q) / 2 * p;
                    sin = Math.Sqrt(1 - Math.Pow(cos, 2));
                    //第一个点
                    Vector2D intersect1 = new Vector2D();
                    intersect1.X = this.Radius * cos + this.Central.X;
                    intersect1.Y = this.Radius * sin + this.Central.Y;
                    if (source.Central.Distance(intersect1).AreCompare(source.Radius) == 0)
                    {
                        intersects.Add(intersect1);
                    }
                    else
                    {
                        intersect1.Y = -this.Radius * sin + this.Central.Y;
                        intersects.Add(intersect1);
                    }

                    cos = (-Math.Sqrt(Math.Pow(q, 2) - 4 * p * r) - q) / 2 * p;
                    sin = Math.Sqrt(1 - Math.Pow(cos, 2));
                    Vector2D intersect2 = new Vector2D();
                    intersect2.X = this.Radius * cos + this.Central.X;
                    intersect2.Y = this.Radius * sin + this.Central.Y;
                    if (source.Central.Distance(intersect2).AreCompare(source.Radius) == 0)
                    {
                        intersects.Add(intersect2);
                    }
                    else
                    {
                        intersect2.Y = -this.Radius * sin + this.Central.Y;
                        intersects.Add(intersect2);
                    }

                    //特殊情况，两个点计算到同一点，如圆形坐标对称，正好是互相反转关系，则需要对其中一个坐标进行调整
                    if (intersect1.IsAlmostEqualTo(intersect2))
                    {
                        intersect2.Y = -intersect2.Y;
                    }
                    return intersects;
                }
            }

        }

        /// <summary>
        /// 当前圆形是否是指定圆形的一部分
        /// </summary>
        /// <param name="circle2d"></param>
        /// <returns></returns>
        public bool InsideOf(Circle2D source)
        {
            var erd = (this.Central.Distance(source.Central) + this.Radius);
            if (erd < source.Radius || erd.AreEqual(source.Radius))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取当前圆形的周长
        /// </summary>
        public double Circumference {

            get {
                return 2 * Math.PI * Radius;
            }
        }

        /// <summary>
        /// 返回当前圆形的面积
        /// </summary>
        public double Area {

            get {
                return Math.PI *Math.Pow(Radius,2);
            }
        }





    }
}
