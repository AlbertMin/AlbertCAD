using Albert.DrawingKernel.Filter;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Consult;
using Albert.DrawingKernel.Geometries.Temporary;
using Albert.DrawingKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Albert.DrawingKernel.Controls
{
    /// <summary>
    /// 用于定义界面上的可区分元素，包含绘制对象，辅助对象，和临时对象等
    /// </summary>
    public class DistinguishingControl : ContentControl
    {

        public bool IsUpdated = false;
        /// <summary>
        /// 当前的构造函数
        /// </summary>
        public DistinguishingControl()
        {

            this.Background = new SolidColorBrush(Colors.Transparent);
    
        }

        /// <summary>
        /// 绘制的对象，需要标识的对象元素
        /// </summary>
        protected List<Geometry2D> DrawingVisuals = new List<Geometry2D>();

        /// <summary>
        /// 当前的固定对象,比如标尺和其他，不随着比例尺变化
        /// </summary>
        protected List<Geometry2D> AuxiliaryVisuals = new List<Geometry2D>();


        /// <summary>
        /// 界面上的临时对象，如捕捉点，频繁需要清除和移除的临时对象
        /// </summary>
        protected List<Geometry2D> TemporaryVisuals = new List<Geometry2D>();



        /// <summary>
        /// 在界面上添加一个
        /// </summary>
        /// <param name="v"></param>
        public void AddDrawingVisual(Geometry2D g)
        {

            DrawingVisuals.Add(g);
            this.AddVisual(g);
            g.Update();
            IsUpdated = true;
        }
        /// <summary>
        /// 删除一个绘制可视化对象
        /// </summary>
        /// <param name="e"></param>
        public void RemoveDrawingVisual(Geometry2D g)
        {
            DrawingVisuals.Remove(g);
            this.RemoveVisual(g);
            IsUpdated = true;
        }

        /// <summary>
        /// 获取所有的绘制对象
        /// </summary>
        public List<Geometry2D> GetDrawingVisuals() {

            return this.DrawingVisuals.ToList();
        }

        /// <summary>
        /// 捕获所有的可视化对象
        /// </summary>
        /// <returns></returns>
        public List<Geometry2D> CatchVisuals()
        {
            List<Geometry2D> gts = this.DrawingVisuals.ToList().FindAll(x => x.IsEnabled);
            return gts;
        }


        /// <summary>
        /// 获取其中一个绘制对象
        /// </summary>
        /// <returns></returns>
        public Geometry2D GetDrawingVisuals(string geometryId)
        {
            return this.DrawingVisuals.Find(x=>x.GeometryId== geometryId);
        }

        //清除所有的绘制对象
        public void RemoveAllDrawingVisual()
        {
            while (this.DrawingVisuals.Count > 0)
            {
                this.RemoveDrawingVisual(this.DrawingVisuals[0]);
            }
        }


        /// <summary>
        /// 添加一个参照对象
        /// </summary>
        /// <param name="e"></param>
        public void AddAuxiliaryVisual(Geometry2D g)
        {
            AuxiliaryVisuals.Add(g);
            this.AddVisual(g);
            g.Update();
            IsUpdated = true;
        }

        /// <summary>
        /// 移除一个参照对象
        /// </summary>
        /// <param name="e"></param>
        public void RemoveAuxiliaryVisual(Geometry2D g)
        {
            AuxiliaryVisuals.Remove(g);
            this.RemoveVisual(g);
            g.Update();
            IsUpdated = true;
        }


        /// <summary>
        /// 获取所有的固定对象
        /// </summary>
        public List<Geometry2D> GetAuxiliaryVisuals()
        {
            return this.AuxiliaryVisuals.ToList();
        }

        /// <summary>
        /// 获取其中一个固定对象
        /// </summary>
        /// <returns></returns>
        public Geometry2D GetAuxiliaryVisual(string geometryId)
        {
            return this.AuxiliaryVisuals.Find(x => x.GeometryId == geometryId);
        }


        //清除所有的绘制对象
        public void RemoveAllAuxiliaryVisual()
        {
            while (this.AuxiliaryVisuals.Count > 0)
            {
                this.RemoveAuxiliaryVisual(this.AuxiliaryVisuals[0]);
            }
        }




        /// <summary>
        /// 添加临时可见对象
        /// </summary>
        /// <param name="e"></param>
        public void AddTemporaryVisual(Geometry2D g)
        {
            TemporaryVisuals.Add(g);
            this.AddVisual(g);
            g.Update();
            IsUpdated = true;
        }

        /// <summary>
        /// 移除一个临时可见对象
        /// </summary>
        /// <param name="e"></param>
        public void RemoveTemporaryVisual(Geometry2D g)
        {
            TemporaryVisuals.Remove(g);
            this.RemoveVisual(g);
            g.Update();
            IsUpdated = true;
        }


        /// <summary>
        /// 获取所有的固定对象
        /// </summary>
        public List<Geometry2D> GetTemporaryVisuals()
        {
            return this.TemporaryVisuals.ToList();
        }

        /// <summary>
        /// 获取其中一个固定对象
        /// </summary>
        /// <returns></returns>
        public Geometry2D GetTemporaryVisual(string geometryId)
        {
            return this.TemporaryVisuals.Find(x => x.GeometryId == geometryId);
        }

        //清除所有的绘制对象
        public void RemoveAllTemporaryVisual()
        {
            while (this.TemporaryVisuals.Count > 0)
            {
                this.RemoveTemporaryVisual(this.TemporaryVisuals[0]);
            }
        }

        /// <summary>
        /// 删除所有的临时信息
        /// </summary>
        /// <param name="rt"></param>
        public void RemoveTemporaryVisualsByType(int rt)
        {
           
            //移除捕获点
            var removet = TemporaryVisuals.FindAll(x => x is SnapGeometry||x is HightLightGeometry);
            //移除所有的捕获点
            removet.ForEach(x => {
                this.RemoveTemporaryVisual(x);
            });

            if (rt == 1) {
                var removes = TemporaryVisuals.FindAll(x => x is SelectedGeometry||x is PickedGeometry||x is SelectLightGeometry);
                removes.ForEach(x => {
                    this.RemoveTemporaryVisual(x);
                });
            }
        }

        /// <summary>
        /// 通过索引查找对应的元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Geometry2D GetElementChild(int index)
        {

            return (Geometry2D)this.GetVisualChild(index);
        }

        /// <summary>
        /// 是否包含指定对象
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        public bool IsContains(Geometry2D gs) {

            Geometry2D gm = this.Visuals.Find(x => x == gs);
            if (gm != null)
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 是否显示网格
        /// </summary>
        public Boolean HasGrid
        {
            get
            {
                Geometry2D grid = AuxiliaryVisuals.Find(x => x is GridGeometry);
                if (grid == null)
                {
                    return false;
                }
                return true;
            }
            set
            {
                if (value)
                {
                    Geometry2D grid = AuxiliaryVisuals.Find(x => x is GridGeometry);
                    if (grid == null)
                    {
                        grid = new GridGeometry();
                        this.AddAuxiliaryVisual(grid);
                        grid.Update();
                    }
                }
                else
                {
                    Geometry2D grid = AuxiliaryVisuals.Find(x => x is GridGeometry);
                    if (grid != null)
                    {
                        this.RemoveAuxiliaryVisual(grid);
                    }
                }
            }
        }

        /// <summary>
        /// 是否显示中心
        /// </summary>
        public Boolean HasCentral
        {
            get
            {
                Geometry2D central = AuxiliaryVisuals.Find(x => x is CentralGeometry);
                if (central == null)
                {
                    return false;
                }
                return true;
            }
            set
            {
                if (value)
                {
                    Geometry2D central = AuxiliaryVisuals.Find(x => x is CentralGeometry);
                    if (central == null)
                    {
                        central = new CentralGeometry();
                        central.Update();
                        this.AddAuxiliaryVisual(central);

                    }
                }
                else
                {
                    Geometry2D central = AuxiliaryVisuals.Find(x => x is CentralGeometry);
                    if (central != null)
                    {
                        this.RemoveAuxiliaryVisual(central);
                    }
                }
            }
        }


        /// <summary>
        /// 设置当前过滤器
        /// </summary>
        /// <param name="filter"></param>
        public void SetPickFilter(IPickFilter filter) {

            if (filter != null)
            {
                List<Geometry2D> gss = this.GetDrawingVisuals();

                //找到不允许选择的图形元素
                List<Geometry2D> gts = gss.FindAll(x => !filter.AllowElement(x));
                //历遍所有的元素
                gts.ForEach(x =>
                {

                  //  x.IsEnabled = false;
                });
            }

        }



        /// <summary>
        /// 刷新当前界面上所有的元素
        /// </summary>
        public void Update()
        {

            AuxiliaryVisuals.ForEach(x => {

                x.Update();
            });

            DrawingVisuals.ForEach(x => {

                x.Update();
            });

            TemporaryVisuals.ForEach(x => {

                x.Update();
            });
        }

    }
}
