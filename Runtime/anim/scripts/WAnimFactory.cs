using System.Collections.Generic;
using UnityEngine;
using Wowsome.Tween;

namespace Wowsome.Anim {
  public static class WAnimFactory {
    public static InterpolationSequence<InterpolationVec2, Vector2> Pulse(float timeMultiplier = 1f) {
      Easing easing = Easing.Linear;

      InterpolationSequence<InterpolationVec2, Vector2> tweener = new InterpolationSequence<InterpolationVec2, Vector2>(
        new InterpolationVec2(new Timing(.09f * timeMultiplier, easing), new Vector2(1f, 1f), new Vector2(.9f, 1.1f)),
        new InterpolationVec2(new Timing(.08f * timeMultiplier, easing), new Vector2(.9f, 1.1f), new Vector2(1.05f, .95f)),
        new InterpolationVec2(new Timing(.06f * timeMultiplier, easing), new Vector2(1.05f, .95f), new Vector2(.95f, 1.05f)),
        new InterpolationVec2(new Timing(.05f * timeMultiplier, easing), new Vector2(.95f, 1.05f), new Vector2(1f, 1f))
      );

      return tweener;
    }

    public static InterpolationSequence<InterpolationFloat, float> Shake(float duration = .1f, float rotation = 5f, float count = 1, Easing easing = Easing.Linear) {
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