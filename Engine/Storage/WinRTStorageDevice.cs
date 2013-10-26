using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.Serialization;
using System.IO;

namespace Yna.Engine.Storage
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
            var storageFolder = ApplicationData.Current.LocalFolder;
 
            var file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            var fileAccess = await file.OpenAsync(FileAccessMode.ReadWrite);
            IOutputStream stream = fileAccess.GetOutputStreamAt(0);

            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream.AsStreamForWrite(), objectToSave);
            await stream.FlushAsync();
        }

        private async Task LoadDatas<T>(string containerName, string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;

            var file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            if (file == null)
                return;

            IInputStream stream = await file.OpenReadAsync();

            var serializer = new DataContractSerializer(typeof(T));
            this.persistedObject = serializer.ReadObject(stream.AsStreamForRead());
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
            return Task<T>.Factory.StartNew(() => 
            { 
                LoadDatas<T>(containerName, fileName);
                return (T)persistedObject;
            }).Result;
            //return default(T);
        }
		
		void IStorageDevice.Clear()
        {
            
        }
    }
}
