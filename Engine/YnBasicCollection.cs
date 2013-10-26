// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;

namespace Yna.Engine
{
    /// <summary>
    /// Define a safe updateable list for YnBase objects
    /// </summary>
    public class YnBasicCollection : YnCollection<YnBasicEntity>
    {
        protected override void DoUpdate(GameTime gameTime)
        {
            for (int i = 0, l = _safeMembers.Count; i < l; i++)
            {
                if (_safeMembers[i].Enabled)
                    _safeMembers[i].Update(gameTime);
            }
        }
    }
}
