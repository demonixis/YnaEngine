using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Helpers;
using Yna.Input;
using Yna.Input.Service;

namespace Yna.Input
{
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

        #region Triggers

        public bool LeftTrigger(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.LeftTrigger);
        }

        public bool LeftShoulder(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.LeftShoulder);
        }

        public bool RightTrigger(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.RightStick);
        }

        public bool RightShoulder(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.RightShoulder);
        }

        public float LeftTriggerValue(PlayerIndex index)
        {
            return service.Triggers(index, true);
        }

        public float RightTriggerValue(PlayerIndex index)
        {
            return service.Triggers(index, false);
        }

        #endregion

        #region Left Thumbstick

        public bool LeftStick(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.LeftStick);
        }

        public bool LeftStickUp(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.LeftThumbstickUp);
        }

        public bool LeftStickDown(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.LeftThumbstickDown);
        }

        public bool LeftStickLeft(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.LeftThumbstickLeft);
        }

        public bool LeftStickRight(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.LeftThumbstickRight);
        }

        public Vector2 LeftStickValue(PlayerIndex index)
        {
            return service.ThumbSticks(index, true);
        }

        #endregion

        #region Right Thumbstick

        public bool RightStick(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.RightStick);
        }

        public bool RightStickUp(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.RightThumbstickUp);
        }

        public bool RightStickDown(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.RightThumbstickDown);
        }

        public bool RightStickLeft(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.RightThumbstickLeft);
        }

        public bool RightStickRight(PlayerIndex index)
        {
            return service.Pressed(index, Buttons.RightThumbstickRight);
        }

        public Vector2 RightStickValue(PlayerIndex index)
        {
            return service.ThumbSticks(index, false);
        }

        #endregion

        #region Public methods

        public bool IsConnected(PlayerIndex index)
        {
            return service.Connected(index);
        }

        public bool JustPressed(PlayerIndex index, Buttons button)
        {
            return service.JustPressed(index, button);
        }

        public bool JustReleased(PlayerIndex index, Buttons button)
        {
            return service.JustReleased(index, button);
        }

        public bool Pressed(PlayerIndex index, Buttons button)
        {
            return service.Pressed(index, button);
        }

        public bool Released(PlayerIndex index, Buttons button)
        {
            return service.Released(index, button);
        }

        #endregion
    }
}
