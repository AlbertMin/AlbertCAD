using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.DrawingKernel.Geometries;

namespace Albert.DrawingKernel.Filter
{
    public class TypeFilter : IPickFilter
    {

        private List<Type> allowElements = null;

  
        /// <summary>
        /// 指定过滤的类型元素
        /// </summary>
        /// <param name="geometryShape"></param>
        /// <returns></returns>
        public bool AllowElement(Geometry2D g)
        {
            if (allowElements != null)
            {
                Type t = g.GetType();
                //获取当前允许的元素
                var tp = allowElements.Find(x => x == t);

                if (tp != null)
                {
                    return true;
                }

                return false;
            }
            return false;

        }


        /// <summary>
        /// 假如是过滤的对象，则返回true,不是要过滤的对象，则返回false
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public void AddElement(dynamic t) {

            if (allowElements == null)
            {

                allowElements = new List<Type>();
            }

            allowElements.Add(t);
        }
    }
}
