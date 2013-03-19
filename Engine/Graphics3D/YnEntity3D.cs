using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// This is a base class for all things that can be drawn on the screen
    /// </summary>
    public abstract class YnEntity3D : YnBase3D, IDrawableEntity3D
    {
        #region Protected & private declarations

        // Direction
        protected Vector3 _rotation;        // @deprected
        protected Vector3 _scale;           // @deprected
        protected Vector3 _lastPosition;    // @deprected
        protected Vector3 _direction;       // @deprected
        protected Vector3 _lastDirection;   // @deprected

        // Bounding Sphere/Box
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
        protected YnEntity3D _parent;

        // Initialization
        protected bool _initialized;
        protected bool _assetLoaded;

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
        /// Gets the parent object.
        /// </summary>
        public YnEntity3D Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Determine if the entity is initialized.
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        /// <summary>
        /// Determine if asset is loaded.
        /// </summary>
        public bool AssetLoaded
        {
            get { return _assetLoaded; }
            set { _assetLoaded = value; }
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

        #region Constructors

        public YnEntity3D(Vector3 position)
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
            _assetLoaded = false;
            _dynamic = false;

            _boundingBox = new BoundingBox();
            _boundingSphere = new BoundingSphere();
        }

        public YnEntity3D()
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

        #region Update methods

        /// <summary>
        /// Update world matrix.
        /// </summary>
        public abstract void UpdateMatrix();

        /// <summary>
        /// Update bounding box and bounding sphere.
        /// </summary>
        public abstract void UpdateBoundingVolumes();

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
        /// Initialize logic.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Load content.
        /// </summary>
        public virtual void LoadContent()
        {

        }

        /// <summary>
        /// Unload Content
        /// </summary>
        public virtual void UnloadContent()
        {

        }

        /// <summary>
        /// Update logic
        /// </summary>
        /// <param name="gameTime"></param>
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

        public virtual void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera)
        {

        }

        #endregion
    }
}
