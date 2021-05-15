using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wowsome.UI {
  [RequireComponent(typeof(Image))]
  [DisallowMultipleComponent]
  public class WDraggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public bool Disabled { get; set; } = false;
    public Action<PointerEventData> OnFocusDrag { get; set; }
    public Action<PointerEventData> OnDragging { get; set; }
    public Action<PointerEventData> OnDragEnd { get; set; }

    [Tooltip("the drag offset pos")]
    public Vector2 offset;

    protected Camera _camera = null;
    protected RectTransform _rt;
    protected RectTransform _parent;
    protected bool _isFocus = false;

    public void InitDraggable() {
      _rt = GetComponent<RectTransform>();
      var root = _rt.root;
      _parent = root?.GetComponent<RectTransform>();
      Assert.Null(_parent, "WDraggable must have a parent");
      _camera = root?.GetComponent<Canvas>()?.worldCamera;
    }

    #region Event Systems

    public void OnBeginDrag(PointerEventData eventData) {
      ExecOnEnabled(() => {
        SetFocus(eventData);
        SetDragPos(eventData);
      });
    }

    public void OnDrag(PointerEventData eventData) {
      ExecOnEnabled(() => {
        SetDragPos(eventData);
        OnDragging?.Invoke(eventData);
      });
    }

    public void OnEndDrag(PointerEventData eventData) {
      ExecOnEnabled(() => {
        SetDragPos(eventData);
        SetUnfocus(eventData);
      });
    }

    public void OnPointerDown(PointerEventData eventData) {
      ExecOnEnabled(() => {
        SetFocus(eventData);
        SetDragPos(eventData);
      });
    }

    public void OnPointerUp(PointerEventData eventData) {
      ExecOnEnabled(() => {
        SetDragPos(eventData);
        SetUnfocus(eventData);
      });
    }

    #endregion

    protected void ExecOnEnabled(Action callback) {
      if (!Disabled) callback();
    }

    void SetFocus(PointerEventData eventData) {
      if (!_isFocus) {
        _isFocus = true;
        OnFocusDrag?.Invoke(eventData);
      }
    }

    void SetUnfocus(PointerEventData eventData) {
      if (_isFocus) {
        _isFocus = false;
        OnDragEnd?.Invoke(eventData);
      }
    }

    void SetDragPos(PointerEventData ed) {
      Vector2 pos = ed.position.ScreenToLocalPos(_parent, _camera);
      _rt.SetPos(pos + offset);
    }
  }
}

