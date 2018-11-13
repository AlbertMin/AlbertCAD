using System;
using System.Collections.Generic;
using Albert.Geometry.Primitives;
using System.Linq;
namespace Albert.Geometry.External
{
    /// <summary>
    /// 用于计算一个多边形是否在另外一个多边形内部，或者部分在多边形内部
    /// </summary>
    public class LineInsideRegionAlgorithm
    {

        /// <summary>
        /// 检测当前线是否在另外一个多边形内部， 0图形相交，1表示在内部，-1表示在外部，2表示线在多边形的线上
        /// </summary>
        /// <param name="vector3D"></param>
        /// <param name="outLines"></param>
        /// <returns></returns>
        public int Check(Line2D cline, List<Line2D> outLines, out List<Line2D> innerLines)
        {

            innerLines = new List<Line2D>();

            if (cline.IsOnRegionEdge(outLines))
            {
                return 2;
            }

            //所有的去掉端点相交的集合
            List<Vector2D> intersects = new List<Vector2D>();


            for (int i = 0; i < outLines.Count; i++)
            {
                var intersect = outLines[i].Intersect2(cline);

                //添加所有的交点
                if (intersect != null)
                {
                    intersects.Add(intersect);
                }

            }
            //通过交点把线段打断成多个线段，只要判断这个线段的中点，是否在多边形内部，那么这个小线段就在内部
            //只有所有小线段在内部的，则整个线段在内部，只有整个线段在外部的，这些线段在外部，有在外部和内部的，则相交

            intersects.Add(cline.Start);
            intersects.Add(cline.End);

            List<Vector2D> orderIntersects = intersects.Distinct(new Vector2DEqualityComparer()).OrderBy(x=>x.Distance(cline.Start)).ToList();

            //获取多线段
            List<Line2D> mulitlines = new List<Line2D>();

            //排序所有的点
            for (int i = 1; i < orderIntersects.Count; i++)
            {
                Line2D line = Line2D.Create(orderIntersects[i - 1], orderIntersects[i]);
                mulitlines.Add(line);
            }
            int innerNum = 0, outerNum = 0,gonNum=0;

            foreach (Line2D x in mulitlines)
            {
                var rs= x.MiddlePoint.IsInRegion(outLines);
              
                //由于当前线段
                if (rs)
                {
                    innerLines.Add(x);
                    innerNum++;
                }
                else
                {
                    var rs2 = x.MiddlePoint.IsOnRegionEdge(outLines);
                    if (!rs2)
                    {
                        outerNum++;
                    }
                    //说明当前线段和多边形中的线段重合，这种线段，成为多边形内部的线段
                    else
                    {
                        gonNum++;
                    }
                }

            }

            if (innerNum+ gonNum == mulitlines.Count)
            {
                innerLines = null;
                return 1;
            }
            else if (outerNum+ gonNum == mulitlines.Count)
            {
                innerLines = null;
                return -1;
            }
            else {
                return 0;
            }
        }

    }

}
