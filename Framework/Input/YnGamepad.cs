using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Framework.Helpers;
using Yna.Framework.Input;
using Yna.Framework.Input.Service;

namespace Yna.Framework.Input
{
    /// <summary>
    /// The gamepad manager
    /// </summary>
    public class YnGamepad
    {
        private IGamepadService service;

        public YnGamepad()
        {
            service = ServiceHelper.Get<IGamepadService>();
        }

        #region Digital pad

        public bool Up(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.DPadUp);
        }

        public bool Down(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.DPadDown);
        }

        public bool Left(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.DPadLeft);
        }

        public bool Right(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.DPadRight);
        }

        #endregion

        #region Buttons

        public bool A(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.A);
        }

        public bool B(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.B);
        }

        public bool X(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.X);
        }

        public bool Y(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.Y);
        }

        public bool Start(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.Start);
        }

        public bool Back(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.Back);
        }

        public bool Guide(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.BigButton);
        }

        #endregion
    }
}
