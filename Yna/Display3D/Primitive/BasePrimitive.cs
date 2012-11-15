﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Light;

namespace Yna.Display3D.Primitive
{
    public class BasePrimitive : YnObject3D, IYnLightable
    {
        #region Private declarations

        protected Texture2D _texture;
        protected string _textureName;
        protected Vector2 _textureRepeat;
        protected Vector3 _segmentSizes;
        protected BasicLight _basicLight;
        protected bool _lightningEnabled;
        protected bool _colorEnabled;
        protected bool _textureEnabled;
        protected bool _constructed;
        protected bool _needUpdate;
        protected bool _needLightUpdate;

        #endregion

        #region Properties

        /// <summary>
        /// Width of the terrain
        /// </summary>
        public new int Width
        {
            get { return (int)_width; }
            protected set { _width = (float)value; }
        }

        /// <summary>
        /// Height of the terrain
        /// </summary>
        public new int Height
        {
            get { return (int)_height; }
            protected set { _height = (float)value; }
        }

        /// <summary>
        /// Depth of the terrain
        /// </summary>
        public new int Depth
        {
            get { return (int)_depth; }
            protected set { _depth = (float)value; }
        }

        /// <summary>
        /// True if the texture is loaded
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        public BasicLight Light
        {
            get { return _basicLight; }
            set { _basicLight = value; }
        }

        public bool LightningEnabled
        {
            get { return _lightningEnabled; }
            set
            {
                _lightningEnabled = value;
                _basicEffect.LightingEnabled = _lightningEnabled; 
            }
        }

        /// <summary>
        /// Texture to use with the terrain
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                _textureName = _texture.Name;
            }
        }

        public Vector2 TextureRepeat
        {
            get { return _textureRepeat; }
            set { _textureRepeat = value; }
        }

        /// <summary>
        /// Get or Set the segments size. It represent the space between two vertex.
        /// The terrain is reconstructed when you set a new value
        /// </summary>
        public Vector3 SegmentSizes
        {
            get { return _segmentSizes; }
            set { _segmentSizes = value; }
        }

        public bool NeedUpdate
        {
            get { return _needUpdate; }
            set { _needUpdate = value; }
        }

        public bool NeedLightUpdate
        {
            get { return _needLightUpdate; }
            set { _needLightUpdate = value; }
        }

        #endregion

        public BasePrimitive()
            : this(new Vector3(0, 0, 0))
        {

        }

        public BasePrimitive(Vector3 position)
            : base(position)
        {
            _texture = null;
            _textureName = String.Empty;
            _textureRepeat = Vector2.One;
            _segmentSizes = Vector3.One;

            _basicLight = new BasicLight();

            _lightningEnabled = true;
            _colorEnabled = false;
            _textureEnabled = false;

            _initialized = false;
            _constructed = false;
            _needUpdate = true;
            _needLightUpdate = true;
        }

        /// <summary>
        /// Setup the basic effet
        /// </summary>
        protected virtual void SetupShader()
        {
            if (_lightningEnabled)
                SetupLightning();
            else
                _basicEffect.EnableDefaultLighting();

            _basicEffect.VertexColorEnabled = _colorEnabled;

            if (_texture != null)
            {
                _basicEffect.Texture = _texture;
                _basicEffect.TextureEnabled = true;
            }
            else
                _basicEffect.TextureEnabled = false;

            _needUpdate = false;
        }

        protected virtual void SetupLightning()
        {
            _basicEffect.LightingEnabled = true;
            _basicEffect.DirectionalLight0.Enabled = true;
            _basicEffect.DirectionalLight0.Direction = _basicLight.Direction;
            _basicEffect.DirectionalLight0.DiffuseColor = _basicLight.Diffuse;
            _basicEffect.DirectionalLight0.SpecularColor = _basicLight.Specular;
            _basicEffect.AmbientLightColor = _basicLight.Ambient;
            _basicEffect.EmissiveColor = _basicLight.Emissive;
            _basicEffect.Alpha = _basicLight.Alpha;
        }

        public override void UpdateMatrix()
        {
            World = Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) *
                Matrix.CreateTranslation(Position);

            View = _camera.View;
        }

        public override void UpdateBoundingVolumes()
        {
            _boundingBox = new BoundingBox(
                new Vector3(X, Y, Z),
                new Vector3(X + Width, Y + Height, Z + Depth));

            _boundingSphere = new BoundingSphere(
                new Vector3(X + Width / 2, Y + Height / 2, Z + Depth / 2),
                Math.Max(Math.Max(Width, Height), Depth));

            _boundingFrustrum = new BoundingFrustum(World * _camera.Projection);
        }

        public override void Draw(GraphicsDevice device)
        {
            UpdateMatrix();

            if (_dynamic)
                UpdateBoundingVolumes();

            if (_needUpdate)
                SetupShader();

            if (_needLightUpdate)
                SetupLightning();
            
            _basicEffect.World = World;
            _basicEffect.View = View;
            _basicEffect.Projection = _camera.Projection;
        }

        #region IYnLightable implementation

        void IYnLightable.SetLight(BasicLight light)
        {
            _basicLight = light;
        }

        BasicLight IYnLightable.GetLight(BasicLight light)
        {
            return _basicLight;
        }

        #endregion
    }
}
