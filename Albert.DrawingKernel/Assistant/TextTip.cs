using Albert.Geometry.External;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Albert.DrawingKernel.Assistant
{

    /// <summary>
    /// 当前提示框的事务
    /// </summary>
    /// <param name="value"></param>
    public delegate void TextTipHandle(double value);

    /// <summary>
    /// 一个提示框
    /// </summary>
    public class TextTip : Popup
    {
        private TextBox textBox = null;

        private bool isEditable = false;

        /// <summary>
        /// 提示改变事件
        /// </summary>
        public event TextTipHandle TipChanged;

        /// <summary>
        /// 提示回车结束事件
        /// </summary>
        public event TextTipHandle TipCompleted;

        /// <summary>
        /// ESC按钮的按下事件
        /// </summary>
        public event TextTipHandle ESCPressed;

        /// <summary>
        /// 构造函数，初始化当前文本提示框
        /// </summary>
        public TextTip()
        {
            AllowsTransparency = true;
            PopupAnimation = PopupAnimation.Slide;
            Placement = PlacementMode.Top;
            textBox = new TextBox();
            System.Windows.Input.InputMethod.SetIsInputMethodEnabled(textBox, false);
            textBox.FontFamily = new System.Windows.Media.FontFamily("微软雅黑");
            this.Child = textBox;
            textBox.KeyDown += TextBox_KeyDown;
            this.SetDefault();
        }

        /// <summary>
        /// 当前文本的改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //说明按下了ESC
            if (e.Key == Key.Escape && e.KeyboardDevice.Modifiers == ModifierKeys.None) {

                if (ESCPressed != null) {

                    ESCPressed(0);
                }
            }

            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Back || e.Key == Key.Enter)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.None)
                {
                    if (!isEditable && (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9))
                    {
                        SetEditable();
                    }
                    else
                    {
                        //假如点击的回车
                        if (e.Key == Key.Enter) {

                            if (TipCompleted != null) {

                                double result = 0;
                                if (double.TryParse(textBox.Text, out result))
                                {
                                    TipCompleted(result);
                                    this.SetHide();
                                }
                                else
                                {
                                    textBox.Foreground = new SolidColorBrush(Colors.Red);
                                }
                            }
                        }

                    }
                }
            }
       

        }

        /// <summary>
        /// 设置当前的样式
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text,Vector2D p,double angle)
        {
            this.IsOpen = true;
            var textPoint = KernelProperty.MMToPix(p);
            this.HorizontalOffset = textPoint.X;
            this.VerticalOffset = textPoint.Y;
            RotateTransform rtf = new RotateTransform();
            rtf.CenterX = textPoint.X;
            rtf.CenterY = textPoint.Y;
            rtf.Angle =angle;
            this.RenderTransform = rtf;
            textBox.Text = text;
            textBox.Focus();
            this.SetDefault();
        }


        /// <summary>
        /// 设置默认状态
        /// </summary>
        public void SetDefault() {
            textBox.FontSize = 14;
            textBox.Foreground = new SolidColorBrush(Colors.Blue);
            textBox.Background = new SolidColorBrush(Colors.Transparent);
            textBox.BorderThickness = new System.Windows.Thickness(0);
            textBox.IsReadOnly = true;
        }

        /// <summary>
        /// 隐藏当前的提示框
        /// </summary>
        public void SetHide() {
            this.textBox.Text = "";
            this.IsOpen = false;
        }
        /// <summary>
        /// 设置为编辑状态
        /// </summary>
        public void SetEditable()
        {
            if (textBox.IsReadOnly)
            {
                textBox.FontSize = 14;
                textBox.Foreground = new SolidColorBrush(Colors.Blue);
                textBox.BorderThickness = new System.Windows.Thickness(1);
                textBox.IsReadOnly = false;
                textBox.SelectAll();
                isEditable = true;
                if (TipChanged != null)
                {
                    TipChanged(0);
                }
            }
            textBox.Foreground = new SolidColorBrush(Colors.Blue);
        }


    }
}
