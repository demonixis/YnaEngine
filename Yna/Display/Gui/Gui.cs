using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Helpers;

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

            // Initializes the skin for all added widgets
            foreach(YnWidget widget in Widgets)
            {
                widget.InitSkin(Skin);
            }


            // Do the layout for all widgets
            foreach (YnWidget widget in Widgets)
            {
                widget.Layout();
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
            // If the skin is already loaded, link it directly
            if(Skin != null)
            {
                widget.Skin = Skin;
            }
            return widget;
        }
    }

    /// <summary>
    /// Default skin with basic rendering
    /// </summary>
    class DefaultSkin : YnSkin
    {
        /// <summary>
        /// The constructor of the default skin builds all textures from scratch.
        /// Must be used after Content Manager has been started
        /// </summary>
        public DefaultSkin()
            : base()
        {
            // Default font
            FontName = "Fonts/DefaultFont";
            Font = YnG.Game.Content.Load<SpriteFont>(FontName);
            DefaultTextColor = Color.White;

            // Default borders
            const int borderSize = 2;
            Color borderColor = Color.LightGray;
            BoxBorder = new YnBorder();
            BoxBorder.TopLeft     = GraphicsHelper.CreateTexture(borderColor, borderSize, borderSize);
            BoxBorder.Top         = GraphicsHelper.CreateTexture(borderColor, 1, borderSize);
            BoxBorder.TopRight    = GraphicsHelper.CreateTexture(borderColor, borderSize, borderSize);
            BoxBorder.Right       = GraphicsHelper.CreateTexture(borderColor, borderSize, 1);
            BoxBorder.BottomRight = GraphicsHelper.CreateTexture(borderColor, borderSize, borderSize);
            BoxBorder.Bottom      = GraphicsHelper.CreateTexture(borderColor, 1, borderSize);
            BoxBorder.BottomLeft  = GraphicsHelper.CreateTexture(borderColor, borderSize, borderSize);
            BoxBorder.Left        = GraphicsHelper.CreateTexture(borderColor, borderSize, 1);

            // Default background
            BoxBackground = GraphicsHelper.CreateTexture(Color.DarkGray, 1, 1);
        }
    }
}
