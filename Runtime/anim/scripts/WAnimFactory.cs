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
  }
}