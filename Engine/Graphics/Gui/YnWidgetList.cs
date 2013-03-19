﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics.Gui.Widgets;

namespace Yna.Engine.Graphics.Gui
{
	/// <summary>
	/// Widget safe list for use in GUI.
	/// </summary>
	public class YnWidgetList : YnList<YnWidget>
    {
        #region Attributes

        /// <summary>
		/// Stores a flag indicating that one of the widgets was hovered during the update.
		/// </summary>
		private bool _hovered;

        #endregion

        #region Properties

        /// <summary>
		/// Return true if one of the widgets was hovered.
		/// </summary>
		public bool Hovered
		{
			get { return _hovered; }
		}

        #endregion

        /// <summary>
        /// Safely reset the member list.
        /// </summary>
        /// <param name="widgets"></param>
        public void SetMembers(YnWidget[] widgets)
        {
            _members.Clear();
            _members.AddRange(widgets);
        }

        /// <summary>
        /// See documentation in YnList.
        /// </summary>
		protected override void DoUpdate(GameTime gameTime, int count)
        {
			_hovered = false;
            for (int i = 0; i < count; i++)
            {
                if (_safeMembers[i].Enabled)
                {
                	_safeMembers[i].Update(gameTime);
                	_hovered |= _safeMembers[i].Hovered;
                }
            }
        }
		
        /// <summary>
        /// Draw the widgets.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			int count = _safeMembers.Count;
			for (int i = 0; i < count; i++)
            {
                if (_safeMembers[i].Enabled)
                {
                	_safeMembers[i].Draw(gameTime, spriteBatch);
                }
            }
		}

        /// <summary>
        /// Ease access to the Contains method on the inner member list.
        /// </summary>
        /// <param name="widget">The widget to search for</param>
        /// <returns>True if the widget is in the list</returns>
        public bool Contains(YnWidget widget)
        {
            return _members.Contains(widget);
        }
	}
}
