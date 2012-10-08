using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Helpers;

namespace Yna.Input.Service
{
    public class MouseService : GameComponent, IMouseService
    {
        private MouseState mouseState;
        private MouseState lastMouseState;

        public MouseState MouseState
        {
            get { return mouseState; }
        }

        public MouseState LastMouseState
        {
            get { return lastMouseState; }
        }

        public Vector2 Delta
        {
            get 
            { 
                return new Vector2(
                    mouseState.X - lastMouseState.X, 
                    mouseState.Y - lastMouseState.Y); 
            }
        }

        public MouseService(Game game)
            : base(game)
        {
            ServiceHelper.Add<IMouseService>(this);
            mouseState = Mouse.GetState();
            lastMouseState = mouseState;
        }

        public override void Update(GameTime gameTime)
        {
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
            base.Update(gameTime); 
        }

        bool IMouseService.Moving()
        {
            return (mouseState.X != lastMouseState.X) || (mouseState.Y != lastMouseState.Y);
        }

        int IMouseService.X()
        {
            return mouseState.X;
        }

        int IMouseService.Y()
        {
            return mouseState.Y;
        }

        int IMouseService.Wheel()
        {
            return mouseState.ScrollWheelValue;
        }

        bool IMouseService.ClickLeft(ButtonState state)
        {
            return mouseState.LeftButton == state;
        }

        bool IMouseService.ClickRight(ButtonState state)
        {
            return mouseState.RightButton == state;
        }

        bool IMouseService.ClickMiddle(ButtonState state)
        {
            return mouseState.MiddleButton == state;
        }

        bool IMouseService.JustClicked(MouseButton button)
        {
            bool justClicked = false;

            if (button == MouseButton.Left)
                justClicked = mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released;
            else if (button == MouseButton.Middle)
                justClicked = mouseState.MiddleButton == ButtonState.Pressed && lastMouseState.MiddleButton == ButtonState.Released;
            else if (button == MouseButton.Right)
                justClicked = mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released;

            return justClicked;
        }

        bool IMouseService.JustReleased(MouseButton button)
        {
            bool justReleased = false;

            if (button == MouseButton.Left)
                justReleased = mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed;
            else if (button == MouseButton.Middle)
                justReleased = mouseState.MiddleButton == ButtonState.Released && lastMouseState.MiddleButton == ButtonState.Pressed;
            else if (button == MouseButton.Right)
                justReleased = mouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed;

            return justReleased;
        }
    }
}
