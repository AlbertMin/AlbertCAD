using Albert.CAD.Analyze;
using Albert.DrawingKernel.Events;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Consult;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Util;
using Albert.Geometry.External;
using Albert.Geometry.Primitives;
using Fluent;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Albert.CAD
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private int lineWidth = 1;

        /// <summary>
        /// 默认的线宽度
        /// </summary>
        public int LineWidth
        {
            get
            {

                return lineWidth;
            }
            set
            {

                lineWidth = value;
            }
        }


        private Color penColor = Colors.Black;
        /// <summary>
        /// 默认的线颜色
        /// </summary>
        public Color PenColor
        {

            get
            {
                return penColor;
            }
            set
            {
                penColor = value;

            }
        }

        private Color fillColor = Colors.Transparent;

        /// <summary>
        /// 当前的填充颜色
        /// </summary>
        public Color FillColor
        {

            get
            {
                return fillColor;
            }
            set
            {
                fillColor = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.drawingKernel.Prompt += DrawingKernel_Prompt;
            this.EndPointCatchBtn.IsChecked = KernelProperty.CanCatchEndPoint;
            this.InjectCatchBtn.IsChecked = KernelProperty.CanCatchIntersect;
            this.ShiftCatchBtn.IsChecked = KernelProperty.CanCatchShift;
            this.CentralCatchBtn.IsChecked = KernelProperty.CanCatchCentral;
        }

        /// <summary>
        /// 当前绘制面板的操作提示
        /// </summary>
        /// <param name="cp"></param>
        private void DrawingKernel_Prompt(CommandPromptEventArgs cp)
        {
            this.DrawTipLab.Content = cp.Message;
        }



        /// <summary>
        /// 线绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.LineAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 绘制一个多线段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PLineBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.PolylineAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 绘制一个多边形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PolygonBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.PolygonAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 绘制一个矩形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectangleBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.RectangleAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 绘制一个圆形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircleBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.CircleAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 绘制椭圆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EllipseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.EllipseAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 绘制半圆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArcBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.ArcAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 绘制一个普通墙
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NWallBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.WallAction(this.PenColor, this.lineWidth, 200, this.FillColor);
        }

        private void CWallBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.WallAction(this.PenColor, this.lineWidth, 89, this.FillColor);
        }
        /// <summary>
        /// 绘制一个梁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeamBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.BeamAction(this.PenColor, this.lineWidth, 100, this.FillColor);
        }
        /// <summary>
        /// 绘制OSB板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColumnBtn_Click(object sender, RoutedEventArgs e)
        {
            //  this.drawingKernel.C
        }

        /// <summary>
        /// 测量按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeasureBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.MeasureAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 文本的输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.TextAction(this.PenColor, 14, 0);
        }

        /// <summary>
        /// 启动移动命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.MoveCommand();
        }

        private void CopyBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.OffsetCommand();
        }

        private void MirrorBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.MirrorCommand();
        }
        /// <summary>
        /// 对齐操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlignBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.AlignCommand();
        }

        /// <summary>
        /// 转动操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RotateBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.RotateCommand();
        }



        /// <summary>
        /// 绘制一个楼板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FloorBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.FloorAction(this.PenColor, this.lineWidth, 100, this.FillColor);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 保存当前数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 绘制一个钢梁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SteelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.SteelBeamAction(this.PenColor, this.lineWidth, 100, this.FillColor);
        }

        /// <summary>
        /// 杆件的绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.MemberAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 绘制OSB板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OsbBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.OSBAction(this.PenColor, this.lineWidth, this.FillColor);
        }


        /// <summary>
        /// 绘制一个圆形柱子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircleColumnBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.ColumnAction(this.PenColor, this.lineWidth, this.FillColor, 0);
        }
        /// <summary>
        /// 绘制一个方形柱子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SquareColumnBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.ColumnAction(this.PenColor, this.lineWidth, this.FillColor, 1);
        }
        /// <summary>
        /// 绘制一个钢柱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SteelColumnBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.SteelColumnAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 当前的标尺的显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StaffBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.StaffAction(this.PenColor, this.lineWidth, this.FillColor);
        }

        /// <summary>
        /// 添加一个索引位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LegwireBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.LegwireAction(this.PenColor, this.lineWidth, this.FillColor);
        }


        /// <summary>
        /// 建立一个新项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //判断当前画板上是否存在图形，如果存在
            if (this.drawingKernel.HasGeometry)
            {

                MessageBoxResult result = MessageBox.Show("当前界面上有图形，请问是否保存当前文档", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.OK)
                {
                    ApplicationCommands.Save.Execute(null, this);
                }
                else
                {

                    this.drawingKernel.Clear();
                }
            }
        }
        /// <summary>
        /// 是否能建立一个新项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        /// <summary>
        /// 打开一个项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        /// <summary>
        /// 是否能打开一个新项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "打开一个绘制文件";
            ofd.Filter = "CDO文件|*.cdo";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                if (ofd.FileName == App.DocumentUrl)
                {

                    MessageBox.Show("打开了相同的文件", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    App.DocumentUrl = ofd.FileName;
                    Input input = new Input(this, drawingKernel);
                    //将外部文件输入到当前界面上
                    input.ReaderXML(App.DocumentUrl);

                }

            }

        }
        /// <summary>
        /// 保存当前项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.drawingKernel != null && this.drawingKernel.HasGeometry)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        /// <summary>
        /// 是否能保存当前项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //说明没有保存过
            if (App.DocumentUrl == null)
            {
                App.DocumentUrl = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CDO文件|*.cdo";
                sfd.DefaultExt = ".cdo";
                sfd.AddExtension = true;
                sfd.InitialDirectory = App.DocumentUrl;

                if (sfd.ShowDialog() == true)
                {
                    App.DocumentUrl = sfd.FileName.ToString();
                }
                else
                {
                    App.DocumentUrl = null;
                    return;
                }
            }
            //保存文件
            Output op = new Output(this, this.drawingKernel);
            try
            {
                op.WriteXML(App.DocumentUrl);
            }
            catch (Exception ex)
            {

                MessageBox.Show("图形存储失败" + ex.Message, "警告", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveASCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.drawingKernel != null && this.drawingKernel.HasGeometry)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        /// <summary>
        /// 是否能另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveASCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            //说明没有保存过
            if (App.DocumentUrl == null)
            {
                App.DocumentUrl = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CDO文件|*.cdo";
                sfd.DefaultExt = ".cdo";
                sfd.AddExtension = true;
                sfd.InitialDirectory = App.DocumentUrl;

                if (sfd.ShowDialog() == true)
                {
                    App.DocumentUrl = sfd.FileName.ToString();
                }
                else
                {
                    App.DocumentUrl = null;
                    return;
                }
            }
            //保存文件
            Output op = new Output(this, this.drawingKernel);
            try
            {
                op.WriteXML(App.DocumentUrl);
            }
            catch (Exception ex)
            {

                MessageBox.Show("图形存储失败" + ex.Message, "警告", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PrintCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.drawingKernel != null && this.drawingKernel.HasGeometry)
            {
                e.CanExecute = true;
            }
            else
            {

                e.CanExecute = false;
            }
        }
        /// <summary>
        /// 执行打印操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PrintDialog pdg = new PrintDialog();

            pdg.SelectedPagesEnabled = true;
            pdg.UserPageRangeEnabled = true;

            if (pdg.ShowDialog() == true)
            {
                pdg.PrintVisual(this.drawingKernel, "图形界面");

            }
        }
        /// <summary>
        /// 是否能关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        /// <summary>
        /// 关闭当前应用程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        /// <summary>
        /// 窗体的加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.DocumentUrl != null)
            {

                Input input = new Input(this, drawingKernel);
                //将外部文件输入到当前界面上
                input.ReaderXML(App.DocumentUrl);
            }
        }


        /// <summary>
        /// 线的宽度设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineWidthBtn_Click(object sender, RoutedEventArgs e)
        {
            string tag = (e.Source as Fluent.MenuItem).Tag.ToString();
            this.lineWidth = int.Parse(tag);
        }

        /// <summary>
        /// 颜色的选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineColorSel_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            this.penColor = LineColorSel.SelectedColor.Value;

        }
        /// <summary>
        /// 填充颜色的改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FillColorSel_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            this.fillColor = FillColorSel.SelectedColor.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArrayBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.ArrayCommand(4);
        }

        /// <summary>
        /// 反转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReversalBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawingKernel.ReversalCommand();
        }

        /// <summary>
        /// 封闭区域测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosetTestBtn1_Click(object sender, RoutedEventArgs e)
        {
            //封闭区域测试
            List<Geometry2D> gss = this.drawingKernel.GeometryShapes;

            List<Line2D> lines = new List<Line2D>();
            //转换图形
            foreach (var g in gss)
            {
                if (g is DrawingKernel.Geometries.Primitives.LineGeometry)
                {
                    DrawingKernel.Geometries.Primitives.LineGeometry line = g as DrawingKernel.Geometries.Primitives.LineGeometry;
                    lines.Add(Line2D.Create(line.Start, line.End));
                }
                if (g.GeometryId == "tp")
                {

                    this.drawingKernel.RemoveGeometryShape(g);
                }
            }
            //查找封闭区域
            List<List<Line2D>> nn = GraphicAlgorithm.ClosedLookup(lines, true, true);
            if (nn != null)
            {
                nn.ForEach(x =>
                {

                    PolygonGeometry pg = new PolygonGeometry();

                    x.ForEach(y => { pg.PPoints.Add(y.Start); });
                    pg.FillColor = KernelProperty.GetRandomColor();
                    pg.GeometryId = "tp";
                    this.drawingKernel.AddShape(pg);

                });
            }
        }

        private void ClosetTestBtn2_Click(object sender, RoutedEventArgs e)
        {
            //封闭区域测试
            List<Geometry2D> gss = this.drawingKernel.GeometryShapes;

            List<Line2D> lines = new List<Line2D>();
            //转换图形
            foreach (var g in gss)
            {
                if (g is DrawingKernel.Geometries.Primitives.LineGeometry)
                {
                    DrawingKernel.Geometries.Primitives.LineGeometry line = g as DrawingKernel.Geometries.Primitives.LineGeometry;
                    lines.Add(Line2D.Create(line.Start, line.End));
                }
                if (g.GeometryId == "tp")
                {

                    this.drawingKernel.RemoveGeometryShape(g);
                }

            }
            //查找封闭区域
            List<List<Line2D>> nn = GraphicAlgorithm.ClosedLookup(lines, false, true);
            if (nn != null)
            {
                nn.ForEach(x =>
                {
                    PolygonGeometry pg = new PolygonGeometry();
                    x.ForEach(y => { pg.PPoints.Add(y.Start); });
                    pg.FillColor = KernelProperty.GetRandomColor();
                    pg.GeometryId = "tp";
                    this.drawingKernel.AddShape(pg);

                });
            }
        }
        /// <summary>
        /// 打断测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DecomposeTestBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Geometry2D> gss = this.drawingKernel.GeometryShapes;
            List<Line2D> lines = new List<Line2D>();
            foreach (var g in gss)
            {
                if (g is DrawingKernel.Geometries.Primitives.LineGeometry)
                {
                    DrawingKernel.Geometries.Primitives.LineGeometry line = g as DrawingKernel.Geometries.Primitives.LineGeometry;
                    lines.Add(Line2D.Create(line.Start, line.End));
                }
                if (g.GeometryId == "tp")
                {

                    this.drawingKernel.RemoveGeometryShape(g);
                }
            }
            //查找封闭区域
            List<Line2D> decomposes = GraphicAlgorithm.Decompose(lines);

            if (decomposes != null)
            {
                decomposes.ForEach(x =>
                {
                    DrawingKernel.Geometries.Primitives.LineGeometry lg = new DrawingKernel.Geometries.Primitives.LineGeometry(x.Start, x.End);
                    lg.PenColor = KernelProperty.GetRandomColor();
                    lg.GeometryId = "tp";
                    this.drawingKernel.AddShape(lg);
                });
            }
        }

        /// <summary>
        /// 检测一个点是否在一个多边形内部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsideTestBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Geometry2D> gss = this.drawingKernel.GeometryShapes;
            List<Line2D> lines = new List<Line2D>();

            List<Vector2D> vts = new List<Vector2D>();
            foreach (var g in gss)
            {
                if (g is PolygonGeometry)
                {
                    PolygonGeometry poly = g as DrawingKernel.Geometries.Primitives.PolygonGeometry;
                    g.FillColor = Colors.Azure;
                    g.Opacity = 0.3;
                    poly.Update();
                    lines.AddRange(poly.Lines);
                }
                if (g is DrawingKernel.Geometries.Primitives.LineGeometry)
                {
                    DrawingKernel.Geometries.Primitives.LineGeometry l = g as DrawingKernel.Geometries.Primitives.LineGeometry;
                    vts.Add(l.Start);
                    vts.Add(l.End);
                }
                if (g.GeometryId == "tp")
                {
                    this.drawingKernel.RemoveGeometryShape(g);
                }
            }

            if (vts != null)
            {

                int resultP = 0;
                for (int i = 0; i < vts.Count; i++)
                {
                    resultP = GraphicAlgorithm.InsideOfRegion(vts[i], lines);
                    if (resultP == 0)
                    {
                        CircleGeometry c = new DrawingKernel.Geometries.Primitives.CircleGeometry();
                        c.FillColor = Colors.Red;
                        c.Start = vts[i];
                        c.End = Vector2D.Create(vts[i].X + 5, vts[i].Y);
                        c.GeometryId = "tp";
                        this.drawingKernel.AddShape(c);
                    }
                    else if (resultP == 1)
                    {
                        CircleGeometry c = new DrawingKernel.Geometries.Primitives.CircleGeometry();
                        c.FillColor = Colors.Green;
                        c.Start = vts[i];
                        c.End = Vector2D.Create(vts[i].X + 5, vts[i].Y);
                        c.GeometryId = "tp";
                        this.drawingKernel.AddShape(c);
                    }
                    else
                    {
                        CircleGeometry c = new DrawingKernel.Geometries.Primitives.CircleGeometry();
                        c.FillColor = Colors.Yellow;
                        c.Start = vts[i];
                        c.End = Vector2D.Create(vts[i].X + 5, vts[i].Y);
                        c.GeometryId = "tp";
                        this.drawingKernel.AddShape(c);
                    }
                }

            }

        }

        /// <summary>
        /// 合并可以合并的线段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MergeTestBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Geometry2D> gss = this.drawingKernel.GeometryShapes;
            List<Line2D> lines = new List<Line2D>();
            foreach (var g in gss)
            {
                if (g is DrawingKernel.Geometries.Primitives.LineGeometry)
                {
                    DrawingKernel.Geometries.Primitives.LineGeometry line = g as DrawingKernel.Geometries.Primitives.LineGeometry;
                    lines.Add(Line2D.Create(line.Start, line.End));
                }
                if (g.GeometryId == "tp")
                {

                    this.drawingKernel.RemoveGeometryShape(g);
                }
            }

            List<Line2D> mergeLines = GraphicAlgorithm.Merge(lines);

            if (mergeLines != null)
            {
                mergeLines.ForEach(x =>
                {
                    DrawingKernel.Geometries.Primitives.LineGeometry lg = new DrawingKernel.Geometries.Primitives.LineGeometry(x.Start, x.End);
                    lg.PenColor = KernelProperty.GetRandomColor();
                    lg.GeometryId = "tp";
                    this.drawingKernel.AddShape(lg);
                });
            }
        }
        /// <summary>
        /// 图形的内缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElasticityTestBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Geometry2D> gss = this.drawingKernel.GeometryShapes;
            List<Line2D> lines = new List<Line2D>();
            foreach (var g in gss)
            {
                if (g is DrawingKernel.Geometries.Primitives.LineGeometry)
                {
                    DrawingKernel.Geometries.Primitives.LineGeometry line = g as DrawingKernel.Geometries.Primitives.LineGeometry;
                    lines.Add(Line2D.Create(line.Start, line.End));
                }
                if (g.GeometryId == "tp")
                {

                    this.drawingKernel.RemoveGeometryShape(g);
                }
            }
            //查找封闭区域
            List<List<Line2D>> nn = GraphicAlgorithm.ClosedLookup(lines, true, true);

            if (nn != null && nn.Count > 0)
            {

                List<Line2D> nt = nn[0];

                List<Line2D> wtn = GraphicAlgorithm.Elastic(nt, -20);

                PolygonGeometry pg = new PolygonGeometry();
                wtn.ForEach(y => { pg.PPoints.Add(y.Start); });
                pg.FillColor = KernelProperty.GetRandomColor();
                pg.GeometryId = "tp";
                this.drawingKernel.AddShape(pg);
            }

        }
        /// <summary>
        /// 端点捕获
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndPointCatchBtn_Click(object sender, RoutedEventArgs e)
        {
            KernelProperty.CanCatchEndPoint = EndPointCatchBtn.IsChecked.Value;
        }

        private void InjectCatchBtn_Click(object sender, RoutedEventArgs e)
        {
            KernelProperty.CanCatchIntersect = InjectCatchBtn.IsChecked.Value;
        }

        private void ShiftCatchBtn_Click(object sender, RoutedEventArgs e)
        {
            KernelProperty.CanCatchShift = ShiftCatchBtn.IsChecked.Value;
        }

        private void CentralCatchBtn_Click(object sender, RoutedEventArgs e)
        {
            KernelProperty.CanCatchCentral = CentralCatchBtn.IsChecked.Value;
        }

        /// <summary>
        /// 添加一个图片元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {

                string filename = ofd.FileName;
                ImageGeometry ig = new ImageGeometry(new Vector2D(100, 100), filename);
                this.drawingKernel.AddShape(ig);
            }
        }


        /// <summary>
        /// 线和多边形的关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PInsideTestBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Geometry2D> gss = this.drawingKernel.GeometryShapes;
            List<Line2D> lines = new List<Line2D>();

            List<Line2D> vts = new List<Line2D>();
            foreach (var g in gss)
            {
                if (g is PolygonGeometry)
                {
                    PolygonGeometry poly = g as DrawingKernel.Geometries.Primitives.PolygonGeometry;
                    g.FillColor = Colors.Azure;
                    g.Opacity = 0.3;
                    poly.Update();
                    lines.AddRange(poly.Lines);
                }
                if (g is DrawingKernel.Geometries.Primitives.LineGeometry)
                {
                    DrawingKernel.Geometries.Primitives.LineGeometry l = g as DrawingKernel.Geometries.Primitives.LineGeometry;
                    vts.Add(Line2D.Create(l.Start, l.End));
                }
                if (g.GeometryId == "tp")
                {
                    this.drawingKernel.RemoveGeometryShape(g);
                }
            }

            if (vts != null)
            {


                for (int i = 0; i < vts.Count; i++)
                {
                    var rs = GraphicAlgorithm.LineInsideOfRegion(vts[i], lines, null);
                    if (rs == 1)
                    {
                        DrawingKernel.Geometries.Primitives.LineGeometry lg = new DrawingKernel.Geometries.Primitives.LineGeometry();
                        lg.Start = vts[i].Start;
                        lg.End = vts[i].End;
                        lg.PenColor = Colors.Blue;
                        lg.GeometryId = "tp";
                        this.drawingKernel.AddShape(lg);
                    }
                    else if (rs==0)
                    {
                        DrawingKernel.Geometries.Primitives.LineGeometry lg = new DrawingKernel.Geometries.Primitives.LineGeometry();
                        lg.PenColor = Colors.Red;
                        lg.Start = vts[i].Start;
                        lg.End = vts[i].End;
                        lg.GeometryId = "tp";
                        this.drawingKernel.AddShape(lg);
                    }
                    else
                    {
                        DrawingKernel.Geometries.Primitives.LineGeometry lg = new DrawingKernel.Geometries.Primitives.LineGeometry();
                        lg.PenColor = Colors.Black;
                        lg.Start = vts[i].Start;
                        lg.End = vts[i].End;
                        lg.GeometryId = "tp";
                        this.drawingKernel.AddShape(lg);
                    }
                }
            }
        }

        private void SteelSupportBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 一个多边形和另外一个多边形之间的关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PolygonInsideTestBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Geometry2D> gss = this.drawingKernel.GeometryShapes;
            List<List<Line2D>> lines = new List<List<Line2D>>();

            foreach (var g in gss)
            {
                if (g is PolygonGeometry)
                {
                    List<Line2D> nlines = new List<Line2D>();
                    PolygonGeometry poly = g as DrawingKernel.Geometries.Primitives.PolygonGeometry;
                    g.FillColor = Colors.Azure;
                    g.Opacity = 0.3;
                    poly.Update();
                    nlines.AddRange(poly.Lines);
                    lines.Add(nlines);
                }
                if (g.GeometryId == "tp")
                {
                    this.drawingKernel.RemoveGeometryShape(g);
                }

            }

            //计算多边形和多边形之间的关系
            if (lines.Count == 2)
            {
                List<List<Line2D>> ins = new List<List<Line2D>>();
                var result = GraphicAlgorithm.PolygonInsideOfRegion(lines[0], lines[1], ins);

                if (result == 1)
                {
                    ins.ForEach(x => {


                        PolygonGeometry pg = new PolygonGeometry();
                        x.ForEach(y => { pg.PPoints.Add(y.Start); });
                        pg.FillColor = Colors.Green;
                        pg.GeometryId = "tp";
                        this.drawingKernel.AddShape(pg);

                    });

                }
                else if (result == 0)
                {
                    ins.ForEach(x => {


                        PolygonGeometry pg = new PolygonGeometry();
                        x.ForEach(y => { pg.PPoints.Add(y.Start); });
                        pg.FillColor = Colors.Red;
                        pg.GeometryId = "tp";
                        this.drawingKernel.AddShape(pg);

                    });
                }
                else
                {

                    MessageBox.Show("相离");
                }
            }
            else
            {

                MessageBox.Show("不表现多个多边形的情况");
            }

        }
    }
}