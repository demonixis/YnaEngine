// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    public static class ModelExtension
    {
        /// <summary>
        /// Gets the bounding sphere of a model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Return the bounding sphere of the model.</returns>
        public static BoundingSphere GetBoundingSphere(this Model model)
        {
            BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, 0);

            foreach (var mesh in model.Meshes)
                boundingSphere = BoundingSphere.CreateMerged(boundingSphere, mesh.BoundingSphere);

            return boundingSphere;
        }
    }
}
