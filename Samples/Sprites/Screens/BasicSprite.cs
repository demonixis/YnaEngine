using System;
using System.Net;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Yna.Engine;
using Yna.Engine.Graphics;

namespace Yna.Samples.Screens
{
    public class BasicSprites : YnState2D
    {
        private const int numSprites = 10;

        private YnSprite background;
        private YnGroup sprites;
        private Color[] colors;
        private Song music;

        public BasicSprites(string name)
            : base(name)
        {
            background = new YnSprite(new Rectangle(0, 0, YnG.Width, YnG.Height), Color.AliceBlue);
            Add(background);

            sprites = new YnGroup(numSprites);
            Add(sprites);

            colors = new Color[numSprites];

            Texture2D texture = GetTextureFromURL(YnG.GraphicsDevice, 200, 523, @"http://residentevil.neoseeker.com/w/i/residentevil/thumb/b/b8/LeonS.Kennedy.jpg/200px-LeonS.Kennedy.jpg");

            Random rand = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 10; i++)
                colors[i] = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        }

        private Texture2D GetTextureFromURL(GraphicsDevice device, int width, int height, string url)
        {
            Texture2D texture = new Texture2D(YnG.GraphicsDevice, width, height);

            var stream = 
            

            WebClient webClient = new WebClient();
            var image = webClient.DownloadData(url);
            byte[] tmp = new byte[image.Length];
            image.CopyTo(tmp, 0);
            webClient.Dispose();
            texture.SetData<byte>(tmp);
            
            return texture;
        }


        public override void Initialize()
        {
            base.Initialize();

            //background.Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);

            int spriteWidth = 50;
            int spriteHeight = 50;

            Random rand = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < numSprites; i++)
            {
                float x = rand.Next(0, YnG.Width - spriteWidth);
                float y = rand.Next(0, YnG.Height - spriteHeight);

                YnSprite sprite = new YnSprite(new Rectangle(0, 0, spriteWidth, spriteHeight), colors[i]);
                sprite.Position = new Vector2(x, y);
                sprite.SetOriginTo(ObjectOrigin.Center);
                sprite.AcrossScreen = true;
                sprites.Add(sprite);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (YnSprite sprite in sprites)
            {
                sprite.X += 3;
                sprite.Y += 2;
                sprite.Rotation += 0.1f;
            }

            if (YnG.Keys.JustPressed(Keys.Escape))
            {
                YnG.StateManager.SetStateActive("menu", true);
            }
        }
    }
}
