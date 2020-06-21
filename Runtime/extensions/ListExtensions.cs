using System.Collections.Generic;
using System;

namespace Wowsome {
  public static class ListExt {
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
