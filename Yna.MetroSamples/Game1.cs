using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

using Yna;
using Yna.Input;
using Yna.Display;

namespace Yna.MetroSamples
{
    public class Game1 : YnGame
    {
        const int numSprites = 10;
        Sprite background;
        YnGroup sprites;
        Color[] colors; 

        public Game1()
            : base (1600, 900, "Title")
        {
            sprites = new YnGroup(numSprites);
            colors = new Color[numSprites];

            Random rand = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 10; i++)
                colors[i] = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        }

        protected override void Initialize()
        {
            base.Initialize();

            background = new Sprite(Vector2.Zero, "Backgrounds//Sky3");
            background.LoadContent();
            background.Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);

            int spriteWidth = 50;
            int spriteHeight = 50;
            Random rand = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < numSprites; i++)
            {
                float x = rand.Next(0, YnG.Width - spriteWidth);
                float y = rand.Next(0, YnG.Height - spriteHeight);

                Sprite sprite = new Sprite(new Rectangle(0, 0, spriteWidth, spriteHeight), colors[i]);
                sprite.Position = new Vector2(x, y);
                sprite.SetOriginTo(SpriteOrigin.Center);
                sprite.AllowAcrossScreen = true;
                sprites.Add(sprite);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.X += 3;
                sprite.Y += 2;
                sprite.Rotation += 0.1f;
            }

            if (YnG.Keys.JustPressed(Keys.Escape))
            {
                YnG.Exit();
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            foreach (Sprite sprite in sprites)
                sprite.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
