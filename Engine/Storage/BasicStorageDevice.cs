using System;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Yna.Engine.Storage
{
    /// <summary>
    /// A basic storage device adapter who use default document folder to save and load datas
    /// It use basic .Net methods (no XNA)
    /// </summary>
    public class BasicStorageDevice : IStorageDevice
    {
        private string _saveFolder;

        public BasicStorageDevice()
        {
            _saveFolder = GetSaveContainer();
        }

        private string GetSaveContainer()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            builder.Append(Path.DirectorySeparatorChar);
            builder.Append("my games");
            builder.Append(Path.DirectorySeparatorChar);
            builder.Append(YnGame.GameTitle);
            builder.Append(Path.DirectorySeparatorChar);
            return builder.ToString();
        }

        private string GetContainer(string containerName)
        {
            _saveFolder = GetSaveContainer();
            return (string)(this as IStorageDevice).GetContainer(containerName);
        }

        private string GetFilePath(string container, string containerName, string fileName)
        {
            StringBuilder pathBuilder = new StringBuilder();
            pathBuilder.Append(container);

            if (containerName != String.Empty)    
                pathBuilder.Append(Path.DirectorySeparatorChar);
            
            pathBuilder.Append(fileName);

            return pathBuilder.ToString();
        }

        object IStorageDevice.GetContainer(string containerName)
        {
            string containerTarget = _saveFolder + containerName; 

            if (!Directory.Exists(containerTarget))
                Directory.CreateDirectory(containerTarget);
            
            return containerTarget;
        }

        bool IStorageDevice.SaveDatas<T>(string containerName, string fileName, T objectToSave)
        {
            string container = GetContainer(containerName);
            string filePath = GetFilePath(container, containerName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath); // TODO : backup file

            StreamWriter writer = new StreamWriter(filePath);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            serializer.Serialize(writer, objectToSave);
            writer.Dispose();

            return true;
        }

        T IStorageDevice.LoadDatas<T>(string containerName, string fileName)
        {
            T datas = default(T);

            string container = GetContainer(containerName);
            string filePath = GetFilePath(container, containerName, fileName);

            if (File.Exists(filePath))
            {
                Stream stream = File.Open(filePath, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                datas = (T)serializer.Deserialize(stream);
                stream.Dispose();
            }

            return datas;
        }
    }
}
