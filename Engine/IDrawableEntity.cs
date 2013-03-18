using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine
{
    /// <summary>
    /// An interface for 2D Entity
    /// </summary>
    public interface IDrawableEntity
    {
        /// <summary>
        /// Initialize logic.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Load content.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Unload content.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Draw entity
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        /// <param name="spriteBatch">SpriteBatch object</param>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
