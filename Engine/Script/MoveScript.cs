// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics;

namespace Yna.Engine.Script
{
    /// <summary>
    /// Moves the target YnObject to the given location at a specific speed
    /// </summary>
    public class MoveScript : BaseScriptNode
    {
        #region Attributes

        protected Vector2 _destination;
        protected float _speed;

        #endregion

        #region Properties

        /// <summary>
        /// The destination point
        /// </summary>
        public Vector2 Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        /// <summary>
        /// The speed used to reach the destination
        /// </summary>
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        #endregion

        /// <summary>
        /// Constructor with destination as Vector2
        /// </summary>
        /// <param name="destination">2D destination point</param>
        /// <param name="speed">The move speed</param>
        public MoveScript(Vector2 destination, float speed)
            : base()
        {
            _destination = destination;
            _speed = speed;
        }

        /// <summary>
        /// Constructor with destination as separate X/Y coordinates
        /// </summary>
        /// <param name="x">Destination point X coordinate</param>
        /// <param name="y">Destination point Y coordinate</param>
        /// <param name="speed">The move speed</param>
        public MoveScript(int x, int y, float speed)
            : this(new Vector2(x, y), speed)
        {
        }

        /// <summary>
        /// Moves the object until the target point is reached
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="o">The object to move</param>
        public override void Update(GameTime gameTime, YnEntity o)
        {
            Vector2 position = o.Position;

            if (position == _destination)
            {
                // The destination is reached by the object
                _scriptDone = true;
            }
            else
            {
                // Create the normalized vector aiming to the target point
                Vector2 direction = _destination - position;
                direction.Normalize();

                // Apply speed factor on the direction
                Vector2 newPosition = Vector2.Multiply(direction, _speed);

                // Move the object
                o.Translate(newPosition);
            }
        }
    }
}
