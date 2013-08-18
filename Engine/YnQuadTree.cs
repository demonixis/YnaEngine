// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Yna.Engine.Graphics;

namespace Yna.Engine
{
    /// <summary>
    /// A QuadTree class for an optimised collision detection
    /// </summary>
    public class YnQuadTree
    {
        /// <summary>
        /// Gets or sets the number of object per node
        /// </summary>
        public int MaxObjectsPerNode
        {
            get { return _maxObjectsPerNode; }
            set
            {
                if (value > 0)
                    _maxObjectsPerNode = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of sub level
        /// </summary>
        public int MaxLevels
        {
            get { return _maxSubLevels; }
            set
            {
                if (value > 0)
                    _maxSubLevels = value;
            }
        }

        private int _level;
        private int _maxObjectsPerNode;
        private int _maxSubLevels;
        private List<ICollidable2> _objects;
        private Rectangle _quadBounds;
        private YnQuadTree[] _nodes;

        #region delegates declarations

        public delegate void TestManyCallback(ICollidable2 colliderA, ICollidable2 colliderB);

        public delegate void TestOneCallback(ICollidable2 collidables);

        #endregion

        /// <summary>
        /// Create a new QuadTree
        /// </summary>
        /// <param name="level">Start level</param>
        /// <param name="quadBounds">The size to use for the QuadTree</param>
        public YnQuadTree(int level, Rectangle quadBounds)
        {
            _maxSubLevels = 5;
            _maxObjectsPerNode = 10;
            _level = level;
            _objects = new List<ICollidable2>();
            _quadBounds = quadBounds;
            _nodes = new YnQuadTree[4];

            for (int i = 0; i < 4; i++)
                _nodes[i] = null;
        }

        /// <summary>
        /// Clear the quadtree and add collidables objects on it
        /// </summary>
        /// <param name="collidables">An array of collidable objects</param>
        public void Begin(ICollidable2[] collidables)
        {
            Clear();
            foreach (ICollidable2 collidable in collidables)
                Add(collidable);
        }

        /// <summary>
        /// Gets all objects close to the collidable objects passed in parameter
        /// </summary>
        /// <param name="collidables"></param>
        /// <param name="functionTest"></param>
        /// <returns></returns>
        public bool TestCandidates(ICollidable2[] collidables, TestManyCallback testFunction)
        {
            bool hasCandidate = false;

            List<ICollidable2> candidates;
            foreach (ICollidable2 collidable in collidables)
            {
                candidates = GetCandidates(collidable);
                foreach (ICollidable2 candidate in candidates)
                {
                    testFunction(collidable, candidate);
                    hasCandidate = true;
                }
            }

            return hasCandidate;
        }

        public bool TestCandidates(ICollidable2 collidable, TestManyCallback functionTest)
        {
            return TestCandidates(new ICollidable2[] { collidable }, functionTest);
        }

        /// <summary>
        /// Gets all objects close to the collidable object passed in parameter
        /// </summary>
        /// <returns>True if objects are available to test otherwise return false</returns>
        /// <param name='colliderToTest'>The object to test with the collection</param>
        /// <param name='testFunction'>A callback function</param>
        public bool TestCandidates(ICollidable2 colliderToTest, TestOneCallback testFunction)
        {
            bool hasCandidate = false;

            List<ICollidable2> candidates = GetCandidates(colliderToTest);

            foreach (ICollidable2 candidate in candidates)
            {
                testFunction(candidate);
                hasCandidate = true;
            }

            return hasCandidate;
        }

        /// <summary>
        /// Get all that can potentially collide with this entity
        /// </summary>
        /// <param name="entity">A collidable entity to test</param>
        /// <returns>An array of collidable elements</returns>
        public List<ICollidable2> GetCandidates(ICollidable2 entity)
        {
            List<ICollidable2> candidates = new List<ICollidable2>();

            int index = GetNodeIndex(entity.Rectangle);

            // If the space is already splited we get node objects that can potentially collide with this entity
            if (index > -1 && _nodes[0] != null)
                candidates.AddRange(_nodes[index].GetCandidates(entity));

            // All remaining objects can potentially collide with this entity
            candidates.AddRange(_objects);

            return candidates;
        }

        /// <summary>
        /// Clear all nodes of the Quadtree
        /// </summary>
        public void Clear()
        {
            _objects.Clear();

            for (int i = 0; i < 4; i++)
            {
                if (_nodes[i] != null)
                {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Split the Quadtree
        /// </summary>
        protected void Split()
        {
            int subWidth = _quadBounds.Width / 2;
            int subHeight = _quadBounds.Height / 2;

            /* ----------------------- 
             * | _node[1] | _node[0] |
             * |----------------------
             * | _node[2] | _node[3] |
             * -----------------------
             */

            _nodes[0] = new YnQuadTree(_level + 1, new Rectangle(_quadBounds.X + subWidth, _quadBounds.Y, subWidth, subHeight));
            _nodes[1] = new YnQuadTree(_level + 1, new Rectangle(_quadBounds.X, _quadBounds.Y, subWidth, subHeight));
            _nodes[2] = new YnQuadTree(_level + 1, new Rectangle(_quadBounds.X, _quadBounds.Y + subHeight, subWidth, subHeight));
            _nodes[3] = new YnQuadTree(_level + 1, new Rectangle(_quadBounds.X + subWidth, _quadBounds.Y + subHeight, subWidth, subHeight));
        }

        /// <summary>
        /// Gets the node index of the object. 
        /// </summary>
        /// <param name="entityRectangle">Rectangle of the object</param>
        /// <returns>-1 if the object cannot completly fit whithin a child node then the node index between 0 and 3</returns>
        protected int GetNodeIndex(Rectangle entityRectangle)
        {
            int index = -1;

            float verticalMidPoint = _quadBounds.X + (_quadBounds.Width / 2);
            float horizontalMidPoint = _quadBounds.Y + (_quadBounds.Height / 2);

            // Object can  fit within the top/bottom quadrants
            bool topQuadrant = (entityRectangle.Y < horizontalMidPoint && entityRectangle.Y + entityRectangle.Height < horizontalMidPoint);
            bool bottomQuadrant = entityRectangle.Y > horizontalMidPoint;

            if (entityRectangle.X < verticalMidPoint && entityRectangle.X + entityRectangle.Width < verticalMidPoint)
            {
                if (topQuadrant)
                    index = 1;
                else if (bottomQuadrant)
                    index = 2;
            }
            else if (entityRectangle.X > verticalMidPoint)
            {
                if (topQuadrant)
                    index = 0;
                else if (bottomQuadrant)
                    index = 3;
            }

            return index;
        }

        /// <summary>
        /// Add a rectangle of an entity.
        /// </summary>
        /// <param name="entity">Rectangle of an entity</param>
        public void Add(ICollidable2 entity)
        {
            // If the Quadtree is already splited
            if (_nodes[0] != null)
            {
                int index = GetNodeIndex(entity.Rectangle);

                if (index > -1)
                {
                    _nodes[index].Add(entity);
                    return;
                }
            }

            _objects.Add(entity);

            // Split the space if we have too many objects. The limit is MaxLevels
            if (_objects.Count > _maxObjectsPerNode && _level < _maxSubLevels)
            {
                if (_nodes[0] == null)
                    Split();

                int i = 0;
                while (i < _objects.Count)
                {
                    int index = GetNodeIndex(_objects[i].Rectangle);
                    if (index > -1)
                    {
                        // Add the object to the correct node en remove it from the its parent
                        _nodes[index].Add(_objects[i]);
                        _objects.RemoveAt(i);
                    }
                    else
                        i++;
                }
            }
        }
    }
}
