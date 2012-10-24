using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna
{
    public interface YnUpdateable
    {
        void Update(GameTime gameTime);
    }

    public interface YnDrawable2D
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }

    public interface YnDrawable3D
    {
        void Draw(GameTime gameTime, GraphicsDevice device);
    }

    public interface IManager
    {
        void Next();
        void Previous();
        void First();
        void Last();
    }
}
