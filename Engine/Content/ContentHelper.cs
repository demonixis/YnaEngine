// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Yna.Engine.Content
{
    /// <summary>
    /// Content helper that provide some methods for load non xnb assets like Texture2D, Song, SoundEffect and XML
    /// </summary>
    public class ContentHelper
    {
        /// <summary>
        /// Get the content folder
        /// </summary>
        /// <returns></returns>
        public static string GetContentDirectory()
        {
            return Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + YnG.Content.RootDirectory + Path.DirectorySeparatorChar;
        }

        private static string NormalizePath(string path)
        {
            string [] temp = path.Split(new char[] { '/' });

            string final = String.Empty;
            int size = temp.Length;

            for (int i = 0; i < size; i++)
            {
                final += temp[i];

                if (i < size - 1)
                    final += "\\";
            }

            return final;
        }

        /// <summary>
        /// Load and deserialize an XML file into an object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="assetName">Asset name</param>
        /// <returns>An instance of the object</returns>
        public static T LoadXMLFromXna<T>(string assetName)
        {
            T dataObject = default(T);

            string path = GetContentDirectory() + NormalizePath(assetName) + ".xml";
            
            // Read the xml
            FileStream file = File.Open(path, FileMode.Open);
            StreamReader stream = new StreamReader(file);

            List<string> lines = new List<string>();
    
            while (!stream.EndOfStream)
            {
                lines.Add(stream.ReadLine());
            }

            // Remove unecessary tags relative to XNA
            lines.RemoveAt(lines.Count - 1);
            lines.RemoveAt(1);
            lines[1] = String.Format("<{0}>", typeof(T).Name);
            lines[lines.Count - 1] = String.Format("</{0}>", typeof(T).Name);
            lines.RemoveAt(0);

            // Close streams
            file.Dispose();
            stream.Dispose();

            // Create a string XML
            string finalXml = String.Empty;

            foreach (string line in lines)
                finalXml += line;

            // Try to deserialize it to the object specified
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                dataObject = (T)serializer.Deserialize(new StringReader(finalXml));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            return dataObject;
        }

        /// <summary>
        /// Load and create a sound effect with a wav file
        /// </summary>
        /// <param name="assetName">Asset name</param>
        /// <returns>An instance of Song</returns>
        public static SoundEffect LoadSoundEffect(string assetName)
        {
            SoundEffect sound = null;
#if WINDOWS_PHONE_7 || XNA || MONOGAME 
            using (StreamReader reader = new StreamReader(GetContentDirectory() + assetName))
            {
                sound = SoundEffect.FromStream(reader.BaseStream);
            }
#else
            byte[] buffer;
            string path = GetContentDirectory() + assetName + ".wav";

            if (File.Exists(path))
            {
                using (BinaryReader b = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    int length = (int)b.BaseStream.Length;
                    buffer = new byte[length];
                    buffer = b.ReadBytes(length);

                    sound = new SoundEffect(buffer, 1, AudioChannels.Stereo);
                }
            }
#endif
            return sound;
        }

        /// <summary>
        /// Load and create a texture2D with a texture file
        /// </summary>
        /// <param name="textureName">The image to use</param>
        /// <param name="ext">Extension of the image</param>
        /// <returns>An instance of Texture2D</returns>
        public static Texture2D LoadTexture2D(string textureName, string ext)
        {
            Texture2D texture = null;
            string path = GetContentDirectory() + textureName + "." + ext; // TODO determine extension

            using (StreamReader reader = new StreamReader(path))
            {
                texture = Texture2D.FromStream(YnG.GraphicsDevice, reader.BaseStream);
            }

            return texture;
        }
    }
}
