using System;
using Wowsome.Generic;

namespace Wowsome {
  namespace Chrono {
    public class TimerData {
      public float Max { get; set; }
      public float Cur { get; set; }
      public float Multiplier { get; set; }

      public TimerData(float max) {
        Max = max;
        Cur = 0f;
        Multiplier = 1f;
      }

      public TimerData(float max, float multiplier) {
        Max = max;
        Multiplier = multiplier;
      }

      public TimerData(TimerData other) {
        Max = other.Max;
        Cur = other.Cur;
        Multiplier = other.Multiplier;
      }
    }

    /// <summary>
    /// Timer
    /// </summary>
    /// <description>
    /// used mainly to perform delays in the game
    /// e.g. Timer timer = new Timer(1f);
    /// call UpdateTimer() in your Update loop until it returns false, means that the timer has
    /// completed the countdown call Reset() to re use the timer once done you might want to set it
    /// to null i.e. timer = null.
    /// </description>
    public class Timer {
      TimerData _data;

      public Timer(TimerData timerData) {
        _data = new TimerData(timerData);
      }

      public Timer(int max) {
        _data = new TimerData((float)max);
      }

      public Timer(float max) {
        _data = new TimerData(max);
      }

      public void Reset() {
        _data.Cur = 0f;
      }

      public void AddTime(float time) {
        //subtract the cur when we add more time to it
        _data.Cur -= time;
        //clamp so it doesnt go below 0 or exceed the max time 
        _data.Cur = _data.Cur.Clamp(0, _data.Max);
      }

      public float GetCurrentTime() {
        return _data.Cur;
      }

      public float GetDeltaTime() {
        return _data.Max - _data.Cur;
      }

      public float GetPercentage() {
        return _data.Cur / _data.Max;
      }

      public bool UpdateTimer(float dT) {
        _data.Cur += dT * _data.Multiplier;
        if (_data.Cur > _data.Max) {
          return false;
        }
        return true;
      }
    }

    public class ObservableTimer {
      public Action<float> Progress { get; set; }
      public Action OnDone { get; set; }
      public Action OnReset { get; set; }

      Timer _timer = null;
      float _duration;

      public ObservableTimer(float duration) {
        Reset(duration);
      }

      public void Reset(float duration) {
        _duration = duration;
        Reset();
      }

      public void Reset() {
        _timer = new Timer(_duration);
        OnReset?.Invoke();
      }

      public bool UpdateTimer(float dt) {
        if (null == _timer) return false;

        bool updating = _timer.UpdateTimer(dt);
        if (updating) {
          Progress?.Invoke(_timer.GetPercentage());
        } else {
          _timer = null;
          OnDone?.Invoke();
        }

        return updating;
      }
    }
  }
}
