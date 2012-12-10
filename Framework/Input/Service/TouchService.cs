using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Yna.Input.Service
{
    public class TouchService : GameComponent, ITouchService
    {
        TouchCollection touchCollection;
        TouchCollection lastTouchCollection;

        public TouchService(Game game)
            : base(game)
        {
            touchCollection = TouchPanel.GetState();
            lastTouchCollection = touchCollection;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            lastTouchCollection = touchCollection;
            touchCollection = TouchPanel.GetState();
        }
    }
}
