// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.Content
{
    /// <summary>
    /// Event used when an asset is successfully loaded from content manager
    /// </summary>
    public class AssetLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// Loading time
        /// </summary>
        public long LoadingTime { get; protected set; }
        
        /// <summary>
        /// The asset name
        /// </summary>
        public string AssetName { get; protected set; }

        public AssetLoadedEventArgs(string assetName)
        {
            AssetName = assetName;
        }

        public AssetLoadedEventArgs(string assetName, long loadingTime)
        {
            AssetName = assetName;
            LoadingTime = loadingTime;
        }
    }

    /// <summary>
    /// Event used when content manager start loading resources
    /// </summary>
    public class ContentLoadStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Number of objects to load
        /// </summary>
        public int Count { get; protected set; }

        public ContentLoadStartedEventArgs()
        {
            Count = 0;
        }

        public ContentLoadStartedEventArgs(int count)
        {
            Count = count;
        }
    }

    /// <summary>
    /// Event used when content manager has finished to load resources
    /// </summary>
    public class ContentLoadFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Loading time
        /// </summary>
        public long LoadingTime { get; protected set; }
        
        /// <summary>
        /// Number of objects loaded
        /// </summary>
        public int Count { get; protected set; }

        public ContentLoadFinishedEventArgs()
        {
            LoadingTime = 0;
            Count = 0;
        }

        public ContentLoadFinishedEventArgs(long loadingTime, int count)
        {
            LoadingTime = loadingTime;
            Count = count;
        }
    }
}
