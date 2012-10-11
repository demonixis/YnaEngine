﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Controls
{
    public class ThirdPersonControl : BaseControl
    {
        YnBase3D _followedObject;

        YnBase3D FollowedObject
        {
            get { return _followedObject; }
            set { _followedObject = value; }
        }

        public ThirdPersonControl(ThirdPersonCamera camera, YnBase3D followedObject)
            : base(camera)
        {
            _followedObject = followedObject;
        }

        public ThirdPersonControl(ThirdPersonCamera camera, YnBase3D followedObject, PlayerIndex index)
            : this(camera, followedObject)
        {
            _playerIndex = index;
        }

        protected override void UpdateKeyboardInput(GameTime gameTime)
        {
            // Translation Up/Down
            if (YnG.Keys.Pressed(Keys.A))
                _followedObject.Translate(0, _moveSpeed, 0);
            else if (YnG.Keys.Pressed(Keys.E))
                _followedObject.Translate(0, -_moveSpeed, 0);

            // Translation Forward/backward
            if (YnG.Keys.Pressed(Keys.Z) || YnG.Keys.Up)
                _followedObject.Translate(0, 0, _moveSpeed);
            else if (YnG.Keys.Pressed(Keys.S) || YnG.Keys.Down)
                _followedObject.Translate(0, 0, -_moveSpeed);

            // Translation Left/Right
            if (YnG.Keys.Pressed(Keys.Q))
                _followedObject.Translate(_strafeSpeed, 0, 0);
            else if (YnG.Keys.Pressed(Keys.D))
                _followedObject.Translate(-_strafeSpeed, 0, 0);

            // Rotation Left/Right
            if (YnG.Keys.Left)
                _followedObject.RotateY(_rotateSpeed);
            else if (YnG.Keys.Right)
                _followedObject.RotateY(-_rotateSpeed);

            // Rotate the camera arround the followed object
            if (YnG.Keys.Pressed(Keys.W))
                _camera.RotateY(-_rotateSpeed);
            else if (YnG.Keys.Pressed(Keys.X))
                _camera.RotateY(_rotateSpeed);

            // Add or reduce the distance between camera and object
            if (YnG.Keys.Pressed(Keys.PageUp) || YnG.Keys.Pressed(Keys.PageDown))
            {
                ThirdPersonCamera camera = (_camera as ThirdPersonCamera);

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
            Vector2 leftStickValue = YnG.Gamepad.LeftStickValue(_playerIndex);
            Vector2 rightStickValue = YnG.Gamepad.RightStickValue(_playerIndex);

            _followedObject.Translate(-leftStickValue.X * _moveSpeed, 0, leftStickValue.Y * _moveSpeed);
            _followedObject.RotateY(-rightStickValue.X * _rotateSpeed);

            if (YnG.Gamepad.LeftShoulder(_playerIndex))
                _followedObject.Translate(0, _moveSpeed, 0);
            else if (YnG.Gamepad.RightShoulder(_playerIndex))
                _followedObject.Translate(0, -_moveSpeed, 0);
        }

        protected override void UpdateMouseInput(GameTime gameTime)
        {
            _followedObject.Translate(-YnG.Mouse.Delta.X * 0.5f, 0, -YnG.Mouse.Delta.Y * 0.5f);
        }
    }
}
