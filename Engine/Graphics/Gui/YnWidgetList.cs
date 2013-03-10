using System;
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
		/// <summary>
		/// Stores a flag indicating that one of the widgets was hovered during the update
		/// </summary>
		private bool _hovered;
		
		/// <summary>
		/// Return true if one of the widgets was hovered
		/// </summary>
		public bool Hovered
		{
			get { return _hovered; }
		}
		
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
	}
}
