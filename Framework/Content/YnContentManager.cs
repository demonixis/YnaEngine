using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework;
using Yna.Framework.Helpers;

namespace Yna.Framework.Content
{
    /// <summary>
    /// Yna extented content manager who work with asset that is not in xnb format.
    /// </summary>
    public class YnContentManager
    {
        // Define paths
        private string _rootDirectory;
        private string _gameDirectory;
        private string _fullPath;
        
        /// <summary>
        /// Collection of assets
        /// </summary>
        protected Dictionary<string, object> _loadedAssets;
        
        /// <summary>
        /// Collection of disposables assets
        /// </summary>
        protected List<IDisposable> _disposableAssets;

        public string RootDirectory
        {
            get { return _rootDirectory; }
            set { _rootDirectory = value; }
        }

        /// <summary>
        /// Create a new content manager
        /// </summary>
        public YnContentManager()
        {
            _loadedAssets = new Dictionary<string, object>();
            _disposableAssets = new List<IDisposable>();
            _rootDirectory = "Content";
            _gameDirectory = Directory.GetCurrentDirectory();
            _fullPath = Path.Combine(_gameDirectory, _rootDirectory);
        }

        /// <summary>
        /// Gets the full path of the asset
        /// </summary>
        /// <param name="assetName">The asset name</param>
        /// <returns>The path of the asset</returns>
        protected virtual string GetAssetPath(string assetName)
        {
            return System.IO.Path.Combine(_fullPath, assetName);
        }

        public T LoadContent<T>(string assetName)
        {
            // The asset already exists and loaded
            if (_loadedAssets.ContainsKey(assetName))
            {
                return (T) _loadedAssets[assetName];
            }
            else // Try to load the new asset
            {
                object objectAsset = null;

                FileInfo fileInfo = new FileInfo(GetAssetPath(assetName));

                if (fileInfo.Exists)
                {
                    if (typeof(T) == typeof(Texture2D))         // Texture2D
                    {
                        objectAsset = ContentHelper.LoadTexture2D(GetAssetPath(assetName), fileInfo.Extension);
                    }
                    else if (typeof(T) == typeof(SoundEffect)) // Sound
                    {
                        objectAsset = ContentHelper.LoadSoundEffect(GetAssetPath(assetName));
                    }
                    else if (fileInfo.Extension == "xml")
                    {
                        // Try to deserialize an object
                        try
                        {
                            objectAsset = ContentHelper.LoadXMLFromXna<T>(assetName);
                        }
                        catch (Exception ex)
                        {
                            DebugHelper.Error(ex.Message);
                        }
                    }

                    // If the asset is properly loaded
                    if (objectAsset != null)
                    {
                        if (objectAsset is IDisposable)
                            _disposableAssets.Add((IDisposable)objectAsset);

                        _loadedAssets.Add(assetName, objectAsset);
                    }
                    else
                    {
                        DebugHelper.Trace("The load was aborted");
                    }
                }

                return (T)objectAsset;
            }
        }

        /// <summary>
        /// Unload all assets from the content manager
        /// </summary>
        public void Unload()
        {
            _loadedAssets.Clear();
        }

        /// <summary>
        /// Unload and dispose all assets from the content
        /// </summary>
        public void Dispose()
        {
            foreach (IDisposable asset in _disposableAssets)
            {
                if (asset != null)
                    asset.Dispose();
            }

            _disposableAssets.Clear();
            _loadedAssets.Clear();
        }
    }
}
