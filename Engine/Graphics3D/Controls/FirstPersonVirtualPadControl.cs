// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics.Component;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Controls
{

    /// <summary>
    /// A first person controller using a virtual pad
    /// </summary>
    public class FirstPersonVirtualPadControl : BaseControl
    {
        private YnVirtualPadController _virtualPadController;

        /// <summary>
        /// Gets the VirtualPad
        /// </summary>
        public YnVirtualPad VirtualPad
        {
            get { return _virtualPadController.VirtualPad; }
        }

        /// <summary>
        /// Gets the VirtualController used for VirtualPad
        /// </summary>
        public YnVirtualPadController VirtualPadController
        {
            get { return _virtualPadController; }
        }

        /// <summary>
        /// Create a new controller with a default virtual pad
        /// </summary>
        /// <param name="camera"></param>
        public FirstPersonVirtualPadControl(FirstPersonCamera camera)
            : base(camera, PlayerIndex.One)
        {
            _virtualPadController = new YnVirtualPadController();
            Initialize();
        }

        /// <summary>
        /// Create a new controller with a custom virtual pad
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="virtualPad"></param>
        public FirstPersonVirtualPadControl(FirstPersonCamera camera, YnVirtualPad virtualPad)
            : base(camera, PlayerIndex.One)
        {
            _virtualPadController = new YnVirtualPadController(virtualPad);
            Initialize();
        }

        /// <summary>
        /// Initialize default values
        /// </summary>
        protected virtual void Initialize()
        {
            _enableGamepad = false;
            _enableKeyboard = false;
            _enableMouse = false;
            _moveSpeed = 0.8f;
            _strafeSpeed = 0.8f;
            _rotationSpeed = 1.4f;
        }

        /// <summary>
        /// Load assets for the virtual pad
        /// </summary>
        public virtual void LoadContent()
        {
            _virtualPadController.LoadContent();
        }

        /// <summary>
        /// Update camera position
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Physics
            PhysicsPosition.Velocity *= PhysicsPosition.Acceleration * PhysicsPosition.MaxVelocity;
            PhysicsRotation.Velocity *= PhysicsRotation.Acceleration * PhysicsRotation.MaxVelocity;

            _virtualPadController.Update(gameTime);

            UpdateVirtualPadInput(gameTime);
            UpdateTouchInput(gameTime);
            ApplyPhysics(gameTime);

            Camera.Update(gameTime);
        }

        /// <summary>
        /// Update virtual pad state
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void UpdateVirtualPadInput(GameTime gameTime)
        {
            // Move
            if (_virtualPadController.Pressed(PadButtons.Up))
                PhysicsPosition.Velocity.Z += _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (_virtualPadController.Pressed(PadButtons.Down))
                PhysicsPosition.Velocity.Z -= _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Strafe
            if (_virtualPadController.Pressed(PadButtons.StrafeLeft))
                PhysicsPosition.Velocity.X += _strafeSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (_virtualPadController.Pressed(PadButtons.StrafeRight))
                PhysicsPosition.Velocity.X -= _strafeSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Rotate 
            if (_virtualPadController.Pressed(PadButtons.Left))
                PhysicsRotation.Velocity.Y += _rotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (_virtualPadController.Pressed(PadButtons.Right))
                PhysicsRotation.Velocity.Y -= _rotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
        }

        /// <summary>
        /// Update touches state
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void UpdateTouchInput(GameTime gameTime)
        {
            if (YnG.Touch.Moved(0) && !_virtualPadController.hasPressedButton())
            {
                PhysicsRotation.Velocity.Y += YnG.Touch.GetDelta(0).X * 0.1f;
                PhysicsRotation.Velocity.X -= YnG.Touch.GetDelta(0).Y * 0.1f;
            }
        }

        /// <summary>
        /// Draw the virtual pad on screen
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _virtualPadController.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Draw the virtual pad on screen in a separate SpriteBatch
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void DrawOnSingleBatch(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _virtualPadController.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        #region BaseControl members that are not used

        protected override void UpdateKeyboardInput(GameTime gameTime)
        {

        }

        protected override void UpdateGamepadInput(GameTime gameTime)
        {

        }

        protected override void UpdateMouseInput(GameTime gameTime)
        {

        }

        #endregion
    }
}
