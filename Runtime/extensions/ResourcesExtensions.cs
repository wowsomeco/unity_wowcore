using UnityEngine;

namespace Wowsome {
  public static class ResourcesExt {
    public static TObject LoadResource<TObject>(this string path, bool assertIfNull = true) where TObject : UnityEngine.Object {
      TObject obj = Resources.Load<TObject>(path);

      if (assertIfNull) {
        Assert.Null<TObject>(obj, $"Can't load Resources from path = {path}");
      }

      return obj;
    }

    public static TComponent CloneResource<TComponent>(this string path, Transform parent, bool assertIfNull = true) where TComponent : UnityEngine.Component {
      TComponent obj = path.LoadResource<TComponent>();

      if (null != obj) {
        return obj.Clone<TComponent>(parent);
      }

      return null;
    }

    public static TObject LoadJSON<TObject>(this string path, bool assertIfNull = true) {
      TextAsset ta = Resources.Load<TextAsset>(path);

      if (assertIfNull) {
        Assert.Null<TObject>(ta, $"Can't load JSON from path = {path}");
      }

      return JsonUtility.FromJson<TObject>(ta.text);
    }
  }
}