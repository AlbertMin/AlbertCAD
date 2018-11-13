using Albert.Geometry.External;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Controls;
using Albert.DrawingKernel.PenAction;
using Albert.DrawingKernel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Albert.DrawingKernel.Assistant
{
    /// <summary>
    /// 当前的基础提示
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseTip<T> where T : IAction
    {

        private TextTip tip = null;

        /// <summary>
        /// 获取当前的文本提示
        /// </summary>
        public TextTip Tip
        {

            get
            {
                return tip;
            }
        }

        /// <summary>
        /// 获取当前的绘制面板
        /// </summary>
        protected DrawingControl drawingControl = null;

        /// <summary>
        /// 当前的输出文本对象
        /// </summary>
        private string output = "";

        /// <summary>
        /// 构造函数，初始化当前的提示框
        /// </summary>
        /// <param name="dc"></param>
        public BaseTip(DrawingControl dc,TextTip tip)
        {

            drawingControl = dc;
            this.tip = tip;
            this.tip.PlacementTarget = this.drawingControl;
        }

        /// <summary>
        /// 给当前绘制输入键盘信息
        /// </summary>
        /// <param name="key"></param>
        public void SendKend(Key key) {

            if (key == Key.Back || key == Key.Enter)
            {

            }
            else {

                output += key.ToString();

            }
        }

        /// <summary>
        /// 取消关心动作
        /// </summary>
        public abstract void unAttention();

        /// <summary>
        /// 当前关心的动作
        /// </summary>
        /// <param name="a"></param>
        public abstract void Attention(T a);



    
    }
}
