using System;
using UnityEngine;

namespace Wowsome {
  public static class AnimatorExtensions {
    public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type) {
      var parameters = self.parameters;
      foreach (var currParam in parameters) {
        if (currParam.type == type && currParam.name == name) {
          return true;
        }
      }
      return false;
    }
    public static bool HasFinishedPlayingAnim(this Animator self, string tag) {
      var curAnimStateInfo = self.GetCurrentAnimatorStateInfo(0);
      if (curAnimStateInfo.normalizedTime >= 1 && !self.IsInTransition(0)) {
        if (curAnimStateInfo.IsTag(tag)) {
          return true;
        }
      }
      return false;
    }
    public static bool IsPlayingTag(this Animator self, string tag) {
      if (self.GetCurrentAnimatorStateInfo(0).IsTag(tag)) {
        return true;
      }
      return false;
    }
  }

  public static class ColorExt {
    public static string ColorToHex(Color32 color) {
      string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
      return hex;
    }

    public static Color HexToColor(string hex) {
      hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
      hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
      byte a = 255;//assume fully visible unless specified in hex
      byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
      byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
      byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
      //Only use alpha if the string has enough characters

      if (hex.Length == 8) {
        a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
      }
      return new Color32(r, g, b, a);

    }

    public static string ColorWithNewAlpha(string hex, float alpha) {
      var col = HexToColor(hex);
      col.a = alpha;
      return ColorToHex(col);
    }

    public static float[] ToFloats(this Color col) {
      return new float[] { col.r, col.g, col.b, col.a };
    }
  }

  public static class Vector2Ext {
    public static float[] ToFloats(this Vector2 self) {
      return new float[2] { self.x, self.y };
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
  }

  public static class GameObjectExt {
    public delegate bool ShouldRecursive(GameObject go);

    public static void DestroyChildren(this GameObject gameObject) {
      Transform goTransform = gameObject.transform;
      for (int i = goTransform.childCount - 1; i >= 0; i--) {
        GameObject.DestroyImmediate(goTransform.GetChild(i).gameObject);
      }
    }

    public static void IterateChildren(GameObject gameObject, ShouldRecursive shouldRecursive) {
      DoIterate(gameObject, shouldRecursive);
    }

    public static T Clone<T>(this GameObject go, Transform parent, string name = "") {
      GameObject instance = GameObject.Instantiate(go) as GameObject;
      instance.transform.SetParent(parent, false);
      if (!string.IsNullOrEmpty(name)) {
        instance.name = name;
      }
      return instance.GetComponent<T>();
    }

    private static void DoIterate(GameObject gameObject, ShouldRecursive shouldRecursive) {
      foreach (Transform child in gameObject.transform) {
        bool recursive = shouldRecursive(child.gameObject);
        if (recursive) {
          DoIterate(child.gameObject, shouldRecursive);
        }
      }
    }
  }
}