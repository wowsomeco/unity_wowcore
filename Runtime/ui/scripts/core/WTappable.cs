using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wowsome.UI {
  [RequireComponent(typeof(Image))]
  [DisallowMultipleComponent]
  public class WTappable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public class Scaler {
      public Scaler(WTappable tappable, RectTransform rt, float scaleTap, float scaleNormal = 1f) {
        tappable.OnStartTap += ev => rt.SetScale(scaleTap);
        tappable.OnEndTap += ev => rt.SetScale(scaleNormal);
      }
    }

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

    protected RectTransform _rt;

    Camera _cam;

    /// <summary>
    /// You need to call this method first in order to make this script functional
    /// </summary>
    public virtual void InitTappable(Camera cam) {
      _rt = GetComponent<RectTransform>();
      _cam = cam;
      Disabled = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
      ExecOnEnabled(() => {
        OnStartTap?.Invoke(eventData);
      });
    }

    public void OnPointerUp(PointerEventData eventData) {
      ExecOnEnabled(() => {
        if (_rt.IsPointInRect(eventData.position, _cam)) {
          OnEndInside?.Invoke(eventData);
        } else {
          OnEndOutside?.Invoke(eventData);
        }

        OnEndTap?.Invoke(eventData);
      });
    }

    protected void ExecOnEnabled(Action callback) {
      if (!Disabled) callback();
    }
  }
}

