using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yna.Engine.Input.Service
{
#if !WINDOWS_PHONE_8
    public class GamepadComponent : GameComponent
    {
        GamePadState [] state;
        GamePadState [] lastState;

        public GamepadComponent(Game game)
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
