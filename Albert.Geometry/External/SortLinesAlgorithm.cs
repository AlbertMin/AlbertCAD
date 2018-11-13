using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.Geometry.External
{
    /// <summary>
    /// 对二维、三维线段所有的排序算法
    /// </summary>
    public class SortLinesAlgorithm
    {

        /// <summary>
        /// 对传入的线段进行排序
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sortedLines"></param>
        public List<Line2D> Sort(List<Line2D> lines)
        {
            Vector2D sPoint = lines[0].Start;
            List<Line2D> sortLines = new List<Line2D>();
            SortByPoint(lines, sPoint, ref sortLines);
            return sortLines;
        }

        /// <summary>
        /// 对传入的线段，通过指定的点，进行排序
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sPoint"></param>
        /// <returns></returns>
        public List<Line2D> SortByPoint(List<Line2D> lines, Vector2D sPoint)
        {

            List<Line2D> sortLines = new List<Line2D>();
            SortByPoint(lines, sPoint, ref sortLines);
            return sortLines;
        }


        /// <summary>
        /// 对当前的线段进行反转
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public List<Line2D> Reversed(List<Line2D> lines)
        {
            List<Line2D> rlines = new List<Line2D>();

            lines.ForEach(x => {

                rlines.Add(x.Reversed());
            });
            return rlines;
        }

        /// <summary>
        /// 对当前的所有的线按照循序进行排序,假如direction大于0，则逆时针，小于0，则顺时针
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public List<Line2D> SortByDirection(List<Line2D> lines, int direction = 1)
        {
            Vector2D sPoint = lines[0].Start;
            return SortByDirection(lines, sPoint, direction);
        }

        /// <summary>
        /// 指定开始点，对当前线段进行按方向排序,direction代表排序的方向，正数代表逆时针，负数代表顺时针
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="direction"></param>
        /// <param name="sortedLines"></param>
        public List<Line2D> SortByDirection(List<Line2D> lines, Vector2D spoint, int direction = 1)
        {
            List<Line2D> sortlines = SortByPoint(lines, spoint);

            if (direction == 0) { return sortlines; }

            //获取所有的点
            List<Vector2D> vts = new List<Vector2D>();
            sortlines.ForEach(x => {

                vts.Add(x.Start);
            });

            //查找X坐标最大的点，也就是其中一个凸点
            var min = Double.MinValue;
            //找到X轴最大的坐标点
            Vector2D maxXV = null;
            for (int i = 0; i < vts.Count; i++)
            {

                if (vts[i].X > min)
                {
                    min = vts[i].X;
                    maxXV = vts[i];
                }
            }

            //查找这共享这个点的两条线

            Line2D p1 = sortlines.Find(x => x.End.IsAlmostEqualTo(maxXV));
            Line2D p2 = sortlines.Find(x => x.Start.IsAlmostEqualTo(maxXV));

            if (p1 != null && p2 != null)
            {
                var pd = p1.Direction.Cross(p2.Direction);
                //假如需求和结果相同，则直接返回结果
                if ((pd > 0 && direction > 0) || (pd < 0 && direction < 0))
                {
                    return sortlines;

                }
                //说明当前图形要重新进行排序
                else
                {

                    return Reversed(sortlines);
                }
            }
            else
            {
                //返回空
                return null;
            }
        }
        /// <summary>
        /// 指定开始点，对当前线段进行普通的排序
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="point"></param>
        /// <param name="sortedLines"></param>
        private void SortByPoint(List<Line2D> lines, Vector2D spoint, ref List<Line2D> sortedLines)
        {

            //确定的线
            Line2D confirmLine = null;

            //查找的线段
            Line2D searchLine = lines.Find(x => x.Start.IsAlmostEqualTo(spoint));

            if (searchLine != null)
            {
                confirmLine = searchLine;
            }
            else
            {
                searchLine = lines.Find(x => x.End.IsAlmostEqualTo(spoint));
                //反转当前直线，
                confirmLine = Line2D.Create(searchLine.End, searchLine.Start);

            }

            if (confirmLine != null)
            {
                sortedLines.Add(confirmLine);
                lines.Remove(searchLine);
                if (lines.Count > 0) {
                    SortByPoint(lines, confirmLine.End, ref sortedLines);
                }
        
            }

        }




        /// <summary>
        /// 对传入的线段进行排序
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sortedLines"></param>
        public List<Line3D> Sort(List<Line3D> lines)
        {
            Vector3D sPoint = lines[0].Start;
            List<Line3D> sortLines = new List<Line3D>();
            SortByPoint(lines, sPoint, ref sortLines);
            return sortLines;
        }

        /// <summary>
        /// 对传入的线段，通过指定的点，进行排序
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sPoint"></param>
        /// <returns></returns>
        public List<Line3D> SortByPoint(List<Line3D> lines, Vector3D sPoint)
        {
            List<Line3D> sortLines = new List<Line3D>();
            SortByPoint(lines, sPoint, ref sortLines);
            return sortLines;
        }


        /// <summary>
        /// 对当前的线段进行反转
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public List<Line3D> Reversed(List<Line3D> lines)
        {
            List<Line3D> rlines = new List<Line3D>();

            lines.ForEach(x => {

                rlines.Add(x.Reversed());
            });
            return rlines;
        }

        /// <summary>
        /// 对当前的所有的线按照循序进行排序,假如direction大于0，则逆时针，小于0，则顺时针
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public List<Line3D> SortByDirection(List<Line3D> lines, Vector3D direction)
        {
            Vector3D sPoint = lines[0].Start;
            return SortByDirection(lines, sPoint, direction);
        }

        /// <summary>
        /// 指定开始点，对当前线段进行按方向排序,direction代表排序的方向，正数代表逆时针，负数代表顺时针
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="direction"></param>
        /// <param name="sortedLines"></param>
        public List<Line3D> SortByDirection(List<Line3D> lines, Vector3D spoint, Vector3D direction)
        {
            List<Line3D> sortlines = SortByPoint(lines, spoint);

            //获取所有的点
            List<Vector3D> vts = new List<Vector3D>();
            sortlines.ForEach(x => {

                vts.Add(x.Start);
            });

            //查找X坐标最大的点，也就是其中一个凸点
            var min = Double.MinValue;
            //找到X轴最大的坐标点
            Vector3D maxXV = null;
            for (int i = 0; i < vts.Count; i++)
            {

                if (vts[i].X > min)
                {
                    min = vts[i].X;
                    maxXV = vts[i];
                }
            }

            //查找这共享这个点的两条线

            Line3D p1 = sortlines.Find(x => x.End.IsAlmostEqualTo(maxXV));
            Line3D p2 = sortlines.Find(x => x.Start.IsAlmostEqualTo(maxXV));

            if (p1 != null && p2 != null)
            {
                var pd = p1.Direction.Cross(p2.Direction);

                if (pd.Normalize().IsAlmostEqualTo(direction))
                {

                    return sortlines;
                }
                else
                {

                    return Reversed(sortlines);
                }
            }
            else
            {
                //返回空
                return null;
            }
        }
        /// <summary>
        /// 指定开始点，对当前线段进行普通的排序
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="point"></param>
        /// <param name="sortedLines"></param>
        private void SortByPoint(List<Line3D> lines, Vector3D spoint, ref List<Line3D> sortedLines)
        {
            //确定的线
            Line3D confirmLine = null;

            //查找的线段
            Line3D searchLine = lines.Find(x => x.Start.IsAlmostEqualTo(spoint));

            if (searchLine != null)
            {
                confirmLine = searchLine;
            }
            else
            {
                searchLine = lines.Find(x => x.End.IsAlmostEqualTo(spoint));
                //反转当前直线，
                confirmLine = Line3D.Create(searchLine.End, searchLine.Start);

            }

            if (confirmLine == null)
            {
                sortedLines = null;
                return;
            }
            else
            {

                sortedLines.Add(confirmLine);
                lines.Remove(searchLine);

                SortByPoint(lines, confirmLine.End, ref sortedLines);
            }
        }

    }
}
