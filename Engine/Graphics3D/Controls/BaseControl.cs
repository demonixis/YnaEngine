using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Controls
{
    /// <summary>
    /// Define a basic controller for a camera
    /// </summary>
    public abstract class BaseControl : YnBase3D
    {
        private BaseCamera _camera;
        protected PlayerIndex _playerIndex;
        protected KeysMapper _keyMapper;

        // Some physics
        protected Vector3 _accelerationPosition;
        protected Vector3 _accelerationRotation;
        protected Vector3 _velocityPosition;
        protected Vector3 _velocityRotation;
        protected float _maxVelocityPosition;
        protected float _maxVelocityRotation;

        // Speed
        protected float _moveSpeed;
        protected float _strafeSpeed;
        protected float _pitchSpeed;
        protected float _rotateSpeed;
        protected bool _mouseLock;

        // Flags
        protected bool _enableKeyboard;
        protected bool _enableGamepad;
        protected bool _enableMouse;

        #region Properties

        /// <summary>
        /// Gets or sets the value for the keys mapper
        /// </summary>
        public KeysMapper KeysMapper
        {
            get { return _keyMapper; }
            set { _keyMapper = value; }
        }

        /// <summary>
        /// Gets or sets the value of acceleration for translations
        /// </summary>
        public Vector3 AccelerationPosition
        {
            get { return _accelerationPosition; }
            set { _accelerationPosition = value; }
        }

        /// <summary>
        /// Gets or sets the value of acceleration for rotations
        /// </summary>
        public Vector3 AccelerationRotation
        {
            get { return _accelerationRotation; }
            set { _accelerationRotation = value; }
        }

        /// <summary>
        /// Gets or sets the value of velocity for translations
        /// </summary>
        public Vector3 VelocityPosition
        {
            get { return _velocityPosition; }
            set { _velocityPosition = value; }
        }

        /// <summary>
        /// Gets or sets the value of velocity for rotations
        /// </summary>
        public Vector3 VelocityRotation
        {
            get { return _velocityRotation; }
            set { _velocityRotation = value; }
        }

        /// <summary>
        /// Gets or sets the velocity factor for translations
        /// </summary>
        public float MaxVelocityPosition
        {
            get { return _maxVelocityPosition; }
            set { _maxVelocityPosition = value; }
        }

        /// <summary>
        /// Gets or sets the velocity factor for rotations
        /// </summary>
        public float MaxVelocityRotation
        {
            get { return _maxVelocityRotation; }
            set { _maxVelocityRotation = value; }
        }

        /// <summary>
        /// Gets or sets the camera used with this control
        /// </summary>
        public BaseCamera Camera
        {
            get { return _camera; }
            protected set { _camera = value; }
        }

        /// <summary>
        /// Define the player index for the gamepad
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get { return _playerIndex; }
            set { _playerIndex = value; }
        }

        /// <summary>
        /// Gets or sets the move speed
        /// </summary>
        public float MoveSpeed
        {
            get { return _moveSpeed; }
            set
            {
                if (value >= 0)
                    _moveSpeed = value;
            }
        }

        /// <summary>
        /// Gets or sets the strafe speed
        /// </summary>
        public float StrafeSpeed
        {
            get { return _strafeSpeed; }
            set
            {
                if (value >= 0)
                    _strafeSpeed = value;
            }
        }

        /// <summary>
        /// Gets or sets the pitch speed
        /// </summary>
        public float PitchSpeed
        {
            get { return _pitchSpeed; }
            set
            {
                if (value >= 0)
                    _pitchSpeed = value;
            }
        }

        /// <summary>
        /// Enable or disable the mouse look
        /// </summary>
        public bool MouseLock
        {
            get { return _mouseLock; }
            set 
            { 
                _mouseLock = value;

            }
        }

        /// <summary>
        /// Gets or sets the rotate speed
        /// </summary>
        public float RotationSpeed
        {
            get { return _rotateSpeed; }
            set
            {
                if (value >= 0)
                    _rotateSpeed = value;
            }
        }

        /// <summary>
        /// Enable or disable keyboard
        /// </summary>
        public bool EnableKeyboard
        {
            get { return _enableKeyboard; }
            set { _enableKeyboard = value; }
        }

        /// <summary>
        /// Enable or disable gamepad
        /// </summary>
        public bool EnableGamepad
        {
            get { return _enableGamepad; }
            set { _enableGamepad = value; }
        }

        /// <summary>
        /// Enable or disable mouse
        /// </summary>
        public bool EnableMouse
        {
            get { return _enableMouse; }
            set 
            {
                _enableMouse = value;
                if (_enableMouse)
                    Mouse.SetPosition(YnG.Width / 2, YnG.Height / 2);
            }
        }

        #endregion

        /// <summary>
        /// A base control with a camera.
        /// </summary>
        /// <param name="camera"></param>
        public BaseControl(BaseCamera camera)
        {
            _camera = camera;
            _keyMapper = new KeysMapper();

            // Physics
            _accelerationPosition = Vector3.One;
            _accelerationRotation = Vector3.One;
            _velocityPosition = Vector3.Zero;
            _velocityRotation = Vector3.Zero;
            _maxVelocityPosition = 0.5f;
            _maxVelocityRotation = 0.5f;

            // Movement
            _moveSpeed = 0.3f;
            _strafeSpeed = 0.2f;
            _pitchSpeed = 0.2f;
            _rotateSpeed = 0.3f;
            _playerIndex = PlayerIndex.One;

            // Flags
            _enableKeyboard = true;
            _enableGamepad = true;
            _enableMouse = false;
#if LINUX
			_useGamepad = false;
#endif
        }

        public BaseControl(BaseCamera camera, PlayerIndex index)
            : this(camera)
        {
            _playerIndex = index;
        }

        public override void Update(GameTime gameTime)
        {
            // Physics
            UpdatePhysics(gameTime);

            // Inputs
            UpdateInputs(gameTime);
        }

        /// <summary>
        /// Update physics.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void UpdatePhysics(GameTime gameTime)
        {
            _velocityPosition *= _accelerationPosition * _maxVelocityPosition;
            _velocityRotation *= _accelerationRotation * _maxVelocityRotation;
        }

        /// <summary>
        /// Apply velocity speed and acceleration
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public virtual void ApplyPhysics(GameTime gameTime)
        {
            Camera.Translate(_velocityPosition.X, _velocityPosition.Y, _velocityPosition.Z);
            Camera.RotateY(_velocityRotation.Y);
            Camera.RotateX(_velocityRotation.X);
            Camera.RotateZ(_velocityRotation.Z);
        }

        /// <summary>
        /// Update all inputs.
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void UpdateInputs(GameTime gameTime)
        {
            if (_enableKeyboard)
                UpdateKeyboardInput(gameTime);

            if (_enableMouse)
                UpdateMouseInput(gameTime);

            if (_enableGamepad)
                UpdateGamepadInput(gameTime); 
        }

        /// <summary>
        /// Update keyboard input
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        protected abstract void UpdateKeyboardInput(GameTime gameTime);

        /// <summary>
        /// Update Gamepad input
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        protected abstract void UpdateGamepadInput(GameTime gameTime);

        /// <summary>
        /// Update mouse input
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        protected abstract void UpdateMouseInput(GameTime gameTime);
    }
}
