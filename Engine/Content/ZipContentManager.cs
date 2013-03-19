using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Engine.Content
{
    class ZipContentManager : IContentManager
    {
        public T LoadContent<T>(string assetName)
        {
            throw new NotImplementedException();
        }

        public Microsoft.Xna.Framework.Graphics.Texture2D LoadTexture2D(string assetName)
        {
            throw new NotImplementedException();
        }

        public Microsoft.Xna.Framework.Audio.SoundEffect LoadSoundEffect(string assetName)
        {
            throw new NotImplementedException();
        }

        public T LoadFromXML<T>(string assetName, bool isXnaXml)
        {
            throw new NotImplementedException();
        }
    }
}
