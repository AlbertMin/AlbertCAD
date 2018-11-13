using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.External;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Selector;
using Albert.DrawingKernel.Util;
using Albert.Geometry.Transform;

namespace Albert.DrawingKernel.Geometries.Primitives
{
    /// <summary>
    /// OSB图形元素
    /// </summary>
    public class OSBGeometry : Geometry2D
    {
        /// <summary>
        /// 构造函数，初始化当前OSB板的图形
        /// </summary>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="locationPoint"></param>
        public OSBGeometry(double length, double width, Vector2D locationPoint)
        {
            Length = length;
            Width = width;
            LocationPoint = locationPoint;
            pPoints = GetAllPoints(locationPoint);
        }

        /// <summary>
        /// 初始化当前图形元素
        /// </summary>
        /// <param name="locationPoint"></param>
        public OSBGeometry(Vector2D locationPoint)
        {
            Length = 2440;
            Width = 1220;
            LocationPoint = locationPoint;
            pPoints = GetAllPoints(locationPoint);
        }

        /// <summary>
        /// 构造函数，初始化
        /// </summary>
        public OSBGeometry() : base(Guid.NewGuid().ToString())
        {
            MirrorF = -1;
        }

        /// <summary>
        /// 构造当前的OSB图形
        /// </summary>
        /// <param name="edges"></param>
        public OSBGeometry(List<Line2D> edges)
        {
            pPoints = edges.Select(x => x.Start).ToList();

        }

        public double Length { get; set; }
        public double Width { get; set; }
        public Vector2D LocationPoint { get; set; }
        private List<Vector2D> pPoints = null;
        public List<Vector2D> PPoints
        {
            get { return pPoints; }
            set { pPoints = value; }
        }
        public int Status { get; set; }

        public int MirrorF { get; set; }

        /// <summary>
        /// 获取当前所有的点
        /// </summary>
        /// <param name="locationPoint"></param>
        /// <returns></returns>
        private List<Vector2D> GetAllPoints(Vector2D locationPoint = null)
        {
            if (LocationPoint != null)
            {
                if (locationPoint == null)
                    locationPoint = LocationPoint;
                if (Status > 3)
                    Status = 0;
                switch (Status)
                {
                    default:
                        return new List<Vector2D>
                    {
                        locationPoint,
                        locationPoint.Offset(new Vector2D(Length, 0)),
                        locationPoint.Offset(new Vector2D(Length, MirrorF*Width)),
                        locationPoint.Offset(new Vector2D(0, MirrorF*Width)),
                    };

                    case 1:
                        return new List<Vector2D>
                    {
                        locationPoint,
                        locationPoint.Offset(new Vector2D(-Width, 0)),
                        locationPoint.Offset(new Vector2D(-Width, MirrorF*Length)),
                        locationPoint.Offset(new Vector2D(0, MirrorF*Length)),

                    };
                    case 2:
                        return new List<Vector2D>
                    {
                        locationPoint,
                        locationPoint.Offset(new Vector2D(-Length, 0)),
                        locationPoint.Offset(new Vector2D(-Length, -MirrorF*Width)),
                        locationPoint.Offset(new Vector2D(0,-MirrorF*Width)),
                    };
                    case 3:
                        return new List<Vector2D>
                    {
                        locationPoint,
                        locationPoint.Offset(new Vector2D(Width,0)),
                        locationPoint.Offset(new Vector2D(Width, -MirrorF*Length)),
                        locationPoint.Offset(new Vector2D(0, -MirrorF*Length)),
                    };
                }
            }

            return PPoints;

        }

        /// <summary>
        /// 刷新当前的图形
        /// </summary>
        public override void Update()
        {
            pPoints = GetAllPoints();
            if (pPoints.Count > 1)
            {
                this.DrawFill(pPoints);
            }
        }

        /// <summary>
        /// 获取当前图形的点
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                return pPoints.ToList();
            }
        }

        /// <summary>
        /// 获取当前所有的线
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
                lines.Add(new Line2D(pPoints.Last(), pPoints.First()));
                return lines;
            }
        }

        /// <summary>
        /// 当前的OSB相交计算
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
        /// 绘制当前图形元素
        /// </summary>
        /// <param name="points"></param>
        protected override void Draw(List<Vector2D> points)
        {
            if (LocationPoint != null)
            {
                DrawFill(pPoints);
            }
        }


        /// <summary>
        /// 当前的线的移动
        /// </summary>
        /// <param name="v"></param>
        public override void Move(Vector2D v)
        {

            this.pPoints.ForEach(x => {

                x.MoveTo(v);
            });
            this.Update();

            ChangeElement();
        }
        /// <summary>
        /// 当前线的缩放
        /// </summary>
        /// <param name="zm"></param>
        public override void Scale(double zm)
        {

            this.pPoints.ForEach(x =>
            {
                x = x * zm;
            });
            this.Update();
            ChangeElement();
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
                x.Rotate(c, angle) ;
            });
            this.Update();
            ChangeElement();
        }

        protected override void ChangeElement()
        {

   
 
        }
        /// <summary>
        /// 当前线的拷贝
        /// </summary>
        /// <param name="v"></param>
        public override Geometry2D Copy(bool isclone)
        {
            List<Vector2D> pps = new List<Vector2D>();
            pPoints.ForEach(x =>
            {
                pps.Add(Vector2D.Create(x.X, x.Y));
            });
            OSBGeometry osbGeometry = new OSBGeometry();
            osbGeometry.pPoints = pps;
            if (isclone)
            {
      
            }
            else
            {
                osbGeometry.Element = this.Element;
            }
  
            osbGeometry.PenColor = this.PenColor;
            osbGeometry.FillColor = this.FillColor;
            osbGeometry.Opacity = this.Opacity;
            return osbGeometry;
        }


        internal override void Mirror(Geometry2D target, Line2D mirrorLine)
        {
            if (target is OSBGeometry)
            {
                OSBGeometry osb = (target as OSBGeometry);
                this.PPoints = new List<Vector2D>();
                osb.pPoints.ForEach(x =>
                {
                    this.PPoints.Add(TransformUtil.Mirror(x, mirrorLine));
                });
            }

        }

    }
}
