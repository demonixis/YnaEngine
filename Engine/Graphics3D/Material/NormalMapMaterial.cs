// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Material
{
    public class NormalMapMaterial : BaseMaterial
    {
        protected Texture2D _normalMap;
        protected string _normalMapName;
        protected Vector4 _specularColor;
        protected float _specularIntensity;
        protected Vector3 _diffuseDirection;

        /// <summary>
        /// Gets the normal texture
        /// </summary>
        public Texture2D NormalTexture
        {
            get { return _normalMap; }
            set { _normalMap = value; }
        }

        /// <summary>
        /// Gets or sets the specular color
        /// </summary>
        public Vector4 SpecularColor
        {
            get { return _specularColor; }
            set { _specularColor = value; }
        }

        /// <summary>
        /// Gets or sets the specular intensity
        /// </summary>
        public float SpecularIntensitiy
        {
            get { return _specularIntensity; }
            set { _specularIntensity = value; }
        }

        /// <summary>
        /// Gets or sets the diffuse direction
        /// </summary>
        public Vector3 DiffuseDirection
        {
            get { return _diffuseDirection; }
            set { _diffuseDirection = value; }
        }

        public NormalMapMaterial(string diffuseMapName, string normalMapName)
        {
            _diffuseIntensity = 1.0f;
            _textureName = diffuseMapName;
            _normalMapName = normalMapName;
            _effectName = "NormalMapEffect";
            _specularColor = Color.Black.ToVector4();
            _specularIntensity = 1.0f;
            _diffuseDirection = Vector3.Zero;
            _textureLoaded = false;
        }

        public NormalMapMaterial(string diffuseMapName, string normalMapName, string effectName)
            : this(diffuseMapName, normalMapName)
        {
            _effectName = effectName;
        }

        public override void LoadContent()
        {
            _texture = YnG.Content.Load<Texture2D>(_textureName);
            _normalMap = YnG.Content.Load<Texture2D>(_normalMapName);
#if MONOGAME
            _effect = LoadShaderFromResources();
#else
            _effect = YnG.Content.Load<Effect>(_effectName);
#endif
        }

#if MONOGAME
        private Effect LoadShaderFromResources()
        {
            string suffix = "ogl";
#if DIRECTX
           suffix = "dx11"; 
#endif
#if WINDOWS_STOREAPP
            var assembly = typeof(NormalMapMaterial).GetTypeInfo().Assembly;
#else
            var assembly = Assembly.GetExecutingAssembly();
#endif
            var stream = assembly.GetManifestResourceStream(String.Format("Yna.Engine.Graphics3D.Material.Resources.NormalMapEffect.{0}.mgfxo", suffix));
            byte[] shaderCode;

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                shaderCode = ms.ToArray();
            }

            return new Effect(YnG.GraphicsDevice, shaderCode);
        }
#endif

        public override void Update(BaseCamera camera, ref Matrix world)
        {
            // Matrices
            _effect.Parameters["World"].SetValue(world);
            _effect.Parameters["View"].SetValue(camera.View);
            _effect.Parameters["Projection"].SetValue(camera.Projection);

            // Lights
            _effect.Parameters["AmbientColor"].SetValue(_ambientColor);
            _effect.Parameters["AmbientIntensity"].SetValue(_ambientIntensity);
            _effect.Parameters["DiffuseColor"].SetValue(_diffuseColor);
            _effect.Parameters["DiffuseIntensity"].SetValue(_diffuseIntensity);

            _effect.Parameters["SpecularColor"].SetValue(_specularColor * _specularIntensity);
            _effect.Parameters["LightDirection"].SetValue(_diffuseDirection);

            // Textures
#if MONOGAME
            _effect.Parameters["ColorMap"].SetValue(_texture);
            _effect.Parameters["NormalMap"].SetValue(_normalMap);
#else
            _effect.Parameters["ColorMapSampler"].SetValue(_texture);
            _effect.Parameters["NormalMapSampler"].SetValue(_normalMap);
#endif
            // Position
            _effect.Parameters["EyePosition"].SetValue(world.Translation);
        }
    }
}
