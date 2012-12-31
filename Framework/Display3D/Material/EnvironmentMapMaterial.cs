using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Lighting;
using Yna.Framework.Display3D.Camera;

namespace Yna.Framework.Display3D.Material
{
    public class EnvironmentMapMaterial : StockMaterial
    {
        #region Protected declarations

        protected TextureCube _environmentTexture;
        protected string _environmentTextureName;
        protected bool _environmentTextureLoaded;
        protected float _environmentAmount;
        protected Vector3 _environmentSpecular;
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
        /// Gets or sets the environment texture name
        /// </summary>
        public string EnvironmentTextureName
        {
            get { return _environmentTextureName; }
            set
            {
                _environmentTextureName = value;
                _environmentTextureLoaded = false;
            }
        }

        /// <summary>
        /// Gets or sets the value for the environment amount. Value must be between 0 and 1
        /// </summary>
        public float EnvironmentAmount
        {
            get { return _environmentAmount; }
            set { _environmentAmount = MathHelper.Clamp(value, 0.0f, 1.0f); }
        }

        /// <summary>
        /// Gets or sets the specular value to use with the environment map
        /// </summary>
        public Vector3 EnvironmentSpecular
        {
            get { return _environmentSpecular; }
            set
            {
                _environmentSpecular.X = MathHelper.Clamp(value.X, 0.0f, 1.0f);
                _environmentSpecular.Y = MathHelper.Clamp(value.Y, 0.0f, 1.0f);
                _environmentSpecular.Z = MathHelper.Clamp(value.Z, 0.0f, 1.0f);
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
            _environmentTextureName = String.Empty;
            _environmentTextureLoaded = false;
            _environmentAmount = 1.0f;
            _environmentSpecular = Color.Black.ToVector3();
            _fresnelFactor = 0.0f; // Disabled
        }

        /// <summary>
        /// Create an EnvironmentMapMaterial with two textures
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="environmentTextureName"></param>
        public EnvironmentMapMaterial(string textureName, string environmentTextureName)
            : this()
        {
            _mainTextureName = textureName;
            _environmentTextureName = environmentTextureName;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (!_environmentTextureLoaded && _environmentTextureName != String.Empty)
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
            // Update matrices
            base.Update(ref world, ref view, ref projection, ref position);

            EnvironmentMapEffect environmentMapEffect = (EnvironmentMapEffect)_effect;
            
            // Texture
            environmentMapEffect.Texture = _mainTexture;
            environmentMapEffect.EnvironmentMap = _environmentTexture;
            environmentMapEffect.EnvironmentMapAmount = _environmentAmount;
            environmentMapEffect.EnvironmentMapSpecular = _environmentSpecular;
            environmentMapEffect.FresnelFactor = _fresnelFactor;

            // Fog
            UpdateFog(environmentMapEffect);

            // Lights
            if (UpdateLights(environmentMapEffect))
            {
                environmentMapEffect.EmissiveColor = _emissiveColor;
                environmentMapEffect.DiffuseColor = new Vector3(_diffuseColor.X, _diffuseColor.Y, _diffuseColor.Z) * _diffuseIntensity;
                environmentMapEffect.Alpha = _alphaColor;
            }
        }

        public new Vector3 SpecularColor { get { return Vector3.Zero; } }
    }
}
