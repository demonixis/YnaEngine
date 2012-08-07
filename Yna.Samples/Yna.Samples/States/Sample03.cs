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
        private YnText informations;

        private Texture2D gradiant;
        private Sprite sephirothSprite; // Sprite du joueur
        private Sprite tifaSprite;

        private SpriteMover mover;
        private YnTimer clearMessage;

        public Sample03 () 
            : base(1000f, 0) 
        {
            // 1 - Création d'un Sprite à la position 50, 50 en utilisant la texture soniclg4 du dossier 2d
            sephirothSprite = new Sprite(new Vector2(150, 250), "Sprites//sephiroth");
            Add(sephirothSprite);

            tifaSprite = new Sprite(new Vector2(350, 150), "Sprites//tifa");
            Add(tifaSprite);

            // 2 - Informations de debug
            informations = new YnText("Fonts/MenuFont", 25, 25, "Informations");
            informations.Color = Color.Yellow;
            informations.Scale = new Vector2(2.0f, 2.0f);
            Add(informations);

            // 3 - Permet de simuler un déplacement aléatoire sur un Sprite
            mover = new SpriteMover(tifaSprite);

            // 4 - Timer pour faire disparaitre les messages d'info
            clearMessage = new YnTimer(3500, 0);
            clearMessage.Completed += new EventHandler<EventArgs>(clearMessage_Completed);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            gradiant = YnG.Content.Load<Texture2D>("Backgrounds//gradient");
        }

        public override void Initialize ()
		{        
            // 4 - Préparation et création des animations des personnages
            CreateAnimation(sephirothSprite, 34, 48);
			sephirothSprite.ForceInsideScreen = true;
            sephirothSprite.SetOriginTo(ObjectOrigin.Center);

            CreateAnimation(tifaSprite, 32, 48);

            // 5 - Evenements souris
            sephirothSprite.MouseOver += new EventHandler<Display.Event.MouseOverSpriteEventArgs>(sephirothSprite_MouseOver);
            sephirothSprite.MouseClicked += new EventHandler<Display.Event.MouseClickSpriteEventArgs>(sephirothSprite_MouseClicked);

            // On affiche la souris
            YnG.Game.IsMouseVisible = true;
		}

        void sephirothSprite_MouseClicked(object sender, Display.Event.MouseClickSpriteEventArgs e)
        {
            informations.Text = "Vous avez cliquez sur le Sprite !";
            clearMessage.Start();
        }

        void sephirothSprite_MouseOver(object sender, Display.Event.MouseOverSpriteEventArgs e)
        {
            informations.Text = "La souris est sur le Sprite";
            clearMessage.Start();
        }

        void clearMessage_Completed(object sender, EventArgs e)
        {
            informations.Text = "Informations en attente...";
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

            clearMessage.Update(gameTime);

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
