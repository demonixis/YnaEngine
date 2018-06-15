// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Cameras;

namespace Yna.Engine.Graphics3D.Materials
{
    /// <summary>
    /// A material who use Xna's stock effect BasicEffect
    /// </summary>
    public class BasicMaterial : StockMaterial
    {
        /// <summary>
        /// Create a basic material with a BasicEffect
        /// </summary>
        public BasicMaterial() : base() { }

        /// <summary>
        /// Create a basic material with a BasicEffect and a texture
        /// </summary>
        /// <param name="textureName">The texture name</param>
        public BasicMaterial(string textureName)
            : this()
        {
            _textureName = textureName;
        }

        public BasicMaterial(Texture2D texture)
            : this()
        {
            _texture = texture;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (_effect != null)
                return;

            _effect = new BasicEffect(YnG.GraphicsDevice);
        }

        public override void Update(Camera camera, SceneLight light, ref Matrix world, ref FogData fog)
        {
            // Update matrices
            base.Update(camera, light, ref world, ref fog);

            BasicEffect basicEffect = (BasicEffect)_effect;

            // Texture
            basicEffect.TextureEnabled = _texture != null;
            basicEffect.Texture = _texture;

            // Fog
            UpdateFog(basicEffect, ref fog);

            // Lights
            if (UpdateLights(basicEffect, light))
            {
                basicEffect.PreferPerPixelLighting = PreferPerPixelLighting;
                basicEffect.EmissiveColor = EmissiveColor;
                basicEffect.DiffuseColor = DiffuseColor * DiffuseIntensity;
                basicEffect.SpecularColor = SpecularColor * SpecularIntensity;
                basicEffect.Alpha = AlphaColor;
            }
        }
    }
}
