using Albert.Geometry.Primitives;
using Albert.Geometry.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.Geometry.External
{
    /// <summary>
    /// 多边形的弹性算法，用于计算图形的扩张与图形的收缩的主要算法
    /// </summary>
    public class ElasticityAlgorithm
    {

        /// <summary>
        /// 图形的扩张与收缩,存在BUG，需要再处理
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public List<Line3D> Elastic(List<Line3D> lines,double range,List<Line3D> neverOffset)
        {

            List<Line3D> NLList = new List<Line3D>();
            //获取当前线段的顺时针排序
            List<Line3D> sortLines = GraphicAlgorithm.Sort(lines);

            //获取当前连续的顶点
            List<Vector3D> PList = new List<Vector3D>();

            //当前直线的方向
            List<Vector3D> DList = new List<Vector3D>();
            //记录所有的点
            sortLines.ForEach(x => {
                PList.Add(x.Start);
                DList.Add(x.Direction);
            });

            //所有点的列表
            List<Vector3D> NPList = new List<Vector3D>();

            int startIndex, endindex;
            //获取所有的点
            for (int i = 0; i < DList.Count; i++)
            {

                startIndex = i == 0 ? DList.Count - 1 : i - 1;
                endindex = i;
                //两个线之间的夹角
                double sina = DList[startIndex].Cross(DList[endindex]).Modulus();

                //由于偏移出是平行四边形，则要移动的长度为
                var moveLen = -range / sina;

                //移动的方向为
                var movedir = DList[endindex] - DList[startIndex];

                Vector3D np = DList[i] + movedir * moveLen;
                //添加新点
                NPList.Add(np);
            }

            ///形成新的线
            for (int i = 0; i < NPList.Count; i++)
            {
                var index = i == (DList.Count - 1) ? 0 : i + 1;
                NLList.Add(Line3D.Create(NPList[i], NPList[index]));
            }

            return NLList;
        }


        /// <summary>
        /// 对多边形进行扩张和缩放，range为负数为内缩小，为正数为外扩
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Line2D> Elastic(List<Line2D> lines, double range,List<Line2D> neverOffset, bool allowDistortion = false)
        {

            if (range < 0)
            {
                //说明会引起多边形形状的变化
                Line2D line = lines.Find(x => x.Length < Math.Abs(range));

                //说明图形已经边型了，需要变形处理
                if (line != null && allowDistortion)
                {
                    //允许变形的缩放
                    this.ElasticDistortion(lines, range);
                }
            }


            List<Line2D> NLList = new List<Line2D>();
            //首先需要对线段进行一次合并，由于多边形可能存在共线的情况，那么偏移会出问题
            var mergelines = GraphicAlgorithm.Merge(lines);
            //获取当前线段的顺时针排序
            List<Line2D> sortLines = GraphicAlgorithm.Sort(mergelines, null, 1);

            //获取当前连续的顶点
            List<Vector2D> PList = new List<Vector2D>();

            //当前直线的方向
            List<Vector2D> DList = new List<Vector2D>();
            //记录所有的点
            sortLines.ForEach(x => {
                PList.Add(x.Start);
                DList.Add(x.Direction);
            });

            //所有点的列表
            List<Vector2D> NPList = new List<Vector2D>();

            int startIndex, endindex;
            //获取所有的点
            for (int i = 0; i < DList.Count; i++) {

     
                startIndex = i == 0 ? DList.Count - 1 : i - 1;
                endindex = i;
                //两个线之间的夹角
                double sina = DList[startIndex].Cross(DList[endindex]);
                //用于判断当前是阴阳角
                var Corndir = 0;

                if (sina > 0)
                {
                    Corndir = 1;
                }
                else {

                    Corndir = -1;
                }
                //由于偏移出是平行四边形，则要移动的长度为
                var moveLen = -range / sina;

                Vector2D movedir = null;
                if (neverOffset != null)
                {

                    var nf = neverOffset.FindAll(x => x.Start.IsAlmostEqualTo(PList[i]) || x.End.IsAlmostEqualTo(PList[i]));

                    if (nf != null && nf.Count == 1)
                    {
                        if (nf[0].Direction.IsAlmostEqualTo(DList[startIndex]) || nf[0].Direction.IsAlmostEqualTo(-DList[startIndex]))
                        {

                            movedir = DList[startIndex];
                            Vector2D npt = PList[i] + movedir * range * Corndir;
                            NPList.Add(npt);
                        }
                        else
                        {

                            movedir = -DList[endindex];
                            Vector2D npt = PList[i] + movedir * range * Corndir;
                            NPList.Add(npt);
                        }

                        continue;
                    }
                    if (nf != null && nf.Count == 2)
                    {

                        NPList.Add(PList[i]);
                        continue;
                    }
       
                }
                //移动的方向为
                movedir = DList[endindex] - DList[startIndex];
                Vector2D np = PList[i] + movedir * moveLen;
                //添加新点
                NPList.Add(np);
            }

            ///形成新的线
            for (int i = 0; i < NPList.Count; i++)
            {
                var index = i == (NPList.Count - 1) ? 0 : i + 1;
                NLList.Add(Line2D.Create(NPList[i], NPList[index]));
            }

            return NLList;
        }


        /// <summary>
        /// 对指定的进行每个线段进行缩放，并且指定缩放的长度,传入的线，不能共线或者重叠，否则会出现错误
        /// </summary>
        /// <param name="nlist"></param>
        /// <returns></returns>
        public List<Line2D> Elastic2(List<Tuple<Line2D, Double>> nlist) {

            List<Line2D> lines = new List<Line2D>();
            //不处理变形的情况
            foreach (var tp in nlist) {

                if (tp.Item1.Length < tp.Item2)
                {
                    return null;
                }
                else {

                    lines.Add(tp.Item1);
                }
            }

            //获取当前线段的顺时针排序
            List<Line2D> sortLines = GraphicAlgorithm.Sort(lines, null, 1);

            List<Line2D> moveLines = new List<Line2D>();
            ///开始线段偏移
            for (int i = 0; i < sortLines.Count; i++) {
                var v1 = Vector3D.Create(sortLines[i].Start.X, sortLines[i].Start.Y, 0);
                var v2 = Vector3D.Create(sortLines[i].End.X, sortLines[i].End.Y, 0);
                var l1 = Line3D.Create(v1, v2);
                var moveDir = l1.Direction.Cross(Vector3D.BasisZ);
                var tp = nlist.Find(x => x.Item1.IsAlmostEqualTo(sortLines[i]));
                var nl = l1.Offset(tp.Item2, -moveDir);
                moveLines.Add(TransformUtil.Projection(nl));
            }
            List<Vector2D> NPList = new List<Vector2D>();
            //开始循环所有的线段
            for (int i = 0; i < moveLines.Count; i++)
            {
                Vector2D v = null;
                if (i == 0)
                {
                    
                    v = moveLines[0].IntersectStraightLine(moveLines[moveLines.Count - 1]);
                }
                else {

                    v = moveLines[i].IntersectStraightLine(moveLines[i - 1]);
                }

                if (v == null)
                {

                    return null;
                }
                else {
                    NPList.Add(v);
                }

            }

            List<Line2D> nliset = new List<Line2D>();
            //生成新的多边形
            for (int i = 0; i < NPList.Count; i++)
            {
                if (i == 0)
                {
                    nliset.Add(Line2D.Create(NPList[NPList.Count - 1], NPList[i]));
                }
                else
                {
                    nliset.Add(Line2D.Create(NPList[i - 1], NPList[i]));
                }

            }

            return nliset;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private List<Line2D> ElasticDistortion(List<Line2D> lines, double range) {

            return null;
        }

        public List<Polygon3D> Elastic(Polygon3D polygon3D)
        {

            return null;
        }

        public List<Polygon2D> Elastic(Polygon2D polygon2D)
        {

            return null;
        }

     

    }
}
