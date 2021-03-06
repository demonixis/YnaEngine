﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Input;
using Yna.Engine.Script;

namespace Yna.Samples.Screens
{
    public class AnimatedSprites : YnState2D
    {
        // We create the background
        private YnEntity background;

        private YnGroup spriteGroup;

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

        private YnQuadTree quadTree;
        private List<YnEntity> spriteToCollide;

        public AnimatedSprites(string name)
            : base(name)
        {
            background = new YnEntity("Sprites/GreenGround");
            Add(background);

            spriteGroup = new YnGroup();
            Add(spriteGroup);

            // Create the sprites at position (0, 0) and add it on the state
            // When we Add an object to the state, it is loaded and initialized
            // The main loop will update and draw the sprite
            // The asset name passed in constructor is a spritesheet
            womanSprite = new YnSprite("Sprites/BRivera-femaleelfwalk");
            spriteGroup.Add(womanSprite);

            manSprite = new YnSprite("Sprites/BRivera-malesoldier");
            spriteGroup.Add(manSprite);

            gunnerSprite = new YnSprite("Sprites/BRivera-gunnerwalkcycle");
            spriteGroup.Add(gunnerSprite);

            // Objects
            woodObject = new YnEntity("Sprites/Tree");
            Add(woodObject);

            wood2Object = new YnEntity("Sprites/Tree2");
            Add(wood2Object);

            houseObject = new YnEntity("Sprites/House");
            Add(houseObject);

            textInfo = new YnText("Fonts/DefaultFont", "Press S for shake the screen\nUse Right click to move the scene\nUse Middle click to rotate the scene\nUseLeft click to reset", Vector2.Zero, Color.YellowGreen);
            Add(textInfo);

            quadTree = new YnQuadTree(0, new Rectangle(0, 0, YnG.Width, YnG.Height));
            quadTree.MaxObjectsPerNode = 2;
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

            textInfo.Position = new Vector2(15, 15);

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

            // Update the quadtree structure
            quadTree.Begin(spriteToCollide.ToArray());
            
            womanAnimator.Update(gameTime);

            // Move the sprite of the man
            if (YnG.Keys.Up)
                manSprite.Y -= 2;
            else if (YnG.Keys.Down)
                manSprite.Y += 2;

            if (YnG.Keys.Left)
                manSprite.X -= 2;
            else if (YnG.Keys.Right)
                manSprite.X += 2;

            if (YnG.Keys.Pressed(Keys.NumPad8))
                spriteGroup.Y--;
            else if (YnG.Keys.Pressed(Keys.NumPad5))
                spriteGroup.Y++;

            if (YnG.Keys.Pressed(Keys.NumPad4))
                spriteGroup.X--;
            else if (YnG.Keys.Pressed(Keys.NumPad6))
                spriteGroup.X++;

            if (YnG.Keys.Pressed(Keys.NumPad7))
                spriteGroup.Rotation += 0.1f;
            else if (YnG.Keys.Pressed(Keys.NumPad9))
                spriteGroup.Rotation -= 0.1f;

            if (YnG.Keys.Pressed(Keys.NumPad1))
                spriteGroup.Scale += new Vector2(0.1f);
            else if (YnG.Keys.Pressed(Keys.NumPad3))
                spriteGroup.Scale -= new Vector2(0.1f);


            // A state can be transformed with translation, rotation and zoom
            // Move the screen
            if (YnG.Mouse.Drag(MouseButton.Left))
            {
                Camera.X += (int)YnG.Mouse.Delta.X;
                Camera.Y += (int)YnG.Mouse.Delta.Y;
            }

            // Rotate the screen
            if (YnG.Mouse.Drag(MouseButton.Middle))
                Camera.Rotation += YnG.Mouse.Delta.X;

            // Reset to default
            // Click on Right button to reset the position
            if (YnG.Mouse.JustClicked(MouseButton.Right))
            {
                Camera.X = 0;
                Camera.Y = 0;
                Camera.Rotation = 0.0f;
            }

            // Shake the screen
            if (YnG.Keys.JustPressed(Keys.S))
            {
                Camera.Shake(15, 2500);
            }

            // Update sprites' animations
            UpdateAnimations(womanSprite);
            UpdateAnimations(manSprite);
            UpdateAnimations(gunnerSprite);
  
            quadTree.TestCandidates(manSprite, (mSprite, lSprite) =>
                {
                    if (mSprite.Rectangle.Intersects(lSprite.Rectangle))
                        manSprite.Position = manSprite.LastPosition;
                });

            // return to the menu if escape key is just pressed
            if (YnG.Keys.JustPressed(Keys.Escape))
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
            sprite.AddAnimation("up", 0, 8, 25, false);
            sprite.AddAnimation("left", 9, 17, 25, false);
            sprite.AddAnimation("down", 18, 26, 25, false);
            sprite.AddAnimation("right", 27, 35, 25, false);
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
