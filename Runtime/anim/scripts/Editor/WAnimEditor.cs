using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Wowsome.Serialization;

namespace Wowsome.Anim {
  using EU = EditorUtils;

  [CustomEditor(typeof(WAnimatable))]
  public class WAnimEditor : Editor {
    public class Initial {
      public Vector2 Pos { get; private set; }
      public Vector2 Scale { get; private set; }
      public float Rotation { get; private set; }
      public Vector2 Pivot { get; private set; }

      RectTransform _rt;

      public Initial(RectTransform target) {
        _rt = target;

        Pos = target.Pos();
        Scale = target.Scale();
        Rotation = target.Rotation();
        Pivot = target.pivot;
      }

      public void Revert() {
        _rt.SetPos(Pos);
        _rt.SetScale(Scale);
        _rt.SetRotation(Rotation);
        _rt.SetPivot(Pivot);
      }
    }

    public class AnimJson {
      public List<WAnimFrame> frames = new List<WAnimFrame>();

      public AnimJson(List<WAnimFrame> f) {
        frames = f;
      }
    }

    bool _recording = false;
    WAnimatable _target = null;
    Initial _initial = null;
    FileBrowser _fileBrowser = new FileBrowser(Directory.GetCurrentDirectory());
    string _saveFileAs = string.Empty;

    void OnDisable() {
      if (null != _initial) _initial.Revert();

      EditorApplication.update -= DoUpdate;
    }

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      _target = (WAnimatable)target;

      EU.VPadding(() => {
        _fileBrowser.Build("Browse anim json", "json", filepath => {
          string txt = File.ReadAllText(filepath);
          AnimJson aj = JsonUtility.FromJson<AnimJson>(txt);
          if (aj.frames.Count > 0) {
            _target.Frames = aj.frames;
          }

          EU.SetSceneDirty();
        });
      });

      if (!_recording) {
        EU.Btn("Play", () => {
          _initial = new Initial(_target.GetComponent<RectTransform>());
          _target.Play();
          EditorApplication.update += DoUpdate;
        });

        EU.Btn("Reset", () => {
          OnDisable();
        });

        _saveFileAs = EditorGUILayout.TextField("Save As", _saveFileAs);
        if (!_saveFileAs.IsEmpty()) {
          EU.Btn("Save", () => SaveModel(_saveFileAs));
        }
      }
    }

    void DoUpdate() {
      if (!_target.Animate(0.02f)) {
        EditorApplication.update -= DoUpdate;
      }
    }

    void SaveModel(string fname) {
      string resourceFolder = Directory.GetCurrentDirectory() + "/Assets/Resources";
      Directory.CreateDirectory(resourceFolder);
      IDataSerializer serializer = new JsonDataSerializer();
      serializer.Save<AnimJson>(new AnimJson(_target.Frames), StringExt.Concat(resourceFolder, "/", fname, ".json"));
      UnityEditor.AssetDatabase.Refresh();
    }
  }
}

