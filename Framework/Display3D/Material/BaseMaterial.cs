using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Lighting;
using Yna.Framework.Display3D.Camera;

namespace Yna.Framework.Display3D.Material
{
    /// <summary>
    /// Base class for define a Material
    /// </summary>
    public abstract class BaseMaterial
    {
        // The effect to use
        protected Effect _effect;
        protected string _effectName;
        protected bool _effectLoaded;

        // A default texture
        protected Texture2D _texture;
        protected string _textureName;
        protected bool _textureLoaded;

        // TODO must be an array
        protected Light _light;

        // Base value for a material
        protected Vector4 _ambientColor;
        protected float _ambientIntensity;
        protected Vector4 _diffuseColor;
        protected float _diffuseIntensity;

        #region Properties

        /// <summary>
        /// Gets or sets the current effect
        /// </summary>
        public Effect Effect
        {
            get { return _effect; }
            set { _effect = value; }
        }

        /// <summary>
        /// Gets or sets the main texture used with this effect
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        /// <summary>
        /// Gets or sets the lights (will change)
        /// </summary>
        public Light Light
        {
            get { return _light; }
            set { _light = value; }
        }

        /// <summary>
        /// Gets or sets the ambient color
        /// </summary>
        public Vector4 AmbientColor
        {
            get { return _ambientColor; }
            set { _ambientColor = value; }
        }

        /// <summary>
        /// Get or sets the value of ambient intensity
        /// </summary>
        public float AmbientIntensity
        {
            get { return _ambientIntensity; }
            set { _ambientIntensity = value; }
        }

        /// <summary>
        /// Gets or sets the default diffuse color
        /// </summary>
        public Vector4 DiffuseColor
        {
            get { return _diffuseColor; }
            set { _diffuseColor = value; }
        }

        /// <summary>
        /// Gets or sets the default diffuse intensity
        /// </summary>
        public float DiffuseIntensity
        {
            get { return _diffuseIntensity; }
            set { _diffuseIntensity = value; }
        }

        #endregion

        public BaseMaterial()
        {
            _ambientColor = Color.White.ToVector4();
            _ambientIntensity = 1.0f;
            _diffuseColor = Color.White.ToVector4();
            _diffuseIntensity = 1.0f;
            _effect = null;
            _effectName = String.Empty;
            _effectLoaded = false;
            _texture = null;
            _textureName = String.Empty;
            _textureLoaded = false;
        }

        public abstract void LoadContent();

        /// <summary>
        /// Update the shader values
        /// </summary>
        /// <param name="world">Entity World matrix</param>
        /// <param name="view">Camera view matrix</param>
        /// <param name="projection">Camera project matrix</param>
        /// <param name="position">Entity position</param>
        public abstract void Update(ref Matrix world, ref Matrix view, ref Matrix projection, ref Vector3 position);
    }
}
