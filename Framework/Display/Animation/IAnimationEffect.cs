using System;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Display.Animation
{
    interface IAnimationEffect
    {
        bool Active { get; set; }
        void Update(GameTime gameTime);
    }
}
