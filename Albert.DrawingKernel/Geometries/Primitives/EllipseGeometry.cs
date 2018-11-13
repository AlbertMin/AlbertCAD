using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Albert.DrawingKernel.Selector;
using Albert.Geometry.External;
using Albert.DrawingKernel.Util;
using System.Xml;
using Albert.Geometry.Transform;

namespace Albert.DrawingKernel.Geometries.Primitives
{

    /// <summary>
    /// 定义一个椭圆事件
    /// </summary>
    public class EllipseGeometry: Geometry2D
    {
        public EllipseGeometry() : base() { 
        
        }
        /// <summary>
        /// 当前的构造函数
        /// </summary>
        /// <param name="GeometryId"></param>
        public EllipseGeometry(string geometryId)
            : base(geometryId)
        { 
        
        }
        /// <summary>
        /// 构造函数，初始化一个椭圆
        /// </summary>
        /// <param name="central"></param>
        /// <param name="reference"></param>
        public EllipseGeometry(Vector2D central, Vector2D reference) {

            this.central = central;
            this.reference = reference;
        }
        /// <summary>
        /// 当前的中心点
        /// </summary>
        private Vector2D central = null;

        /// <summary>
        /// 当前图形的中心点
        /// </summary>
        public Vector2D Central {

            get {
                return central;
            }
            set {
                central = value;
            }
        }

        private Vector2D reference = null;

        /// <summary>
        /// 当前图形的参照点
        /// </summary>
        public Vector2D Reference
        {

            get
            {
                return reference;
            }
            set
            {
                reference = value;
            }
        }
        /// <summary>
        /// 椭圆的X轴
        /// </summary>
        public double RadiusX
        {

            get
            {
                if (Reference != null && central != null)
                {
                    return Math.Abs(Reference.X - central.X);
                }
                return 0;
            }
        }

        /// <summary>
        /// 椭圆的Y轴
        /// </summary>
        public double RadiusY
        {

            get
            {
                if (Reference != null && central != null)
                {
                    return Math.Abs(Reference.Y - central.Y);
                }
                return 0;
            }
        }


        /// <summary>
        /// 获取当前关注的顶点信息
        /// </summary>
        public override List<Vector2D> Points {
            get {
                if (Central != null)
                {
                    List<Vector2D> points = new List<Vector2D>();
                    var v1 = Vector2D.Create(Central.X + RadiusX, Central.Y);
                    var v2 = Vector2D.Create(Central.X - RadiusX, Central.Y);
                    var v3 = Vector2D.Create(Central.X, Central.Y + RadiusY);
                    var v4 = Vector2D.Create(Central.X, Central.Y - RadiusY);
                    points.Add(Central);
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
        /// 获取当前的图形上的线
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {
                return null;
            }
        }


        /// <summary>
        /// 更新当前图形
        /// </summary>
        /// <param name="ms"></param>
        public override void Update()
        {
            if (Central != null)
            {
                this.DrawArcFill(Central, RadiusX, RadiusY);
            }

        }
        /// <summary>
        /// 获取相交点
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
        /// 取消当前的选择
        /// </summary>
        public override void UnSelect()
        {
            base.UnSelect();
        }


        /// <summary>
        /// 当前的线的移动
        /// </summary>
        /// <param name="v"></param>
        public override void Move(Vector2D v)
        {

            this.central.MoveTo(v);
            this.reference.MoveTo(v);
            this.Update();
        }
        /// <summary>
        /// 当前线的缩放
        /// </summary>
        /// <param name="zm"></param>
        public override void Scale(double zm)
        {

            this.central = this.central * zm;
            this.reference = this.reference * zm;
            this.Update();
        }
        /// <summary>
        /// 当前线的旋转
        /// </summary>
        /// <param name="c"></param>
        /// <param name="angle"></param>
        public override void Rolate(Vector2D c, double angle)
        {

            this.central.Rotate(c, angle);
            this.reference.Rotate(c, angle);
            this.Update();
        }
        /// <summary>
        /// 当前线的拷贝
        /// </summary>
        /// <param name="v"></param>
        public override Geometry2D Copy(bool isclone)
        {
            var ncentral = Vector2D.Create(this.central.X, this.central.Y);
            var nreference = Vector2D.Create(this.reference.X, this.reference.Y);
            EllipseGeometry ellipseGeometry = new EllipseGeometry(ncentral, nreference);
            if (isclone)
            {
              
            }
            else
            {
                ellipseGeometry.Element = this.Element;
            }
            ellipseGeometry.PenColor = this.PenColor;
            ellipseGeometry.FillColor = this.FillColor;
            ellipseGeometry.Element = this.Element;
            return ellipseGeometry;
        }

        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is EllipseGeometry)
            {
                EllipseGeometry ellipse = (target as EllipseGeometry);
                this.central = TransformUtil.Mirror(ellipse.central, mirrorLine);
                this.reference = TransformUtil.Mirror(ellipse.reference, mirrorLine);
            }

        }
        /// <summary>
        /// 写入XML信息
        /// </summary>
        /// <param name="sw"></param>
        public override void WriteXML(XmlWriter sw)
        {
            sw.WriteStartElement("EllipseGeometry");
            sw.WriteAttributeString("Central", this.central.X + " " + this.central.Y);
            sw.WriteAttributeString("Reference", this.Reference.X + " " + this.Reference.Y);
            sw.WriteEndElement();
        }

        /// <summary>
        /// 读取XML信息
        /// </summary>
        /// <param name="xmlReader"></param>
        public override void ReadXML(XmlReader xmlReader)
        {
            string central = xmlReader.GetAttribute("Central");
            var ns = central.Split(' ');
            this.central = new Vector2D(double.Parse(ns[0]), double.Parse(ns[1]));

            string reference = xmlReader.GetAttribute("Reference");
            var ne = reference.Split(' ');
            this.Reference = new Vector2D(double.Parse(ne[0]), double.Parse(ne[1]));
        }
    }
}
