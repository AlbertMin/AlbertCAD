using Albert.Geometry.Primitives;
using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Util
{
   public class BindingBox
    {
        public BindingBox(Vector2D min, Vector2D max)
        {

            this.Min = min;
            this.Max = max;
            Adjustment();
        }
        /// <summary>
        /// 边界的最大值，代表右下
        /// </summary>
        public Vector2D Max
        {

            get;
            set;
        }
        //边界的最小值，代表左上
        public Vector2D Min
        {
            get;
            set;
        }


       /// <summary>
       /// 直接定位到合适区域
       /// </summary>
        public void Adjustment()
        {
            var mx = Max.X - Min.X;
            var my = Max.Y - Min.Y;
            var sx = mx / KernelProperty.MeasureWidth;
            var sy = my / KernelProperty.MeasureHeight;
            KernelProperty.PixelToSize = Math.Max(sx, sy);
            KernelProperty.Center(new Vector2D((Max.X + Min.X) / 2, (Max.Y + Min.Y) / 2));
        }

    }
}
