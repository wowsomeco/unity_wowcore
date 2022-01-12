using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome {
  public static class GameObjectExt {
    public static void ExecOnActive(this GameObject g, Action callback) {
      if (g.activeInHierarchy) callback();
    }

    public static bool Same(this GameObject g, GameObject other) {
      if (null == g || null == other) return false;
      return g.GetInstanceID() == other.GetInstanceID();
    }

    public static bool HasComponent<T>(this GameObject g) where T : Component {
      return g.GetComponent<T>() != null;
    }

    public static void DestroyChildren(this GameObject gameObject) {
      Transform goTransform = gameObject.transform;
      for (int i = goTransform.childCount - 1; i >= 0; --i) {
        GameObject.DestroyImmediate(goTransform.GetChild(i).gameObject);
      }
    }

    public static void IterateChildren(this GameObject gameObject, Delegate<bool, GameObject> shouldRecursive) {
      DoIterate(gameObject, shouldRecursive);
    }

    public static void IterateSelfAndChildren(this GameObject g, Delegate<bool, GameObject> shouldRecursive) {
      // if it's self, it wont check should recursive.
      shouldRecursive(g);
      // iterate children
      DoIterate(g, shouldRecursive);
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

    public static List<TComponent> GetComponentsInChildrenWithCallback<TComponent>(this GameObject go, Action<TComponent> eachComponent = null) {
      List<TComponent> components = new List<TComponent>();

      var comps = go.GetComponentsInChildren<TComponent>();
      foreach (TComponent c in comps) {
        eachComponent?.Invoke(c);

        components.Add(c);
      }

      return components;
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