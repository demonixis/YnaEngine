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

namespace Yna.Samples3D.States
{
    public class GameMenu : YnState
    {
        private YnText title;
        private YnGroup items;
        private int index;

        YnSprite background;

        private int ItemsLength
        {
            get { return items.Count; }
        }

        public GameMenu()
            : base(1000f, 0)
        {
            background = new YnSprite("Backgrounds/gradient");
            Add(background);

            title = new YnText("Fonts/MenuFont", Vector2.Zero, "YNA Samples");
            title.Color = Color.DarkSlateBlue;
            Add(title);

            items = new YnGroup();
            Add(items);

            index = 0;
        }

        public override void Initialize()
        {
            base.Initialize();

            title.Scale = new Vector2(2.0f, 2.0f);
            title.CenterRelativeTo(YnG.Width, 100);

            background.Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);
            background.SourceRectangle = background.Rectangle;

            items.Add(new MenuItem(1, "Space Ship", "Loading an FBX space ship model and play with\nit with keyboard or gamepad", true));
            items.Add(new MenuItem(2, "Nasa Station", "Loading an FBX model from the Nasa web site.\nThis sample demonstrate how to use\nthe FPS Camera."));
            items.Add(new MenuItem(3, "Simple Terrain", "Creating a simple rectangular terrain with a texture"));
            items.Add(new MenuItem(4, "Heightmap", "Create a heightmap with 2 images file. The \nfirst for the geometry and the second\nfor the texture."));
            items.Add(new MenuItem(5, "Third Person", "Loading an object and setting up a third person camera who follow\nthe model and can rotate arround it."));
            items.Add(new MenuItem(6, "3rd with heightmap", "Using a ThirdPersonCamera for follow an object on a heightmap\nwith basic ground collide."));
            items.Add(new MenuItem(7, "Exit"));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Up) || YnG.Keys.JustPressed(Keys.Down))
            {
                if (YnG.Keys.JustPressed(Keys.Up))
                {
                    index--;

                    if (index < 0)
                        index = ItemsLength - 1;
                }
                else if (YnG.Keys.JustPressed(Keys.Down))
                {
                    index++;

                    if (index >= ItemsLength)
                        index = 0;
                }

                for (int i = 0; i < ItemsLength; i++)
                    (items[i] as MenuItem).Selected = false;

                (items[index] as MenuItem).Selected = true;
            }

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.Exit();

            if (YnG.Keys.JustPressed(Keys.Enter))
            {
                switch (index)
                {
                    case 0: YnG.SwitchState(new SpaceGame()); break;
                    case 1: YnG.SwitchState(new NasaSample()); break;
                    case 2: YnG.SwitchState(new SimpleTerrainSample()); break;
                    case 3: YnG.SwitchState(new HeightmapSample()); break;
                    case 4: YnG.SwitchState(new ThirdPersonSample()); break;
                    case 5: YnG.SwitchState(new ThirdPersonHeightmapSample()); break;
                    default: YnG.Exit(); break;
                }
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

        public MenuItem(int position, string name, string description = "", bool selected = false)
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