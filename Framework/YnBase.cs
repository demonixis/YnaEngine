using System;
using Microsoft.Xna.Framework;

namespace Yna.Framework
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
        protected bool _enabled;

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
            get { return _enabled && !_dirty; }
            set
            {
                _enabled = value;
                _dirty = !value;
            }
        }

        /// <summary>
        /// Pause or resume updates
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
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
                _enabled = !value;
            }
        }

        #endregion

        public YnBase()
        {
            _id = counterId++;
            _name = Id.ToString();
            _enabled = true;
            _dirty = false;
        }

        /// <summary>
        /// Update method called on each engine update
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);
    }
}
