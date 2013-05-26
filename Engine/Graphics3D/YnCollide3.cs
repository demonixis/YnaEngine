using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine
{
    public class YnCollider3
    {
        public static bool PointInsideBoundingSphere(Vector3 point, BoundingSphere boudingSphere)
        {
            Vector3 diff = point - boudingSphere.Center;
            return (diff.LengthSquared() < (float)Math.Abs(boudingSphere.Radius * boudingSphere.Radius));
        }

        public static bool PointInsideBoundingBox(Vector3 point, BoundingBox boundingBox)
        {
            if (point.X < boundingBox.Min.X || point.X > boundingBox.Max.X)
                return false;

            if (point.Y < boundingBox.Min.Y || point.Y > boundingBox.Max.Y)
                return false;

            if (point.Z < boundingBox.Min.Z || point.Z > boundingBox.Max.Z)
                return false;

            return true;
        }
    }
}
