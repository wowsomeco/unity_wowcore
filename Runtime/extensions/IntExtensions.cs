using System;
using System.Collections.Generic;

namespace Wowsome {
  public static class IntExt {
    public static int GetNearestPointi(this int candidate, IList<int> list) {
      int nearest = list[0];
      for (int i = 0; i < list.Count; ++i) {
        if (Math.Abs(list[i] - candidate) < Math.Abs(nearest - candidate)) nearest = list[i];
      }

      return nearest;
    }

    public static int Flip(this int i) {
      return -i;
    }

    public static bool IsWithin(this int val, int lower, int upper) {
      return (val >= lower && val <= upper);
    }

    public static string SecToString(this int sec) {
      int minutes = (sec % 3600) / 60;
      int seconds = (sec % 60);
      return string.Format("{0}:{1}", minutes, seconds.ToString("00"));
    }

    public static int PrevPowerOfTwo(this int v) {
      if (v.IsPowerOfTwo()) return v;

      v |= v >> 1;
      v |= v >> 2;
      v |= v >> 4;
      v |= v >> 8;
      v |= v >> 16;

      return v - (v >> 1);
    }

    public static bool IsPowerOfTwo(this int x) {
      return (x != 0) && ((x & (x - 1)) == 0);
    }
  }
}