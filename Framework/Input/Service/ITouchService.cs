using System;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Input.Service
{
    public interface ITouchService
    {
        bool Pressed(int id);
        bool Released(int id);
        bool Moved(int id);
        Vector2 GetPosition(int id);
        Vector2 GetLastPosition(int id);
    }
}
