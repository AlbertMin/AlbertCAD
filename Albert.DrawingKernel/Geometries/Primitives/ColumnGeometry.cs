using Albert.Geometry.Primitives;
using Albert.Geometry.External;
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
    /// 绘制圆的图形
    /// </summary>
    public class ColumnGeometry : Geometry2D
    {

        private Vector2D start = null;
        /// <summary>
        /// 绘制的起始点
        /// </summary>
        public Vector2D Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        private Vector2D end = null;
        /// <summary>
        /// 绘制的起始点
        /// </summary>
        public Vector2D End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }
        public ColumnGeometry()
            : base()
        {

        }
        /// <summary>
        /// 构造函数，初始化当前圆形
        /// </summary>
        /// <param name="GeometryId"></param>
        public ColumnGeometry(string GeometryId) : base(GeometryId) { }

        /// <summary>
        /// 构造函数，初始化当前的圆形图形
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public ColumnGeometry(Vector2D start, Vector2D end) {

            this.start = start;
            this.end = end;
        }
        /// <summary>
        /// 获取当前圆形的半径
        /// </summary>
        public double Radius
        {

            get
            {
                if (end != null && start != null)
                {
                    return End.Distance(start);
                }
                else {
                    return 0;
                }
              
            }
        }

        /// <summary>
        /// 更新当前图形
        /// </summary>
        /// <param name="ms"></param>
        public override void Update()
        {
            if (Start != null&&End!=null)
            {
                if (this.ColumnType == 0)
                {
                    var Radius = End.Distance(Start);
                    this.DrawArcFill(Start, Radius, Radius);
                }
                else {

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
        }

        /// <summary>
        /// 获取当前所有的顶点集合
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                if (Start != null)
                {
                    List<Vector2D> points = new List<Vector2D>();
                    var v1 = Start.Offset(Radius, Vector2D.BasisX);
                    var v2 = Start.Offset(Radius, -Vector2D.BasisX);
                    var v3 = Start.Offset(Radius, Vector2D.BasisY);
                    var v4 = Start.Offset(Radius, -Vector2D.BasisY);
                    points.Add(Start);
                    points.Add(v1);
                    points.Add(v2);
                    points.Add(v3);
                    points.Add(v4);
                    return points;
                }
                return null;
            }
        }

        /// <summary>
        /// 获取当前圆形上所有的线段
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {
                return null;
            }
        }

        public int ColumnType { get; internal set; }

        /// <summary>
        /// 捕获圆形上的点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override IntersectPoint IntersectPoint(Vector2D v)
        {
            if (Math.Abs(v.Distance(this.Start) - this.Radius) < KernelProperty.PixelToSize)
            {
                Line2D l = Line2D.Create(this.Start, v);
                Vector2D nv = this.Start.Offset(this.Radius, l.Direction);
                IntersectPoint ip = new IntersectPoint();
                ip.IntersectPointStyle = 1;
                ip.Line = l;
                ip.Point = nv;
                return ip;
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
            var nstart = Vector2D.Create(this.start.X,this.start.Y);
            var nend = Vector2D.Create(this.end.X, this.end.Y);
            CircleGeometry circleGeometry = new CircleGeometry(nstart, nend);
            if (isclone)
            {
   
            }
            else
            {
                circleGeometry.Element = this.Element;
            }
            circleGeometry.PenColor = this.PenColor;
            circleGeometry.FillColor = this.FillColor;
            return circleGeometry;
        }


        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is ColumnGeometry)
            {
                ColumnGeometry column = (target as ColumnGeometry);
                this.start = TransformUtil.Mirror(column.start, mirrorLine);
                this.end = TransformUtil.Mirror(column.end, mirrorLine);
            }

        }
        /// <summary>
        /// 写入XML信息
        /// </summary>
        /// <param name="sw"></param>
        public override void WriteXML(XmlWriter sw)
        {
            sw.WriteStartElement("CircleGeometry");
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
