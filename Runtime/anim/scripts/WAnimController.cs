using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  /// <summary>
  /// The animation controller.
  /// It is the lightweight yet more flexible as compared to Unity Animator.
  /// 
  /// Attach this script to a gameobject
  /// It will then iterate over the children and find all the [[WAnimatorBase]]
  /// 
  /// This class needs to get called by another controller,
  /// meaning that it doesn't have any Awake() nor Update()
  /// You need to have a reference to this class somewhere and call them manually
  /// This is intentional since you can control when it should get initialized, 
  /// as well as the update phase e.g. dont update when game is paused, etc.
  /// </summary>
  public class WAnimController : MonoBehaviour {
    [Serializable]
    public sealed class Sequence {
      [Tooltip("the on complete anim id(s) reference")]
      public List<string> ids;
      [Tooltip("the anim id that gets played after the id has completed")]
      public string onCompleteId;
    }

    [Tooltip("the anim that gets played on init, leave it to null / blank if nothing")]
    public string defaultAnimId;
    [Tooltip("the next animation that gets played once the current one has completed")]
    public List<Sequence> sequences = new List<Sequence>();

    Dictionary<string, List<WAnimatorBase>> _animators = new Dictionary<string, List<WAnimatorBase>>();
    List<WAnimatorBase> _playings = new List<WAnimatorBase>();
    string _curPlayingAnimId;
    bool _hasPlayedSequence = false;

    public void PlayAnim(string id) {
      // TODO: need to somehow reset the prev id state to the initial value(s),
      // otherwise if this function gets called, it might look broken
      // e.g. say that prev anim sets rotation to -45, then when this gets called,
      // the rotation wont reset.
      if (_animators.ContainsKey(id)) {
        _curPlayingAnimId = id;
        _hasPlayedSequence = false;

        _playings.ForEach(p => p.Stop());
        _playings = _animators[_curPlayingAnimId];
        _playings.ForEach(p => p.Play());
      }
    }

    public void InitAnim() {
      var animators = GetComponentsInChildren<WAnimatorBase>();
      foreach (var anim in animators) {
        anim.InitAnimator();
        string animId = anim.id;
        if (!_animators.ContainsKey(animId)) {
          _animators[animId] = new List<WAnimatorBase>();
        }
        _animators[animId].Add(anim);
      }

      if (!defaultAnimId.IsEmpty()) {
        PlayAnim(defaultAnimId);
      }
    }

    public void UpdateAnim(float dt) {
      if (_playings.Count == 0) return;

      bool isAnimating = false;
      foreach (var anim in _playings) {
        if (anim.Animate(dt)) {
          isAnimating = true;
        }
      }
      // play next sequence when animating is done and the sequence has not been played yet
      if (!isAnimating && !_hasPlayedSequence) {
        _hasPlayedSequence = true;
        Sequence seq = sequences.Find(x => x.ids.Contains(_curPlayingAnimId));
        if (null != seq) {
          PlayAnim(seq.onCompleteId.Trim());
        }
      }
    }
  }
}