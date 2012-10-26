using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Controls
{
    public abstract class BaseControl : YnBase3D
    {
        protected BaseCamera _camera;

        protected PlayerIndex _playerIndex;

        protected float _moveSpeed;
        protected float _strafeSpeed;
        protected float _pitchSpeed;
        protected float _rotateSpeed;

        protected bool _useKeyboard;
        protected bool _useGamepad;
        protected bool _useMouse;

        #region Properties

        /// <summary>
        /// Get or Set the move speed
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
        /// Get or Set the strafe speed
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
        /// Get or Set the pitch speed
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
        /// Get or Set the rotate speed
        /// </summary>
        public float RotateSpeed
        {
            get { return _rotateSpeed; }
            set
            {
                if (value >= 0)
                    _rotateSpeed = value;
            }
        }

        public bool UseKeyboard
        {
            get { return _useKeyboard; }
            set { _useKeyboard = value; }
        }

        public bool UseGamepad
        {
            get { return _useGamepad; }
            set { _useGamepad = value; }
        }

        public bool UseMouse
        {
            get { return _useMouse; }
            set { _useMouse = value; }
        }

        #endregion

        public BaseControl(BaseCamera camera)
        {
            _camera = camera;

            _moveSpeed = 0.3f;
            _strafeSpeed = 0.2f;
            _pitchSpeed = 0.2f;
            _rotateSpeed = 0.3f;
            _playerIndex = PlayerIndex.One;

            _useKeyboard = true;
            _useGamepad = true;
            _useMouse = false;
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
            if (_useKeyboard)
                UpdateKeyboardInput(gameTime);

            if (_useMouse)
                UpdateMouseInput(gameTime);

            if (_useGamepad)
                UpdateGamepadInput(gameTime);

            _camera.Update(gameTime);
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
