using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Wowsome.TwoDee {
  /// <summary>
  /// The base class for a tappable object.
  /// 
  /// You need to extends this accordingly.
  /// Take a look at [[WTappableUI]] for example of how to use
  /// </summary>
  [DisallowMultipleComponent]
  public abstract class WTappable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public bool Disabled { get; set; } = true;
    /// <summary>
    /// Invoked the first time this object receives a touch
    /// </summary>    
    public Action<PointerEventData> OnStartTap { get; set; }
    /// <summary>
    /// Invoked when the touch ends outside of this rect transform
    /// </summary>   
    public Action<PointerEventData> OnEndOutside { get; set; }
    /// <summary>
    /// Invoked when the touch ends inside of this rect transform
    /// </summary>   
    public Action<PointerEventData> OnEndInside { get; set; }
    /// <summary>
    /// Invoked when the touch ends regardless inside or outside of the rect transform
    /// </summary>
    public Action<PointerEventData> OnEndTap { get; set; }

    protected abstract void OnTap(PointerEventData ed);

    protected Camera _camera;

    /// <summary>
    /// You need to call this method first in order to make this script functional
    /// </summary>
    public virtual void InitTappable(Camera cam) {
      _camera = cam;

      Disabled = false;
    }

    public void AddScaleListener(float startTapScale, float endTapScale = 1f) {
      OnStartTap += _ => transform.Scale(startTapScale);
      OnEndTap += _ => transform.Scale(endTapScale);
    }

    public void OnPointerDown(PointerEventData eventData) {
      ExecOnEnabled(() => {
        OnStartTap?.Invoke(eventData);
      });
    }

    public void OnPointerUp(PointerEventData eventData) {
      ExecOnEnabled(() => {
        OnTap(eventData);

        OnEndTap?.Invoke(eventData);
      });
    }

    protected void ExecOnEnabled(Action callback) {
      if (!Disabled) callback();
    }
  }
}