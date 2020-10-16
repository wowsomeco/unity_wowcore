using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Wowsome {
  namespace UI {
    public enum SwipeAxis {
      Horizontal = 0
        , Vertical = 1
    }

    public struct SwipeEventData {
      public Vector2 Delta;
      public Vector2 Pos;

      public int Axis {
        get { return Mathf.Abs(Delta.x) > Mathf.Abs(Delta.y) ? 0 : 1; }
      }

      public int Direction {
        get { return Axis == 0 ? Math.Sign(Delta.x) : Math.Sign(Delta.y) * -1; }
      }

      public SwipeEventData(Vector2 delta, Vector2 pos) {
        Delta = delta;
        Pos = pos;
      }
    }

    public delegate void OnTap(Vector2 pos);
    public delegate void OnStartSwipe(SwipeEventData ev);
    public delegate void OnSwiping(SwipeEventData ev);
    public delegate void OnEndSwipe(SwipeEventData ev);
    public delegate void OnStartPress();
    public delegate void OnPressing(float dt);
    public delegate void OnEndPress(float totalPressTime);

    public class CGestureHandler {
      EventTrigger m_eventTrigger;
      bool m_isSwiping;
      bool m_onDown;
      float m_pressTime;

      public OnTap OnTapListeners { get; set; }

      public OnStartSwipe OnStartSwipeListeners { get; set; }

      public OnSwiping OnSwipingListeners { get; set; }

      public OnEndSwipe OnEndSwipeListeners { get; set; }

      public OnStartPress OnStartPressListeners { get; set; }

      public OnPressing OnPressingListeners { get; set; }

      public OnEndPress OnEndPressListeners { get; set; }

      public CGestureHandler(GameObject gameObject) {
        m_eventTrigger = gameObject.GetComponent<EventTrigger>();
        if (null == m_eventTrigger) {
          m_eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
      }

      public void UpdateHandler(float dt) {
        if (m_onDown) {
          m_pressTime += dt;
          if (null != OnPressingListeners) {
            OnPressingListeners.Invoke(dt);
          }
        }
      }

      public CGestureHandler SetTappable() {
        //add tap listener
        m_eventTrigger.AddEventTriggerListener(EventTriggerType.PointerClick, OnTap);
        return this;
      }

      public CGestureHandler SetDraggable() {
        //add begin drag listener
        m_eventTrigger.AddEventTriggerListener(EventTriggerType.BeginDrag, OnBeginDrag);
        //add drag listener
        m_eventTrigger.AddEventTriggerListener(EventTriggerType.Drag, OnDragging);
        //add end drag listener
        m_eventTrigger.AddEventTriggerListener(EventTriggerType.EndDrag, OnEndDrag);
        return this;
      }

      public CGestureHandler SetPressable() {
        //add on pointer up
        m_eventTrigger.AddEventTriggerListener(EventTriggerType.PointerUp, OnPointerUp);
        //add on pointer down
        m_eventTrigger.AddEventTriggerListener(EventTriggerType.PointerDown, OnPointerDown);
        return this;
      }

      #region Tap
      void OnTap(BaseEventData data) {
        //not able to tap when dragging
        if (!m_isSwiping && null != OnTapListeners) {
          PointerEventData pointerEventData = data as PointerEventData;
          OnTapListeners.Invoke(pointerEventData.position);
        }
      }
      #endregion

      #region Drag
      void OnBeginDrag(BaseEventData eventData) {
        m_isSwiping = true;
        if (null != OnStartSwipeListeners) {
          PointerEventData pointerEventData = eventData as PointerEventData;
          OnStartSwipeListeners.Invoke(new SwipeEventData(pointerEventData.delta, pointerEventData.position));
        }
      }

      void OnDragging(BaseEventData eventData) {
        if (null != OnSwipingListeners) {
          PointerEventData pointerEventData = eventData as PointerEventData;
          OnSwipingListeners.Invoke(new SwipeEventData(pointerEventData.delta, pointerEventData.position));
        }
      }

      void OnEndDrag(BaseEventData eventData) {
        if (null != OnEndSwipeListeners) {
          PointerEventData pointerEventData = eventData as PointerEventData;
          OnEndSwipeListeners.Invoke(new SwipeEventData(pointerEventData.delta, pointerEventData.position));
        }
        //reset
        m_isSwiping = false;
      }
      #endregion

      #region Press
      void OnPointerUp(BaseEventData eventData) {
        m_onDown = false;
        if (null != OnEndPressListeners) {
          OnEndPressListeners.Invoke(m_pressTime);
          m_pressTime = 0f;
        }
      }

      void OnPointerDown(BaseEventData eventData) {
        m_onDown = true;
        if (null != OnStartPressListeners) {
          OnStartPressListeners.Invoke();
        }
      }
      #endregion
    }

    public class CTapHandler {
      CGestureHandler m_gesture;

      public OnTap OnTapListeners {
        set { m_gesture.OnTapListeners += value; }
        get { return m_gesture.OnTapListeners; }
      }

      public CTapHandler(GameObject go, OnTap listener) {
        m_gesture = new CGestureHandler(go);
        m_gesture.SetTappable();
        OnTapListeners = listener;
      }
    }
  }
}
