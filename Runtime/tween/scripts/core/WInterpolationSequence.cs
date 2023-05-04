using System;
using System.Collections.Generic;

namespace Wowsome.Tween {
  /// <summary>
  /// Plays interpolation(s) sequentially
  /// </summary>    
  public class InterpolationSequence<TInterpolation, TType> where TInterpolation : InterpolationBase<TType> {
    /// <summary>
    /// Observable the current interpolation value
    /// </summary>      
    public Action<TType> OnLerp { get; set; }
    /// <summary>
    /// Observable on done all sequence
    /// </summary>    
    public Action OnDone { get; set; }

    IList<TInterpolation> _steps = null;
    int _curStepIdx = 0;
    bool _isDone = false;

    public InterpolationSequence(IList<TInterpolation> steps, bool autoStart = true) {
      _steps = steps;

      if (autoStart) Start();
    }

    public InterpolationSequence(params TInterpolation[] steps) {
      _steps = steps;

      Start();
    }

    public void Start() {
      _curStepIdx = -1;
      _isDone = false;

      SetNext();
    }

    void SetNext() {
      ++_curStepIdx;

      if (_curStepIdx >= _steps.Count) {
        _isDone = true;

        OnDone?.Invoke();
      } else {
        var curStep = _steps[_curStepIdx];

        curStep.Start();

        curStep.OnLerp = null;
        curStep.OnLerp += t => {
          OnLerp?.Invoke(t);
        };

        curStep.OnDone = null;
        curStep.OnDone += () => {
          SetNext();
        };
      }
    }

    public bool Update(float dt) {
      if (_isDone) return false;

      _steps[_curStepIdx].Run(dt);

      return true;
    }
  }
}
