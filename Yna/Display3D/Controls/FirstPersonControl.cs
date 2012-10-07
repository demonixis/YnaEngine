using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Controls
{
    public class FirstPersonControl
    {
        private FirstPersonCamera _camera;

        private PlayerIndex _playerIndex;

        private float _moveSpeed;
        private float _strafSpeed;
        private float _pitchSpeed;
        private float _rotateSpeed;

        public FirstPersonControl(FirstPersonCamera camera)
        {
            _camera = camera;
            _moveSpeed = 0.3f;
            _strafSpeed = 0.2f;
            _pitchSpeed = 0.2f;
            _rotateSpeed = 0.3f;
            _playerIndex = PlayerIndex.One;
        }

        public void Update(GameTime gameTime)
        {
            UpdateKeyboardInput(gameTime);
            UpdateMouseInput(gameTime);
            UpdateGamepadInput(gameTime);

            _camera.Update(gameTime);
        }

        protected virtual void UpdateKeyboardInput(GameTime gameTime)
        {
            // Translation Up/Down
            if (YnG.Keys.Pressed(Keys.A))
                _camera.Translate(0, _moveSpeed, 0);
            else if (YnG.Keys.Pressed(Keys.E))
                _camera.Translate(0, -_moveSpeed, 0);

            // Translation Forward/backward
            if (YnG.Keys.Pressed(Keys.Z) || YnG.Keys.Up)
                _camera.Translate(0, 0, _moveSpeed);
            else if (YnG.Keys.Pressed(Keys.S) || YnG.Keys.Down)
                _camera.Translate(0, 0, -_moveSpeed);

            // Look Up/Down
            if (YnG.Keys.Pressed(Keys.PageUp))
                _camera.PitchUp(_pitchSpeed);
            else if (YnG.Keys.Pressed(Keys.PageDown))
                _camera.PitchDown(_pitchSpeed);

            // Translation Left/Right
            if (YnG.Keys.Pressed(Keys.Q))
                _camera.Translate(_strafSpeed, 0, 0);
            else if (YnG.Keys.Pressed(Keys.D))
                _camera.Translate(-_strafSpeed, 0, 0);

            // Rotation Left/Right
            if (YnG.Keys.Left)
                _camera.RotateY(_rotateSpeed);
            else if (YnG.Keys.Right)
                _camera.RotateY(-_rotateSpeed);
        }

        protected virtual void UpdateGamepadInput(GameTime gameTime)
        {
            Vector2 leftStickValue = YnG.Gamepad.LeftStickValue(_playerIndex);
            Vector2 rightStickValue = YnG.Gamepad.RightStickValue(_playerIndex);

            _camera.Translate(-leftStickValue.X * _moveSpeed, 0, leftStickValue.Y * _moveSpeed);
            _camera.RotateY(-rightStickValue.X * _rotateSpeed);
            _camera.Pitch(-rightStickValue.Y * _pitchSpeed);

            if (YnG.Gamepad.LeftShoulder(_playerIndex))
                _camera.Translate(0, _moveSpeed, 0);
            else if (YnG.Gamepad.RightShoulder(_playerIndex))
                _camera.Translate(0, -_moveSpeed, 0);
        }

        protected virtual void UpdateMouseInput(GameTime gameTime)
        {

        }
    }
}
