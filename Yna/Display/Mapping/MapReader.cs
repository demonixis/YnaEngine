using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// TODO: replace this with the type you want to read.
using TRead = Yna.Display.Mapping.Map;

namespace Yna.Display.Mapping
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content
    /// Pipeline to read the specified data type from binary .xnb format.
    /// 
    /// Unlike the other Content Pipeline support classes, this should
    /// be a part of your main game project, and not the Content Pipeline
    /// Extension Library project.
    /// </summary>
    public class MapReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            existingInstance = new Map();
            existingInstance.Name = input.ReadString();
            existingInstance.Position = input.ReadVector2();
            existingInstance.Width = input.ReadInt32();
            existingInstance.Height = input.ReadInt16();
            existingInstance.Tiles = input.ReadObject<Tile[,]>();
            return existingInstance;
        }
    }
}
