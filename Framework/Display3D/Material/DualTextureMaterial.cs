﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Lighting;
using Yna.Framework.Display3D.Camera;

namespace Yna.Framework.Display3D.Material
{
    public class DualTextureMaterial : StockMaterial
    {
        protected Texture2D _secondTexture;
        protected string _secondTextureName;
        protected bool _secondTextureLoaded;

        /// <summary>
        /// Gets or sets the second texture
        /// </summary>
        public Texture2D SecondTexture
        {
            get { return _secondTexture; }
            set { _secondTexture = value; }
        }

        private DualTextureMaterial()
            : base()
        {
            _enableDefaultLighting = false;
            _secondTextureLoaded = false;
            _secondTextureName = String.Empty;
            _effectName = "DualTextureEffect";
        }

        /// <summary>
        /// Create a DualTextureMaterial with two textures
        /// </summary>
        /// <param name="textureName">First texture name</param>
        /// <param name="secondTextureName">Second texture name</param>
        public DualTextureMaterial(string textureName, string secondTextureName)
            : this()
        {
            _textureName = textureName;
            _secondTextureName = secondTextureName;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (!_secondTextureLoaded && _secondTextureName != String.Empty)
            {
                _secondTexture = YnG.Content.Load<Texture2D>(_secondTextureName);
                _secondTextureLoaded = true;
            }

            if (!_effectLoaded)
            {
                _effect = new DualTextureEffect(YnG.GraphicsDevice);
                _effectLoaded = true;
            }
        }

        public override void Update(ref Matrix world, ref Matrix view, ref Matrix projection, ref Vector3 position)
        {
            // Update matrices
            base.Update(ref world, ref view, ref projection, ref position);

            DualTextureEffect dualTextureEffect = (DualTextureEffect)_effect;

            // Fog
            UpdateFog(dualTextureEffect);

            // Textures
            dualTextureEffect.Texture = _texture;
            dualTextureEffect.Texture2 = _secondTexture;
            
            // Lights
            dualTextureEffect.DiffuseColor = new Vector3(_diffuseColor.X, _diffuseColor.Y, _diffuseColor.Z) * _diffuseIntensity;
            dualTextureEffect.Alpha = _alphaColor;
        }
    }
}
