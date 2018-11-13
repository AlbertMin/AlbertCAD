using Albert.Geometry.External;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Selector;
using Albert.DrawingKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using Albert.Geometry.Transform;

namespace Albert.DrawingKernel.Geometries.Primitives
{

    /// <summary>
    /// 当前的矩形对象，初始化矩形元素
    /// </summary>
    public class RectangleGeometry : Geometry2D
    {

        public RectangleGeometry()
            : base()
        {

        }
        private Vector2D start = null;
        private Vector2D end = null;

        /// <summary>
        /// 当前的起点坐标
        /// </summary>
        public Vector2D Start
        {

            get { return start; }
            set { start = value; }
        }
        /// <summary>
        /// 当前的终点坐标
        /// </summary>
        public Vector2D End
        {
            get { return end; }
            set { end = value; }
        }

        /// <summary>
        /// 构造函数，初始化一个矩形图形
        /// </summary>
        public RectangleGeometry(string GeometryId)
            : base()
        {

        }
        /// <summary>
        /// 构造函数，初始化一个矩形图形
        /// </summary>
        public RectangleGeometry(Vector2D start, Vector2D end)
            : base()
        {
            this.start = start;
            this.end = end;

        }
        /// <summary>
        /// 获取当前多边形的顶点信息
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                List<Vector2D> points = new List<Vector2D>();
                if (start != null && end != null)
                {

                    Vector2D v1 = start;
                    Vector2D v2 = Vector2D.Create(end.X, start.Y);
                    Vector2D v3 = end;
                    Vector2D v4 = Vector2D.Create(start.X, end.Y);
                    points.Add(v1);
                    points.Add(v2);
                    points.Add(v3);
                    points.Add(v4);
                    return points;
                }
                return points;
            }
        }

        /// <summary>
        /// 获取当前图形上的所有直线
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {

                List<Line2D> lines = null;
                if (this.start != null && this.end != null)
                {
                    lines = new List<Line2D>();
                    List<Vector2D> points = new List<Vector2D>();
                    Vector2D v1 = start;
                    Vector2D v2 = Vector2D.Create(end.X, start.Y);
                    Vector2D v3 = end;
                    Vector2D v4 = Vector2D.Create(start.X, end.Y);
                    points.Add(v1);
                    points.Add(v2);
                    points.Add(v3);
                    points.Add(v4);
                    points.Add(v1);
                    for (int i = 1; i < points.Count; i++)
                    {
                        var l = Line2D.Create(points[i - 1], points[i]);
                        lines.Add(l);
                    }
                }

                return lines;
            }
        }


        /// <summary>
        /// 捕获当前图形上的点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override IntersectPoint IntersectPoint(Vector2D v)
        {

            if (Lines != null)
            {
                for (int i = 0; i < Lines.Count; i++)
                {
                    var len = Lines[i].Distance(v);
                    if (len < KernelProperty.Tolerance)
                    {
                        IntersectPoint ip = new IntersectPoint();

                        var nv = v.ProjectOn(Lines[i]);
                        ip.IntersectPointStyle = 1;
                        ip.Line = Lines[i];
                        ip.Point = nv;
                        return ip;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 不允许重合出现，所有假如绘制当前图形，则相交点总为空
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override IntersectPoint IntersectPoint2(Vector2D v)
        {
            return null;
        }



        /// <summary>
        /// 更新当前图形
        /// </summary>
        /// <param name="ms"></param>
        public override void Update()
        {
            if (Start != null && End != null)
            {
                List<Vector2D> points = new List<Vector2D>();
                var v1 = start;
                var v3 = end;
                var v2 = new Vector2D(v1.X, v3.Y);
                var v4 = new Vector2D(v3.X, v1.Y);
                points.Add(v1);
                points.Add(v2);
                points.Add(v3);
                points.Add(v4);
                points.Add(v1);
                this.DrawFill(points);
            }
        }


        /// <summary>
        /// 当前的线的移动
        /// </summary>
        /// <param name="v"></param>
        public override void Move(Vector2D v)
        {

            this.start.MoveTo(v);
            this.end.MoveTo(v);
            this.Update();
        }
        /// <summary>
        /// 当前线的缩放
        /// </summary>
        /// <param name="zm"></param>
        public override void Scale(double zm)
        {

            this.start = this.start * zm;
            this.end = this.end * zm;
            this.Update();
        }
        /// <summary>
        /// 当前线的旋转
        /// </summary>
        /// <param name="c"></param>
        /// <param name="angle"></param>
        public override void Rolate(Vector2D c, double angle)
        {

            this.start.Rotate(c, angle);
            this.end.Rotate(c, angle);
            this.Update();
        }
        /// <summary>
        /// 当前线的拷贝
        /// </summary>
        /// <param name="v"></param>
        public override Geometry2D Copy(bool isclone)
        {
            var nstart = Vector2D.Create(this.start.X, this.start.Y);
            var nend = Vector2D.Create(this.end.X, this.end.Y);
            RectangleGeometry rectangleGeometry = new RectangleGeometry(nstart, nend);
            if (isclone)
            {

            }
            else
            {
                rectangleGeometry.Element = this.Element;
            }
            rectangleGeometry.PenColor = this.PenColor;
            rectangleGeometry.FillColor = this.FillColor;
            return rectangleGeometry;
        }

        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is RectangleGeometry)
            {
                RectangleGeometry rectangle = (target as RectangleGeometry);

                this.start = TransformUtil.Mirror(rectangle.start, mirrorLine);
                this.end = TransformUtil.Mirror(rectangle.end, mirrorLine);
            }

        }
        /// <summary>
        /// 写入XML信息
        /// </summary>
        /// <param name="sw"></param>
        public override void WriteXML(XmlWriter sw)
        {
            sw.WriteStartElement("RectangleGeometry");
            sw.WriteAttributeString("Start", this.start.X + " " + this.start.Y);
            sw.WriteAttributeString("End", this.end.X + " " + this.end.Y);
            sw.WriteEndElement();
        }

        /// <summary>
        /// 读取XML信息
        /// </summary>
        /// <param name="xmlReader"></param>
        public override void ReadXML(XmlReader xmlReader)
        {
            string start = xmlReader.GetAttribute("Start");
            var ns = start.Split(' ');
            this.start = new Vector2D(double.Parse(ns[0]), double.Parse(ns[1]));

            string end = xmlReader.GetAttribute("End");
            var ne = end.Split(' ');
            this.end = new Vector2D(double.Parse(ne[0]), double.Parse(ne[1]));
        }
    }
}
