﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome {
  public static class FloatExt {
    public static float GetNearestPointf(this float candidate, IList<float> lists) {
      float nearest = lists[0];
      for (int i = 0; i < lists.Count; ++i) {
        if (Mathf.Abs(lists[i] - candidate) < Mathf.Abs(nearest - candidate)) nearest = lists[i];
      }

      return nearest;
    }

    public static Vector2 ToVector2(this float v) { return new Vector2(v, v); }

    public static Vector2 ToVector2(this float[] self) {
      return new Vector2(self[0], self[1]);
    }

    public static Vector2 ToVector2(this List<float> v) {
      return new Vector2(v[0], v.Count > 1 ? v[1] : 0f);
    }

    public static Color ToColor(this float[] f) {
      return new Color(f[0], f[1], f[2], f[3]);
    }

    public static float Round(this float f, int digits = 2) {
      return (float)Math.Round((double)f, digits);
    }

    /// <summary>
    /// e.g. 270 will get converted to -90
    /// </summary>
    public static float WrapAngle(this float angle) {
      angle %= 360f;
      if (angle > 180f) return angle - 360f;

      return angle;
    }

    public static float AngleBetween(float x, float y, float angleOffset = 0f) {
      float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg + angleOffset;
      if (angle < 0f) {
        angle += 360f;
      }
      return angle % 360f;
    }

    public static float Multiply(this float v, float multiplier) {
      return v * multiplier;
    }

    public static float Half(this float v) {
      return v.Multiply(.5f);
    }
  }
}