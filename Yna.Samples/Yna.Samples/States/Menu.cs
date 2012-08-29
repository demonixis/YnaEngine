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
using Yna.Samples.States;

namespace Yna.Samples.States
{
    public class Menu : YnState
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

        public Menu() 
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
            items.Add(new MenuItem(2, "Animated Sprite", "Learn how to create an animated Sprite with a SpriteSheet"));
            items.Add(new MenuItem(3, "Simple Tiled Map", "Create a tilemap and a camera"));
            items.Add(new MenuItem(4, "Isometric Tiled Map", "Create an isometric tilemap and a camera"));
            items.Add(new MenuItem(9, "Exit"));

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
                case 0: YnG.SwitchState(new BasicSprites());      break;
                case 1: YnG.SwitchState(new AnimatedSprites());   break;
                case 2: YnG.SwitchState(new TiledMap2DSample());  break;
                case 3: YnG.SwitchState(new IsoTiledMapSample()); break;
                case 8: YnG.Exit();                               break;
                default:                                          break;
            }
        }
    }
}
