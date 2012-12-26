﻿using System;
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

        protected uint _id;
        protected string _name;
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
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// Pause or resume updates
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        #endregion

        public YnBase()
        {
            _id = counterId++;
            _name = String.Format("YnBase_{0}", Id.ToString());
            _enabled = true;
        }

        /// <summary>
        /// Update method called on each engine update
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);
    }
}
