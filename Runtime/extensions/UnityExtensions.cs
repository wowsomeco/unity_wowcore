using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Wowsome {
  public static class Print {
    /// <summary>
    /// Wrapper around unity Debug.Log but only gets printed on unity editor,
    /// it wont be printed during production e.g. on mobile devices.
    /// </summary>
    /// <param name="msg">The message to print on the editor</param>
    public static void Log(object msg) {
#if UNITY_EDITOR
      Debug.Log(msg);
#endif
    }
  }

  public static class Assert {
    public static void Null(object obj, string err = null) {
#if UNITY_EDITOR
      Debug.Assert(null != obj, string.IsNullOrEmpty(err) ? "obj is null" : err);
#endif
    }
  }

  public static class TextureExt {
    public static Sprite ToSprite(this Texture2D texture) {
      Sprite sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.one * 0.5f);
      return sprite;
    }

    public static Sprite ToSprite(this string filePath) {
      return filePath.ToTexture2D().ToSprite();
    }

    public static Texture2D ToTexture2D(this string filePath) {
      Texture2D t = new Texture2D(1, 1);
      t.LoadImage(File.ReadAllBytes(filePath));
      return t;
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

    public static float[] ToFloats(this Color col) {
      return new float[] { col.r, col.g, col.b, col.a };
    }

    public static Color SetAlpha(this Color col, float a) {
      return new Color(col.r, col.g, col.b, a);
    }
  }

  public static class Vector3Ext {
    public static Vector2 ToVector2(this Vector3 v) {
      return new Vector2(v.x, v.y);
    }
  }

  public static class Vector2Ext {
    public static bool IsZero(this Vector2 v) {
      return Mathf.FloorToInt(v.x) == 0 && Mathf.FloorToInt(v.y) == 0;
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
  }

  public static class ComponentExt {
    public static bool Same(this Component c, Component other) {
      return c.GetInstanceID() == other.GetInstanceID();
    }

    public static void SetVisible(this Component c, bool flag) {
      c.gameObject.SetActive(flag);
    }

    public static List<T> GetComponentsWithoutSelf<T>(this Component obj, bool includeInactive) where T : Component {
      List<T> components = new List<T>();
      var comps = obj.GetComponentsInChildren<T>(includeInactive);
      // might be slow though ...
      foreach (T c in comps) {
        // dont include self
        if (c.GetInstanceID() != obj.GetInstanceID()) {
          components.Add(c);
        }
      }
      return components;
    }

    public static T Clone<T>(this Component c, Transform parent, string name = "") {
      return c.gameObject.Clone<T>(parent, name);
    }

    /// <summary>
    /// Given a list that consists of 1 component in it, then clone to the same parent of that component,
    /// then add the cloned to the list afterwards
    /// </summary>
    /// <param name="l">The List</param>
    /// <param name="count">The number of clone</param>
    /// <typeparam name="T">Any Component.</typeparam>
    public static void CloneToParent<T>(this List<T> l, int count, Action<T> onClone = null) where T : Component {
      for (int i = 0; i < count; ++i) {
        T cloned = l[0].gameObject.Clone<T>(l[0].transform.parent);
        l.Add(cloned);
        if (onClone != null) onClone.Invoke(cloned);
      }
    }

    public static T CreateComponent<T>(Transform parent, string objName) where T : Component {
      GameObject go = GameObjectExt.CreateGameObject(objName, parent);
      return go.AddComponent<T>();
    }

    public static T CreateFromResource<T>(string resourcePath, Transform parent, string name = "") where T : Component {
      GameObject obj = GameObject.Instantiate(Resources.Load(resourcePath)) as GameObject;
      Debug.Assert(obj != null, "gamobject is null");

      obj.transform.SetParent(parent, false);
      if (!name.IsEmpty()) obj.name = name;

      T c = obj.GetComponent<T>();
      Debug.Assert(c != null, "component is null");

      return c;
    }

    public static void IterateParent(this Transform t, Delegate<bool, Transform> shouldRecursive) {
      Transform parent = t.parent;
      if (null != parent) {
        bool recursive = shouldRecursive(parent);
        if (recursive) {
          IterateParent(parent, shouldRecursive);
        }
      }
    }
  }

  public static class GameObjectExt {
    public static void DestroyChildren(this GameObject gameObject) {
      Transform goTransform = gameObject.transform;
      for (int i = goTransform.childCount - 1; i >= 0; i--) {
        GameObject.DestroyImmediate(goTransform.GetChild(i).gameObject);
      }
    }

    public static void IterateChildren(GameObject gameObject, Delegate<bool, GameObject> shouldRecursive) {
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

    public static GameObject CreateGameObject(string name, Transform parent) {
      GameObject go = new GameObject(name);
      go.transform.SetParent(parent, false);
      return go;
    }

    public static void DestroyComponent<T>(this GameObject g) where T : Component {
      T c = g.GetComponent<T>();
      if (c == null) return;

#if UNITY_EDITOR
      GameObject.DestroyImmediate(c);
#else
      GameObject.Destroy(c);
#endif
    }

    private static void DoIterate(GameObject gameObject, Delegate<bool, GameObject> shouldRecursive) {
      foreach (Transform child in gameObject.transform) {
        bool recursive = shouldRecursive(child.gameObject);
        if (recursive) {
          DoIterate(child.gameObject, shouldRecursive);
        }
      }
    }
  }
}