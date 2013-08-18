// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Material
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

        // Base value for a material
        protected Vector3 _ambientColor;
        protected float _ambientIntensity;
        protected Vector3 _diffuseColor;
        protected float _diffuseIntensity;

        protected BaseLight _light;

        #region Properties

        /// <summary>
        /// Gets or sets the current effect
        /// </summary>
        public Effect Effect
        {
            get { return _effect; }
            set { _effect = value; }
        }

        public BaseLight Light
        {
            get { return _light; }
            set { _light = value; }
        }

        /// <summary>
        /// Gets the status of the effect. True if loaded then false
        /// </summary>
        public bool Loaded
        {
            get { return _effectLoaded; }
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
        /// Gets or sets the ambient color
        /// </summary>
        public Vector3 AmbientColor
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
        public Vector3 DiffuseColor
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
            _ambientColor = Color.White.ToVector3();
            _ambientIntensity = 0.75f;
            _diffuseColor = Color.White.ToVector3();
            _diffuseIntensity = 0.75f;
            _effect = null;
            _effectName = String.Empty;
            _effectLoaded = false;
            _texture = null;
            _textureName = String.Empty;
            _textureLoaded = false;
            _light = null;
        }

        public abstract void LoadContent();

        /// <summary>
        /// Update the shader values
        /// </summary>
        /// <param name="camera">Camera to use.</param>
        /// <param name="world">Entity World matrix</param>
        public abstract void Update(BaseCamera camera, ref Matrix world);
    }
}
