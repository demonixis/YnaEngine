using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yna.Input.Service
{
    public interface IGamepadService
    {
        bool Connected(PlayerIndex index);

        bool Pressed(PlayerIndex index, Buttons button);

        bool Released(PlayerIndex index, Buttons button);

        bool JustPressed(PlayerIndex index, Buttons button);

        bool JustReleased(PlayerIndex index, Buttons button);

        float Triggers(PlayerIndex index, bool left);

        Vector2 ThumbSticks(PlayerIndex index, bool left);
    }
}
