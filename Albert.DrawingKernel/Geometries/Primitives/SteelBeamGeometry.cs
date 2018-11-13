using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using Albert.Geometry.Transform;
using Albert.DrawingKernel.Selector;
using System.Windows.Media;
using Albert.DrawingKernel.Util;
using System.Xml;

namespace Albert.DrawingKernel.Geometries.Primitives
{
    public class SteelBeamGeometry : Geometry2D
    {
        protected Vector2D start;

        /// <summary>
        /// 墙的起点
        /// </summary>
        public Vector2D Start
        {

            get { return start; }
            set { start = value; }
        }
        protected Vector2D end;

        /// <summary>
        /// 墙的终端
        /// </summary>
        public Vector2D End
        {

            get { return end; }
            set { end = value; }
        }



       /// <summary>
       /// 初始化一个钢梁
       /// </summary>
       /// <param name="start"></param>
       /// <param name="end"></param>
       /// <param name="thickness">钢梁的界面</param>
        public SteelBeamGeometry(Vector2D start, Vector2D end, double thickness = 89, double supportThinkness = 100, double height=100)
        {
            this.start = start;
            this.end = end;
            this.Thickness = thickness;
            this.Height = height;
        }
        /// <summary>
        /// 构造函数，初始化一个直线
        /// </summary>
        public SteelBeamGeometry()
        {

        }
        /// <summary>
        /// 获取当前所有的顶点信息
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                List<Vector2D> points = new List<Vector2D>();

                if (Lines != null)
                {
                    Lines.ForEach(x =>
                    {

                        points.Add(x.Start);
                        points.Add(x.End);
                    });
                }

