using System;
using UnityEngine;
using Wowsome.Chrono;
using Wowsome.Tween;

namespace Wowsome.Anim {
  [Serializable]
  public class Timing {
    public double dur;
    public double dly;
    public Easing easing;
    public int repeat;
    public bool yoyo;

    public Timing(double duration, double delay, Easing eas, int rep, bool yo) {
      dur = duration;
      dly = delay;
      easing = eas;
      repeat = rep;
      yoyo = yo;
    }

    public Timing(double duration, Easing easing) : this(duration, 0f, easing, 0, false) { }

    public Timing(double duration) : this(duration, Easing.Linear) { }
  }

  [Serializable]
  public class Interpolation {
    Timing _timing = null;
    Timer _timer = null;
    Timer _delay = null;
    int _counter = 0;

    public int Percent {
      get {
        if (null != _timer) {
          return (int)(Time * 100f);
        }

        return 100;
      }
    }

    public float Time {
      get {
        if (null != _timer) {
          float t = CEasings.GetEasing(_timer.GetPercentage(), _timing.easing);
          // reverse t if it's yoyo and counter is odd
          // e.g when counter is 0 it goes forward, 1 goes backwards, etc.
          if (_timing.yoyo && _counter % 2 != 0) t = 1f - t;
          return t;
        }
        return 1f;
      }
    }

    public Interpolation(Timing timing) {
      _timing = timing;
    }

    public void Start() {
      _counter = 0;
      _timer = new Timer((float)_timing.dur);
      if (_timing.dly > 0f) _delay = new Timer((float)_timing.dly);
    }

    public bool Update(float dt) {
      if (null != _delay) {
        if (!_delay.UpdateTimer(dt)) {
          _delay = null;
        }
        return true;
      }

      if (null != _timer && !_timer.UpdateTimer(dt)) {
        if (_timing.repeat > 0) {
          ++_counter;
          if (_counter <= _timing.repeat || _timing.repeat == -1) {
            _timer.Reset();
          } else {
            _timer = null;
          }
        } else {
          _timer = null;
        }
      }

      return _timer != null;
    }
  }

  [Serializable]
  public class InterpolationFloat : Interpolation {
    public float from;
    public float to;

    public InterpolationFloat(Timing timing, float f, float t) : base(timing) {
      from = f;
      to = t;
    }

    public float Lerp() {
      return Mathf.Lerp(from, to, Time);
    }
  }

  [Serializable]
  public class InterpolationInt : Interpolation {
    public int from;
    public int to;

    public InterpolationInt(Timing timing, int f, int t) : base(timing) {
      from = f;
      to = t;
    }

    public int Lerp() {
      return (int)Mathf.Lerp(from, to, Time);
    }
  }

  [Serializable]
  public class InterpolationVec : Interpolation {
    public Vector2 from;
    public Vector2 to;

    public InterpolationVec(Timing timing, Vector2 f, Vector2 t) : base(timing) {
      from = f;
      to = t;
    }

    public Vector2 Lerp() {
      return Vector2.Lerp(from, to, Time);
    }
  }
}