using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Albert.CAD
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string DocumentUrl = null;
        /// <summary>
        /// 当前应用程序的打开
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            string[] arguments = e.Args;
            if (arguments.Length == 1 )
            {
                if (arguments[0].EndsWith(".cdo"))
                {
                    DocumentUrl = arguments[0];
                }
                else
                {
                    MessageBox.Show("打不开此文件的数据", "错误", MessageBoxButton.OK, MessageBoxImage.Error);

                    Application.Current.Shutdown();
                }
            }

            base.OnStartup(e);
        }
    }
}
