using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.Geometry.External
{
    /// <summary>
    /// 算法描述： 在一个正多边形的内部，不允许出现尖角对象，则需要按照一定的规则，将多边形切分成多个规则的矩形元素.
    /// 算法约束：所有当前算法的前提条件是，所有的角度必须是直角，不允许出现非直角
    /// </summary>
    public class CornerDivisionAlgorithm
    {
        /// <summary>
        /// 进行矩形划分出任意多个矩形元素
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public List<List<Line2D>> Division(List<Line2D> lines) {

            if (lines != null && lines.Count > 4)
            {

                //首先要把所有的线打断
                List<Line2D> decomposesLines = GraphicAlgorithm.Decompose(lines);
                //查找最大的封闭区域,且不需要打断
                List<List<Line2D>> large = GraphicAlgorithm.ClosedLookup(decomposesLines, true, false);

                if (large != null && large.Count > 0)
                {

                    //获取所有外部的线段
                    List<Line2D> outerSide = large.FirstOrDefault();

                    //获取所有无端点的线段
                    List<Line2D> weeds = GraphicAlgorithm.WeedLess(decomposesLines);

                    //获取内部线段，要去掉
                    //List<Line2D> innerSide = GraphicAlgorithm.RejectLines(decomposesLines,);
                }
            }
            return null;
        }



        /// <summary>
        /// 数据模型
        /// </summary>
        internal class LineModel
        {

            public LineModel(Line2D l, int useage, LineType type)
            {

                this.Line = l;
                this.UseAge = useage;
                this.LineType = type;
            }
            /// <summary>
            /// 线的信息
            /// </summary>
            public Line2D Line
            {

                get; set;
            }
            /// <summary>
            /// 使用次数
            /// </summary>
            public int UseAge
            {

                set; get;
            }

            public LineType LineType { set; get; }
        }

        internal enum LineType
        {

            outer = 0,
            inner = 1,
            Separate = 2,
            Assist = 3
        }
    }
}
