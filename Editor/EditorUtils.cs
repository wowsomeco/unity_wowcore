using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wowsome {
  using EU = EditorUtils;

  public static class EditorUtils {
    public static void ApplyPrefab(this GameObject go) {
      GameObject prefab = (GameObject)PrefabUtility.InstantiatePrefab(go);
      PrefabUtility.ApplyPrefabInstance(prefab, InteractionMode.AutomatedAction);
      AssetDatabase.SaveAssets();
      MonoBehaviour.DestroyImmediate(prefab.gameObject);

      UnityEditor.AssetDatabase.Refresh();
    }

    public static void ApplyPrefab(this Component c) {
      c.gameObject.ApplyPrefab();
    }

    public static void SetSceneDirty() {
      Scene curScene = SceneManager.GetActiveScene();
      EditorSceneManager.MarkSceneDirty(curScene);
      EditorSceneManager.SaveScene(curScene);
    }

    public static void Refresh() {
      UnityEditor.AssetDatabase.Refresh();
    }

    public static void Btn(string txt, Action onClick, params GUILayoutOption[] options) {
      if (GUILayout.Button(txt, options)) onClick();
    }

    public static void BtnWithAlert(string txt, Action onClick, params GUILayoutOption[] options) {
      if (GUILayout.Button(txt, options)) Alert(onClick);
    }

    public static void Alert(Action onYes, string content = "You Sure?", string title = "") {
      if (EditorUtility.DisplayDialog(title, content, "Yes", "No")) onYes();
    }

    public static void VSpacing(float pixels = 10f) {
      GUILayout.Space(pixels);
    }

    public static void VPadding(Action render, float pixels = 10f) {
      VSpacing(pixels);
      render();
      VSpacing(pixels);
    }

    public static void HGroup(Action render) {
      GUILayout.BeginHorizontal();
      render();
      GUILayout.EndHorizontal();
    }

    public static string TextfieldWithOk(string label, string value, Action<string> onSubmit, string submitLbl = "OK") {
      EU.HGroup(() => {
        value = EditorGUILayout.TextField(label, value);
        EU.Btn(submitLbl, () => onSubmit.Invoke(value));
      });
      return value;
    }

    public static Rect Resize(this Rect rect, Vector2 size) {
      Rect r = new Rect(rect);
      r.size = size;
      return r;
    }

    public static Rect ResizeWidth(this Rect rect, float w) {
      return rect.Resize(new Vector2(w, rect.size.y));
    }
  }

  #region Toggleable

  public class ToggleState {
    public bool State { get; set; }
    public int Idx {
      get { return State ? 1 : 0; }
    }

    public ToggleState(bool state = false) {
      State = state;
    }

    public void Toggle() {
      State = !State;
    }
  }

  public class Toggleable {
    public class Item {
      public string Text;
      public Action Build;

      public Item(string t, Action b) {
        Text = t;
        Build = b;
      }
    }

    public void Build(ToggleState state, List<Item> items, Action<ToggleState> onToggle) {
      EU.HGroup(() => {
        items.Loop((it, idx) => {
          bool selected = state.Idx == idx;
          var style = new GUIStyle(GUI.skin.button);
          style.normal.textColor = selected ? Color.blue : Color.black;
          if (GUILayout.Button(it.Text, style) && !selected) {
            onToggle(new ToggleState(!state.State));
          }
        });
      });

      Item item = items[state.Idx];
      EU.VPadding(() => item.Build());
    }
  }

  #endregion  

  #region Selectable

  public class SelectState<T> where T : class {
    public int Idx;
    public T Model;

    public bool Selected {
      get { return Idx > -1; }
    }

    public SelectState() : this(-1, null) { }

    public SelectState(int idx, T model) {
      Idx = idx;
      Model = model;
    }

    public void Reset() {
      Idx = -1;
      Model = null;
    }
  }

  public class Dropdown<T> where T : class {
    public bool Build(string lbl, T value, List<T> origins, ListExt.Mapper<T, string> mapper, Action<SelectState<T>> onSelected = null) {
      int cur = origins.IndexOf(value);
      int selected = EditorGUILayout.Popup(lbl, cur, origins.Map(x => mapper(x)).ToArray());
      if (cur != selected) {
        onSelected?.Invoke(new SelectState<T>(selected, origins[selected]));
        cur = selected;
      }

      return cur >= 0;
    }
  }

  public class Menu<T> where T : class, new() {
    public class AddAction {
      public string Label;
      public Action<int> OnAdd;

      public AddAction(string lbl, Action<int> onAdd = null) {
        Label = lbl;
        OnAdd = onAdd;
      }
    }

    public class DeleteAction {
      public string Label;
      public Action<SelectState<T>> OnDelete;
      public Action<int> OnDeleted;

      public DeleteAction(Action<int> onDeleted = null, Action<SelectState<T>> onDel = null, string lbl = "X") {
        Label = lbl;
        OnDelete = onDel;
        OnDeleted = onDeleted;
      }
    }

    public class MoveAction {
      public string Label;
      public Action<int> Move;
      public Action OnMoved;
      public MoveAction(Action onMoved = null, Action<int> move = null, string lbl = "V") {
        OnMoved = onMoved;
        Move = move;
        Label = lbl;
      }
    }

    public class BuildCallback {
      public string Label;
      public T Value = null;
      public List<T> Origins;
      public ListExt.Mapper<T, string> Mapper;
      public Action<SelectState<T>> OnSelected = null;
      public AddAction AddAction = null;
      public DeleteAction DelAction = null;
      public MoveAction MoveAction = null;
      public Action<T> Prefix = null;
      public Action<T> Suffix = null;

      public BuildCallback(string lbl, List<T> or, ListExt.Mapper<T, string> mapper, Action<SelectState<T>> os) {
        Label = lbl;
        Origins = or;
        Mapper = mapper;
        OnSelected = os;
      }

      public BuildCallback(string lbl, List<T> or, ListExt.Mapper<T, string> mapper, Action<SelectState<T>> os, DeleteAction delAction)
      : this(lbl, or, mapper, os) {
        DelAction = delAction;
      }

      public BuildCallback(string lbl, T v, List<T> or, ListExt.Mapper<T, string> mapper, Action<SelectState<T>> os)
      : this(lbl, or, mapper, os) {
        Value = v;
      }

      public BuildCallback(string lbl, T v, List<T> or, ListExt.Mapper<T, string> mapper, Action<SelectState<T>> os, AddAction addAction, DeleteAction delAction)
      : this(lbl, v, or, mapper, os) {
        AddAction = addAction;
        DelAction = delAction;
      }

      public BuildCallback(string lbl, T v, List<T> or, ListExt.Mapper<T, string> mapper, Action<SelectState<T>> os, AddAction addAction, DeleteAction delAction, MoveAction moveAction)
      : this(lbl, v, or, mapper, os, addAction, delAction) {
        MoveAction = moveAction;
      }

      public BuildCallback(string lbl, T v, List<T> or, ListExt.Mapper<T, string> mapper, Action<SelectState<T>> os, AddAction addAction, DeleteAction delAction, Action<T> prefix)
      : this(lbl, v, or, mapper, os, addAction, delAction) {
        Prefix = prefix;
      }

      public BuildCallback(string lbl, T v, List<T> or, ListExt.Mapper<T, string> mapper, Action<SelectState<T>> os, AddAction addAction, DeleteAction delAction, Action<T> prefix, Action<T> suffix)
      : this(lbl, v, or, mapper, os, addAction, delAction, prefix) {
        Suffix = suffix;
      }
    }

    public enum Align { V, H }

    Align m_alignment;

    public Menu(bool canMove = false, Align align = Align.V) {
      m_alignment = align;
    }

    public bool Build(BuildCallback cb) {
      int cur = cb.Origins.IndexOf(cb.Value);

      EU.VPadding(() => {
        if (null != cb.AddAction && GUILayout.Button(cb.AddAction.Label)) {
          T item = new T();
          cb.Origins.Add(item);
          cb.AddAction.OnAdd?.Invoke(cb.Origins.Count);
        }

        EditorGUILayout.LabelField(cb.Label, EditorStyles.boldLabel);

        if (m_alignment == Align.V) { GUILayout.BeginVertical(); } else { GUILayout.BeginHorizontal(); }

        cb.Origins.Loop((t, idx) => {
          EU.HGroup(() => {
            bool isSelected = idx == cur;
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = isSelected ? Color.blue : Color.black;

            if (null != cb.Prefix) cb.Prefix(t);

            if (GUILayout.Button(cb.Mapper(t), style)) {
              cb.OnSelected?.Invoke(new SelectState<T>(idx, cb.Origins[idx]));
              cur = idx;
            }

            if (null != cb.Suffix) cb.Suffix(t);

            if (null != cb.MoveAction && cb.Origins.Count > idx + 1) {
              EU.BtnWithAlert("V", () => {
                if (cb.MoveAction.OnMoved != null) { cb.MoveAction.Move(idx); } else { cb.Origins.Swap(idx, idx + 1); }
                cb.MoveAction.OnMoved?.Invoke();
              }, GUILayout.Width(20f));
            }

            if (null != cb.DelAction) {
              EU.BtnWithAlert(cb.DelAction.Label, () => {
                if (null != cb.DelAction.OnDelete) { cb.DelAction.OnDelete(new SelectState<T>(idx, cb.Origins[idx])); } else { cb.Origins.RemoveAt(idx); }
                cb.DelAction.OnDeleted?.Invoke(idx);
              }, GUILayout.Width(20f));
            }
          });
        });

        if (m_alignment == Align.V) { GUILayout.EndVertical(); } else { GUILayout.EndHorizontal(); }
      });

      return cur > -1;
    }

    public bool Build(string lbl, T value, List<T> origins, ListExt.Mapper<T, string> mapper, Action<SelectState<T>> onSelected = null, AddAction addAction = null, DeleteAction delAction = null) {
      return Build(new BuildCallback(
        lbl,
        value,
        origins,
        mapper,
        onSelected,
        addAction,
        delAction
      ));
    }
  }

  #endregion

  public class FileBrowser {
    string m_lastPath = string.Empty;

    public FileBrowser(string defaultPath = "") {
      m_lastPath = defaultPath;
    }

    public void Build(string btnTxt, string acceptedFile, Action<string> onSelected) {
      if (GUILayout.Button(btnTxt)) {
        string path = EditorUtility.OpenFilePanel(btnTxt, string.IsNullOrEmpty(m_lastPath) ? "~/" : m_lastPath, acceptedFile);
        if (!string.IsNullOrEmpty(path)) {
          m_lastPath = path;
          onSelected(path);
        }
      }
    }
  }

  public class FolderBrowser {
    string m_lastPath = string.Empty;

    public void Build(string btnTxt, Action<string[]> onSelected) {
      if (GUILayout.Button(btnTxt)) {
        string path = EditorUtility.OpenFolderPanel(btnTxt, string.IsNullOrEmpty(m_lastPath) ? "~/" : m_lastPath, "");
        if (!string.IsNullOrEmpty(path)) {
          m_lastPath = path;
          string[] files = Directory.GetFiles(path);
          onSelected(files);
        }
      }
    }
  }
}
