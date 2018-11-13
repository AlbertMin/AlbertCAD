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
using System.Windows.Media.Imaging;

namespace Albert.DrawingKernel.Geometries.Consult
{
    /// <summary>
    /// 绘制圆的图形
    /// </summary>
    public class MutilImageGeometry : Geometry2D
    {

        /// <summary>
        /// 用于储存一个有多种状态和图片的图形对象
        /// </summary>
        /// <param name="v"></param>
        /// <param name="imgstate"></param>
        /// <param name="imageAlign"></param>
        public MutilImageGeometry(Vector2D central, ImageState[] imgstate, Align imageAlign = Align.CENTER)
        {

            this.central = central;
            this.ImageStates = imgstate;
            this.ImageAlign = imageAlign;
            this.Index = 0;
        
        }

        private Vector2D central = null;
        /// <summary>
        /// 当前多图片的中心位置
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

        /// <summary>
        /// 当前的图片和状态数组
        /// </summary>
        public ImageState[] ImageStates
        {

            get;
            set;
        }

        private int index = 0;

        /// <summary>
        /// 指定当前图形绘制器的图片索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int Index
        {
            set
            {

                if (index >= ImageStates.Length - 1)
                {
                    index = ImageStates.Length - 1;
                }
                if (index < 0)
                {

                    index = 0;
                }
                try
                {
                    Uri uri = new Uri(ImageStates[index].ImageURL);
                    BitmapImage img = new BitmapImage(uri);
                    this.bitmapImage = img;

                    index = value;
                    state = ImageStates[index].State;
                }
                catch (Exception ex)
                {
                    string Message = ex.Message;
                }

                this.Update();

            }
        }

        private string state = null;
        /// <summary>
        /// 通过名称指定显示的图片
        /// </summary>
        public string State {

            set {
            
                ImageState myis = null;
                for (int i = 0; i < this.ImageStates.Length; i++)
                {

                    if (ImageStates[i].State == value)
                    {
                        myis=ImageStates[i];
                        break;
                    }
                }
                if (myis != null)
                {
                    Uri uri = new Uri(myis.ImageURL);
                    BitmapImage img = new BitmapImage(uri);
                    this.bitmapImage = img;
                    state = value;
                }
                else {

                    this.Index = 0;
                   
                }
            }
        }
        /// <summary>
        /// 当前的图片的对齐方式
        /// </summary>
        public Align ImageAlign
        {
            get;
            set;
        }



        /// <summary>
        /// 当前的图片资源
        /// </summary>
        protected BitmapImage bitmapImage = null;
        protected BitmapImage BitmapImage
        {

            get
            {
                return bitmapImage;
            }
            set
            {
                bitmapImage = value;
            }

        }
        /// <summary>
        /// 更新当前图形
        /// </summary>
        /// <param name="ms"></param>
        public override void Update()
        {

            if (BitmapImage != null)
            {

                DrawingContext dc = this.RenderOpen();
                //冻结画笔，这样能加快绘图速度
                Pen.Freeze();
                var v = KernelProperty.MMToPix(central);
                Rect rect = new Rect();
                double ax, ay;
                switch (ImageAlign)
                {

                    case Align.CENTER:
                        ax = v.X - BitmapImage.Width / 2;
                        ay = v.Y - BitmapImage.Height / 2;
                        rect = new Rect(ax, ay, BitmapImage.Width, BitmapImage.Height);
                        break;
                    case Align.CENTER_TOP:
                        ax = v.X - BitmapImage.Width / 2;
                        ay = v.Y;
                        rect = new Rect(ax, ay, BitmapImage.Width, BitmapImage.Height);
                        break;
                    case Align.CENTER_BOTTOM:
                        ax = v.X - BitmapImage.Width / 2;
                        ay = v.Y - BitmapImage.Height;
                        rect = new Rect(ax, ay, BitmapImage.Width, BitmapImage.Height);
                        break;
                    case Align.LEFT_TOP:
                        ax = v.X;
                        ay = v.Y;
                        rect = new Rect(ax, ay, BitmapImage.Width, BitmapImage.Height);
                        break;
                    case Align.LEFT_CENTER:
                        ax = v.X;
                        ay = v.Y - BitmapImage.Height / 2;
                        rect = new Rect(ax, ay, BitmapImage.Width, BitmapImage.Height);
                        break;
                    case Align.LEFT_BOTTOM:
                        ax = v.X;
                        ay = v.Y - BitmapImage.Height;
                        rect = new Rect(ax, ay, BitmapImage.Width, BitmapImage.Height);
                        break;
                    case Align.RIGHT_TOP:
                        ax = v.X - BitmapImage.Width;
                        ay = v.Y;
                        rect = new Rect(ax, ay, BitmapImage.Width, BitmapImage.Height);
                        break;
                    case Align.RIGHT_CENTER:
                        ax = v.X - BitmapImage.Width;
                        ay = v.Y - BitmapImage.Height / 2;
                        rect = new Rect(ax, ay, BitmapImage.Width, BitmapImage.Height);
                        break;
                    case Align.RIGHT_BOTTOM:
                        ax = v.X - BitmapImage.Width;
                        ay = v.Y - BitmapImage.Height;
                        rect = new Rect(ax, ay, BitmapImage.Width, BitmapImage.Height);
                        break;
                }
                dc.DrawImage(bitmapImage, rect);
                dc.Close();
            }
        }


