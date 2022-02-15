using System;
using System.Collections.Generic;

namespace Wowsome {
  public static class MathExtensions {
    public static T Clamp<T>(this T val, T lower, T upper) where T : IComparable<T> {
      return val.CompareTo(lower) < 0 ? lower : val.CompareTo(upper) > 0 ? upper : val;
    }

    public static System.Random GetRandom() {
      return new System.Random(Guid.NewGuid().GetHashCode());
    }

    public static int RandomBetween(int min, int max) {
      return GetRandom().Next(min, max);
    }

    public static List<int> GetRandomValue(int min, int max, int count) {
      System.Random rand = GetRandom();
      List<int> randVals = new List<int>();

      while (randVals.Count < count) {
        int next = rand.Next(min, max);
        randVals.Add(next);
      }

      return randVals;
    }

    public static HashSet<int> GetUniqueRandomValue(int min, int max, int count, List<int> exception = null) {
      System.Random rand = GetRandom();
      count = count.Clamp(0, max);
      HashSet<int> randVals = new HashSet<int>();
      while (randVals.Count < count) {
        //rand.Next max value is exclusive
        int next = rand.Next(min, max);
        if (exception != null && exception.Contains(next)) {
          continue;
        } else {
          randVals.Add(next);
        }
      }

      return randVals;
    }

    public static int[] GetUniqueRandomValueArr(int min, int max, int count, List<int> exception = null) {
      HashSet<int> rands = GetUniqueRandomValue(min, max, count, exception);
      int[] randArr = new int[count];
      int i = 0;
      foreach (int rand in rands) {
        randArr[i] = rand;
        ++i;
      }
      return randArr;
    }
  }
}
