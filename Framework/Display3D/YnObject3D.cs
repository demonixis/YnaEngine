using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Lighting;

namespace Yna.Framework.Display3D
{
    /// <summary>
    /// This is a base class for all things that can be drawn on the screen
    /// </summary>
    public abstract class YnObject3D : YnBase3D, IDisposable
    {
        #region Protected & private declarations

        protected BaseCamera _camera;
        protected Light _light;

        // Direction
        protected Vector3 _rotation;
        protected Vector3 _scale;
        protected Vector3 _lastPosition;
        protected Vector3 _direction;
        protected Vector3 _lastDirection;

        // Bounding Sphere/Box/Frustrom
        protected BasicEffect _basicEffect;
        protected BoundingBox _boundingBox;
        protected BoundingSphere _boundingSphere;

        // Visibility
        protected bool _visible;
        protected bool _dirty;

        // Static or dynamic object
        protected bool _dynamic;

        // Sizes
        protected float _width;
        protected float _height;
        protected float _depth;

        // Parent
        protected YnObject3D _parent;

        // Initialization
        protected bool _initialized;

        // View matrix
        protected Matrix _view;

        #endregion

        #region Global properties

        /// <summary>
        /// Gets or sets the status of the object
        /// If true the object is not paused and is visible
        /// Else it's paused and not visible
        /// </summary>
        public new bool Active
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
        /// Gets or sets the visibility status of the object
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// Gets or sets the value of dynamic, if true the bouding values will be updated on each update
        /// </summary>
        public bool Dynamic
        {
            get { return _dynamic; }
            set { _dynamic = value; }
        }

        /// <summary>
        /// Get the parent object of the scene
        /// </summary>
        public YnObject3D Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        #endregion

        #region Properties for position, direction, rotation

        /// <summary>
        /// Get the last position
        /// </summary>
        public Vector3 LastPosition
        {
            get { return _lastPosition; }
        }

        public Vector3 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public Vector3 LastDirection
        {
            get { return _lastDirection; }
        }

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

        #region Properties for size

        /// <summary>
        /// Get the width of the model
        /// </summary>
        public float Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Get the height of the model
        /// </summary>
        public float Height
        {
            get { return _height; }
        }

        /// <summary>
        /// Get the depth of the model
        /// </summary>
        public float Depth
        {
            get { return _depth; }
        }

        #endregion

        #region Properties for camera and light

        /// <summary>
        /// Gets or sets the camera used for this model
        /// </summary>
        public BaseCamera Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }

        /// <summary>
        /// Gets or sets the light for this model
        /// </summary>
        public Light Light
        {
            get { return _light; }
            set { _light = value; }
        }

        /// <summary>
        /// Shader effect
        /// </summary>
        public BasicEffect BasicEffect
        {
            get { return _basicEffect; }
            set { _basicEffect = value; }
        }

        #endregion

        #region Bounding volumes

        /// <summary>
        /// Get the bounding box of the object
        /// </summary>
        public BoundingBox BoundingBox
        {
            get { return _boundingBox; }
        }

        /// <summary>
        /// Get the bounding sphere of the model
        /// </summary>
        public BoundingSphere BoundingSphere
        {
            get { return _boundingSphere; }
        }

        #endregion

        #region Properties for matrices

        /// <summary>
        /// Gets or sets the view matrix
        /// </summary>
        public Matrix View
        {
            get { return _view; }
            set { _view = value; }
        }

        #endregion

        #region Constructors

        public YnObject3D(Vector3 position)
            : base()
        {
            _position = position;
            _lastPosition = position;
            _rotation = Vector3.Zero;
            _scale = Vector3.One;

            _width = 0;
            _height = 0;
            _depth = 0;

            _direction = Vector3.Zero;
            _lastDirection = Vector3.Zero;

            _visible = true;
            _dirty = false;
            _initialized = false;
            _dynamic = false;
            _light = new Light();

            _boundingBox = new BoundingBox();
            _boundingSphere = new BoundingSphere();
        }

        public YnObject3D()
            : this(new Vector3(0.0f, 0.0f, 0.0f))
        {

        }

        #endregion

        #region Rotation & Translation methods

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
        /// Translate on X, Y and Z axis
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        public virtual void Translate(float x, float y, float z)
        {
            Vector3 move = new Vector3(x, y, z);

            Matrix forwardMovement = Matrix.CreateRotationY(_rotation.Y);

            Vector3 v = Vector3.Transform(move, forwardMovement);

            _position.X += v.X;
            _position.Y += v.Y;
            _position.Z += v.Z;
        }

        /// <summary>
        /// Translate the object on X axis
        /// </summary>
        /// <param name="x">X value</param>
        public virtual void TranslateX(float x)
        {
            Translate(x, 0.0f, 0.0f);
        }

        /// <summary>
        /// Translate the object on Y axis
        /// </summary>
        /// <param name="y"></param>
        public virtual void TranslateY(float y)
        {
            Translate(0.0f, y, 0.0f);
        }

        public virtual void TranslateZ(float z)
        {
            Translate(0.0f, 0.0f, z);
        }

        #endregion

        public abstract void UpdateMatrices();

        public abstract void UpdateBoundingVolumes();

        #region GameState pattern

        /// <summary>
        /// Load Content
        /// </summary>
        public virtual void LoadContent()
        {
            _basicEffect = new BasicEffect(YnG.GraphicsDevice);
        }

        /// <summary>
        /// Unload Content
        /// </summary>
        public virtual void UnloadContent()
        {
            if (_basicEffect != null)
                _basicEffect.Dispose();
        }

        public override void Update(GameTime gameTime)
        {
            if (_dynamic)
            {
                UpdateBoundingVolumes();

                _lastDirection = (_position - _lastPosition);
                _lastDirection.Normalize();

                _lastPosition = _position;
            }

        }

        /// <summary>
        /// Draw the object
        /// </summary>
        /// <param name="device">GraphicsDevice object</param>
        public abstract void Draw(GraphicsDevice device);

        #endregion

        #region IDisposable implementation

        void IDisposable.Dispose()
        {
            UnloadContent();
        }

        #endregion
    }
}
