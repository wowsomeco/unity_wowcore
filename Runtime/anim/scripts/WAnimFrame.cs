using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Tween;

namespace Wowsome.Anim {
  [Serializable]
  public enum FrameType { Position, Scale, Rotation, Sprite, Alpha }

  [Serializable]
  public class AnimFrame {
    public Timing Timing => new Timing(duration, delay, easing, 0, false);

    public List<float> to;
    public float duration;
    public float delay;
    public Easing easing;
  }

  [Serializable]
  public class AnimStep {
    public WAnimClip clip;
    [Tooltip("the anim delta time multiplier. the more, the faster")]
    public float timeMultiplier = 1f;
    [Tooltip("the current value modifier e.g. set to {-1,1} to flip the x value. set to {1,1} if nothing should change")]
    public Vector2 valueMultiplier = Vector2.one;
    [Tooltip("how many times the animation should repeat e.g 1 = 1 repeat, 0 = no repeats, -1 = repeat forever")]
    public int repeat;
    public TweenType tweenType;
    [Tooltip("the random range delay for the first time the animation starts, only once")]
    public RangeData startDelay;
    [Tooltip("the random range delay between repeat")]
    public RangeData repeatDelay;
  }
}