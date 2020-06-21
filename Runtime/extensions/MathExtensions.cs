using System.Collections.Generic;
using UnityEngine;
using System;

namespace Wowsome {
  public static class MathExtensions {
    public static float GetNearestPointf(this float candidate, List<float> lists) {
      float nearest = lists[0];
      for (int i = 0; i < lists.Count; ++i) {
        if (Mathf.Abs(lists[i] - candidate) < Mathf.Abs(nearest - candidate)) nearest = lists[i];
      }

      return nearest;
    }

    public static int GetNearestPointi(this int candidate, List<int> list) {
      int nearest = list[0];
      for (int i = 0; i < list.Count; ++i) {
        if (Math.Abs(list[i] - candidate) < Math.Abs(nearest - candidate)) nearest = list[i];
      }

      return nearest;
    }

    public static float AngleBetween(float x, float y, float angleOffset = 0f) {
      float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg + angleOffset;
      if (angle < 0f) {
        angle += 360f;
      }
      return angle % 360f;
    }

    public static float ClampAngle(float angle, float min, float max) {
      if (angle < 0f)
        angle += 360;
      if (angle > 360)
        angle -= 360;

      return angle.Clamp(min, max);
    }

    public static int Flip(this int i) {
      return -i;
    }

    public static T Clamp<T>(this T val, T lower, T upper) where T : IComparable<T> {
      return val.CompareTo(lower) < 0 ? lower : val.CompareTo(upper) > 0 ? upper : val;
    }

    public static bool IsWithin(this int val, int lower, int upper) {
      return (val >= lower && val <= upper);
    }

    public static System.Random GetRandom() {
      return new System.Random(Guid.NewGuid().GetHashCode());
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

    public static float Round(this float f) {
      return Mathf.Round(f * 100f) / 100f;
    }

    public static string SecToString(this int sec) {
      int minutes = (sec % 3600) / 60;
      int seconds = (sec % 60);
      return string.Format("{0}:{1}", minutes, seconds.ToString("00"));
    }
  }
}
