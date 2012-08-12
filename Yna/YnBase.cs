using System;
using Microsoft.Xna.Framework;
namespace Yna
{
    public abstract class YnBase
    {
        private static uint counterId = 0x0001;

        public uint Id { get; set; }
        public string Name { get; set; }

        protected bool _dirty;
        protected bool _paused;

        public bool Active
        {
            get { return !_paused && !_dirty; }
            set
            {
                if (value)
                {
                    _paused = false;
                    _dirty = false;
                }
                else
                {
                    _paused = true;
                    _dirty = false;
                }
            }
        }

        /// <summary>
        /// Active ou désactive la pause (Influance la mise à jour)
        /// </summary>
        public bool Pause
        {
            get { return _paused; }
            set { _paused = value; }
        }

        /// <summary>
        /// Indique si l'objet doit être détruit totalement
        /// </summary>
        public bool Dirty
        {
            get { return _dirty; }
            set
            {
                _dirty = value;

                if (value)
                {
                    _paused = true;
                }
                else
                {
                    _paused = false;
                }
            }
        }

        public YnBase()
        {
            Id = counterId++;
            Name = Id.ToString();
            _paused = false;
            _dirty = false;
        }

        public abstract void Update(GameTime gameTime);
    }
}
