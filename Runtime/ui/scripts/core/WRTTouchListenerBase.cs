using System;
using UnityEngine;

namespace Wowsome.UI {
  using ITouchListener = WTouchSurface.ITouchListener;
  using Touch = WTouchSurface.Touch;

  [RequireComponent(typeof(RectTransform))]
  [DisallowMultipleComponent]
  public class WRTTouchListenerBase : MonoBehaviour, ITouchListener {
    /// <summary>
    /// Gets called whenever the screen pos touch surface is inside this rect transform.
    /// </summary>    
    public Action<Touch> OnStartTouch { get; set; }
    /// <summary>
    /// Gets called whenever touch surface ends the touch and the point is inside this rect transform
    /// </summary>    
    public Action<Touch> OnEndTouch { get; set; }

    protected Camera _camera = null;
    protected WTouchSurface _touchSurface;
    protected RectTransform _rt;
    protected RectTransform _parent;
    protected bool _isFocus = false;

    public virtual void InitTouchListener(WTouchSurface surface) {
      _touchSurface = surface;

      _rt = GetComponent<RectTransform>();
      _parent = _rt.parent as RectTransform;

      surface.OnStartTouch += (Touch touch) => {
        ExecOnActive(() => {
          _isFocus = IsInside(touch.ScreenPos);
          if (_isFocus) {
            OnStartTouch?.Invoke(touch);
          }
        });
      };

      _touchSurface.OnEndTouch += (Touch touch) => {
        ExecOnActive(() => {
          if (_isFocus && IsInside(touch.ScreenPos)) {
            OnEndTouch?.Invoke(touch);
          }

          _isFocus = false;
        });
      };
    }

    protected bool IsInside(Vector2 screenPos) {
      return RectTransformUtility.RectangleContainsScreenPoint(_rt, screenPos, _camera);
    }

    /// <summary>
    /// Only execute the callback when currently active in hierarchy
    /// </summary>
    protected void ExecOnActive(Action callback) {
      if (gameObject.activeInHierarchy) callback();
    }
  }
}

