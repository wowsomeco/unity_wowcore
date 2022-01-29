using System;
using Wowsome.Chrono;

namespace Wowsome.Anim {
  public class WAnimPingPong {
    public Action<float> Current { get; set; }
    public Action OnDone { get; set; }
    public Action OnSwitch { get; set; }

    float _from;
    float _to;
    float _duration;
    int _count;
    int _counter = 0;
    bool _isBackwards = false;
    ObservableTimer _timer = null;

    public WAnimPingPong(float f, float t, float duration, int count = -1) {
      _from = f;
      _to = t;
      _duration = duration;
      _count = count;

      Start();
    }

    public void Update(float dt) {
      _timer?.UpdateTimer(dt);
    }

    void Switch() {
      Current?.Invoke(_isBackwards ? 0f : 1f);
      _isBackwards = !_isBackwards;
      Start();

      OnSwitch?.Invoke();
    }

    void Start() {
      _timer = new ObservableTimer(_duration);
      _timer.Progress += percentage => {
        float t = _isBackwards ? (1f - percentage) : percentage;
        Current?.Invoke(t);
      };
      _timer.OnDone += () => {
        if (_count == -1) {
          Switch();
        } else {
          ++_counter;
          if (_counter >= _count) {
            _timer = null;

            Current?.Invoke(_isBackwards ? 0f : 1f);
            OnDone?.Invoke();
          } else {
            Switch();
          }
        }
      };
    }
  }
}

