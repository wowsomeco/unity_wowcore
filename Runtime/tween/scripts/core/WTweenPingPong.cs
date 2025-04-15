using System;
using UnityEngine;
using Wowsome.Chrono;

namespace Wowsome.Tween {
  public class PingPongOptions {
    public float From { get; set; }
    public float To { get; set; }
    public float Duration { get; set; }
    public float Delay { get; set; } = 0f;
    public int Count { get; set; } = -1;

    public float GetCurrent(float t) => Mathf.Lerp(From, To, t);
  }

  public class WPingPong {
    public Action<float> Current { get; set; }
    public Action OnDone { get; set; }
    public Action OnSwitch { get; set; }

    PingPongOptions _options = null;
    int _counter = 0;
    bool _isBackwards = false;
    ObservableTimer _timer = null;

    public WPingPong(float duration, int count = -1) : this(new PingPongOptions {
      From = 0f,
      To = 1f,
      Duration = duration,
      Count = count
    }) { }

    public WPingPong(float f, float t, float duration, int count = -1) : this(new PingPongOptions {
      From = f,
      To = t,
      Duration = duration,
      Count = count
    }) { }

    public WPingPong(float f, float t, float duration, float delay, int count = -1) : this(new PingPongOptions {
      From = f,
      To = t,
      Duration = duration,
      Delay = delay,
      Count = count
    }) { }

    public WPingPong(PingPongOptions options) {
      _options = options;

      Start();
    }

    public void Update(float dt) {
      _timer?.UpdateTimer(dt);
    }

    void Start() {
      if (_options.Delay > 0f) {
        _timer = new ObservableTimer(_options.Delay);
        _timer.OnDone += () => {
          Play();
        };
      } else {
        Play();
      }
    }

    void Play() {
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
      Play();

      OnSwitch?.Invoke();
    }
  }

  public class WSmoothPingPong {
    public class InitOptions {
      public float Delay { get; set; } = 0f;
      public float Value { get; set; } = 0f;
    }

    public Action<float> Current { get; set; }
    public Action OnDone { get; set; }

    WPingPong _pingPong = null;
    InterpolationFloat _smoother = null;
    PingPongOptions _options = null;
    InitOptions _initOptions = null;
    float _cur;

    public WSmoothPingPong(InitOptions initOptions, PingPongOptions options) {
      _initOptions = initOptions;
      _options = options;

      SmoothStart();
    }

    public WSmoothPingPong(float initValue, PingPongOptions options) :
      this(new InitOptions { Value = initValue }, options) { }

    public void Update(float dt) {
      _pingPong?.Update(dt);
      _smoother?.Run(dt);
    }

    void Smoothen(float f, float t, Action onDone, float delay = 0f) {
      // duration will be 1/2 of the ping pong duration
      _smoother = new InterpolationFloat(
        new Timing(_options.Duration.Half(), delay),
        f, t, true
      );

      _smoother.OnLerp += c => Current?.Invoke(c);

      _smoother.OnDone += () => {
        _smoother = null;

        onDone();
      };
    }

    void SmoothStart() {
      Smoothen(_initOptions.Value, _options.From, () => {
        _pingPong = new WPingPong(_options);

        _pingPong.Current += c => {
          _cur = c;
          Current?.Invoke(c);
        };

        _pingPong.OnDone += () => {
          SmoothEnd();
        };
      }, _initOptions.Delay);
    }

    void SmoothEnd() {
      Smoothen(_cur, _initOptions.Value, () => {
        OnDone?.Invoke();
      });
    }
  }
}

