using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Chrono;

namespace Wowsome.Generic {
  [Serializable]
  public class AnimData {
    public bool LoopForever => loop < 0;
    public bool ShouldLoop => loop != 0;

    public string id;
    public List<Sprite> sprites = new List<Sprite>();
    public RangeData duration;
    public RangeData delay;
    [Tooltip("The number of time the anim needs to loop e.g. 0 will play 1 time, -1 = infinitely")]
    public int loop;
    [Tooltip("true = play backwards once reached the last idx, false = start over from the first idx again")]
    public bool pingPong;
  }

  /// <summary>
  /// Acts as a container for multiple animations in a Gameobject.
  /// </summary>
  public class CAnimations {
    public Action<Sprite> CurSprite { get; set; }

    Dictionary<string, AnimData> _anims = new Dictionary<string, AnimData>();
    AnimData _curAnim = null;
    int _curIdx = 0;
    int _counter = 0;
    ObservableTimer _timerDelay;
    Timer _timer;
    Action _onDone;
    bool _isBackwards = false;

    public CAnimations(params AnimData[] anims) {
      foreach (AnimData anim in anims) {
        _anims.Add(anim.id, anim);
      }
    }

    public CAnimations(IList<AnimData> anims) {
      for (int i = 0; i < anims.Count; ++i) {
        _anims.Add(anims[i].id, anims[i]);
      }
    }

    public void Play(string id, Action onDone = null) {
      if (_anims.TryGetValue(id, out _curAnim)) {
        TryInitDelay();

        _timer = new Timer(_curAnim.duration.GetRand());
        _counter = _curIdx = 0;
        _onDone = onDone;
      } else {
        Debug.LogError("Can't find anim with id=" + id);
      }
    }

    public void Update(float dt) {
      if (null != _curAnim) {
        int spriteCount = _curAnim.sprites.Count;

        // update delay
        if (null != _timerDelay) {
          _timerDelay.UpdateTimer(dt);
          return;
        }
        // update timer here
        if (!_timer.UpdateTimer(dt)) {
          _timer.Reset();

          _curIdx = _curIdx + (_isBackwards ? -1 : 1);

          // if it should loop...
          if (_curAnim.ShouldLoop) {
            // check whether has reached bottom / upper limit
            bool hasReachedLimit = _curAnim.pingPong ?
            (_isBackwards ? _curIdx < 0 : _curIdx >= spriteCount) : _curIdx >= spriteCount;
            // if has reached limit
            if (hasReachedLimit) {
              // if pingpong, toggle the backwards flag
              // otherwise reset cur idx to 0 again
              if (_curAnim.pingPong) {
                _isBackwards = !_isBackwards;
              } else {
                _curIdx = 0;
              }

              ++_counter;

              TryInitDelay();
            }
          }
          // make sure the index wont be out of bound
          _curIdx = _curIdx.Clamp(0, spriteCount - 1);
        }
        // check loop count here  
        bool isDone = false;
        bool shouldChangeSprite = false;

        if (_curAnim.LoopForever) {
          shouldChangeSprite = true;
        } else {
          if ((_curAnim.ShouldLoop && _counter > _curAnim.loop)) {
            isDone = true;
          } else {
            shouldChangeSprite = true;

            if (!_curAnim.ShouldLoop && _curIdx == spriteCount - 1) {
              isDone = true;
            }
          }
        }

        if (shouldChangeSprite) {
          var curSprite = _curAnim.sprites[_curIdx];
          CurSprite?.Invoke(curSprite);
        }

        if (isDone) {
          _onDone?.Invoke();
          _curAnim = null;
        }
      }
    }

    void TryInitDelay() {
      if (_curAnim.delay.max > 0f) {
        _timerDelay = new ObservableTimer(_curAnim.delay.GetRand());
        _timerDelay.OnDone += () => _timerDelay = null;
      }
    }
  }
}