using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Helpers;

namespace Yna.Display.Gui
{
    /// <summary>
    /// The GUI manager as a Game Component
    /// </summary>
    public class YnGui : YnObject
    {
        #region Protected declarations

        protected YnSkin _skin;
        protected string _skinName;
        protected List<YnWidget> _widgets;
        private List<YnWidget> _safeWidgets;

        #endregion

        #region Properties

        /// <summary>
        /// The Gui current skin
        /// </summary>
        private YnSkin Skin 
        {
            get { return _skin; }
            set { _skin = value; }
        }

        /// <summary>
        /// The skin name
        /// </summary>
        public string SkinName
        {
            get { return _skinName; }
            set { _skinName = value; }
        }

        /// <summary>
        /// The widgets to handle
        /// </summary>
        public List<YnWidget> Widgets
        {
            get { return _widgets; }
            set { _widgets = value; }
        }

        #endregion

        public YnGui()
        {
            _widgets = new List<YnWidget>();
            _safeWidgets = new List<YnWidget>();
            _skin = null;
            _skinName = String.Empty;
        }

        public YnGui(YnSkin skin)
            : base()
        {
            _skin = skin;
        }

        #region GameState pattern

        public override void Initialize()
        {
            
        }

        public override  void LoadContent()
        {
            if (_skinName == String.Empty)
            {
                // No skin, use the default one
                // Skin = YnSkinGenerator.Generate(new Color(250, 100, 50)); // Orange
                //Skin = YnSkinGenerator.Generate(Color.Coral);
                // Skin = YnSkinGenerator.Generate(Color.DeepPink);
                Skin = YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/DefaultFont");
            }
            else
            {
                // TODO Load the skin
                // We must make a serializable class for a skin for an XML import with content manager
                // Or writing a custom pipeline importer (must be better)
                //Skin = Game.Content.Load<YnSkin>(SkinName);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_widgets.Count > 0)
            {
                _safeWidgets.Clear();
                _safeWidgets.AddRange(_widgets);

                foreach (YnWidget widget in _safeWidgets)
                    widget.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw the GUI
        /// </summary>
        /// <param name="gameTime">Time since last update</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_safeWidgets.Count > 0)
            {
                foreach (YnWidget widget in _safeWidgets)
                    widget.Draw(gameTime, spriteBatch, Skin);
            }
        }

        /// <summary>
        /// Draw the GUI with another batch
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void DrawGui(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a widget to the UI
        /// </summary>
        /// <typeparam name="W">The widget type</typeparam>
        /// <param name="widget">The widget</param>
        /// <returns>The widget added for ease</returns>
        public YnWidget Add(YnWidget widget)
        {
            _widgets.Add(widget);
            
            // If the skin is already loaded, link it directly
            if(_skin != null)
                widget.Skin = _skin;

            return widget;
        }

        /// <summary>
        /// Remove all widgets
        /// </summary>
        public void Clear()
        {
            _widgets.Clear();
        }

        public void Remove(YnWidget widget)
        {
            if (_widgets.Count > 0)
                _widgets.Remove(widget);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (YnWidget widget in Widgets)
                yield return widget;
        }

        #endregion

        public void PrepareWidgets()
        {
            // Initializes the skin for all added widgets
            foreach (YnWidget widget in _widgets)
            {
                widget.InitSkin(_skin);
            }

            // Do the layout for all widgets
            foreach (YnWidget widget in _widgets)
            {
                widget.Layout();
            }
        }

        public void SetSkin(YnSkin skin)
        {
            // Hide all components to stop rendering
            foreach (YnWidget widget in _widgets)
            {
                widget.Show(false);
            }

            // Set the new skin for all components
            _skin = skin;

            // Redo all widgets initializations
            PrepareWidgets();


            // Show all components back
            foreach (YnWidget widget in _widgets)
            {
                widget.Show(true);
            }
        }
    }

    /// <summary>
    /// Default skin with basic rendering
    /// </summary>
    internal class DefaultSkin : YnSkin
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
