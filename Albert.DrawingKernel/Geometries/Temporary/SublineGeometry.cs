using Albert.DrawingKernel.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Selector;
using Albert.DrawingKernel.Util;
using System.Windows.Media;
using Albert.Geometry.External;


namespace Albert.DrawingKernel.Geometries.Temporary
{
    /// <summary>
    /// 当前的辅助线
    /// </summary>
    public class SublineGeometry : Geometry2D
    {

        private Vector2D start = null;
        /// <summary>
        /// 辅助线的起点
        /// </summary>
        public Vector2D Start {
            get {
                return start;
            }
            set {
                start = value;
            }
        }
        private Vector2D end = null;
        /// <summary>
        /// 辅助线的终点
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

        /// <summary>
        /// 当前的辅助线偏移量
        /// </summary>
        private int sublineOffset = 30;

        /// <summary>
        /// 设置标尺的偏移量
        /// </summary>
        public int SublineOffset
        {

            get { return sublineOffset; }
            set { sublineOffset = value; }
        }

        private bool showSize = true;
        /// <summary>
        /// 当前标尺是否显示尺寸信息
        /// </summary>
        public bool ShowSize {

            get { return showSize; }
            set { showSize = value; }
        }
        /// <summary>
        /// 构造函数，初始化当前的绘制模块
        /// </summary>
        /// <param name="p"></param>
        public SublineGeometry(Vector2D start, Vector2D end)
        {
            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// 构造函数，初始化当前的对象
        /// </summary>
        public SublineGeometry() : base()
        {

        }

        private Vector2D textpoint = Vector2D.Zero;
        /// <summary>
        /// 文本的位置信息
        /// </summary>
        public Vector2D TextPosition
        {
            private set {
                textpoint = value;
            }
            get
            {
                return textpoint;
            }
        }

        private double textAngle = 0;

        /// <summary>
        /// 文字的角度
        /// </summary>
        public double TextAngle
        {

            private set
            {
                textAngle = value;
            }
            get
            {
                return textAngle;
            }
        }

        private string textValue = "";
        /// <summary>
        /// 当前提示框的值
        /// </summary>
        public string TextValue
        {
            private set
            {
                textValue = value;
            }
            get
            {
                return textValue;
            }

        }
        /// <summary>
        /// 获取上面所有的直线信息
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前所有关心的顶点
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                return null;
            }
        }



        /// <summary>
        /// 绘制当前直线
        /// </summary>
        public override void Update()
        {
            if (Start != null && end != null)
            {
                //对直线矩形偏移
                Line3D subline3d = Line3D.Create(Vector3D.Create(Start.X, Start.Y, 0), Vector3D.Create(end.X, end.Y, 0));
                var offsetDirection = subline3d.Direction.Cross(Vector3D.BasisZ);
                //获取偏移量
                var offsetdir = Vector2D.Create(offsetDirection.X, offsetDirection.Y) * KernelProperty.PixToMM(SublineOffset);
                //添加偏移
                var offline = Line2D.Create(Start, end).Offset(offsetdir);
                List<Vector2D> points = new List<Vector2D>();
                this.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);
                this.PenColor = Colors.DeepSkyBlue;
                this.Pen.EndLineCap = PenLineCap.Triangle;
                points.Add(Start);
                points.Add(offline.Start);
                points.Add(offline.End);
                points.Add(end);
                this.Draw(points);
            }
        }

        /// <summary>
        /// 当前直线的绘制功能
        /// </summary>
        /// <param name="vss"></param>
        protected override void Draw(List<Vector2D> points) {
            DrawingContext dc = this.RenderOpen();
            //冻结画笔，这样能加快绘图速度
            Pen.Freeze();  
            dc.DrawLine(Pen, KernelProperty.MMToPix(points[0]), KernelProperty.MMToPix(points[1]));
            dc.DrawLine(Pen, KernelProperty.MMToPix(points[1]), KernelProperty.MMToPix(points[2]));
            SeText(dc, Line2D.Create(points[1], points[2]));
            dc.DrawLine(Pen, KernelProperty.MMToPix(points[2]), KernelProperty.MMToPix(points[3]));
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
            if (ShowSize)
            {
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

            this.TextPosition = middle;
            this.textAngle = angle;
            this.textValue = text;
        }
    }
}
