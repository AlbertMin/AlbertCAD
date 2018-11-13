using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Selector;
using System.Xml;
using Albert.Geometry.Transform;
using Albert.DrawingKernel.Util;

namespace Albert.DrawingKernel.Geometries.Primitives
{
    /// <summary>
    /// 绘制一个楼板
    /// </summary>
    public class FloorGeometry : Geometry2D
    {
        /// <summary>
        /// 初始化一个多边形
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public FloorGeometry(List<Vector2D> points)
        {
            pPoints = points;
        }


        /// <summary>
        /// 构造函数，初始化一个直线
        /// </summary>
        public FloorGeometry()
        {
            pPoints = new List<Vector2D>();
        }
        /// <summary>
        /// 构造一个多线段
        /// </summary>
        /// <param name="geometryId"></param>
        public FloorGeometry(string geometryId) : base(geometryId)
        {

        }

        /// <summary>
        /// 当前点的集合
        /// </summary>
        private List<Vector2D> pPoints = null;

        /// <summary>
        /// 获取当前所有的顶点信息
        /// </summary>
        public List<Vector2D> PPoints
        {
            get
            {
                return pPoints;
            }
        }


        /// <summary>
        /// 更新当前的绘制
        /// </summary>
        public override void Update()
        {
            if (PPoints.Count > 1)
            {
                this.DrawFill(PPoints);
            }

        }


        /// <summary>
        /// 获取当前图形能捕捉的点
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                return pPoints.ToList();
            }
        }


        /// <summary>
        /// 获取当前多边形上的点信息
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {
                List<Line2D> lines = new List<Line2D>();
                for (int i = 1; i < pPoints.Count; i++)
                {
                    var l = Line2D.Create(pPoints[i - 1], pPoints[i]);
                    lines.Add(l);
                }
                if (pPoints.Count > 2)
                {
                    lines.Add(Line2D.Create(pPoints[pPoints.Count - 1], pPoints[0]));
                }
                return lines;
            }
        }

        public double Thickness { get; internal set; }

        /// <summary>
        /// 在当前图形的点捕获
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
                    var nv = v.ProjectOn(Lines[i]);
                    IntersectPoint ip = new IntersectPoint();
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
        /// 当前的线的移动
        /// </summary>
        /// <param name="v"></param>
        public override void Move(Vector2D v)
        {
            this.pPoints.ForEach(x =>
            {
                x.MoveTo(v);
            });
            this.Update();
        }
        /// <summary>
        /// 当前线的缩放
        /// </summary>
        /// <param name="zm"></param>
        public override void Scale(double zm)
        {

            for (int i = 0; i < pPoints.Count; i++)
            {

                pPoints[i] = pPoints[i] * zm;
            }
            this.Update();
        }
        /// <summary>
        /// 当前线的旋转
        /// </summary>
        /// <param name="c"></param>
        /// <param name="angle"></param>
        public override void Rolate(Vector2D c, double angle)
        {
            this.pPoints.ForEach(x =>
            {
                x.Rotate(c, angle);
            });

            this.Update();
        }
        /// <summary>
        /// 当前线的拷贝
        /// </summary>
        /// <param name="v"></param>
        public override Geometry2D Copy(bool isclone)
        {
            List<Vector2D> vts = new List<Vector2D>();
            this.pPoints.ForEach(x =>
            {

                vts.Add(Vector2D.Create(x.X, x.Y));
            });

            PolygonGeometry polygonGeometry = new PolygonGeometry(vts);
            if (isclone)
            {

            }
            else
            {
                polygonGeometry.Element = this.Element;
            }
            polygonGeometry.PenColor = this.PenColor;
            return polygonGeometry;
        }

        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is FloorGeometry)
            {
                FloorGeometry floor = (target as FloorGeometry);

                this.pPoints = new List<Vector2D>();
                floor.pPoints.ForEach(x => {
                    pPoints.Add(TransformUtil.Mirror(x, mirrorLine));
                });
            }

        }
        /// <summary>
        /// 写入当前数据
        /// </summary>
        /// <param name="sw"></param>
        public override void WriteXML(XmlWriter sw)
        {
            sw.WriteStartElement("FloorGeometry");
            string pointstring = string.Empty;
            if (this.pPoints != null)
            {
                pPoints.ForEach(x => {
                    if (pointstring == string.Empty)
                    {
                        pointstring += x.X + " " + x.Y;
                    }
                    else
                    {
                        pointstring += "," + x.X + " " + x.Y;
                    }

                });
            }
            sw.WriteAttributeString("Points", pointstring);
            sw.WriteEndElement();
        }

        /// <summary>
        /// 读取一个xml数据
        /// </summary>
        /// <param name="xmlReader"></param>
        public override void ReadXML(XmlReader xmlReader)
        {
            string points = xmlReader.GetAttribute("Points");
            string[] ps = points.Split(',');
            for (int i = 0; i < ps.Length; i++)
            {

                var ns = ps[i].Split(' ');
                Vector2D v = new Vector2D(double.Parse(ns[0]), double.Parse(ns[1]));
                this.pPoints.Add(v);
            }
        }

    }
}
