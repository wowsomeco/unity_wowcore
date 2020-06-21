using System.Collections.Generic;
using Wowsome.Tween;
using Wowsome.Core;

namespace Wowsome {
  namespace UI {
    public class CScreen : ViewComponent, IView {
      public ViewData m_data;

      IViewManager m_viewManager;

      #region ViewComponent
      public override void Setup(ISceneStarter sceneStarter, IViewManager viewManager) {
        //cache the listener
        m_viewManager = viewManager;
        //init the tweens
        Tweens = new HashSet<ITween>(GetComponentsInChildren<ITween>(true));
        //add this view to the view manager
        m_viewManager.AddView(this, m_data.m_isDefault);
      }
      #endregion

      #region IView
      public bool Visible {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
      }

      public ViewData ViewData {
        get { return m_data; }
      }

      public ICollection<ITween> Tweens {
        get; private set;
      }
      #endregion
    }
  }
}

