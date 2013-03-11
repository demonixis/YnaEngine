using System;

namespace Yna.Engine.Graphics.Gui
{
	/// <summary>
	/// Define widget properties
	/// </summary>
	public struct YnWidgetProperties
	{
		/// <summary>
		/// X position (relative)
		/// </summary>
		public int X;
		
		/// <summary>
		/// Y position (relative)
		/// </summary>
		public int Y;
		
		/// <summary>
		/// The widget's width
		/// </summary>
		public int Width;
		
		/// <summary>
		/// The widget's height
		/// </summary>
		public int Height;

        /// <summary>
        /// The skin name
        /// </summary>
        public string SkinName;
	}
}
