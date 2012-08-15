using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.Display;
using Yna.State;

namespace Yna.Sample.States
{
    public class GameMenu : YnState
    {
        private YnText title;
        private YnGroup items;
        private int index;
        private bool itemClicked;

        YnSprite background;

        private int ItemsLength
        {
            get { return items.Count; }
        }

        public GameMenu() 
            : base (1000f, 0)
        {
            background = new YnSprite("Backgrounds/gradient");
            Add(background);

            title = new YnText("Fonts/MenuFont", Vector2.Zero, "YNA Samples");
            title.Color = Color.DarkSlateBlue;
            Add(title);

            items = new YnGroup();
            Add(items);

            itemClicked = false;

            YnG.ShowMouse = true;

            index = 0;
        }

        public override void Initialize()
        {
            base.Initialize();

            title.Scale = new Vector2(2.0f, 2.0f);
            title.CenterRelativeTo(YnG.Width, 100);

            background.Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);
            background.SourceRectangle = background.Rectangle;

            items.Add(new MenuItem(1, "Basic Sprites", "This sample shows you how to create a collection of sprite\nwithout texture.", true));
            items.Add(new MenuItem(2, "2D Plateformer", "An example that shows you how to easily create a fast\n2D Platformer with some physics (Acceleration and Velocity).\n\nIt show you how to using the Gamepad too"));
            items.Add(new MenuItem(3, "Animated Sprite", "How create an animated Sprite with a SpriteSheet ?\nThe answord in this sample"));
            items.Add(new MenuItem(4, "Simple Tiled Map", "Create a tilemap and a camera"));
            items.Add(new MenuItem(5, "Isometric Tiled Map", "Create an isometric tilemap and a camera"));
            items.Add(new MenuItem(6, "Exit"));

            foreach (MenuItem item in items)
            {
                item.Label.MouseOver += new EventHandler<Display.Event.MouseOverSpriteEventArgs>(Label_MouseOver);
                item.Label.MouseJustClicked += new EventHandler<Display.Event.MouseClickSpriteEventArgs>(Label_MouseJustClicked);
            }
        }

        void Label_MouseJustClicked(object sender, Display.Event.MouseClickSpriteEventArgs e)
        {
            // We are on an event callback
            // We can't switch to another state here because the update method of YnState is maybe updating
            // an object, so we can't break that
            itemClicked = true;
        }

        void Label_MouseOver(object sender, Display.Event.MouseOverSpriteEventArgs e)
        {
            YnText ynText = (sender as YnText);

            if (ynText != null)
            {
                MenuItem group = ynText.Parent as MenuItem;

                if (group != null)
                {
                    if (index != group.ItemPosition - 2)
                    {
                        for (int i = 0; i < ItemsLength; i++)
                            (items[i] as MenuItem).Selected = false;

                        group.Selected = true;

                        index = group.ItemPosition - 2;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Up) || YnG.Keys.JustPressed(Keys.Down) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.DPadUp) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.DPadDown))
            {
                if (YnG.Keys.JustPressed(Keys.Up) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.DPadUp))
                {
                    index--;

                    if (index < 0)
                        index = ItemsLength - 1;
                }
                else if (YnG.Keys.JustPressed(Keys.Down) || YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.DPadDown))
                {
                    index++;

                    if (index >= ItemsLength)
                        index = 0;
                }

                for (int i = 0; i < ItemsLength; i++)
                    (items[i] as MenuItem).Selected = false;

                (items[index] as MenuItem).Selected = true;
            }

            if (YnG.Keys.JustPressed(Keys.Enter) || itemClicked)
            {
                itemClicked = false;
                LaunchDemo();
            }

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.Exit();
        }

        private void LaunchDemo()
        {
            switch (index)
            {
                case 0:
                    YnG.SwitchState(new Sample01());
                    break;
                case 1:
                    YnG.SwitchState(new Sample02());
                    break;
                case 2:
                    YnG.SwitchState(new Sample03());
                    break;
                case 3:
                    YnG.SwitchState(new TiledMap2DSample());
                    break;
                case 4:
                    YnG.SwitchState(new IsoTiledMapSample());
                    break;
                case 5:
                    YnG.Exit();
                    break;
                default:
                    break;
            }
        }
    }

    class MenuItem : YnGroup
    {
        private const int coefX = 55;
        private const int coefY = 50;
        private const int offset = 60;

        private YnText _label;
        private YnText _description;
        private bool _selected;
        private int _itemPosition;

        public int ItemPosition
        {
            get { return _itemPosition; }
        }

        public YnText Label
        {
            get { return _label; }
        }

        public Color LabelColor
        {
            get
            {
                if (_selected)
                    return Color.GreenYellow;
                else
                    return Color.BlanchedAlmond;
            }
        }

        public bool Selected 
        {
            get { return _selected; }
            set
            {
                _selected = value;
                _label.Color = LabelColor;
                _description.Visible = _selected;
            }
        }

        public MenuItem(int position, string name, string description = "",  bool selected = false)
        {
            Name = name;
            _selected = selected;
            _itemPosition = position;
            
            _label = new YnText("Fonts/MenuFont", new Vector2(coefX, offset + coefY * _itemPosition), Name);
            _label.Color = LabelColor;
            Add(_label);


            _description = new YnText("Fonts/MenuFont", new Vector2(coefX + 200, offset + coefY), description);
            _description.Color = Color.AliceBlue;
            _description.Visible = Selected;
            Add(_description);

            _itemPosition++;
        }
    }
}
