using UnityEngine;

namespace Wowsome.TwoDee {
  [ExecuteInEditMode]
  public class WScaleFitCanvas : MonoBehaviour {
    public Transform canvasObject;
    public Transform rootObject;

    void ReScale() {
      if (null != canvasObject) {
        rootObject.localScale = canvasObject.localScale;
      }
    }

    void Awake() {
      ReScale();
    }

    void Update() {
#if UNITY_EDITOR      
      ReScale();
#endif
    }
  }
}

