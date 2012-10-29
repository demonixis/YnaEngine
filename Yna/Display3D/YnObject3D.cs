using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    /// <summary>
    /// This is a base class for all things that can be drawn on the screen
    /// </summary>
    public abstract class YnObject3D : YnBase3D, IDisposable
    {
        #region Protected & private declarations

        protected BaseCamera _camera;

        // Direction
        protected Vector3 _lastPosition;
        protected Vector3 _direction;
        protected Vector3 _lastDirection;

        // Bounding Sphere/Box/Frustrom
        protected BasicEffect _basicEffect;
        protected BoundingBox _boundingBox;
        protected BoundingSphere _boundingSphere;
        protected BoundingFrustum _boundingFrustrum;

        // Visibility
        protected bool _visible;

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

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set the status of the object
        /// If true the object is not paused and is visible
        /// Else it's paused and not visible
        /// </summary>
        public new bool Active
        {
            get { return !_enabled && _visible && !_dirty; }
            set
            {
                if (value)
                {
                    _visible = true;
                    _enabled = false;
                    _dirty = false;
                }
                else
                {
                    _visible = false;
                    _enabled = true;
                    _dirty = false;
                }
            }
        }

        /// <summary>
        /// Get or Set the visibility status of the object
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// Get or Set the value of dynamic, if true the bouding values will be updated on each update
        /// </summary>
        public bool Dynamic
        {
            get { return _dynamic; }
            set { _dynamic = value; }
        }

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

        /// <summary>
        /// Get or Set the camera used for this model
        /// </summary>
        public BaseCamera Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }

        /// <summary>
        /// Get the parent object of the scene
        /// </summary>
        public YnObject3D Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

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

        /// <summary>
        /// Get the bounding frustrum of the model
        /// </summary>
        public BoundingFrustum BoundingFrustrum
        {
            get { return _boundingFrustrum; }
        }

        /// <summary>
        /// Shader effect
        /// </summary>
        public BasicEffect BasicEffect
        {
            get { return _basicEffect; }
            set { _basicEffect = value; }
        }

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
        
        #endregion

        #region Constructors

        public YnObject3D(Vector3 position)
            : base()
        {
            _position = position;
            _lastPosition = position;

            _width = 0;
            _height = 0;
            _depth = 0;

            _direction = Vector3.Zero;
            _lastDirection = Vector3.Zero;

            _visible = true;
            _initialized = false;
            _dynamic = false;

            _boundingBox = new BoundingBox();
            _boundingSphere = new BoundingSphere();
            _boundingFrustrum = new BoundingFrustum(Matrix.Identity);
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
        /// <param name="angle">Angle in degress</param>
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

        #endregion

        #region Bounding volumes

        /// <summary>
        /// Get the updated bounding box
        /// </summary>
        /// <returns>Updated bounding box</returns>
        public virtual BoundingBox GetBoundingBox()
        {
            return new BoundingBox(Vector3.Add(_boundingBox.Min, _position), Vector3.Add(_boundingBox.Max, _position));
        }

        /// <summary>
        /// Get the updated bounding sphere
        /// </summary>
        /// <returns>Updated bounding sphere</returns>
        public virtual BoundingSphere GetBoundingSphere()
        {
            return new BoundingSphere(_position, _boundingSphere.Radius);
        }

        /// <summary>
        /// Get the updated bounding frustrum
        /// </summary>
        /// <returns>Updated bounding frustrum</returns>
        public virtual BoundingFrustum GetBoundingFrustrum()
        {
            return new BoundingFrustum(_camera.Projection * World);
        }

        public abstract void UpdateMatrix();

        public abstract void UpdateBoundingVolumes();

        #endregion

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
