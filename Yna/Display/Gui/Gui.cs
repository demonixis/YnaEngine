using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.Gui
{
    /// <summary>
    /// The GUI manager as a Game Component
    /// </summary>
    public class Gui : DrawableGameComponent
    {
        /// <summary>
        /// Specific GUI sprite batch
        /// </summary>
        private SpriteBatch GuiSpriteBatch { get; set; }

        /// <summary>
        /// The Gui current skin
        /// </summary>
        private YnSkin Skin { get; set; }

        /// <summary>
        /// The skin name
        /// </summary>
        public string SkinName { get; set; }

        /// <summary>
        /// The widgets to handle
        /// </summary>
        public List<YnWidget> Widgets{ get; set; }

        public Gui(Game game)
            : base(game)
        {
            // Add the GUI Manager to the game components
            Game.Components.Add(this);

            Widgets = new List<YnWidget>();
        }

        protected override void LoadContent()
        {
            // Create the specific Sprite Batch
            GuiSpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            if (SkinName == null)
            {
                // No skin, use the default one
                Skin = new DefaultSkin();
            }
            else
            {
                // TODO Load the skin
                //Skin = Game.Content.Load<YnSkin>(SkinName);
            }
        }

        /// <summary>
        /// Draw the GUI
        /// </summary>
        /// <param name="gameTime">Time since last update</param>
        public override void Draw(GameTime gameTime)
        {
            GuiSpriteBatch.Begin();

            foreach(YnWidget widget in Widgets)
            {
                widget.Draw(gameTime, GuiSpriteBatch, Skin);
            }

            GuiSpriteBatch.End();
        }

        /// <summary>
        /// Add a widget to the UI
        /// </summary>
        /// <typeparam name="W">The widget type</typeparam>
        /// <param name="widget">The widget</param>
        /// <returns>The widget added for ease</returns>
        public W Add<W>(W widget) where W : YnWidget
        {
            Widgets.Add(widget);
            return widget;
        }
    }

    /// <summary>
    /// Default skin
    /// </summary>
    class DefaultSkin : YnSkin
    {
        public DefaultSkin()
            : base()
        {
            FontName = "Fonts/DefaultFont";
            Font = YnG.Game.Content.Load<SpriteFont>(FontName);
            DefaultTextColor = Color.White;
        }
    }
}
