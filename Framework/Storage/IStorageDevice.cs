﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Framework.Storage
{
    interface IStorageDevice
    {
        /// <summary>
        /// Get a container that contains files
        /// </summary>
        /// <param name="containerName">Container's name</param>
        /// <returns>Container object (various type)</returns>
        object GetContainer(string containerName);

        /// <summary>
        /// Save an object in the local storage of the user
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <param name="objectToSave"></param>
        /// <returns></returns>
        bool SaveDatas<T>(string containerName, string fileName, T objectToSave);

        /// <summary>
        /// Load an object from a serialized object stored in the local storage of the user
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        T LoadDatas<T>(string containerName, string fileName);
    }
}
