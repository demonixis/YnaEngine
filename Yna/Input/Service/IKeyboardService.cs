using System;
using Microsoft.Xna.Framework.Input;

namespace Yna.Input.Service
{
    public interface IKeyboardService
    {
        bool Pressed(Keys key);
        bool Released(Keys key);
        bool JustPressed(Keys key);
        bool JustReleased(Keys key);
    }
}
