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
    /// 绘制一个钢梁
    /// </summary>
    public class SteelColumnGeometry : Geometry2D
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
        public SteelColumnGeometry()
            : base()
        {

        }
        /// <summary>
        /// 构造函数，初始化当前圆形
        /// </summary>
        /// <param name="GeometryId"></param>
        public SteelColumnGeometry(string GeometryId) : base(GeometryId) { }

        /// <summary>
        /// 构造函数，初始化当前的圆形图形
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public SteelColumnGeometry(Vector2D start, Vector2D end) {

            this.start = start;
            this.end = end;
        }


        /// <summary>
        /// 获取当前圆形的半径
        /// </summary>
        public double Length
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
                List<Vector2D> fillPoints = new List<Vector2D>();


                var halfFB = 300 / 2;
                var start3D = new Albert.Geometry.Primitives.Vector3D(Start.X, Start.Y, 0);
                var end3D = new Albert.Geometry.Primitives.Vector3D(End.X, End.Y, 0);
                Vector3D lineDir = Line3D.Create(end3D, start3D).Direction;
                Vector3D offsetDir = lineDir.Cross(new Vector3D(0, 0, 1)).Normalize();

                //偏移出上下点
                Vector3D offv1 = start3D + halfFB * offsetDir;
                Vector3D offv2 = start3D - halfFB * offsetDir;
                Vector3D offv3 = end3D - halfFB * offsetDir;
                Vector3D offv4 = end3D + halfFB * offsetDir;

                var HalfYYThinkness = 30 / 2;

                Vector3D v1 = offv1 + HalfYYThinkness * lineDir;
                Vector3D v2 = offv1 + HalfYYThinkness * -lineDir;

                Vector3D v3 = offv2 + HalfYYThinkness * lineDir;
                Vector3D v4 = offv2 + HalfYYThinkness * -lineDir;


                var HalfFBThinkness = 30 / 2;

                Vector3D v5 = v2 + (halfFB - HalfFBThinkness) * -offsetDir;
                Vector3D v6 = v4 + (halfFB - HalfFBThinkness) * offsetDir;




                Vector3D v7 = offv3 + HalfYYThinkness * -lineDir;
                Vector3D v8 = offv3 + HalfYYThinkness * lineDir;

                Vector3D v9 = offv4 + HalfYYThinkness * -lineDir;
                Vector3D v10 = offv4 + HalfYYThinkness * lineDir;


                Vector3D v12 = v8 + (halfFB - HalfFBThinkness) * offsetDir;
                Vector3D v11 = v10 + (halfFB - HalfFBThinkness) * -offsetDir;


                fillPoints.Add(TransformUtil.Projection(v1));
                fillPoints.Add(TransformUtil.Projection(v2));
                fillPoints.Add(TransformUtil.Projection(v5));
                fillPoints.Add(TransformUtil.Projection(v11));
                fillPoints.Add(TransformUtil.Projection(v8));
                fillPoints.Add(TransformUtil.Projection(v7));
                fillPoints.Add(TransformUtil.Projection(v9));
                fillPoints.Add(TransformUtil.Projection(v10));
                fillPoints.Add(TransformUtil.Projection(v12));
                fillPoints.Add(TransformUtil.Projection(v6));
                fillPoints.Add(TransformUtil.Projection(v4));
                fillPoints.Add(TransformUtil.Projection(v3));
                this.DrawFill(fillPoints);
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
                    points.Add(Start);
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
            SteelColumnGeometry steelColumnGeometry = new SteelColumnGeometry(nstart, nend);
            if (isclone)
            {
           
            }
            else
            {
                steelColumnGeometry.Element = this.Element;
            }
            steelColumnGeometry.PenColor = this.PenColor;
            steelColumnGeometry.FillColor = this.FillColor;
            return steelColumnGeometry;
        }

        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is SteelColumnGeometry)
            {
                SteelColumnGeometry steelColumn = (target as SteelColumnGeometry);

                this.start = TransformUtil.Mirror(steelColumn.start, mirrorLine);
                this.end = TransformUtil.Mirror(steelColumn.end, mirrorLine);
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
