// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D.Materials
{
    /// <summary>
    /// Base class for define a Material
    /// </summary>
    public abstract class Material
    {
        protected Effect _effect;
        protected Texture2D _texture;
        protected string _textureName = string.Empty;
 
        #region Properties

        /// <summary>
        /// Gets or sets the current effect
        /// </summary>
        public Effect Effect
        {
            get => _effect;
            set => _effect = value;
        }

        /// <summary>
        /// Gets or sets the main texture used with this effect
        /// </summary>
        public Texture2D Texture { get; set; }

        #endregion

        public abstract void LoadContent();

        /// <summary>
        /// Update the shader values
        /// </summary>
        /// <param name="camera">Camera to use.</param>
        /// <param name="world">Entity World matrix</param>
        public abstract void Update(Camera camera, SceneLight light, ref Matrix world, ref FogData fog);
    }
}
