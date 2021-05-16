using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wowsome.UI {
  [RequireComponent(typeof(Image))]
  [DisallowMultipleComponent]
  public class WTappable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public bool Disabled { get; set; } = false;
    public Action<PointerEventData> OnStartTap { get; set; }
    public Action<PointerEventData> OnEndTap { get; set; }

    public void OnPointerDown(PointerEventData eventData) {
      ExecOnEnabled(() => {
        OnStartTap?.Invoke(eventData);
      });
    }

    public void OnPointerUp(PointerEventData eventData) {
      ExecOnEnabled(() => {
        OnEndTap?.Invoke(eventData);
      });
    }

    protected void ExecOnEnabled(Action callback) {
      if (!Disabled) callback();
    }
  }
}

