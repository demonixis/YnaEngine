// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Controls
{
    public struct PhysicsController
    {
        public Vector3 Acceleration;
        public Vector3 Velocity;
        public float MaxVelocity;
    }

    /// <summary>
    /// Define a basic controller for a camera
    /// </summary>
    public abstract class BaseControl : YnBasicEntity
    {
        private BaseCamera _camera;
        protected PlayerIndex _playerIndex;

        // Some physics
        public PhysicsController PhysicsPosition;
        public PhysicsController PhysicsRotation;

        // Speed
        protected float _moveSpeed;
        protected float _strafeSpeed;
        protected float _pitchSpeed;
        protected float _rotationSpeed;
        protected bool _mouseLock;

        // Flags
        protected bool _enableKeyboard;
        protected bool _enableGamepad;
        protected bool _enableMouse;

        // Inverts
        protected int _xDirection;
        protected int _yDirection;
        protected int _zDirection;
        protected int _xRotation;
        protected int _yRotation;
        protected int _zRotation;

        #region Properties

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
            get { return _rotationSpeed; }
            set
            {
                if (value >= 0)
                    _rotationSpeed = value;
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
#if !DIRECTX
                if (_enableMouse)
                    Mouse.SetPosition(YnG.Width / 2, YnG.Height / 2);
#endif
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

            // Physics
            PhysicsPosition = new PhysicsController()
            {
                Acceleration = Vector3.One,
                Velocity = Vector3.Zero,
                MaxVelocity = 0.5f
            };

            PhysicsRotation = new PhysicsController()
            {
                Acceleration = Vector3.One,
                Velocity = Vector3.Zero,
                MaxVelocity = 0.5f
            };

            // Movement
            _moveSpeed = 0.3f;
            _strafeSpeed = 0.2f;
            _pitchSpeed = 0.2f;
            _rotationSpeed = 0.3f;
            _playerIndex = PlayerIndex.One;

            // Flags
            _enableKeyboard = true;
            _enableGamepad = true;
            _enableMouse = false;

            // Invert
            _xDirection = 1;
            _yDirection = 1;
            _zDirection = 1;
            _xRotation = 1;
            _yRotation = 1;
            _zRotation = 1;
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
            PhysicsPosition.Velocity *= PhysicsPosition.Acceleration * PhysicsPosition.MaxVelocity;
            PhysicsRotation.Velocity *= PhysicsRotation.Acceleration * PhysicsRotation.MaxVelocity;
        }

        /// <summary>
        /// Apply velocity speed and acceleration
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public virtual void ApplyPhysics(GameTime gameTime)
        {
            Camera.Translate(PhysicsPosition.Velocity.X * _xDirection, PhysicsPosition.Velocity.Y * _yDirection, PhysicsPosition.Velocity.Z * _zDirection);
            Camera.RotateY(PhysicsRotation.Velocity.Y * _yRotation);
            Camera.RotateX(PhysicsRotation.Velocity.X * _xRotation);
            Camera.RotateZ(PhysicsRotation.Velocity.Z * _zRotation);
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
        /// Set invertion axis for translations.
        /// </summary>
        /// <param name="invertX">Invert X axis if set to true</param>
        /// <param name="invertY">Invert Y axis if set to true</param>
        /// <param name="invertZ">Invert Z axis if set to true</param>
        public virtual void SetInvertTranslation(bool invertX, bool invertY, bool invertZ)
        {
            _xDirection = invertX ? -1 : 1;
            _yDirection = invertY ? -1 : 1;
            _zDirection = invertZ ? -1 : 1;
        }

        /// <summary>
        /// Set invertion axis for rotations.
        /// </summary>
        /// <param name="invertX">Invert X axis if set to true</param>
        /// <param name="invertY">Invert Y axis if set to true</param>
        /// <param name="invertZ">Invert Z axis if set to true</param>
        public virtual void SetInvertRotation(bool invertX, bool invertY, bool invertZ)
        {
            _xRotation = invertX ? -1 : 1;
            _yRotation = invertY ? -1 : 1;
            _zRotation = invertZ ? -1 : 1;
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
