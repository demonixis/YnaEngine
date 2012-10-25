using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Content
{
    public class YnContent
    {
        private Dictionary<string, Texture2D> _textures2D;
        private Dictionary<string, Song> _musics;
        private Dictionary<string, SoundEffect> _sounds;

        public YnContent()
        {
            _textures2D = new Dictionary<string, Texture2D>();
            _musics = new Dictionary<string, Song>();
            _sounds = new Dictionary<string, SoundEffect>();
        }
    }
}
