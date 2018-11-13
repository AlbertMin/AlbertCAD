using Albert.DrawingKernel.Commands;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.Filter;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Geometries.Temporary;
using Albert.DrawingKernel.PenAction;
using Albert.DrawingKernel.Selector;
using Albert.DrawingKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Grid = System.Windows.Controls.Grid;
using Albert.DrawingKernel.Functional;
using System.Windows.Controls;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel
{
    /// <summary>
    /// 当前的绘制浏览器
    /// </summary>
    public class Explorer : Grid
    {
        /// <summary>
        /// 绘制控制器
        /// </summary>
        private DrawingControl DrawingControl = null;

        /// <summary>
        /// 当前面板上的绘制动作事件
        /// </summary>
        public event ActionEvent ActionEstablish;
        public event ActionEvent ActionStart;
        public event ActionEvent ActionRuning;
        public event ActionEvent ActionComplete;
        public event ActionEvent ActionTerminal;
        public event MenuEvent RightMenuEvent;


        /// <summary>
        /// 当前界面上的选择和提示命令
        /// </summary>

        public event MultiShapeSelectHandler SelectChanged;

        private event ShapePickHandler m_ShapePickHandler = null;
        public event ShapePickHandler Picked {

            add {
                if (m_ShapePickHandler != null) {

                    m_ShapePickHandler = null;
                }
                if (m_ShapePickHandler == null)
                {
                    m_ShapePickHandler += value;
                }

            }
            remove {

                m_ShapePickHandler -= value;
            }
        }
        public event MultiShapeSelectHandler Deleted;
        public event CommandPromptHandler Prompt;
        public event FunctionalHanlder FunctionalEvent;


        /// <summary>
        /// 最后使用的命令
        /// </summary>
        public IAction LastAction = null;



        /// <summary>
        /// 构造函数，初始化当前界面
        /// </summary>
        public Explorer()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 初始化当前界面
        /// </summary>
        private void InitializeComponent()
        {
            DrawingControl = new DrawingControl();
            DrawingControl.ActionEstablish += DrawingControl_ActionEstablish;
            DrawingControl.ActionStart += DrawingControl_ActionStart;
            DrawingControl.ActionRuning += DrawingControl_ActionRuning;
            DrawingControl.ActionTerminal += DrawingControl_ActionTerminal;
            DrawingControl.ActionComplete += DrawingControl_ActionComplete;
            DrawingControl.SelectChanged += DrawingControl_SelectChanged;
            DrawingControl.Deleted += DrawingControl_Deleted;
            DrawingControl.Picked += DrawingControl_Picked;
            DrawingControl.FunctionalEvent += DrawingControl_FunctionalEvent;
            DrawingControl.RightMenuEvent += DrawingControl_RightMenuEvent;
            this.Loaded += Explorer_Loaded;
            DrawingControl.Prompt += DrawingControl_Prompt;
            this.Children.Add(DrawingControl);

        }


        /// <summary>
        /// 当前的右键点击事件
        /// </summary>
        /// <param name="r"></param>
        private void DrawingControl_RightMenuEvent(Events.RightMouseEventArgs r)
        {
            if (RightMenuEvent != null)
            {
                RightMenuEvent(r);
            }
        }

        /// <summary>
        /// 当前的撤销动作
        /// </summary>
        /// <param name="bf"></param>
        void DrawingControl_FunctionalEvent(BaseFunctional bf)
        {
            if (FunctionalEvent != null) {

                FunctionalEvent(bf);
            }
        }

        /// <summary>
        /// 创建一个新命令，则触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="action"></param>
        void DrawingControl_ActionEstablish(object sender, IAction action)
        {
            if (ActionEstablish != null)
            {
                ActionEstablish(sender, action);
            }
            DrawingControl.UnPick();
        }

        /// <summary>
        /// 弹出提示框
        /// </summary>
        /// <param name="cp"></param>
        void DrawingControl_Prompt(Events.CommandPromptEventArgs cp)
        {
            if (Prompt != null)
                Prompt(cp);
        }



        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="g"></param>
        private void DrawingControl_Deleted(Events.MultiSelectedEventArgs g)
        {
            if (Deleted != null)
            {
                Deleted(g);

            }
        }

        /// <summary>
        /// 绘制面板上的选取事件
        /// </summary>
        /// <param name="g"></param>
        void DrawingControl_Picked(Events.PickedEventArgs g)
        {
            if (m_ShapePickHandler != null)
            {
                m_ShapePickHandler(g);
            }

        }
        /// <summary>
        /// 当前的全部选择事件
        /// </summary>
        /// <param name="g"></param>
        private void DrawingControl_SelectChanged(Events.MultiSelectedEventArgs g)
        {

            if (SelectChanged != null)
            {
                SelectChanged(g);
            }
        }
        /// <summary>
        /// 绘制动作完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="action"></param>
        private void DrawingControl_ActionComplete(object sender, IAction action)
        {
            if (ActionComplete != null)
            {
                ActionComplete(sender, action);
            }
        }

        /// <summary>
        /// 绘制动作终止事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="action"></param>
        private void DrawingControl_ActionTerminal(object sender, IAction action)
        {
            if (ActionTerminal != null)
            {
                ActionTerminal(sender, action);
            }
        }



        /// <summary>
        /// 绘制进行中的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="action"></param>
        private void DrawingControl_ActionRuning(object sender, IAction action)
        {
            if (ActionRuning != null)
            {
                ActionRuning(sender, action);
            }
        }

        /// <summary>
        /// 绘制开始的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="action"></param>
        private void DrawingControl_ActionStart(object sender, IAction action)
        {
            if (ActionStart != null)
            {
                ActionStart(sender, action);
            }
        }

        /// <summary>
        /// 镜像命令
        /// </summary>
        public MirrorCommand MirrorCommand()
        {
            var m = new MirrorCommand();
            DrawingControl.SetCommand(m);
            return m;
        }


        /// <summary>
        /// 当前浏览器加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Explorer_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Window w = this.FinadWindow();

            if (w != null)
            {
                w.PreviewKeyDown += W_KeyDown;
            }

        }

        /// <summary>
        /// 当前窗体的主要事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void W_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
            switch (e.Key)
            {
                case System.Windows.Input.Key.Escape:
                    this.DrawingControl.Esc();
                    break;
                case System.Windows.Input.Key.Delete:
                    this.DrawingControl.Delete();
                    break;
                case System.Windows.Input.Key.Space:
                    this.Restart();
                    break;
                default:
                    this.DrawingControl.SendKeyBorad(e.Key, e.KeyboardDevice.Modifiers);
                    break;

            }
        }

        /// <summary>
        /// 重启命令
        /// </summary>
        public void Restart()
        {
            if (this.LastAction != null) {

                CommandRestart cr = new CommandRestart(this, this.LastAction);
                cr.Start();
            }
        }

        /// <summary>
        /// 用于查找当前窗体的父主要窗体
        /// </summary>
        /// <returns></returns>
        public System.Windows.Window FinadWindow()
        {
            System.Windows.Window fw = null;
            dynamic w = this.Parent;
            while (w != null)
            {
                if (w is System.Windows.Window)
                {
                    fw = w as System.Windows.Window;
                    break;
                }
                else
                {
                    w = w.Parent;
                }
            }
            return fw;
        }


        /// <summary>
        /// 当前绘制面板的边界信息和边界数据
        /// </summary>
        public BindingBox BindingBox
        {
            set
            {
                DrawingControl.BindingBox = value;
            }
            get
            {
                return DrawingControl.BindingBox;
            }
        }

        /// <summary>
        /// 直线命令
        /// </summary>
        public LineAction LineAction(Color? penColor = null, int lineWidth = 1, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new LineAction();
            a.Geometry.Element = Element;
            a.Geometry.PenColor = penColor.Value;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            a.Geometry.LineWidth = lineWidth;
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }
        /// <summary>
        /// 多线段命令
        /// </summary>
        public PolylineAction PolylineAction(Color? penColor = null, int lineWidth = 1, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new PolylineAction();
            a.Geometry.Element = Element;
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }

        public ColumnAction ColumnAction(Color? penColor, int lineWidth=1, Color? fillColor=null, int type=0, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new ColumnAction();
            a.Geometry.Element = Element;
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            (a.Geometry as ColumnGeometry).ColumnType = type;
            if (fillColor != null)
            {
                a.Geometry.FillColor = fillColor.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }
        /// <summary>
        /// 绘制引线的动作
        /// </summary>
        /// <param name="penColor"></param>
        /// <param name="lineWidth"></param>
        /// <param name="fillColor"></param>
        public LegwireAction LegwireAction(Color? penColor=null, int lineWidth=1, Color? fillColor=null,dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new LegwireAction();
            a.Geometry.Element = Element;
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            if (fillColor != null)
            {
                a.Geometry.FillColor = fillColor.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }

        /// <summary>
        /// 绘制一个标尺
        /// </summary>
        /// <param name="penColor"></param>
        /// <param name="lineWidth"></param>
        /// <param name="fillColor"></param>
        public StaffAction StaffAction(Color? penColor = null, int lineWidth = 1, Color? fillColor = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new StaffAction();
            a.Geometry.Element = Element;
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            if (fillColor != null)
            {
                a.Geometry.FillColor = fillColor.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;


            return a;
        }
     

        /// <summary>
        /// 绘制一个钢柱
        /// </summary>
        /// <param name="penColor"></param>
        /// <param name="lineWidth"></param>
        /// <param name="fillColor"></param>
        /// <returns></returns>
        public SteelColumnAction SteelColumnAction(Color? penColor, int lineWidth, Color? fillColor, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new SteelColumnAction();
            a.Geometry.Element = Element;
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            if (fillColor != null)
            {
                a.Geometry.FillColor = fillColor.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }

        /// <summary>
        /// 绘制圆形动作
        /// </summary>
        public CircleAction CircleAction(Color? penColor = null, int lineWidth = 1, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new CircleAction();
            a.Geometry.Element = Element;
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }


        /// <summary>
        /// 绘制矩形动作
        /// </summary>
        public RectangleAction RectangleAction(Color? penColor = null, int lineWidth = 1, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new RectangleAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }

        /// <summary>
        /// 绘制一个龙骨杆件
        /// </summary>
        public MemberAction MemberAction(Color? penColor = null, int lineWidth = 1, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new MemberAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }


        /// <summary>
        /// 显示测量线
        /// </summary>
        public MeasureCommand MeasureAction(Color? penColor = null, int lineWidth = 1, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new MeasureCommand();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }
        /// <summary>
        /// 绘制一个任意多边形
        /// </summary>
        /// <param name="penColor"></param>
        /// <param name="thinkness"></param>
        /// <param name="fill"></param>
        /// <param name="Element"></param>
        /// <returns></returns>
        public PolygonAction PolygonAction(Color? penColor = null, int lineWidth = 1, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new PolygonAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }
        /// <summary>
        /// 绘制椭圆动作
        /// </summary>
        public EllipseAction EllipseAction(Color? penColor = null, int lineWidth = 1, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new EllipseAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }
        /// <summary>
        /// 绘制墙动作
        /// </summary>
        public WallAction WallAction(Color? penColor = null, int lineWidth = 1, int thickness = 10, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new WallAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
            (a.Geometry as WallGeometry).Thickness = thickness;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }

        /// <summary>
        /// 绘制梁信息
        /// </summary>
        public BeamAction BeamAction(Color? penColor = null, int lineWidth = 1, double thickness = 10, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new BeamAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
            (a.Geometry as BeamGeometry).Thickness = thickness;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }

 

        /// <summary>
        /// 绘制一个楼板对象
        /// </summary>
        public FloorAction FloorAction(Color? penColor = null, int lineWidth = 1, double thickness = 10, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new FloorAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
            (a.Geometry as FloorGeometry).Thickness = thickness;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }


        /// <summary>
        /// 绘制圆弧
        /// </summary>
        /// <param name="penColor"></param>
        /// <param name="lineWidth"></param>
        /// <param name="thickness"></param>
        /// <param name="fill"></param>
        /// <param name="Element"></param>
        /// <returns></returns>
        public ArcAction ArcAction(Color? penColor = null, int lineWidth = 1, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new ArcAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }

        /// <summary>
        /// 绘制一个钢梁
        /// </summary>
        public SteelBeamAction SteelBeamAction(Color? penColor = null, int lineWidth = 1, double thickness = 200, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new SteelBeamAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
            (a.Geometry as SteelBeamGeometry).Thickness = thickness;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }


        /// <summary>
        /// 绘制一个钢梁
        /// </summary>
        public SteelSupportAction SteelSupportAction(Color? penColor = null, int lineWidth = 1, double thickness = 200, Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new SteelSupportAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;
           // (a.Geometry as SteelSupportGeometry).Thickness = thickness;
            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }
        /// <summary>
        /// OSB的绘制
        /// </summary>
        /// <param name="penColor"></param>
        /// <param name="lineWidth"></param>
        /// <param name="thickness"></param>
        /// <param name="fill"></param>
        /// <param name="Element"></param>
        /// <param name="floor"></param>
        /// <returns></returns>
        public OSBAction OSBAction(Color? penColor = null, int lineWidth = 1,  Color? fill = null, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new OSBAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.LineWidth = lineWidth;
            a.Geometry.Element = Element;

            if (fill != null)
            {
                a.Geometry.FillColor = fill.Value;
            }
            else
            {
                a.Geometry.FillColor = Colors.DarkGoldenrod;
                a.Geometry.Opacity = 0.5;
            }
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }

        /// <summary>
        /// 绘制一个文本
        /// </summary>
        /// <param name="penColor"></param>
        /// <param name="fontsize"></param>
        /// <param name="fontAngle"></param>
        public TextAction TextAction(Color? penColor = null, int fontsize = 12, int fontAngle = 0, dynamic Element = null)
        {
            if (penColor == null)
            {
                penColor = Colors.Black;
            }
            var a = new TextAction();
            a.Geometry.PenColor = penColor.Value;
            a.Geometry.Element = Element;
            (a.Geometry as TextGeometry).FontSize = fontsize;
            (a.Geometry as TextGeometry).Angle = fontAngle;
            DrawingControl.SetAction(a);
            LastAction = a;
            return a;
        }

        /// <summary>
        /// 启动移动命令
        /// </summary>
        /// <returns></returns>
        public MoveCommand MoveCommand(IPickFilter pf=null)
        {
            var c = new MoveCommand();
            c.SetPickFilter(pf);
            DrawingControl.SetCommand(c);
            return c;
        }

        /// <summary>
        /// 当前图形的偏移命令
        /// </summary>
        /// <returns></returns>
        public OffsetCommand OffsetCommand(IPickFilter pf = null)
        {
            var c = new OffsetCommand();
            c.SetPickFilter(pf);
            DrawingControl.SetCommand(c);
            return c;
        }

        /// <summary>
        /// 转动指定的图形元素
        /// </summary>
        /// <returns></returns>
        public RotateCommand RotateCommand()
        {

            var c = new RotateCommand();
            DrawingControl.SetCommand(c);
            return c;
        }

        /// <summary>
        /// 图形的对齐命令
        /// </summary>
        /// <returns></returns>
        public AlignCommand AlignCommand(IPickFilter pf = null)
        {
            var c = new AlignCommand();
            c.SetPickFilter(pf);
            DrawingControl.SetCommand(c);
            return c;
        }

        /// <summary>
        /// 一个反转的命令
        /// </summary>
        public ReverseCommand ReversalCommand(IPickFilter pf = null)
        {
            var c = new ReverseCommand();
            c.SetPickFilter(pf);
            DrawingControl.SetCommand(c);
            return c;
        }
        /// <summary>
        /// 阵列命令
        /// </summary>
        public ArrayCommand ArrayCommand(int number)
        {
            var c = new ArrayCommand(number);
            DrawingControl.SetCommand(c);
            return c;
        }
        /// <summary>
        /// 启动测量命令
        /// </summary>
        /// <returns></returns>
        public MeasureCommand MeasureCommand() {
            var c = new MeasureCommand();
            DrawingControl.SetAction(c);
            return c;
        }


        /// <summary>
        /// 在界面上添加一个图形
        /// </summary>
        /// <param name="shape"></param>
        public void AddShape(Geometry2D shape)
        {
            DrawingControl.AddDrawingVisual(shape);
        }
        /// <summary>
        /// 终止当前所有的绘制操作
        /// </summary>
        public void Esc()
        {
            this.DrawingControl.Esc();
            LastAction = null;
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        public void Delete()
        {

            this.DrawingControl.Delete();
        }

        /// <summary>
        /// 在当前界面中选取感兴趣的点
        /// </summary>
        /// <returns></returns>
        public void Pick(DrawingKernel.Events.PickType type = DrawingKernel.Events.PickType.SetSupport, Filter.IPickFilter filter = null)
        {
            DrawingControl.Pick(type,filter);
        }

        /// <summary>
        /// 获取界面上所有的图形元素
        /// </summary>
        public List<Geometry2D> GeometryShapes
        {
            get
            {
                return this.DrawingControl.GetDrawingVisuals();
            }
        }
        /// <summary>
        /// 当前是否有图形
        /// </summary>
        public bool HasGeometry
        {
            get
            {

                if (this.DrawingControl.GetDrawingVisuals().Count > 0)
                {

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 删除界面上一个图形元素
        /// </summary>
        /// <param name="gs"></param>
        public void RemoveGeometryShape(Geometry2D gs)
        {
            if (gs != null)
            {
                this.DrawingControl.RemoveDrawingVisual(gs);
            }
        }

        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {

            this.DrawingControl.ShowMessage(message);
        }


        /// <summary>
        /// 清除界面上所有的图形元素
        /// </summary>
        public void Clear()
        {

            this.DrawingControl.RemoveAllDrawingVisual();
            this.DrawingControl.RemoveAllTemporaryVisual();
        }


        /// <summary>
        /// 清除所有动作
        /// </summary>
        public void ClearFunctional() {
            this.DrawingControl.Clear();
        }

        /// <summary>
        /// 向当前浏览器添加一个元素
        /// </summary>
        /// <param name="bf"></param>
        public void PushFunctional(BaseFunctional bf) {

            this.DrawingControl.PushFunctional(bf);
        }


        /// <summary>
        /// 设置指定的图形元素高亮且可以点击
        /// </summary>
        /// <param name="geos"></param>
        public void SetLightSelected(params Geometry2D[] geos)
        {
            this.DrawingControl.SetLightSelected(geos);
        }

        /// <summary>
        /// 设置指定区域高亮
        /// </summary>
        /// <param name="region"></param>
        public void SetHightLight(List<Line2D> region) {

            this.DrawingControl.SetHightLight(region);
        }

        /// <summary>
        /// 指定当前物品高亮
        /// </summary>
        /// <param name="g"></param>
        public void SetHightLight(Geometry2D g) {

            List<Line2D> lines = g.Lines;

            if (lines != null) {

                this.SetHightLight(lines);
            }
        }

        /// <summary>
        /// 选择指定的对象选中高亮状态
        /// </summary>
        /// <param name="g"></param>
        public void SetSelectedLight(Geometry2D g) {
    
            this.DrawingControl.SetSelectedLight(g);
        }
        
   
    }
}
