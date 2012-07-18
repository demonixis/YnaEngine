using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Yna;
using Yna.Display;
using Yna.State;

namespace Yna.Sample.States
{
    public class Sample01 : YnState
    {
    	const int numSprites = 10;
    	
        Sprite background;
        YnGroup sprites;
        Color [] colors;   
        
      //  SoundEffect song;
        Song music;

        public Sample01() 
            : base (500f, 50f) 
        { 
        	sprites = new YnGroup(numSprites);
        	colors = new Color[numSprites];
        	
        	Random rand = new Random(DateTime.Now.Millisecond);
        	
        	for (int i = 0; i < 10; i++)
        		colors[i] = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        }

        public override void Initialize()
        {
            base.Initialize();

            //song = YnG.Content.Load<SoundEffect>("audio//scary-tone");
            music = YnG.Content.Load<Song>("audio//welcome-to-nova");
            MediaPlayer.Play(music);
            
            background = new Sprite(Vector2.Zero, "Backgrounds//Sky3");
            background.Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);
			
            int spriteWidth = 50;
            int spriteHeight = 50;
            Random rand = new Random(DateTime.Now.Millisecond);
            
            for (int i = 0; i < numSprites; i++)
            {
            	float x = rand.Next(0, YnG.Width - spriteWidth);
            	float y = rand.Next(0, YnG.Height - spriteHeight);

            	Sprite sprite = new Sprite(new Rectangle(0, 0, spriteWidth, spriteHeight), colors[i]);
            	sprite.Position = new Vector2(x, y);
            	sprite.SetOriginTo(SpriteOrigin.Center);
            	sprite.ForceInsideOutsideScreen = true;
            	sprites.Add(sprite);
            }

            Add(background);
            Add(sprites);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (YnG.Keys.JustPressed(Keys.Enter))
            {
                if (MediaPlayer.State == MediaState.Stopped)
                    MediaPlayer.Play(music);
                else if (MediaPlayer.State == MediaState.Paused)
                    MediaPlayer.Resume();
            }
            
            if (YnG.Keys.JustPressed(Keys.Space))
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Pause();
            }
            
            if (YnG.Keys.JustPressed(Keys.Tab))
                MediaPlayer.Pause();
            
            foreach (Sprite sprite in sprites)
            {
            	sprite.X += 3;
            	sprite.Y += 2;
            	sprite.Rotation += 0.1f;
            }

            if (YnG.Keys.JustPressed(Keys.Escape))
            {
                SoundEffect sound = YnG.Content.Load<SoundEffect>("Audio//terminated");
                sound.Play();
                YnG.SwitchState(new GameMenu());
            }
        }
    }
}
