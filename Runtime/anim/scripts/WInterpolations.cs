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

    public Timing(float duration, float delay, Easing eas, int rep) {
      dur = duration;
      dly = delay;
      easing = eas;
      repeat = rep;
    }

    public Timing(float duration, Easing easing) : this(duration, 0f, easing, 0) { }

    public Timing(float duration, float delay, Easing easing) : this(duration, delay, easing, 0) { }

    public Timing(float duration) : this(duration, Easing.Linear) { }

    public Timing(float duration, float delay) : this(duration, delay, Easing.Linear, 0) { }

    public Timing(float duration, int repeat) : this(duration, 0f, Easing.Linear, repeat) { }

    public Timing(Timing other) {
      dur = other.dur;
      dly = other.dly;
      easing = other.easing;
      repeat = other.repeat;
    }
  }

  /// <summary>
  /// The base class of any different Interpolation with various data types below.  
  /// sub-class it accordingly to create your own custom logic.
  /// </summary>
  public abstract class Interpolation {
    /// <summary>
    /// The percentage from 0 to 100
    /// </summary>    
    public WObservable<int> Percent { get; private set; }
    /// <summary>
    /// The current time from 0.0 to 1.0
    /// </summary>    
    public WObservable<float> Time { get; private set; }
    /// <summary>
    /// Gets called whenever the interpolation has complete
    /// </summary>    
    public Action OnDone { get; set; }
    /// <summary>
    /// Gets called as soon as the delay has finished and is about to start lerping 
    /// </summary>    
    public Action OnStart { get; set; }

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

    protected bool Update(float dt) {
      if (null != _delay) {
        if (!_delay.UpdateTimer(dt)) {
          _delay = null;

          OnStart?.Invoke();
        }
        // return false when delaying so that OnLerp in InterpolationBase below wont get called on Run()
        return false;
      }

      if (null != _timer && !_timer.UpdateTimer(dt)) {
        // if it needs to repeat...
        if (_timing.repeat > 0) {
          ++_counter;
          if (_counter <= _timing.repeat) {
            _timer.Reset();
          } else {
            return SetDone();
          }
        } else if (_timing.repeat < 0) {
          // restart if it needs to repeat forever
          Start();
        } else {
          // if it doesnt need to repeat, set done immediately
          return SetDone();
        }
      }

      // update observables
      if (null != _timer) {
        float t = CEasings.GetEasing(_timer.GetPercentage(), _timing.easing);

        Time.Next(t);

        Percent.Next((int)(Time.Value * 100f));
      }

      return _timer != null;
    }

    bool SetDone() {
      _timer = null;

      Time.Next(1f);
      Percent.Next(100);

      OnDone?.Invoke();

      return true;
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

    /// <summary>
    /// Updates and performs OnLerp automagically.    
    /// </summary>
    public bool Run(float dt) {
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

    public InterpolationFloat(float f, float t, float duration, float delay = 0f) : this(new Timing(duration, delay), f, t, true) { }

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
    public InterpolationVec2(Vector2 f, Vector2 t, float duration) : this(new Timing(duration), f, t, true) { }

    public override Vector2 Lerp() {
      return Vector2.Lerp(_from, _to, Time.Value);
    }
  }

  public class InterpolationVec3 : InterpolationBase<Vector3> {
    public InterpolationVec3(Timing timing, Vector3 f, Vector3 t, bool autoPlay = false) : base(timing, f, t, autoPlay) { }

    public override Vector3 Lerp() {
      return Vector3.Lerp(_from, _to, Time.Value);
    }
  }

  public class InterpolationColor : InterpolationBase<Color> {
    public InterpolationColor(Timing timing, Color f, Color t, bool autoPlay = false) : base(timing, f, t, autoPlay) { }

    public override Color Lerp() {
      return Color.Lerp(_from, _to, Time.Value);
    }
  }
}