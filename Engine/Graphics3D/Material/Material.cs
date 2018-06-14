// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Cameras;

namespace Yna.Engine.Graphics3D.Materials
{
    /// <summary>
    /// Base class for define a Material
    /// </summary>
    public abstract class Material
    {
        // The effect to use
        protected Effect _effect;
        protected string _effectName = string.Empty;
        protected bool _effectLoaded;

        // A default texture
        protected Texture2D _texture;
        protected string _textureName = string.Empty;
        protected bool _textureLoaded;

        // Base value for a material
        protected Vector3 _ambientColor = Color.White.ToVector3();
        protected float _ambientIntensity = 0.75f;
        protected Vector3 _diffuseColor = Color.White.ToVector3();
        protected float _diffuseIntensity = 0.75f;

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

        public abstract void LoadContent();

        /// <summary>
        /// Update the shader values
        /// </summary>
        /// <param name="camera">Camera to use.</param>
        /// <param name="world">Entity World matrix</param>
        public abstract void Update(Cameras.Camera camera, ref Matrix world);
    }
}
