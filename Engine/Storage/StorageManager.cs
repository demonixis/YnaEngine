// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Yna.Engine.Storage
{
    /// <summary>
    /// The storage manager is an object that can be used for store and load informations like scores, achievments, etc..
    /// </summary>
    public class StorageManager : GameComponent
    {
        private string _saveFolder;

        public StorageManager(Game game)
            : base(game)
        {
            game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            _saveFolder = GetSaveContainer();
        }

        private string GetSaveContainer()
        {
            var builder = new StringBuilder();
            builder.Append(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            builder.Append(Path.DirectorySeparatorChar);
            builder.Append("My Games");
            builder.Append(Path.DirectorySeparatorChar);
            builder.Append(YnGame.GameTitle);
            builder.Append(Path.DirectorySeparatorChar);
            return builder.ToString();
        }

        private string GetContainer(string containerName)
        {
            var containerTarget = _saveFolder + containerName;

            if (!Directory.Exists(containerTarget))
                Directory.CreateDirectory(containerTarget);

            return containerTarget;
        }

        /// <summary>
        /// Save a serializable object in the user's local storage
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="containerName">Folder in the user's storage. If the folder doesn't exist, it's created</param>
        /// <param name="fileName">The file's name</param>
        /// <param name="obj">Serializable object</param>
        public virtual void Save<T>(string containerName, string fileName, T obj)
        {
            var container = GetContainer(containerName);
            var filePath = GetFilePath(container, containerName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (var writer = new StreamWriter(filePath))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
            }
        }

        /// <summary>
        /// Load a serialized object from the user's local storage
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="containerName">Folder in the user's storage.</param>
        /// <param name="fileName">The file's name</param>
        /// <returns>Instance of the object type with previous saved datas</returns>
        public virtual T Load<T>(string containerName, string fileName)
        {
            T data = default(T);

            var container = GetContainer(containerName);
            var filePath = GetFilePath(container, containerName, fileName);

            if (!File.Exists(filePath))
                return data;

            using (var stream = File.Open(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(T));
                data = (T)serializer.Deserialize(stream);
            }

            return data;
        }

        private string GetFilePath(string container, string containerName, string fileName)
        {
            var pathBuilder = new StringBuilder();
            pathBuilder.Append(container);

            if (containerName != String.Empty)
                pathBuilder.Append(Path.DirectorySeparatorChar);

            pathBuilder.Append(fileName);

            return pathBuilder.ToString();
        }
    }
}
