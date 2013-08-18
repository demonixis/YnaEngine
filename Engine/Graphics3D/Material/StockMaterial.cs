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
    /// Define a base class for all XNA stock effects
    /// </summary>
    public abstract class StockMaterial : BaseMaterial
    {
        #region Protected declarations

        protected float _alphaColor;
        protected Vector3 _fogColor;
        protected float _fogStart;
        protected float _fogEnd;
        protected Vector3 _emissiveColor;
        protected float _emissiveIntensity;
        protected Vector3 _specularColor;
        protected float _specularIntensity;
        protected bool _enableMainTexture;
        protected bool _enableFog;
        protected bool _enablePerPixelLighting;
        protected bool _enableVertexColor;
        protected bool _enableDefaultLighting;
        protected bool _enableLighting;
        protected SceneLight _basicLight;

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
        /// Gets or sets the value of emissive color
        /// </summary>
        public Vector3 EmissiveColor
        {
            get { return _emissiveColor; }
            set { _emissiveColor = value; }
        }

        public float EmissiveIntensity
        {
            get { return _emissiveIntensity; }
            set { _emissiveIntensity = value; }
        }

        /// <summary>
        /// Gets or sets the value of specular color
        /// </summary>
        public Vector3 SpecularColor
        {
            get { return _specularColor; }
            set { _specularColor = value; }
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
        /// Enable or disable lighting when the default lighting isn't enabled.
        /// </summary>
        public bool EnableLighting
        {
            get { return _enableLighting; }
            set { _enableLighting = value; }
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
            _emissiveIntensity = 0.0f;
            _specularColor = Color.Black.ToVector3();
            _specularIntensity = 1.0f;
            _enableDefaultLighting = true;
            _enableFog = false;
            _enableMainTexture = false;
            _enablePerPixelLighting = false;
            _enableVertexColor = false;
            _enableLighting = false;
            _effectName = "XNA Stock Effect";
        }

        /// <summary>
        /// Load the main texture. If you want to reload another texture, use the MainTextureName propertie and call this method
        /// </summary>
        public override void LoadContent()
        {
            if (!_textureLoaded && _textureName != String.Empty)
            {
                _texture = YnG.Content.Load<Texture2D>(_textureName);
                _enableMainTexture = true;
                _textureLoaded = true;
            }
            else
                _enableMainTexture = _texture != null ? true : false;
        }

        public override void Update(BaseCamera camera, ref Matrix world)
        {
            // Matrices
            IEffectMatrices effectMatrices = (IEffectMatrices)_effect;
            effectMatrices.World = world;
            effectMatrices.View = camera.View;
            effectMatrices.Projection = camera.Projection;
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
                effectLights.LightingEnabled = _enableLighting ;
                effectLights.AmbientLightColor = _ambientColor * _ambientIntensity;

                if (_light is SceneLight)
                {
                    UpdateLighting(effectLights, (SceneLight)_light);
                    effectLights.AmbientLightColor *= _light.AmbientColor * _light.AmbientIntensity;
                }

                return true;
            }
        }

        public static void UpdateLighting(IEffectLights effect, SceneLight light)
        {
            effect.DirectionalLight0.Enabled = light.DirectionalLights[0].Enabled;
            effect.DirectionalLight0.Direction = light.DirectionalLights[0].Direction;
            effect.DirectionalLight0.DiffuseColor = light.DirectionalLights[0].DiffuseColor * light.DirectionalLights[0].DiffuseIntensity;
            effect.DirectionalLight0.SpecularColor = light.DirectionalLights[0].SpecularColor * light.DirectionalLights[0].SpecularIntensity;

            effect.DirectionalLight1.Enabled = light.DirectionalLights[1].Enabled;
            effect.DirectionalLight1.Direction = light.DirectionalLights[1].Direction;
            effect.DirectionalLight1.DiffuseColor = light.DirectionalLights[1].DiffuseColor * light.DirectionalLights[1].DiffuseIntensity;
            effect.DirectionalLight1.SpecularColor = light.DirectionalLights[1].SpecularColor * light.DirectionalLights[1].SpecularIntensity;

            effect.DirectionalLight2.Enabled = light.DirectionalLights[2].Enabled;
            effect.DirectionalLight2.Direction = light.DirectionalLights[2].Direction;
            effect.DirectionalLight2.DiffuseColor = light.DirectionalLights[2].DiffuseColor * light.DirectionalLights[2].DiffuseIntensity;
            effect.DirectionalLight2.SpecularColor = light.DirectionalLights[2].SpecularColor * light.DirectionalLights[2].SpecularIntensity;
        }
    }
}
