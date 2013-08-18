// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Material
{
    public class EnvironmentMapMaterial : StockMaterial
    {
        #region Protected declarations

        protected TextureCube _environmentTexture;
        protected string[] _environmentTextureNames;
        protected bool _environmentTextureLoaded;
        protected int _environmentTextureSize;
        protected bool _enableTextureMipmap;
        protected float _environmentAmount;
        protected float _fresnelFactor;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the environment texture
        /// </summary>
        public TextureCube EnvironmentTexture
        {
            get { return _environmentTexture; }
            set { _environmentTexture = value; }
        }

        /// <summary>
        /// Gets or sets the value for the environment amount. Value must be between 0 and 1
        /// </summary>
        public float Amount
        {
            get { return _environmentAmount; }
            set { _environmentAmount = MathHelper.Clamp(value, 0.0f, 1.0f); }
        }

        /// <summary>
        /// Gets or sets the value of the environment texture size
        /// </summary>
        public int EnvironmentTextureSize
        {
            get { return _environmentTextureSize; }
            set
            {
                _environmentTextureSize = value;
                _environmentTextureLoaded = false;
            }
        }

        /// <summary>
        /// Gets or sets the fresnel factor. Value must be between 0 and 1
        /// </summary>
        public float FresnelFactor
        {
            get { return _fresnelFactor; }
            set { _fresnelFactor = MathHelper.Clamp(value, 0.0f, 1.0f); }
        }

        #endregion

        private EnvironmentMapMaterial()
        {
            _environmentTextureNames = null;
            _environmentTextureLoaded = false;
            _environmentAmount = 1.0f;
            _fresnelFactor = 0.5f; // Disabled
            _environmentTextureSize = 0;
            _enableTextureMipmap = false;
            _effectName = "EnvironmentMapEffect";
        }

        /// <summary>
        /// Create an EnvironmentMapMaterial with two textures
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="environmentTextureName"></param>
        public EnvironmentMapMaterial(string textureName, string[] environmentTextureNames)
            : this()
        {
            _textureName = textureName;
            _environmentTextureNames = environmentTextureNames;
        }

        public EnvironmentMapMaterial(string textureName, string environmentTextureName)
            : this()
        {
            _textureName = textureName;
            _environmentTextureNames = new string[6];
            for (int i = 0; i < 6; i++)
                _environmentTextureNames[i] = environmentTextureName;
        }

        public EnvironmentMapMaterial(string textureName)
            : this(textureName, new string[] { textureName })
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();

            // To hacked for now
            if (!_environmentTextureLoaded && _environmentTextureNames != null)
            {
                // Detect the texture size if it doesn't specified
                if (_environmentTextureSize == 0)
                {
                    Texture2D firstTexture = YnG.Content.Load<Texture2D>(_environmentTextureNames[0]);
                    _environmentTextureSize = Math.Min(firstTexture.Width, firstTexture.Height);
                }

                // Create the environment texture
                _environmentTexture = new TextureCube(YnG.GraphicsDevice, _environmentTextureSize, _enableTextureMipmap, SurfaceFormat.Color);

                Texture2D texture = null;   // Temp texture
                Color[] textureData;        // Temp textureData array
                string[] tempTextureNames = new string[6];

                // If the texture array has not a size of 6, we replace empty texture by the latest
                int nbTextures = _environmentTextureNames.Length;

                for (int i = 0; i < 6; i++)
                {
                    if (i < nbTextures) // Texture
                        tempTextureNames[i] = _environmentTextureNames[i];
                    else // Copied texture
                        tempTextureNames[i] = _environmentTextureNames[nbTextures - 1];

                    // Load the texture and add it to the TextureCube
                    texture = YnG.Content.Load<Texture2D>(tempTextureNames[i]);
                    textureData = new Color[texture.Width * texture.Height];
                    texture.GetData<Color>(textureData);
                    _environmentTexture.SetData<Color>((CubeMapFace)i, textureData);
                }
                
                // Update the texture names array
                _environmentTextureNames = tempTextureNames;
                _environmentTextureLoaded = true;

                // If the first texture is null we create a dummy texture with the same size of environment texture
                if (_texture == null)
                {
                    _texture = YnGraphics.CreateTexture(Color.White, _environmentTextureSize, _environmentTextureSize);
                    _textureLoaded = true;
                }
            }

            if (!_effectLoaded)
            {
                _effect = new EnvironmentMapEffect(YnG.GraphicsDevice);
				_effectLoaded = true;
            }
        }

        public override void Update(BaseCamera camera, ref Matrix world)
        {
            if (!_effectLoaded) return;

            // Update matrices
            base.Update(camera, ref world);

            EnvironmentMapEffect environmentMapEffect = (EnvironmentMapEffect)_effect;

            // Texture
            environmentMapEffect.Texture = _texture;
            environmentMapEffect.EnvironmentMap = _environmentTexture;
            environmentMapEffect.EnvironmentMapAmount = _environmentAmount;
            environmentMapEffect.EnvironmentMapSpecular = _specularColor * _specularIntensity;
            environmentMapEffect.FresnelFactor = _fresnelFactor;
            // Fog
            UpdateFog(environmentMapEffect);

            // Lights
            if (UpdateLights(environmentMapEffect))
            {
                environmentMapEffect.EmissiveColor = _emissiveColor;
                environmentMapEffect.DiffuseColor = _diffuseColor * _diffuseIntensity;
                environmentMapEffect.Alpha = _alphaColor;
            }
        }
    }
}
