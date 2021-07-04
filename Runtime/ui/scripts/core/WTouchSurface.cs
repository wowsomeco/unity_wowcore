using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Wowsome.UI {
  public class WTouchSurface : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public class Touch {
      public PointerEventData PointerData { get; private set; }
      public Vector2 ScreenPos => PointerData.position;

      public Touch(PointerEventData eventData) {
        PointerData = eventData;
      }
    }

    public Action<Touch> OnStartTouch { get; set; }
    public Action<Touch> OnMovingTouch { get; set; }
    public Action<Touch> OnEndTouch { get; set; }

    Dictionary<int, Touch> _touches = new Dictionary<int, Touch>();

    public void OnPointerDown(PointerEventData eventData) {
      Touch touch = new Touch(eventData);
      _touches[eventData.pointerId] = touch;
      OnStartTouch?.Invoke(touch);
    }

    public void OnPointerUp(PointerEventData eventData) {
      Touch touch = new Touch(eventData);
      _touches[eventData.pointerId] = touch;
      OnEndTouch?.Invoke(touch);
    }

    public void OnBeginDrag(PointerEventData eventData) {
      if (_touches.ContainsKey(eventData.pointerId)) {
        var t = _touches[eventData.pointerId];
        t = new Touch(eventData);
        OnMovingTouch?.Invoke(t);
      }
    }

    public void OnDrag(PointerEventData eventData) {
      if (_touches.ContainsKey(eventData.pointerId)) {
        var t = _touches[eventData.pointerId];
        t = new Touch(eventData);
        OnMovingTouch?.Invoke(t);
      }
    }

    public void OnEndDrag(PointerEventData eventData) {
      _touches.Remove(eventData.pointerId);
    }
  }
}

