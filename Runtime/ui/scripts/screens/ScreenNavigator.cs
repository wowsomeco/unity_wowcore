using UnityEngine;
using Wowsome.Core;
using Wowsome.Audio;

namespace Wowsome {
  namespace UI {
    public class ScreenNavigator : ViewComponent, IViewListener {
      public ViewNavigatorData m_data;
      public GameObject[] m_listeners;

      IViewManager m_viewManager;
      CGestureHandler m_tapHandler;
      SfxManager m_sfxManager;

      public override void Setup(ISceneStarter sceneStarter, IViewManager viewManager) {
        //cache the sfx manager
        m_sfxManager = sceneStarter.Engine.GetSystem<AudioSystem>().GetManager<SfxManager>();
        Debug.Assert(null != m_sfxManager);
        //cache the view manager
        m_viewManager = viewManager;
        //setup tap handler
        m_tapHandler = new CGestureHandler(gameObject);
        m_tapHandler.OnTapListeners += OnTap;
        //add this as view listener if there's at least 1 obj in m_listeners
        if (m_listeners.Length > 0) {
          m_viewManager.AddViewListener(this);
        }
      }

      void OnTap(Vector2 pos) {
        if (m_viewManager.SwitchView(m_data.m_viewId, m_data.m_flag)) {
          m_sfxManager.PlaySound(m_data.m_sfx);
        }
      }

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
