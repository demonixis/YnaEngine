using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;

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

    public class MenuButton : YnGroup
    {
        public const int ButtonWidth = 160;
        public const int ButtonHeight = 35;
        private const string FontName = "Fonts/DefaultFont";
        private YnEntity2D _background;
        private YnText _text;

        public MenuButton(string text)
        {
            _background = new YnEntity2D(new Rectangle(0, 0, ButtonWidth, ButtonHeight), Color.DarkGreen);
            Add(_background);

            _text = new YnText(FontName, text);
            Add(_text);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _text.Position = new Vector2(
                _background.X + (_background.Width / 2) - (_text.ScaledWidth / 2),
                _background.Y + (_background.Height / 2) - (_text.ScaledHeight / 2));
        }
    }

    /// <summary>
    /// A basic menu state ready to use
    /// </summary>
    public class YnMenu : YnState2D
    {
        private MenuEntry[] _menuItems;
        private YnEntity _background;
        private YnText _title;
        private MenuButton[] _clickableItems;
      

        public YnMenu(string name, string title, MenuEntry[] items)
            : base(name, false, true)
        {
            _menuItems = items;

            YnG.ShowMouse = true;
            YnText.DefaultColor = Color.White;

            _background = new YnEntity2D(new Rectangle(0, 0, YnG.Width, YnG.Height), new Color(13, 34, 56));
            Add(_background);

            _clickableItems = new MenuButton[_menuItems.Length];
            for (int i = 0, l = _clickableItems.Length; i < l; i++)
            {
                _clickableItems[i] = new MenuButton(_menuItems[i].TitleName);
                _clickableItems[i].MouseClicked += YnMenu_MouseClicked;
                _clickableItems[i].Name = "item_" + i;
                Add(_clickableItems[i]);
            }

            _title = new YnText("Fonts/DefaultFont", title);
            _title.Scale = new Vector2(2.5f);
            Add(_title);
        }

        void YnMenu_MouseClicked(object sender, Event.MouseClickEntityEventArgs e)
        {
            MenuButton item = sender as MenuButton;

            if (item != null)
            {
                int id = int.Parse(item.Name.Split(new char[] { '_' })[1].ToString());
                YnG.StateManager.SetActive(_menuItems[id].StateName, true);
            }
        }

        /// <summary>
        ///  Initialize the state and rebuild GUI
        /// </summary>
        public override void Initialize()
        {
            int startPx = 65;
            int startPy = 95;
            int px = startPx;
            int py = startPy;
            int counter = 0;
            int itemCount = _clickableItems.Length;
            int nbMaxPerRow = YnG.Height / (MenuButton.ButtonHeight * 2);

            for (int i = 0; i < itemCount; i++)
            {
                if (counter > nbMaxPerRow)
                {
                    px += MenuButton.ButtonWidth + 35;
                    py = startPy;
                    counter = 0;
                }
                _clickableItems[i].Position = new Vector2(px, py);
                py += MenuButton.ButtonHeight + 15;
                counter++;
            }

            _title.Position = new Vector2(YnG.Width / 2 - _title.ScaledWidth / 2, 15);
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
