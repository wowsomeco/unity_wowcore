using System;
using UnityEngine;

namespace Wowsome.Anim {
  /// <summary>
  /// Unity animation listener that can be added programmatically
  /// 
  /// Attach this script to the Gameobject that has [[Animator]] 
  /// that you want to observe to  
  /// </summary>
  [RequireComponent(typeof(Animator))]
  public class WAnimEvents : MonoBehaviour {
    /// <summary>
    /// Gets triggered when the animation just starts
    /// </summary>
    /// <value>The animation clip name</value>       
    public Action<string> OnAnimStart { get; set; }
    /// <summary>
    /// Gets triggered when the animation completes
    /// </summary>
    /// <value>The animation clip name</value>
    public Action<string> OnAnimComplete { get; set; }

    Animator _animator;

    /// <summary>
    /// Gets triggered by AnimationEvent everytime the clip starts playing
    /// </summary>
    public virtual void HandleAnimationStart(string name) {
      OnAnimStart?.Invoke(name);
    }

    /// <summary>
    /// Gets triggered by AnimationEvent everytime the clip completes playing
    /// </summary>    
    public virtual void HandleAnimationComplete(string name) {
      OnAnimComplete?.Invoke(name);
    }

    void CreateAnimEvent(AnimationClip clip, float time, string funcName) {
      AnimationEvent animEv = new AnimationEvent();
      animEv.time = time;
      animEv.functionName = funcName;
      animEv.stringParameter = clip.name;

      clip.AddEvent(animEv);
    }

    void Awake() {
      _animator = GetComponent<Animator>();

      for (int i = 0; i < _animator.runtimeAnimatorController.animationClips.Length; ++i) {
        AnimationClip clip = _animator.runtimeAnimatorController.animationClips[i];

        CreateAnimEvent(clip, 0f, "HandleAnimationStart");
        CreateAnimEvent(clip, clip.length, "HandleAnimationComplete");
      }
    }
  }
}
