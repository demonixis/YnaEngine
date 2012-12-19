﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Framework.Helpers;
using Yna.Framework.Input.Service;

namespace Yna.Framework.Input
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
        private IMouseService service;

        private Vector2 _delta;

        public YnMouse() 
        {
            service = ServiceHelper.Get<IMouseService>();
        }

        /// <summary>
        /// Get the X coordinate of the mouse on the screen
        /// </summary>
        public int X 
        { 
            get { return service.X(); } 
        }

        /// <summary>
        /// Get the Y coordinate of the mouse on the screen
        /// </summary>
        public int Y
        {
            get { return service.Y(); }
        }

        /// <summary>
        /// Get the mouse position on screen
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
        }

        public Vector2 Delta
        {
            get 
            {
                _delta = new Vector2(
                    (X == LastMouseState.X) ? 0 : (LastMouseState.X - X),
                    (Y == LastMouseState.Y) ? 0 : (LastMouseState.Y - Y));

                return _delta; 
            }
            protected set { _delta = value; }
        }

        /// <summary>
        /// Get the wheel value
        /// </summary>
        public int Wheel
        {
            get { return service.Wheel(); }
        }

        /// <summary>
        /// Get the position state
        /// </summary>
        public bool Moving
        {
            get { return service.Moving(); }
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
                case MouseButton.Left: result = service.ClickLeft(state); break;
                case MouseButton.Middle: result = service.ClickMiddle(state); break;
                case MouseButton.Right: result = service.ClickRight(state); break;
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
            return service.JustClicked(button);
        }

        public bool JustReleased(MouseButton button)
        {
            return service.JustReleased(button);
        }

        /// <summary>
        /// Get the currrent mouse state
        /// </summary>
        public MouseState MouseState
        {
            get { return (service as MouseService).MouseState; }
        }

        /// <summary>
        /// Get the last mouse state
        /// </summary>
        public MouseState LastMouseState
        {
            get { return (service as MouseService).LastMouseState; }
        }

        public void SetPosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }

        public void ResetDelta()
        {
            Delta = Vector2.Zero;
        }
    }
}