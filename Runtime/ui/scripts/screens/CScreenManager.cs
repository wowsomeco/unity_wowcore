using UnityEngine;
using Wowsome.Tween;
using Wowsome.Core;

namespace Wowsome {
  namespace UI {
    public class CScreenManager : MonoBehaviour, ISceneController {
      [Tooltip("determines how the transition between the screens should be, either in parralel or one at a time")]
      public TweenerType m_transitionType;
      [Tooltip("true = the screens are stackable, false = if it's showing one at a time")]
      public bool m_isStackable;
      public string m_onShowTweenId = "onshowscreen";
      public string m_onHideTweenId = "onhidescreen";

      CViewManager m_viewManager;

      public bool IsTransitioning {
        get { return m_viewManager.IsTransitioning; }
      }

      #region ISceneController implementation
      public void InitSceneController(ISceneStarter sceneStarter) {
        //instantiate the view manager
        m_viewManager = new CViewManager(m_transitionType, m_isStackable, m_onShowTweenId, m_onHideTweenId);
        //setup the view components
        m_viewManager.SetupViewComponents(sceneStarter, GetComponentsInChildren<IViewComponent>(true));
      }

      public void UpdateSceneController(float dt) {
        m_viewManager.UpdateViewManager(dt);
      }
      #endregion

      public void ShowScreen(string screenId, bool flag) {
        m_viewManager.SwitchView(screenId, flag);
      }

      public void ShowScreen(ViewNavigatorData navData) {
        ShowScreen(navData.m_viewId, navData.m_flag);
      }

      public void AddScreenListener(IViewListener listener) {
        m_viewManager.AddViewListener(listener);
      }
    }
  }
}