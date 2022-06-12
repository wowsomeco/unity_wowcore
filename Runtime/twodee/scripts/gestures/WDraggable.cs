using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Wowsome.Generic;

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
    /// Indicates when the object gets focused, this one changes to true on pointerDown ev
    /// </summary>
    public WObservable<bool> IsFocus { get; private set; } = new WObservable<bool>(false);
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
    public Action<bool> OnUnfocus { get; set; }

    [Tooltip("the drag offset pos")]
    public Vector2 offset;

    protected Camera _camera = null;
    protected bool _isDragged = false;

    public WDraggable InitDraggable(Camera camera) {
      _camera = camera;

      return this;
    }

    #region Event Systems

    public virtual void OnBeginDrag(PointerEventData eventData) {
      ExecOnEnabledAndFocus(() => {
        OnDragStart?.Invoke(eventData);

        _isDragged = true;

        SetDragPos(eventData);
      });
    }

    public void OnDrag(PointerEventData eventData) {
      ExecOnEnabledAndFocus(() => {
        SetDragPos(eventData);
      });
    }

    public void OnEndDrag(PointerEventData eventData) {
      ExecOnEnabledAndFocus(() => {
        OnDragEnd?.Invoke(eventData);
      });

      SetUnfocus(true);

      _isDragged = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
      ExecOnEnabled(() => {
        SetFocus();
      });
    }

    public void OnPointerUp(PointerEventData eventData) {
      ExecOnEnabled(() => {
        // this forces to unfocus when no drag gets triggered
        // without this IsFocus will remain true when drag end does not get entered
        if (!_isDragged) {
          SetUnfocus(false);
        }
      });
    }

    #endregion

    protected void ExecOnEnabled(Action callback) {
      if (!Disabled) callback();
    }

    protected void ExecOnEnabledAndFocus(Action callback) {
      ExecOnEnabled(() => {
        if (IsFocus.Value) callback();
      });
    }

    void SetFocus() {
      if (!IsFocus.Value) {
        IsFocus.Next(true);
      }
    }

    void SetUnfocus(bool isDragged) {
      if (IsFocus.Value) {
        IsFocus.Next(false);

        OnUnfocus?.Invoke(isDragged);
      }
    }

    void SetDragPos(PointerEventData ed) {
      if (!IsFocus.Value) return;

      Vector2 worldPos = _camera.ScreenToWorldPoint(ed.position);

      OnDragging?.Invoke(worldPos + offset);
    }
  }
}

