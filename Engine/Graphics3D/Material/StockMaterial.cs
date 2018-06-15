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
    using XnaDirectionalLight = Microsoft.Xna.Framework.Graphics.DirectionalLight;

    /// <summary>
    /// Define a base class for all XNA stock effects
    /// </summary>
    public abstract class StockMaterial : Material
    {
        #region Protected declarations

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default diffuse color
        /// </summary>
        public Vector3 DiffuseColor { get; set; } = Vector3.One;

        /// <summary>
        /// Gets or sets the default diffuse intensity
        /// </summary>
        public float DiffuseIntensity { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets alpha value
        /// </summary>
        public float AlphaColor { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets the value of emissive color
        /// </summary>
        public Vector3 EmissiveColor { get; set; } = Vector3.Zero;

        public float EmissiveIntensity { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets the value of specular color
        /// </summary>
        public Vector3 SpecularColor { get; set; } = new Vector3(0.3f);

        /// <summary>
        /// Gets or sets the value of specular intensity
        /// </summary>
        public float SpecularIntensity { get; set; } = 0.4f;

        /// <summary>
        /// Enable or disable the default lighting
        /// </summary>
        public bool EnableDefaultLighting { get; set; } = false;

        /// <summary>
        /// Enable or disable lighting when the default lighting isn't enabled.
        /// </summary>
        public bool EnableLighting { get; set; } = true;

        /// <summary>
        /// Enable or disable per pixel lighting
        /// </summary>
        public bool PreferPerPixelLighting { get; set; } = true;

        /// <summary>
        /// Enable or disable vertex color
        /// </summary>
        public bool EnableVertexColor { get; set; } = false;

        #endregion

        /// <summary>
        /// Load the main texture. If you want to reload another texture, use the MainTextureName propertie and call this method
        /// </summary>
        public override void LoadContent()
        {
            if (_texture == null && _textureName != String.Empty)
                _texture = YnG.Content.Load<Texture2D>(_textureName);
        }

        public override void Update(Camera camera, SceneLight light, ref Matrix world, ref FogData fog)
        {
            // Matrices
            var effectMatrices = (IEffectMatrices)_effect;
            effectMatrices.World = world;
            effectMatrices.View = camera.View;
            effectMatrices.Projection = camera.Projection;
        }

        protected virtual void UpdateFog(IEffectFog effectFog, ref FogData data)
        {
            effectFog.FogEnabled = data.Enabled;
            effectFog.FogColor = data.Color;
            effectFog.FogStart = data.Start;
            effectFog.FogEnd = data.End;
        }

        /// <summary>
        /// Update lighting on this effect
        /// </summary>
        /// <param name="effectLights">The light effect</param>
        /// <returns>True if the lighting is enabled</returns>
        protected virtual bool UpdateLights(IEffectLights effectLights, SceneLight light)
        {
            effectLights.LightingEnabled = EnableLighting;

            if (!EnableDefaultLighting && !light.DefaultLighting)
            {
                effectLights.AmbientLightColor = light.AmbientColor * light.AmbientIntensity;
                effectLights.AmbientLightColor *= light.AmbientColor * light.AmbientIntensity;
                UpdateLighting(effectLights, light);
            }
            else
                effectLights.EnableDefaultLighting();

            return !EnableDefaultLighting;
        }

        public static void UpdateLighting(IEffectLights effect, SceneLight light)
        {
            UpdateLight(effect.DirectionalLight0, 0, light);
            UpdateLight(effect.DirectionalLight1, 1, light);
            UpdateLight(effect.DirectionalLight2, 2, light);
        }

        public static void UpdateLight(XnaDirectionalLight directionalLight, int index, SceneLight light)
        {
            directionalLight.Enabled = light.DirectionalLights[index].Enabled;
            directionalLight.Direction = light.DirectionalLights[index].Direction;
            directionalLight.DiffuseColor = light.DirectionalLights[index].DiffuseColor * light.DirectionalLights[index].DiffuseIntensity;
            directionalLight.SpecularColor = light.DirectionalLights[index].SpecularColor * light.DirectionalLights[index].SpecularIntensity;
        }
    }
}
