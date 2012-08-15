using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Helpers;
using Yna.Input.Service;

namespace Yna.Input
{
    public enum MouseButton
    {
        Left, Middle, Right
    }

    public class YnMouse
    {
        private IMouseService service;

        public YnMouse() 
        {
            service = ServiceHelper.Get<IMouseService>();
        }

        public int X 
        { 
            get { return service.X(); } 
        }

        public int Y
        {
            get { return service.Y(); }
        }

        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
        }

        public int Wheel
        {
            get { return service.Wheel(); }
        }

        public bool Moving
        {
            get { return service.Moving(); }
        }

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
                case MouseButton.Right: result = service.ClickMiddle(state); break;
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
    }
}
