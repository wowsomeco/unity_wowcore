using UnityEngine;

namespace Wowsome {
  namespace UI {
    public class UIScrollable : MonoBehaviour {
      public RectTransform m_reference;

      RectTransform m_rectTransform;
      RectTransform m_rootTransform;
      CGestureHandler m_swipeHandler;
      float m_rootWidth;
      float m_left;
      float m_right;
      Waypoint m_inertiaPoint;
      Vector2 m_delta;

      const float m_inertiaRate = 0.5f;

      #region ISwipeListener
      public void OnStartSwipe(SwipeEventData ev) {
        m_inertiaPoint = null;
      }

      public void OnSwiping(SwipeEventData ev) {
        //right now only works for horizontal scroll
        int axis = 0;
        m_delta = m_rectTransform.GetScaledPos(ev.Delta);
        Vector2 pos = m_rectTransform.Pos();
        pos[axis] += m_delta[axis];
        pos[axis] = pos[axis].Clamp(-m_right, m_left);
        m_rectTransform.SetPos(pos);
      }

      public void OnEndSwipe(SwipeEventData ev) {
        Vector2 delta = m_delta;
        if (!Mathf.Approximately(delta.x, 0f)) {
          //m_inertiaPoint = new Waypoint(m_rectTransform.Pos(), m_rectTransform.Pos() + new Vector2(delta.x * 2f, 0f), Mathf.Abs(delta.x) * m_inertiaRate);
        }
      }
      #endregion

      void Start() {
        //cache the rect transform
        m_rectTransform = GetComponent<RectTransform>();
        m_rootTransform = m_rectTransform.root.GetComponent<RectTransform>();
        //init the swipeable
        m_swipeHandler = new CGestureHandler(gameObject);
        m_swipeHandler.SetDraggable();
        m_swipeHandler.OnStartSwipeListeners += OnStartSwipe;
        m_swipeHandler.OnSwipingListeners += OnSwiping;
        m_swipeHandler.OnEndSwipeListeners += OnEndSwipe;
      }

      void Update() {
        UpdateLimit();

        if (null != m_inertiaPoint) {
          float dt = Time.deltaTime;
          if (m_inertiaPoint.Moving(dt)) {
            m_rectTransform.SetPos(m_inertiaPoint.Cur);
          } else {
            m_rectTransform.SetPos(m_inertiaPoint.Max);
            m_inertiaPoint = null;
          }
          ClampPos();
        }
      }

      void ClampPos() {
        Vector2 pos = m_rectTransform.Pos();
        pos.x = pos.x.Clamp(-m_right, m_left);
        m_rectTransform.SetPos(pos);
      }

      void UpdateLimit() {
        if (!Mathf.Approximately(m_rootWidth, m_rootTransform.Width())) {
          m_rootWidth = m_rootTransform.Width();
          //get the scrollable limit 
          float delta = Mathf.Abs(m_reference.Width() - m_rootWidth) / 2f;
          m_left = delta - m_reference.X();
          m_right = delta + m_reference.X();
          m_rectTransform.SetX(m_left);
        }
      }
    }
  }
}