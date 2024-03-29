using System.Collections.Generic;
using UnityEngine;

namespace Wowsome {
  public static class Vector3Ext {
    public static Vector2 ToVector2(this Vector3 v) {
      return new Vector2(v.x, v.y);
    }

    public static Vector3 SetX(this Vector3 v, float x) {
      return new Vector3(x, v.y, v.z);
    }

    public static Vector3 AddX(this Vector3 v, float x) {
      return new Vector3(v.x + x, v.y, v.z);
    }

    public static Vector3 SetY(this Vector3 v, float y) {
      return new Vector3(v.x, y, v.z);
    }

    public static Vector3 AddY(this Vector3 v, float y) {
      return new Vector3(v.x, v.y + y, v.z);
    }

    public static Vector3 SetZ(this Vector3 v, float z) {
      return new Vector3(v.x, v.y, z);
    }

    public static Vector3 AddZ(this Vector3 v, float z) {
      return new Vector3(v.x, v.y, v.z + z);
    }
  }

  public static class Vector2Ext {
    public static Vector2Int ToVector2Int(this Vector2 v) {
      return new Vector2Int((int)v.x, (int)v.y);
    }

    public static Vector3 ToVector3(this Vector2 v) {
      return new Vector3(v.x, v.y, 0f);
    }

    public static Vector2 Create(float f) {
      return new Vector2(f, f);
    }

    public static Vector2 LerpWithOffset(this Vector2 v, Vector2 other, float t, Vector2 offset) {
      return Vector2.Lerp(v, other, t) + offset;
    }

    public static bool IsZero(this Vector2 v) {
      return Mathf.Approximately(v.x, 0f) && Mathf.Approximately(v.y, 0f);
    }

    public static float[] ToFloats(this Vector2 self) {
      return new float[2] { self.x, self.y };
    }

    public static List<float> ToListFloat(this Vector2 self) {
      return new List<float> { self.x, self.y };
    }

    public static Vector2 ClampVec2(this Vector2 v, Vector2 min, Vector2 max) {
      v.x = Mathf.Clamp(v.x, min.x, max.x);
      v.y = Mathf.Clamp(v.y, min.y, max.y);
      return v;
    }

    public static bool IsEqual(this Vector2 lhs, Vector2 rhs) {
      return (Mathf.Approximately(lhs.x, rhs.x) && Mathf.Approximately(lhs.y, rhs.y));
    }

    public static Vector2 AspectRatio(this Vector2 v, float maxSize) {
      float max = Mathf.Max(v.x, v.y);
      if (max > maxSize) {
        float ratio = maxSize / max;
        v *= ratio;
        return v;
      }
      return v;
    }

    public static float Max(this Vector2 v) {
      return Mathf.Max(v.x, v.y);
    }

    public static Vector2 Mul(this Vector2 v, Vector2 other) {
      return new Vector2(v.x * other.x, v.y * other.y);
    }

    public static Vector2 Mul(this Vector2 v, float value) {
      return new Vector2(v.x * value, v.y * value);
    }

    public static Vector2 Add(this Vector2 v, Vector2 other) {
      return new Vector2(v.x + other.x, v.y + other.y);
    }

    public static Vector2 AddX(this Vector2 v, float x) {
      return new Vector2(v.x + x, v.y);
    }

    public static Vector2 AddY(this Vector2 v, float y) {
      return new Vector2(v.x, v.y + y);
    }

    public static Vector2 SetX(this Vector2 v, float x) {
      return new Vector2(x, v.y);
    }

    public static Vector2 SetY(this Vector2 v, float y) {
      return new Vector2(v.x, y);
    }

    public static Vector2 ScreenToLocalPos(this Vector2 screenPos, RectTransform parent, Camera camera = null) {
      Vector2 pos;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPos, camera, out pos);
      return pos;
    }

    public static Vector2 WorldToLocalPos(this Vector3 worldPos, RectTransform parent, Camera camera = null) {
      Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, worldPos);
      return screenPos.ScreenToLocalPos(parent, camera);
    }

    public static Vector2 Randomize(this Vector2 v, float range = 1f) {
      return v.Randomize(-range, range);
    }

    public static Vector2 Randomize(this Vector2 v, float from, float to) {
      float x = v.x.Multiply(FloatExt.Random(from, to));
      float y = v.y.Multiply(FloatExt.Random(from, to));

      return new Vector2(x, y);
    }

    public static float InverseLerp(Vector2 a, Vector2 b, Vector2 v) {
      Vector2 ab = b - a;
      Vector2 av = v - a;

      float t = Vector2.Dot(av, ab) / Vector2.Dot(ab, ab);
      return t.Clamp(0f, 1f);
    }
  }

  public static class Vector2IntExt {
    public static Vector2 ToVector2(this Vector2Int v) {
      return new Vector2(v.x, v.y);
    }

    public static Vector2Int SetX(this Vector2Int v, int x) {
      return new Vector2Int(x, v.y);
    }

    public static Vector2Int AddX(this Vector2Int v, int x) {
      return v.SetX(v.x + x);
    }

    public static Vector2Int SetY(this Vector2Int v, int y) {
      return new Vector2Int(v.x, y);
    }

    public static Vector2Int AddY(this Vector2Int v, int y) {
      return v.SetY(v.y + y);
    }

    public static Vector2Int Flip(this Vector2Int v) {
      return new Vector2Int(v.y, v.x);
    }
  }
}