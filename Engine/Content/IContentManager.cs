using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Content
{
    public interface IContentManager
    {
        T LoadContent<T>(string assetName);
        Texture2D LoadTexture2D(string assetName);
        SoundEffect LoadSoundEffect(string assetName);
        T LoadFromXML<T>(string assetName, bool isXnaXml);
    }
}
