using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Framework.Helpers;

namespace Yna.Framework.Input.Service
{
    public class MouseService : GameComponent, IMouseService
    {
        private MouseState _mouseState;
        private MouseState _lastMouseState;
        private Vector2 _delta;

        #region IMouseService properties

        int IMouseService.X
        {
            get { return _mouseState.X; }
        }

        int IMouseService.Y
        {
            get { return _mouseState.Y; }
        }

        int IMouseService.Wheel
        {
            get { return _mouseState.ScrollWheelValue; }
        }

        bool IMouseService.Moving
        {
            get { return (_mouseState.X != _lastMouseState.X) || (_mouseState.Y != _lastMouseState.Y); }
        }

        Vector2 IMouseService.Delta
        {
            get { return _delta; }
        }

        #endregion

        #region Service properties

        /// <summary>
        /// Gets the current mouse state
        /// </summary>
        public MouseState MouseState
        {
            get { return _mouseState; }
        }

        /// <summary>
        /// Get the last mouse state
        /// </summary>
        public MouseState LastMouseState
        {
            get { return _lastMouseState; }
        }

        #endregion

        public MouseService(Game game)
            : base(game)
        {
            ServiceHelper.Add<IMouseService>(this);
            _mouseState = Mouse.GetState();
            _lastMouseState = _mouseState;
            _delta = new Vector2();
        }

        public override void Update(GameTime gameTime)
        {
            // Update states
            _lastMouseState = _mouseState;
            _mouseState = Mouse.GetState();

            // Calculate the delta
            _delta.X = _mouseState.X - _lastMouseState.X;
            _delta.Y = _mouseState.Y - _lastMouseState.Y;

            base.Update(gameTime);
        }

        #region Mouse click

        bool IMouseService.ClickLeft(ButtonState state)
        {
            return _mouseState.LeftButton == state;
        }

        bool IMouseService.ClickRight(ButtonState state)
        {
            return _mouseState.RightButton == state;
        }

        bool IMouseService.ClickMiddle(ButtonState state)
        {
            return _mouseState.MiddleButton == state;
        }

        bool IMouseService.JustClicked(MouseButton button)
        {
            bool justClicked = false;

            if (button == MouseButton.Left)
                justClicked = _mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released;
            else if (button == MouseButton.Middle)
                justClicked = _mouseState.MiddleButton == ButtonState.Pressed && _lastMouseState.MiddleButton == ButtonState.Released;
            else if (button == MouseButton.Right)
                justClicked = _mouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released;

            return justClicked;
        }

        bool IMouseService.JustReleased(MouseButton button)
        {
            bool justReleased = false;

            if (button == MouseButton.Left)
                justReleased = _mouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed;
            else if (button == MouseButton.Middle)
                justReleased = _mouseState.MiddleButton == ButtonState.Released && _lastMouseState.MiddleButton == ButtonState.Pressed;
            else if (button == MouseButton.Right)
                justReleased = _mouseState.RightButton == ButtonState.Released && _lastMouseState.RightButton == ButtonState.Pressed;

            return justReleased;
        }

        #endregion
    }
}
