using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Display.Gui;

namespace Yna.Samples.States
{
	/// <summary>
	/// Description of UIExample.
	/// </summary>
	public class UIExample : YnState
	{
		private Gui Gui{ get; set;}
		
		private Texture2D Background { get; set; }
		
		public UIExample()
		{
            Gui = new Gui(YnG.Game);

            YnLabel titleLabel = Gui.Add(new YnLabel());
            titleLabel.Text = "[GUI Examples]";

            YnLabel label = Gui.Add(new YnLabel());
            label.Text = "This is a simple label";
            label.Position = new Vector2(50, 50);


            /*
			UiManager = new YnUiManager();
			YnUiText simpleLabel = UiManager.Add(new YnUiText());
			simpleLabel.Text = "Simple labe";
			simpleLabel.Position = Vector2.Zero;
			
			YnUiWindow window = UiManager.Add(new YnUiWindow());
			window.Rectangle = new Rectangle(50, 50, 250, 200);
			YnUiText windowLabel = window.Add(new YnUiText());
			windowLabel.Text = "Label inside window";
             */
		}
		
		public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            // return to the menu if escape key is just pressed
            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new Menu());
        }
		
		public override void Initialize()
		{
			base.Initialize();
            Gui.Initialize();
		}
		
		public override void LoadContent()
		{
			
			base.LoadContent();
			
			Background = YnG.Content.Load<Texture2D>("Backgrounds/greenground1");
		}
		
		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			
			spriteBatch.Begin();
			//spriteBatch.Draw(Background, Vector2.Zero, Color.White);
			spriteBatch.End();
			
			
			// Draw the HUD
			//UiManager.Draw(gameTime, spriteBatch);
		}
	}
}
