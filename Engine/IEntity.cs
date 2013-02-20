using Microsoft.Xna.Framework;

namespace Yna.Engine
{
    /// <summary>
    /// An interface for a basic entity.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Initialize logic.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Load assets.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Update logic.
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);
    }
}
