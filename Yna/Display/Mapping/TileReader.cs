using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// TODO: replace this with the type you want to read.
using TRead = Yna.Display.Mapping.Tile;

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
    public class TileReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            existingInstance.Position = input.ReadVector2();
            existingInstance.Rectangle = input.ReadRawObject<Rectangle>();
            existingInstance.TextureName = input.ReadString();
            existingInstance.Scale = input.ReadVector2();
            return existingInstance;
        }
    }
}
