using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Controls
{
    public class FirstPersonControl : BaseControl
    {
        public FirstPersonControl(FirstPersonCamera camera)
            : base(camera)
        {

        }

        public FirstPersonControl(FirstPersonCamera camera, PlayerIndex index)
            : base(camera, index)
        {
            
        }

        public void Update(GameTime gameTime)
        {
            if (_useKeyboard)
                UpdateKeyboardInput(gameTime);

            if (_useMouse)
                UpdateMouseInput(gameTime);

            if (_useGamepad)
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
                _camera.Translate(_strafeSpeed, 0, 0);
            else if (YnG.Keys.Pressed(Keys.D))
                _camera.Translate(-_strafeSpeed, 0, 0);

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
            _camera.RotateY(-YnG.Mouse.Delta.X * 0.5f);
            _camera.Pitch(YnG.Mouse.Delta.Y * 0.5f);
        }
    }
}
