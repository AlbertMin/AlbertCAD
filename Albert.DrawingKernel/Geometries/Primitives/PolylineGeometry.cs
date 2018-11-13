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
    /// 多线段的图形
    /// </summary>
    public class PolylineGeometry : Geometry2D
    {

        /// <summary>
        /// 初始化一个多边形
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public PolylineGeometry(List<Vector2D> points)
        {
            pPoints = points;
        }


        /// <summary>
        /// 构造函数，初始化一个直线
        /// </summary>
        public PolylineGeometry()
        {
            pPoints = new List<Vector2D>();
        }


        /// <summary>
        /// 构造一个多线段
        /// </summary>
        /// <param name="geometryId"></param>
        public PolylineGeometry(string geometryId) : base(geometryId)
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
                this.Draw(PPoints);
            }

        }


        /// <summary>
        /// 获取当前所有的顶点信息
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
                return lines;
            }
        }


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

            if (this.Lines.Count > 0)
            {
                var clines = this.Lines;
                clines.RemoveAt(this.Lines.Count - 1);
                for (int i = 0; i < clines.Count; i++)
                {
                    var len = clines[i].Distance(v);
                    if (len < KernelProperty.Tolerance)
                    {
                        IntersectPoint ip = new IntersectPoint();
                        var nv = v.ProjectOn(Lines[i]);
                        ip.IntersectPointStyle = 1;
                        ip.Line = clines[i];
                        ip.Point = nv;
                        return ip;
                    }
                }

            }

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
            PolylineGeometry polylineGeometry = new PolylineGeometry(vts);
            if (isclone)
            {
       
            }
            else
            {
                polylineGeometry.Element = this.Element;
            }
            polylineGeometry.PenColor = this.PenColor;
            return polylineGeometry;
        }


        /// <summary>
        /// 对当前图形进行镜像处理
        /// </summary>
        /// <param name="mirrorLine"></param>
        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is PolylineGeometry)
            {
                PolylineGeometry polyline = (target as PolylineGeometry);
                this.pPoints = new List<Vector2D>();
                polyline.pPoints.ForEach(x => {
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
            sw.WriteStartElement("PolylineGeometry");
            string pointstring = string.Empty;
            if (this.pPoints != null) {
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
            string[] ps= points.Split(',');
            for (int i = 0; i < ps.Length; i++) {

                var ns = ps[i].Split(' ');
                Vector2D v = new Vector2D(double.Parse(ns[0]), double.Parse(ns[1]));
                this.pPoints.Add(v);
            }
        }
    }
}
