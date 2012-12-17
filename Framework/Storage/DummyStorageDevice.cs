using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Framework.Storage
{
    /// <summary>
    /// A dummy storage device who do nothing
    /// </summary>
    public class DummyStorageDevice : IStorageDevice
    {
        public DummyStorageDevice()
        {

        }

        object IStorageDevice.GetContainer(string containerName)
        {
            return new object();
        }

        bool IStorageDevice.SaveDatas<T>(string containerName, string fileName, T objectToSave)
        {
            return false;
        }

        T IStorageDevice.LoadDatas<T>(string containerName, string fileName)
        {
            return default(T);
        }
    }
}
