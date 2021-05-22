﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wowsome.UI {
  [RequireComponent(typeof(Image))]
  [DisallowMultipleComponent]
  public class WTappable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public bool Disabled { get; set; } = true;
    /// <summary>
    /// Invoked the first time this object receives a touch
    /// </summary>    
    public Action<PointerEventData> OnStartTap { get; set; }
    /// <summary>
    /// Invoked when the touch ends outside of this rect transform
    /// </summary>   
    public Action<PointerEventData> OnEndCancel { get; set; }
    /// <summary>
    /// Invoked when the touch ends inside the rect transform
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
          OnEndTap?.Invoke(eventData);
        } else {
          OnEndCancel?.Invoke(eventData);
        }
      });
    }

    protected void ExecOnEnabled(Action callback) {
      if (!Disabled) callback();
    }
  }
}

