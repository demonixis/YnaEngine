using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics
{
    public interface IEntity2D
    {
        bool Enabled { get; set; }
        bool Visible { get; set; }
        bool Active { get; set; }

        void Initialize();
        void LoadContent();
        void Update(GameTime gameTime);
        void UnloadContent();
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
