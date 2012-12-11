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
