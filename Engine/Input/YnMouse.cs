// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yna.Engine.Input
{
    public enum MouseButton
    {
        Left, Middle, Right
    }

    public class YnMouse : GameComponent
    {
        private MouseState _mouseState;
        private MouseState _lastMouseState;
        private Vector2 _delta;

        #region Fields

        public int X 
        {
            get { return _mouseState.X; }
        }

        public int Y
        {
            get { return _mouseState.Y; }
        }

        public int Wheel
        {
            get { return _mouseState.ScrollWheelValue; }
        }

        public bool Moving
        {
            get { return (_mouseState.X != _lastMouseState.X) || (_mouseState.Y != _lastMouseState.Y); }
        }

        public Vector2 Delta
        {
            get { return _delta; }
        }

        /// <summary>
        /// Specifies whether the mouse is draggin
        /// </summary>
        /// <param name="button">The button to test</param>
        /// <returns>True if draggin then false</returns>
        public bool Drag(MouseButton button)
        {
            return Click(button) && Moving;
        }


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

        /// <summary>
        /// Get the mouse position on screen
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(_mouseState.X, _mouseState.Y); }
        }

        public Vector2 LastPosition
        {
            get { return new Vector2(_lastMouseState.X, _lastMouseState.Y); }
        }

        #endregion

        public YnMouse(Game game)
            : base(game)
        {
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

        public bool Released(MouseButton button)
        {
            return ClickOn(button, ButtonState.Released);
        }

        public void SetPosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }

        #region Mouse click

        public bool ClickLeft(ButtonState state)
        {
            return _mouseState.LeftButton == state;
        }

        public bool ClickRight(ButtonState state)
        {
            return _mouseState.RightButton == state;
        }

        public bool ClickMiddle(ButtonState state)
        {
            return _mouseState.MiddleButton == state;
        }

        public bool JustClicked(MouseButton button)
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

        public bool JustReleased(MouseButton button)
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

        public bool ClickOn(MouseButton button, ButtonState state)
        {
            bool result = false;

            switch (button)
            {
                case MouseButton.Left: result = ClickLeft(state); break;
                case MouseButton.Middle: result = ClickMiddle(state); break;
                case MouseButton.Right: result = ClickRight(state); break;
            }

            return result;
        }

        public bool Click(MouseButton button)
        {
            return ClickOn(button, ButtonState.Pressed);
        }

        #endregion
    }
}
