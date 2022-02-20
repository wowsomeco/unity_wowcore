using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Tween;

namespace Wowsome.Anim {
  public static class WAnimFactory {
    public class PulseOptions {
      public Vector2 InitScale { get; set; } = Vector2.one;
      public float ScaleDelta { get; set; } = .05f;
      public float TimeMultiplier { get; set; } = 1f;
      public int Count { get; set; } = 1;
      public Easing Easing { get; set; } = Easing.Linear;
    }

    public static InterpolationSequence<InterpolationVec2, Vector2> Pulse(PulseOptions options = null) {
      if (null == options) {
        options = new PulseOptions();
      }

      List<InterpolationVec2> tws = new List<InterpolationVec2>();

      float timeMultiplier = options.TimeMultiplier;
      Easing easing = options.Easing;
      Vector2 toScale = new Vector2(options.InitScale.x + options.ScaleDelta, options.InitScale.y + options.ScaleDelta);

      for (int i = 0; i < options.Count; ++i) {
        tws.Add(new InterpolationVec2(new Timing(.09f * timeMultiplier, easing), options.InitScale, toScale));
        tws.Add(new InterpolationVec2(new Timing(.05f * timeMultiplier, easing), toScale, options.InitScale));
      }

      InterpolationSequence<InterpolationVec2, Vector2> tweener = new InterpolationSequence<InterpolationVec2, Vector2>(tws);

      return tweener;
    }

    public class ShakeRotateOptions {
      public float Duration { get; set; } = .1f;
      public float Rotation { get; set; } = 5f;
      public int Count { get; set; } = 1;
      public Easing Easing { get; set; } = Easing.Linear;
    }

    public static InterpolationSequence<InterpolationFloat, float> ShakeRotate(ShakeRotateOptions options = null) {
      if (null == options) options = new ShakeRotateOptions();

      float rotation = options.Rotation;

      Timing timing = new Timing(options.Duration, options.Easing);
      List<InterpolationFloat> tws = new List<InterpolationFloat>();

      for (int i = 0; i < options.Count; ++i) {
        tws.Add(new InterpolationFloat(timing, 0f, rotation));
        tws.Add(new InterpolationFloat(timing, rotation, -rotation));
        tws.Add(new InterpolationFloat(timing, -rotation, 0f));
      }

      InterpolationSequence<InterpolationFloat, float> tweener = new InterpolationSequence<InterpolationFloat, float>(tws);

      return tweener;
    }

    public static InterpolationSequence<InterpolationVec2, Vector2> ShakeMove(Vector2 initPos, float duration, IEnumerable<Vector2> offsets) {
      Timing timing = new Timing(duration);

      Vector2 from = initPos;
      List<InterpolationVec2> tws = new List<InterpolationVec2>();

      foreach (Vector2 offset in offsets) {
        Vector2 to = from.Add(offset);

        InterpolationVec2 tw = new InterpolationVec2(
          timing,
          from,
          to
        );

        tws.Add(tw);

        from = to;
      }

      // add last interpolation to go back to init pos again
      tws.Add(new InterpolationVec2(timing, from, initPos));

      return new InterpolationSequence<InterpolationVec2, Vector2>(tws);
    }

    public class BezierMoveOptions {
      public Vector2 From { get; set; }
      public Vector2 To { get; set; }
      public float Duration { get; set; } = .5f;
      public float Delay { get; set; } = 0f;
      public Vector2 ControlPoint { get; set; } = Vector2.one;
      public Action<Vector2> CurPos { get; set; }
      public Action OnDone { get; set; }
    }

    public static InterpolationFloat BezierMove(BezierMoveOptions options) {
      var tw = new InterpolationFloat(0f, 1f, options.Duration, options.Delay);

      tw.OnLerp += t => {
        Vector2 curPos = WBezierCurve.Quadratic(
          options.From,
          options.From.LerpWithOffset(options.To, .5f, new Vector2(3f, 1f)),
          options.To,
          t
        );

        options.CurPos?.Invoke(curPos);
      };

      tw.OnDone += () => {
        options.OnDone?.Invoke();
      };

      return tw;
    }
  }
}