        /// <summary>
        /// 绘制圆形，并且填充当前形状
        /// </summary>
        /// <param name="Central"></param>
        /// <param name="RadiusX"></param>
        /// <param name="RadiusY"></param>
        /// <param name="brush"></param>
        protected override void DrawArcFill(Vector2D Central, double RadiusX, double RadiusY)
        {
            DrawingContext dc = this.RenderOpen();
            Pen.Freeze();  //冻结画笔，这样能加快绘图速度
            EllipseGeometry Geometry = new EllipseGeometry();
            Geometry.Center = KernelProperty.MMToPix(Central);
            Geometry.RadiusX = RadiusX;
            Geometry.RadiusY = RadiusY;
            dc.DrawGeometry(Brush, Pen, Geometry);
            dc.Close();
        }


        /// <summary>
        /// 获取当前所有的顶点集合
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                if (Central != null)
                {
                    List<Vector2D> points = new List<Vector2D>();
                    points.Add(central);
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
        /// <summary>
        /// 捕获圆形上的点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override IntersectPoint IntersectPoint(Vector2D v)
        {

            double ax = 0, ay = 0;
            switch (ImageAlign)
            {

                case Align.CENTER:
                    ax = Central.X - BitmapImage.Width / 2;
                    ay = Central.Y - BitmapImage.Height / 2;
                    break;
                case Align.CENTER_TOP:
                    ax = Central.X - BitmapImage.Width / 2;
                    ay = Central.Y;
                    break;
                case Align.CENTER_BOTTOM:
                    ax = Central.X - BitmapImage.Width / 2;
                    ay = Central.Y - BitmapImage.Height;
                    break;
                case Align.LEFT_TOP:
                    ax = Central.X;
                    ay = Central.Y;
                    break;
                case Align.LEFT_CENTER:
                    ax = Central.X;
                    ay = Central.Y - BitmapImage.Height / 2;
                    break;
                case Align.LEFT_BOTTOM:
                    ax = Central.X;
                    ay = Central.Y - BitmapImage.Height;
                    break;
                case Align.RIGHT_TOP:
                    ax = Central.X - BitmapImage.Width;
                    ay = Central.Y;
                    break;
                case Align.RIGHT_CENTER:
                    ax = Central.X - BitmapImage.Width;
                    ay = Central.Y - BitmapImage.Height / 2;
                    break;
                case Align.RIGHT_BOTTOM:
                    ax = Central.X - BitmapImage.Width;
                    ay = Central.Y - BitmapImage.Height;
                    break;
            }

            if ((v.X > ax && v.X < ax + bitmapImage.Width) && (v.Y > ay && v.Y < ay + bitmapImage.Height))
            {
                IntersectPoint ip = new IntersectPoint();
                ip.IntersectPointStyle = 1;
                ip.Point = v;
                return ip;
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

    }

    /// <summary>
    /// 记录图片和状态信息
    /// </summary>
    public class ImageState{
    
        /// <summary>
        /// 当前的状态下的图片路径
        /// </summary>
       public String ImageURL{
       
           get;
           set;
       }

        /// <summary>
        /// 当前状态下的名称
        /// </summary>
        public string State{
            get;
            set;
        }
    }
}
