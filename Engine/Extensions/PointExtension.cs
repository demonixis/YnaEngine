using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    public static class PointExtension
    {
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
