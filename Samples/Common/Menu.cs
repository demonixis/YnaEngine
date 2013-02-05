using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Gui;

namespace Yna.Samples
{
    public struct MenuEntry
    {
        public string StateName;
        public string TitleName;
        public string Description;

        public MenuEntry(string stateName, string titleName, string description)
        {
            StateName = stateName;
            TitleName = titleName;
            Description = description;
        }
    }

    public class Menu : YnState2D
    {
        private YnLabel _tooltip;
        private string _menuTitle;
        private MenuEntry[] menuItems;

        public Menu(string name, string title, MenuEntry[] items)
            : base(name)
        {
            YnG.StateManager.ClearColor = new Color(13, 34, 56);
            YnG.ShowMouse = true;
            menuItems = items;
            _menuTitle = title;

            Gui.SetSkin(YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/MenuFont"));
        }

        public override void Initialize()
        {
            BuildGui();
        }

        public void BuildGui()
        {
            Gui.Clear();

            const int w = 250;
            const int h = 50;

            YnLabel titleLabel = new YnLabel(_menuTitle);
            titleLabel.Skin = YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/TitleFont");
            titleLabel.Scale = new Vector2(3.0f);
            titleLabel.TextColor = Color.DodgerBlue;
            Gui.Add(titleLabel);

            YnLabel toggleLabel = new YnLabel("F5 for fullscreen");
            toggleLabel.TextColor = Color.DeepSkyBlue;
            toggleLabel.Scale = new Vector2(0.9f);
            Gui.Add(toggleLabel);

            YnPanel menu = new YnPanel();
            menu.Padding = 10;
            menu.WithBackground = false;
            menu.X = 500;
            Gui.Add(menu);

            for (int i = 0, l = menuItems.Length; i < l; i++)
                menu.Add(CreateButton(menuItems[i].StateName, menuItems[i].TitleName, menuItems[i].Description));

            YnTextButton exitButton = new YnTextButton("Exit", w, h, false);
            exitButton.MouseJustClicked += (o, e) => YnG.Exit();
            exitButton.MouseOver += (o, e) => ShowTooltip("Wanna leave? :(");
            menu.Add(exitButton);

            _tooltip = new YnLabel();
            _tooltip.Position = new Vector2(270, 150);
            Gui.Add(_tooltip);

            Gui.PrepareWidgets();

            menu.Position = new Vector2(YnG.Width / 2 - menu.Width / 2, 250);
            titleLabel.Position = new Vector2(YnG.Width / 2 - titleLabel.Width / 2, 50);
            toggleLabel.Position = new Vector2(YnG.Width - toggleLabel.Width - 15, 15);
        }

        private YnButton CreateButton(string stateName, string label, string tooltip)
        {
            const int w = 250;
            const int h = 50;

            YnTextButton button = new YnTextButton(label, w, h, false);
            button.MouseJustClicked += (s, e) => YnG.StateManager.SetStateActive(stateName, true);
            button.MouseOver += (s, e) => ShowTooltip(tooltip);

            return button;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.F5))
                YnG.GraphicsDeviceManager.ToggleFullScreen();

            if (YnG.Gamepad.Back(PlayerIndex.One) || YnG.Keys.JustPressed(Keys.Escape))
                YnG.Exit();
        }

        private void ShowTooltip(string tooltip)
        {
            _tooltip.Text = tooltip;
            Vector2 size = _tooltip.Skin.Font.MeasureString(tooltip);
            _tooltip.X = (int)YnG.Width / 2 - (int)size.X / 2;
        }
    }
}
