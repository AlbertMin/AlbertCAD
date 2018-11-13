using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.Geometry.External
{
    /// <summary>
    /// 当前算法，是对界面上的直线图形进行合并操作，让在一条线上的元素合并成完整的直线
    /// </summary>
    public class MergeLinesAlgorithm
    {

        public List<Line3D> Merge(List<Line3D> lines) {

            if (lines != null && lines.Count > 0)
            {
                List<Line3D> findLines = new List<Line3D>(lines);
                List<Line3D> result = new List<Line3D>();
                Line3D startLine = findLines[0];
                //移除需要查找的线
                findLines.Remove(startLine);
                this.FindAndMerge(findLines, startLine, result);
                return result;
            }

            return null;
        }

        /// <summary>
        /// 用于合并线段，具有以下特征的线需要合并，方向相同或者相反，且至少共有一个点
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public List<Line2D> Merge(List<Line2D> lines) {

            if (lines != null && lines.Count > 0) {
                //首先合并相同的线段
                List<Line2D> mlines = lines.Distinct(new Line2DEqualityComparer()).ToList();
                //开始排序查找
                List<Line2D> findLines = mlines.OrderBy(x=>x.Length).ToList();
                

                List<Line2D> result = new List<Line2D>();
                Line2D startLine = findLines[0];
               
                //移除需要查找的线
                findLines.Remove(startLine);
                this.FindAndMerge(findLines, startLine, result);
                return result;
            }

            return null;
        }


        /// <summary>
        /// 查找合并线段
        /// </summary>
        /// <param name="remainLines"></param>
        /// <param name="searchLine"></param>
        /// <param name="result"></param>
        private void FindAndMerge(List<Line2D> remainLines, Line2D searchLine, List<Line2D> result) {

            //查找可以合并的线
            Line2D findLine = null;

            for (int i = 0; i < remainLines.Count; i++) {

                if (remainLines[i].Direction.IsAlmostEqualTo(searchLine.Direction) || remainLines[i].Direction.IsAlmostEqualTo(-searchLine.Direction))
                {
                    if (searchLine.Start.IsOnLine(remainLines[i]) || searchLine.End.IsOnLine(remainLines[i]) || searchLine.IsPartOf(remainLines[i])|| remainLines[i].IsPartOf(searchLine) || searchLine.IsAlmostEqualTo(remainLines[i]))
                    {
                        findLine = remainLines[i];
                        //合并当前线段
                        break;
                    }

                }
            }

        

            if (findLine == null)
            {
                result.Add(searchLine);
        
                if (remainLines.Count > 0)
                {
                    var nsearchLine = remainLines[0];
                    remainLines.Remove(nsearchLine);
                    FindAndMerge(remainLines, nsearchLine, result);
                }
            }
            else
            {
                if (searchLine.IsPartOf(findLine)|| searchLine.IsAlmostEqualTo(findLine)) {

                    var nsearchLine = remainLines[0];
                    remainLines.Remove(nsearchLine);
                    FindAndMerge(remainLines, nsearchLine, result);
                }
                else
                {

                    var nsearchLine = MergeLine(searchLine, findLine);
                    if (nsearchLine != null)
                    {
                        remainLines.Remove(findLine);
                        FindAndMerge(remainLines, nsearchLine, result);
                    }
                    else
                    {
                        result.Add(searchLine);
                        if (remainLines.Count > 0)
                        {
                            nsearchLine = remainLines[0];
                            remainLines.Remove(nsearchLine);
                            FindAndMerge(remainLines, nsearchLine, result);
                        }
                    }

                }
    

            }
        }


        /// <summary>
        /// 查找合并的线段
        /// </summary>
        /// <param name="remainLines"></param>
        /// <param name="searchLine"></param>
        /// <param name="result"></param>
        private void FindAndMerge(List<Line3D> remainLines, Line3D searchLine, List<Line3D> result)
        {

            //查找可以合并的线
            Line3D findLine = null;

            for (int i = 0; i < remainLines.Count; i++)
            {

                if (remainLines[i].Direction.IsAlmostEqualTo(searchLine.Direction) || remainLines[i].Direction.IsAlmostEqualTo(-searchLine.Direction))
                {
                    if (searchLine.Start.IsOnLine(remainLines[i]) || searchLine.End.IsOnLine(remainLines[i]))
                    {
                        findLine = remainLines[i];
                        //合并当前线段
                        break;
                    }

                }
            }

            if (findLine == null)
            {
                result.Add(searchLine);

                if (remainLines.Count > 0)
                {
                    var nsearchLine = remainLines[0];
                    remainLines.Remove(nsearchLine);
                    FindAndMerge(remainLines, nsearchLine, result);
                }
            }
            else
            {
                var nsearchLine = MergeLine(searchLine, findLine);
                if (nsearchLine != null)
                {
                    remainLines.Remove(findLine);
                    FindAndMerge(remainLines, nsearchLine, result);
                }
                else
                {
                    result.Add(searchLine);
                    if (remainLines.Count > 0)
                    {
                        nsearchLine = remainLines[0];
                        remainLines.Remove(nsearchLine);
                        FindAndMerge(remainLines, nsearchLine, result);
                    }
                }

            }
        }

        /// <summary>
        /// 对两个线进行合并
        /// </summary>
        /// <param name="L1"></param>
        /// <param name="L2"></param>
        /// <returns></returns>
        private Line2D MergeLine(Line2D L1, Line2D L2)
        {

            if (L1.Direction.IsAlmostEqualTo(L2.Direction) || L1.Direction.IsAlmostEqualTo(-L2.Direction))
            {

                if (L1.Start.IsOnLine(L2) && L1.End.IsOnLine(L2))
                {

                    return L2;
                }
                if (L2.Start.IsOnLine(L1) && L2.End.IsOnLine(L1)) {

                    return L1;
                }
                //假如L1的起点在L2上，则说明远端点位L1的终点
                if (L1.Start.IsOnLine(L2))
                {
                    //两个都为起点
                    if (L2.Start.IsOnLine(L1))
                    {

                        return Line2D.Create(L1.End, L2.End);
                    }
                    else
                    {

                        return Line2D.Create(L1.End, L2.Start);
                    }
                }
                else
                {
                    if (L2.Start.IsOnLine(L1))
                    {
                        return Line2D.Create(L1.Start, L2.End);
                    }
                    else
                    {

                        return Line2D.Create(L1.Start, L2.Start);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 对两个线进行合并
        /// </summary>
        /// <param name="L1"></param>
        /// <param name="L2"></param>
        /// <returns></returns>
        private Line3D MergeLine(Line3D L1, Line3D L2)
        {

            if (L1.Direction.IsAlmostEqualTo(L2.Direction) || L1.Direction.IsAlmostEqualTo(-L2.Direction))
            {
                if (L1.Start.IsOnLine(L2) && L1.End.IsOnLine(L2) || L2.Start.IsOnLine(L1) && L2.End.IsOnLine(L1))
                {


                }

                //假如L1的起点在L2上，则说明远端点位L1的终点
                else if (L1.Start.IsOnLine(L2))
                {
                    //两个都为起点
                    if (L2.Start.IsOnLine(L1))
                    {

                        return Line3D.Create(L1.End, L2.End);
                    }
                    else
                    {

                        return Line3D.Create(L1.End, L2.Start);
                    }
                }
                else
                {
                    if (L2.Start.IsOnLine(L1))
                    {
                        return Line3D.Create(L1.Start, L2.End);
                    }
                    else
                    {

                        return Line3D.Create(L1.Start, L2.Start);
                    }
                }
            }
            return null;
        }
    }
}
