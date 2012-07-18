using System;
using Microsoft.Xna.Framework.Input;

namespace Yna.Input
{
    public interface IMouseService
    {
        int X();

        int Y();

        int Wheel();

        bool Moving();

        bool ClickLeft(ButtonState state);

        bool ClickRight(ButtonState state);

        bool ClickMiddle(ButtonState state);
    }
}
