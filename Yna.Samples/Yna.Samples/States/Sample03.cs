using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Display;
using Yna.Input;
using Yna.State;
using Yna.Display.Animation;
using Yna.Display.Event;

namespace Yna.Sample.States
{
    public class Sample03 : YnState
    {
        private YnText informations;

        private YnSprite background;
        private YnSprite sephirothSprite; // Sprite du joueur
        private YnSprite tifaSprite;
        private YnSprite cloudSprite;

        private SpriteMover tifaMover;
        private SpriteMover cloudMover;
        
        private YnTimer clearMessage;

        public Sample03 () 
            : base(1000f, 0) 
        {
            // 0 - The background
            background = new YnSprite("Backgrounds/back-ff6");

            // 1 - Création d'un Sprite à la position 50, 50 en utilisant la texture soniclg4 du dossier 2d
            sephirothSprite = new YnSprite(new Vector2(150, 250), "Sprites/sephiroth");
            Add(sephirothSprite);

            tifaSprite = new YnSprite(new Vector2(350, 150), "Sprites/tifa");
            Add(tifaSprite);

            cloudSprite = new YnSprite(new Vector2(350, 25), "Sprites/cloud");
            Add(cloudSprite);

            // 2 - Informations de debug
            informations = new YnText("Fonts/MenuFont", 25, 25, "Informations");
            informations.Color = Color.Yellow;
            informations.Scale = new Vector2(2.0f, 2.0f);
            Add(informations);

            // 3 - Permet de simuler un déplacement aléatoire sur un Sprite
            tifaMover = new SpriteMover(tifaSprite);
            cloudMover = new SpriteMover(cloudSprite);

            // 4 - Timer pour faire disparaitre les messages d'info
            clearMessage = new YnTimer(1500, 0);
            clearMessage.Completed += new EventHandler<EventArgs>(clearMessage_Completed);
        }

        public override void Initialize ()
		{
            background.LoadContent();
            background.SetRectangles(new Rectangle(0, 0, YnG.Width, YnG.Height));

            // 4 - Préparation et création des animations des personnages
            CreateAnimation(sephirothSprite, 34, 48);
			sephirothSprite.ForceInsideScreen = true;

            CreateAnimation(tifaSprite, 32, 48);
            CreateAnimation(cloudSprite, 32, 48);

            // 5 - Evenements souris
            tifaSprite.MouseOver += new EventHandler<MouseOverSpriteEventArgs>(OnSprite_MouseOver);
            sephirothSprite.MouseClicked += new EventHandler<MouseClickSpriteEventArgs>(OnSprite_MouseClicked);

            // 6 - Scale of the sprites
            Vector2 scale = new Vector2(1.5f, 1.5f);
            tifaSprite.Scale = scale;
            sephirothSprite.Scale = scale;
            cloudSprite.Scale = scale;

            // On affiche la souris
            YnG.Game.IsMouseVisible = true;
		}

        void OnSprite_MouseClicked(object sender, MouseClickSpriteEventArgs e)
        {   
            informations.Text = "You clicked on Sephiroth !";
            clearMessage.Start();
        }

        void OnSprite_MouseOver(object sender, MouseOverSpriteEventArgs e)
        {
            informations.Text = "The mouse is over Tifa's Sprite !";
            clearMessage.Start();
        }

        void clearMessage_Completed(object sender, EventArgs e)
        {
            informations.Text = "Waiting for mouse action";
        }

        private void CreateAnimation(YnSprite sprite, int animWidth, int animHeight)
        {
            sprite.PrepareAnimation(animWidth, animHeight);
            sprite.AddAnimation("down", new int[] { 0, 1, 2, 3 }, 150, false);
            sprite.AddAnimation("left", new int[] { 4, 5, 6, 7 }, 150, false);
            sprite.AddAnimation("right", new int[] { 8, 9, 10, 11 }, 150, false);
            sprite.AddAnimation("up", new int[] { 12, 13, 14, 15 }, 150, false);
        }

        private void UpdateAnimations(YnSprite sprite)
        {
            if (sprite.Direction.X == -1)
                sprite.Play("left");
            else if (sprite.Direction.X == 1)
                sprite.Play("right");
            else if (sprite.Direction.Y == -1)
                sprite.Play("up");
            else if (sprite.Direction.Y == 1)
                sprite.Play("down");
            else
                sprite.Play("down");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Les objets attachés sont mis à jour

            clearMessage.Update(gameTime);

            tifaMover.Update(gameTime);
            cloudMover.Update(gameTime);

            UpdateAnimations(tifaSprite);
            UpdateAnimations(cloudSprite);

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

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background.Texture, background.Rectangle, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
