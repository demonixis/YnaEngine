using System;
using System.Collections.Generic;

namespace Yna.Utils
{
    /// <summary>
    /// An hybrid collection who work like a Stack with the advantages
    /// of a List. It's a List object with 3 extension methods who simulates a Stack
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListStack<T> : List<T>
    {
        /// <summary>
        /// Add an item to the end of the collection
        /// </summary>
        /// <param name="item">An instance of T</param>
        public void Push(T item)
        {
            Add(item);
        }

        /// <summary>
        /// Get the last item of the list. This object is removed from the collection
        /// </summary>
        /// <returns>The last item of the list</returns>
        public T Pop()
        {
            int size = this.Count;

            if (size == 0)
                throw new InvalidOperationException("The list is empty");

            int lastIndex = size - 1;

            // Get the last item
            T result = this[lastIndex];

            // Remove the last item
            RemoveAt(lastIndex);

            return result;
        }

        /// <summary>
        /// Return the last object of the collection without remove it from the collection
        /// </summary>
        /// <returns>The last object of the collection</returns>
        public T Peek()
        {
            int size = this.Count;

            if (size == 0)
                throw new InvalidOperationException("The list is empty");

            int lastIndex = size - 1;

            return this[lastIndex];
        }
    }
}
