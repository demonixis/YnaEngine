using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.State;

namespace Yna.Samples3D.States
{
    public class GameMenu : YnState
    {
        private SpriteFont font;
        private string title;
        private List<MenuItem> items;
        private int index;
        private Texture2D background;

        private int ItemsLength
        {
            get { return items.Count; }
        }

        public GameMenu() 
            : base (1000f, 0)
        {
            title = "Game Menu";

            items = new List<MenuItem>();
			items.Add(new MenuItem("SpaceShip 3D", true));
            items.Add(new MenuItem("Nasa Sample"));
            items.Add(new MenuItem("Exit"));

            index = 0;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            background = YnG.Content.Load<Texture2D>("Backgrounds/gradient");

            font = YnG.Content.Load<SpriteFont>("Fonts/MenuFont");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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
                items[i].Selected = false;

            items[index].Selected = true;

            if (YnG.Keys.JustPressed(Keys.Enter))
            {
                switch (index)
                {
                    case 0: YnG.SwitchState(new SpaceGame()); break;
                    case 1: YnG.SwitchState(new NasaSample()); break;
                    case 2: YnG.Exit(); break;
                    default: break;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, YnG.Width, YnG.Height), Color.White);

            spriteBatch.DrawString(font, title, new Vector2(getMiddlePosition(title, 2.0f), 25), Color.AntiqueWhite, 0.0f, Vector2.Zero, new Vector2(2.0f, 2.0f), SpriteEffects.None, 1.0f);

            for (int i = 0; i < ItemsLength; i++)
            {
                spriteBatch.DrawString(font, items[i].Name, new Vector2(getMiddlePosition(items[i].Name, 1.5f), (i  * 65) + 125), items[i].Color, 0.0f, Vector2.Zero, new Vector2(1.5f, 1.5f), SpriteEffects.None, 1.0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private int getMiddlePosition(string text, float zoom = 1)
        {
            return (int)(YnG.Width / 2 - (font.MeasureString(text).X / 2) * zoom);
        }
    }

    class MenuItem
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
        public Color Color
        {
            get
            {
                if (Selected)
                    return Color.DarkBlue;
                else
                    return Color.LightCyan;
            }
        }

        public MenuItem(string name, bool selected = false)
        {
            Name = name;
            Selected = selected;
        }
    }
}
