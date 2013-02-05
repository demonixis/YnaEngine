using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;


namespace Yna.Engine.Storage
{
    /// <summary>
    /// The storage manager is an object that can be used for store and load information like scores, achievments, etc..
    /// </summary>
    public class StorageManager
    {
        private IStorageDevice _storageDevice;

        public StorageManager()
        {
#if WINDOWS_PHONE_7
            _storageDevice = new XnaPhoneStorageDevice();
#elif MONOGAME && WINDOWS || LINUX || MACOSX || XNA
            _storageDevice = new BasicStorageDevice();
#elif WINDOWS_STOREAPP
            _storageDevice = new WinRTStorageDevice();
#else
            _storageDevice = new DummyStorageDevice();
#endif
        }

        public StorageManager(IStorageDevice storageDevice)
        {
            _storageDevice = storageDevice;
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
            _storageDevice.SaveDatas<T>(containerName, fileName, obj);
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
            return _storageDevice.LoadDatas<T>(containerName, fileName);
        }
    }
}
