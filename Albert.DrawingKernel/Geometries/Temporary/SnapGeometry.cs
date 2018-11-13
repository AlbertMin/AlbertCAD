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

namespace Albert.DrawingKernel.Geometries.Temporary
{

    /// <summary>
    /// 用于在界面上显示一个点的捕获，这个点主要是端点和交点
    /// </summary>
    public class SnapGeometry : Geometry2D
    {
        /// <summary>
        /// 构造函数，定义当前的捕获点
        /// </summary>
        /// <param name="ip"></param>
        public SnapGeometry(IntersectPoint ip)
        {
            central = ip;
        }

        private IntersectPoint central = null;
        //获取当前选中点
        public IntersectPoint Central
        {
            get
            {
                return central;
            }
            private set
            {
                central = value;
            }
        }


        /// <summary>
        /// 当前图形点不计入捕获范围，则范围为Null
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 当前捕获点不产生需要捕获的直线，所以返回为空
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
            List<Vector2D> points = new List<Vector2D>();

            if (Central != null)
            {
                if (this.central.IntersectPointStyle == 0)
                {
                    var v1 = new Vector2D(central.Point.X - 5 * KernelProperty.PixelToSize, central.Point.Y - 5 * KernelProperty.PixelToSize);
                    var v2 = new Vector2D(central.Point.X + 5 * KernelProperty.PixelToSize, central.Point.Y - 5 * KernelProperty.PixelToSize);
                    var v3 = new Vector2D(central.Point.X + 5 * KernelProperty.PixelToSize, central.Point.Y + 5 * KernelProperty.PixelToSize);
                    var v4 = new Vector2D(central.Point.X - 5 * KernelProperty.PixelToSize, central.Point.Y + 5 * KernelProperty.PixelToSize);
                    points.Add(v1);
                    points.Add(v2);
                    points.Add(v3);
                    points.Add(v4);
                    points.Add(v1);
                    base.Draw(points);

                }
                else if (this.central.IntersectPointStyle == 1)
                {

                    if (this.central.Line != null)
                    {

                        var Start = this.central.Line.Start;
                        var End = this.central.Line.End;
                        var Middle = central.Point;
                        //对直线矩形偏移
                        Line3D subline3d = Line3D.Create(Vector3D.Create(Start.X, Start.Y, 0), Vector3D.Create(End.X, End.Y, 0));
                        var offsetDirection = subline3d.Direction.Cross(Vector3D.BasisZ);
                        //获取偏移量
                        var offsetdir = Vector2D.Create(offsetDirection.X, offsetDirection.Y) * KernelProperty.PixToMM(KernelProperty.SublineOffset);
                        
                        //冻结画笔，这样能加快绘图速度
                        DrawingContext dc = this.RenderOpen();
                        Pen.Freeze();
                        this.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);
                        this.PenColor = Colors.DeepSkyBlue;
                        this.Pen.EndLineCap = PenLineCap.Triangle;
                        var v1 = Start;
                        //添加偏移
                        var v2 = Start.Offset(offsetdir);
                        //绘制第一个线
                        dc.DrawLine(Pen, KernelProperty.MMToPix(v1), KernelProperty.MMToPix(v2));
                        var v3 = Middle.Offset(offsetdir);
                        //第二个线
                        dc.DrawLine(Pen, KernelProperty.MMToPix(v2), KernelProperty.MMToPix(v3));
                        //绘制文本
                        this.SetText(dc,Line2D.Create(v2, v3));
                        //绘制三个线竖线
                        dc.DrawLine(Pen, KernelProperty.MMToPix(v3), KernelProperty.MMToPix(Middle));

                        var v4 = End.Offset(offsetdir);
                        //绘制第四个线
                        dc.DrawLine(Pen, KernelProperty.MMToPix(v3), KernelProperty.MMToPix(v4));
                        this.SetText(dc, Line2D.Create(v3, v4));
                        //绘制第五个线
                        dc.DrawLine(Pen, KernelProperty.MMToPix(v4), KernelProperty.MMToPix(End));


                        //绘制偏移的线

                        var v5 = new Vector2D(central.Point.X - 5 * KernelProperty.PixelToSize, central.Point.Y - 5 * KernelProperty.PixelToSize);
                        var v6 = new Vector2D(central.Point.X + 5 * KernelProperty.PixelToSize, central.Point.Y - 5 * KernelProperty.PixelToSize);
                        var v7 = new Vector2D(central.Point.X - 5 * KernelProperty.PixelToSize, central.Point.Y + 5 * KernelProperty.PixelToSize);
                        var v8 = new Vector2D(central.Point.X + 5 * KernelProperty.PixelToSize, central.Point.Y + 5 * KernelProperty.PixelToSize);
                        dc.DrawLine(Pen, KernelProperty.MMToPix(v5), KernelProperty.MMToPix(v6));
                        dc.DrawLine(Pen, KernelProperty.MMToPix(v6), KernelProperty.MMToPix(v7));
                        dc.DrawLine(Pen, KernelProperty.MMToPix(v7), KernelProperty.MMToPix(v8));
                        dc.DrawLine(Pen, KernelProperty.MMToPix(v8), KernelProperty.MMToPix(v5));
                        dc.Close();


                    }
                }
                else if (this.central.IntersectPointStyle == 2)
                {
                    //冻结画笔，这样能加快绘图速度
                    DrawingContext dc = this.RenderOpen();
                    Pen.Freeze();
                    this.DashStyle = new System.Windows.Media.DashStyle(new double[] { 5, 5 }, 10);
                    this.PenColor = Colors.DeepSkyBlue;
                    this.Pen.EndLineCap = PenLineCap.Triangle;
                    if (this.central.Refences != null) {
                        this.central.Refences.ForEach(x => {
                            dc.DrawLine(Pen, KernelProperty.MMToPix(x.Start), KernelProperty.MMToPix(x.End));
                        });
                    }

                    dc.Close();

                }
                else if(this.central.IntersectPointStyle == 3)
                {
                    DrawingContext dc = this.RenderOpen();
                    Pen.Freeze();



                }
              
            }

        }
        /// <summary>
        /// 绘制相关的文字信息
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="startToLine"></param>
        private void SetText(DrawingContext dc, Line2D startToLine)
        {
            string text = ((int)startToLine.Length).ToString();
            Vector2D middle = startToLine.MiddlePoint;
            Vector2D dir = startToLine.Direction;
            var rad = dir.AngleFrom(Vector2D.BasisX);
            var md = KernelProperty.MMToPix(middle);
            RotateTransform rt = new RotateTransform();
            var angle = Extension.RadToDeg(rad);
            if (angle > 90 && angle < 270)
            {
                angle = angle + 180;
            }
            rt.Angle = angle;
            rt.CenterX = md.X;
            rt.CenterY = md.Y;
            dc.PushTransform(rt);
            FormattedText ft = new FormattedText(text, new System.Globalization.CultureInfo(0x0804, false), System.Windows.FlowDirection.LeftToRight, new Typeface("微软雅黑"), 14, Brushes.Blue);
            dc.DrawText(ft, md);
            dc.Pop();

        }
    }
}
