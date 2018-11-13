using Albert.DrawingKernel.Filter;
using Albert.DrawingKernel.Functional;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.PenAction;
using Albert.DrawingKernel.Selector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albert.DrawingKernel.Commands
{

    /// <summary>
    /// 记录当前动作的委托
    /// </summary>
    /// <param name="bf"></param>
    public delegate void RecordCommand(Object sender,BaseFunctional bf);
    /// <summary>
    /// 定义一个命令的接口信息
    /// </summary>
    public interface ICommand : IAction
    {

        event RecordCommand RecordCommand;
        /// <summary>
        /// 设置当前命令操作的图形元素
        /// </summary>
        /// <param name="shape"></param>
        void SetIntersectGeometry(IntersectGeometry shape);

        //返回当前命令过滤选中指定对象
        void SetPickFilter(IPickFilter f);
        /// <summary>
        /// 当前命令的目标对象
        /// </summary>
        /// <returns></returns>
        Geometry2D GetTarget();

        

    }
}
