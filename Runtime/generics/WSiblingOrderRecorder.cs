using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Generic {
  public class WSiblingOrderRecorder {
    public class Record {
      List<Transform> _children = new List<Transform>();

      public Record(Transform transform) {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i) {
          _children.Add(transform.GetChild(i));
        }
      }

      public void Reset() {
        for (int i = 0; i < _children.Count; ++i) {
          _children[i].SetSiblingIndex(i);
        }
      }
    }

    List<Record> _records = new List<Record>();

    public WSiblingOrderRecorder(GameObject root) {
      RecurseChildren(root.transform);
    }

    public void Reset() {
      for (int i = 0; i < _records.Count; ++i) {
        _records[i].Reset();
      }
    }

    void RecurseChildren(Transform t) {
      Record rec = new Record(t);
      _records.Add(rec);

      for (int i = 0; i < t.childCount; ++i) {
        RecurseChildren(t.GetChild(i));
      }
    }
  }
}