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
            _enabledDefaultLighting = false;
        }

        /// <summary>
        /// Create a basic material with a BasicEffect and a texture
        /// </summary>
        /// <param name="textureName">The texture name</param>
        public BasicMaterial(string textureName)
            : this()
        {
            _mainTextureName = textureName;
            _enabledMainTexture = true;
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
            BasicEffect basicEffect = (BasicEffect)_effect;

            // Matrices
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            
            // Texture
            basicEffect.TextureEnabled = _enabledMainTexture;
            basicEffect.Texture = _mainTexture;

            // Fog
            if (_enabledFog)
            {
                basicEffect.FogEnabled = _enabledFog;
                basicEffect.FogColor = _fogColor;
                basicEffect.FogStart = _fogStart;
                basicEffect.FogEnd = _fogEnd;
            }

            // Lights
            if (_enabledDefaultLighting)
            {
                basicEffect.EnableDefaultLighting();
            }
            else
            {
                basicEffect.LightingEnabled = !_enabledDefaultLighting;
                
                // TODO To bad, you must make a real and rocky lighting system ok ?
                
                if (_light != null)
                {
                    basicEffect.DirectionalLight0.Enabled = true;
                    basicEffect.DirectionalLight0.Direction = _light.Direction;
                    basicEffect.DirectionalLight0.DiffuseColor = _light.Diffuse;
                    basicEffect.DirectionalLight0.SpecularColor = _light.Specular;

                    // TODO more in the next episode :D
                }

                basicEffect.AmbientLightColor = new Vector3(_ambientColor.X, _ambientColor.Y, _ambientColor.Z) * _ambientIntensity;
                basicEffect.EmissiveColor = _emissiveColor;
                basicEffect.DiffuseColor = new Vector3(_diffuseColor.X, _diffuseColor.Y, _diffuseColor.Z) * _diffuseIntensity;
                basicEffect.SpecularColor = new Vector3(_specularColor.X, _specularColor.Y, _specularColor.Z) * _specularIntensity;
                basicEffect.Alpha = _alphaColor;
            }
        }
    }
}
