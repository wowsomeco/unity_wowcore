using System;
using System.Collections.Generic;
using Wowsome.Audio;
using Wowsome.Core;
using Wowsome.Tween;

namespace Wowsome {
  namespace UI {
    public enum ViewState {
      WillAppear,
      DidAppear,
      WillDisappear,
      DidDisappear
    }

    [Serializable]
    public struct ViewNavigatorData {
      public string m_viewId;
      public bool m_flag;
      public SfxData m_sfx;

      public ViewNavigatorData(string id, bool flag, SfxData sfx) {
        m_viewId = id;
        m_flag = flag;
        m_sfx = sfx;
      }
    }

    [Serializable]
    public struct ViewData {
      public string m_viewId;
      public bool m_isDefault;

      public ViewData(string id, bool isDefault) {
        m_viewId = id;
        m_isDefault = isDefault;
      }
    }

    public interface IView {
      ViewData ViewData { get; }
      ICollection<ITween> Tweens { get; }
      bool Visible { get; set; }
    }

    public interface IViewListener {
      void OnChangeVisibility(string viewId, ViewState state);
    }

    public interface IViewManager {
      string OnShowTweenId { get; }
      string OnHideTweenId { get; }
      bool IsTransitioning { get; }
      void AddView(IView view, bool isDefault = false);
      void AddViewListener(IViewListener listener);
      bool SwitchView(string viewId, bool flag);
      void OnSwitchedView(string viewId, ViewState state);
      void UpdateViewManager(float dt);
    }

    public interface IViewComponent {
      void Setup(ISceneStarter sceneStarter, IViewManager viewManager);
    }
  }
}
