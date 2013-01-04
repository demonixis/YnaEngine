using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Helpers;

namespace Yna.Framework.Display.Gui
{
    /// <summary>
    /// The GUI manager
    /// </summary>
    public class YnGui : YnEntity
    {
        #region Protected declarations

        protected YnSkin _skin;
        protected string _skinName;
        protected List<YnWidget> _widgets;
        private List<YnWidget> _safeWidgets;
        protected bool _hovered;

        #endregion

        #region Properties

        public YnWidget this[int index]
        {
            get
            {
                if (index < 0 || index > Count)
                    throw new IndexOutOfRangeException();

                return _widgets[index];
            }
            set
            {
                if (index < 0 || index > Count)
                    throw new IndexOutOfRangeException();

                _widgets[index] = value;
            }
        }

        /// <summary>
        /// Gets the number of widgets
        /// </summary>
        public int Count
        {
            get { return _widgets.Count; }
        }

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

        /// <summary>
        /// Flag indicating if a gui component is currently hovered. Usefull to prevent other actions
        /// out of the UI
        /// </summary>
        public bool Hovered
        {
            get { return _hovered; }
            set { _hovered = value; }
        }
        #endregion

        public YnGui()
        {
            _widgets = new List<YnWidget>();
            _safeWidgets = new List<YnWidget>();
            _skinName = String.Empty;
        }

        public YnGui(YnSkin skin)
            : this()
        {
            _skin = skin;
        }

        #region GameState pattern

        /// <summary>
        /// Gamestate pattern : initialize
        /// </summary>
        public override void Initialize()
        {
            // Nothing!
        }

        /// <summary>
        /// Gamestate pattern : load content
        /// </summary>
        public override void LoadContent()
        {
            if (_skinName == String.Empty && _skin == null)
            {
                // No skin, generate the default one
                Skin = YnSkinGenerator.Generate(Color.DodgerBlue, null);
            }
            else
            {
                // TODO Load the skin for XML file
                // We must make a serializable class for a skin for an XML import with content manager
                // Or writing a custom pipeline importer (must be better)
                //_skin = Game.Content.Load<YnSkin>(_skinName);
            }
        }

        /// <summary>
        /// Gamestate pattern : update
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _hovered = false;

            if (_widgets.Count > 0)
            {
                _safeWidgets.Clear();
                _safeWidgets.AddRange(_widgets);

                foreach (YnWidget widget in _safeWidgets)
                {
                    widget.Update(gameTime);
                    _hovered |= widget.IsHovered;
                }
            }
        }

        /// <summary>
        /// Gamestate pattern : Draw the GUI
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_safeWidgets.Count > 0)
            {
                foreach (YnWidget widget in _safeWidgets)
                    widget.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Draw the GUI with another batch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch to use</param>
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
            if (widget.Skin == null && _skin != null)
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

        /// <summary>
        /// Get a widget by its name
        /// </summary>
        /// <param name="name">The widget name</param>
        /// <returns>A widget or null</returns>
        public virtual YnWidget GetWidgetByName(string name)
        {
            YnWidget widget = null;

            int size = Count;
            int i = 0;

            while (i < size && widget == null)
            {
                if (_widgets[i].Name == name)
                    widget = _widgets[i];
                i++;
            }

            return widget;
        }

        #endregion

        public void PrepareWidgets()
        {
            // Initializes the skin for all added widgets if not done yet
            foreach (YnWidget widget in _widgets)
            {
                if (widget.Skin == null)
                    widget.SetSkin(_skin);

                widget.InitSkin();
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

            // Set the new skin
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
}
