using System.Collections.Generic;
using UnityEngine;
using Wowsome.Tween;

namespace Wowsome.Anim {
  public static class WAnimFactory {
    public class PulseOptions {
      public float ScaleX { get; set; } = 1.1f;
      public float ScaleY { get; set; } = .9f;
      public float TimeMultiplier { get; set; } = 1f;
      public int Count { get; set; } = 1;
      public Easing Easing { get; set; } = Easing.Linear;
    }

    public static InterpolationSequence<InterpolationVec2, Vector2> Pulse(PulseOptions options) {
      List<InterpolationVec2> tws = new List<InterpolationVec2>();

      float timeMultiplier = options.TimeMultiplier;
      Easing easing = options.Easing;
      Vector2 toScale = new Vector2(options.ScaleX, options.ScaleY);

      for (int i = 0; i < options.Count; ++i) {
        tws.Add(new InterpolationVec2(new Timing(.09f * timeMultiplier, easing), Vector2.one, toScale));
        tws.Add(new InterpolationVec2(new Timing(.05f * timeMultiplier, easing), toScale, Vector2.one));
      }

      InterpolationSequence<InterpolationVec2, Vector2> tweener = new InterpolationSequence<InterpolationVec2, Vector2>(tws);

      return tweener;
    }

    public static InterpolationSequence<InterpolationFloat, float> Shake(float duration = .1f, float rotation = 5f, int count = 1, Easing easing = Easing.Linear) {
      Timing timing = new Timing(duration, easing);
      List<InterpolationFloat> tws = new List<InterpolationFloat>();

      for (int i = 0; i < count; ++i) {
        tws.Add(new InterpolationFloat(timing, 0f, rotation));
        tws.Add(new InterpolationFloat(timing, -rotation, rotation));
        tws.Add(new InterpolationFloat(timing, rotation, 0f));
      }

      InterpolationSequence<InterpolationFloat, float> tweener = new InterpolationSequence<InterpolationFloat, float>(tws);

      return tweener;
    }
  }
}