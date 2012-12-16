using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Framework.Event
{
    /// <summary>
    /// Event used when content manager start loading resources
    /// </summary>
    public class ContentLoadStartedEventArgs : EventArgs
    {
        public int Count { get; protected set; }

        public ContentLoadStartedEventArgs(int count)
        {
            Count = count;
        }
    }

    public class AssetLoadedEventArgs : EventArgs
    {
        public TimeSpan LoadingTime { get; protected set; }
        public string AssetName { get; protected set; }

        public AssetLoadedEventArgs(string assetName)
        {
            AssetName = assetName;
        }
    }

    /// <summary>
    /// Event used when content manager has finished loading resources
    /// </summary>
    public class ContentLoadDoneEventArgs : EventArgs
    {
        public TimeSpan LoadingTime { get; protected set; }
        public int Count { get; protected set; }

        public ContentLoadDoneEventArgs(TimeSpan loadingTime, int count)
        {
            LoadingTime = loadingTime;
            Count = count;
        }
    }
}
