using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Wowsome.TwoDee {
  /// <summary>
  /// Acts as a drag listener for the gameobject where this component gets attached to.
  /// 
  /// You need to call InitDraggable(Camera c) from the other component to make it work.
  /// </summary>
  [DisallowMultipleComponent]
  public class WDraggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {
    /// <summary>
    /// when true, will not trigger any of the dragging events
    /// </summary>    
    public bool Disabled { get; set; } = false;
    /// <summary>
    /// Gets called when the first time the object begins dragging
    /// </summary>    
    public Action<PointerEventData> OnDragStart { get; set; }
    /// <summary>
    /// The world pos of the current drag, gets called when dragging
    /// </summary>    
    public Action<Vector2> OnDragging { get; set; }
    /// <summary>
    /// Gets called when the first time the object ends dragging
    /// </summary>    
    public Action<PointerEventData> OnDragEnd { get; set; }

    [Tooltip("the drag offset pos")]
    public Vector2 offset;

    protected Camera _camera = null;
    protected bool _isFocus = false;

    public WDraggable InitDraggable(Camera camera) {
      _camera = camera;

      return this;
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
      });
    }

    public void OnPointerUp(PointerEventData eventData) {
      ExecOnEnabled(() => {
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
        OnDragStart?.Invoke(eventData);
      }
    }

    void SetUnfocus(PointerEventData eventData) {
      if (_isFocus) {
        _isFocus = false;
        OnDragEnd?.Invoke(eventData);
      }
    }

    void SetDragPos(PointerEventData ed) {
      Vector2 worldPos = _camera.ScreenToWorldPoint(ed.position);

      OnDragging?.Invoke(worldPos + offset);
    }
  }
}

