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
      if (min == max) return max;

      int _min = min;
      int _max = max;

      if (_max < _min) {
        _min = max;
        _max = min;
      } else if (_max - _min == 1) {
        // if max is just 1 number higher than min, add up 1 number since random.next.max is exclusive
        _max += 1;
      }

      return GetRandom().Next(_min, _max);
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

    public static List<int> GetUniqueRandomValue(int min, int max, int count, List<int> exception = null) {
      System.Random rand = GetRandom();
      count = count.Clamp(0, max);
      List<int> randVals = new List<int>();

      while (randVals.Count < count) {
        // rand.Next max value is exclusive
        int randValue = rand.Next(min, max);
        // skip when exception has the value or randVals already contains the value itself
        bool shouldSkip =
          (!exception.IsEmpty() && exception.Contains(randValue)) || randVals.Contains(randValue);

        if (shouldSkip) {
          continue;
        } else {
          randVals.Add(randValue);
        }
      }

      return randVals;
    }

    public static int[] GetUniqueRandomValueArr(int min, int max, int count, List<int> exception = null) {
      var rands = GetUniqueRandomValue(min, max, count, exception);
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
