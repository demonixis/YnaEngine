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

        protected override void UpdateKeyboardInput(GameTime gameTime)
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
                _camera.Pitch(-_pitchSpeed);
            else if (YnG.Keys.Pressed(Keys.PageDown))
                _camera.Pitch(_pitchSpeed);

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

            if (YnG.Keys.Pressed(Keys.W))
                _camera.Roll(_rotateSpeed);
            else if (YnG.Keys.Pressed(Keys.X))
                _camera.Roll(-_rotateSpeed);
        }

        protected override void UpdateGamepadInput(GameTime gameTime)
        {
            Vector2 leftStickValue = YnG.Gamepad.LeftStickValue(_playerIndex);
            Vector2 rightStickValue = YnG.Gamepad.RightStickValue(_playerIndex);

            // Translate/Rotate/Picth
            _camera.Translate(-leftStickValue.X * _moveSpeed, 0, leftStickValue.Y * _moveSpeed);
            _camera.RotateY(-rightStickValue.X * _rotateSpeed);
            _camera.Pitch(-rightStickValue.Y * _pitchSpeed);

            // Move Up
            if (YnG.Gamepad.LeftShoulder(_playerIndex))
                _camera.Translate(0, _moveSpeed, 0);
            else if (YnG.Gamepad.RightShoulder(_playerIndex))
                _camera.Translate(0, -_moveSpeed, 0);
        }

        protected override void UpdateMouseInput(GameTime gameTime)
        {
            _camera.RotateY(YnG.Mouse.Delta.X * 0.2f);
            _camera.Pitch(-YnG.Mouse.Delta.Y * 0.2f);
        }
    }
}
