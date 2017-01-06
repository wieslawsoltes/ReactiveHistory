// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveHistory
{
    /// <summary>
    /// Stack history extension methods for the generic list implementations.
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// Adds item to the source list with history.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="item">The item to add.</param>
        /// <param name="history">The history object.</param>
        public static void AddWithHistory<T>(this IList<T> source, T item, IHistory history)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (history == null)
                throw new ArgumentNullException(nameof(history));

            int index = source.Count;
            Action redo = () => source.Insert(index, item);
            Action undo = () => source.RemoveAt(index);
            history.Snapshot(undo, redo);
            redo.Invoke();
        }

        /// <summary>
        /// Inserts item to the source list with history.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="index">The item insertion index.</param>
        /// <param name="item">The item to insert.</param>
        /// <param name="history">The history object.</param>
        public static void InsertWithHistory<T>(this IList<T> source, int index, T item, IHistory history)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (index < 0)
                throw new IndexOutOfRangeException("Index can not be negative.");

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (history == null)
                throw new ArgumentNullException(nameof(history));

            Action redo = () => source.Insert(index, item);
            Action undo = () => source.RemoveAt(index);
            history.Snapshot(undo, redo);
            redo.Invoke();
        }

        /// <summary>
        /// Replaces item at specified index in the source list with history.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="index">The item index to replace.</param>
        /// <param name="item">The replaced item.</param>
        /// <param name="history">The history object.</param>
        public static void ReplaceWithHistory<T>(this IList<T> source, int index, T item, IHistory history)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (index < 0)
                throw new IndexOutOfRangeException("Index can not be negative.");

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (history == null)
                throw new ArgumentNullException(nameof(history));

            T oldValue = source[index];
            T newValue = item;
            Action redo = () => source[index] = newValue;
            Action undo = () => source[index] = oldValue;
            history.Snapshot(undo, redo);
            redo.Invoke();
        }

        /// <summary>
        /// Removes item at specified index from the source list with history.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="item">The item to remove.</param>
        /// <param name="history">The history object.</param>
        public static void RemoveWithHistory<T>(this IList<T> source, T item, IHistory history)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (history == null)
                throw new ArgumentNullException(nameof(history));

            int index = source.IndexOf(item);
            Action redo = () => source.RemoveAt(index);
            Action undo = () => source.Insert(index, item);
            history.Snapshot(undo, redo);
            redo.Invoke();
        }

        /// <summary>
        /// Removes item from the source list with history.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="index">The item index to remove.</param>
        /// <param name="history">The history object.</param>
        public static void RemoveWithHistory<T>(this IList<T> source, int index, IHistory history)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (index < 0)
                throw new IndexOutOfRangeException("Index can not be negative.");

            if (history == null)
                throw new ArgumentNullException(nameof(history));

            T item = source[index];
            Action redo = () => source.RemoveAt(index);
            Action undo = () => source.Insert(index, item);
            history.Snapshot(undo, redo);
            redo.Invoke();
        }

        /// <summary>
        /// Removes all items from the source list with history.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="history">The history object.</param>
        public static void ClearWithHistory<T>(this IList<T> source, IHistory history)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (history == null)
                throw new ArgumentNullException(nameof(history));

            if (source.Count > 0)
            {
                T[] items = source.ToArray();
                Action redo = () =>
                {
                    foreach (var item in items)
                    {
                        source.Remove(item);
                    }
                };
                Action undo = () =>
                {
                    foreach (var item in items)
                    {
                        source.Add(item);
                    }
                };

                history.Snapshot(undo, redo);
                redo.Invoke();
            }
        }
    }
}
