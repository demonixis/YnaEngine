using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Controls
{
    /// <summary>
    /// A first person controller who can be used with keyboard, mouse and gamepad
    /// </summary>
    public class FirstPersonControl : BaseControl
    {
        
        /// <summary>
        /// Create a new FPS Controller for the first player
        /// </summary>
        /// <param name="camera">The FirstPersonCamera to use with this controller</param>
        public FirstPersonControl(FirstPersonCamera camera)
            : this(camera, PlayerIndex.One)
        {
  
        }

        /// <summary>
        /// Create a new FPS controller for the specified player
        /// </summary>
        /// <param name="camera">The FirstPersonCamera to use with this controller</param>
        public FirstPersonControl(FirstPersonCamera camera, PlayerIndex index)
            : base(camera, index)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ApplyPhysics(gameTime);

            Camera.Update(gameTime);
        }

        protected override void UpdateKeyboardInput(GameTime gameTime)
        {
            // Translation Up/Down
            if (YnG.Keys.Pressed(Keys.A))
                _velocityPosition.Y += _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(Keys.E))
                _velocityPosition.Y -= _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Translation Forward/backward
            if (YnG.Keys.Pressed(_keyMapper.Up[0]) || YnG.Keys.Pressed(_keyMapper.Up[1]))
                _velocityPosition.Z += _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(_keyMapper.Down[0]) || YnG.Keys.Pressed(_keyMapper.Down[1]))
                _velocityPosition.Z -= _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Translation Left/Right
            if (YnG.Keys.Pressed(_keyMapper.StrafeLeft[0]))
                _velocityPosition.X += _strafeSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(_keyMapper.StrafeRight[0]))
                _velocityPosition.X -= _strafeSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Rotation Left/Right
            if (YnG.Keys.Pressed(_keyMapper.Left[0]))
                _velocityRotation.Y += _rotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(_keyMapper.Right[0]))
                _velocityRotation.Y -= _rotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Look Up/Down
            if (YnG.Keys.Pressed(_keyMapper.PitchUp[0]))
                _velocityRotation.X += _pitchSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(_keyMapper.PitchDown[0]))
                _velocityRotation.X -= _pitchSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Roll
            if (YnG.Keys.Pressed(Keys.W))
                _velocityRotation.Z += _pitchSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(Keys.X))
                _velocityRotation.Z -= _pitchSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
        }

        protected override void UpdateGamepadInput(GameTime gameTime)
        {
            Vector2 leftStickValue = YnG.Gamepad.LeftStickValue(_playerIndex);
            Vector2 rightStickValue = YnG.Gamepad.RightStickValue(_playerIndex);

            // Translate
            _velocityPosition.X += -leftStickValue.X * _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            _velocityPosition.Z += leftStickValue.Y * _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Rotate
            _velocityRotation.Y += -rightStickValue.X * _rotationSpeed  * gameTime.ElapsedGameTime.Milliseconds * 0.01f; 

            // Pitch
            _velocityRotation.X += -rightStickValue.Y * _pitchSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f; 

            // Move Up
            if (YnG.Gamepad.LeftShoulder(_playerIndex))
                _velocityPosition.Y += _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Gamepad.RightShoulder(_playerIndex))
                _velocityPosition.Y -= _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
        }

        protected override void UpdateMouseInput(GameTime gameTime)
        {
            Camera.RotateY(YnG.Mouse.Delta.X * 0.2f);
            Camera.RotateX(-YnG.Mouse.Delta.Y * 0.2f);
        }
    }
}
