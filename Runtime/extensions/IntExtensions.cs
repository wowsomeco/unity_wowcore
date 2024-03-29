﻿using System;
using System.Collections.Generic;

namespace Wowsome {
  public static class IntExt {
    public static void Loop(this int val, Action<int> each) {
      for (int i = 0; i < val; ++i) {
        each?.Invoke(i);
      }
    }

    public static void LoopReverse(this int val, Action<int> each) {
      for (int i = (val - 1); i >= val; --i) {
        each?.Invoke(i);
      }
    }

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

    public static string SecToClockDigit(this int sec) {
      int minutes = (sec % 3600) / 60;
      int seconds = (sec % 60);

      return string.Format("{0}:{1}", minutes, seconds.ToString("00"));
    }

    public static string SecToReadableTime(this int sec) {
      int minutes = (sec % 3600) / 60;
      int seconds = (sec % 60);

      string m = minutes > 0 ? $"{minutes}m " : "";
      string s = seconds > 0 ? $"{seconds}s" : "";

      return m + s;
    }

    public static int PrevPowerOfTwo(this int v) {
      bool isPOT = v.IsPowerOfTwo();
      if (isPOT) {
        return (v - 1).PrevPowerOfTwo();
      }

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

    public static int Add(this int i, int delta) {
      return i + delta;
    }

    public static bool IsEven(this int i) => i % 2 == 0;

    public static bool IsOdd(this int i) => !i.IsEven();

    public static int GetRandomIdxByWeights(this List<int> weights) {
      int total = weights.Fold(0, (p, c) => p + c);

      int randomNumber = MathExtensions.RandomBetween(0, total) + 1;

      int accumulatedProbability = 0;

      for (int i = 0; i < weights.Count; i++) {
        accumulatedProbability += weights[i];

        if (randomNumber <= accumulatedProbability)
          return i;
      }

      return 0;
    }
  }
}