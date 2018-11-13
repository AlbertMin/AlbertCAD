using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Albert.DrawingKernel.Events;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.PenAction
{


    /// <summary>
    /// 代表当前绘制结束
    /// </summary>
    public delegate void DrawHandler(ActionEventArgs gs);

    /// <summary>
    /// 定义绘制的基本动作
    /// </summary>
    public interface IAction
    {

        /// <summary>
        /// 绘制的几个事件
        /// </summary>
        event DrawHandler DrawStartEvent;
        event DrawHandler DrawEraseEvent;
        event DrawHandler DrawCompleteEvent;
        event DrawHandler DrawTermimationEvent;



        /// <summary>
        /// 设置绘制点
        /// </summary>
        /// <param name="p"></param>
        void SetPoint(Vector2D p);
        /// <summary>
        /// 橡皮筋的拉伸效果
        /// </summary>
        /// <param name="p"></param>
        void Erase(Vector2D p);


        /// <summary>
        /// 代表传入数据，进行绘制
        /// </summary>
        void Complete();

        /// <summary>
        /// 终止当前的绘制
        /// </summary>
        void Termimal();

        /// <summary>
        /// 获取绘制的最后一个点
        /// </summary>
        /// <returns></returns>
        Vector2D GetLastPoint();

        /// <summary>
        /// 获取上一步的绘制点
        /// </summary>
        /// <returns></returns>
        Vector2D GetPreviousPoint();
        /// <summary>
        /// 获取当前绘制的图形对象
        /// </summary>
        /// <typeparam name="Geometry"></typeparam>
        /// <returns></returns>
        Geometry2D Geometry { get; }

        /// <summary>
        /// 直接给定结果
        /// </summary>
        /// <param name="result"></param>
        void SetResult(dynamic result);
        /// <summary>
        /// 获取当前的动作
        /// </summary>
        /// <returns></returns>
        string GetCommandStatus();

        /// <summary>
        /// 暂停当前的绘制动作
        /// </summary>
        void Pause();
    }
}
