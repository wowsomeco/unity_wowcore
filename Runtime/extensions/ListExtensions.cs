using System.Collections.Generic;
using System;

namespace Wowsome {
  /// <summary>
  /// Collection of List Extensions that are currently non existent in C# Lib.
  /// </summary>
  public static class ListExt {
    public delegate U Mapper<T, U>(T t);
    public delegate T Reducer<T, U>(T prev, U current);
    public delegate void Iterator<T>(T item, int idx);

    /// <summary>
    /// Maps the current IEnumerable into a new List<U>
    /// </summary>
    /// <param name="list">The origin list</param>
    /// <param name="mapper">The callback that gets called during the iteration which gives the origin list item and returns U</param>
    /// <typeparam name="T">The origin type of the list</typeparam>
    /// <typeparam name="U">The new type of the return.</typeparam>
    /// <example>
    /// <code>
    /// List<Vector2> vecs = new List<Vector2>() {
    ///   new Vector2(10f, 20f),
    ///   new Vector2(20f, 30f),
    ///   new Vector2(30f, 40f),
    /// };
    /// List<float> toFloats = vecs.Map(item => item.x + item.y);    
    /// </code>
    /// </example>
    public static List<U> Map<T, U>(this IEnumerable<T> list, Mapper<T, U> mapper) {
      List<U> newList = new List<U>();
      foreach (T itm in list) {
        newList.Add(mapper(itm));
      }
      return newList;
    }

    public static T Fold<T, U>(this IEnumerable<U> l, T initialValue, Reducer<T, U> reducer) {
      T cur = initialValue;
      foreach (U itm in l) {
        cur = reducer(cur, itm);
      }
      return cur;
    }

    public static void Loop<T>(this IList<T> l, Iterator<T> iter) {
      for (int i = 0; i < l.Count; ++i) {
        iter(l[i], i);
      }
    }

    public static void Swap<T>(this IList<T> list, int indexA, int indexB) {
      T tmp = list[indexA];
      list[indexA] = list[indexB];
      list[indexB] = tmp;
    }

    public static void Shuffle<T>(this IList<T> list) {
      System.Random rand = MathExtensions.GetRandom();
      int n = list.Count;
      while (n > 1) {
        n--;
        int k = rand.Next(n + 1);
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }

    public static bool IsEqual<T>(this IList<T> list1, IList<T> list2) where T : IEquatable<T> {
      if (list1.Count != list2.Count) {
        return false;
      }

      for (int i = 0; i < list1.Count; ++i) {
        if (!list1[i].Equals(list2[i])) {
          return false;
        }
      }

      return true;
    }

    public static bool IsContain<T>(this IList<T> list1, IList<T> list2, bool shouldSameSize = true) {
      if (shouldSameSize && list1.Count != list2.Count) {
        return false;
      }

      for (int i = 0; i < list2.Count; ++i) {
        if (!list1.Contains(list2[i])) {
          return false;
        }
      }

      return true;
    }

    public static T GetNullable<T>(this IList<T> list) where T : class {
      for (int i = 0; i < list.Count; ++i) {
        T t = list[i] as T;
        if (null != t) {
          return t;
        }
      }
      return null;
    }
  }
}
