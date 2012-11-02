﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Content
{
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