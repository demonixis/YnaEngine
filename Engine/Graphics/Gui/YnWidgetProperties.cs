// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.Graphics.Gui
{
	/// <summary>
	/// This structure can be used to initialize a set of common
    /// properties an a widget. Only non-null values will be considered
    /// so you can use this structure even if you don't want to use 
    /// all properties.
	/// </summary>
	public struct YnWidgetProperties
	{
		/// <summary>
		/// X position (relative)
		/// </summary>
		public int? X;
		
		/// <summary>
		/// Y position (relative)
		/// </summary>
		public int? Y;
		
		/// <summary>
		/// The widget's width
		/// </summary>
		public int? Width;
		
		/// <summary>
		/// The widget's height
		/// </summary>
		public int? Height;

        /// <summary>
        /// The skin name
        /// </summary>
        public String SkinName;

        /// <summary>
        /// Set to true to use borders
        /// </summary>
        public bool? HasBorders;

        /// <summary>
        /// Set to true to use a background
        /// </summary>
        public bool? HasBackground;

        /// <summary>
        /// Widget padding value. Use may differ depending on widgets
        /// </summary>
        public int? Padding;
	}
}