                //四边的交点
                return points;
            }
        }

        protected double thickness = 89;
        /// <summary>
        /// 当前墙体的厚度
        /// </summary>
        public double Thickness
        {
            get
            {
                return thickness;
            }
            set
            {
                thickness = value;
            }
        }

        /// <summary>
        /// 更新当前的绘制
        /// </summary>
        public override void Update()
        {

            this.Draw(null);
        }

        /// <summary>
        /// 获取当前钢梁的边界区域
        /// </summary>
        public List<Line2D> Boundary {

            get
            {
                List<Line2D> lines = new List<Line2D>();
                var halfThickness = thickness / 2;
                var start3D = new Albert.Geometry.Primitives.Vector3D(Start.X, Start.Y, 0);
                var end3D = new Albert.Geometry.Primitives.Vector3D(End.X, End.Y, 0);
                Vector3D lineDir = end3D - start3D;
                Vector3D offsetDir = lineDir.Cross(new Vector3D(0, 0, 1)).Normalize();
                Vector3D offv1 = start3D + halfThickness * offsetDir;
                Vector3D offv2 = start3D - halfThickness * offsetDir;
                Vector3D offv3 = end3D - halfThickness * offsetDir;
                Vector3D offv4 = end3D + halfThickness * offsetDir;
                Line2D l1 = Line2D.Create(TransformUtil.Projection(offv1), TransformUtil.Projection(offv2));
                Line2D l2 = Line2D.Create(TransformUtil.Projection(offv2), TransformUtil.Projection(offv3));
                Line2D l3 = Line2D.Create(TransformUtil.Projection(offv3), TransformUtil.Projection(offv4));
                Line2D l4 = Line2D.Create(TransformUtil.Projection(offv4), TransformUtil.Projection(offv1));
                lines.Add(l1);
                lines.Add(l2);
                lines.Add(l3);
                lines.Add(l4);

                return lines;
            }
        }

        /// <summary>
        /// 获取当前所有直线信息
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {
                List<Line2D> lines = new List<Line2D>();
                if (start != null && end != null)
                {
                    if (this.IsActioning)
                    {
                        Line2D l5 = Line2D.Create(start, end);
                        lines.Add(l5);
                    }
                    else
                    {
                        var halfThickness = thickness / 2;
                        var start3D = new Albert.Geometry.Primitives.Vector3D(Start.X, Start.Y, 0);
                        var end3D = new Albert.Geometry.Primitives.Vector3D(End.X, End.Y, 0);
                        Vector3D lineDir = end3D - start3D;
                        Vector3D offsetDir = lineDir.Cross(new Vector3D(0, 0, 1)).Normalize();
                        Vector3D offv1 = start3D + halfThickness * offsetDir;
                        Vector3D offv2 = start3D - halfThickness * offsetDir;
                        Vector3D offv3 = end3D - halfThickness * offsetDir;
                        Vector3D offv4 = end3D + halfThickness * offsetDir;
                        Line2D l1 = Line2D.Create(TransformUtil.Projection(offv1), TransformUtil.Projection(offv2));
                        Line2D l2 = Line2D.Create(TransformUtil.Projection(offv2), TransformUtil.Projection(offv3));
                        Line2D l3 = Line2D.Create(TransformUtil.Projection(offv3), TransformUtil.Projection(offv4));
                        Line2D l4 = Line2D.Create(TransformUtil.Projection(offv4), TransformUtil.Projection(offv1));
                        Line2D l5 = Line2D.Create(start, end);
                        lines.Add(l1);
                        lines.Add(l2);
                        lines.Add(l3);
                        lines.Add(l4);
                        lines.Add(l5);
                    }
                }
                return lines;
            }
        }
        /// <summary>
        /// 当前图形的着重线
        /// </summary>
        public override List<Line2D> Emphasize
        {

            get
            {
                List<Line2D> lines = new List<Line2D>();
                if (start != null && end != null)
                {
                    Line2D l5 = Line2D.Create(start, end);
                    lines.Add(l5);
                }
                return lines;
            }
        }
        /// <summary>
        /// 捕获当前图形信息
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
        /// 相交计算
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override IntersectPoint IntersectPoint2(Vector2D v)
        {
            return null;
        }

        /// <summary>
        /// 绘制当前的墙体
        /// </summary>
        /// <param name="points"></param>
        protected override void Draw(List<Vector2D> points)
        {
            if (start != null && end != null)
            {
                var halfThickness = Thickness / 2;

                List<Vector2D> fillPoints = new List<Vector2D>();
                var start3D = new Albert.Geometry.Primitives.Vector3D(Start.X, Start.Y, 0);
                var end3D = new Albert.Geometry.Primitives.Vector3D(End.X, End.Y, 0);

                Vector3D lineDir = end3D - start3D;

                Vector3D offsetDir = lineDir.Cross(new Vector3D(0, 0, 1)).Normalize();
                Vector3D offv1 = start3D + halfThickness * offsetDir;
                Vector3D offv2 = start3D - halfThickness * offsetDir;
                Vector3D offv3 = end3D - halfThickness * offsetDir;
                Vector3D offv4 = end3D + halfThickness * offsetDir;


                Line2D l5 = Line2D.Create(start, end);


                //钢梁
                fillPoints.Add(TransformUtil.Projection(offv1));
                fillPoints.Add(TransformUtil.Projection(offv2));
                fillPoints.Add(TransformUtil.Projection(offv3));
                fillPoints.Add(TransformUtil.Projection(offv4));
                fillPoints.Add(TransformUtil.Projection(offv4));


                DrawingContext dc = this.RenderOpen();
                Pen.Freeze();  //冻结画笔，这样能加快绘图速度
                PathGeometry paths = new PathGeometry();
                paths.FillRule = FillRule.EvenOdd;
                PathFigureCollection pfc = new PathFigureCollection();
                PathFigure pf = new PathFigure();
                pfc.Add(pf);
                pf.StartPoint = KernelProperty.MMToPix(fillPoints[0]);


                for (int i = 0; i < fillPoints.Count; i++)
                {
                    LineSegment ps = new LineSegment();
                    ps.Point = KernelProperty.MMToPix(fillPoints[i]);
                    pf.Segments.Add(ps);
                }


                pf.IsClosed = true;
                paths.Figures = pfc;
                PenColor = Colors.Black;
                dc.DrawGeometry(Brush, Pen, paths);
                PenColor = Colors.DeepPink;
                dc.DrawLine(Pen, KernelProperty.MMToPix(l5.Start), KernelProperty.MMToPix(l5.End));
                dc.Close();
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
            this.ChangeElement();
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
            this.ChangeElement();
        }
        /// <summary>
        /// 拷贝一个完整的元素
        /// </summary>
        /// <param name="v"></param>
        public override Geometry2D Copy(bool isclone)
        {
            var nstart = Vector2D.Create(this.start.X, this.start.Y);
            var nend = Vector2D.Create(this.end.X, this.end.Y);
            SteelBeamGeometry steelBeamGeometry = new SteelBeamGeometry(nstart, nend, this.thickness);

            if (isclone)
            {
        
            }
            else
            {
                steelBeamGeometry.Element = this.Element;
            }
            steelBeamGeometry.IsActioning = false;
            steelBeamGeometry.Thickness = this.thickness;
            steelBeamGeometry.PenColor = this.PenColor;
            steelBeamGeometry.FillColor = this.FillColor;
            steelBeamGeometry.Opacity = this.Opacity;
            return steelBeamGeometry;
        }


        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is SteelBeamGeometry)
            {
                SteelBeamGeometry steelBeam = (target as SteelBeamGeometry);

                this.start = TransformUtil.Mirror(steelBeam.start, mirrorLine);
                this.end = TransformUtil.Mirror(steelBeam.end, mirrorLine);
            }

        }

        /// <summary>
        /// 改变当前元素的位置
        /// </summary>
        protected override void ChangeElement()
        {

        }


        /// <summary>
        /// 写入XML信息
        /// </summary>
        /// <param name="sw"></param>
        public override void WriteXML(XmlWriter sw)
        {
            sw.WriteStartElement("SteelBeamGeometry");
            sw.WriteAttributeString("Start", this.start.X + " " + this.start.Y);
            sw.WriteAttributeString("End", this.end.X + " " + this.end.Y);
            sw.WriteAttributeString("Thinkness", this.Thickness.ToString());
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

            string thinknesss = xmlReader.GetAttribute("Thinkness");
            this.thickness = double.Parse(thinknesss);
        }

        /// <summary>
        /// 钢梁的高度
        /// </summary>
        public double Height { get; set; }
    }
}