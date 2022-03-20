using System;
using UnityEngine;

namespace Wowsome.Tween {
  public class WTweenCircular {
    public class Options {
      public float Radius { get; set; }
      public float Speed { get; set; } = 1f;
      public Vector2 InitPos { get; set; } = Vector2.zero;
    }

    public Action<Vector2> OnLerp { get; set; }

    Options _options;
    float _cur = 0f;

    public WTweenCircular(Options options) {
      _options = options;
    }

    public WTweenCircular(Vector2 initPos, float rad, float speed) : this(new Options {
      InitPos = initPos,
      Radius = rad,
      Speed = speed
    }) { }

    public WTweenCircular(float rad, float speed) : this(Vector2.zero, rad, speed) { }

    public void Update(float dt) {
      _cur += dt * _options.Speed;

      float rad = _options.Radius;
      float x = Mathf.Cos(_cur) * rad;
      float y = Mathf.Sin(_cur) * rad;

      OnLerp?.Invoke(_options.InitPos.Add(new Vector2(x, y)));
    }
  }
}
