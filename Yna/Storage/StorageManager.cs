using System;
using System.Collections.Generic;
#if !NETFX_CORE
using System.IO;
#endif
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
#if !WP7
using Microsoft.Xna.Framework.Storage;
#endif

namespace Yna.Manager
{
    public class StorageManager
    {
#if !NETFX_CORE && !WINDOWS_PHONE
        private StorageDevice _storageDevice;

        public StorageManager()
        {
            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            _storageDevice = StorageDevice.EndShowSelector(result);
        }

        /// <summary>
        /// Save a serializable object in the player's storage
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="containerName">Folder in the player's storage. If the folder doesn't exist, it's created</param>
        /// <param name="fileName">The filename</param>
        /// <param name="obj">Serializable object</param>
        public virtual void SaveData<T>(string containerName, string fileName, T obj)
        {
            StorageContainer container = GetContainer(containerName);

            if (container.FileExists(fileName))
                container.DeleteFile(fileName); // TODO : backup file

            Stream stream = container.CreateFile(fileName);

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            serializer.Serialize(stream, obj);

            stream.Close();

            container.Dispose();
        }

        public virtual T LoadData<T>(string containerName, string fileName)
        {
            T datas = default(T);

            StorageContainer container = GetContainer(containerName);

            if (container.FileExists(fileName))
            {
                Stream stream = container.OpenFile(fileName, FileMode.Open);

                XmlSerializer serializer = new XmlSerializer(typeof(T));

                datas = (T)serializer.Deserialize(stream);

                stream.Close();
            }

            container.Dispose();

            return datas;
        }

        protected virtual StorageContainer GetContainer(string name)
        {
            IAsyncResult result = _storageDevice.BeginOpenContainer(name, null, null);
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = _storageDevice.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();

            return container;
        }
#endif
    }
}
