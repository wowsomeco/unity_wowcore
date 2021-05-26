using System.Collections.Generic;
using UnityEngine;
using Wowsome.Chrono;
using Wowsome.Tween;

namespace Wowsome.Anim {
  public interface IAnimatable {
    Vector2 GetCurrentValue(FrameType type);
    void OnLerp(FrameType type, Vector2 value);
  }

  public class AnimFrameController {
    public FrameType Type { get; private set; }
    public Vector2 From { get; private set; }
    public Vector2 To { get; private set; }
    public Timing Timing { get; private set; }

    InterpolationVec2 _lerper;

    public AnimFrameController(FrameType type, Timing timing, Vector2 from, Vector2 to) {
      Type = type;
      Timing = timing;
      From = from;
      To = to;
    }

    public void Start() {
      _lerper = new InterpolationVec2(Timing, From, To);
      _lerper.Start();
    }

    public bool Animate(float dt, out Vector2 cur) {
      bool updating = _lerper.Update(dt);
      cur = _lerper.Lerp();

      return updating;
    }
  }

  public class AnimStepController {
    IAnimatable _target;
    AnimStep _step;
    Queue<AnimFrameController> _lerpers = new Queue<AnimFrameController>();
    int _counter = 0;
    Vector2 _initialValue;
    Timer _timerStartDelay;
    Timer _timerRepeatDelay;
    // TODO: add observable anim progress percentage

    public AnimStepController(IAnimatable target, AnimStep step) {
      _target = target;
      _step = step;
      _initialValue = target.GetCurrentValue(_step.clip.type);
    }

    public void Start(bool isRepeat = false) {
      _lerpers.Clear();

      Vector2 curValue = _initialValue;
      // always force to initial value on start
      _target.OnLerp(_step.clip.type, curValue);
      // iterate over the anim clip(s)
      _step.clip.frames.LoopWithPointer((frame, idx, first, last) => {
        // if tween type is to (fixed), 
        // from = prevFrame.to and to = the current frame.to
        // else if tween type is by (additive),
        // from = curValue, to = curValue + current frame.to
        Vector2 from = Vector2.zero;
        Vector2 to = _step.tweenType == TweenType.To ? frame.to.ToVector2() : curValue.Add(frame.to.ToVector2());
        // if first index, add a frame controller from the initial value to current frame
        // otherwise, add a controller from prev frame to current frame
        if (first) {
          from = curValue;
        } else {
          AnimFrame prevFrame = _step.clip.frames[idx - 1];
          from = _step.tweenType == TweenType.To ? prevFrame.to.ToVector2() : curValue;
        }
        // instantiate a frame controller
        AnimFrameController fc = new AnimFrameController(
          _step.clip.type,
          frame.Timing,
          from,
          to
        );
        // do start and add to queue
        fc.Start();
        _lerpers.Enqueue(fc);
        // add value with the cur frame, useful for tween type by
        curValue = curValue.Add(frame.to.ToVector2());
      });
      // TODO: might want to create another class that handles these delays
      // handle start delay
      if (!isRepeat && _step.startDelay.max > 0f) {
        _timerStartDelay = new Timer(_step.startDelay.GetRand());
      }
      // handle repeat delay
      if (isRepeat && _step.repeatDelay.max > 0f) {
        _timerRepeatDelay = new Timer(_step.repeatDelay.GetRand());
      }
    }

    public bool Animate(float dt) {
      if (_lerpers.Count <= 0) return false;

      AnimFrameController cur = _lerpers.Peek();
      Vector2 curValue;

      if (HandleDelay(_timerStartDelay, dt)) return true;
      if (HandleDelay(_timerRepeatDelay, dt)) return true;

      bool updating = cur.Animate(dt * _step.timeMultiplier, out curValue);
      if (updating) {
        _target.OnLerp(cur.Type, curValue * _step.valueMultiplier);
      }
      // will enter here on done
      else {
        _target.OnLerp(cur.Type, cur.To * _step.valueMultiplier);
        // set next item
        _lerpers.Dequeue();
      }

      bool isDone = _lerpers.Count == 0;
      // when done, check if should repeat
      if (isDone) {
        // negative means loop forever
        bool shouldRepeat = _step.repeat <= -1;

        if (_step.repeat > 0) {
          ++_counter;
          if (_counter <= _step.repeat) {
            shouldRepeat = true;
          }
        }

        if (shouldRepeat) {
          Start(true);
          return true;
        }
      }

      return !isDone;
    }

    bool HandleDelay(Timer timer, float dt) {
      if (null != timer) {
        bool isDelaying = timer.UpdateTimer(dt);
        if (isDelaying) {
          return true;
        } else {
          timer = null;
        }
      }

      return false;
    }
  }
}

