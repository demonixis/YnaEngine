// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
namespace Yna.Engine.Storage
{
    /// <summary>
    /// The storage manager is an object that can be used for store and load informations like scores, achievments, etc..
    /// </summary>
    public class StorageManager
    {
        private IStorageDevice _storageDevice;

        public StorageManager()
        {
#if WINDOWS_PHONE_7
            _storageDevice = new XnaPhoneStorageDevice();
#elif WINDOWS_STOREAPP || WINDOWS_PHONE_8
            _storageDevice = new XnaStorageDevice();
#elif UNSUPPORTED
			_storageDevice = new DummyStorageDevice();
#else
            _storageDevice = new BasicStorageDevice();
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
        public virtual void Save<T>(string containerName, string fileName, T obj)
        {
            _storageDevice.Save<T>(containerName, fileName, obj);
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
            return _storageDevice.Load<T>(containerName, fileName);
        }

        public virtual void Clear()
        {
            _storageDevice.Clear();
        }
    }
}
