using Albert.DrawingKernel.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Filter
{
    /// <summary>
    /// 定义一个选取的类型元素
    /// </summary>
    public interface IPickFilter
    {
        /// <summary>
        /// 允许选取
        /// </summary>
        /// <param name="geometryShape"></param>
        /// <returns></returns>
        bool AllowElement(Geometry2D g);

        /// <summary>
        /// 是否过滤
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        void AddElement(dynamic t);
    }
}
