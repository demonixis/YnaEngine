using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Gui;
using Yna.Engine.Graphics.Gui.Widgets;

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
        protected YnWidgetProperties _buttonProps;

        public YnMenu(string name, string title, MenuEntry[] items)
            : base(name, false, true)
        {
            // Show the mouse on the menu
            YnG.ShowMouse = true;

            _background = new YnEntity(new Rectangle(0, 0, YnG.Width, YnG.Height), new Color(13, 34, 56));
            Add(_background);

            _menuItems = items;

            _menuTitle = title;

            // Register the menu skin
            YnGui.RegisterSkin("menuSkin", YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/MenuFont"));
            
            // Register the title skin
            YnGui.RegisterSkin("titleSkin", YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/TitleFont"));
            
            // Initialize the button properties
            _buttonProps = new YnWidgetProperties();
            _buttonProps.Width = 250;
            _buttonProps.Height = 50;

        }

        /// <summary>
        /// Shows the tooltip.
        /// </summary>
        /// <param name='tooltip'>Text to show</param>
        protected void ShowTooltip(string tooltip)
        {
        	YnSkin tooltipSkin = YnGui.GetSkin(_tooltip.SkinName);
            _tooltip.Text = tooltip;
            Vector2 size = tooltipSkin.FontDefault.MeasureString(tooltip);
            _tooltip.X = (int)YnG.Width / 2 - (int)size.X / 2;
        }

        /// <summary>
        ///  Initialize the state and rebuild GUI
        /// </summary>
        public override void Initialize()
        {
            Gui.Clear();

            YnLabel titleLabel = new YnLabel();
            titleLabel.Text = _menuTitle;
            titleLabel.SkinName = "titleSkin";
            titleLabel.Scale = new Vector2(3.0f);
            titleLabel.TextColor = Color.DodgerBlue;
            Gui.Add(titleLabel);

            YnLabel toggleLabel = new YnLabel();
            toggleLabel.Text = "F5 for fullscreen";
            toggleLabel.TextColor = Color.DeepSkyBlue;
            toggleLabel.Scale = new Vector2(0.9f);
            Gui.Add(toggleLabel);

            YnPanel menu = new YnPanel(YnOrientation.Vertical);
            menu.Padding = 10;
            menu.HasBackground = false;
            menu.X = YnG.Width/2 - ((int)_buttonProps.Width)/2;
            menu.Y = 230;
            Gui.Add(menu);

            for (int i = 0, l = _menuItems.Length; i < l; i++)
                menu.Add(CreateButton(_menuItems[i].StateName, _menuItems[i].TitleName, _menuItems[i].Description));

            YnTextButton exitButton = new YnTextButton(_buttonProps);
            exitButton.Text = "Exit";
            exitButton.MouseClicked += (o, e) => YnG.Exit();
            exitButton.MouseOver += (o, e) => ShowTooltip("Wanna leave? :(");
            menu.Add(exitButton);

            menu.Layout();

            _tooltip = new YnLabel();
            _tooltip.Position = new Vector2(270, 150);
            Gui.Add(_tooltip);

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
            YnTextButton button = new YnTextButton(_buttonProps);
            button.Text = label;
            button.MouseClicked += (s, e) => YnG.StateManager.SetStateActive(stateName, true);
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
