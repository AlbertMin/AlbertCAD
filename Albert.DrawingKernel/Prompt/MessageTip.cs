using Albert.DrawingKernel.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Albert.DrawingKernel.Prompt
{
    public class MessageTip : Popup
    {
        private DrawingControl DrawingControl = null;

        private TextTip texttip = new TextTip();

        /// <summary>
        /// 时间提示
        /// </summary>
        private Timer timer = new Timer(1000);
        /// <summary> 
        /// 构造函数
        /// </summary>
        /// <param name="dc"></param>
        public MessageTip(DrawingControl dc) {
            this.DrawingControl = dc;
            this.Width = 300;
            this.Height = 60;
            this.PlacementTarget = dc;
            this.Placement = PlacementMode.Center;
            this.AllowsTransparency = true;
            this.Child = texttip;
            this.StaysOpen = false;
            this.PopupAnimation = PopupAnimation.Slide;
            this.IsOpen = false;
            timer.Elapsed += timer_Elapsed;
        }

        /// <summary>
        /// 到达时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            //关闭当前的提示
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate() {

                this.IsOpen = false;
            });
        }
        /// <summary>
        /// 操作提示框
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message) {

            this.IsOpen = true;
            //设置当前的提示框
            texttip.MessageText.Content = message;
            timer.Start();
        }
    }
}
