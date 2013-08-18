// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yna.Engine.Input
{
#if !WINDOWS_PHONE_8
    public class YnGamepad : GameComponent
    {
        GamePadState [] state;
        GamePadState [] lastState;

        public YnGamepad(Game game)
            : base(game)
        {
            state = new GamePadState[4];
            lastState = new GamePadState[4];
            
            for (int i = 0; i < 4; i++)
            {
                state[i] = GamePad.GetState((PlayerIndex)i);
                lastState[i] = state[i];
            }
        }

        public override void Update(GameTime gameTime)
		{
            for (int i = 0; i < 4; i++)
            {
                lastState[i] = state[i];
                state[i] = GamePad.GetState((PlayerIndex)i);
            }

            base.Update(gameTime);
        }

        public bool Connected(PlayerIndex index)
        {
            return state[(int)index].IsConnected;
        }

        public bool Pressed(PlayerIndex index, Buttons button)
        {
            return state[(int)index].IsButtonDown(button);
        }

        public bool Released(PlayerIndex index, Buttons button)
        {
            return state[(int)index].IsButtonUp(button);
        }

        public bool JustPressed(PlayerIndex index, Buttons button)
        {
            return state[(int)index].IsButtonUp(button) && lastState[(int)index].IsButtonDown(button);
        }

        public bool JustReleased(PlayerIndex index, Buttons button)
        {
            return state[(int)index].IsButtonDown(button) && lastState[(int)index].IsButtonUp(button);
        }

        public float Triggers(PlayerIndex index, bool left)
        {
            if (left)
                return state[(int)index].Triggers.Left;
            else
                return state[(int)index].Triggers.Right;
        }

        public Vector2 ThumbSticks(PlayerIndex index, bool left)
        {
            if (left)
                return state[(int)index].ThumbSticks.Left;
            else
                return state[(int)index].ThumbSticks.Right;
        }

        #region Digital pad

        public bool Up(PlayerIndex index)
        {
            return Pressed(index, Buttons.DPadUp);
        }

        public bool Down(PlayerIndex index)
        {
            return Pressed(index, Buttons.DPadDown);
        }

        public bool Left(PlayerIndex index)
        {
            return Pressed(index, Buttons.DPadLeft);
        }

        public bool Right(PlayerIndex index)
        {
            return Pressed(index, Buttons.DPadRight);
        }

        #endregion

        #region Buttons

        public bool A(PlayerIndex index)
        {
            return Pressed(index, Buttons.A);
        }

        public bool B(PlayerIndex index)
        {
            return Pressed(index, Buttons.B);
        }

        public bool X(PlayerIndex index)
        {
            return Pressed(index, Buttons.X);
        }

        public bool Y(PlayerIndex index)
        {
            return Pressed(index, Buttons.Y);
        }

        public bool Start(PlayerIndex index)
        {
            return Pressed(index, Buttons.Start);
        }

        public bool Back(PlayerIndex index)
        {
            return Pressed(index, Buttons.Back);
        }

        public bool Guide(PlayerIndex index)
        {
            return Pressed(index, Buttons.BigButton);
        }

        #endregion

        #region Triggers

        public bool LeftTrigger(PlayerIndex index)
        {
            return Pressed(index, Buttons.LeftTrigger);
        }

        public bool LeftShoulder(PlayerIndex index)
        {
            return Pressed(index, Buttons.LeftShoulder);
        }

        public bool RightTrigger(PlayerIndex index)
        {
            return Pressed(index, Buttons.RightTrigger);
        }

        public bool RightShoulder(PlayerIndex index)
        {
            return Pressed(index, Buttons.RightShoulder);
        }

        public float LeftTriggerValue(PlayerIndex index)
        {
            return Triggers(index, true);
        }

        public float RightTriggerValue(PlayerIndex index)
        {
            return Triggers(index, false);
        }

        #endregion

        #region Left Thumbstick

        public bool LeftStick(PlayerIndex index)
        {
            return Pressed(index, Buttons.LeftStick);
        }

        public bool LeftStickUp(PlayerIndex index)
        {
            return Pressed(index, Buttons.LeftThumbstickUp);
        }

        public bool LeftStickDown(PlayerIndex index)
        {
            return Pressed(index, Buttons.LeftThumbstickDown);
        }

        public bool LeftStickLeft(PlayerIndex index)
        {
            return Pressed(index, Buttons.LeftThumbstickLeft);
        }

        public bool LeftStickRight(PlayerIndex index)
        {
            return Pressed(index, Buttons.LeftThumbstickRight);
        }

        public Vector2 LeftStickValue(PlayerIndex index)
        {
            return ThumbSticks(index, true);
        }

        #endregion

        #region Right Thumbstick

        public bool RightStick(PlayerIndex index)
        {
            return Pressed(index, Buttons.RightStick);
        }

        public bool RightStickUp(PlayerIndex index)
        {
            return Pressed(index, Buttons.RightThumbstickUp);
        }

        public bool RightStickDown(PlayerIndex index)
        {
            return Pressed(index, Buttons.RightThumbstickDown);
        }

        public bool RightStickLeft(PlayerIndex index)
        {
            return Pressed(index, Buttons.RightThumbstickLeft);
        }

        public bool RightStickRight(PlayerIndex index)
        {
            return Pressed(index, Buttons.RightThumbstickRight);
        }

        public Vector2 RightStickValue(PlayerIndex index)
        {
            return ThumbSticks(index, false);
        }

        #endregion
    }
#else
    public class GamepadComponent : GameComponent
    {
        public GamepadComponent(Game game)
            : base(game)
        {

        }

        public bool Connected(PlayerIndex index)
        {
            return false;
        }

        public bool Pressed(PlayerIndex index, Buttons button)
        {
            return false;
        }

        public bool Released(PlayerIndex index, Buttons button)
        {
            return false;
        }

        public bool JustPressed(PlayerIndex index, Buttons button)
        {
            return false;
        }

        public bool JustReleased(PlayerIndex index, Buttons button)
        {
            return false;
        }

        public float Triggers(PlayerIndex index, bool left)
        {
            return 0.0f;
        }

        public Vector2 ThumbSticks(PlayerIndex index, bool left)
        {
            return Vector2.Zero;
        }
    }
#endif
}
