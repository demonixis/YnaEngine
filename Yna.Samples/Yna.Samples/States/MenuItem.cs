using System;
using Microsoft.Xna.Framework;
using Yna;
using Yna.Display;
using Yna.State;

namespace Yna.Samples.States
{
    class MenuItem : YnGroup
    {
        private const int coefX = 55;
        private const int coefY = 50;
        private const int offset = 60;

        private YnText _label;
        private YnText _description;
        private bool _selected;
        private int _itemPosition;

        public int ItemPosition
        {
            get { return _itemPosition; }
        }

        public YnText Label
        {
            get { return _label; }
        }

        public Color LabelColor
        {
            get
            {
                if (_selected)
                    return Color.GreenYellow;
                else
                    return Color.BlanchedAlmond;
            }
        }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                _label.Color = LabelColor;
                _description.Visible = _selected;
            }
        }

        public MenuItem(int position, string name, string description = "", bool selected = false)
        {
            Name = name;
            _selected = selected;
            _itemPosition = position;

            _label = new YnText("Fonts/MenuFont", new Vector2(coefX, offset + coefY * _itemPosition), Name);
            _label.Color = LabelColor;
            Add(_label);


            _description = new YnText("Fonts/MenuFont", new Vector2(coefX + 200, offset + coefY), description);
            _description.Color = Color.AliceBlue;
            _description.Visible = Selected;
            Add(_description);

            _itemPosition++;
        }
    }
}
