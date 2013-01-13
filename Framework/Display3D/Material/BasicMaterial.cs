using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Lighting;
using Yna.Framework.Display3D.Camera;

namespace Yna.Framework.Display3D.Material
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

        public override void LoadContent()
        {
            base.LoadContent();

            if (!_effectLoaded)
            {
                _effect = new BasicEffect(YnG.GraphicsDevice);
                _effectLoaded = true;
            }
        }

        public override void Update(ref Matrix world, ref Matrix view, ref Matrix projection, ref Vector3 position)
        {
            // Update matrices
            base.Update(ref world, ref view, ref projection, ref position);

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
