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
            YnG.ShowMouse = true;

            Gui = new Gui(YnG.Game);

            // Simple Label example
            YnLabel simpleLabel = Gui.Add(new YnLabel());
            simpleLabel.Text = "Simple Label";
            simpleLabel.Position = new Vector2(10, 10);
            

            // Simple vertical panel
            YnLabel simpleTitle = Gui.Add(new YnLabel());
            simpleTitle.Text = "Automatic resize of panels :";
            simpleTitle.Position = new Vector2(10, 30);

            YnPanel panel = Gui.Add(new YnPanel());
            panel.Orientation = YnOrientation.Vertical;
            panel.Bounds = new Rectangle(10, 50, 50, 50);
            panel.Padding = 5;

            panel.Add(new YnLabel() { Text = "Short" });
            panel.Add(new YnLabel() { Text = "Some basic text" });
            panel.Add(new YnLabel() { Text = "Some extreme and dangerously long label" });
            

            // Complex panel
            YnLabel complexTitle = Gui.Add(new YnLabel());
            complexTitle.Text = "Complex panel management :";
            complexTitle.Position = new Vector2(350, 80);

            YnPanel complexPanel = Gui.Add(new YnPanel());
            complexPanel.Orientation = YnOrientation.Horizontal;
            complexPanel.Position = new Vector2(350, 100);
            complexPanel.Padding = 3;

            YnPanel leftPanel = complexPanel.Add(new YnPanel());
            leftPanel.Padding = 3;
            leftPanel.Orientation = YnOrientation.Vertical;
            leftPanel.Add(new YnLabel() { Text = "Left label A with long text" });
            leftPanel.Add(new YnLabel() { Text = "Left label B" });

            YnPanel RightPanel = complexPanel.Add(new YnPanel());
            RightPanel.Padding = 3;
            RightPanel.Orientation = YnOrientation.Vertical;
            RightPanel.Add(new YnLabel() { Text = "Right label A" });
            RightPanel.Add(new YnLabel() { Text = "Right label B withe loooooong text" });
        
            // Panel without padding
            YnPanel noPaddingPanel = Gui.Add(new YnPanel());
            noPaddingPanel.Position = new Vector2(10, 180);
            noPaddingPanel.Add(new YnLabel() { Text = "Panel without padding" });

            // Panel with padding
            YnPanel paddedPanel = Gui.Add(new YnPanel());
            paddedPanel.Position = new Vector2(10, 210);
            paddedPanel.Padding = 20;
            paddedPanel.Add(new YnLabel() { Text = "Panel without big padding" });
            
            // Buttons in a toolbar
            YnPanel toolbar = Gui.Add(new YnPanel());
            toolbar.Orientation = YnOrientation.Horizontal;
            toolbar.Position = new Vector2(300, 300);
            toolbar.Padding = 2;

            toolbar.Add(new YnTextButton() { Text = "Load" });
            toolbar.Add(new YnTextButton() { Text = "Save" });
            toolbar.Add(new YnTextButton() { Text = "Options" });
            toolbar.Add(new YnTextButton() { Text = "Exit" });
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
		}
		
		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			
			spriteBatch.Begin();
			// Nothing!
			spriteBatch.End();
			
			
			// Draw the HUD
			//UiManager.Draw(gameTime, spriteBatch);
		}
	}
}
