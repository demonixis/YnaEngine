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
            _useGamepad = false;
            _useKeyboard = false;
            _useMouse = false;
            _keyMapper = null;
            _moveSpeed = 0.8f;
            _strafeSpeed = 0.8f;
            _rotateSpeed = 1.4f;
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
            _velocityPosition *= _accelerationPosition * _maxVelocityPosition;
            _velocityRotation *= _accelerationRotation * _maxVelocityRotation;

            _virtualPadController.Update(gameTime);

            UpdateVirtualPadInput(gameTime);
            UpdateTouchInput(gameTime);
            UpdatePhysics(gameTime);

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
                _velocityPosition.Z += _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (_virtualPadController.Pressed(PadButtons.Down))
                _velocityPosition.Z -= _moveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Strafe
            if (_virtualPadController.Pressed(PadButtons.StrafeLeft))
                _velocityPosition.X += _strafeSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (_virtualPadController.Pressed(PadButtons.StrafeRight))
                _velocityPosition.X -= _strafeSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Rotate 
            if (_virtualPadController.Pressed(PadButtons.Left))
                _velocityRotation.Y += _rotateSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (_virtualPadController.Pressed(PadButtons.Right))
                _velocityRotation.Y -= _rotateSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
        }

        /// <summary>
        /// Update touches state
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void UpdateTouchInput(GameTime gameTime)
        {
            /*
             *  // Look Up/Down
            if (YnG.Keys.Pressed(_keyMapper.PitchUp[0]))
                _velocityRotation.X += _pitchSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(_keyMapper.PitchDown[0]))
                _velocityRotation.X -= _pitchSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

            // Roll
            if (YnG.Keys.Pressed(Keys.W))
                _velocityRotation.Z += _pitchSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
            else if (YnG.Keys.Pressed(Keys.X))
                _velocityRotation.Z -= _pitchSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.01f;
             * */

            if (YnG.Touch.Moved)
            {
                _velocityRotation.Y += YnG.Touch.Delta.X * 0.1f;
                _velocityRotation.X -= YnG.Touch.Delta.Y * 0.1f;
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
