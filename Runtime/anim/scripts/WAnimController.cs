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
      [Tooltip("the anim id that will get played on start / end. When the item is more than 1, it will get randomized")]
      public List<string> animId = new List<string>();

      public string PickRandom() => animId.IsEmpty() ? null : animId.PickRandom().Trim();
    }

    public Action<string> OnAnimEnd { get; set; }
    public string CurPlayingAnimId { get; private set; }

    [Tooltip("the anim that gets played on init. When the item is more than 1, it will get randomized")]
    public List<string> defaultAnimId = new List<string>();
    [Tooltip("the next animation that gets played when a new anim id is about to start (pre-start)")]
    public List<AnimTrigger> startTriggers = new List<AnimTrigger>();
    [Tooltip("the next animation that gets played once the current one has completed (post-complete)")]
    public List<AnimTrigger> endTriggers = new List<AnimTrigger>();

    Dictionary<string, List<WAnimatorBase>> _animators = new Dictionary<string, List<WAnimatorBase>>();
    List<WAnimatorBase> _playings = new List<WAnimatorBase>();
    Queue<string> _nextAnimIds = new Queue<string>();
    bool _playing = false;

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
        PlayAnim(defaultAnimId.PickRandom());
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

    public void PlayAnim(string id, bool checkStartTrigger = true) {
      if (!id.IsEmpty() && _animators.ContainsKey(id)) {
        _playing = true;
        // only play anim if the start trigger doesnt interfere        
        if (checkStartTrigger && TriggerStartAnim(id)) return;
        // play the anim id
        SetCurPlaying(id);
      }
    }

    bool TriggerStartAnim(string nextAnimId) {
      AnimTrigger startTrigger = startTriggers.Find(x => x.ids.Contains(nextAnimId));
      // check if start trigger is valid,
      // if so, play it
      if (null != startTrigger) {
        string animId = startTrigger.PickRandom();
        // set the next anim id so that once the start trigger anim id is done,
        // we play this one by then
        // right now, it will all the previous _nextAnimIds, if any
        _nextAnimIds.Clear();
        _nextAnimIds.Enqueue(nextAnimId);
        // play the startTrigger.animId
        // set cur playing as the startTrigger.animId only if they're different
        if (CurPlayingAnimId != animId) {
          SetCurPlaying(animId);
        }

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
        // broadcast on end
        OnAnimEnd?.Invoke(CurPlayingAnimId);
        // check if there is a debt of next anim id,
        // if so, play that one first
        // otherwise check the end trigger and play that one if any
        if (_nextAnimIds.Count > 0) {
          string next = _nextAnimIds.Dequeue();
          PlayAnim(next, false);
        } else {
          AnimTrigger endTrigger = endTriggers.Find(x => x.ids.Contains(CurPlayingAnimId));
          if (null != endTrigger) {
            PlayAnim(endTrigger.PickRandom());
          }
        }
      }
    }

    void SetCurPlaying(string animId) {
      CurPlayingAnimId = animId;
      _playings = _animators[CurPlayingAnimId];
      _playings.ForEach(p => p.Play());
    }
  }
}