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
            _secondTextureLoaded = false;
            _secondTextureName = String.Empty;
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

            if (_effect == null)
                _effect = new DualTextureEffect(YnG.GraphicsDevice);
        }

        public override void Update(Camera camera, SceneLight light, ref Matrix world, ref FogData fog)
        {
            // Update matrices
            base.Update(camera, light, ref world, ref fog);

            var dualTextureEffect = (DualTextureEffect)_effect;
			
            // Fog
            UpdateFog(dualTextureEffect, ref fog);

            // Textures
            dualTextureEffect.Texture = _texture;
            dualTextureEffect.Texture2 = _secondTexture;
            
            // Lights
            dualTextureEffect.DiffuseColor = DiffuseColor * DiffuseIntensity;
            dualTextureEffect.Alpha = AlphaColor;
        }
    }
}
