using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.Events;
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

/// <summary>
/// 所有的图形的核心类，用于在界面上显示二维图形元素
/// </summary>
namespace Albert.DrawingKernel.Geometries
{

    /// <summary>
    /// 图形的选择事件，定义了一个基础事件，图形的选择
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate void ShapeSelectHandle(SelectedEventArgs e);


    /// <summary>
    /// 当前CAD上要显示所有的图形元素
    /// </summary>
    public abstract class Geometry2D : DrawingVisual
    {
        /// <summary>
        /// 选中事件
        /// </summary>
        public event ShapeSelectHandle Selected;
        public event ShapeSelectHandle UnSelected;

        #region 图形的常规信息
        /// <summary>
        /// 当前的构造函数
        /// </summary>
        public Geometry2D()
        {
            GeometryId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 构造函数，初始化当前的图形对象
        /// </summary>
        /// <param name="geometryId"></param>
        public Geometry2D(string geometryId)
        {

            this.GeometryId = geometryId;
        }

        /// <summary>
        /// 当前图形关联的对象
        /// </summary>
        public dynamic Element
        {

            get;
            set;
        }

        private bool isActioning = false;
        /// <summary>
        /// 当前图形是否是正在绘制中的图形
        /// </summary>
        public bool IsActioning
        {
            get
            {
                return isActioning;
            }
            set
            {
                isActioning = value;
            }
        }



        private bool isEnabled = true;
        /// <summary>
        /// 判断当前的图形是否可以被选中
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (value)
                {

                    this.Opacity = 1;
                }
                else {
                    this.Opacity = 0.3;
                }
                isEnabled = value;
            }
        }

        /// <summary>
        /// 当前图形进行镜像处理
        /// </summary>
        /// <param name="tagert"></param>
        /// <param name="mirrorLine"></param>
        internal virtual void Mirror(Geometry2D tagert, Line2D mirrorLine)
        {

        }


        private bool isCommand = true;
        /// <summary>
        /// 判断当前对象是否可以执行命令
        /// </summary>
        public bool IsCommand
        {

            get { return isCommand; }
            set { isCommand = value; }
        }




        /// <summary>
        /// 当前元素的ID信息
        /// </summary>
        public string GeometryId
        {
            get;
            set;
        }


