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
    /// 当前的线图形
    /// </summary>
    public class MeasureGeometry: Geometry2D
    {

        /// <summary>
        /// 构造函数，初始化一个直线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public MeasureGeometry(Vector2D start, Vector2D end)
        {

            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// 通过ID构造当前图形
        /// </summary>
        /// <param name="geometryId"></param>
        public MeasureGeometry(string geometryId) : base(geometryId)
        {

        }
        /// <summary>
        /// 初始化当前线绘制
        /// </summary>
        public MeasureGeometry() { }

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
                this.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);
                this.PenColor = Colors.DeepSkyBlue;
                List<Vector2D> points = new List<Vector2D>();
                points.Add(start);
                points.Add(end);
                this.Draw(points);
            }
        }
        /// <summary>
        /// 按照点，绘制连续的线段，且没有填充
        /// </summary>
        /// <param name="points"></param>
        /// <param name="color"></param>
        /// <param name="thinkness"></param>
        /// <returns></returns>
        protected override void Draw(List<Vector2D> points)
        {
            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();  //冻结画笔，这样能加快绘图速度
            for (int i = 0; i < points.Count - 1; i++)
            {

                dc.DrawLine(Pen, KernelProperty.MMToPix(points[i]), KernelProperty.MMToPix(points[i + 1]));
            }
            var line = Line2D.Create(start, end);
            this.DrawArrow(dc, line);
            SeText(dc, line);
            dc.Close();
        }
        /// <summary>
        /// 绘制相关的文字信息
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="startToLine"></param>
        private void SeText(DrawingContext dc, Line2D startToLine)
        {
            string text = ((int)Math.Round(startToLine.Length)).ToString();
            Vector2D middle = startToLine.MiddlePoint;

            Vector2D dir = startToLine.Direction;
            var rad = dir.AngleFrom(Vector2D.BasisX);

            var angle = Extension.RadToDeg(rad);
            if (angle > 90 && angle < 270)
            {
                angle = angle + 180;
            }

            var md = KernelProperty.MMToPix(middle);
            RotateTransform rt = new RotateTransform();
            rt.Angle = angle;
            rt.CenterX = md.X;
            rt.CenterY = md.Y;
            dc.PushTransform(rt);
            FormattedText ft = new FormattedText(text, new System.Globalization.CultureInfo(0x0804, false), System.Windows.FlowDirection.LeftToRight, new Typeface("微软雅黑"), 14, Brushes.Blue);
            dc.DrawText(ft, md);
            dc.Pop();
   

        }

        /// <summary>
        /// 当前的线的移动
        /// </summary>
        /// <param name="v"></param>
        public override void Move(Vector2D v) {

            this.start.MoveTo(v);
            this.end.MoveTo(v);
            this.Update();
        }
        /// <summary>
        /// 当前线的缩放
        /// </summary>
        /// <param name="zm"></param>
        public override void Scale(double zm) {

            this.start = this.start * zm;
            this.end = this.end * zm;
            this.Update();
        }
        /// <summary>
        /// 当前线的旋转
        /// </summary>
        /// <param name="c"></param>
        /// <param name="angle"></param>
        public override void Rolate(Vector2D c, double angle) {

            this.start.Rotate(c, angle);
            this.end.Rotate(c, angle);
            this.Update();
        }
        /// <summary>
        /// 当前线的拷贝
        /// </summary>
        /// <param name="v"></param>
        public override Geometry2D Copy(bool isclone) {

            var nstart = new Vector2D(this.start.X, this.start.Y);
            var nend = new Vector2D(this.end.X, this.end.Y);
            MeasureGeometry nline = new MeasureGeometry(nstart, nend);
            if (isclone)
            {
   
            }
            else
            {
                nline.Element = this.Element;
            }
            return nline;
        }

        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is MeasureGeometry)
            {
                MeasureGeometry measure = (target as MeasureGeometry);

                this.start = TransformUtil.Mirror(measure.start, mirrorLine);
                this.end = TransformUtil.Mirror(measure.end, mirrorLine);
            }

        }
        /// <summary>
        /// 写入XML信息
        /// </summary>
        /// <param name="sw"></param>
        public override void WriteXML(XmlWriter sw)
        {
            sw.WriteStartElement("MeasureGeometry");
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
