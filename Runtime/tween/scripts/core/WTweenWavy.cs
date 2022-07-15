using System;
using UnityEngine;

namespace Wowsome.Tween {
  public class WTweenWavy {
    public enum Direction { Horizontal = 0, Vertical = 1 }

    public class Options {
      public Direction Dir { get; set; } = Direction.Horizontal;
      public float Magnitude { get; set; } = 0.01f;
      public float Frequency { get; set; } = 5f;
      public float Speed { get; set; }
      public Func<Vector2> Pos { get; set; }
    }

    public Action<Vector2> CurPos { get; set; }

    Options _options;
    int _intDirection;

    public WTweenWavy(Options opt) {
      _options = opt;
      _intDirection = (int)_options.Dir;
    }

    public void Update(float dt) {
      Vector2 pos = _options.Pos();

      pos[1 - _intDirection] += Mathf.Sin(Time.time * _options.Frequency) * _options.Magnitude;
      pos[_intDirection] += _options.Speed * dt;

      CurPos?.Invoke(pos);
    }
  }
}