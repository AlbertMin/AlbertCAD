using Albert.DrawingKernel.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albert.Geometry.Primitives;

namespace Albert.DrawingKernel.Geometries.Temporary
{
    /// <summary>
    /// 记录当前的选择框
    /// </summary>
    public class SelectingBox : Geometry2D
    {

        public Vector2D Start;
        public Vector2D End;
        public override List<Line2D> Lines
        {
            get
            {
                return null;
            }
        }

        public override List<Vector2D> Points
        {
            get
            {
                return null;
            }
        }

        public override void Update()
        {
            if (Start != null && End != null)
            {
                List<Vector2D> points = new List<Vector2D>();
                var v1 = Start;
                var v3 = End;
                var v2 = new Vector2D(v1.X, v3.Y);
                var v4 = new Vector2D(v3.X, v1.Y);
                points.Add(v1);
                points.Add(v2);
                points.Add(v3);
                points.Add(v4);
                points.Add(v1);

                this.DrawFill(points);

            }
        }
    }
}
