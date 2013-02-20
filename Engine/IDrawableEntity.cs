using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine
{
    /// <summary>
    /// An interface for 2D Entity
    /// </summary>
    public interface IDrawableEntity : IEntity
    {
        /// <summary>
        /// Draw entity
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        /// <param name="spriteBatch">SpriteBatch object</param>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
