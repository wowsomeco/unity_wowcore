using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  /// <summary>
  /// The Auto play controller for the [[WAnimController]]  
  /// Ideally this is used for testing only,
  /// since you might want to have a control over the life cycle of each [[WAnimController]],
  /// e.g. supply your own hierarchical Init + Update methods instead of calling it on Awake() and Update() like how it is here.
  /// </summary>
  public class WAnimPlayer : MonoBehaviour {
    List<WAnimController> _animators = new List<WAnimController>();

    public void PlayAnim(string id) {
      _animators.ForEach(p => p.PlayAnim(id));
    }

    void Awake() {
      var animators = GetComponentsInChildren<WAnimController>();
      foreach (var anim in animators) {
        anim.InitAnim();
        _animators.Add(anim);
      }
    }

    void Update() {
      foreach (var anim in _animators) {
        anim.UpdateAnim(Time.deltaTime);
      }
    }
  }
}

