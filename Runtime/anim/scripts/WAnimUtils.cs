using System;
using UnityEngine;
using Wowsome.Chrono;

namespace Wowsome.Anim {
  public class PingPongOptions {
    public float From { get; set; }
    public float To { get; set; }
    public float Duration { get; set; }
    public int Count { get; set; }

    public float GetCurrent(float t) => Mathf.Lerp(From, To, t);
  }

  public class WAnimPingPong {
    public Action<float> Current { get; set; }
    public Action OnDone { get; set; }
    public Action OnSwitch { get; set; }


    PingPongOptions _options = null;
    int _counter = 0;
    bool _isBackwards = false;
    ObservableTimer _timer = null;

    public WAnimPingPong(float f, float t, float duration, int count = -1) : this(new PingPongOptions {
      From = f,
      To = t,
      Duration = duration,
      Count = count
    }) { }

    public WAnimPingPong(PingPongOptions options) {
      _options = options;

      Start();
    }

    public void Update(float dt) {
      _timer?.UpdateTimer(dt);
    }

    void Start() {
      _timer = new ObservableTimer(_options.Duration);
      _timer.Progress += percentage => {
        float t = _isBackwards ? (1f - percentage) : percentage;
        float cur = _options.GetCurrent(t);

        Current?.Invoke(cur);
      };
      _timer.OnDone += () => {
        if (_options.Count == -1) {
          Switch();
        } else {
          ++_counter;
          if (_counter >= _options.Count) {
            _timer = null;

            float cur = _options.GetCurrent(_isBackwards ? 0f : 1f);
            Current?.Invoke(cur);

            OnDone?.Invoke();
          } else {
            Switch();
          }
        }
      };
    }

    void Switch() {
      float cur = _options.GetCurrent(_isBackwards ? 0f : 1f);
      Current?.Invoke(cur);

      _isBackwards = !_isBackwards;
      Start();

      OnSwitch?.Invoke();
    }
  }

  public class WSmoothPingPong {
    public Action<float> Current { get; set; }
    public Action OnDone { get; set; }

    WAnimPingPong _pingPong = null;
    InterpolationFloat _smoother = null;
    PingPongOptions _options = null;
    float _cur;
    float _initValue;

    public WSmoothPingPong(float initValue, PingPongOptions options) {
      _initValue = initValue;
      _options = options;

      SmoothStart();
    }

    public void Update(float dt) {
      _pingPong?.Update(dt);
      _smoother?.Run(dt);
    }

    void Smoothen(float f, float t, Action onDone) {
      // duration will be 1/2 of the ping pong duration
      _smoother = new InterpolationFloat(
        new Timing(_options.Duration.Half()),
        f, t, true
      );

      _smoother.OnLerp += c => Current?.Invoke(c);

      _smoother.OnDone += () => {
        _smoother = null;

        onDone();
      };
    }

    void SmoothStart() {
      Smoothen(_initValue, _options.From, () => {
        _pingPong = new WAnimPingPong(_options);

        _pingPong.Current += c => {
          _cur = c;
          Current?.Invoke(c);
        };

        _pingPong.OnDone += () => {
          SmoothEnd();
        };
      });
    }

    void SmoothEnd() {
      Smoothen(_cur, _initValue, () => {
        OnDone?.Invoke();
      });
    }
  }
}

