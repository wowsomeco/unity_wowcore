using System;
using System.Collections.Generic;
using Wowsome.Generic;

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
    /// <summary>
    /// Observable of the current progress
    /// </summary>    
    public WObservable<CapacityData> Progress { get; private set; } = new WObservable<CapacityData>(null);

    Queue<TInterpolation> _lerpers = new Queue<TInterpolation>();
    IEnumerable<TInterpolation> _steps = null;

    public InterpolationSequence(IEnumerable<TInterpolation> steps, bool autoStart = true) {
      _steps = steps;

      if (autoStart) Start();
    }

    public InterpolationSequence(params TInterpolation[] steps) {
      _steps = steps;
      Start();
    }

    public void Start() {
      _lerpers.Clear();

      foreach (TInterpolation step in _steps) {
        step.Start();
        _lerpers.Enqueue(step);
      }

      Progress.Next(new CapacityData(_lerpers.Count));
    }

    public bool Update(float dt) {
      if (_lerpers.Count > 0) {
        TInterpolation peek = _lerpers.Peek();
        bool updating = peek.Run(dt);
        if (updating) {
          TType cur = peek.Lerp();
          // trigger the global lerp event
          OnLerp?.Invoke(cur);
        } else {
          _lerpers.Dequeue();
          // update progress 
          Progress.Value.Add();
          Progress.Broadcast();
          // broadcast done if no more item in the queue
          if (_lerpers.Count == 0) {
            OnDone?.Invoke();
          }
        }

        return true;
      }

      return false;
    }
  }
}
