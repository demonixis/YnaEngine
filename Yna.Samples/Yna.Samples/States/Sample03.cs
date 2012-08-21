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

        private YnPath tifaPath;
        private YnPath cloudPath;
        
        private YnTimer clearMessage;

        public Sample03 () 
            : base(1000f, 0) 
        {
            // 0 - The background
            background = new YnSprite("Backgrounds/back-ff6");

            // 1 - Player's Sprite
            sephirothSprite = new YnSprite(new Vector2(150, 250), "Sprites/sephiroth");
            Add(sephirothSprite);

            // 2 - Let's create 2 PNJs player
            tifaSprite = new YnSprite(new Vector2(350, 150), "Sprites/tifa");
            Add(tifaSprite);

            cloudSprite = new YnSprite(new Vector2(350, 25), "Sprites/cloud");
            Add(cloudSprite);

            // 3 - We wan't debug informations
            informations = new YnText("Fonts/MenuFont", 25, 25, "Informations");
            informations.Color = Color.Yellow;
            informations.Scale = new Vector2(2.0f, 2.0f);
            Add(informations);

            // Add a clear timer for update informations
            clearMessage = new YnTimer(1500, 0);
            clearMessage.Completed += new EventHandler<EventArgs>(clearMessage_Completed);

            // 4 - Create 2 path for PNJs
            tifaPath = new YnPath(tifaSprite, true);
            cloudPath = new YnPath(cloudSprite, true);
        }

        public override void Initialize ()
		{
            background.LoadContent();
            background.SetRectangles(new Rectangle(0, 0, YnG.Width, YnG.Height));

            // Create animations for all Sprites
            CreateAnimation(sephirothSprite, 34, 48);
            CreateAnimation(tifaSprite, 32, 48);
            CreateAnimation(cloudSprite, 32, 48);

            // The player can't exit the screen
			sephirothSprite.InsideScreen = true; 

            // Ok now we'll play with mouse events !
            tifaSprite.MouseOver += new EventHandler<MouseOverSpriteEventArgs>(OnSprite_MouseOver);
            sephirothSprite.MouseClick += new EventHandler<MouseClickSpriteEventArgs>(OnSprite_MouseClicked);

            // Change the scale
            Vector2 scale = new Vector2(1.5f, 1.5f);
            tifaSprite.Scale = scale;
            sephirothSprite.Scale = scale;
            cloudSprite.Scale = scale;

            // Create paths relative to the Sprite's start position
            cloudSprite.Position = new Vector2(150, 250);
            cloudPath.AddTo(150, 0, 2);
            cloudPath.AddTo(0, 200, 2);
            cloudPath.AddTo(0, -250, 2);
            cloudPath.AddTo(0, 100, 2);
            cloudPath.AddTo(-150, -50, 2);
            cloudPath.AddTo(-10, 10, 2);
            cloudPath.AddTo(100, 0, 2);
            cloudPath.AddTo(0, 100, 2);
            cloudPath.End();

            tifaSprite.Position = new Vector2(350, 350);
            tifaPath.AddTo(150, 0, 2);
            tifaPath.AddTo(0, 200, 2);
            tifaPath.AddTo(0, -250, 2);
            tifaPath.AddTo(0, 100, 2);
            tifaPath.AddTo(-150, -50, 2);
            tifaPath.End();

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
            if (sprite.Direction.X < 0)
                sprite.Play("left");
            else if (sprite.Direction.X > 0)
                sprite.Play("right");
            else if (sprite.Direction.Y < 0)
                sprite.Play("up");
            else if (sprite.Direction.Y > 0)
                sprite.Play("down");
            else
                sprite.Play("down");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // Les objets attachés sont mis à jour

            clearMessage.Update(gameTime);

            tifaPath.Update(gameTime);
            cloudPath.Update(gameTime);

			// Moving the Player's Sprite with the keyboard
            if (YnG.Keys.Pressed(Keys.Up))
            {
                sephirothSprite.Y -= 2;
            }
            else if (YnG.Keys.Pressed(Keys.Down))
            {
                sephirothSprite.Y += 2;
            }
            else if (YnG.Keys.Pressed(Keys.Left))
            {
                sephirothSprite.X -= 2;
            }
            else if (YnG.Keys.Pressed(Keys.Right))
            {
                sephirothSprite.X += 2;
            }

            // Update animation relative to there own direction
            UpdateAnimations(sephirothSprite);
            UpdateAnimations(tifaSprite);
            UpdateAnimations(cloudSprite);

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
