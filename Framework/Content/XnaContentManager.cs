using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Yna.Framework.Content
{
    /// <summary>
    /// Content manager using XNA native content manager
    /// </summary>
    public class XnaContentManager : IContentManager
    {
        private ContentManager _contentManager;

        /// <summary>
        /// Gets or sets the content folder name
        /// </summary>
        public string RootDirectory
        {
            get { return _contentManager.RootDirectory; }
            set { _contentManager.RootDirectory = value; }
        }

        /// <summary>
        /// Create a new Content manager based on XNA's ContentManager
        /// </summary>
        /// <param name="game"></param>
        public XnaContentManager(Game game)
        {
            _contentManager = game.Content;
        }

        public T LoadContent<T>(string assetName)
        {
            return _contentManager.Load<T>(assetName);
        }

        public void Unload()
        {
            _contentManager.Unload();
        }

        public void Dispose()
        {
            _contentManager.Dispose();
        }
    }
}
