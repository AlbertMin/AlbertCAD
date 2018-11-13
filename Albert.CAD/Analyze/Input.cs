using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

using Albert.DrawingKernel;
using Albert.DrawingKernel.Geometries;
using Albert.DrawingKernel.Geometries.Primitives;

namespace Albert.CAD.Analyze
{
    public class Input
    {
        private Explorer drawingKernel;
        private MainWindow mainWindow;


        /// <summary>
        /// 构造函数，初始化当前的输入操作
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="drawingKernel"></param>
        public Input(MainWindow mainWindow, Explorer drawingKernel)
        {
            this.mainWindow = mainWindow;
            this.drawingKernel = drawingKernel;
        }


        /// <summary>
        /// 读取数据，并且进行存储
        /// </summary>
        /// <param name="sAVE_PATH"></param>
        internal void ReaderXML(string ReadPath)
        {
            try
            {
                using (FileStream fs = new FileStream(ReadPath, FileMode.Open, FileAccess.Read))
                {
                    using (XmlReader xmlReader = XmlReader.Create(fs)) {

                        while (xmlReader.Read())
                        {
                            if (xmlReader.NodeType == XmlNodeType.Element)
                            {
                                this.ParseXML(xmlReader);
                            }
                        }
                    }
                    
                }
          
            }
            catch (Exception ex) {

                throw ex;
            }
        }

        /// <summary>
        /// 解析指定的XML对象
        /// </summary>
        /// <param name="messageXML"></param>
        private void ParseXML(XmlReader xmlReader)
        {
            Geometry2D shape = null;

            switch (xmlReader.Name) {

                case "ArcGeometry":
                    shape = new ArcGeometry();
                    break;
                case "BeamGeometry":
                    shape = new BeamGeometry();
                    break;
                case "CircleGeometry":
                    shape = new CircleGeometry();
                    break;
                case "CSectionGeometry":
                    shape = new CSectionGeometry();
                    break;
                case "EllipseGeometry":
                    shape = new EllipseGeometry();
                    break;
                case "FloorGeometry":
                    shape = new FloorGeometry();
                    break;
                case "LineGeometry":
                    shape = new LineGeometry();
                    break;
                case "MeasureGeometry":
                    shape = new MeasureGeometry();
                    break;
                case "MemberGeometry":
                    shape = new MemberGeometry();
                    break;
                case "OSBGeometry":
                    shape = new OSBGeometry();
                    break;
                case "PointGeometry":
                    shape = new PointGeometry();
                    break;
                case "PolygonGeometry":
                    shape = new PolygonGeometry();
                    break;
                case "PolylineGeometry":
                    shape = new PolylineGeometry();
                    break;
                case "RectangleGeometry":
                    shape = new RectangleGeometry();
                    break;
                case "SteelBeamGeometry":
                    shape = new SteelBeamGeometry();
                    break;
                case "TextGeometry":
                    shape = new TextGeometry();
                    break;
                case "WallGeometry":
                    shape = new WallGeometry();
                    break;
            }

            if (shape != null) {
                //将信息写入数据流中
                shape.ReadXML(xmlReader);
                //将图形添加都界面上
                this.drawingKernel.AddShape(shape);
            }
       
        }
    }
}
