using System;
using UnityEngine;
using Wowsome.Generic;

namespace Wowsome.Tween {
  public class WTweenProjectile {
    public class LaunchEv {
      public Vector2 Pos { get; set; }
      public float LerpProgress { get; set; }
    }

    public class Options {
      public Vector2 From { get; set; }
      public Vector2 To { get; set; }
      public float Duration { get; set; }
      public float StartDelay { get; set; } = 0f;
      public float ArcHeight { get; set; }
      public Action<LaunchEv> OnLaunching { get; set; }
      public Action OnDone { get; set; }
      public Action OnStart { get; set; }
    }

    public WObservable<bool> IsPlaying { get; private set; } = new WObservable<bool>(false);

    Options _options;
    bool _isLaunching = false;
    InterpolationFloat _lerper = null;
    Vector2 _curPos;

    public WTweenProjectile(Options opt) {
      _options = opt;
    }

    public WTweenProjectile Launch() {
      if (_isLaunching) return this;

      _isLaunching = true;

      _curPos = _options.From;

      _lerper = new InterpolationFloat(0f, 1f, _options.Duration, _options.StartDelay);

      _lerper.OnLerp += t => {
        float parabola = 1.0f - 4.0f * (t - 0.5f) * (t - 0.5f);
        Vector2 nextPos = Vector2.Lerp(_options.From, _options.To, t);
        nextPos.y += parabola * _options.ArcHeight;

        _options.OnLaunching?.Invoke(new LaunchEv {
          Pos = nextPos,
          LerpProgress = t
        });

        _curPos = nextPos;
      };

      _lerper.OnDone += () => {
        _isLaunching = false;
        _lerper = null;

        _options.OnDone?.Invoke();
      };

      _lerper.OnStart += () => {
        _options.OnStart?.Invoke();
      };

      return this;
    }

    public void Update(float dt) {
      _lerper?.Run(dt);
    }
  }
}
