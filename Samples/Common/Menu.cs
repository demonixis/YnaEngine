using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Display.Gui;

namespace Yna.Samples
{
    public class Menu : YnState
    {
        private YnGui _gui;
        private YnLabel _tooltip;

        public Menu(string name)
            : base(name, 1000f, 0)
        {
            YnG.ShowMouse = true;
            _gui = new YnGui(YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/MenuFont"));
        }

        public override void BuildGui()
        {
            _gui.Clear();

            const int w = 250;
            const int h = 50;

            YnLabel titleLabel = new YnLabel("YNA Samples");
            titleLabel.Skin = YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/TitleFont");
            titleLabel.TextColor = Color.DodgerBlue;
            _gui.Add(titleLabel);

            YnPanel menu = new YnPanel();
            menu.Padding = 10;
            menu.WithBackground = false;
            menu.X = 500;
            _gui.Add(menu);

            menu.Add(CreateButton("Basic Sprites", "basic_sample", "This sample shows you how to create a collection of sprite\nwithout texture."));
            menu.Add(CreateButton("Animated Sprites", "animated_sample", "Learn how to create an animated Sprite with a SpriteSheet"));
            menu.Add(CreateButton("Simple Tiled Map", "tilemap_sample", "Create a tilemap and a camera"));
            menu.Add(CreateButton("Isometric Tiled Map", "iso_sample", "Create an isometric tilemap and a camera"));
            menu.Add(CreateButton("UI Example", "ui_sample", "Integrated UI examples"));

            YnTextButton exitButton = new YnTextButton("Exit", w, h, false);
            exitButton.MouseJustClicked += (o, e) => YnG.Exit();
            exitButton.MouseOver += (o, e) => ShowTooltip("Wanna leave? :(");
            menu.Add(exitButton);

            _tooltip = new YnLabel();
            _tooltip.Position = new Vector2(270, 150);
            _gui.Add(_tooltip);

            _gui.PrepareWidgets();

            menu.Position = new Vector2(YnG.Width / 2 - menu.Width / 2, 250);
            titleLabel.Position = new Vector2(YnG.Width / 2 - titleLabel.Width / 2, 50);
        }

        private YnButton CreateButton(string label, string stateName, string tooltip)
        {
            const int w = 250;
            const int h = 50;

            YnTextButton button = new YnTextButton(label, w, h, false);
            button.MouseJustClicked += (o, e) => YnG.ScreenManager.SetScreenActive(stateName, true);
            button.MouseOver += (o, e) => ShowTooltip(tooltip);

            return button;
        }

        public override void Initialize()
        {
            base.Initialize();

            _gui.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _gui.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _gui.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.Exit();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            _gui.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void ShowTooltip(string tooltip)
        {
            _tooltip.Text = tooltip;
            Vector2 size = _tooltip.Skin.Font.MeasureString(tooltip);
            _tooltip.X = (int)YnG.Width / 2 - (int)size.X / 2;
        }
    }
}
