using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Serialization;

namespace Yna.Engine.Storage
{
    /// <summary>
    /// Xna Phone Storage Device is a device adaptor for Windows Phone 7 who use isolated storage api to store and load datas.
    /// </summary>
    public class XnaPhoneStorageDevice : IStorageDevice
    {
        public XnaPhoneStorageDevice()
        {

        }

        object IStorageDevice.GetContainer(string containerName)
        {
            return null;
        }

        bool IStorageDevice.Save<T>(string containerName, string fileName, T objectToSave)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            if (storage.FileExists(fileName))
            {
                storage.DeleteFile(fileName);
            }

            IsolatedStorageFileStream stream = storage.OpenFile(fileName, FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(stream);

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer.BaseStream, objectToSave);

            writer.Flush();
            writer.Close();
            stream.Close();

            return true;
        }

        T IStorageDevice.Load<T>(string containerName, string fileName)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            if (storage.FileExists(fileName))
            {
                IsolatedStorageFileStream file = storage.OpenFile(fileName, FileMode.Open);

                BinaryReader reader = new BinaryReader(file);
                
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                T datas = (T)serializer.Deserialize(reader.BaseStream);

                reader.Close();
                file.Close();

                return datas;
            }

            return default(T);
        }

        void IStorageDevice.Clear()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            storage.Remove();
        }
    }
}
