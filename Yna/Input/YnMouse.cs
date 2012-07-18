using System;
using Microsoft.Xna.Framework.Input;
using Yna.Helpers;

namespace Yna.Input
{
    public enum MouseButton
    {
        Left, Middle, Right
    }

    public class YnMouse
    {
        public YnMouse() { }

        public int X 
        { 
            get { return ServiceHelper.Get<IMouseService>().X(); } 
        }

        public int Y
        {
            get { return ServiceHelper.Get<IMouseService>().Y(); }
        }

        public int Wheel
        {
            get { return ServiceHelper.Get<IMouseService>().Wheel(); }
        }

        public bool Moving
        {
            get { return ServiceHelper.Get<IMouseService>().Moving(); }
        }

        public bool Drag(MouseButton button)
        {
            return Clicked(button) && Moving;
        }

        public bool Drop (MouseButton button)
        {
            return Released(button) && !Moving;
        }

        public bool Click(MouseButton button, ButtonState state)
        {
            bool result = false;

            switch (button)
            {
                case MouseButton.Left: result = ServiceHelper.Get<IMouseService>().ClickLeft(state); break;
                case MouseButton.Middle: result = ServiceHelper.Get<IMouseService>().ClickMiddle(state); break;
                case MouseButton.Right: result = ServiceHelper.Get<IMouseService>().ClickMiddle(state); break;
            }

            return result; 
        }

        public bool Clicked(MouseButton button)
        {
            return Click(button, ButtonState.Pressed);
        }

        public bool Released(MouseButton button)
        {
            return Click(button, ButtonState.Released);
        }
    }
}
