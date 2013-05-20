using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Input.Service;

namespace Yna.Engine.Input
{
    public enum MouseButton
    {
        Left, Middle, Right
    }

    /// <summary>
    /// The mouse manager
    /// </summary>
    public class YnMouse
    {
        private MouseComponent _mouseComponent;

        public YnMouse(MouseComponent component) 
        {
            _mouseComponent = component;
        }

        public int X
        {
            get { return _mouseComponent.X; }
        }

        public int Y
        {
            get { return _mouseComponent.Y; }
        }

        /// <summary>
        /// Get the mouse position on screen
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(_mouseComponent.X, _mouseComponent.Y); }
        }

        public Vector2 LastPosition
        {
            get { return new Vector2(LastMouseState.X, LastMouseState.Y); }
        }

        public Vector2 Delta
        {
            get { return _mouseComponent.Delta; }
        }

        /// <summary>
        /// Get the wheel value
        /// </summary>
        public int Wheel
        {
            get { return _mouseComponent.Wheel; }
        }

        /// <summary>
        /// Get the position state
        /// </summary>
        public bool Moving
        {
            get { return _mouseComponent.Moving; }
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

        public bool Drop (MouseButton button)
        {
            return Released(button) && !Moving;
        }

        public bool ClickOn(MouseButton button, ButtonState state)
        {
            bool result = false;

            switch (button)
            {
                case MouseButton.Left: result = _mouseComponent.ClickLeft(state); break;
                case MouseButton.Middle: result = _mouseComponent.ClickMiddle(state); break;
                case MouseButton.Right: result = _mouseComponent.ClickRight(state); break;
            }

            return result; 
        }

        public bool Click(MouseButton button)
        {
            return ClickOn(button, ButtonState.Pressed);
        }

        public bool Released(MouseButton button)
        {
            return ClickOn(button, ButtonState.Released);
        }

        public bool JustClicked(MouseButton button)
        {
            return _mouseComponent.JustClicked(button);
        }

        public bool JustReleased(MouseButton button)
        {
            return _mouseComponent.JustReleased(button);
        }

        /// <summary>
        /// Get the currrent mouse state
        /// </summary>
        public MouseState MouseState
        {
            get { return _mouseComponent.MouseState; }
        }

        /// <summary>
        /// Get the last mouse state
        /// </summary>
        public MouseState LastMouseState
        {
            get { return _mouseComponent.LastMouseState; }
        }

        public void SetPosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }

        public void ResetDelta()
        {
            
        }
    }
}
