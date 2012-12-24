using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Framework.Content
{
    /// <summary>
    /// Interface for a content manager
    /// </summary>
    interface IContentManager
    {
        /// <summary>
        /// Gets or sets the default asset directory
        /// </summary>
        string RootDirectory { get; set; }

        /// <summary>
        /// Load an asset from the content manager
        /// </summary>
        /// <typeparam name="T">Type of object you wan't to load</typeparam>
        /// <param name="assetName">The name of the asset</param>
        /// <returns>An instance of an object of T</returns>
        T LoadContent<T>(string assetName);

        /// <summary>
        /// Unload all assets from the content manager
        /// </summary>
        void Unload();

        /// <summary>
        /// Unload and dispose all assets from the content
        /// </summary>
        void Dispose();
    }
}
