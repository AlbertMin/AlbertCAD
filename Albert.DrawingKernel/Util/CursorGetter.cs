using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;

namespace Albert.DrawingKernel.Util
{
    /// <summary>
    /// 用于获取鼠标样式
    /// </summary>
    public class CursorGetter
    {

        /// <summary>
        /// 获取鼠标的样式
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static Cursor Getter(CursorStyle style)
        {

            Cursor cursor = Cursors.Arrow;
            try
            {
                //获取要切换的鼠标样式类型
                switch (style)
                {

                    case CursorStyle.Cross:
                        cursor = GetResource(@"pack://application:,,,/Albert.DrawingKernel;component/Resources/Cursor/cross.cur");
                        break;
                    case CursorStyle.Move:
                        cursor = GetResource(@"pack://application:,,,/Albert.DrawingKernel;component/Resources/Cursor/aero_move.cur");
                        break;
                    case CursorStyle.Pick:
                        cursor = GetResource(@"pack://application:,,,/Albert.DrawingKernel;component/Resources/Cursor/pick.cur");
                        break;
                    case CursorStyle.Command:
                        cursor = GetResource(@"pack://application:,,,/Albert.DrawingKernel;component/Resources/Cursor/select.cur");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return cursor;
        }

        /// <summary>
        /// 获取鼠标的样式
        /// </summary>
        /// <returns></returns>
        private static Cursor GetResource(string imageUrl)
        {

            Uri u = new Uri(imageUrl, UriKind.Absolute);
            StreamResourceInfo ri = Application.GetResourceStream(u);

           
            Cursor customCursor = new Cursor(ri.Stream);

            return customCursor;
        }
    }

    /// <summary>
    /// 当时鼠标的样式枚举
    /// </summary>
    public enum CursorStyle
    {

        Cross = 0,
        Move=1,
        Pick=2,
        Command=3

    }
}
