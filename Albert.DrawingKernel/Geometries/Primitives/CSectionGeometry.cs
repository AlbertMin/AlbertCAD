using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Albert.Geometry.Primitives;
using Albert.DrawingKernel.Util;

namespace Albert.DrawingKernel.Geometries.Primitives
{
    public class CSectionGeometry : BeamGeometry
    {
        public CSectionGeometry()
        {
        }

        public CSectionGeometry(Vector2D start, Vector2D end, double thickness) : base(start, end, thickness)
        {
            
        }
    }
}
