using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wowsome.Anim {
  /// <summary>
  /// Generic animation helper.
  /// Right now it can only handle UI Image.
  /// </summary>
  [RequireComponent(typeof(Image))]
  public class WAnimatable : MonoBehaviour {
    public class Controller {
      public class ActionHandler {
        public delegate void Lerp(Image target, Vector2 cur);
        public delegate Vector2 GetCur(Image target);

        public Lerp OnLerp { get; private set; }
        public GetCur Current { get; private set; }

        public ActionHandler(Lerp l, GetCur cur) {
          OnLerp = l;
          Current = cur;
        }
      }

      Image _target;
      WAnimFrame _frame;
      Dictionary<WFrameType, ActionHandler> _handlers = new Dictionary<WFrameType, ActionHandler>();
      InterpolationVec _interpolation = null;
      Dictionary<int, List<Controller>> _subController = new Dictionary<int, List<Controller>>();
      Dictionary<int, List<Controller>> _playingSub = new Dictionary<int, List<Controller>>();

      public Controller(GameObject go, WAnimFrame frame) {
        _target = go.GetComponent<Image>();

        _handlers[WFrameType.Position] = new ActionHandler(
          (img, cur) => img.SetPos(cur),
          img => img.rectTransform.Pos()
        );

        _handlers[WFrameType.Scale] = new ActionHandler(
          (img, cur) => img.SetScale(cur),
          img => img.rectTransform.Scale()
        );

        _handlers[WFrameType.Rotation] = new ActionHandler(
          (img, cur) => img.SetRotation(cur[0]),
          img => new Vector2(img.rectTransform.Rotation(), 0f)
        );

        _handlers[WFrameType.Pivot] = new ActionHandler(
          (img, cur) => img.rectTransform.SetPivot(cur),
          img => img.rectTransform.pivot
        );

        _frame = frame;
        _frame.progressCallbacks.ForEach(cb => {
          int p = cb.percent;
          if (!_subController.ContainsKey(p)) _subController[p] = new List<Controller>();

          cb.frames.ForEach(fr => {
            _subController[p].Add(new Controller(go, new WAnimFrame(fr)));
          });
        });

        _interpolation = new InterpolationVec(_frame.timing, _handlers[_frame.type].Current(_target), _frame.to.ToVec2());
        _interpolation.Start();
      }

      public bool Animate(float dt) {
        bool updating = _interpolation.Update(dt);
        if (updating) {
          _handlers[_frame.type].OnLerp(_target, _interpolation.Lerp());

          int percent = _interpolation.Percent;
          if (_subController.ContainsKey(percent)) {
            _playingSub[percent] = _subController[percent];
          }
        } else {
          // done
          OnDone();
        }

        foreach (var subConts in _playingSub) {
          foreach (Controller sc in subConts.Value) {
            if (sc.Animate(dt)) {
              updating = true;
            }
          }
        }

        return updating;
      }

      void OnDone() {
        Vector2 doneTarget = _frame.timing.PingPong ? _interpolation.from : _interpolation.to;
        _handlers[_frame.type].OnLerp(_target, doneTarget);
      }
    }

    public string Id;
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

