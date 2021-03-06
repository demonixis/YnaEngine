﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Component;
using Yna.Engine.Input;
using Yna.Engine.Script;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Samples.Screens
{
    public class AnimatedSpriteVirtualPad : YnState2D
    {
        // We create the background
        private YnEntity background;

        // We create 3 sprites
        private YnSprite womanSprite;
        private YnSprite manSprite;
        private YnSprite gunnerSprite;

        // And now some objects
        private YnEntity woodObject;
        private YnEntity wood2Object;
        private YnEntity houseObject;

        // Info text
        private YnText textInfo;

        private ScriptAnimator womanAnimator;

        private List<YnEntity> spriteToCollide;

        private YnVirtualPadController virtualPadController;

        public AnimatedSpriteVirtualPad(string name)
            : base(name)
        {

            background = new YnEntity("Sprites/GreenGround");
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
            woodObject = new YnEntity("Sprites/Tree");
            Add(woodObject);

            wood2Object = new YnEntity("Sprites/Tree2");
            Add(wood2Object);

            houseObject = new YnEntity("Sprites/House");
            Add(houseObject);

            string message = "Use the virtual pad for moving the player\nPress the first button for shake the scene\nPress the top left button for back to menu";

            textInfo = new YnText("Fonts/DefaultFont", message, Vector2.Zero, Color.YellowGreen);
            Add(textInfo);

            virtualPadController = new YnVirtualPadController();
            virtualPadController.VirtualPad.InverseDirectionStrafe = true;
            Add(virtualPadController.VirtualPad);

            spriteToCollide = new List<YnEntity>(5);
            spriteToCollide.Add(womanSprite);
            spriteToCollide.Add(gunnerSprite);
            spriteToCollide.Add(woodObject);
            spriteToCollide.Add(wood2Object);
            spriteToCollide.Add(houseObject);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // The background size is the window size
            background.Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);

            textInfo.Position = new Vector2(YnG.Width - textInfo.Width - 15, 15);

            // Place sprites on the screen
            // Here, sprites are already loaded (assets)
            womanSprite.Position = new Vector2(50, 50);
            manSprite.Position = new Vector2(350, 350);
            gunnerSprite.Position = new Vector2((YnG.Width / 2) - (gunnerSprite.Width / 2), (YnG.Height / 2) - (gunnerSprite.Height / 2));

            // Force the sprite to stay on the screen
            manSprite.ForceInsideScreen = true;

            // Create animations for sprites
            CreateSpriteAnimations(womanSprite);
            CreateSpriteAnimations(manSprite);
            CreateSpriteAnimations(gunnerSprite);

            // Position for objects
            woodObject.Position = new Vector2(50, YnG.Height - (1.5f * (woodObject.Height)));
            wood2Object.Position = new Vector2((YnG.Width - 50) - wood2Object.Width, YnG.Height - (1.5f * (wood2Object.Height)));
            houseObject.Position = new Vector2((YnG.Width / 2) - (houseObject.Width / 2), 10);

            womanAnimator = new ScriptAnimator(womanSprite);
            womanAnimator.RepeatAnimation = true;
            womanAnimator.Add(new WaitScript(2000));
            womanAnimator.Add(new MoveScript(250, 50, 1));
            womanAnimator.Add(new WaitScript(2000));
            womanAnimator.Add(new MoveScript(50, 50, 1));
            womanAnimator.Add(new MoveScript(50, 250, 1));
            womanAnimator.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            womanAnimator.Update(gameTime);

            // Move the sprite of the man
            if (YnG.Keys.Up || virtualPadController.Pressed(PadButtons.Up))
                manSprite.Y -= 2;
            else if (YnG.Keys.Down || virtualPadController.Pressed(PadButtons.Down))
                manSprite.Y += 2;

            if (YnG.Keys.Left || virtualPadController.Pressed(PadButtons.Left))
                manSprite.X -= 2;
            else if (YnG.Keys.Right || virtualPadController.Pressed(PadButtons.Right))
                manSprite.X += 2;

            // Shake the screen
            if (YnG.Keys.JustPressed(Keys.S) || virtualPadController.Pressed(PadButtons.ButtonA))
            {
                Camera.Shake(15, 2500);
            }

            // Update sprites' animations
            UpdateAnimations(womanSprite);
            UpdateAnimations(manSprite);
            UpdateAnimations(gunnerSprite);

            if (YnCollider.CollideOneWithGroup(manSprite, spriteToCollide))
                manSprite.Position = manSprite.LastPosition;

            // return to the menu if escape key is just pressed
            if (YnG.Keys.JustPressed(Keys.Escape) || virtualPadController.Pressed(PadButtons.Pause))
                YnG.StateManager.SetActive("menu", true);
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
