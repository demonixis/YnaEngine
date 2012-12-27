using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;


namespace Yna.Framework.Storage
{
    /// <summary>
    /// The storage manager is an object that can be used for store and load information like score, achievments, etc..
    /// </summary>
    public class StorageManager
    {
        private IStorageDevice storageDevice;

        public StorageManager()
        {
#if XNA
            storageDevice = new XnaStorageDevice();
#elif WINDOWS_PHONE_7
            storageDevice = new XnaPhoneStorageDevice();
#elif MONOGAME && WINDOWS || LINUX || MACOSX || XNA
            storageDevice = new BasicStorageDevice();
#elif NETFX_CORE
            storageDevice = new WinRTStorageDevice();
#else
            storageDevice = new DummyStorageDevice();
#endif
        }

        /// <summary>
        /// Save a serializable object in the user's local storage
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="containerName">Folder in the user's storage. If the folder doesn't exist, it's created</param>
        /// <param name="fileName">The file's name</param>
        /// <param name="obj">Serializable object</param>
        public virtual void SaveDatas<T>(string containerName, string fileName, T obj)
        {
            storageDevice.SaveDatas<T>(containerName, fileName, obj);
        }

        /// <summary>
        /// Load a serialized object from the user's local storage
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="containerName">Folder in the user's storage.</param>
        /// <param name="fileName">The file's name</param>
        /// <returns>Instance of the object type with previous saved datas</returns>
        public virtual T LoadDatas<T>(string containerName, string fileName)
        {
            return storageDevice.LoadDatas<T>(containerName, fileName);
        }
    }
}
