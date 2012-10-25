using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;

namespace Yna.Manager
{
    public class StorageManager
    {
        private StorageDevice _storageDevice;

        public StorageManager()
        {
            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            _storageDevice = StorageDevice.EndShowSelector(result);
        }

        private StorageContainer GetContainer(string name)
        {
            IAsyncResult result = _storageDevice.BeginOpenContainer(name, null, null);
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = _storageDevice.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();

            return container;
        }
    }
}
