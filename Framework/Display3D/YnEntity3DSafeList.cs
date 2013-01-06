using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display.Gui;


namespace Yna.Framework.Display3D
{
    /// <summary>
    /// Define a safe list for 3D objects
    /// </summary>
    public class YnEntity3DSafeList : YnSafeList<YnEntity3D>
    {
        /// <summary>
        /// Update enabled objects
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void DoUpdate(GameTime gameTime)
        {
            for (int i = 0; i < _listSize; i++)
            {
                if (_members[i].Enabled)
                    _members[i].Update(gameTime);
            }
        }

        protected virtual void DoDraw(GraphicsDevice device, int memberIndex)
        {
            if(_safeMembers[memberIndex].Visible)
                _safeMembers[memberIndex].Draw(device);
        }

        /// <summary>
        /// Draw the visible objects
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GraphicsDevice device)
        {
            // TODO replace that by a SceneGraph.Draw() [lights, Shadows, Meshs]
            if (_listSize > 0)
            {
                for (int i = 0; i < _listSize; i++)
                {
                    DoDraw(device, i);
                }
            }
        }
    }
}
