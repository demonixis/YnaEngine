using Microsoft.Xna.Framework;

namespace Yna.Engine
{
    /// <summary>
    /// Define a safe list for YnBase objects
    /// </summary>
    public class YnBasicCollection : YnCollection<YnBasicEntity>
    {
        protected override void DoUpdate(GameTime gameTime)
        {
            for (int i = 0; i < SafeMembersCount; i++)
            {
                if (_safeMembers[i].Enabled)
                    _safeMembers[i].Update(gameTime);
            }
        }
    }
}
