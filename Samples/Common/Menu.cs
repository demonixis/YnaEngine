using Yna.Framework.Display;
using Yna.Framework.Display.Gui;

namespace Yna.Samples
{
    public abstract class Menu : YnState
    {
        private YnGui _gui;
        private YnLabel _tooltip;

        public Menu(string name, string [] items)
            : base(name, 1000f, 0)
        {

        }
    }
}
