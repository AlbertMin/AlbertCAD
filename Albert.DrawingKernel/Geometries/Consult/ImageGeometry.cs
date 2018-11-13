using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using Albert.DrawingKernel.Util;
using Albert.DrawingKernel.Selector;
using System.Windows.Media.Effects;

namespace Albert.DrawingKernel.Geometries.Consult
{
    /// <summary>
    /// 在CAD上添加一个图片图形
    /// </summary>
    public class ImageGeometry : Geometry2D
    {

        /// <summary>
        ///构造函数，初始化一个图片图形
        /// </summary>
        /// <param name="v"></param>
        /// <param name="imageURL"></param>
        public ImageGeometry(Vector2D v, string imageURL, Align imageAlign= Align.CENTER) {

            this.Central = v;
            this.ImageURL = imageURL;
            this.ImageAlign = imageAlign;
        }

        /// <summary>
        /// 当前图片的中心坐标
        /// </summary>
        public Vector2D Central
        {
            get;set;
        }

        private string imageURL = null;

        /// <summary>
        /// 当前图片的路径
        /// </summary>
        public string ImageURL {

            get {
                return imageURL;
            }
            set {
                imageURL = value;

                try
                {
                    Uri uri = new Uri(ImageURL);
                    BitmapImage img = new BitmapImage(uri);
                    this.bitmapImage = img;
                }
                catch (Exception ex) {

                    string Message = ex.Message;
                }
            }
        }

        /// <summary>
        /// 当前的图片资源
        /// </summary>
        protected BitmapImage bitmapImage = null;
        protected BitmapImage BitmapImage {

            get {
                return bitmapImage;
            }
            set {
                bitmapImage = value;
            }

        }

        /// <summary>
        /// 当前图片元素的对齐方式
        /// </summary>
        public Align ImageAlign
        {

            get;
            set;
        }
        /// <summary>
        /// 返回当前图形上关联的线
        /// </summary>
        public override List<Line2D> Lines
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override List<Vector2D> Points
        {
            get
            {
                List<Vector2D> vts = new List<Vector2D>() { Central };

                return vts;
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

            var cv = KernelProperty.MMToPix(Central);
            switch (ImageAlign)
            {

                case Align.CENTER:
                    ax = cv.X - BitmapImage.Width / 2;
                    ay = cv.Y - BitmapImage.Height / 2;
                    break;
                case Align.CENTER_TOP:
                    ax = cv.X - BitmapImage.Width / 2;
                    ay = cv.Y;
                    break;
                case Align.CENTER_BOTTOM:
                    ax = cv.X - BitmapImage.Width / 2;
                    ay = cv.Y - BitmapImage.Height;
                    break;
                case Align.LEFT_TOP:
                    ax = cv.X;
                    ay = cv.Y;
                    break;
                case Align.LEFT_CENTER:
                    ax = cv.X;
                    ay = cv.Y - BitmapImage.Height / 2;
                    break;
                case Align.LEFT_BOTTOM:
                    ax = cv.X;
                    ay = cv.Y - BitmapImage.Height;
                    break;
                case Align.RIGHT_TOP:
                    ax = cv.X - BitmapImage.Width;
                    ay = cv.Y;
                    break;
                case Align.RIGHT_CENTER:
                    ax = cv.X - BitmapImage.Width;
                    ay = cv.Y - BitmapImage.Height / 2;
                    break;
                case Align.RIGHT_BOTTOM:
                    ax = cv.X - BitmapImage.Width;
                    ay = cv.Y - BitmapImage.Height;
                    break;
            }

            Point v1 = new Point(ax, ay);
            Point v2 = new Point(ax + BitmapImage.Width, ay + BitmapImage.Height);

            var lft = KernelProperty.PixToMM(v1);
            var rfb = KernelProperty.PixToMM(v2);
            if ((v.X > lft.X && v.X < rfb.X) && (v.Y > rfb.Y && v.Y < lft.Y))
            {
                IntersectPoint ip = new IntersectPoint();
                ip.IntersectPointStyle = 1;
                ip.Point = v;
                return ip;
            }
            return null;
        }

        /// <summary>
        /// 图形的选择
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public override IntersectGeometry Select(Vector2D v = null)
        {
            var dse= new DropShadowEffect();
            dse.Color = Colors.Red;
            dse.ShadowDepth = 0;
            this.Effect = dse;
            return base.Select(v);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UnSelect()
        {
            this.Effect = null;
            base.UnSelect();
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
        /// 更新当前的图片信息
        /// </summary>
        public override void Update()
        {
            if (BitmapImage!= null){

                DrawingContext dc = this.RenderOpen();
                //冻结画笔，这样能加快绘图速度
                Pen.Freeze();
                var v = KernelProperty.MMToPix(Central);
                Rect rect = new Rect();
                double ax, ay;
                switch (ImageAlign) {

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
                         ay = v.Y ;
                        rect = new Rect(ax, ay, BitmapImage.Width, BitmapImage.Height);
                        break;
                    case Align.RIGHT_CENTER:
                         ax = v.X - BitmapImage.Width;
                         ay = v.Y - BitmapImage.Height/2;
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


    }
    public enum Align {
        CENTER = 0,
        CENTER_BOTTOM = 1,
        CENTER_TOP = 2,

        LEFT_TOP = 3,
        LEFT_CENTER =4,
        LEFT_BOTTOM = 5,

        RIGHT_TOP = 6,
        RIGHT_CENTER =7,
        RIGHT_BOTTOM=8
    }
}
