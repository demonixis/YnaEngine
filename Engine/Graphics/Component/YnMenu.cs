using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Gui;

namespace Yna.Engine.Graphics.Component
{
    /// <summary>
    /// Define an item to use with the menu
    /// </summary>
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

    /// <summary>
    /// A basic menu state ready to use
    /// </summary>
    public class YnMenu : YnState2D
    {
        protected YnLabel _tooltip;
        protected YnEntity _background;
        protected string _menuTitle;
        protected MenuEntry[] _menuItems;
        protected int _buttonWidth;
        protected int _buttonHeight;

        public YnMenu(string name, string title, MenuEntry[] items)
            : base(name)
        {
            _background = new YnEntity(new Rectangle(0, 0, YnG.Width, YnG.Height), new Color(13, 34, 56));
            Add(_background);

            _menuItems = items;

            _menuTitle = title;

            _buttonWidth = 250;
            _buttonHeight = 50;

            Gui.SetSkin(YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/Menu"));
        }

        /// <summary>
        /// Shows the tooltip.
        /// </summary>
        /// <param name='tooltip'>Text to show</param>
        protected void ShowTooltip(string tooltip)
        {
            _tooltip.Text = tooltip;
            Vector2 size = _tooltip.Skin.Font.MeasureString(tooltip);
            _tooltip.X = (int)YnG.Width / 2 - (int)size.X / 2;
        }

        /// <summary>
        ///  Initialize the state and rebuild GUI
        /// </summary>
        public override void Initialize()
        {
            Gui.Clear();

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

            for (int i = 0, l = _menuItems.Length; i < l; i++)
                menu.Add(CreateButton(_menuItems[i].StateName, _menuItems[i].TitleName, _menuItems[i].Description));

            YnTextButton exitButton = new YnTextButton("Exit", _buttonWidth, _buttonHeight, false);
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

        /// <summary>
        /// Creates a new button
        /// </summary>
        /// <returns>A button.</returns>
        /// <param name='stateName'>The state's name to launch when you click on this button</param>
        /// <param name='label'>Label content</param>
        /// <param name='tooltip'>Tooltip content</param>
        protected YnButton CreateButton(string stateName, string label, string tooltip)
        {
            YnTextButton button = new YnTextButton(label, _buttonWidth, _buttonHeight, false);
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
    }
}
