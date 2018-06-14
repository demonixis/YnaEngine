// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// This is a base class for all things that can be drawn on the screen
    /// </summary>
    public abstract class YnEntity3D : YnEntity
    {
        #region Protected & private declarations

        protected Vector3 _position;
        protected Vector3 _rotation;
        protected Vector3 _scale = Vector3.One;
        protected Vector3 _lastPosition;
        protected BoundingBox _boundingBox;
        protected BoundingSphere _boundingSphere;
        protected bool _dirty;
        protected bool _static;
        protected float _width;
        protected float _height;
        protected float _depth;
        protected YnEntity3D _parent;
        protected Matrix _world = Matrix.Identity;

        #endregion

        #region Global properties

        /// <summary>
        /// Gets or sets the status of the object
        /// If true the object is not paused and is visible
        /// Else it's paused and not visible
        /// </summary>
        public override bool Active
        {
            get { return _enabled && _visible && !_dirty; }
            set
            {
                _visible = value;
                _enabled = value;
                _dirty = !value;
            }
        }

        /// <summary>
        /// Gets or sets the value of dynamic, if true the bouding values will be updated on each update
        /// </summary>
        public bool Static
        {
            get { return _static; }
            set { _static = value; }
        }

        /// <summary>
        /// Gets the parent object.
        /// </summary>
        public YnEntity3D Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Get or Set the World matrix of the object
        /// </summary>
        public Matrix World
        {
            get { return _world; }
            set { _world = value; }
        }

        #endregion

        #region Properties for position, direction, rotation, scale

        /// <summary>
        /// Gets or sets the position of the entity.
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Get the last position
        /// </summary>
        public Vector3 LastPosition => _lastPosition;

        /// <summary>
        /// Gets or sets the rotation value
        /// </summary>
        public Vector3 Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Gets or sets the scale value
        /// </summary>
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// Get or Set the position on X axis
        /// </summary>
        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        /// <summary>
        /// Get or Set the position on Y axis
        /// </summary>
        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        /// <summary>
        /// Get or Set the position on Z axis
        /// </summary>
        public float Z
        {
            get { return _position.Z; }
            set { _position.Z = value; }
        }

        /// <summary>
        /// Gets or sets the rotation value on X axis
        /// </summary>
        public float RotationX
        {
            get { return _rotation.X; }
            set { _rotation.X = value; }
        }

        /// <summary>
        /// Gets or sets the rotation value on Y axis
        /// </summary>
        public float RotationY
        {
            get { return _rotation.Y; }
            set { _rotation.Y = value; }
        }

        /// <summary>
        /// Gets or sets the rotation value on Z axis
        /// </summary>
        public float RotationZ
        {
            get { return _rotation.Z; }
            set { _rotation.Z = value; }
        }

        #endregion

        #region Properties for size and bouding volumes

        /// <summary>
        /// Get the width of the model
        /// </summary>
        public float Width => _width;

        /// <summary>
        /// Get the height of the model
        /// </summary>
        public float Height => _height;

        /// <summary>
        /// Get the depth of the model
        /// </summary>
        public float Depth => _depth;

        /// <summary>
        /// Get the bounding box of the object
        /// </summary>
        public BoundingBox BoundingBox => _boundingBox;

        /// <summary>
        /// Get the bounding sphere of the model
        /// </summary>
        public BoundingSphere BoundingSphere => _boundingSphere;

        public Vector3 Forward => _world.Forward;
        public Vector3 Backward => _world.Backward;
        public Vector3 Right => _world.Right;
        public Vector3 Left => _world.Left;
        public Vector3 Up => Vector3.Normalize(Position + Vector3.Transform(Vector3.Up, _world));

        #endregion


        #region Rotation & Translation methods

        /// <summary>
        /// Rotate arround X axis
        /// </summary>
        /// <param name="angle">An angle in degrees</param>
        public virtual void RotateX(float angle)
        {
            _rotation.X += MathHelper.ToRadians(angle);

            if ((_rotation.X >= MathHelper.Pi * 2) || (_rotation.X <= -MathHelper.Pi * 2))
                _rotation.X = 0.0f;
        }

        /// <summary>
        /// Rotate arround Y axis
        /// </summary>
        /// <param name="angle">An angle in degrees</param>
        public virtual void RotateY(float angle)
        {
            _rotation.Y += MathHelper.ToRadians(angle);

            if ((_rotation.Y >= MathHelper.Pi * 2) || (_rotation.Y <= -MathHelper.Pi * 2))
                _rotation.Y = 0.0f;
        }

        /// <summary>
        /// Rotate arround Z axis
        /// </summary>
        /// <param name="angle">An angle in degrees</param>
        public virtual void RotateZ(float angle)
        {
            _rotation.Z += MathHelper.ToRadians(angle);

            if ((_rotation.Z >= MathHelper.Pi * 2) || (_rotation.Z <= -MathHelper.Pi * 2))
                _rotation.Z = 0.0f;
        }

        /// <summary>
        /// Translate on X, Y and Z axis
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        public virtual void Translate(float x, float y, float z)
        {
            _position.X += x;
            _position.Y += y;
            _position.Z += z;
        }

        #endregion

        #region Update methods

        /// <summary>
        /// Update world matrix.
        /// </summary>
        public virtual void UpdateMatrix()
        {
            _world = Matrix.Identity;
            _world *= Matrix.CreateScale(_scale);
            _world *= Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z);
            _world *= Matrix.CreateTranslation(_position);

            if (_parent != null)
                _world *= _parent.World;
        }

        /// <summary>
        /// Update bounding box and bounding sphere.
        /// </summary>
        public virtual void UpdateBoundingVolumes()
        {
            _boundingBox.Min.X = X;
            _boundingBox.Min.Y = Y;
            _boundingBox.Min.Z = Z;
            _boundingBox.Max.X = X + Width;
            _boundingBox.Max.Y = Y + Height;
            _boundingBox.Max.Z = Z + Depth;

            _boundingSphere.Center.X = X + (Width / 2);
            _boundingSphere.Center.Y = Y + (Height / 2);
            _boundingSphere.Center.Z = Z + (Depth / 2);
            _boundingSphere.Radius = Math.Max(Math.Max(Width, Height), Depth);
        }

        /// <summary>
        /// Update lights.
        /// </summary>
        /// <param name="light">Light to use.</param>
        public virtual void UpdateLighting(SceneLight light)
        {
        }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Update logic
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (_static)
                return;

            UpdateBoundingVolumes();

            _lastPosition = _position;
        }

        public virtual void Draw(GameTime gameTime, GraphicsDevice device, Cameras.Camera camera)
        {
        }

        #endregion
    }
}
