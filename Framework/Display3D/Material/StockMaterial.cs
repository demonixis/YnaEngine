using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Lighting;

namespace Yna.Framework.Display3D.Material
{
    /// <summary>
    /// Define a base class for all XNA stock effects
    /// </summary>
    public abstract class StockMaterial : BaseMaterial, ILightSpecular
    {
        #region Protected declarations

        protected float _alphaColor;
        protected Vector3 _fogColor;
        protected float _fogStart;
        protected float _fogEnd;
        protected Vector3 _emissiveColor;
        protected Vector3 _specularColor;
        protected float _specularIntensity;
        protected Texture2D _mainTexture;
        protected string _mainTextureName;
        protected bool _mainTextureLoaded;
        protected bool _enableMainTexture;
        protected bool _enableFog;
        protected bool _enablePerPixelLighting;
        protected bool _enableVertexColor;
        protected bool _enableDefaultLighting;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets alpha value
        /// </summary>
        public float AlphaColor
        {
            get { return _alphaColor; }
            set { _alphaColor = value; }
        }

        /// <summary>
        /// Gets or sets for color
        /// </summary>
        public Vector3 FogColor
        {
            get { return _fogColor; }
            set { _fogColor = value; }
        }

        /// <summary>
        /// Gets or sets fog start value
        /// </summary>
        public float FogStart
        {
            get { return _fogStart; }
            set { _fogStart = value; }
        }

        /// <summary>
        /// Gets or sets fog end value
        /// </summary>
        public float FogEnd
        {
            get { return _fogEnd; }
            set { _fogEnd = value; }
        }

        /// <summary>
        /// Gets or sets the main texture name
        /// </summary>
        public string MainTextureName
        {
            get { return _mainTextureName; }
            set
            {
                _mainTextureName = value;
                _mainTextureLoaded = false;
            }
        }

        /// <summary>
        /// Gets or sets the main texture used with this effect
        /// </summary>
        public Texture2D MainTexture
        {
            get { return _mainTexture; }
            set { _mainTexture = value; }
        }

        /// <summary>
        /// Gets or sets the value of emissive color
        /// </summary>
        public Vector3 EmissiveColor
        {
            get { return _emissiveColor; }
            set { _emissiveColor = value; }
        }

        /// <summary>
        /// Gets or sets the value of specular color
        /// </summary>
        public Vector4 SpecularColor
        {
            get { return new Vector4(_specularColor.X, _specularColor.Y, _specularColor.Z, 1.0f); }
            set
            {
                _specularColor.X = value.X;
                _specularColor.Y = value.Y;
                _specularColor.Z = value.Z;
            }
        }

        /// <summary>
        /// Gets or sets the value of specular intensity
        /// </summary>
        public float SpecularIntensity
        {
            get { return _specularIntensity; }
            set { _specularIntensity = value; }
        }

        /// <summary>
        /// Enable or disable fog
        /// </summary>
        public bool EnableFog
        {
            get { return _enableFog; }
            set { _enableFog = value; }
        }

        /// <summary>
        /// Enable or disable the default lighting
        /// </summary>
        public bool EnableDefaultLighting
        {
            get { return _enableDefaultLighting; }
            set { _enableDefaultLighting = value; }
        }

        /// <summary>
        /// Enable or disable the main texture
        /// </summary>
        public bool EnableMainTexture
        {
            get { return _enableMainTexture; }
            set { _enableMainTexture = value; }
        }

        /// <summary>
        /// Enable or disable per pixel lighting
        /// </summary>
        public bool EnabledPerPixelLighting
        {
            get { return _enablePerPixelLighting; }
            set { _enablePerPixelLighting = value; }
        }

        /// <summary>
        /// Enable or disable vertex color
        /// </summary>
        public bool EnableVertexColor
        {
            get { return _enableVertexColor; }
            set { _enableVertexColor = value; }
        }

        #endregion

        public StockMaterial()
            : base()
        {
            _alphaColor = 1.0f;
            _fogColor = Color.White.ToVector3();
            _fogStart = 0.0f;
            _fogEnd = 0.0f;
            _emissiveColor = Color.Black.ToVector3();
            _specularColor = Color.Black.ToVector3();
            _specularIntensity = 1.0f;
            _mainTexture = null;
            _mainTextureName = String.Empty;
            _enableDefaultLighting = true;
            _enableFog = false;
            _enableMainTexture = false;
            _enablePerPixelLighting = false;
            _enableVertexColor = false;
            _effectName = "XNA Stock Effect";
            _mainTextureLoaded = false;
        }

        /// <summary>
        /// Load the main texture. If you want to reload another texture, use the MainTextureName propertie and call this method
        /// </summary>
        public override void LoadContent()
        {
            if (!_mainTextureLoaded && _mainTextureName != String.Empty)
            {
                _mainTexture = YnG.Content.Load<Texture2D>(_mainTextureName);
                _enableMainTexture = true;
            }
            else
                _enableMainTexture = false;
        }

        public override void Update(ref Matrix world, ref Matrix view, ref Matrix projection, ref Vector3 position)
        {
            // Matrices
            IEffectMatrices effectMatrices = (IEffectMatrices)_effect;
            effectMatrices.World = world;
            effectMatrices.View = view;
            effectMatrices.Projection = projection;
        }

        protected virtual void UpdateFog(IEffectFog effectFog)
        {
            // Fog
            if (_enableFog)
            {
                effectFog.FogEnabled = _enableFog;
                effectFog.FogColor = _fogColor;
                effectFog.FogStart = _fogStart;
                effectFog.FogEnd = _fogEnd;
            }
        }

        /// <summary>
        /// Update lighting on this effect
        /// </summary>
        /// <param name="effectLights">The light effect</param>
        /// <returns>True if the lighting is enabled</returns>
        protected virtual bool UpdateLights(IEffectLights effectLights)
        {
            // Lights
            if (_enableDefaultLighting)
            {
                effectLights.EnableDefaultLighting();
                return false;
            }
            else
            {
                effectLights.LightingEnabled = !_enableDefaultLighting;

                // TODO To bad, you must make a real and rocky lighting system ok ?

                if (_light != null)
                {
                    effectLights.DirectionalLight0.Enabled = true;
                    effectLights.DirectionalLight0.Direction = _light.Direction;
                    effectLights.DirectionalLight0.DiffuseColor = _light.Diffuse;
                    effectLights.DirectionalLight0.SpecularColor = _light.Specular;

                    // TODO more in the next episode :D
                }

                effectLights.AmbientLightColor = new Vector3(_ambientColor.X, _ambientColor.Y, _ambientColor.Z) * _ambientIntensity;
                
                return true;
            }
        }
    }
}
