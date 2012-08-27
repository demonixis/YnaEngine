using System;
using Microsoft.Xna.Framework;
namespace Yna
{
    /// <summary>
    /// Base class for all object on the Framework
    /// </summary>
    public abstract class YnBase
    {
        #region private declarations

        private static uint counterId = 0x0001;

        private uint _id;
        private string _name;
        protected bool _dirty;
        protected bool _paused;

        #endregion

        #region Properties

        /// <summary>
        /// Get the unique identification code of this object
        /// </summary>
        public uint Id 
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Get or Set the name of this object
        /// </summary>
        public string Name 
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Active or Desactive this object
        /// </summary>
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
        /// Pause or resume updates
        /// </summary>
        public bool Pause
        {
            get { return _paused; }
            set { _paused = value; }
        }

        /// <summary>
        /// Flags who determine if this object must be cleaned and removed
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

        #endregion

        public YnBase()
        {
            _id = counterId++;
            _name = Id.ToString();
            _paused = false;
            _dirty = false;
        }

        public abstract void Update(GameTime gameTime);
    }
}
