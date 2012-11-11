using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Light;
namespace Yna
{
    public interface IYnUpdateable
    {
        void Update(GameTime gameTime);
    }

    public interface IYnDrawable2D
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }

    public interface IYnDrawable3D
    {
        void Draw(GameTime gameTime, GraphicsDevice device);
    }

    public interface IYnLightable
    {
        void SetLight(BasicLight light);
        BasicLight GetLight(BasicLight light);
    }
}
