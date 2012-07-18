using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.Input;
using Yna.State;

namespace Yna.Samples3D.States
{
    public class SpaceGame : YnState
    {
        Texture2D background;

        private float aspectRatio;

        Model modelShip;
        Vector3 modelPosition = Vector3.Zero;
        Vector3 modelRotation = Vector3.Zero;
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);
        Vector3 modelVelocity = Vector3.Zero;

        Rectangle rectangle = Rectangle.Empty;

        public SpaceGame()
            : base()
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();

            background = YnG.Content.Load<Texture2D>("Backgrounds\\space");
            modelShip = YnG.Content.Load<Model>("Models\\p1_wedge");
            aspectRatio = YnG.GraphicsDevice.Viewport.AspectRatio;
            rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        private void UpdateInput()
        {
            GamePadState pad = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();

            if (pad.IsConnected)
            {
                modelRotation.Y -= pad.ThumbSticks.Left.X * 0.10f;

                Vector3 modelVelocityAdd = Vector3.Zero;

                modelVelocityAdd.X = -(float)Math.Sin(modelRotation.Y);
                modelVelocityAdd.Z = -(float)Math.Cos(modelRotation.Y);

                float velocityUp = 0.0f;

                if (pad.IsButtonDown(Buttons.RightTrigger))
                    velocityUp += pad.Triggers.Right * 5;
                else if (pad.IsButtonDown(Buttons.LeftTrigger))
                    velocityUp -= pad.Triggers.Left * 5;

                modelVelocityAdd *= velocityUp;

                modelVelocity += modelVelocityAdd;

                modelPosition.Y += pad.ThumbSticks.Left.Y * 25.0f;

                GamePad.SetVibration(PlayerIndex.One, pad.Triggers.Right, pad.Triggers.Left);

                if (pad.Buttons.A == ButtonState.Pressed)
                {
                    modelPosition = Vector3.Zero;
                    modelVelocity = Vector3.Zero;
                    modelRotation = Vector3.Zero;
                }
            }
            else
            {
                if (keyboard.IsKeyDown(Keys.Left))
                    modelRotation.Y += 0.10f;
                else if (keyboard.IsKeyDown(Keys.Right))
                    modelRotation.Y -= 0.10f;

                Vector3 modelVelocityAdd = Vector3.Zero;

                modelVelocityAdd.X = -(float)Math.Sin(modelRotation.Y);
                modelVelocityAdd.Z = -(float)Math.Cos(modelRotation.Y);

                float velocityUp = 0.0f;

                if (keyboard.IsKeyDown(Keys.Up))
                    velocityUp += 5.0f;
                else if (keyboard.IsKeyDown(Keys.Down))
                    velocityUp -= 5.0f;

                modelVelocityAdd *= velocityUp;

                modelVelocity += modelVelocityAdd;

                if (keyboard.IsKeyDown(Keys.A))
                    modelPosition.Y -= 25.0f;
                else if (keyboard.IsKeyDown(Keys.W))
                    modelPosition.Y += 25.0f;

                if (keyboard.IsKeyDown(Keys.Space))
                {
                    modelPosition = Vector3.Zero;
                    modelVelocity = Vector3.Zero;
                    modelRotation = Vector3.Zero;
                }

                if (keyboard.IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            UpdateInput();

            modelPosition += modelVelocity;
            modelVelocity *= 0.95f;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(background, rectangle, Color.White);
            spriteBatch.End();

            YnG.GraphicsDevice.BlendState = BlendState.Opaque;
            YnG.GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            Matrix[] transforms = new Matrix[modelShip.Bones.Count];
            modelShip.CopyAbsoluteBoneTransformsTo(transforms);


            foreach (ModelMesh mesh in modelShip.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World =
                        transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(modelRotation.Y) *
                        Matrix.CreateTranslation(modelPosition);

                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 100000.0f);
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
