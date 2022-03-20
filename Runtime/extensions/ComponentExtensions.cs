using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome {
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

    public static T SetVisible<T>(this T c, bool flag) where T : Component {
      c.gameObject.SetActive(flag);
      return c;
    }

    public static List<T> GetComponentsWithoutSelf<T>(this Component obj, bool includeInactive) where T : Component {
      List<T> components = new List<T>();
      var comps = obj.GetComponentsInChildren<T>(includeInactive);
      // might be slow though ...
      foreach (T c in comps) {
        // dont include self
        if (!c.Same(obj)) {
          components.Add(c);
        }
      }
      return components;
    }

    public static T Clone<T>(this Component c, Transform parent, string name = "") {
      return c.gameObject.Clone<T>(parent, name);
    }

    public static Vector3 WorldPos(this Component c) {
      return c.transform.position;
    }

    public static Vector3 LocalPos(this Component c) {
      return c.transform.localPosition;
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

    public static T GetNearestDistance<T>(this IList<T> list, Vector2 pos) where T : Component {
      if (list.IsEmpty()) return null;

      T nearest = list.First();
      float distance = Vector2.Distance(pos, nearest.WorldPos());

      foreach (T comp in list) {
        float d = Vector2.Distance(pos, comp.WorldPos());

        if (d < distance) {
          distance = d;
          nearest = comp;
        }
      }

      return nearest;
    }
  }
}