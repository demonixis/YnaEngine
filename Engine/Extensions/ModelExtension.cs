using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    public static class ModelExtension
    {
        public static BoundingSphere GetBoundingSphere(this Model model)
        {
            BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, 0);

            foreach (var mesh in model.Meshes)
                boundingSphere = BoundingSphere.CreateMerged(boundingSphere, mesh.BoundingSphere);

            return boundingSphere;
        }
    }
}
