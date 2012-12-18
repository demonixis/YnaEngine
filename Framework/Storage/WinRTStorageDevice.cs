using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using System.Runtime.Serialization;
using System.IO;
using Windows.System.Threading;

namespace Yna.Framework.Storage
{
    public class WinRTStorageDevice : IStorageDevice
    {
        private object persistedObject;

        public WinRTStorageDevice()
        {
            persistedObject = null;
        }

        private async Task SaveDatas<T>(string containerName, string fileName, T objectToSave)
        {
            var storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(containerName);

            var file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            var fileAccess = await file.OpenAsync(FileAccessMode.ReadWrite);
            IOutputStream stream = fileAccess.GetOutputStreamAt(0);

            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream.AsStreamForWrite(), objectToSave);
            await stream.FlushAsync();
        }

        private async Task LoadDatas<T>(string containerName, string fileName)
        {
            var storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(containerName);

            var file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            if (file == null)
                return;

            IInputStream stream = await file.OpenReadAsync();

            var serializer = new DataContractSerializer(typeof(T));
            this.persistedObject = serializer.ReadObject(stream.AsStreamForRead());
        }

        private async Task Save<T>(string containerName, string fileName, T objectToSave)
        {
            await ThreadPool.RunAsync((sender) => SaveDatas<T>(containerName, fileName, objectToSave).Wait(), WorkItemPriority.Normal);
        }

        private async Task Load<T>(string containerName, string fileName)
        {
            await ThreadPool.RunAsync((sender) => LoadDatas<T>(containerName, fileName).Wait(), WorkItemPriority.Normal);
        }

        object IStorageDevice.GetContainer(string containerName)
        {
            return new object();
        }

        bool IStorageDevice.SaveDatas<T>(string containerName, string fileName, T objectToSave)
        {
            Task<bool>.Factory.StartNew(() =>
            {
                SaveDatas<T>(containerName, fileName, objectToSave);
                return true;
            });

            return true;
        }

        T IStorageDevice.LoadDatas<T>(string containerName, string fileName)
        {
            Task<T>.Factory.StartNew(() => 
            { 
                LoadDatas<T>(containerName, fileName);
                return (T)persistedObject;
            });
            return default(T);
        }
    }
}
