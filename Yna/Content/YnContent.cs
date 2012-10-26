using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Content
{
    public class YnContent : IDisposable
    {
        #region Privates declarations

        protected Dictionary<string, object> _loadedAssets;
        protected List<IDisposable> _disposableAssets;
        protected string _contentDirectory;
        protected string _gameDirectory;

        #endregion

        #region Properties

        protected GraphicsDevice GraphicsDevice
        {
            get { return YnG.GraphicsDevice; }
        }

        /// <summary>
        /// Get the game directory
        /// </summary>
        public string GameDirectory
        {
            get { return _gameDirectory; }
        }

        /// <summary>
        /// Get or set the custom content directory relative to the game directory.
        /// If set, the directory structure is checked and created if it's necessary.
        /// </summary>
        public string ContentDirectory
        {
            get { return _contentDirectory; }
            set 
            {
                if (value != String.Empty)
                {
                    _contentDirectory = value;
                    CheckDirectoryStructure();
                }
            }
        }

        #endregion

        /// <summary>
        /// Create a custom content manager who allow you to don't use XNA's ContentManager.
        /// You can load Texture2D, Song and SoundEffect
        /// </summary>
        /// <param name="folder"></param>
        public YnContent(string folder = "Datas")
        {
            _loadedAssets = new Dictionary<string, object>();
            _disposableAssets = new List<IDisposable>();

            _contentDirectory = folder;
            
            CheckDirectoryStructure();
        }

        protected void CheckDirectoryStructure()
        {
            _gameDirectory = Directory.GetCurrentDirectory();

            if (!Directory.Exists(Path.Combine(_gameDirectory, _contentDirectory)))
                Directory.CreateDirectory(Path.Combine(_gameDirectory, _contentDirectory));

        }

        /// <summary>
        /// Get the name of the asset
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected string ExtractNameFromPath(string path)
        {
            string temp = path.Replace("\\\\", "\\");
            string[] tArray = temp.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

            return tArray[temp.Length - 1];
        }

        protected virtual string GetAssetPath(string assetName)
        {
#if !NETFX_CORE
            return System.IO.Path.Combine(_contentDirectory, assetName);
#else
            return String.Empty;
#endif
        }

        /// <summary>
        /// Load an asset from the custom content folder. Allowed assets are Texture2D, Song & SoundEffect 
        /// Assets are stored in an internal cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="assetPath">Define if the path is absolute or from custom content</param>
        /// <returns>Loaded asset</returns>
        public T Load<T>(string assetName, bool relativePath = true)
        {
            if (typeof(T) != typeof(Texture2D) || typeof(T) != typeof(Song) || typeof(T) != typeof(SoundEffect))
                return default(T);

            string name = assetName;
            string path = GetAssetPath(assetName);

            if (relativePath)
            {
                name = ExtractNameFromPath(assetName);
                path = assetName;
            }

            if (!_loadedAssets.ContainsKey(name))
            {
                if (typeof(T) == typeof(Texture2D))
                {
                    Texture2D texture = Texture2D.FromStream(GraphicsDevice, new StreamReader(path).BaseStream);
                    texture.Name = name;
                    _loadedAssets.Add(name, texture);
                    _disposableAssets.Add(texture);
                }

                else if (typeof(T) == typeof(Song))
                {
                    Song song = Song.FromUri(name, new Uri(path));
                    _loadedAssets.Add(name, song);
                    _disposableAssets.Add(song);
                }

                else if (typeof(T) == typeof(SoundEffect))
                {
                    SoundEffect sound = SoundEffect.FromStream(new StreamReader(path).BaseStream);
                    _loadedAssets.Add(name, sound);
                    _disposableAssets.Add(sound);
                }
            }

            return (T)_loadedAssets[name];
        }

        /// <summary>
        /// Unload all assets stored in the custom content
        /// </summary>
        public void Unload()
        {
            foreach (IDisposable asset in _disposableAssets)
            {
                if (asset != null)
                    asset.Dispose();
            }
        }

        void IDisposable.Dispose()
        {
            Unload();
        }
    }
}
