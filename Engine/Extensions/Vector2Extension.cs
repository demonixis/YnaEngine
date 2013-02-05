using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    public static class Vector2Extension
    {
        public static double GetAngle(this Vector2 va, Vector2 vb)
        {
            return Math.Atan2((vb.Y - va.Y) * -1, vb.X - va.X);
        }
    }
}
