﻿//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Copyright (c) 2010-2013 Garrett Serack and CoApp Contributors. 
//     Contributors can be discovered using the 'git log' command.
//     All rights reserved.
// </copyright>
// <license>
//     The software is licensed under the Apache 2.0 License (the "License")
//     You may not use the software except in compliance with the License. 
// </license>
//-----------------------------------------------------------------------

namespace ClrPlus.Core.Extensions {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Collections;

    public static class LinqExtensions {
        /// <summary>
        ///     Traverses a recursive collection producing a flattened collection.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="source"> </param>
        /// <param name="recurseFunction"> </param>
        /// <returns> </returns>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> recurseFunction) {
            foreach (var item in source) {
                yield return item;
                var enumerable = recurseFunction(item);

                if (enumerable != null) {
                    foreach (var itemRecurse in Traverse(enumerable, recurseFunction)) {
                        yield return itemRecurse;
                    }
                }
            }
        }

        /// <summary>
        ///     Returns the element of a given collection which is highest value based on the function provided.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <typeparam name="U"> </typeparam>
        /// <param name="source"> </param>
        /// <param name="selector"> </param>
        /// <returns> </returns>
        public static T MaxElement<T, U>(this IEnumerable<T> source, Func<T, U> selector) where U : IComparable<U> {
            if (source == null) {
                throw new ArgumentNullException("source");
            }
            bool first = true;
            T maxObj = default(T);
            U maxKey = default(U);
            foreach (var item in source) {
                if (first) {
                    maxObj = item;
                    maxKey = selector(maxObj);
                    first = false;
                } else {
                    U currentKey = selector(item);
                    if (currentKey.CompareTo(maxKey) > 0) {
                        maxKey = currentKey;
                        maxObj = item;
                    }
                }
            }
            return maxObj;
        }

        /// <summary>
        ///     Creates an enumerable consisting of a single element.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of <paramref name="source" />
        /// </typeparam>
        /// <param name="source"> The sole element in the return IEnumerable </param>
        /// <returns>
        ///     An <see cref="IEnumerable{T}" /> with <paramref name="source" /> as it's only element.
        /// </returns>
        public static IEnumerable<T> SingleItemAsEnumerable<T>(this T source) {
            return source as object == null ? Enumerable.Empty<T>() : new[] {
                source
            };
        }

        /// <summary>
        ///     Returns all the contiguous elements from sequence except for a specified number from the end.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the elements of <paramref name="source" />
        /// </typeparam>
        /// <param name="source"> The sequence to return elements from. </param>
        /// <param name="count"> The number of elements to not return from the end. </param>
        /// <returns>
        ///     An <see cref="System.Collections.Generic.IEnumerable{T}" /> consisting of all the elements of
        ///     <paramref
        ///         name="source" />
        ///     except for the last <paramref name="count" /> elements.
        /// </returns>
        public static IEnumerable<T> TakeAllBut<T>(this IEnumerable<T> source, int count) {
            return source.Reverse().Skip(count).Reverse();
        }

        /// <summary>
        ///     Returns the given number of elements from the end of the sequence.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the elements of <paramref name="source" /> .
        /// </typeparam>
        /// <param name="source"> The sequence to return elements from. </param>
        /// <param name="count"> The number of elements to return from the end of the array. </param>
        /// <returns>
        ///     An <see cref="System.Collections.Generic.IEnumerable{T}" /> consisting of the last <paramref name="count" /> elements of
        ///     <paramref
        ///         name="source" />
        ///     .
        /// </returns>
        public static IEnumerable<T> TakeFromEnd<T>(this IEnumerable<T> source, int count) {
            return source.Reverse().Take(count).Reverse();
        }

        /// <summary>
        ///     Returns a new collection including the given item. DOES NOT MODIFY THE ORIGINAL COLLECTION.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="collection"> </param>
        /// <param name="newItem"> </param>
        /// <returns> </returns>
        public static IEnumerable<T> UnionSingleItem<T>(this IEnumerable<T> collection, T newItem) {
            return collection.Concat(new[] {
                newItem
            });
        }

        public static IEnumerable<T> ConcatSingleItem<T>(this IEnumerable<T> collection, T newItem) {
            return collection.Concat(new[] {
                newItem
            });
        }

        public static IEnumerable<T> ConcatHappily<T>(this IEnumerable<T> collection, T newItem) {
            return collection == null ? new[] {
                newItem
            } : collection.Concat(new [] {
                newItem
            });
        }

        public static IEnumerable<T> ConcatHappily<T>(this IEnumerable<T> collection, IEnumerable<T> newItems) {
            return collection == null ? newItems: collection.Concat(newItems);
        }

        public static T[] UnionA<T>(this IEnumerable<T> collection, T newItem) {
            return collection.UnionSingleItem(newItem).ToArray();
        }

