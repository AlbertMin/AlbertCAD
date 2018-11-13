using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Assistant;
using Albert.DrawingKernel.Events;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Temporary;
using Albert.DrawingKernel.PenAction;
using Albert.DrawingKernel.Selector;
using Albert.DrawingKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Albert.DrawingKernel.Geometries.Primitives;
using Albert.DrawingKernel.Prompt;
using Albert.DrawingKernel.Filter;
using Albert.DrawingKernel.Commands;
using Albert.DrawingKernel.Functional;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.Geometries.Consult;

namespace Albert.DrawingKernel.Controls
{
    public delegate void ActionEvent(object sender, IAction action);
    public delegate void MenuEvent(RightMouseEventArgs r);
    public delegate void MultiShapeSelectHandler(MultiSelectedEventArgs g);
    public delegate void ShapePickHandler(PickedEventArgs p);
    /// <summary>
    /// 当前正在执行的命令的提示符
    /// </summary>
    /// <param name="cp"></param>
    public delegate void CommandPromptHandler(CommandPromptEventArgs cp);
    /// <summary>
    /// 定义一个核心画布
    /// </summary>
    public class DrawingControl : DistinguishingControl
    {

        public event ActionEvent ActionEstablish;
        public event ActionEvent ActionStart;
        public event ActionEvent ActionRuning;
        public event ActionEvent ActionComplete;
        public event ActionEvent ActionTerminal;


        public event MultiShapeSelectHandler SelectChanged;
        public event ShapePickHandler Picked;
        public event MultiShapeSelectHandler Deleted;
        public event CommandPromptHandler Prompt;

        public event FunctionalHanlder FunctionalEvent;

        /// <summary>
        /// 当前的右键点击菜单事件
        /// </summary>
        public event MenuEvent RightMenuEvent;

        /// <summary>
        /// 当前的动作
        /// </summary>
        public IAction EAction
        {
            get;
            set;
        }
        /// <summary>
        /// 辅助提示工具
        /// </summary>
        private ActionTip ActionTip = null;

        /// <summary>
        /// 当前的消息提示
        /// </summary>
        private MessageTip messageTip = null;

        /// <summary>
        /// 当前界面的焦点捕获器
        /// </summary>
        private IntersectPointCatch InterestPointCatch = null;

        /// <summary>
        /// 框选捕获功能
        /// </summary>
        private SelectionBoxCatch SelectionBoxCatch = null;

        /// <summary>
        /// 当前绘制控件的边界信息
        /// </summary>
        private BindingBox bindingBox = null;

        /// <summary>
        /// 当前是否拖动
        /// </summary>
        private bool IsDraging = false;

        /// <summary>
        /// 是否进行框选
        /// </summary>
        private bool isBoxSelect = false;

        /// <summary>
        /// 当前点选一个物品
        /// </summary>
        private bool isPick = false;

        /// <summary>
        /// 点选的类型
        /// </summary>
        private PickType pickType = PickType.SetSupport;

        /// <summary>
        /// 绘制结束的点
        /// </summary>
        private Point? EndPoint;
        /// <summary>
        /// 当前界面上选中的对象
        /// </summary>
        private List<IntersectGeometry> SelectIntersectGeometrys = new List<IntersectGeometry>();


        /// <summary>
        /// 动作上下文信息
        /// </summary>
        private FunctionalContext functionalContext = null;


        /// <summary>
        /// 构造函数，初始化当前画板
        /// </summary>
        public DrawingControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化当前界面
        /// </summary>
        private void InitializeComponent()
        {
            InterestPointCatch = new IntersectPointCatch(this);
            SelectionBoxCatch = new SelectionBoxCatch(this);
            functionalContext = new FunctionalContext(this);
            functionalContext.FunctionalEvent += functionalContext_FunctionalEvent;
            ActionTip = new ActionTip(this);
            messageTip = new MessageTip(this);

            this.Focusable = true;
            this.AddEventListener();

        }

