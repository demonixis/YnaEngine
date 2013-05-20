using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yna.Engine.Input.Service
{
    public class MouseComponent : GameComponent
    {
        private MouseState _mouseState;
        private MouseState _lastMouseState;
        private Vector2 _delta;

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

        public MouseComponent(Game game)
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

        #endregion
    }
}
