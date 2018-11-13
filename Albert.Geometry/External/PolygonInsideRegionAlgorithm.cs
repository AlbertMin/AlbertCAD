using System;
using System.Collections.Generic;
using Albert.Geometry.Primitives;

namespace Albert.Geometry.External
{
    /// <summary>
    /// 用于计算一个多边形是否在另外一个多边形内部，或者部分在多边形内部
    /// </summary>
    public class PolygonInsideRegionAlgorithm
    {
        /// <summary>
        /// 对三维坐标进行转换,0表示点在边上，1表示在内部，-1表示在外部
        /// </summary>
        /// <param name="vector3D"></param>
        /// <param name="outLines"></param>
        /// <returns></returns>
        public int Check(List<Line3D> clines, List<Line3D> outLines)
        {

            return 0;
        }


        /// <summary>
        /// 检测当前多边形和另外一个多边形的关系，
        /// 0图形相交
        /// 1 表示包容，返回较小的多边形区域，为共有区域
        /// -1 表示相离
        /// </summary>
        /// <param name="clines">需要检测的多边形</param>
        /// <param name="outLines">目标多边形</param>
        /// <param name="innerLines">相交的区域</param>
        /// <returns></returns>
        public int Check(List<Line2D> clines, List<Line2D> outLines, out List<List<Line2D>> innerLines)
        {
            int ins = 0, ous = 0, gon = 0;

            //相交的区域
            innerLines = new List<List<Line2D>>();

            //循环当前所有的线段
            for (int i = 0; i < clines.Count; i++)
            {
                //当前线段是否在指定多边形内部
                var result = GraphicAlgorithm.LineInsideOfRegion(clines[i], outLines, null);

                //假如是内部，则内部数量+1
                if (result == 1 )
                {
                    ins++;
                }
                //假如在外部，则外部数量+1
                else if (result == -1)
                {
                    ous++;
                }
                else if (result == 2)
                {
                    gon++;
                }
                //否则有相交区域，则不记录
                else
                {
                    break;
                }
            }
            //都在内部，说明当前图形被包含在指定图形内部
            if ((ins+gon)== clines.Count)
            {
                innerLines.Add(clines);
                return 1;
            }

            //全部在外部，有两种情况，一种是两个图形分离，一种是当前图形包含了指定图形
            else if ((ous+gon) == clines.Count)
            {
                //开始检测两个之间的关系，找到目标图形的一个线，判断在当前图形的关系
                var r = GraphicAlgorithm.LineInsideOfRegion(outLines[0], clines, null);
                //说明当前outLines就是在clines的内部
                if (r == 1)
                {
                    innerLines.Add(outLines);
                    return 1;
                }
                //两个图形属于相离的状态
                else
                {
                    innerLines = null;
                    return -1;
                }
            }
            //计算没有记录
            else
            {
                //没有找到一个相交的线，说明两个图形相连
                if (ins + ous+gon == clines.Count)
                {
                    innerLines = null;
                    return -1;
                }
                //说明图形确实相交，需要获取相交区域
                else
                {
                    innerLines = GetSuperposition(clines, outLines);
                    return 0;
                }

            }

        }


        /// <summary>
        /// 获取重合区域的图形
        /// </summary>
        /// <param name="clines"></param>
        /// <param name="outLines"></param>
        /// <returns></returns>
        private List<List<Line2D>> GetSuperposition(List<Line2D> clines, List<Line2D> outLines)
        {
            List<Line2D> mgslines = new List<Line2D>(clines);
            mgslines.AddRange(outLines);
            //查找到所有最小的封闭区域
            var closelines = GraphicAlgorithm.ClosedLookup(mgslines, false, true);

            List<List<Line2D>> closeIntersets = new List<List<Line2D>>();
            //只要指定区域的一个点，不在任何一个给定的图形内部，则剔除
            closelines.ForEach(x => {
                List<List<Line2D>> nlines = null;
                int r1 = Check(x, clines, out nlines);
                int r2 = Check(x, outLines, out nlines);

                if (r1 == 1 && r2 == 1)
                {

                    closeIntersets.Add(x);
                }
            });

            return closeIntersets;
        }

    }

}
