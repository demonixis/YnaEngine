// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Material
{
    /// <summary>
    /// A material who use Xna's stock effect BasicEffect
    /// </summary>
    public class BasicMaterial : StockMaterial
    {
        /// <summary>
        /// Create a basic material with a BasicEffect
        /// </summary>
        public BasicMaterial()
            : base()
        {
            _effectName = "BasicEffect";
            _enableDefaultLighting = false;
            _enableLighting = true;
        }

        /// <summary>
        /// Create a basic material with a BasicEffect and a texture
        /// </summary>
        /// <param name="textureName">The texture name</param>
        public BasicMaterial(string textureName)
            : this()
        {
            _textureName = textureName;
            _enableMainTexture = true;
        }

        public BasicMaterial(Texture2D texture)
            : this()
        {
            _texture = texture;
            _textureLoaded = true;
            _enableMainTexture = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (!_effectLoaded)
            {
                _effect = new BasicEffect(YnG.GraphicsDevice);
                _effectLoaded = true;
            }
        }

        public override void Update(BaseCamera camera, ref Matrix world)
        {
            // Update matrices
            base.Update(camera, ref world);

            BasicEffect basicEffect = (BasicEffect)_effect;

            // Texture
            basicEffect.TextureEnabled = _enableMainTexture;
            basicEffect.Texture = _texture;

            // Fog
            UpdateFog(basicEffect);

            // Lights
            if (UpdateLights(basicEffect))
            {
                basicEffect.PreferPerPixelLighting = _enablePerPixelLighting;
                basicEffect.EmissiveColor = _emissiveColor;
                basicEffect.DiffuseColor = _diffuseColor * _diffuseIntensity;
                basicEffect.SpecularColor = _specularColor * _specularIntensity;
                basicEffect.Alpha = _alphaColor;
            }
        }
    }
}
