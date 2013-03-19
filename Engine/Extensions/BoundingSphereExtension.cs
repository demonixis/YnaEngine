namespace Microsoft.Xna.Framework
{
    public static class BoundingSphereExtension
    {
        public static Vector3 GetMin(this BoundingSphere boudingSphere)
        {
            Vector3 min = new Vector3(
                    boudingSphere.Center.X - boudingSphere.Radius,
                    boudingSphere.Center.Y - boudingSphere.Radius,
                    boudingSphere.Center.Z - boudingSphere.Radius);

            return min;
        }

        public static Vector3 GetMax(this BoundingSphere boudingSphere)
        {
            Vector3 min = new Vector3(
                    boudingSphere.Center.X + boudingSphere.Radius,
                    boudingSphere.Center.Y + boudingSphere.Radius,
                    boudingSphere.Center.Z + boudingSphere.Radius);
            return min;
        }

        public static BoundingBox ToBoundingBox(this BoundingSphere sphere)
        {
            var boundingBox = new BoundingBox(sphere.GetMin(), sphere.GetMax());
            return boundingBox;
        }
    }
}
