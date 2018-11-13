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
    /// 当前算法，是将一个规则的矩形，按照更小的矩形尺寸进行划分
    /// </summary>
    public class PartitionRectanglesAlgorithm
    {
        /// <summary>
        /// 按照指定的高度和宽度，对规则矩形进行划分，当前算法，运行的前提条件是，传入的图形是矩形元素
        /// </summary>
        /// <param name="square"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public List<List<Line2D>> Partition(List<Line2D> square, double width, double height,Vector2D offset)
        {
            //根据板子宽度和高度，重新定义偏移量
             var loffset = this.LayoutOffset(width, height, offset);
            //获取要划分的矩形裕兴
            List<Line2D> lines = new List<Line2D>(square);

            //获取当前的划分多边形
            List<List<Line2D>> result = new List<List<Line2D>>();

            //获取图形中的最小点
            List<Vector2D> vst = new List<Vector2D>();

            square.ForEach(x =>
            {

                vst.Add(x.Start);
                vst.Add(x.End);
            });

            //排除相同点
            vst = vst.Distinct(new Vector2DEqualityComparer()).ToList();

            //查找最小角点
            var origin = vst.OrderBy(a =>
            {

                return Math.Round(a.X, 7);
            }).ThenBy(b =>
            {

                return Math.Round(b.Y, 7);
            }).FirstOrDefault();
            ////查找最小角点
            //var origin = vst.OrderBy(a => a.X).ThenBy(c => c.Y).FirstOrDefault();
            //逆时针排序,排列出
            List<Line2D> sequeeLines = GraphicAlgorithm.Sort(lines, origin, 1);

            //获取当前矩形的实际宽度和高度
            double sw = sequeeLines[0].Length + loffset.X;
            double sh = sequeeLines[1].Length + loffset.Y;

            //代表需要水平偏移的个数
            var lnx = Math.Ceiling(sw / width);
            //代表垂直偏移的个数
            var lny = Math.Ceiling(sh / height);

            for (int i = 0; i < lnx; i++)
            {
                for (int j = 0; j < lny; j++)
                {
                    Vector2D startPoint = null;
                    if (loffset != null) {

                        startPoint = origin - loffset;
                    }
                    //代表顶点的偏移
                    Vector2D v1 = startPoint.Offset(width * i * sequeeLines[0].Direction).Offset(height * j * sequeeLines[1].Direction);
                    
                    //最右边的顶点偏移
                    Vector2D v2 = startPoint.Offset(width * (i+1) * sequeeLines[0].Direction).Offset(height * j * sequeeLines[1].Direction);
                
                    //偏移Y方向的区域，假如超过边界，则取边界值
                    Vector2D v4 = startPoint.Offset(width * i * sequeeLines[0].Direction).Offset(height * (j+1) * sequeeLines[1].Direction);

                    if (v1.X < origin.X)
                    {
                        v1.X = origin.X;
                    }
                    if (v1.Y < origin.Y)
                    {
                        v1.Y = origin.Y;
                    }
                    //假如超出边界，则取边界值
                    if (v2.X > (origin.X + sequeeLines[0].Length))
                    {
                        v2 = Vector2D.Create(origin.X + sequeeLines[0].Length, v2.Y);
                    }

                    if (v2.Y < origin.Y)
                    {
                        v2.Y = origin.Y;
                    }
                    if (v4.Y > (origin.Y + sequeeLines[1].Length))
                    {
                        v4 = Vector2D.Create(v1.X, origin.Y + sequeeLines[1].Length);
                    }

                    if (v4.X < origin.X)
                    {
                        v4.X = origin.X;
                    }
                    if (v4.Y < origin.Y)
                    {
                        v4.Y = origin.Y;
                    }
                    //计算对角点
                    Vector2D v3 = Vector2D.Create(v2.X, v4.Y);
       
                
                    //获取划分的小多边形
                    List<Line2D> line2ds = new List<Line2D>() { 
                        Line2D.Create(v1,v2),
                        Line2D.Create(v2,v3),
                        Line2D.Create(v3,v4),
                        Line2D.Create(v4,v1)
                    };
                    result.Add(line2ds);
                }
            }
            return result;

        }

        /// <summary>
        /// 重新定义偏移量
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private Vector2D LayoutOffset(double w, double h, Vector2D offset) {
            var mx = offset.X % w;
            var my = offset.Y % h;
            if (mx < 0)
            {

                mx = mx + w;
            }
            if (my < 0)
            {

                my = my + h;
            }
            return new Vector2D(mx, my);
        }

        public List<List<Line3D>> Partition(List<Line3D> square, double width, double height)
        {

            //获取当前面的转换举证
            Matrix4 m = TransformUtil.GetMatrix(square);

            //将当前坐标转换到平面坐标
            List<Line3D> tf = TransformUtil.Transform(square, m);

            //计算投影
            List<Line2D> pf = TransformUtil.Projection(tf);


            List<List<Line2D>> partitions = Partition(pf, width, height,null);

             var rm=TransformUtil.GetInversetMatrix(m);


             //当前的结果
             List<List<Line3D>> result = new List<List<Line3D>>();

            //进行坐标转换
            partitions.ForEach(x => {

                List<Line3D> rt= TransformUtil.Projection(x,0);

                result.Add(TransformUtil.Transform(rt, rm));
            });

            return result;
        }
    }
}
