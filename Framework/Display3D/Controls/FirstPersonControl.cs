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

            Camera.Update(gameTime);
        }

        protected override void UpdateKeyboardInput(GameTime gameTime)
        {
            // Translation Up/Down
            if (YnG.Keys.Pressed(Keys.A))
                Camera.Translate(0, _moveSpeed, 0);
            else if (YnG.Keys.Pressed(Keys.E))
                Camera.Translate(0, -_moveSpeed, 0);

            // Translation Forward/backward
            if (YnG.Keys.Pressed(Keys.Z) || YnG.Keys.Up)
                Camera.Translate(0, 0, _moveSpeed);
            else if (YnG.Keys.Pressed(Keys.S) || YnG.Keys.Down)
                Camera.Translate(0, 0, -_moveSpeed);

            // Look Up/Down
            if (YnG.Keys.Pressed(Keys.PageUp))
                Camera.Pitch(-_pitchSpeed);
            else if (YnG.Keys.Pressed(Keys.PageDown))
                Camera.Pitch(_pitchSpeed);

            // Translation Left/Right
            if (YnG.Keys.Pressed(Keys.Q))
                Camera.Translate(_strafeSpeed, 0, 0);
            else if (YnG.Keys.Pressed(Keys.D))
                Camera.Translate(-_strafeSpeed, 0, 0);

            // Rotation Left/Right
            if (YnG.Keys.Left)
                Camera.RotateY(_rotateSpeed);
            else if (YnG.Keys.Right)
                Camera.RotateY(-_rotateSpeed);

            if (YnG.Keys.Pressed(Keys.W))
                Camera.Roll(_rotateSpeed);
            else if (YnG.Keys.Pressed(Keys.X))
                Camera.Roll(-_rotateSpeed);
        }

        protected override void UpdateGamepadInput(GameTime gameTime)
        {
            Vector2 leftStickValue = YnG.Gamepad.LeftStickValue(_playerIndex);
            Vector2 rightStickValue = YnG.Gamepad.RightStickValue(_playerIndex);

            // Translate/Rotate/Picth
            Camera.Translate(-leftStickValue.X * _moveSpeed, 0, leftStickValue.Y * _moveSpeed);
            Camera.RotateY(-rightStickValue.X * _rotateSpeed);
            Camera.Pitch(-rightStickValue.Y * _pitchSpeed);

            // Move Up
            if (YnG.Gamepad.LeftShoulder(_playerIndex))
                Camera.Translate(0, _moveSpeed, 0);
            else if (YnG.Gamepad.RightShoulder(_playerIndex))
                Camera.Translate(0, -_moveSpeed, 0);
        }

        protected override void UpdateMouseInput(GameTime gameTime)
        {
            Camera.RotateY(YnG.Mouse.Delta.X * 0.2f);
            Camera.Pitch(-YnG.Mouse.Delta.Y * 0.2f);
        }
    }
}
