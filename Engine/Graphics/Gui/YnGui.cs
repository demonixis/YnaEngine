// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
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
    	/// Loads a skin directly from an asset file.
    	/// </summary>
    	/// <param name="name">The skin resource file</param>
    	public static void LoadSkin(string asset)
    	{
    		// TODO Load the skin from XML file or whatever
    	}
    	
    	#endregion
    	
        #region Attributes

        /// <summary>
        /// The widget list. This list contains only widgets added directly to the GUI. Children
        /// of those widgets will not be in this list.
        /// </summary>
        protected YnWidgetList _widgets;
        
        /// <summary>
        /// The default's GUI skin name. If a widget is added to the GUI without 
        /// a specific skin, this one will be used.
        /// </summary>
        protected string _skinName;

        /// <summary>
        /// This is the cuirrent max widget deppth value. This number is used for dept management.
        /// </summary>
        private int _maxDepth;

        #endregion

        #region Properties

        /// <summary>
        /// The skin name.
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
        /// Return true if at least one widget was added to the GUI.
        /// </summary>
        public bool HasWidgets
        {
        	get { return _widgets.Count > 0; }
        }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public YnGui()
        {
        	_widgets = new YnWidgetList();
            _skinName = DEFAULT_SKIN; // The default skin

            // The max depth is initialized at -1 for the first widget added to have a depth of 0
            _maxDepth = -1;
        }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Gamestate pattern : load content.
        /// </summary>
        public override void LoadContent()
        {
        	// Generate the default skin
        	// FIXME : load a stored default skin file instead of generating one with a spritefont asset
        	RegisterSkin("default", YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/DefaultFont"));
        }

        /// <summary>
        /// Gamestate pattern : update.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            // Update the widgets
            _widgets.Update(gameTime);
            
            // Update the GUI hover flag
            _hovered = _widgets.Hovered;
        }

        /// <summary>
        /// Gamestate pattern : Draw the GUI.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        	_widgets.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Draw the GUI in a new spriteBatch pipe.
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
        /// Add a widget to the UI.
        /// </summary>
        /// <param name="widget">The widget</param>
        /// <returns>The widget added for ease</returns>
        public YnWidget Add(YnWidget widget)
        {
            // First, test if the widget is not already added
            if(!_widgets.Contains(widget))
            {
                // Increase the max depth and set it up on the added widget
                _maxDepth++;
                widget.Depth = _maxDepth;

                // Apply skin
                widget.ApplySkin();

                // Add the widget to the list
                _widgets.Add(widget);

                // If the widget has no skin, then use the default one
                // The skin name is defined in default widgets constructors but this 
                // is a double security
                if (widget.SkinName == null)
                {
                    widget.SkinName = DEFAULT_SKIN;
                }
            }


            return widget;
        }

        /// <summary>
        /// Remove all widgets.
        /// </summary>
        public void Clear()
        {
            _widgets.Clear(); // Clear the list
            _maxDepth = 0; // Reset the max depth
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

        #region Widget depth management

        /// <summary>
        /// Swap the depth of the two widgets.
        /// </summary>
        /// <param name="widget1">The first widget</param>
        /// <param name="widget2">The second widget</param>
        public void DepthSwap(YnWidget widget1, YnWidget widget2)
        {
            // The widget's depth is also it's index in the widget list : magic!
            // No need to make a temp value as we have the references here with
            // widget1 and widget2
            _widgets[widget1.Depth] = widget2;
            _widgets[widget2.Depth] = widget1;

            // Swap depths values in wigets
            int firstDepth = widget1.Depth;
            widget1.Depth = widget2.Depth;
            widget2.Depth = firstDepth;
        }

        /// <summary>
        /// Move the widget up in the layer depth. If the widget is already on top of
        /// the layer, nothing is done.
        /// </summary>
        /// <param name="widget">The widget to move up</param>
        public void DepthUp(YnWidget widget)
        {
            if(widget.Depth < _maxDepth)
            {
                // The widget is not already on top of the layer
                // Swap the widget with the one just above
                DepthSwap(widget, _widgets[widget.Depth + 1]);
            }
        }

        /// <summary>
        /// Move the widget down in the layer depth. If the widget is already on the bottom of
        /// the layer, nothing is done.
        /// </summary>
        /// <param name="widget">The widget to mode down</param>
        public void DepthDown(YnWidget widget)
        {
            if(widget.Depth > 0)
            {
                // The widget is not already on the bottom of the layer
                // Swap the widget with the one just below
                DepthSwap(widget, _widgets[widget.Depth - 1]);
            }
        }

        /// <summary>
        /// Move the widget to the bottom of the layer depth. All widgets below are moved up.
        /// </summary>
        /// <param name="widget">The widget to move to the bottom</param>
        public void DepthToBottom(YnWidget widget)
        {
            // All widgets below the widget have to go up in the depth
            // Depth - 1 => Depth
            // Depth - 2 => Depth - 1
            // and so on...
            for (int i = widget.Depth -1; i >= 0; i--)
            {
                DepthUp(_widgets[i]);
            }

            // Now that all other widgets were moved up, just set the widget
            // new depth
            widget.Depth = 0;
            _widgets[0] = widget;
        }

        /// <summary>
        /// Move the widget to the top of the lauer depth. All widgets above are moved down.
        /// </summary>
        /// <param name="widget">The widget to move to the top</param>
        public void DepthToTop(YnWidget widget)
        {
            // All widgets above the widget have to go down in the depth
            // Depth + 1 => Depth
            // Depth + 2 => Depth + 1
            // and so on...
            for (int i = widget.Depth +1; i <= _maxDepth; i++)
            {
                DepthDown(_widgets[i]);
            }

            // Now that all other widgets were moved up, just set the widget
            // new depth
            widget.Depth = _maxDepth;
            _widgets[_maxDepth] = widget;
        }

        #endregion
    }
}