        /// <summary>
        /// 进行撤销操作的事务事件
        /// </summary>
        /// <param name="bf"></param>
        void functionalContext_FunctionalEvent(BaseFunctional bf)
        {
            if (FunctionalEvent != null)
            {

                FunctionalEvent(bf);
            }
        }
        /// <summary>
        /// 加载当前的事件监听
        /// </summary>
        private void AddEventListener()
        {

            this.Loaded += DrawingControl_Initialized;
            this.MouseLeftButtonDown += DrawingControl_MouseLeftButtonDown;
            this.MouseDown += DrawingControl_MouseDown;
            this.MouseUp += DrawingControl_MouseUp;
            this.MouseMove += DrawingControl_MouseMove;
            this.MouseLeftButtonUp += DrawingControl_MouseLeftButtonUp;
            this.MouseRightButtonDown += DrawingControl_MouseRightButtonDown;
            this.MouseWheel += DrawingControl_MouseWheel;
            this.MouseLeave += DrawingControl_MouseLeave;
            this.MouseEnter += DrawingControl_MouseEnter;

            //当前窗体的改变事件
            SizeChanged += DrawingControl_SizeChanged;
        }

        /// <summary>
        /// 鼠标进入状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DrawingControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (EAction != null)
            {
                if (EAction is DrawingKernel.Commands.ICommand && ((EAction as DrawingKernel.Commands.ICommand).GetTarget() == null || EAction is AlignCommand))
                {
                    this.Cursor = CursorGetter.Getter(CursorStyle.Command);
                }
                else
                {
                    Cursor = CursorGetter.Getter(CursorStyle.Cross);
                }
            }
            else if (isPick)
            {

                Cursor = CursorGetter.Getter(CursorStyle.Pick); ;
            }
            else
            {
                Cursor = Cursors.Arrow;
            }

        }



