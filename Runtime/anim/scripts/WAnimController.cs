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
    /// <summary>
    /// The anim event that gets triggered,
    /// either for pre-start or post-complete playing anim.
    /// </summary>
    [Serializable]
    public sealed class AnimTrigger {
      [Tooltip("the anim id(s) reference")]
      public List<string> ids;
      [Tooltip("the anim id that will get played")]
      public string animId;
    }

    [Tooltip("the anim that gets played on init, leave it to null / blank if nothing")]
    public string defaultAnimId;
    [Tooltip("the next animation that gets played when a new anim id is about to start (pre-start)")]
    public List<AnimTrigger> startTriggers = new List<AnimTrigger>();
    [Tooltip("the next animation that gets played once the current one has completed (post-complete)")]
    public List<AnimTrigger> endTriggers = new List<AnimTrigger>();

    Dictionary<string, List<WAnimatorBase>> _animators = new Dictionary<string, List<WAnimatorBase>>();
    List<WAnimatorBase> _playings = new List<WAnimatorBase>();
    string _curPlayingAnimId;
    string _nextAnimId = null;
    bool _playing = false;

    public void PlayAnim(string id) {
      if (_animators.ContainsKey(id)) {
        _playing = true;
        // only play anim if the start trigger doesnt interfere
        if (!TriggerStartAnim(id)) {
          _curPlayingAnimId = id;
          // play the cur anim id
          _playings = _animators[_curPlayingAnimId];
          _playings.ForEach(p => p.Play());
        }
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
      for (int i = 0; i < _playings.Count; ++i) {
        if (_playings[i].Animate(dt)) {
          isAnimating = true;
        }
      }
      // play next sequence when animating is done and the sequence has not been played yet
      if (!isAnimating) {
        TriggerEndAnim();
      }
    }

    bool TriggerStartAnim(string nextAnimId) {
      // make sure start anim only gets triggered once
      if (!_nextAnimId.IsEmpty()) {
        _curPlayingAnimId = string.Copy(_nextAnimId);
        _nextAnimId = null;
        return false;
      }

      AnimTrigger startTrigger = startTriggers.Find(x => x.ids.Contains(nextAnimId));
      // check if start trigger is valid,
      // if so, play it
      if (null != startTrigger && _animators.ContainsKey(startTrigger.animId)) {
        // set cur playing as the startTrigger.animId                
        _curPlayingAnimId = startTrigger.animId;
        // set the next anim id so that once the start trigger anim id is done,
        // we play this one by then
        _nextAnimId = nextAnimId;
        // play the startTrigger.animId
        _playings = _animators[_curPlayingAnimId];
        _playings.ForEach(p => p.Play());

        return true;
      }

      return false;
    }

    /// <summary>
    /// Gets called once the cur playing anim is done
    /// </summary>
    void TriggerEndAnim() {
      if (_playing) {
        // make sure only enter here once
        _playing = false;
        // check if there is a debt of next anim id,
        // if so, play that one first
        // otherwise check the end trigger and play that one if any
        if (!_nextAnimId.IsEmpty()) {
          PlayAnim(_nextAnimId.Trim());
        } else {
          AnimTrigger endTrigger = endTriggers.Find(x => x.ids.Contains(_curPlayingAnimId));
          if (null != endTrigger) {
            PlayAnim(endTrigger.animId.Trim());
          }
        }
      }
    }
  }
}