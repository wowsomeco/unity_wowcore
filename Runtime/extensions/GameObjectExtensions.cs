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
      for (int i = goTransform.childCount - 1; i >= 0; i--) {
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

    private static void DoIterate(GameObject gameObject, Delegate<bool, GameObject> shouldRecursive) {
      foreach (Transform child in gameObject.transform) {
        bool recursive = shouldRecursive(child.gameObject);
        if (recursive) {
          DoIterate(child.gameObject, shouldRecursive);
        }
      }
    }
  }

  public static class ComponentExt {
    public static T ReplaceWith<T>(this Component component) where T : Component {
      GameObject g = component.gameObject;
#if UNITY_EDITOR
      GameObject.DestroyImmediate(component);
#else
      GameObject.Destroy(component);
#endif
      return g.AddComponent<T>();
    }

    public static bool Same(this Component c, Component other) {
      if (null == c || null == other) return false;

      return c.gameObject.Same(other.gameObject);
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

    public static void AddComponentsInChildren<T>(this IList<T> list, GameObject parent, Action<T> onEachComponent, bool includeInactive = true) {
      var components = parent.GetComponentsInChildren<T>(includeInactive);
      for (int i = 0; i < components.Length; ++i) {
        T comp = components[i];
        onEachComponent(comp);
        list.Add(comp);
      }
    }
  }
}