        private int lineWidth = 1;
        /// <summary>
        /// 当前的尺寸
        /// </summary>
        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }



        private DashStyle dashStyle = null;
        /// <summary>
        /// 是否有点划分样式
        /// </summary>
        public DashStyle DashStyle
        {
            get { return dashStyle; }
            set { dashStyle = value; }
        }


        private Color penColor = Colors.Black;
        /// <summary>
        /// 当前画笔的颜色
        /// </summary>
        public Color PenColor
        {
            get
            {
                return penColor;
            }
            set
            {
                penColor = value;
            }
        }



        private double brushOpacity = 1;
        /// <summary>
        /// 当前画笔的透明度
        /// </summary>
        public double BrushOpacity
        {

            get { return brushOpacity; }
            set { brushOpacity = value; }
        }



        private Color fillColor = Colors.Transparent;

        /// <summary>
        /// 当前内部填充的颜色
        /// </summary>
        public Color FillColor
        {
            get
            {
                return fillColor;
            }
            set
            {
                isFill = true;
                fillColor = value;
            }
        }



        /// <summary>
        /// 当前的笔画信息
        /// </summary>
        protected Pen Pen
        {
            get
            {
                var currentPen = new Pen(new SolidColorBrush(PenColor), lineWidth);
                if (DashStyle != null)
                {
                    currentPen.DashStyle = DashStyle;
                }
                return currentPen;

            }
        }


        /// <summary>
        /// 当前的填充画刷
        /// </summary>
        protected Brush Brush
        {

            get
            {
                var currentBrush = new SolidColorBrush(fillColor);
                currentBrush.Opacity = brushOpacity;
                return currentBrush;

            }
        }


        /// <summary>
        /// 当前图形是否可以填充
        /// </summary>
        private bool isFill = false;
        public bool IsFill
        {
            get
            {
                return isFill;
            }
            private set
            {
                isFill = value;
            }
        }



        /// <summary>
        /// 获取当前图形中需要捕捉的点
        /// </summary>
        public abstract List<Vector2D> Points
        {
            get;
        }

        /// <summary>
        /// 记录当前界面上需要捕捉的线
        /// </summary>
        public abstract List<Line2D> Lines
        {
            get;
        }


        /// <summary>
        /// 当前图形的着重线，核心线段，需要强调显示
        /// </summary>
        public virtual List<Line2D> Emphasize
        {

            get
            {
                return null;
            }
        }


        /// <summary>
        /// 绘制当前的元素
        /// </summary>
        /// <param name="ms"></param>
        public abstract void Update();


        /// <summary>
        /// 对当前进行选择,当前函数，是元素边界选择，非中心选择，元素的图形选择，交给外部实现
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public virtual IntersectGeometry Select(Vector2D v = null)
        {

            if (IsEnabled)
            {
                IntersectPoint ip = null;
                ip = IntersectPoint(v);
                if (ip != null)
                {
                    IntersectGeometry ig = new IntersectGeometry();
                    ig.GeometryShape = this;
                    ig.IntersectPoint = ip;
                    SelectedEventArgs gea = new SelectedEventArgs(ig);
                    if (Selected != null)
                    {
                        Selected(gea);
                    }
                    this.DashStyle = new DashStyle(new double[] { 3, 3 }, 0);
                    this.Update();
                    return ig;
                }
                else
                {

                    return null;
                }
            }
            return null;

        }


        /// <summary>
        /// 取消选择事件
        /// </summary>
        public virtual void UnSelect()
        {
            this.DashStyle = null;
            this.Update();

            if (UnSelected != null)
            {
                SelectedEventArgs gea = new SelectedEventArgs(null);
                UnSelected(gea);
            }
        }


        /// <summary>
        /// 获取在当前图形上的捕获点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public virtual IntersectPoint IntersectPoint(Vector2D v)
        {
            return null;
        }

        /// <summary>
        /// 这个函数，用于剔除最后一个点和最后一个线后的相交情况，主要是用于正在绘制的图形
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public virtual IntersectPoint IntersectPoint2(Vector2D v)
        {
            return null;
        }



        internal virtual void Revserse()
        {

        }
        #endregion

        #region 绘制函数，用于在界面上绘制指定的图形元素
        /// <summary>
        /// 按照点，绘制连续的线段，且没有填充
        /// </summary>
        /// <param name="points"></param>
        /// <param name="color"></param>
        /// <param name="thinkness"></param>
        /// <returns></returns>
        protected virtual void Draw(List<Vector2D> points)
        {
            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();  //冻结画笔，这样能加快绘图速度
            for (int i = 0; i < points.Count - 1; i++)
            {
                dc.DrawLine(Pen, KernelProperty.MMToPix(points[i]), KernelProperty.MMToPix(points[i + 1]));
            }
            dc.Close();
        }


        /// <summary>
        /// 根据所有的点，组成一个封闭区域，且可以填充，并且填充
        /// </summary>
        /// <param name="points"></param>
        /// <param name="brush"></param>
        protected virtual void DrawFill(List<Vector2D> points)
        {
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
            dc.Close();
        }

        /// <summary>
        /// 绘制圆形，且没有填充
        /// </summary>
        /// <param name="points"></param>
        protected virtual void DrawArc(Vector2D Central, double RadiusX, double RadiusY)
        {
            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();  //冻结画笔，这样能加快绘图速度
            EllipseGeometry Geometry = new EllipseGeometry();
            Geometry.Center = KernelProperty.MMToPix(Central);
            Geometry.RadiusX = KernelProperty.MMToPix(RadiusX);
            Geometry.RadiusY = KernelProperty.MMToPix(RadiusY);
            dc.DrawGeometry(new SolidColorBrush(Colors.Transparent), Pen, Geometry);
            dc.Close();
        }

        /// <summary>
        /// 绘制圆形，并且填充当前形状
        /// </summary>
        /// <param name="Central"></param>
        /// <param name="RadiusX"></param>
        /// <param name="RadiusY"></param>
        /// <param name="brush"></param>
        protected virtual void DrawArcFill(Vector2D Central, double RadiusX, double RadiusY)
        {
            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();  //冻结画笔，这样能加快绘图速度
            EllipseGeometry Geometry = new EllipseGeometry();
            Geometry.Center = KernelProperty.MMToPix(Central);
            Geometry.RadiusX = KernelProperty.MMToPix(RadiusX);
            Geometry.RadiusY = KernelProperty.MMToPix(RadiusY);
            dc.DrawGeometry(Brush, Pen, Geometry);
            dc.Close();
        }

        /// <summary>
        /// 给当前线段绘制箭头
        /// </summary>
        /// <param name="line"></param>
        protected virtual void DrawArrow(DrawingContext dc, Line2D line)
        {
            this.dashStyle = null;
            var start = line.Start;
            Line3D line3d = Line3D.Create(Vector3D.Create(line.Start.X, line.Start.Y, 0), Vector3D.Create(line.End.X, line.End.Y, 0));
            var dir = line3d.Direction.Cross(Vector3D.BasisZ);
            var ndir = Vector2D.Create(dir.X, dir.Y);
            var v1 = start.Offset(ndir * KernelProperty.PixToMM(5)).Offset(line.Direction * KernelProperty.PixToMM(5));
            var v2 = start.Offset(-ndir * KernelProperty.PixToMM(5)).Offset(line.Direction * KernelProperty.PixToMM(5));
            dc.DrawLine(Pen, KernelProperty.MMToPix(start), KernelProperty.MMToPix(v1));
            dc.DrawLine(Pen, KernelProperty.MMToPix(v2), KernelProperty.MMToPix(start));
            var end = line.End;
            var v3 = end.Offset(ndir * KernelProperty.PixToMM(5)).Offset(-line.Direction * KernelProperty.PixToMM(5));
            var v4 = end.Offset(-ndir * KernelProperty.PixToMM(5)).Offset(-line.Direction * KernelProperty.PixToMM(5));
            dc.DrawLine(Pen, KernelProperty.MMToPix(end), KernelProperty.MMToPix(v3));
            dc.DrawLine(Pen, KernelProperty.MMToPix(v4), KernelProperty.MMToPix(end));
        }

        #endregion

        #region 图形的操作函数
        /// <summary>
        /// 移动当前的图形,主要是移动多少向量
        /// </summary>
        /// <param name="v"></param>
        public virtual void Move(Vector2D v) { }
        public virtual void Scale(double zm) { }
        public virtual void Rolate(Vector2D v, double angle) { }
        public virtual Geometry2D Copy(bool IsClone = false)
        {
            return null;
        }

        //根据当前情况改变对应的元素对象,暂时只支持当前三种元素的移动
        protected virtual void ChangeElement()
        {



        }

        #endregion

        #region 图形的转换和输出

        /// <summary>
        /// 输出当前的XML对象
        /// </summary>
        /// <returns></returns>
        public string ToXMLString()
        {
            return "";
        }

        /// <summary>
        /// 将XML转化为当前对象
        /// </summary>
        public void FromXMLString()
        {

        }
        /// <summary>
        /// 读取一个XML
        /// </summary>
        /// <param name="xmlReader"></param>
        public virtual void ReadXML(XmlReader xmlReader)
        {

        }
        /// <summary>
        /// 写入一个XML
        /// </summary>
        /// <param name="sw"></param>
        public virtual void WriteXML(XmlWriter sw)
        {

        }

        #endregion

    }
}
