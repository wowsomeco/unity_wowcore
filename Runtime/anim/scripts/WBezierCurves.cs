using System;
using UnityEngine;
using Wowsome.Chrono;

namespace Wowsome.Anim {
  // WIP
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

    /// <summary>
    /// The current interpolation value between _from and _to
    /// </summary>    
    public Action<Vector2> Cur { get; set; }
    /// <summary>
    /// The current t.
    /// Useful if you want to do something when it's currently lerp ing
    /// </summary>
    public Action<float> Time { get; set; }
    public Action OnDone { get; set; }

    Vector2 _control;
    Vector2 _from;
    Vector2 _to;
    Timer _timer;
    Timer _delay;

    public WBezierCurve(Vector2 from, Vector2 to, Vector2 control, float duration, float delay = 0f) {
      _from = from;
      _to = to;
      _control = control;
      _timer = new Timer(duration);

      if (delay > 0f) {
        _delay = new Timer(delay);
      }
    }

    public bool Update(float dt) {
      if (null != _delay) {
        bool isDelayed = _delay.UpdateTimer(dt);
        if (isDelayed) {
          // return true if its being delayed
          return true;
        } else {
          _delay = null;
        }
      }

      if (null != _timer) {
        if (_timer.UpdateTimer(dt)) {
          float t = _timer.GetPercentage();
          Vector2 cur = Quadratic(_from, _control, _to, t);
          // notify
          Time?.Invoke(t);
          Cur?.Invoke(cur);
          // return true since it's still updating
          return true;
        } else {
          Done();
        }
      }

      return false;
    }

    void Done() {
      Time?.Invoke(1f);
      Cur?.Invoke(_to);
      OnDone?.Invoke();
      _timer = null;
    }
  }
}