using UnityEngine;
using Wowsome.Core;

namespace Wowsome {
  namespace UI {
    public class ScreenNavigator : MonoBehaviour, IViewListener, IViewComponent {
      public ViewNavigatorData m_data;
      public GameObject[] m_listeners;

      IViewManager _viewManager;

      #region IViewComponent
      public void Setup(ISceneStarter sceneStarter, IViewManager viewManager) {
        // cache the view manager
        _viewManager = viewManager;
        // setup tap handler
        new CTapHandler(gameObject, pos => {
          _viewManager.SwitchView(m_data.m_viewId, m_data.m_flag);
        });
        // add this as view listener if there's at least 1 obj in m_listeners
        if (m_listeners.Length > 0) {
          _viewManager.AddViewListener(this);
        }
      }
      #endregion

      #region IViewListener implementation
      public void OnChangeVisibility(string viewId, ViewState state) {
        if (viewId == m_data.m_viewId) {
          for (int i = 0; i < m_listeners.Length; ++i) {
            m_listeners[i].SetActive(state == ViewState.WillAppear || state == ViewState.DidAppear);
          }
        }
      }
      #endregion
    }
  }
}
