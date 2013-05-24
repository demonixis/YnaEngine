using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Input.Service;

namespace Yna.Engine.Input
{
    /// <summary>
    /// The gamepad manager
    /// </summary>
    public class YnGamepad
    {
        GamepadComponent _gamepadComponent;

        public YnGamepad(GamepadComponent gamepadComponent)
        {
            _gamepadComponent = gamepadComponent;
        }

        #region Digital pad

        public bool Up(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.DPadUp);
        }

        public bool Down(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.DPadDown);
        }

        public bool Left(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.DPadLeft);
        }

        public bool Right(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.DPadRight);
        }

        #endregion

        #region Buttons

        public bool A(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.A);
        }

        public bool B(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.B);
        }

        public bool X(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.X);
        }

        public bool Y(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.Y);
        }

        public bool Start(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.Start);
        }

        public bool Back(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.Back);
        }

        public bool Guide(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.BigButton);
        }

        #endregion

        #region Triggers

        public bool LeftTrigger(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.LeftTrigger);
        }

        public bool LeftShoulder(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.LeftShoulder);
        }

        public bool RightTrigger(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.RightTrigger);
        }

        public bool RightShoulder(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.RightShoulder);
        }

        public float LeftTriggerValue(PlayerIndex index)
        {
            return _gamepadComponent.Triggers(index, true);
        }

        public float RightTriggerValue(PlayerIndex index)
        {
            return _gamepadComponent.Triggers(index, false);
        }

        #endregion

        #region Left Thumbstick

        public bool LeftStick(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.LeftStick);
        }

        public bool LeftStickUp(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.LeftThumbstickUp);
        }

        public bool LeftStickDown(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.LeftThumbstickDown);
        }

        public bool LeftStickLeft(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.LeftThumbstickLeft);
        }

        public bool LeftStickRight(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.LeftThumbstickRight);
        }

        public Vector2 LeftStickValue(PlayerIndex index)
        {
            return _gamepadComponent.ThumbSticks(index, true);
        }

        #endregion

        #region Right Thumbstick

        public bool RightStick(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.RightStick);
        }

        public bool RightStickUp(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.RightThumbstickUp);
        }

        public bool RightStickDown(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.RightThumbstickDown);
        }

        public bool RightStickLeft(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.RightThumbstickLeft);
        }

        public bool RightStickRight(PlayerIndex index)
        {
            return _gamepadComponent.Pressed(index, Buttons.RightThumbstickRight);
        }

        public Vector2 RightStickValue(PlayerIndex index)
        {
            return _gamepadComponent.ThumbSticks(index, false);
        }

        #endregion

        #region Public methods

        public bool IsConnected(PlayerIndex index)
        {
            return _gamepadComponent.Connected(index);
        }

        public bool JustPressed(PlayerIndex index, Buttons button)
        {
            return _gamepadComponent.JustPressed(index, button);
        }

        public bool JustReleased(PlayerIndex index, Buttons button)
        {
            return _gamepadComponent.JustReleased(index, button);
        }

        public bool Pressed(PlayerIndex index, Buttons button)
        {
            return _gamepadComponent.Pressed(index, button);
        }

        public bool Released(PlayerIndex index, Buttons button)
        {
            return _gamepadComponent.Released(index, button);
        }

        #endregion
    }
}
