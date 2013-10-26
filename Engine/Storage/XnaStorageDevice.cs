using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;

namespace Yna.Engine.Storage
{
    /// <summary>
    /// XNA storage device that use native XNA methods for save and load datas.
    /// </summary>
    public class XnaStorageDevice : IStorageDevice
    {
        private StorageDevice _storageDevice;

        public XnaStorageDevice()
        {
            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            _storageDevice = StorageDevice.EndShowSelector(result);
        }

        private StorageContainer GetContainer(string containerName)
        {
            return (StorageContainer)(this as IStorageDevice).GetContainer(containerName);
        }

        object IStorageDevice.GetContainer(string containerName)
        {
            IAsyncResult result = _storageDevice.BeginOpenContainer(containerName, null, null);
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = _storageDevice.EndOpenContainer(result);
            result.AsyncWaitHandle.Dispose();

            return container;
        }

        bool IStorageDevice.Save<T>(string containerName, string fileName, T objectToSave)
        {
            StorageContainer container = GetContainer(containerName);

            if (container.FileExists(fileName))
                container.DeleteFile(fileName); // TODO : backup file

            Stream stream = container.CreateFile(fileName);

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            serializer.Serialize(stream, objectToSave);

            stream.Dispose();

            container.Dispose();

            return true;
        }

        T IStorageDevice.Load<T>(string containerName, string fileName)
        {
            T datas = default(T);

            StorageContainer container = GetContainer(containerName);

            if (container.FileExists(fileName))
            {
                Stream stream = container.OpenFile(fileName, FileMode.Open);

                XmlSerializer serializer = new XmlSerializer(typeof(T));

                datas = (T)serializer.Deserialize(stream);

                stream.Dispose();
            }

            container.Dispose();

            return datas;
        }
		
		void IStorageDevice.Clear()
        {

        }
    }
}
