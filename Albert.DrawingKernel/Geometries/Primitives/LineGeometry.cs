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
using System.Windows.Media.Effects;
using System.Xml;
using Albert.Geometry.Transform;

namespace Albert.DrawingKernel.Geometries.Primitives
{
    /// <summary>
    /// 当前的直线图形
    /// </summary>
    public class LineGeometry : Geometry2D
    {

        /// <summary>
        /// 构造函数，初始化一个直线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public LineGeometry(Vector2D start, Vector2D end)
        {

            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// 通过ID构造当前图形
        /// </summary>
        /// <param name="geometryId"></param>
        public LineGeometry(string geometryId) : base(geometryId)
        {


        }
        /// <summary>
        /// 初始化当前线绘制
        /// </summary>
        public LineGeometry() { }



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
        /// 当前直线的终点坐标
        /// </summary>
        public Vector2D End
        {
            get { return end; }
            set { end = value; }
        }



        /// <summary>
        /// 获取所有的点
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                List<Vector2D> points = new List<Vector2D>();
                if (start != null)
                {
                    points.Add(Start);
                }
                if (end != null)
                {
                    points.Add(End);
                }

                return points;
            }
        }


        /// <summary>
        /// 返回当前的线段信息
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {
                List<Line2D> lines = new List<Line2D>();
                if (start != null && End != null)
                {
                    var l = Line2D.Create(Start, End);
                    lines.Add(l);
                }
                return lines;
            }

        }


        /// <summary>
        /// 判断和当前图形是否有相交
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override IntersectPoint IntersectPoint(Vector2D v)
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
        /// 开始绘制
        /// </summary>
        /// <param name="ms"></param>
        public override void Update()
        {
            if (start != null && End != null)
            {
                List<Vector2D> points = new List<Vector2D>();
                points.Add(start);
                points.Add(end);
                this.Draw(points);
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

            var nstart = new Vector2D(this.start.X, this.start.Y);
            var nend = new Vector2D(this.end.X, this.end.Y);
            LineGeometry nline = new LineGeometry(nstart, nend);
            nline.PenColor = this.PenColor;
            if (isclone)
            {
       
            }
            else
            {
                nline.Element = this.Element;
            }
            return nline;
        }

        /// <summary>
        /// 对当前图形进行镜像处理
        /// </summary>
        /// <param name="mirrorLine"></param>
        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is LineGeometry)
            {
                LineGeometry line = (target as LineGeometry);
                this.start = TransformUtil.Mirror(line.start, mirrorLine);
                this.end = TransformUtil.Mirror(line.end, mirrorLine);
            }

        }


        /// <summary>
        /// 写入XML信息
        /// </summary>
        /// <param name="sw"></param>
        public override void WriteXML(XmlWriter sw)
        {
            sw.WriteStartElement("LineGeometry");
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
