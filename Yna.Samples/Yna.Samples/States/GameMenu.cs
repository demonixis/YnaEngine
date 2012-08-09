using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.Display;
using Yna.Display.TiledMap._2D;
using Yna.Samples.Windows.States;
using Yna.State;

namespace Yna.Sample.States
{
    public class GameMenu : YnState
    {
        private YnText title;
        private YnGroup items;
        private int index;

        Sprite background;

        private int ItemsLength
        {
            get { return items.Count(); }
        }

        public GameMenu() 
            : base (1000f, 0)
        {
            background = new Sprite("Backgrounds/gradient");
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

            items.Add(new MenuItem(1, "Basic Sprites", "This sample shows you how to create a collection of sprite\nwithout texture.", true));
            items.Add(new MenuItem(2, "2D Plateformer", "An example that shows you how to easily create a fast\n2D Platformer with some physics (Acceleration and Velocity)."));
            items.Add(new MenuItem(3, "Animated Sprite", "How create an animated Sprite with a SpriteSheet ?\nThe answord in this sample"));
            items.Add(new MenuItem(4, "Simple Tiled Map", "Create a tilemap and a camera"));
            items.Add(new MenuItem(5, "Exit"));
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
                    case 0: YnG.SwitchState(new Sample01()); break;
                    case 1: YnG.SwitchState(new Sample02()); break;
                    case 2: YnG.SwitchState(new Sample03()); break;
                    case 3: YnG.SwitchState(new TiledMap2DSample()); break;
                    case 4: YnG.Exit(); break;
                    default: break;
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
        private int _position;

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
            _position = position;
            
            _label = new YnText("Fonts/MenuFont", new Vector2(coefX, offset + coefY * _position), Name);
            _label.Color = LabelColor;
            Add(_label);


            _description = new YnText("Fonts/MenuFont", new Vector2(coefX + 200, offset + coefY), description);
            _description.Color = Color.AliceBlue;
            _description.Visible = Selected;
            Add(_description);

            _position++;
        }
    }
}
