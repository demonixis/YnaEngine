using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    public abstract class YnObject3D : YnBase3D, IDisposable
    {
        protected BaseCamera _camera;
        protected Matrix _world;
        protected BasicEffect _basicEffect;
        protected BoundingBox _boundingBox;
        protected bool _visible;
        protected float _width;
        protected float _height;
        protected float _depth;
        protected YnObject3D _parent;
        protected bool _initialized;

        #region Properties

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
        /// Determine if this object is visible. If not, it's not rendered
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
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
        /// Get or Set the World matrix of the object
        /// </summary>
        public Matrix World
        {
            get { return _world; }
            set { _world = value; }
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

        #region Constructors

        public YnObject3D(Vector3 position)
            : base()
        {
            _position = position;

            _width = 0;
            _height = 0;
            _depth = 0;

            _visible = true;
            _initialized = false;
        }

        public YnObject3D()
            : this(new Vector3(0.0f, 0.0f, 0.0f))
        {

        }

        #endregion

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

        /// <summary>
        /// Get the bounding box of the object
        /// </summary>
        /// <returns></returns>
        public abstract BoundingBox GetBoundingBox();

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

        /// <summary>
        /// Draw the object
        /// </summary>
        /// <param name="device">GraphicsDevice object</param>
        public abstract void Draw(GraphicsDevice device);

        #endregion

        void IDisposable.Dispose()
        {
            UnloadContent();
        }
    }
}
