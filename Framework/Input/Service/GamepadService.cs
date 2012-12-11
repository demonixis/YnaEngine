using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.Helpers;

namespace Yna.Input.Service
{
    public class GamepadService : GameComponent, IGamepadService
    {
#if !WINDOWS_PHONE_8
        GamePadState [] state;
        GamePadState [] lastState;

        public GamepadService(Game game)
            : base(game)
        {
            ServiceHelper.Add<IGamepadService>(this);
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

        #region IGamepadService Membres

        bool IGamepadService.Connected(PlayerIndex index)
        {
            return state[(int)index].IsConnected;
        }

        bool IGamepadService.Pressed(PlayerIndex index, Buttons button)
        {
            return state[(int)index].IsButtonDown(button);
        }

        bool IGamepadService.Released(PlayerIndex index, Buttons button)
        {
            return state[(int)index].IsButtonUp(button);
        }

        bool IGamepadService.JustPressed(PlayerIndex index, Buttons button)
        {
            return state[(int)index].IsButtonUp(button) && lastState[(int)index].IsButtonDown(button);
        }

        bool IGamepadService.JustReleased(PlayerIndex index, Buttons button)
        {
            return state[(int)index].IsButtonDown(button) && lastState[(int)index].IsButtonUp(button);
        }

        float IGamepadService.Triggers(PlayerIndex index, bool left)
        {
            if (left)
                return state[(int)index].Triggers.Left;
            else
                return state[(int)index].Triggers.Right;
        }

        Vector2 IGamepadService.ThumbSticks(PlayerIndex index, bool left)
        {
            if (left)
                return state[(int)index].ThumbSticks.Left;
            else
                return state[(int)index].ThumbSticks.Right;
        }

    #endregion
#else
        public GamepadService(Game game)
            : base(game)
        {

        }

        bool IGamepadService.Connected(PlayerIndex index)
        {
            return false;
        }

        bool IGamepadService.Pressed(PlayerIndex index, Buttons button)
        {
            return false;
        }

        bool IGamepadService.Released(PlayerIndex index, Buttons button)
        {
            return false;
        }

        bool IGamepadService.JustPressed(PlayerIndex index, Buttons button)
        {
            return false;
        }

        bool IGamepadService.JustReleased(PlayerIndex index, Buttons button)
        {
            return false;
        }

        float IGamepadService.Triggers(PlayerIndex index, bool left)
        {
            return 0.0f;
        }

        Vector2 IGamepadService.ThumbSticks(PlayerIndex index, bool left)
        {
            return Vector2.Zero;
        }
#endif
    }
}