        /// <summary>
        /// 鼠标的离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DrawingControl_MouseLeave(object sender, MouseEventArgs e)
        {

            if (isBoxSelect)
            {
                List<IntersectGeometry> igs = SelectionBoxCatch.SelectEnd();
                isBoxSelect = false;
                this.SetSelects(igs);
            }

            this.IsDraging = false;

            Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// 中间按钮的弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DrawingControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                IsDraging = false;
                EndPoint = null;
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// 中间按钮的按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                IsDraging = true;
                this.Cursor = CursorGetter.Getter(CursorStyle.Move);
                EndPoint = e.GetPosition(this);
            }
        }


        /// <summary>
        /// 绘图板的窗体尺寸改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            KernelProperty.MeasureWidth = this.ActualWidth;
            KernelProperty.MeasureHeight = this.ActualHeight;
            if (BindingBox == null)
            {
                BindingBox = new BindingBox(Vector2D.Create(-this.ActualWidth / 2, -this.ActualHeight / 2), Vector2D.Create(this.ActualWidth / 2, this.ActualHeight / 2));
            }
            //刷新当前界面
            this.Update();
        }






        /// <summary>
        /// 鼠标的滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingControl_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (!IsDraging)
            {
                if (e.Delta > 0)
                {
                    KernelProperty.ZoomIn(e.GetPosition(this));
                }
                else
                {
                    KernelProperty.ZoomOut(e.GetPosition(this));
                }

                this.Update();

                this.RemoveTemporaryVisualsByType(0);
            }
        }

        /// <summary>
        /// 右键按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingControl_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            //定义当前右键点击事件
            if (RightMenuEvent != null)
            {
                Vector2D p = KernelProperty.PixToMM(e.GetPosition(this));

                if (EAction != null)
                {
                    IntersectPoint catchPoint = GetIntersectPointCatch(p);

                    //设置捕获点
                    if (catchPoint != null)
                    {
                        p = catchPoint.Point;
                    }
                }
                RightMouseEventArgs rmea = new RightMouseEventArgs(EAction, p);
                RightMenuEvent(rmea);
            }
            else {

                EAction.Termimal();
            }
       
        }

        /// <summary>
        /// 鼠标的移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

            Vector2D p = KernelProperty.PixToMM(e.GetPosition(this));

            //清除当前所有的捕捉点
            this.SetIntersectPointCatch(null);

            //按着中间键，说明只是想拖动
            if (IsDraging)
            {
                this.DragMoveView(e.GetPosition(this));
            }
            else
            {
                if (EAction != null)
                {

                    //假如在绘制过程中，则直接捕获兴趣点
                    IntersectPoint catchPoint = GetIntersectPointCatch(p);


                    //假如设置当前橡皮筋效果
                    if (EAction.Geometry != null)
                    {
                        //进行橡皮筋效果
                        if (EAction.Geometry.IsActioning)
                        {
                            if (catchPoint != null)
                            {
                                EAction.Erase(catchPoint.Point);
                            }
                            else
                            {
                                EAction.Erase(p);
                            }

                        }

                    }

                }
                else
                {
                    //判断当前是否是框选状态
                    if (isBoxSelect)
                    {
                        SelectionBoxCatch.Select(p);
                    }
                }
            }

        }
        /// <summary>
        /// 鼠标的按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //进行坐标转化
            Vector2D p = KernelProperty.PixToMM(e.GetPosition(this));
            if (EAction != null)
            {
                if (EAction is DrawingKernel.Commands.ICommand && (EAction as DrawingKernel.Commands.ICommand).GetTarget() == null)
                {
                    //没有绘制动作，则是物品选择
                    isBoxSelect = true;
                    //开始进行选择操作
                    SelectionBoxCatch.SelectStart(p);

                }
                else
                {
                    IntersectPoint catchPoint = GetIntersectPointCatch(p);

                    //设置捕获点
                    if (catchPoint != null)
                    {
                        EAction.SetPoint(catchPoint.Point);
                    }
                    else
                    {
                        EAction.SetPoint(p);
                    }
                }
            }
            else
            {
                //没有绘制动作，则是物品选择
                isBoxSelect = true;
                SelectionBoxCatch.SelectStart(p);
            }
        }

        /// <summary>
        /// 鼠标的弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.SetIntersectPointCatch(null);
            if (isBoxSelect)
            {
                List<IntersectGeometry> igs = SelectionBoxCatch.SelectEnd();
                isBoxSelect = false;
                this.SetSelects(igs);
            }
        }
        /// <summary>
        /// 当前控件的初始化完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingControl_Initialized(object sender, EventArgs e)
        {
            this.ClipToBounds = true;
            this.HasCentral = false;
            this.HasGrid = false;

        }

        /// <summary>
        /// 拖动控件
        /// </summary>
        /// <param name="p"></param>
        private void DragMoveView(Point? p)
        {


            if (EndPoint != null)
            {
                this.Cursor = CursorGetter.Getter(CursorStyle.Move);
                KernelProperty.PanTo(p.Value, EndPoint.Value);
                this.Update();
            }
            EndPoint = p;
        }


        /// <summary>
        /// 进行绘制
        /// </summary>
        public void SetAction(IAction action)
        {

            if (EAction != null)
            {
                EAction.Termimal();
            }
            this.Cursor = CursorGetter.Getter(CursorStyle.Cross);
            EAction = action;
            this.Focus();
            //获取需要捕获的对象
            GetStatus();
            EAction.DrawStartEvent += EAction_DrawStartEvent;
            EAction.DrawEraseEvent += EAction_DrawEraseEvent;
            EAction.DrawCompleteEvent += EAction_DrawCompleteEvent;
            EAction.DrawTermimationEvent += EAction_DrawTermimationEvent;
            ActionTip.SetAction(action);
         

            if (this.SelectIntersectGeometrys.Count > 0)
            {

                SelectIntersectGeometrys.ForEach(x =>
                {

                    x.GeometryShape.UnSelect();
                });
            }

            //发布一个命令
            if (this.ActionEstablish != null)
            {
                this.ActionEstablish(this, EAction);
            }
            this.RemoveTemporaryVisualsByType(1);
            this.SelectIntersectGeometrys = new List<IntersectGeometry>();
        }

        /// <summary>
        /// 获取当前状态
        /// </summary>
        private void GetStatus()
        {
            if (EAction != null)
            {
                string ss = EAction.GetCommandStatus();
                if (Prompt != null)
                {
                    CommandPromptEventArgs args = new CommandPromptEventArgs(ss);
                    Prompt(args);
                }
            }
        }

        /// <summary>
        /// 设置定的移动命令
        /// </summary>
        /// <param name="command"></param>
        public void SetCommand(DrawingKernel.Commands.ICommand command)
        {
            this.SetAction(command);
            //当前命令的事件记录
            command.RecordCommand += Command_RecordCommand;

            if (this.SelectIntersectGeometrys != null && this.SelectIntersectGeometrys.Count == 1)
            {
                command.SetIntersectGeometry(this.SelectIntersectGeometrys[0]);
                Cursor = CursorGetter.Getter(CursorStyle.Cross);
            }
            else
            {
                this.Cursor = CursorGetter.Getter(CursorStyle.Command);
            }

        }

        /// <summary>
        ///触发当前的记录事件
        /// </summary>
        /// <param name="bf"></param>
        private void Command_RecordCommand(Object sender, BaseFunctional bf)
        {
            if (sender != null && sender is Commands.ICommand)
            {
                (sender as Commands.ICommand).RecordCommand -= Command_RecordCommand;
            }
            this.functionalContext.PushFunctional(bf);

        }

        /// <summary>
        /// 拖拽绘制
        /// </summary>
        /// <param name="g"></param>
        private void EAction_DrawEraseEvent(ActionEventArgs g)
        {
            if (ActionRuning != null)
            {
                ActionRuning(this, EAction);
            }
            GetStatus();
        }

        /// <summary>
        /// 开始绘制
        /// </summary>
        /// <param name="g"></param>
        private void EAction_DrawStartEvent(ActionEventArgs g)
        {
            GetStatus();
            if (EAction.Geometry != null)
            {
                this.AddDrawingVisual(EAction.Geometry);
            }
            if (ActionStart != null)
            {
                ActionStart(this, EAction);
            }
        }


        /// <summary>
        /// 动作暂停
        /// </summary>
        private void EAction_DrawTermimationEvent(ActionEventArgs g)
        {
            GetStatus();
            if (EAction != null)
            {
                EAction.DrawStartEvent -= EAction_DrawStartEvent;
                EAction.DrawEraseEvent -= EAction_DrawEraseEvent;
                EAction.DrawCompleteEvent -= EAction_DrawCompleteEvent;
                EAction.DrawTermimationEvent -= EAction_DrawTermimationEvent;
                if (g != null) {
                    this.RemoveDrawingVisual(g.Geometry2D);
                }
            
            }

            if (ActionTerminal != null)
            {
                ActionTerminal(this, EAction);
            }
            EAction = null;
            this.Cursor = Cursors.Arrow;
            this.RemoveTemporaryVisualsByType(1);
        }


        /// <summary>
        /// 代表一个绘制事件的结束
        /// </summary>
        /// <param name="e"></param>
        private void EAction_DrawCompleteEvent(ActionEventArgs g)
        {
            GetStatus();

            EAction.DrawStartEvent -= EAction_DrawStartEvent;
            EAction.DrawEraseEvent -= EAction_DrawEraseEvent;
            EAction.DrawCompleteEvent -= EAction_DrawCompleteEvent;
            EAction.DrawTermimationEvent -= EAction_DrawTermimationEvent;

            if (ActionComplete != null)
            {
                this.ActionComplete(this, this.EAction);
            }
            if (EAction is AlignCommand)
            {
                ShowMessage("选取的两条线，无法对齐");
            }
            else
            {
                functionalContext.PushFunctional(new AddFunctional(g.Geometry2D));
            }
            if (!g.IsContinuous)
            {
                EAction = null;
            }

            this.Cursor = Cursors.Arrow;
            this.IsUpdated = true;
            this.RemoveTemporaryVisualsByType(1);
        }


        /// <summary>
        /// 当前的ESC事件
        /// </summary>
        public void Esc()
        {
            if (EAction != null)
            {
                EAction.Termimal();
            }
            this.Cursor = Cursors.Arrow;
            this.UnPick();
            if (this.SelectIntersectGeometrys.Count > 0)
            {

                SelectIntersectGeometrys.ForEach(x =>
                {

                    x.GeometryShape.UnSelect();
                });
            }
            this.EAction = null;
            this.RemoveTemporaryVisualsByType(1);
            //清除当前所有的选择对象
            this.SelectIntersectGeometrys = new List<IntersectGeometry>();


        }


        /// <summary>
        /// 删除一个元素
        /// </summary>
        public void Delete()
        {

            if (SelectIntersectGeometrys.Count != 0)
            {

                if (MessageBox.Show("确认删除此图形?", "删除确认", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    MultiSelectedEventArgs ges = new MultiSelectedEventArgs(SelectIntersectGeometrys);
                    if (Deleted != null)
                    {
                        Deleted(ges);

                    }
                    if (ges.CanDeleted)
                    {
                        SelectIntersectGeometrys.ForEach(
                            x =>
                            {
                                RemoveDrawingVisual(x.GeometryShape);
                            }
                         );
                    }

                    this.RemoveTemporaryVisualsByType(1);
                }
            }
        }


        /// <summary>
        /// 添加操作当前数据库
        /// </summary>
        /// <param name="bf"></param>
        public void PushFunctional(BaseFunctional bf)
        {

            this.functionalContext.PushFunctional(bf);
        }


        /// <summary>
        /// 获取捕获点
        /// </summary>
        /// <param name="v"></param>
        private IntersectPoint GetIntersectPointCatch(Vector2D v)
        {

            IntersectPoint catchPoint = null;
            //是否按住了shift键
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) || KernelProperty.CanCatchShift)
            {
                Vector2D pnt = EAction.GetPreviousPoint();
                if (pnt != null)
                {
                    //安装Shift按钮的选中状态
                    catchPoint = InterestPointCatch.GetShiftPoint(pnt, v, EAction);
                }
                else
                {
                    catchPoint = InterestPointCatch.Catch(v, EAction);
                }
            }
            else
            {
                //查找捕获点
                catchPoint = InterestPointCatch.Catch(v, EAction);
            }
            //假如不是命令的话，则显示标尺信息
            if (!(this.EAction is DrawingKernel.Commands.ICommand))
            {
                //在界面上显示捕获点对象
                this.SetIntersectPointCatch(catchPoint);
            }

            //返回当前的兴趣点
            return catchPoint;

        }
        /// <summary>
        /// 捕获当前的兴趣点
        /// </summary>
        /// <param name="p"></param>
        private void SetIntersectPointCatch(IntersectPoint p)
        {

            ///首先移除对应的捕捉
            this.RemoveTemporaryVisualsByType(0);

            if (p != null)
            {
                if (p.Line != null)
                {
                    SnapGeometry hotpoint = new SnapGeometry(p);

                    //在当前长度捕捉位置可以输入距离
                    this.AddTemporaryVisual(hotpoint);
                    //更新捕捉点
                    hotpoint.Update();
                }
                else
                {
                    //在当前长度捕捉位置可以输入距离
                    SnapGeometry hotpoint = new SnapGeometry(p);
                    //更新捕捉点
                    this.AddTemporaryVisual(hotpoint);
                    hotpoint.Update();
                }

            }


        }

        /// <summary>
        /// 把选中的对象设置为选中状态
        /// </summary>
        /// <param name="gss"></param>
        private void SetSelects(List<IntersectGeometry> gss)
        {

            if (gss.Count > 0)
            {
                this.RemoveTemporaryVisualsByType(1);
                SelectIntersectGeometrys.ForEach(x =>
                {
                    x.GeometryShape.UnSelect();
                });

                SelectIntersectGeometrys = gss;

                SelectIntersectGeometrys.ForEach(x =>
                {
                    if (x.IntersectPoint != null && x.IntersectPoint.Point != null)
                    {
                        x.GeometryShape.Select(x.IntersectPoint.Point);

                    }
                 
                });
                if (isPick)
                {
                    if (Picked != null)
                    {
                        var resultSSG = gss.FindAll(x => !(x.GeometryShape is SelectingBox));
                        if (resultSSG != null && resultSSG.Count > 0)
                        {
                            PickedEventArgs ges = new PickedEventArgs(resultSSG[0]);
                            ges.PickType = pickType;
                            Picked(ges);

                            PickedGeometry sg = new PickedGeometry(gss[0]);
                            this.AddTemporaryVisual(sg);
                            sg.Update();
                        }
                    }
                }
                else
                {

                    //对选中的对象进行清除
                    gss.ForEach(x =>
                    {
                        if (!(x.GeometryShape is ImageGeometry))
                        {
                            SelectedGeometry sg = new SelectedGeometry(x);
                            this.AddTemporaryVisual(sg);
                            sg.Update();
                        }
                    });
                    //多选事件
                    if (SelectChanged != null)
                    {
                        MultiSelectedEventArgs ges = new MultiSelectedEventArgs(gss);
                        SelectChanged(ges);
                    }

                }
                //指定当前的命名模式
                if (this.EAction is DrawingKernel.Commands.ICommand)
                {
                    if (gss[0].GeometryShape != null && gss[0].GeometryShape.IsCommand )
                    {

                        (this.EAction as DrawingKernel.Commands.ICommand).SetIntersectGeometry(gss[0]);

                        if (EAction is DrawingKernel.Commands.AlignCommand && (this.EAction as DrawingKernel.Commands.ICommand).GetTarget() == null)
                        {
                            this.Cursor = CursorGetter.Getter(CursorStyle.Command);
                        }
                        else
                        {
                            this.Cursor = CursorGetter.Getter(CursorStyle.Cross);
                        }
                    }
                }

            }
            else
            {

                if (this.EAction is DrawingKernel.Commands.ICommand)
                {
                    this.Cursor = CursorGetter.Getter(CursorStyle.Command);
                }

            }


        }
        /// <summary>
        /// 获取可视化对象
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public List<Geometry2D> CatchVisual(Point point)
        {
            List<Geometry2D> hitGeometry2Ds = new List<Geometry2D>();
            VisualTreeHelper.HitTest(this, null, f =>
            {
                if (f.VisualHit is Geometry2D)
                {
                    hitGeometry2Ds.Add(f.VisualHit as Geometry2D);
                }

                return HitTestResultBehavior.Continue;

            }, new PointHitTestParameters(point));

            return hitGeometry2Ds;
        }

        /// <summary>
        /// 设置当前CAD的边界信息
        /// </summary>
        public BindingBox BindingBox
        {
            set
            {
                bindingBox = value;
            }
            get
            {
                return bindingBox;
            }
        }
        /// <summary>
        /// 选取指定的线
        /// </summary>
        /// <returns></returns>
        public void Pick(PickType type, IPickFilter filter)
        {
            this.SetPickFilter(filter);
            this.isPick = true;
            this.pickType = type;
            Cursor = CursorGetter.Getter(CursorStyle.Pick);
            //首先删除界面上所有的选择对象
            this.RemoveTemporaryVisualsByType(1);
            this.SelectIntersectGeometrys = new List<IntersectGeometry>();
        }

        /// <summary>
        /// 取消选择
        /// </summary>
        public void UnPick()
        {
            this.isPick = false;
        }


        /// <summary>
        /// 向当前绘制窗体发送键盘操作
        /// </summary>
        /// <param name="key"></param>
        public void SendKeyBorad(Key key, ModifierKeys m)
        {

            if (key == Key.Space)
            {
                if (EAction is OSBAction)
                {
                    var action = EAction as OSBAction;
                    action.Space_OnClick();
                }
            }
            if (key == Key.LeftCtrl)
            {
                if (EAction is OSBAction)
                {
                    var action = EAction as OSBAction;
                    action.Ctrl_OnClick();
                }
            }
            if (key == Key.Z && m == ModifierKeys.Control)
            {

                this.functionalContext.PopFunctional();
            }
            this.ActionTip.SendKeyBorad(key, m);
        }


        /// <summary>
        /// 在当前绘制面板上显示当前的消息
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            this.messageTip.ShowMessage(message);
        }

        /// <summary>
        /// 清理当前所有的绘制工作
        /// </summary>
        public void Clear()
        {

            this.functionalContext.Clear();
        }


        /// <summary>
        /// 设置指定的图形元素高亮
        /// </summary>
        /// <param name="geo"></param>
        internal void SetLightSelected(params Geometry2D[] geos)
        {
            List<Geometry2D> alls = this.GetDrawingVisuals();
            if (geos != null)
            {
                alls.ForEach(x => {

                    if (geos.Contains(x)||x is MeasureGeometry)
                    {
                        x.IsEnabled = true;
                    }
                    else
                    {
                        x.IsEnabled = false;
                    }
                });
      
            }
            else {

                if (alls != null) {

                    alls.ForEach(x => { x.IsEnabled = true; });
                }
                
            }
        }

        /// <summary>
        /// 设置指定的线段集合高亮
        /// </summary>
        /// <param name="region"></param>
        internal void SetHightLight(List<Line2D> region)
        {
            HightLightGeometry hlg = new HightLightGeometry(region);
            this.AddTemporaryVisual(hlg);
        }

        /// <summary>
        /// 当前选中高亮显示
        /// </summary>
        /// <param name="lines"></param>
        internal void SetSelectedLight(Geometry2D g)
        {
            var lines = g.Lines;
            if (lines != null && lines.Count > 0)
            {
                SelectLightGeometry sg = new SelectLightGeometry(lines);
                this.AddTemporaryVisual(sg);
            }

        }
    }



}
