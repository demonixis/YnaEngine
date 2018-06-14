// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Materials;
using Yna.Engine.Graphics3D.Cameras;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// A mesh object that contains a geometry and a material.
    /// </summary>
    public abstract class YnMesh : YnEntity3D
    {
        protected Materials.Material _material;

        /// <summary>
        /// Gets or sets the material for this object
        /// </summary>
        public Material Material
        {
            get => _material;
            set
            {
                _material = value;
                if (_initialized && !_material.Loaded)
                    _material.LoadContent();
            }
        }

        /// <summary>
        /// Update matrix world, bounding volumes if the mesh is dynamic and update the material.
        /// </summary>
        public virtual void PreDraw(Camera camera)
        {
            UpdateMatrix();

            if (!_static)
                UpdateBoundingVolumes();

            _material.Update(camera, ref _world);
        }
    }
}
