using System;
using UnityEngine;
using Wowsome.Chrono;
using Wowsome.Generic;
using Wowsome.Tween;

namespace Wowsome.Anim {
  [Serializable]
  public class Timing {
    public float dur;
    public float dly;
    public Easing easing;
    public int repeat;
    public bool yoyo;

    public bool PingPong {
      get { return yoyo && repeat > 0 && (repeat % 2 != 0); }
    }

    public Timing(float duration, float delay, Easing eas, int rep, bool yo) {
      dur = duration;
      dly = delay;
      easing = eas;
      repeat = rep;
      yoyo = yo;
    }

    public Timing(float duration, Easing easing) : this(duration, 0f, easing, 0, false) { }

    public Timing(float duration, float delay, Easing easing) : this(duration, delay, easing, 0, false) { }

    public Timing(float duration) : this(duration, Easing.Linear) { }
  }

  public class Interpolation {
    public WObservable<int> Percent { get; private set; }
    public WObservable<float> Time { get; private set; }

    Timing _timing = null;
    Timer _timer = null;
    Timer _delay = null;
    int _counter = 0;

    public Interpolation(Timing timing) {
      Percent = new WObservable<int>(0);
      Time = new WObservable<float>(0f);

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

      // update observables
      if (null != _timer) {
        float t = CEasings.GetEasing(_timer.GetPercentage(), _timing.easing);
        // reverse t if it's yoyo and counter is odd
        // e.g when counter is 0 it goes forward, 1 goes backwards, etc.
        if (_timing.yoyo && _counter % 2 != 0) t = 1f - t;
        Time.Next(t);

        Percent.Next((int)(Time.Value * 100f));
      } else {
        Time.Next(1f);
        Percent.Next(100);
      }

      return _timer != null;
    }
  }

  public abstract class InterpolationBase<T> : Interpolation {
    /// <summary>
    /// The current interpolation value to be observed
    /// </summary>    
    public Action<T> OnLerp { get; set; }

    protected T _from;
    protected T _to;

    public InterpolationBase(Timing timing, T f, T t, bool autoPlay = false) : base(timing) {
      _from = f;
      _to = t;

      if (autoPlay) Start();
    }

    public abstract T Lerp();

    public bool UpdateAutoLerp(float dt) {
      if (Update(dt)) {
        T cur = Lerp();
        OnLerp?.Invoke(cur);
        return true;
      }

      return false;
    }
  }

  public class InterpolationFloat : InterpolationBase<float> {

    public InterpolationFloat(Timing timing, float f, float t, bool autoPlay = false) : base(timing, f, t, autoPlay) { }

    public override float Lerp() {
      return Mathf.Lerp(_from, _to, Time.Value);
    }
  }

  public class InterpolationInt : InterpolationBase<int> {
    public InterpolationInt(Timing timing, int f, int t, bool autoPlay = false) : base(timing, f, t, autoPlay) { }

    public override int Lerp() {
      return (int)Mathf.Lerp(_from, _to, Time.Value);
    }
  }

  public class InterpolationVec2 : InterpolationBase<Vector2> {
    public InterpolationVec2(Timing timing, Vector2 f, Vector2 t, bool autoPlay = false) : base(timing, f, t, autoPlay) { }

    public override Vector2 Lerp() {
      return Vector2.Lerp(_from, _to, Time.Value);
    }
  }
}