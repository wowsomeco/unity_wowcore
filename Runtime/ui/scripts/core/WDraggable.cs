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
    public RectTransform RectTransform => _rt;

    [Tooltip("the drag offset pos")]
    public Vector2 offset;

    protected Camera _camera = null;
    protected RectTransform _rt;
    protected RectTransform _root;
    protected bool _isFocus = false;

    public void InitDraggable() {
      _rt = GetComponent<RectTransform>();
      var root = _rt.root;
      _root = root?.GetComponent<RectTransform>();
      Assert.Null(_root, "WDraggable must have a root");
      _camera = root?.GetComponent<Canvas>()?.worldCamera;
    }

    #region Event Systems

    public virtual void OnBeginDrag(PointerEventData eventData) {
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
      Vector3 worldPos;
      RectTransformUtility.ScreenPointToWorldPointInRectangle(_root, ed.position, _camera, out worldPos);
      _rt.transform.position = worldPos;
      _rt.SetPos(_rt.Pos() + offset);
    }
  }
}

