using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Helpers;

namespace Yna.Input
{
    public class MouseService : GameComponent, IMouseService
    {
        private MouseState mouseState;
        private MouseState lastMouseState;

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
    }
}
