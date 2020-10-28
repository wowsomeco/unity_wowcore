using System.Collections.Generic;
using UnityEngine;
using Wowsome.Core;
using Wowsome.Tween;

namespace Wowsome {
  namespace UI {
    public class CViewManager : MonoBehaviour, IViewManager {
      [Tooltip("determines how the transition between the screens should be, either in parralel or one at a time")]
      public TweenerType m_transitionType;
      [Tooltip("true = the screens are stackable, false = if it's showing one at a time")]
      public bool m_isStackable;
      public string m_onShowTweenId = "onshowscreen";
      public string m_onHideTweenId = "onhidescreen";

      Dictionary<string, IView> m_views = new Dictionary<string, IView>();
      Stack<IView> m_showings = new Stack<IView>();
      HashSet<IViewListener> m_viewListeners = new HashSet<IViewListener>();
      CTweenChainer m_tweener;

      public void InitViewManager(ISceneStarter sceneStarter) {
        var vcs = GetComponentsInChildren<IViewComponent>(true);
        // instantiate the tweener
        m_tweener = new CTweenChainer(m_transitionType);
        // setup the components
        for (int i = 0; i < vcs.Length; ++i) {
          vcs[i].Setup(sceneStarter, this);
        }
        // start the views afterwards
        StartView();
      }

      public void StartView() {
        foreach (KeyValuePair<string, IView> kvp in m_views) {
          IView view = kvp.Value;
          // show if default, hide immediately otherwise
          if (view.ViewData.m_isDefault) {
            ShowView(view, true);
          } else {
            view.Visible = false;
          }
        }
      }

      #region IViewManager            
      public bool IsTransitioning {
        get { return m_tweener.IsPlaying; }
      }

      public void AddView(IView view, bool isDefault = false) {
        // add to the dictionary
        m_views.Add(view.ViewData.m_viewId, view);
        // setup the tweens
        foreach (ITween tween in view.Tweens) {
          tween.Setup();
        }
      }

      public void AddViewListener(IViewListener listener) {
        m_viewListeners.Add(listener);
      }

      public bool SwitchView(string viewId, bool flag) {
        IView view = null;
        // if the view exists, try showing it
        if (m_views.TryGetValue(viewId, out view)) {
          return TryShow(view, flag);
        }
        return false;
      }

      public void OnSwitchedView(string viewId, ViewState state) {
        foreach (IViewListener listener in m_viewListeners) {
          listener.OnChangeVisibility(viewId, state);
        }
      }

      public void UpdateViewManager(float dt) {
        m_tweener.Update(dt);
      }

      public void ShowScreen(string screenId, bool flag) {
        SwitchView(screenId, flag);
      }

      public void ShowScreen(ViewNavigatorData navData) {
        ShowScreen(navData.m_viewId, navData.m_flag);
      }
      #endregion

      bool TryShow(IView view, bool flag) {
        // dont process if the flag is same as the visibility
        if (view.Visible == flag) {
          return false;
        }
        // or if the tweener is currently playing and it's not stackable
        if (IsTransitioning && !m_isStackable) {
          return false;
        }
        // check whether the view manager is not stackable
        if (!m_isStackable && flag) {
          // hide the current showing if so
          if (m_showings.Count > 0) {
            ShowView(m_showings.Peek(), false);
          }
        }
        // finally, show time
        ShowView(view, flag);
        return true;
      }

      void ShowView(IView view, bool flag) {
        // get the tween
        HashSet<ITween> tweens = new HashSet<ITween>();
        string tweenId = flag ? m_onShowTweenId : m_onHideTweenId;
        foreach (ITween viewTween in view.Tweens) {
          if (viewTween.TweenId.IsEqual(tweenId)) {
            tweens.Add(viewTween);
          }
        }
        // check if the tween exists
        if (tweens.Count > 0) {
          // set visibly on appear
          if (flag) {
            view.Visible = true;
          }
          // broadcast will appear msg
          OnSwitchedView(view.ViewData.m_viewId, flag ? ViewState.WillAppear : ViewState.WillDisappear);
          // add to the chainer and play the hide/show tween
          m_tweener.PlayOnly(tweens, () => {
            OnSwitchedView(view.ViewData.m_viewId, flag ? ViewState.DidAppear : ViewState.DidDisappear);
            // hide on did disappear
            if (!flag) {
              view.Visible = false;
            }
          });
        }
        // otherwise if no tweens, just set the visibility directly
        else {
          view.Visible = flag;
        }
        // add the view to the showing stack if flag is true
        if (flag) {
          m_showings.Push(view);
        } else {
          m_showings.Pop();
        }
      }
    }
  }
}
