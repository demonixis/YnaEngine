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

namespace Yna.Samples.States
{
    public class AnimatedSprites : YnState
    {
        // We create the background
        private YnImage background;

        // We create 3 sprites
        private YnSprite womanSprite;
        private YnSprite manSprite;
        private YnSprite gunnerSprite;

        // And now some objects
        private YnImage woodObject;
        private YnImage wood2Object;
        private YnImage houseObject;

        public AnimatedSprites(string name)
            : base (name, 1500, 0)
        {
            background = new YnImage("Backgrounds/greenground1");
            Add(background);

            // Create the sprites at position (0, 0) and add it on the state
            // When we Add an object to the state, it is loaded and initialized
            // The main loop will update and draw the sprite
            // The asset name passed in constructor is a spritesheet
            womanSprite = new YnSprite("Sprites/BRivera-femaleelfwalk");
            Add(womanSprite);

            manSprite = new YnSprite("Sprites/BRivera-malesoldier");
            Add(manSprite);

            gunnerSprite = new YnSprite("Sprites/BRivera-gunnerwalkcycle");
            Add(gunnerSprite);

            // Objects
            woodObject = new YnImage("Objects/fullpufftree");
            Add(woodObject);

            wood2Object = new YnImage("Objects/whitheredtree1");
            Add(wood2Object);

            houseObject = new YnImage("Objects/building11");
            Add(houseObject);
        }

        public override void Initialize()
        {
            base.Initialize();

            // The background size is the window size
            background.Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);

            // Place sprites on the screen
            // Here, sprites are already loaded (assets)
            womanSprite.Position = new Vector2(50, 50);
            manSprite.Position = new Vector2((YnG.Width - manSprite.Width) - 50, (YnG.Height - manSprite.Height) - 50);
            gunnerSprite.Position = new Vector2((YnG.Width / 2) - (gunnerSprite.Width / 2), (YnG.Height / 2) - (gunnerSprite.Height / 2));

            // Force the sprite to stay on the screen
            manSprite.InsideScreen = true;

            // Create animations for sprites
            CreateSpriteAnimations(womanSprite);
            CreateSpriteAnimations(manSprite);
            CreateSpriteAnimations(gunnerSprite);

            // Position for objects
            woodObject.Position = new Vector2(50, YnG.Height - (1.5f * (woodObject.Height)));
            wood2Object.Position = new Vector2((YnG.Width - 50) - wood2Object.Width, YnG.Height - (1.5f * (wood2Object.Height)));
            houseObject.Position = new Vector2((YnG.Width / 2) - (houseObject.Width / 2), 10); 
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Move the sprite of the man
            if (YnG.Keys.Up)
                manSprite.Y -= 2;
            else if (YnG.Keys.Down)
                manSprite.Y += 2;

            if (YnG.Keys.Left)
                manSprite.X -= 2;
            else if (YnG.Keys.Right)
                manSprite.X += 2;

            // Update sprites' animations
            UpdateAnimations(womanSprite);
            UpdateAnimations(manSprite);
            UpdateAnimations(gunnerSprite);

            // return to the menu if escape key is just pressed
            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new Menu());
        }

        private void CreateSpriteAnimations(YnSprite sprite)
        {
            // The size of a sprite on the spritesheet is 64x64
            // The spritesheet have 4 animations and each animations has 9 frames
            // The size of the spreetshit is 576x256, so a the size for a sprite is
            // 576 / 9 = 64 (width) and 256 / 4 = 64 (height)
            sprite.PrepareAnimation(64, 64);
            
            // Add 4 animations
            // 1 - Name for the animation
            // 2 - An array of index that represent what image we want using
            // 3 - The framerate for the animation
            // 4 - We don't reverse the image because we have all animation in the spritesheet
            sprite.AddAnimation("up", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, 25, false);
            sprite.AddAnimation("left", new int[] { 9, 10, 11, 12, 13, 14, 15, 16, 17 }, 25, false);
            sprite.AddAnimation("down", new int[] { 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 25, false);
            sprite.AddAnimation("right", new int[] { 27, 28, 29, 30, 31, 32, 33, 34, 35 }, 25, false);
        }

        private void UpdateAnimations(YnSprite sprite)
        {
            if (sprite.Direction.X < 0)
                sprite.Play("left");
            else if (sprite.Direction.X > 0)
                sprite.Play("right");
            
            if (sprite.Direction.Y < 0)
                sprite.Play("up");
            else if (sprite.Direction.Y > 0)
                sprite.Play("down");
        }
    }
}
