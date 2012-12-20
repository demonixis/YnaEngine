using System;
#if !NETFX_CORE && !WINDOWS_PHONE
using System.IO;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Helpers
{
    public class ContentHelper
    {

        public static string GetContentDirectory()
        {
            return Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + YnG.Content.RootDirectory + Path.DirectorySeparatorChar;
        }

        public static T LoadXMLFromXna<T>(string assetName)
        {
            string path = GetContentDirectory() + assetName + ".xml";

            FileStream file = File.Open(path, FileMode.Open);

            StreamReader stream = new StreamReader(file);
            List<string> lines = new List<string>();
    
            while (!stream.EndOfStream)
            {
                lines.Add(stream.ReadLine());
            }

            lines.RemoveAt(lines.Count - 1);
            lines.RemoveAt(1);
            lines[1] = String.Format("<{0}>", typeof(T).Name);
            lines[lines.Count - 1] = String.Format("</{0}>", typeof(T).Name);
            lines.RemoveAt(0);

            string finalXml = "";

            foreach (string line in lines)
                finalXml += line;

            MemoryStream mem = new MemoryStream();
            StreamWriter writer = new StreamWriter(mem);
            writer.Write(finalXml);
            writer.Flush();

            return default(T);
        }

        public static SoundEffect LoadSoundEffect(string assetName)
        {
            SoundEffect sound = null;
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

            return sound;
        }

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

        protected string ExtractNameFromPath(string path)
        {
            string temp = path.Replace("\\\\", "\\");
            string[] tArray = temp.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

            return tArray[temp.Length - 1];
        }
    }
}
