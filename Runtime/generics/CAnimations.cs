using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wowsome.Chrono;

namespace Wowsome.Generic {
  [Serializable]
  public class AnimData {
    public bool LoopForever => loop < 0;

    public string id;
    public List<Sprite> sprites = new List<Sprite>();
    public float duration;
    [Tooltip("The number of time the anim needs to loop e.g. 0 will play 1 time, -1 = infinitely")]
    public int loop;
    [Tooltip("true = play backwards once reached the last idx, false = start over from the first idx again")]
    public bool pingPong;
    public float maxSize;
  }

  /// <summary>
  /// Acts as a container for multiple animations in a Gameobject.
  /// </summary>
  public class CAnimations {
    Dictionary<string, AnimData> _anims = new Dictionary<string, AnimData>();
    Image _image;
    AnimData _curAnim = null;
    int _curIdx = 0;
    int _counter = 0;
    Timer _timer;
    Action _onDone;
    bool _isBackwards = false;

    public CAnimations(IList<AnimData> anims, Image image) {
      for (int i = 0; i < anims.Count; ++i) {
        _anims.Add(anims[i].id, anims[i]);
      }
      //cache the img
      _image = image;
    }

    public void Play(string id, Action onDone = null) {
      if (_anims.TryGetValue(id, out _curAnim)) {
        _timer = new Timer(_curAnim.duration);
        _counter = _curIdx = 0;
        _onDone = onDone;
      } else {
        Debug.LogError("Can't find anim with id=" + id);
      }
    }

    public void Update(float dt) {
      if (null != _curAnim) {
        // update timer here
        if (!_timer.UpdateTimer(dt)) {
          _timer.Reset();

          _curIdx = _curIdx + (_isBackwards ? -1 : 1);
          int spriteCount = _curAnim.sprites.Count;

          // if it should loop...
          if (_curAnim.LoopForever) {
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
            }
          }
          // otherwise, check whether it has reached max
          // if so, increment the counter 
          else if (_curIdx >= spriteCount) {
            ++_counter;
          }
          // make sure the index wont be out of bound
          _curIdx = _curIdx.Clamp(0, spriteCount - 1);
        }
        // check loop count here
        if (_curAnim.loop >= 0 && _counter > _curAnim.loop) {
          _onDone?.Invoke();
          _curAnim = null;
        } else {
          _image.sprite = _curAnim.sprites[_curIdx];
          float maxSize = _curAnim.maxSize;
          if (maxSize > 0f) {
            _image.SetMaxSize(maxSize);
          }
        }
      }
    }
  }
}