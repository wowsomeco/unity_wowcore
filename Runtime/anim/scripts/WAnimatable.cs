using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wowsome.Anim {
  public class WAnimatable : MonoBehaviour {
    public class Controller {
      delegate void Lerp(Image target, Vector2 cur);

      Image _target;
      WAnimFrame _frame;
      Dictionary<WFrameType, Lerp> _handlers = new Dictionary<WFrameType, Lerp>();
      InterpolationVec _interpolation = null;
      Dictionary<int, List<Controller>> _subController = new Dictionary<int, List<Controller>>();
      List<List<Controller>> _playingSub = new List<List<Controller>>();

      public Controller(GameObject go, WAnimFrame frame) {
        _target = go.GetComponent<Image>();

        _handlers[WFrameType.Position] = (img, cur) => img.SetPos(cur);
        _handlers[WFrameType.Scale] = (img, cur) => img.SetScale(cur);
        _handlers[WFrameType.Rotation] = (img, cur) => img.SetRotation(cur[0]);

        _frame = frame;
        _frame.callbacks.ForEach(cb => {
          int p = cb.percent;
          if (!_subController.ContainsKey(p)) _subController[p] = new List<Controller>();

          cb.frames.ForEach(fr => {
            _subController[p].Add(new Controller(go, new WAnimFrame(fr)));
          });
        });

        _interpolation = _frame.GetInterpolationVec();
        _interpolation.Start();
      }

      public bool Animate(float dt) {
        bool updating = _interpolation.Update(dt);
        if (updating) {
          _handlers[_frame.type](_target, _interpolation.Lerp());
          int percent = _interpolation.Percent;
          if (_subController.ContainsKey(percent)) {
            _playingSub.Add(_subController[percent]);
          }
        } else {
          // done
          //   _handlers[_frame.type](_target, _interpolation.to);
        }

        foreach (var subConts in _playingSub) {
          foreach (Controller sc in subConts) {
            if (sc.Animate(dt)) {
              updating = true;
            }
          }
        }

        return updating;
      }
    }

    public List<WAnimFrame> Frames = new List<WAnimFrame>();

    List<Controller> _controllers = new List<Controller>();

    public void Play() {
      _controllers.Clear();
      Frames.ForEach(fr => {
        _controllers.Add(new Controller(gameObject, fr));
      });
    }

    public bool Animate(float dt) {
      bool isAnimating = false;

      foreach (Controller c in _controllers) {
        bool animating = c.Animate(dt);
        if (!isAnimating && animating) isAnimating = true;
      }

      return isAnimating;
    }
  }
}

