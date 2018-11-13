using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.Geometry.External
{
    /// <summary>
    /// 用于比较多边形是否相等
    /// </summary>
    public class ComparisonPolygonAlgorithm
    {


        /// <summary>
        /// 比较两个多边形是否相等
        /// </summary>
        /// <param name="lines1"></param>
        /// <param name="lines2"></param>
        /// <returns></returns>
        public bool Compare(List<Line2D> lines1, List<Line2D> lines2)
        {
            if (lines1.Count == lines2.Count)
            {
                //先注册两个对应的集合
                List<Line2D> nlist1 = new List<Line2D>(lines1);
                List<Line2D> nlist2 = new List<Line2D>(lines2);

                for (int i = 0; i < nlist1.Count; i++)
                {

                    var line = nlist1[i];
                    //找到的线
                    var fline = nlist2.Find(x => x.IsAlmostEqualTo(line));

                    if (fline == null)
                    {

                        return false;
                    }

                    nlist2.Remove(fline);
                }

                if (nlist2.Count == 0)
                {

                    return true;
                }

            }
            return false;
        }
    }
}
