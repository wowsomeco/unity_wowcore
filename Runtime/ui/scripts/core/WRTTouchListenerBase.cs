using System;
using UnityEngine;

namespace Wowsome.UI {
  using Touch = WTouchSurface.Touch;

  [RequireComponent(typeof(RectTransform))]
  [DisallowMultipleComponent]
  public class WRTTouchListenerBase : MonoBehaviour {
    /// <summary>
    /// Gets called whenever the screen pos touch surface is inside this rect transform.
    /// </summary>    
    public Action<Touch> OnStartTouch { get; set; }
    /// <summary>
    /// Gets called whenever touch surface ends the touch regardless it's inside or outside the rect transform
    /// </summary>    
    public Action<Touch> OnEndTouch { get; set; }
    /// <summary>
    /// Gets called whenever touch surface ends the touch and the point is inside this rect transform
    /// </summary>    
    public Action<Touch> OnEndTouchInside { get; set; }
    /// <summary>
    /// Gets called whenever touch surface ends the touch and the point is outside this rect transform
    /// </summary>    
    public Action<Touch> OnEndTouchOutside { get; set; }

    protected Camera _camera = null;
    protected WTouchSurface _touchSurface;
    protected RectTransform _rt;
    protected RectTransform _parent;
    protected bool _isFocus = false;

    public virtual void InitTouchListener(WTouchSurface surface, Camera cam) {
      _touchSurface = surface;
      _camera = cam;

      _rt = GetComponent<RectTransform>();
      _parent = _rt.parent as RectTransform;

      _touchSurface.OnStartTouch += ObserveStartTouch;
      _touchSurface.OnEndTouch += ObserveEndTouch;
    }

    protected bool IsInside(Vector2 screenPos) {
      return RectTransformUtility.RectangleContainsScreenPoint(_rt, screenPos, _camera);
    }

    protected virtual void OnDestroy() {
      _touchSurface.OnStartTouch -= ObserveStartTouch;
      _touchSurface.OnEndTouch -= ObserveEndTouch;
    }

    void ObserveStartTouch(Touch touch) {
      gameObject.ExecOnActive(() => {
        _isFocus = IsInside(touch.ScreenPos);
        if (_isFocus) {
          OnStartTouch?.Invoke(touch);
        }
      });
    }

    void ObserveEndTouch(Touch touch) {
      gameObject.ExecOnActive(() => {
        if (_isFocus) {
          OnEndTouch?.Invoke(touch);
          bool isInside = IsInside(touch.ScreenPos);
          if (isInside) {
            OnEndTouchInside?.Invoke(touch);
          } else {
            OnEndTouchOutside?.Invoke(touch);
          }
        }

        _isFocus = false;
      });
    }
  }
}

