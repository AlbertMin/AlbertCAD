using System;
using System.Windows;
using System.Windows.Media;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Util
{
    /// <summary>
    /// 用于记录比例尺的信息
    /// </summary>
    public static class KernelProperty
    {
        /// <summary>
        /// 获取距离
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double Distance(this Point x, Point y)
        {
            return Math.Sqrt(Math.Pow(x.X - y.X, 2) + Math.Pow(x.Y - y.Y, 2));
        }

        private static double tolerance = 10;

        /// <summary>
        /// 判断当前的容差范围
        /// </summary>
        public static double Tolerance
        {
            get
            {
                return tolerance * pixelToSize;
            }
            set
            {
                tolerance = value;
            }
        }

        private static int sublineOffset = 30;

        /// <summary>
        /// 辅助线的偏移量
        /// </summary>
        public static int SublineOffset
        {

            get
            {
                return sublineOffset;
            }
            set
            {
                sublineOffset = value;
            }
        }

        /// <summary>
        /// 中心坐标的位置
        /// </summary>
        private static Vector2D central = new Vector2D(0, 0);

        /// <summary>
        /// 当前的中心点，应该最小单位的毫米信息
        /// </summary>
        public static Vector2D Central
        {
            get
            {
                return central;
            }
        }
        /// <summary>
        /// 1像素对应的原始长度，单位毫米
        /// </summary>
        private static double pixelToSize = 1;



        //当前一像素代表的多少尺度,默认1像素为1毫米
        public static double PixelToSize
        {
            get
            {
                return pixelToSize;
            }
            set
            {
                pixelToSize = value;
            }
        }
        /// <summary>
        /// 当前容器的宽度
        /// </summary>
        public static double MeasureWidth
        {

            get;
            set;
        }
        /// <summary>
        /// 当前容器的参照高度
        /// </summary>
        public static double MeasureHeight
        {
            get;
            set;
        }


        /// <summary>
        /// 将屏幕上的像素转换为尺寸信息
        /// </summary>
        /// <returns></returns>
        public static Vector2D PixToMM(Point p)
        {
            //获取中心点的实际尺寸
            var mx = central.X + (p.X - MeasureWidth / 2) * pixelToSize;
            var my = central.Y + (-p.Y + MeasureHeight / 2) * pixelToSize;
            return Vector2D.Create(mx, my);

        }

        /// <summary>
        /// 将实际长度转换为像素信息
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Point MMToPix(Vector2D v)
        {
            var vx = (v.X - central.X) / pixelToSize + MeasureWidth / 2;
            var vy = (-v.Y + central.Y) / pixelToSize + MeasureHeight / 2;
            return new Point(vx, vy);
        }

        public static double MMToPix(double measure)
        {
            var mea = measure / pixelToSize;
            return mea;
        }


        /// <summary>
        /// 转换为像素
        /// </summary>
        /// <param name="measure"></param>
        /// <returns></returns>
        public static double PixToMM(double measure)
        {
            var mea = measure * pixelToSize;
            return mea;
        }




        /// <summary>
        /// 获取移动的距离
        /// </summary>
        /// <param name="ep"></param>
        /// <param name="sp"></param>
        public static void PanTo(Point ep, Point sp)
        {
            var e = PixToMM(ep);
            var s = PixToMM(sp);
            var mx = e.X - s.X;
            var my = e.Y - s.Y;
            central = new Vector2D(central.X - mx, central.Y - my);

        }

        /// <summary>
        /// 指定当前的中心位置
        /// </summary>
        /// <param name="v"></param>
        public static void Center(Vector2D v)
        {
            //每个像素点的偏移距离
            central = v;
        }
        /// <summary>
        /// 放大尺寸
        /// </summary>
        public static void ZoomIn(Point? p = null)
        {

            if (p != null)
            {
                //到中心点的坐标距离
                var dx = p.Value.X - MeasureWidth / 2;
                var dy = -p.Value.Y + MeasureHeight / 2;

                //当前鼠标点到中心点的距离
                var dxg = PixToMM(dx);
                var dyg = PixToMM(dy);


                pixelToSize = pixelToSize * 0.8;

                //获取缩放后，鼠标点到中心点的距离
                var dxg2 = PixToMM(dx);
                var dyg2 = PixToMM(dy);

                //则中心点需要移动的距离
                var mx = dxg - dxg2;
                var my = dyg - dyg2;

                central = new Vector2D(central.X + mx, central.Y + my);
            }
            else
            {
                pixelToSize = pixelToSize * 0.8;
            }

        }

        /// <summary>
        /// 缩小尺寸
        /// </summary>
        public static void ZoomOut(Point? p = null)
        {
            if (p != null)
            {
                //到中心点的坐标距离
                var dx = p.Value.X - MeasureWidth / 2;
                var dy = -p.Value.Y + MeasureHeight / 2;

                //当前鼠标点到中心点的距离
                var dxg = PixToMM(dx);
                var dyg = PixToMM(dy);


                pixelToSize = pixelToSize * 1.2;

                //获取缩放后，鼠标点到中心点的距离
                var dxg2 = PixToMM(dx);
                var dyg2 = PixToMM(dy);

                //则中心点需要移动的距离
                var mx = dxg - dxg2;
                var my = dyg - dyg2;

                central = new Vector2D(central.X + mx, central.Y + my);
            }
            else
            {
                pixelToSize = pixelToSize * 1.2;
            }

        }

        /// <summary>
        /// 获取两点之间的距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double GetDistance(Vector2D start, Vector2D end)
        {
            return end.Distance(start);
        }

        private static Random random = new Random();
        /// <summary>
        ///产生随机颜色
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColor()
        {
            return Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255));
        }


        private static bool canCatchEndPoint = true;
        public static bool CanCatchEndPoint
        {
            get
            {
                return canCatchEndPoint;
            }
            set
            {
                canCatchEndPoint = value;
            }
        }

        private static bool canCatchIntersect = true;
        public static bool CanCatchIntersect
        {
            get
            {
                return canCatchIntersect;
            }
            set
            {
                canCatchIntersect = value;
            }
        }
        private static bool canCatchCentral = false;
        public static bool CanCatchCentral
        {
            get
            {
                return canCatchCentral;
            }
            set
            {
                canCatchCentral = value;
            }
        }
        private static bool canCatchShift = false;
        public static bool CanCatchShift
        {
            get
            {
                return canCatchShift;
            }
            set
            {
                canCatchShift = value;
            }
        }


    }
}
