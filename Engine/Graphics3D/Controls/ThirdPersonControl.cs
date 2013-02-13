using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Graphics3D.Camera;

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
            if (camera.FollowedObject == null)
                throw new Exception("[ThirdPersonCamera] The followed object is null");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Camera.Update(gameTime);
        }

        protected override void UpdateKeyboardInput(GameTime gameTime)
        {
            var camera = Camera as ThirdPersonCamera;

            // Translation Up/Down
            if (YnG.Keys.Pressed(Keys.A))
                camera.FollowedObject.Translate(0, _moveSpeed, 0);
            else if (YnG.Keys.Pressed(Keys.E))
                camera.FollowedObject.Translate(0, -_moveSpeed, 0);

            // Translation Forward/backward
            if (YnG.Keys.Pressed(Keys.Z) || YnG.Keys.Up)
                camera.FollowedObject.Translate(0, 0, _moveSpeed);
            else if (YnG.Keys.Pressed(Keys.S) || YnG.Keys.Down)
                camera.FollowedObject.Translate(0, 0, -_moveSpeed);

            // Translation Left/Right
            if (YnG.Keys.Pressed(Keys.Q))
                camera.FollowedObject.Translate(_strafeSpeed, 0, 0);
            else if (YnG.Keys.Pressed(Keys.D))
                camera.FollowedObject.Translate(-_strafeSpeed, 0, 0);

            // Rotation Left/Right
            if (YnG.Keys.Left)
                camera.FollowedObject.RotateY(_rotateSpeed);
            else if (YnG.Keys.Right)
                camera.FollowedObject.RotateY(-_rotateSpeed);

            // Rotate the camera arround the followed object
            if (YnG.Keys.Pressed(Keys.W))
                Camera.RotateY(-_rotateSpeed);
            else if (YnG.Keys.Pressed(Keys.X))
                Camera.RotateY(_rotateSpeed);

            // Add or reduce the distance between camera and object
            if (YnG.Keys.Pressed(Keys.PageUp) || YnG.Keys.Pressed(Keys.PageDown))
            {
                if (YnG.Keys.Pressed(Keys.PageUp))
                {
                    camera.Distance += 0.5f;
                    camera.Angle -= 0.5f;
                }
                else
                {
                    camera.Distance -= 0.5f;
                    camera.Angle += 0.5f;
                }
            }
        }

        protected override void UpdateGamepadInput(GameTime gameTime)
        {
            var camera = Camera as ThirdPersonCamera;

            Vector2 leftStickValue = YnG.Gamepad.LeftStickValue(_playerIndex);
            Vector2 rightStickValue = YnG.Gamepad.RightStickValue(_playerIndex);

            camera.FollowedObject.Translate(-leftStickValue.X * _moveSpeed, 0, leftStickValue.Y * _moveSpeed);
            camera.FollowedObject.RotateY(-rightStickValue.X * _rotateSpeed);

            if (YnG.Gamepad.LeftShoulder(_playerIndex))
                camera.FollowedObject.Translate(0, _moveSpeed, 0);
            else if (YnG.Gamepad.RightShoulder(_playerIndex))
                camera.FollowedObject.Translate(0, -_moveSpeed, 0);
        }

        protected override void UpdateMouseInput(GameTime gameTime)
        {
            (Camera as ThirdPersonCamera).FollowedObject.Translate(-YnG.Mouse.Delta.X * 0.5f, 0, -YnG.Mouse.Delta.Y * 0.5f);
        }
    }
}
