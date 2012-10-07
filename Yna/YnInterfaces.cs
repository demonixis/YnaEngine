using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna
{
    public interface YnContentLoadable
    {
        void LoadContent();
    }

    public interface IYnUpdatable
    {
        void Update(GameTime gameTime);
    }

    public interface IYnDrawable2
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }

    public interface IYnDrawable3
    {
        void Draw(GraphicsDevice device);
    }
}
