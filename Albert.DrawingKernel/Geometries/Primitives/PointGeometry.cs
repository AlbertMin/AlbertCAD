using Albert.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Albert.DrawingKernel.Geometries.Primitives
{
    public class PointGeometry : Geometry2D
    {
        public override List<Line2D> Lines
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override List<Vector2D> Points {

            get{
                return null;
            }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
