using System.Collections.Generic;
using UnityEngine;
using Wowsome.Core;
using Wowsome.Tween;

namespace Wowsome {
  namespace UI {
    public class CScreen : MonoBehaviour, IView, IViewComponent {
      public ViewData m_data;

      IViewManager m_viewManager;

      #region IViewComponent
      public void Setup(ISceneStarter sceneStarter, IViewManager viewManager) {
        // cache the listener
        m_viewManager = viewManager;
        // get all ITween component(s) in this CScreen
        List<ITween> allTweens = GetComponentsInChildren<ITween>(true).ToList();
        // just grab the ones with onshow / onhide id defined in the view manager
        Tweens = allTweens.FindAll(x =>
          x.TweenId.CompareStandard(viewManager.OnShowTweenId) || x.TweenId.CompareStandard(viewManager.OnHideTweenId)
        );
        // add this view to the view manager
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

