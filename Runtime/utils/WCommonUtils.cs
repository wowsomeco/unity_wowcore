using UnityEngine;

namespace Wowsome {
  public static class ScreenUtils {
    public static float MinResolution() {
      return Mathf.Min((float)Screen.width, (float)Screen.height);
    }

    public static float MaxResolution() {
      return Mathf.Max((float)Screen.width, (float)Screen.height);
    }
  }

  public static class Utils {
    public static float AspectRatioScaler(Vector2 referenceResolution) {
      return (referenceResolution.x / referenceResolution.y) / ((float)Screen.width / Screen.height);
    }
  }

  public static class Assert {
    public static void Null<T>(object obj, string err = null) {
#if UNITY_EDITOR
      Debug.Assert(null != obj, err.IsEmpty() ? (typeof(T).Name + " is null") : err);
#endif
    }

    public static void Null<TObject, TComponent>(object obj, GameObject go, string err = null) {
#if UNITY_EDITOR
      Debug.Assert(null != obj, $"{typeof(TObject).Name} is null, component = {typeof(TComponent).Name}, gameobject = {go.name}");
#endif
    }

    public static void Null(object obj, string err = null) {
#if UNITY_EDITOR
      Debug.Assert(null != obj, err.IsEmpty() ? "obj is null" : err);
#endif
    }

    public static void If(bool condition, string err) {
#if UNITY_EDITOR
      Debug.Assert(!condition, err);
#endif
    }
  }
}