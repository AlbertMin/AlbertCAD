using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Albert.Geometry.External;
using System.Windows;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Util;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Selector
{
    public class SelectionBoxCatch
    {


        /// <summary>
        /// 当前的绘制对象
        /// </summary>
        private DrawingControl DrawingControl
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数，初始化当前兴趣点捕获
        /// </summary>
        public SelectionBoxCatch(DrawingControl dc)
        {

            DrawingControl = dc;

        }

        //选择框
        private SelectingBox sg = new SelectingBox();


        /// <summary>
        /// 开始绘制
        /// </summary>
        /// <param name="v"></param>
        public void SelectStart(Vector2D v)
        {

            sg.Start = v;
            sg.End = v;
            sg.FillColor = Colors.CadetBlue;
            sg.PenColor = Colors.OrangeRed;
            sg.BrushOpacity = 0.5;
            this.DrawingControl.AddTemporaryVisual(sg);

        }

        /// <summary>
        /// 拖拽选择
        /// </summary>
        /// <param name="v"></param>
        public void Select(Vector2D v)
        {
            if (sg.Start != null)
            {
                sg.End = v;
                sg.Update();

            }

        }

        /// <summary>
        /// 鼠标弹起，表示选取结束
        /// </summary>
        /// <returns></returns>
        public List<IntersectGeometry> SelectEnd()
        {
            List<IntersectGeometry> selects = null;
            //检查选取点
            selects = this.SelectCheck(sg.Start, sg.End);

            sg.Start = Vector2D.Zero;
            sg.End = Vector2D.Zero;
            //更新选取
            sg.Update();
            //移除当前元素
            this.DrawingControl.RemoveTemporaryVisual(sg);
            //返回选取
            return selects;
        }

        /// <summary>
        /// 选择碰撞
        /// </summary>
        /// <returns></returns>
        private List<IntersectGeometry> SelectCheck(Vector2D start, Vector2D end)
        {
            List<IntersectGeometry> selects = new List<IntersectGeometry>();
            if (start.IsAlmostEqualTo(end))
            {
                //单个对象选择
                selects = this.SingleSelect(start);
                return selects;
            }
            else
            {
                //多个对象选择
                selects = this.MuiltSelect(start, end);
                return selects;
            }
        }

        /// <summary>
        /// 开始单选
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public List<IntersectGeometry> SingleSelect(Vector2D v)
        {
            //选择的所有图形元素
            List<IntersectGeometry> selectIGS = new List<IntersectGeometry>();
            //获取图形上所有的元素
            List<Geometry2D> gss = this.DrawingControl.GetDrawingVisuals().FindAll(x => x.IsEnabled);

            //当相交的图形
            List<IntersectGeometry> shapeSelects = new List<IntersectGeometry>();
            //循环所有的图形元素
            for (int i = 0; i < gss.Count; i++)
            {
                //获取满足相交运算的所有图形
                IntersectPoint ip = gss[i].IntersectPoint(v);

                if (ip != null)
                {
                    IntersectGeometry igip = new IntersectGeometry();
                    igip.IntersectPoint = ip;
                    igip.GeometryShape = gss[i];
                    shapeSelects.Add(igip);
                }

            }
            //查找一个离点击点最近的图形
            if (shapeSelects.Count > 0)
            {
                //查找一个最近的图形
                var selectShape = shapeSelects.OrderBy(x => x.IntersectPoint.Point.Distance(v)).FirstOrDefault();

                //查找具有相同距离的图形
                List<IntersectGeometry> igs = shapeSelects.FindAll(x => x.IntersectPoint.Point.Distance(v).AreEqual(selectShape.IntersectPoint.Point.Distance(v)));

                if (igs.Count == 1)
                {
                    selectIGS.Add(selectShape);

                    return selectIGS;
                }
                else
                {
                    /**需要判断，当前选择点是否在图形内部，假如直接命中了图形获取选择的点*/
                    Point point = KernelProperty.MMToPix(v);
                    var hittestShapes = DrawingControl.CatchVisual(point).FindAll(x => x.IsEnabled);

                    if (hittestShapes != null && hittestShapes.Count > 0)
                    {
                        //假如有多个命中，则要在命中中分离，也有相交关系的图形
                        List<IntersectGeometry> csigs = new List<IntersectGeometry>();
                        hittestShapes.ForEach(x =>
                        {

                            IntersectGeometry ig = igs.Find(y => y.GeometryShape == x);
                            if (ig != null)
                            {
                                csigs.Add(ig);
                            }
                        });

                        //假如有相交元素中，没有一个被命中，说明当前点没有命中任何相交元素，则还是选择弹出选择
                        if (csigs != null && csigs.Count > 0)
                        {
                            //多个命中，多个选择
                            return csigs;

                        }
                        else
                        {
                            //弹出选择
                            return igs;
                        }

                    }
                    else
                    {
                        //弹出选择
                        return igs;
                    }
                }

            }
            else
            {
                //获取选择的点
                Point point = KernelProperty.MMToPix(v);
                //假如直接命中了图形
                var selectedShapes = DrawingControl.CatchVisual(point).FindAll(x => x.IsEnabled);
                if (selectedShapes != null && selectedShapes.Count > 0)
                {

                    List<IntersectGeometry> csigs = new List<IntersectGeometry>();

                    selectedShapes.ForEach(x =>
                    {
                        IntersectGeometry igip = new IntersectGeometry();
                        IntersectPoint ip = new IntersectPoint();
                        ip.Point = v;
                        igip.IntersectPoint = ip;
                        igip.GeometryShape = x;
                        csigs.Add(igip);

                    });

                    return csigs;

                }
                else
                {
                    return selectIGS;
                }
            }

        }




        /// <summary>
        /// 多选操作
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<IntersectGeometry> MuiltSelect(Vector2D start, Vector2D end)
        {

            List<IntersectGeometry> selects = new List<IntersectGeometry>();
            List<Geometry2D> sgs = this.DrawingControl.GetDrawingVisuals();
            Vector2D v1 = start;
            Vector2D v3 = end;
            Vector2D v2 = Vector2D.Create(v3.X, v1.Y);
            Vector2D v4 = Vector2D.Create(v1.X, v3.Y);
            Line2D l1 = Line2D.Create(v1, v2);
            Line2D l2 = Line2D.Create(v2, v3);
            Line2D l3 = Line2D.Create(v3, v4);
            Line2D l4 = Line2D.Create(v4, v1);
            List<Line2D> region = new List<Line2D>() { l1, l2, l3, l4 };

            //下拉
            if (v1.X < v3.X)
            {
                for (int i = 0; i < sgs.Count; i++)
                {
                    //必须所有的线都在选中框内部
                    List<Line2D> lines = sgs[i].Lines;
                    bool isInRegion = false;
                    if (lines != null)
                    {

                        for (int j = 0; j < lines.Count; j++)
                        {
                            isInRegion = true;
                            if (!lines[j].IsInRegion(region))
                            {
                                isInRegion = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (sgs[i] is CircleGeometry)
                        {
                            var circle = sgs[i] as CircleGeometry;

                            //当前中心点在矩形区域之内
                            if (circle.Start.IsInRegion(region))
                            {
                                isInRegion = true;
                            }
                            for (int j = 0; j < region.Count; j++)
                            {
                                if (circle.Start.DistanceTo(region[j]) < circle.Radius)
                                {
                                    isInRegion = false;
                                }
                            }
                        }

                    }


                    if (isInRegion)
                    {
                        IntersectGeometry ig = new IntersectGeometry();
                        ig.GeometryShape = sgs[i];
                        selects.Add(ig);
                    }
                }

            }
            else
            {
                for (int i = 0; i < sgs.Count; i++)
                {
                    //主要部分在其中，则选中
                    List<Line2D> lines = sgs[i].Lines;
                    bool isInRegion = false;
                    if (lines != null)
                    {

                        for (int j = 0; j < lines.Count; j++)
                        {
                            if (lines[j].IsInRegion(region) || lines[j].IsPartInRegion(region))
                            {
                                isInRegion = true;
                                break;
                            }
                        }
                    }
                    else
                    {

                        if (sgs[i] is CircleGeometry)
                        {
                            var circle = sgs[i] as CircleGeometry;
                            for (int j = 0; j < region.Count; j++)
                            {
                                if (circle.Start.DistanceTo(region[j]) < circle.Radius)
                                {
                                    isInRegion = true;
                                }
                            }
                        }

                        if (sgs[i] is Geometries.Primitives.EllipseGeometry)
                        {


                        }
                    }
                    if (isInRegion)
                    {
                        IntersectGeometry ig = new IntersectGeometry();
                        ig.GeometryShape = sgs[i];
                        selects.Add(ig);
                    }
                }
            }
            return selects;
        }

    }
}
