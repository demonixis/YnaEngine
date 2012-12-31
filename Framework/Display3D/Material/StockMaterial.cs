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
        protected bool _enabledMainTexture;
        protected bool _enabledFog;
        protected bool _enabledPerPixelLightning;
        protected bool _enabledVertexColor;
        protected bool _enabledDefaultLighting;

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

        public Texture2D MainTexture
        {
            get { return _mainTexture; }
            set
            {
                _mainTexture = value;
                _mainTextureLoaded = false;
            }
        }

        public bool MainTextureLoaded
        {
            get { return _mainTextureLoaded; }
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
            _enabledDefaultLighting = true;
            _enabledFog = false;
            _enabledMainTexture = false;
            _enabledPerPixelLightning = false;
            _enabledVertexColor = false;
            _effectName = "XNA Stock Effect";
            _mainTextureLoaded = false;
        }

        /// <summary>
        /// Gets or sets the alpha value
        /// </summary>
        public float AlphaColor
        {
            get { return _alphaColor; }
            set { _alphaColor = value; }
        }

        public override void LoadContent()
        {
            if (!_mainTextureLoaded && _mainTextureName != String.Empty)
            {
                _mainTexture = YnG.Content.Load<Texture2D>(_mainTextureName);
                _enabledMainTexture = true;
            }
            else
                _enabledMainTexture = false;
        }
    }
}
