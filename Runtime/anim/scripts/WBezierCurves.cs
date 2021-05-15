using System;
using UnityEngine;
using Wowsome.Chrono;

namespace Wowsome.Anim {
  public class WBezierCurve {
    public static Vector2 Quadratic(Vector2 p0, Vector2 p1, Vector2 p2, float t) {
      float u = 1 - t;
      float tt = t * t;
      float uu = u * u;
      Vector2 p = uu * p0;
      p += 2 * u * t * p1;
      p += tt * p2;
      return p;
    }

    public static Vector2 Cubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t) {
      return (1 - t) * ((1 - t) * (p0 + t * (p1 - p0)) + t * (p1 + t * (p2 - p1))) + t * ((1 - t) * (p1 + t * (p2 - p1)) + t * (p2 + t * (p3 - p2)));
    }

    Vector2 _controlOffset;
    Func<Vector2> _from;
    Vector2 _target;
    Timer _timer;

    public WBezierCurve(Func<Vector2> from, Vector2 to, Vector2 control, float duration) {
      _from = from;
      _target = to;
      _controlOffset = control;
      _timer = new Timer(duration);
    }

    public bool Update(float dt, out Vector2 curPos) {
      curPos = Vector2.zero;

      if (null != _timer) {
        if (_timer.UpdateTimer(dt)) {
          float t = _timer.GetPercentage();
          Vector2 fr = _from();
          Vector2 controlPoint = Vector2.Lerp(fr, _target, t).Add(_controlOffset * (1f - t));
          curPos = Quadratic(fr, controlPoint, _target, t);
          return true;
        } else {
          _timer = null;
        }
      }

      return false;
    }
  }
}