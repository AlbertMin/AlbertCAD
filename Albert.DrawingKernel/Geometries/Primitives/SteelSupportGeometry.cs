using Albert.DrawingKernel.Util;
using Albert.Geometry.Primitives;
using Albert.Geometry.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Albert.DrawingKernel.Geometries.Primitives
{
    /// <summary>
    /// 用于定义钢梁的支座
    /// </summary>
    public class SteelSupportGeometry : Geometry2D
    {

        private Vector2D central = null;

        /// <summary>
        /// 定义当前钢梁的定位点
        /// </summary>
        public Vector2D Central
        {
            get
            {
                return central;
            }

            set
            {
                central = value;
            }
        }

        private Vector2D face = null;
        /// <summary>
        /// 当前面向的物体
        /// </summary>
        public Vector2D Face
        {
            get
            {
                return face;
            }

            set
            {
                face = value;
            }
        }


        private double plateWidth = 400;
        /// <summary>
        /// 钢板的宽度
        /// </summary>
        public double PlateWidth
        {
            get
            {
                return plateWidth;
            }

            set
            {
                plateWidth = value;
            }
        }

        private double plateHeight = 100;
        /// <summary>
        /// 钢板的高度
        /// </summary>
        public double PlateHeight
        {
            get
            {
                return plateHeight;
            }

            set
            {
                plateHeight = value;
            }
        }

        private double sternaLength = 95;
        /// <summary>
        /// 腹板的长度
        /// </summary>
        public double SternaLength
        {
            get
            {
                return sternaLength;
            }

            set
            {
                sternaLength = value;
            }
        }



        private double sternaSeparation = 260;
        /// <summary>
        /// 腹板的间距
        /// </summary>
        public double SternaSeparation
        {
            get
            {
                return sternaSeparation;
            }

            set
            {
                sternaSeparation = value;
            }
        }

        private List<Vector2D> points = null;
        /// <summary>
        /// 获取当前所有的点
        /// </summary>
        public override List<Geometry.Primitives.Vector2D> Points
        {
            get
            {

                return points;
            }

        }
        /// <summary>
        /// 获取当前所有的线
        /// </summary>
        public override List<Geometry.Primitives.Line2D> Lines
        {

            get
            {

                List<Vector2D> fillPoints = new List<Vector2D>();


                var halfFW = PlateWidth / 2;
                var Orgion = new Albert.Geometry.Primitives.Vector3D(Central.X, Central.Y, 0);
                var Direction = new Albert.Geometry.Primitives.Vector3D(this.face.X, this.face.Y, 0);

                //当前偏移的方向
                Vector3D offsetDir = Direction.Cross(new Vector3D(0, 0, 1)).Normalize();

                var v1 = Orgion.Offset(offsetDir * halfFW);
                var v2 = Orgion.Offset(-offsetDir * halfFW);

                var v3 = v1.Offset(Direction * (SternaLength+5));
                var v4 = v2.Offset(Direction * (SternaLength+5));

                fillPoints.Add(TransformUtil.Projection(v1));
                fillPoints.Add(TransformUtil.Projection(v2));
                fillPoints.Add(TransformUtil.Projection(v4));
                fillPoints.Add(TransformUtil.Projection(v3));
                fillPoints.Add(TransformUtil.Projection(v1));
                List<Line2D> lineList = new List<Line2D>();
                for (int i = 1; i < fillPoints.Count; i++)
                {
              

                        lineList.Add(Line2D.Create(fillPoints[i - 1], fillPoints[i]));
                 

                }
                return lineList;
            }

        }





        /// <summary>
        /// 绘制当前钢梁
        /// </summary>
        public override void Update()
        {
            points = new List<Vector2D>();
            var halfFW = PlateWidth / 2;
            var Orgion = new Albert.Geometry.Primitives.Vector3D(Central.X, Central.Y, 0);
            var Direction = new Albert.Geometry.Primitives.Vector3D(this.face.X, this.face.Y, 0);

            //当前偏移的方向
            Vector3D offsetDir = Direction.Cross(new Vector3D(0, 0, 1)).Normalize();

            var v1 = Orgion.Offset(offsetDir * halfFW);
            var v2 = Orgion.Offset(-offsetDir * halfFW);

            var v3 = v2.Offset(Direction * 5);
            var v12 = v1.Offset(Direction * 5);

            var cent = Orgion.Offset(Direction * 5);

            //把中心区域向两边偏移

            var v4 = cent.Offset(-offsetDir * (SternaSeparation / 2 + 5));
            var v5 = v4.Offset(Direction * SternaLength);

            var v7 = cent.Offset(-offsetDir * (SternaSeparation / 2));
            var v6 = v7.Offset(Direction * SternaLength);

            var v8 = cent.Offset(offsetDir * (SternaSeparation / 2 + 5));
            var v9 = v8.Offset(Direction * SternaLength);

            var v11 = cent.Offset(offsetDir * (SternaSeparation / 2));
            var v10 = v11.Offset(Direction * SternaLength);


            var v13 = v12.Offset(Direction * SternaLength);
            var v14 = v3.Offset(Direction * SternaLength);

            points.Add(TransformUtil.Projection(v1));
            points.Add(TransformUtil.Projection(v2));
            points.Add(TransformUtil.Projection(v3));
            points.Add(TransformUtil.Projection(v4));
            points.Add(TransformUtil.Projection(v5));
            points.Add(TransformUtil.Projection(v6));
            points.Add(TransformUtil.Projection(v7));
            points.Add(TransformUtil.Projection(v8));
            points.Add(TransformUtil.Projection(v9));
            points.Add(TransformUtil.Projection(v10));
            points.Add(TransformUtil.Projection(v11));
            points.Add(TransformUtil.Projection(v12));


            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();  //冻结画笔，这样能加快绘图速度
            PathGeometry paths = new PathGeometry();

            PathFigureCollection pfc = new PathFigureCollection();
            PathFigure pf = new PathFigure();
            pfc.Add(pf);
            pf.StartPoint = KernelProperty.MMToPix(points[0]);
            for (int i = 0; i < points.Count; i++)
            {
                LineSegment ps = new LineSegment();
                ps.Point = KernelProperty.MMToPix(points[i]);
                pf.Segments.Add(ps);
            }
            pf.IsClosed = true;
            paths.Figures = pfc;
            dc.DrawGeometry(Brush, Pen, paths);

            this.Pen.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);

            dc.DrawLine(Pen, KernelProperty.MMToPix(TransformUtil.Projection(v12)), KernelProperty.MMToPix(TransformUtil.Projection(v13)));
            dc.DrawLine(Pen, KernelProperty.MMToPix(TransformUtil.Projection(v3)), KernelProperty.MMToPix(TransformUtil.Projection(v14)));
            dc.DrawLine(Pen, KernelProperty.MMToPix(TransformUtil.Projection(v13)), KernelProperty.MMToPix(TransformUtil.Projection(v14)));
            dc.Close();
        }
    }
}
