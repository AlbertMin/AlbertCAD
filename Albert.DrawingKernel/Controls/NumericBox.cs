using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Albert.DrawingKernel.Controls
{
    public class NumericBox : TextBox
    {
        private Int32 _index;  //光标位置
        private bool _isReentry; //标识TextChanged事件是否重入

        public int Maximum=int.MaxValue;
        public int Minimum = 0;
        public NumericBox()
        {
            DataObject.AddPastingHandler(this, Text_Pasting);
        }


        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right||e.Key==Key.Enter||e.Key==Key.Space)
            {
                if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }

        }

       

        /// <summary>
        /// 文本内容变更检查
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (_isReentry)
            {
                SelectionStart = _index;
                return;
            }
            _isReentry = true;
            double temp = 0;
            if (double.TryParse(Text, out temp))
            {
                if (temp > Maximum || temp < Minimum)
                {
                    temp = temp > Maximum ? Maximum : Minimum;
                    _index = SelectionStart;
                }
                Text = temp.ToString();
            }
            //类型不正确或者超长会导致转换失败
            else
            {
                Text = Minimum.ToString();
            }
            _isReentry = false;

            base.OnTextChanged(e);
        }

        private void Text_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            //禁止Pasting
            e.CancelCommand();
        }





        /// <summary>
        /// 输入合法性检查
        /// </summary>
        private bool TextCheck()
        {

            return true;
        }

    }
}
