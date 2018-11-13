using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Albert.DrawingKernel;
using Albert.DrawingKernel.Geometries;

namespace Albert.CAD.Analyze
{
    public class Output
    {
        private Explorer drawingKernel;


        private MainWindow mainWindow;

        /// <summary>
        /// 构造函数，初始化当前的数据输出类
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="drawingKernel"></param>
        public Output(MainWindow mainWindow, Explorer drawingKernel)
        {
            this.mainWindow = mainWindow;
            this.drawingKernel = drawingKernel;
        }

        /// <summary>
        /// 将数据保存为文件
        /// </summary>
        internal void WriteXML(string WritePath)
        {
            try
            {
                using (FileStream fs = new FileStream(WritePath, FileMode.Create, FileAccess.Write))
                {
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.Indent = true;
                    xws.Encoding = new UTF8Encoding(false);
                    xws.NewLineChars = Environment.NewLine;
                    XmlWriter sw = XmlWriter.Create(fs, xws);
                    sw.WriteStartDocument(false);
                    sw.WriteStartElement("Shapes");
                    List<Geometry2D> gss = this.drawingKernel.GeometryShapes;

                    if (gss != null)
                    {
                        foreach (Geometry2D gs in gss)
                        {
                            gs.WriteXML(sw);
                        }
                    }
                    sw.WriteEndElement();
                    sw.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
