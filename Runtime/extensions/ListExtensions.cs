using System;
using System.Collections.Generic;

namespace Wowsome {
  /// <summary>
  /// Collection of List Extensions that are currently non existent in C# Lib.
  /// </summary>
  public static class ListExt {
    public delegate U Mapper<T, U>(T t);
    public delegate T Reducer<T, U>(T prev, U current);
    public delegate void Iterator<T>(T item, int idx);
    public delegate void IteratorWithPointer<T>(T item, int idx, bool first, bool last);

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

    public static void LoopWithPointer<T>(this IList<T> l, IteratorWithPointer<T> iter) {
      for (int i = 0; i < l.Count; ++i) {
        iter(l[i], i, i == 0, i == l.Count - 1);
      }
    }

    public static void LoopReverse<T>(this IList<T> l, IteratorWithPointer<T> iter) {
      for (int i = (l.Count - 1); i >= 0; --i) {
        iter(l[i], i, i == 0, i == l.Count - 1);
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

    public static T Last<T>(this IList<T> l) {
      return l.Count > 0 ? l[l.Count - 1] : default(T);
    }

    public static bool IsEmpty<T>(this IList<T> l) {
      return null == l || l.Count == 0;
    }

    public static List<T> RemoveWhere<T>(this List<T> l, Predicate<T> p) {
      List<T> found = l.FindAll(p);
      if (found.Count > 0) l.RemoveAll(p);

      return found;
    }

    public static void TryFind<T>(this List<T> l, Predicate<T> predicate, Action<T> found, Action notFound = null) {
      T t = l.Find(predicate);
      if (t == null) {
        if (null != notFound) notFound.Invoke();
      } else {
        found(t);
      }
    }

    public static List<T> Combine<T>(params IList<T>[] lists) {
      List<T> newList = new List<T>();
      foreach (IList<T> l in lists) {
        newList.AddRange(l);
      }
      return newList;
    }

    public static T Push<T>(this IList<T> list, T item) {
      list.Add(item);
      return item;
    }

    public static bool RemoveLast<T>(this IList<T> list) {
      if (list.Count > 0) {
        list.RemoveAt(list.Count - 1);
        return true;
      }
      return false;
    }

    public static T TryGet<T>(this IList<T> list, int idx) {
      if (list != null && idx > -1 && list.Count > idx) { return list[idx]; }
      return default(T);
    }

    public static T First<T>(this IList<T> arr) {
      return null != arr && arr.Count > 0 ? arr[0] : default(T);
    }

    public static List<T> ToList<T>(this T[] arr) {
      return new List<T>(arr);
    }

    public static string Flatten(this IList<string> strs, char separator = ' ') {
      string s = string.Empty;
      strs.LoopWithPointer((str, idx, first, last) => {
        s += str;
        if (!last) s += separator;
      });

      return s;
    }

    public static T PickRandom<T>(this IList<T> list) {
      if (list.IsEmpty()) return default(T);

      System.Random rand = MathExtensions.GetRandom();
      int r = rand.Next(0, list.Count);

      return list[r];
    }

    public static List<T> PickRandoms<T>(this IList<T> list, int count) {
      if (list.IsEmpty()) return new List<T>();

      List<T> l = new List<T>();
      List<T> origin = new List<T>(list);

      int randCount = count.Clamp(0, list.Count);

      System.Random rand = MathExtensions.GetRandom();

      for (int i = 0; i < randCount; ++i) {
        if (origin.Count == 0) break;

        int randIdx = rand.Next(0, origin.Count);

        l.Add(origin[randIdx]);
        origin.RemoveAt(randIdx);
      }

      return l;
    }

    public static bool IsLastIdx<T>(this IList<T> list, int idx) {
      if (list.IsEmpty()) return false;

      return idx == (list.Count - 1);
    }

    public static bool AddIfNotExist<T>(this List<T> list, T item, Predicate<T> predicate) {
      if (!list.Exists(predicate)) {
        list.Add(item);

        return true;
      }

      return false;
    }
  }
}
