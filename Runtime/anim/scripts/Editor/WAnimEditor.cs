using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Wowsome.Serialization;

namespace Wowsome.Anim {
  using EU = EditorUtils;

  [CustomEditor(typeof(WAnimatable))]
  public class WAnimEditor : Editor {
    public class AnimJson {
      public List<WAnimFrame> frames = new List<WAnimFrame>();

      public AnimJson(List<WAnimFrame> f) {
        frames = f;
      }
    }

    bool _recording = false;
    WAnimatable _target = null;
    FileBrowser _fileBrowser = new FileBrowser(Directory.GetCurrentDirectory());
    string _saveFileAs = string.Empty;

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
        /*
        EU.Btn("Start Recording", () => {
          _recording = true;
          // _curFrame = new AnimFrame();
        });
        */

        EU.Btn("Play", () => {
          _target.Play();
          EditorApplication.update += DoUpdate;
        });

        EU.Btn("Stop", () => {
          EditorApplication.update -= DoUpdate;
        });

        _saveFileAs = EditorGUILayout.TextField("Save As", _saveFileAs);
        if (!_saveFileAs.IsEmpty()) {
          EU.Btn("Save", () => SaveModel(_saveFileAs));
        }
      }

      if (_recording) {
        EU.Btn("Record", () => {
          // TODO : handle recording
          // Image img = _target.GetComponent<Image>();
          // _target.TryAddSprite(img.sprite);
          // _curFrame.FromImage(img, _selectedAnim.frames.Last(), _posTiming, _scaleTiming, _rotTiming);
          // _selectedAnim.frames.Add(_curFrame);
          // _curFrame = new AnimFrame();
          EU.SetSceneDirty();
        });

        EU.Btn("Stop Recording", () => {
          _recording = false;
        });
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
      serializer.Save<AnimJson>(new AnimJson(_target.Frames), StringExt.Concat(resourceFolder, "/", fname));
      UnityEditor.AssetDatabase.Refresh();
    }
  }
}

