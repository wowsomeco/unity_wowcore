using UnityEngine;
using UnityEngine.UI;

namespace Wowsome {
  namespace UI {
    [RequireComponent(typeof(Image))]
    public class UIDraggable : MonoBehaviour {
      CGestureHandler m_gestureHandler;
      RectTransform m_rectTransform;

      void Awake() {
        //init gesture
        m_gestureHandler = new CGestureHandler(gameObject);
        m_gestureHandler.SetDraggable();
        m_gestureHandler.OnStartSwipeListeners += OnStartDrag;
        m_gestureHandler.OnSwipingListeners += OnDrag;
        //cache rt
        m_rectTransform = GetComponent<RectTransform>();
      }

      void OnStartDrag(SwipeEventData swipeEventData) {
        m_rectTransform.SetAsLastSibling();
      }

      void OnDrag(SwipeEventData swipeEventData) {
        m_rectTransform.SetScaledPos(swipeEventData.Delta);
      }
    }
  }
}
