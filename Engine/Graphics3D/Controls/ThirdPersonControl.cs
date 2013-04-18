using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Input;

namespace Yna.Engine.Graphics3D.Controls
{
    /// <summary>
    /// Define a Third Person Controller who will move an object
    /// </summary>
    public class ThirdPersonControl : BaseControl
    {
        /// <summary>
        /// Create a Third Person Controller with a camera. The camera must be have registered a followed object
        /// </summary>
        /// <param name="camera">Camera to use</param>
        public ThirdPersonControl(ThirdPersonCamera camera)
            : base(camera)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ApplyPhysics(gameTime);
            Camera.Update(gameTime);
        }

        public override void ApplyPhysics(GameTime gameTime)
        {
            var camera = Camera as ThirdPersonCamera;
            camera.FollowedObject.Translate(_velocityPosition.X * _xDirection, _velocityPosition.Y * _yDirection, _velocityPosition.Z * _zDirection);
            camera.FollowedObject.RotateY(_velocityRotation.Y * _yRotation);
        }

        protected override void UpdateKeyboardInput(GameTime gameTime)
        {
            // Translation Up/Down
            if (YnG.Keys.Pressed(Keys.A))
                _velocityPosition.Y += _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(Keys.E))
                _velocityPosition.Y -= _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Translation Forward/backward
            if (YnG.Keys.Pressed(Keys.Z) || YnG.Keys.Up)
                _velocityPosition.Z += _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(Keys.S) || YnG.Keys.Down)
                _velocityPosition.Z -= _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Translation Left/Right
            if (YnG.Keys.Pressed(Keys.Q))
                _velocityPosition.X += _strafeSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(Keys.D))
                _velocityPosition.X -= _strafeSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Rotation Left/Right
            if (YnG.Keys.Left)
                _velocityRotation.Y += _rotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Right)
                _velocityRotation.Y -= _rotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Rotate the camera arround the followed object
            if (YnG.Keys.Pressed(Keys.W))
                Camera.RotateY(-_rotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.1f);
            else if (YnG.Keys.Pressed(Keys.X))
                Camera.RotateY(_rotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.1f);
        }

        protected override void UpdateGamepadInput(GameTime gameTime)
        {
            Vector2 leftStickValue = YnG.Gamepad.LeftStickValue(_playerIndex);
            Vector2 rightStickValue = YnG.Gamepad.RightStickValue(_playerIndex);

            _velocityPosition.X += leftStickValue.X * _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            _velocityPosition.Z += -leftStickValue.Y * _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            _velocityRotation.Y += -rightStickValue.X * _rotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            if (YnG.Gamepad.LeftTrigger(PlayerIndex.One))
                _velocityPosition.Y += _moveSpeed * YnG.Gamepad.LeftTriggerValue(PlayerIndex.One) * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Gamepad.RightTrigger(PlayerIndex.One))
                _velocityPosition.Y += -_moveSpeed * YnG.Gamepad.RightTriggerValue(PlayerIndex.One) * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            Camera.RotateX(-rightStickValue.Y * _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.1f);


            if (YnG.Gamepad.LeftShoulder(PlayerIndex.One))
                Camera.RotateY(-_moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.1f);
            else if (YnG.Gamepad.RightShoulder(PlayerIndex.One))
                Camera.RotateY(_moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.1f);

            //Camera.RotateY(-rightStickValue.X * _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.1f);
        }

        protected override void UpdateMouseInput(GameTime gameTime)
        {
            var camera = Camera as ThirdPersonCamera;

            if (YnG.Mouse.Click(MouseButton.Left) || YnG.Mouse.Click(MouseButton.Right))
            {
                if (YnG.Mouse.Click(MouseButton.Left))
                    _velocityPosition.Z += YnG.Mouse.Delta.Y * 0.5f * _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

                _velocityRotation.Y = -YnG.Mouse.Delta.X * 0.5f * _rotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            }

            if (YnG.Mouse.Click(MouseButton.Right))
            {
                Camera.RotateX(Camera.Pitch + YnG.Mouse.Delta.Y);
                Camera.RotateY(Camera.Roll - YnG.Mouse.Delta.X);
            }

            else if (YnG.Mouse.Click(MouseButton.Middle))
            {
                camera.Distance += YnG.Mouse.Delta.X;
                camera.Angle += YnG.Mouse.Delta.Y;
            }
        }
    }
}
