using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Display;
using Yna.Input;
using Yna.State;
using Yna.Display.Animation;

namespace Yna.Sample.States
{
    public class Sample03 : YnState
    {
        YnText infos;
        Texture2D gradiant;
        Sprite sephirothSprite; // Sprite du joueur
        bool clicked = false;
        bool arrived = false;
        Vector2 path = Vector2.Zero;
        Vector2 dest = Vector2.Zero;
        
        Sprite tifaSprite;

        SpriteMover mover;

        public Sample03 () 
            : base(1000f, 0) 
        { }

        public override void Initialize ()
		{        
			// 1 - Création d'un Sprite à la position 50, 50 en utilisant la texture soniclg4 du dossier 2d
			sephirothSprite = new Sprite (new Vector2 (50, 48), "Sprites//sephiroth");
            sephirothSprite.LoadContent();
            CreateAnimation(sephirothSprite, 34, 48);
			sephirothSprite.ForceInsideScreen = true;
            sephirothSprite.SetOriginTo(SpriteOrigin.Center);
			Add (sephirothSprite);

            tifaSprite = new Sprite(new Vector2(350, 150), "Sprites//tifa");
            tifaSprite.LoadContent();

            CreateAnimation(tifaSprite, 32, 48);
            Add(tifaSprite);

            mover = new SpriteMover(tifaSprite);

            gradiant = YnG.Content.Load<Texture2D>("Backgrounds//gradient");

            infos = new YnText("Fonts//MenuFont", new Vector2(10, 10), String.Format(" X: {0} | Y: {1}", YnG.Mouse.X, YnG.Mouse.Y));
            infos.Initialize();
            Add(infos);
          
            YnG.Game.IsMouseVisible = true;
		}

        private void UpdateInfosText(string text = "")
        {
            if (text == "")
                infos.Text = String.Format(" X: {0} | Y: {1}", YnG.Mouse.X, YnG.Mouse.Y);
            else
                infos.Text = text;
        }

        private void CreateAnimation(Sprite sprite, int animWidth, int animHeight)
        {
            sprite.PrepareAnimation(animWidth, animHeight);
            sprite.AddAnimation("down", new int[] { 0, 1, 2, 3 }, 150, false);
            sprite.AddAnimation("left", new int[] { 4, 5, 6, 7 }, 150, false);
            sprite.AddAnimation("right", new int[] { 8, 9, 10, 11 }, 150, false);
            sprite.AddAnimation("up", new int[] { 12, 13, 14, 15 }, 150, false);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Les objets attachés sont mis à jour

            mover.Update(gameTime);

            if (tifaSprite.Direction.X == -1)
                tifaSprite.Play("left");
            else if (tifaSprite.Direction.X == 1)
                tifaSprite.Play("right");
            else if (tifaSprite.Direction.Y == -1)
                tifaSprite.Play("up");
            else if (tifaSprite.Direction.Y == 1)
                tifaSprite.Play("down");
            else
                tifaSprite.Play("down");

			// Déplacement du Sprite
            if (YnG.Keys.Pressed(Keys.Up))
            {
                sephirothSprite.Play("up");
                sephirothSprite.Y -= 2;
            }
            else if (YnG.Keys.Pressed(Keys.Down))
            {
                sephirothSprite.Play("down");
                sephirothSprite.Y += 2;
            }
            else if (YnG.Keys.Pressed(Keys.Left))
            {
                sephirothSprite.Play("left");
                sephirothSprite.X -= 2;
            }
            else if (YnG.Keys.Pressed(Keys.Right))
            {
                sephirothSprite.Play("right");
                sephirothSprite.X += 2;
            }

            if (YnG.Mouse.Clicked(MouseButton.Left))
            {
                Vector2 mousePosition = new Vector2(YnG.Mouse.X, YnG.Mouse.Y);
                
                Vector2 vDirection = Vector2.Subtract(mousePosition, sephirothSprite.Position);
                float angle = (float)Math.Atan2(vDirection.Y, vDirection.X);
                //vDirection.Normalize();
                UpdateInfosText(String.Format("Angle: {0} | vDirection: {1}", angle * 180, vDirection.ToString()));
                dest = mousePosition;
                path = vDirection;
                path.Normalize();
            }

            if (dest != Vector2.Zero)
            {
                if (sephirothSprite.Position == dest)
                {
                    dest = Vector2.Zero;
                    path = Vector2.One;
                }
                else
                {
                    sephirothSprite.X += (int)(2 * path.X);
                    sephirothSprite.Y += (int)(2 * path.Y);
                }
            }

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());
        }

        public override void Draw (GameTime gameTime)
		{
            spriteBatch.Begin();
            spriteBatch.Draw(gradiant, new Rectangle(0, 0, YnG.Width, YnG.Height), Color.White);
            spriteBatch.End();

            base.Draw(gameTime); // Les objets attachés sont dessinés
        }
    }
}
