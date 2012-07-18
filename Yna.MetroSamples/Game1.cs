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
        private Sprite sprite;

        public Game1() : base(1024, 768, "YNA Framework : Metro Gaming", false)
        {
            
        }

        protected override void Initialize()
        {
            sprite = new Sprite(new Vector2(50, 50), "mech_final");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            sprite.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            sprite.UnloadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            float speed = 2.5f;

            if (YnG.Keys.Up)
                sprite.VelocityY -= speed;
            else if (YnG.Keys.Down)
                sprite.VelocityY += speed;

            if (YnG.Keys.Left)
                sprite.VelocityX -= speed;
            else if (YnG.Keys.Right)
                sprite.VelocityX += speed;

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            sprite.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
