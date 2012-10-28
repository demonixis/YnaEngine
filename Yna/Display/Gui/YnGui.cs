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
    public class YnGui : DrawableGameComponent
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

        public YnGui(Game game)
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
                // Skin = YnSkinGenerator.Generate(new Color(250, 100, 50)); // Orange
                //Skin = YnSkinGenerator.Generate(Color.Coral);
                // Skin = YnSkinGenerator.Generate(Color.DeepPink);
                Skin = YnSkinGenerator.Generate(Color.DodgerBlue);
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

            List<YnWidget> safeList = new List<YnWidget>(Widgets);
            foreach (YnWidget widget in safeList)
            {
                widget.Draw(gameTime, GuiSpriteBatch, Skin);
            }

            GuiSpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            List<YnWidget> safeList = new List<YnWidget>(Widgets);
            foreach (YnWidget widget in safeList)
            {
                widget.Update(gameTime);
            }
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

        public void Clear()
        {
            Widgets.Clear();
        }

        public void PrepareWidgets()
        {
            // Initializes the skin for all added widgets
            foreach (YnWidget widget in Widgets)
            {
                widget.InitSkin(Skin);
            }

            // Do the layout for all widgets
            foreach (YnWidget widget in Widgets)
            {
                widget.Layout();
            }
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
            Color transparentColor = new Color(0, 0, 0, 0);
            const int borderSize = 1;
            Color borderColor = Color.Gray;
            PanelBorder = new YnBorder();
            PanelBorder.TopLeft     = GraphicsHelper.CreateTexture(transparentColor, borderSize, borderSize);
            PanelBorder.Top         = GraphicsHelper.CreateTexture(borderColor, 1, borderSize);
            PanelBorder.TopRight    = GraphicsHelper.CreateTexture(transparentColor, borderSize, borderSize);
            PanelBorder.Right       = GraphicsHelper.CreateTexture(borderColor, borderSize, 1);
            PanelBorder.BottomRight = GraphicsHelper.CreateTexture(transparentColor, borderSize, borderSize);
            PanelBorder.Bottom      = GraphicsHelper.CreateTexture(borderColor, 1, borderSize);
            PanelBorder.BottomLeft  = GraphicsHelper.CreateTexture(transparentColor, borderSize, borderSize);
            PanelBorder.Left        = GraphicsHelper.CreateTexture(borderColor, borderSize, 1);

            // Default background
            /*
            BoxBackground = GraphicsHelper.CreateTexture(Color.DarkGray, 1, 1);

            // Hovered borders
            borderColor = Color.White;
            HoveredBoxBorder = new YnBorder();
            HoveredBoxBorder.TopLeft     = GraphicsHelper.CreateTexture(transparentColor, borderSize, borderSize);
            HoveredBoxBorder.Top         = GraphicsHelper.CreateTexture(borderColor, 1, borderSize);
            HoveredBoxBorder.TopRight    = GraphicsHelper.CreateTexture(transparentColor, borderSize, borderSize);
            HoveredBoxBorder.Right       = GraphicsHelper.CreateTexture(borderColor, borderSize, 1);
            HoveredBoxBorder.BottomRight = GraphicsHelper.CreateTexture(transparentColor, borderSize, borderSize);
            HoveredBoxBorder.Bottom      = GraphicsHelper.CreateTexture(borderColor, 1, borderSize);
            HoveredBoxBorder.BottomLeft  = GraphicsHelper.CreateTexture(transparentColor, borderSize, borderSize);
            HoveredBoxBorder.Left        = GraphicsHelper.CreateTexture(borderColor, borderSize, 1);

            // Hovered background
            HoveredBackground = GraphicsHelper.CreateTexture(new Color(100, 100, 100), 1, 1);
             * */
        }
    }
}
