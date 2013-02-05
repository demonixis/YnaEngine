using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine
{
    public interface IContent
    {
        void Initialize();
        void LoadContent(ContentManager content);
        void UnloadContent();
    }

    public interface IYnUpdateable
    {
        bool Enabled { get; set; }
        void Update(GameTime gameTime);
    }

    public interface IDrawable2
    {
        bool Visible { get; set; }
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }

    public interface IDrawable3
    {
        bool Visible { get; set; }
        void Draw(GameTime gameTime, GraphicsDevice device);
    }

    public interface ICollidable2
    {
        Rectangle Rectangle { get; set; }
    }

    public interface ICollidable3
    {
        BoundingBox BoundingBox { get; set; }
        BoundingSphere BoundingSphere { get; set; }
    }
}
