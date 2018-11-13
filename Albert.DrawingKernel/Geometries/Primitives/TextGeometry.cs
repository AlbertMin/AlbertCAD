using Albert.DrawingKernel.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using System.Windows.Media;
using Albert.DrawingKernel.Util;
using System.Windows;

namespace Albert.DrawingKernel.Geometries.Primitives
{
    public class TextGeometry : Geometry2D
    {

        private Vector2D central = null;
        /// <summary>
        /// 获取当前文字的中心位置
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
        private string text = "";

        /// <summary>
        /// 获取当前文字信息
        /// </summary>
        public string Text
        {

            get { return text; }
            set { text = value; }
        }

        private int fontsize = 14;
        /// <summary>
        /// 当前文字的尺寸大小
        /// </summary>
        public int FontSize
        {
            get
            {
                return fontsize;
            }
            set
            {

                fontsize = value;
            }

        }

        private double angle = 0;
        /// <summary>
        /// 获取当前文本的角度信息
        /// </summary>
        public double Angle
        {

            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }
        /// <summary>
        /// 获取当前所有直线的列表
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {
                List<Line2D> lines = new List<Line2D>();
                return lines;
            }
        }

        /// <summary>
        /// 获取所有顶点
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                List<Vector2D> vectors = new List<Vector2D>();
                return vectors;
            }
        }

        /// <summary>
        /// 绘制当地文字
        /// </summary>
        public override void Update()
        {

            if (Central != null)
            {
                List<Vector2D> vs = new List<Vector2D>();
                vs.Add(Central);
                Draw(vs);
            }
        }


        protected override void Draw(List<Vector2D> points)
        {


            if (this.Text != "")
            {
                DrawingContext dc = this.RenderOpen();
                RotateTransform rt = new RotateTransform();
                var md = KernelProperty.MMToPix(Central);
                rt.CenterX = md.X;
                rt.CenterY = md.Y;
                dc.PushTransform(rt);
                FormattedText ft = new FormattedText(text, new System.Globalization.CultureInfo(0x0804, false), System.Windows.FlowDirection.LeftToRight, new Typeface("宋体"), FontSize, new SolidColorBrush(PenColor));
                dc.DrawText(ft, md);
                dc.Close();
            }
         
          

        }
    }
}
