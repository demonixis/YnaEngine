using Yna.Engine;
using Yna.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Graphics.Particle;

namespace Yna.Samples.Screens
{
    public class ParticlesSample : YnState2D
    {
        // Create 2 emitters
        private YnEmitter emitter;
        private YnEmitter emitter2;
        private YnText demoText;

        public ParticlesSample(string name)
            : base(name)
        {
            // An emitter is an entity who launch particles in a time interval.
            // You must provide a position, a direction, an angle and a the maximum of particles to use.
            emitter = new YnEmitter(new Vector2(105, 465), new Vector2(0.5f, -1f), MathHelper.PiOver4, 150);
            emitter2 = new YnEmitter(new Vector2(625, 465), new Vector2(-0.5f, -1f), MathHelper.PiOver4, 150);

            // A debug text
            demoText = new YnText("Fonts/DefaultFont", "Left click for start or stop particle emission\nDrag the mouse with right click for move emitters");
            demoText.Color = Color.White;
            demoText.Scale = new Vector2(0.9f);
        }

        public override void Initialize()
        {
            // Load content a initialization
            emitter.LoadContent();
            emitter.Initialize(YnG.Content.Load<Texture2D>("Sprites/sphere_red"));
            emitter.Intensity = 200; // Intensity of launch

            // You can change the configuration for particle.
            emitter.SetParticleConfiguration(new ParticleConfiguration()
            {
                Width = 7,              // Width of a particle
                Height = 7,
                EnabledRotation = true, // Particles will rotates
                RotationIncrement = 1,  // Rotation value
                Speed = 6.5f,           // Speed
                LifeTime = 700,         // Life time
            });

            emitter2.LoadContent();
            emitter2.Initialize(Color.LightGoldenrodYellow, 4, 4);

            demoText.LoadContent();
            demoText.Position = new Vector2(YnG.Width / 2 - demoText.Width * demoText.Scale.X / 2, 10);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (YnG.Keys.JustPressed(Keys.Escape))
                Exit();

            emitter.Update(gameTime);
            emitter2.Update(gameTime);

            // Just start or stop the particle system with a left click
            if (YnG.Mouse.JustClicked(Yna.Engine.Input.MouseButton.Left))
            {
                if (emitter.Active)
                {
                    emitter.Stop();
                    emitter2.Stop();
                }
                else
                {
                    emitter.Start();
                    emitter2.Start();
                }
            }

            // Move emitters with a drag
            if (YnG.Mouse.Drag(Yna.Engine.Input.MouseButton.Right))
            {
                var delta = YnG.Mouse.Delta;
                emitter.SetPosition((int)(emitter.X + delta.X), (int)(emitter.Y + delta.Y));
                emitter2.SetPosition((int)(emitter2.X + delta.X), (int)(emitter2.Y + delta.Y));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            emitter.Draw(gameTime, spriteBatch);
            emitter2.Draw(gameTime, spriteBatch);
            demoText.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
