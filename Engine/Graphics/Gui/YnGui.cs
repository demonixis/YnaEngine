using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics.Gui.Widgets;
using Yna.Engine.Helpers;

namespace Yna.Engine.Graphics.Gui
{
    /// <summary>
    /// This class is a manager for all UI widgets. It can be used as a simple manager handling widgets and 
    /// as a layer for global rendering.
    /// </summary>
    public class YnGui : YnEntity
    {
    	#region Static skin storage
    	
    	public const string DEFAULT_SKIN = "default";
    	
    	/// <summary>
    	/// This dictionary contains all registered GUI skins. They're identified by a name.
    	/// </summary>
    	private static Dictionary<string, YnSkin> __skinCache = new Dictionary<string, YnSkin>();
    	
    	/// <summary>
    	/// Retreive a skin definition from the static cache.
    	/// </summary>
    	/// <param name="name">The skin name</param>
    	/// <returns>the skin</returns>
    	public static YnSkin GetSkin(string name)
    	{
    		if(!__skinCache.ContainsKey(name))
    		{
    			// The skin was not found. Use the default one instead
    			// TODO Add warning log
    			__skinCache[name] = __skinCache[DEFAULT_SKIN];
    			
    			return __skinCache[DEFAULT_SKIN];
    		}
    		return __skinCache[name];
    	}
    	
    	/// <summary>
    	/// Register a new skin in the cache. Be carefull when registering a new skin : names must
    	/// be unique. 
    	/// </summary>
    	/// <param name="name">The skin name</param>
    	/// <param name="skin">The skin definition</param>
    	public static void RegisterSkin(string name, YnSkin skin)
    	{
    		if(__skinCache.ContainsKey(name))
    		{
    			// Skin overwriting
    			// TODO Add warning log
    		}
    		__skinCache[name] = skin;
    	}
    	
    	/// <summary>
    	/// Loads a skin directly from an asset file
    	/// </summary>
    	/// <param name="name">The skin resource file</param>
    	public static void LoadSkin(string asset)
    	{
    		// TODO Load the skin from XML file or whatever
    	}
    	
    	#endregion
    	
        #region Protected declarations

        /// <summary>
        /// The widget list
        /// </summary>
        protected YnWidgetList _widgets;
        
        /// <summary>
        /// The default's GUI skin name. If a widget is added to the GUI without 
        /// a specific skin, this one will be used
        /// </summary>
        protected string _skinName;
        
        /// <summary>
        /// Store a flag indicating that the GUI is currently hovered. May be usefull to prevent
        /// other input handling
        /// </summary>
        protected bool _hovered;
        
        #endregion

        #region Properties

        /// <summary>
        /// The skin name
        /// </summary>
        public string SkinName
        {
            get { return _skinName; }
            set { _skinName = value; }
        }

        /// <summary>
        /// The widgets currently registered in the GUI. Note that children of widgets 
        /// added in the GUI will bot appear here.
        /// </summary>
        public YnWidgetList Widgets
        {
            get { return _widgets; }
            set { _widgets = value; }
        }

        /// <summary>
        /// Flag indicating if a GUI component is currently hovered. Usefull to prevent other actions
        /// out of the UI
        /// </summary>
        public bool Hovered
        {
            get { return _hovered; }
            set { _hovered = value; }
        }
        
        /// <summary>
        /// Return true if at least one widget was added to the GUI
        /// </summary>
        public bool HasWidgets
        {
        	get { return _widgets.Count > 0; }
        }
        
        #endregion

        public YnGui()
        {
        	_widgets = new YnWidgetList();
            _skinName = DEFAULT_SKIN; // The default skin
        }

        #region GameState pattern

        /// <summary>
        /// Gamestate pattern : load content
        /// </summary>
        public override void LoadContent()
        {
        	// Generate the default skin
        	// FIXME : load a stored default skin file instead of generating one with a spritefont asset
        	RegisterSkin("default", YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/Font"));
        }

        /// <summary>
        /// Gamestate pattern : update
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            // Reset the global hover flag
            _hovered = false;

            // Update the widgets
            _widgets.Update(gameTime);
            
            // Update the hover flag
            _hovered = _widgets.Hovered;
        }

        /// <summary>
        /// Gamestate pattern : Draw the GUI
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        	_widgets.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Draw the GUI in a new spriteBatch pipe
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
        /// <param name="widget">The widget</param>
        /// <returns>The widget added for ease</returns>
        public YnWidget Add(YnWidget widget)
        {
            _widgets.Add(widget);

            // If the widget has no skin, then use the default one
            if(widget.SkinName == null)
            {
            	widget.SkinName = DEFAULT_SKIN;
            }

            return widget;
        }

        /// <summary>
        /// Remove all widgets.
        /// </summary>
        public void Clear()
        {
            _widgets.Clear();
        }

        /// <summary>
        /// Removes a widget from the GUI. Only widgets added directly in the GUI can be removed by this method.
        /// </summary>
        /// <param name="widget">The widget to remove</param>
        public void Remove(YnWidget widget)
        {
        	// TODO : make possible to remove any hierarchy level widget with this method
            if (_widgets.Count > 0)
                _widgets.Remove(widget);
        }

        /// <summary>
        /// Get a widget by its name.
        /// </summary>
        /// <param name="name">The widget name</param>
        /// <returns>A widget or null if the name was not found</returns>
        public virtual YnWidget GetWidgetByName(string name)
        {
            YnWidget widget = null;

            int size = _widgets.Count;
            int i = 0;

            while (i < size && widget == null)
            {
                if (_widgets[i].Name == name)
                {
                	widget = _widgets[i];
                }
                i++;
            }

            return widget;
        }

        #endregion
    }
}
