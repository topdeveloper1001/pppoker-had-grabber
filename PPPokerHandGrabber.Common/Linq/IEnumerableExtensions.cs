//-----------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PPPokerHandGrabber.Common.Linq
{
    public static class CollectionExtensions
    {
        public static T SingleOrDefaultNoThrow<T>(this IEnumerable<T> source)
            where T : class
        {
            return source == null || source.Count() != 1 ? default(T) : source.First();
        }

        public static T SingleOrDefaultNoThrow<T>(this IEnumerable<T> source, Func<T, bool> predicate)
            where T : class
        {
            return source == null ? default(T) : source.Where(predicate).SingleOrDefaultNoThrow();
        }

        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> roots, Func<T, IEnumerable<T>> childrenSelector)
        {
            return roots.SelectMany(e => Traverse(e, childrenSelector));
        }

        public static IEnumerable<T> Traverse<T>(T root, Func<T, IEnumerable<T>> childrenSelector)
        {
            yield return root;

            foreach (var subItem in childrenSelector(root).Traverse(childrenSelector))
            {
                yield return subItem;
            }
        }

        public static bool EqualsToArray<T>(this T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2)) return true;
            if (a1 == null || a2 == null) return false;
            if (a1.Length != a2.Length) return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }

            return true;
        }

        public static IEnumerable Append(this IEnumerable first, params object[] second)
        {
            return first.OfType<object>().Concat(second);
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> first, params T[] second)
        {
            return first.Concat(second);
        }

        public static IEnumerable Prepend(this IEnumerable first, params object[] second)
        {
            return second.Concat(first.OfType<object>());
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> first, params T[] second)
        {
            return second.Concat(first);
        }

        public static void AddRange(this IList list, IEnumerable listToAdd)
        {
            foreach (object item in listToAdd)
            {
                list.Add(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> listToAdd)
        {
            foreach (T item in listToAdd)
            {
                list.Add(item);
            }
        }

        public static void RemoveRange<T>(this ICollection<T> list, IEnumerable<T> items)
        {
            items.ForEach(item => list.Remove(item));
        }

        public static void RemoveRange(this IList list, IEnumerable items)
        {
            items.ForEach(list.Remove);
        }

        public static void ForEach(this IEnumerable enumerable, Action<object> action)
        {
            foreach (var item in enumerable)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T t in enumerable)
                action(t);
        }

        public static bool Replace<T>(this IList<T> list, T oldItem, T newItem, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < list.Count; i++)
            {
                if (!comparer.Equals(oldItem, list[i])) continue;
                list[i] = newItem;
                return true;
            }

            return false;
        }

        public static void Replace(this IList list, IEnumerable items)
        {
            list.Clear();
            list.AddRange(items);
        }

        public static void Replace<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            collection.AddRange(items);
        }

        /// <summary>
        /// Create a single string from a sequence of items, separated by the provided <paramref name="separator"/>,
        /// and with the conversion to string done by the given <paramref name="converter"/>.
        /// </summary>
        /// <remarks>This method does basically the same thing as <see cref="string.Join(string,string[])"/>,
        /// but will work on any sequence of items, not just arrays.</remarks>
        /// <typeparam name="T">Type of items in the sequence.</typeparam>
        /// <param name="sequence">Sequence of items to convert.</param>
        /// <param name="separator">Separator to place between the items in the string.</param>
        /// <param name="converter">The conversion function to change T -&gt; string.</param>
        /// <returns>The resulting string.</returns>
        public static string Concatenate<T>(this IEnumerable<T> sequence, string separator, Func<T, string> converter)
        {
            var sb = new StringBuilder();

            sequence.Aggregate(sb, (builder, item) =>
            {
                if (builder.Length > 0)
                {
                    builder.Append(separator);
                }

                builder.Append(converter(item));

                return builder;
            });

            return sb.ToString();
        }

        /// <summary>
        /// Create a single string from a sequenc of items, separated by the provided <paramref name="separator"/>,
        /// and with the conversion to string done by the item's <see cref='object.ToString'/> method.
        /// </summary>
        /// <remarks>This method does basically the same thing as <see cref="string.Join(string,string[])"/>,
        /// but will work on any sequence of items, not just arrays.</remarks>
        /// <typeparam name="T">Type of items in the sequence.</typeparam>
        /// <param name="sequence">Sequence of items to convert.</param>
        /// <param name="separator">Separator to place between the items in the string.</param>
        /// <returns>The resulting string.</returns>
        public static string Concatenate<T>(this IEnumerable<T> sequence, string separator)
        {
            return sequence.Concatenate(separator, item => item.ToString());
        }

        public static int RemoveByCondition<T>(this ICollection<T> collection, Func<T, bool> condition)
        {
            var itemsToRemove = collection.Where(condition).ToList();

            int count = 0;

            foreach (var itemToRemove in itemsToRemove)
            {
                collection.Remove(itemToRemove);
                count++;
            }

            return count;
        }

        public static T MinOrDefault<T>(this IEnumerable<T> sequence)
        {
            return sequence.Any() ? sequence.Min() : default(T);
        }

        public static T MinOrDefault<T>(this IEnumerable<T> sequence, T defaultValue)
        {
            return sequence.Any() ? sequence.Min() : defaultValue;
        }

        public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TResult> selector)
        {
            var selectorResult = sequence.Select(selector);
            return MinOrDefault(selectorResult);
        }

        public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TResult> selector, TResult defaultValue)
        {
            var selectorResult = sequence.Select(selector);
            return MinOrDefault(selectorResult, defaultValue);
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> sequence)
        {
            return sequence.Any() ? sequence.Max() : default(T);
        }

        public static T MaxOrDefault<T>(this IEnumerable<T> sequence, T defaultValue)
        {
            return sequence.Any() ? sequence.Max() : defaultValue;
        }

        public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TResult> selector)
        {
            var selectorResult = sequence.Select(selector);
            return MaxOrDefault(selectorResult);
        }

        public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TResult> selector, TResult defaultValue)
        {
            var selectorResult = sequence.Select(selector);
            return MaxOrDefault(selectorResult, defaultValue);
        }

        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            int retVal = 0;

            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }

            return -1;
        }

        /// <summary>
        /// Removes all the elements that match the conditions specified  defined by the specified predicate.
        /// </summary>
        /// <typeparam name="T">Type of items in the sequence.</typeparam>
        /// <param name="collection">The collection to remove from.</param>
        /// <param name="condition">Remove condition.</param>
        /// <returns>Removed items amount.</returns>
        public static int RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
        {
            var itemsToRemove = collection.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                collection.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Indicates whenever the specified <see cref="IEnumerable{T}" /> is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static bool All<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool mustExist)
        {
            foreach (var e in source)
            {
                if (!predicate(e))
                {
                    return false;
                }

                mustExist = false;
            }

            return !mustExist;
        }

        public static Tuple<int, int> CountWithConditions<T>(this IEnumerable<T> items, Func<T, bool> predicate1, Func<T, bool> predicate2)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (predicate1 == null)
            {
                throw new ArgumentNullException(nameof(predicate1));
            }

            if (predicate2 == null)
            {
                throw new ArgumentNullException(nameof(predicate2));
            }

            var counter1 = 0;
            var counter2 = 0;

            foreach (var item in items)
            {
                if (predicate1(item))
                {
                    counter1++;
                }

                if (predicate2(item))
                {
                    counter2++;
                }
            }

            return Tuple.Create(counter1, counter2);
        }
    }
}