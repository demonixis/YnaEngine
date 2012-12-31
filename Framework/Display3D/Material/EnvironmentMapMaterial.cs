using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Lighting;
using Yna.Framework.Display3D.Camera;

namespace Yna.Framework.Display3D.Material
{
    public class EnvironmentMapMaterial : StockMaterial
    {
        protected TextureCube _environmentTexture;
        protected string _environmentTextureName;
        protected bool _environmentTextureLoaded;
        protected float _environmentAmount;
        protected Vector3 _environmentSpecular;
        protected float _fresnelFactor;

        public float EnvironmentAmount
        {
            get { return _environmentAmount; }
            set
            {
                if (value > 1.0f)
                    _environmentAmount = 1.0f;
                else if (value < 0.0f)
                    _environmentAmount = 0.0f;
                else
                    _environmentAmount = value;
            }
        }

        public EnvironmentMapMaterial()
        {
            _environmentTextureLoaded = false;
            _environmentAmount = 1.0f;
            _environmentSpecular = Color.Black.ToVector3();
            _fresnelFactor = 0.0f; // Disabled
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (!_environmentTextureLoaded)
            {
                _environmentTexture = YnG.Content.Load<TextureCube>(_environmentTextureName);
                _environmentTextureLoaded = true;
            }

            if (!_effectLoaded)
            {
                _effect = new EnvironmentMapEffect(YnG.GraphicsDevice);
                _effectLoaded = true;
            }
        }

        public override void Update(ref Matrix world, ref Matrix view, ref Matrix projection, ref Vector3 position)
        {
            EnvironmentMapEffect environmentMapEffect = (EnvironmentMapEffect)_effect;

            // Matrices
            environmentMapEffect.World = world;
            environmentMapEffect.View = view;
            environmentMapEffect.Projection = projection;
            
            // Texture
            environmentMapEffect.Texture = _mainTexture;
            environmentMapEffect.EnvironmentMap = _environmentTexture;
            environmentMapEffect.EnvironmentMapAmount = _environmentAmount;
            environmentMapEffect.EnvironmentMapSpecular = _environmentSpecular;
            environmentMapEffect.FresnelFactor = _fresnelFactor;

            // Fog
            if (_enabledFog)
            {
                environmentMapEffect.FogEnabled = _enabledFog;
                environmentMapEffect.FogColor = _fogColor;
                environmentMapEffect.FogStart = _fogStart;
                environmentMapEffect.FogEnd = _fogEnd;
            }

            // Lights
            if (_enabledDefaultLighting)
            {
                environmentMapEffect.EnableDefaultLighting();
            }
            else
            {
                // TODO To bad, you must make a real and rocky lighting system ok ?
                if (_light != null)
                {
                    environmentMapEffect.DirectionalLight0.Enabled = true;
                    environmentMapEffect.DirectionalLight0.Direction = _light.Direction;
                    environmentMapEffect.DirectionalLight0.DiffuseColor = _light.Diffuse;
                    environmentMapEffect.DirectionalLight0.SpecularColor = _light.Specular;

                    // TODO more in the next episode :D
                }

                environmentMapEffect.AmbientLightColor = new Vector3(_ambientColor.X, _ambientColor.Y, _ambientColor.Z) * _ambientIntensity;
                environmentMapEffect.EmissiveColor = _emissiveColor;
                environmentMapEffect.DiffuseColor = new Vector3(_diffuseColor.X, _diffuseColor.Y, _diffuseColor.Z) * _diffuseIntensity;
                environmentMapEffect.Alpha = _alphaColor;
            }
        }
    }
}