        public static T[] UnionA<T>(this IEnumerable<T> collection, IEnumerable<T> newItems) {
            return collection.Union(newItems).ToArray();
        }



        public static IEnumerable<TSource> SortBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            return source.OrderBy(keySelector);
        }

        public static IEnumerable<TSource> SortBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) {
            return source.OrderBy(keySelector, comparer);
        }

        public static IEnumerable<TSource> SortByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            return source.OrderByDescending(keySelector);
        }

        public static IEnumerable<TSource> SortByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) {
            return source.OrderByDescending(keySelector, comparer);
        }

        public static IEnumerable<TSource> ThenBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            return Enumerable.ThenBy((IOrderedEnumerable<TSource>)source, keySelector);
        }

        public static IEnumerable<TSource> ThenBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) {
            return Enumerable.ThenBy((IOrderedEnumerable<TSource>)source, keySelector, comparer);
        }

        public static IEnumerable<TSource> ThenByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) {
            return Enumerable.ThenByDescending((IOrderedEnumerable<TSource>)source, keySelector);
        }

        public static IEnumerable<TSource> ThenByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) {
            return Enumerable.ThenByDescending((IOrderedEnumerable<TSource>)source, keySelector, comparer);
        }

        public static IEnumerable<TSource> WhereDistinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> each) {
            return source.GroupBy(each).Select(x => x.First());
        }

        public static bool ContainsAll<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> matchItems) {
            return matchItems.All(source.Contains);
        }

        #region Don't test this crap.

        /// <summary>
        ///     Don
        /// </summary>
        private class IndexedEnumerator : IEnumerator<int> {
            private readonly int _max;

            internal IndexedEnumerator(int i) {
                _max = i;
                Current = -1;
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                return ++Current < _max;
            }

            public void Reset() {
                Current = -1;
            }

            public int Current {get; private set;}

            object IEnumerator.Current {
                get {
                    return Current;
                }
            }
        }

        private class ListIndex : IEnumerable<int> {
            private readonly int _max;

            internal ListIndex(int i) {
                _max = i;
            }

            public IEnumerator<int> GetEnumerator() {
                return new IndexedEnumerator(_max);
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        public static IEnumerable<int> ByIndex<T>(this IList<T> lst) {
            return new ListIndex(lst.Count);
        }

        public static CompareResult<T, T> Compare<T>(this IEnumerable<T> left, IEnumerable<T> right) {
            return Compare(left, right, (x, y) => x.Equals(y));
        }

        public static CompareResult<T, TRight> Compare<T, TRight>(this IEnumerable<T> left, IEnumerable<TRight> right, Func<T, TRight, bool> isEqual) {
            return Compare(left, right, isEqual, isEqual);
        }

        public static CompareResult<TLeft, TRight> Compare<TLeft, TRight>(this IEnumerable<TLeft> leftList, IEnumerable<TRight> rightList, Func<TLeft, TRight, bool> isEqual, Func<TLeft, TRight, bool> isSame) {
            var results = new CompareResult<TLeft, TRight>();

            results.Removed.AddRange(leftList.Where(x => rightList.Any(y => isSame(x, y)) == false));
            results.Added.AddRange(rightList.Where(x => leftList.Any(y => isSame(y, x)) == false));

            foreach (var left in leftList) {
                var right = rightList.FirstOrDefault(x => isSame(left, x));

                if (right == null) {
                    continue;
                }

                if (!isEqual(left, right)) {
                    results.Different.Add(left, right);
                } else {
                    results.Equal.Add(left, right);
                }
            }
            return results;
        }
    }

    public class CompareResult<TLeft, TRight> {
        #region Fields

        private List<TLeft> _onlyInLeftList = new List<TLeft>();
        private List<TRight> _onlyInRightList = new List<TRight>();
        private IDictionary<TLeft, TRight> _different = new XDictionary<TLeft, TRight>();
        private IDictionary<TLeft, TRight> _equal = new XDictionary<TLeft, TRight>();

        #endregion

        #region Properties

        public IDictionary<TLeft, TRight> Equal {
            get {
                return _equal;
            }
        }

        public IDictionary<TLeft, TRight> Different {
            get {
                return _different;
            }
        }

        /// <summary>
        ///     Items in the left list no also in the right list
        /// </summary>
        public List<TLeft> Removed {
            get {
                return _onlyInLeftList;
            }
        }

        /// <summary>
        ///     Items in the right list not in the left list
        /// </summary>
        public List<TRight> Added {
            get {
                return _onlyInRightList;
            }
        }

        public bool IsSame {
            get {
                return TotalDifferences == 0;
            }
        }

        public int TotalDifferences {
            get {
                return _onlyInLeftList.Count + _onlyInRightList.Count + _different.Count;
            }
        }

        #endregion
    }

    #endregion
}