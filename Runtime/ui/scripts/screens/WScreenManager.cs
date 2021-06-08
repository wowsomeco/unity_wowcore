using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Core;
using Wowsome.Tween;

namespace Wowsome.UI {
  public class WScreenManager : MonoBehaviour {
    public Action<VisibilityEv> OnChangeVisibility { get; set; }
    public bool IsTransitioning => _tweener.IsPlaying;

    [Tooltip("determines how the transition between the screens should be, either in parralel or one at a time")]
    public TweenerType transitionType;
    [Tooltip("true = the screens are stackable, false = if it's showing one at a time")]
    public bool isStackable;
    public string onShowTweenId = "onshow";
    public string onHideTweenId = "onhide";

    Dictionary<string, WScreen> _screens = new Dictionary<string, WScreen>();
    Stack<WScreen> _showings = new Stack<WScreen>();
    CTweenChainer _tweener;
    HashSet<IScreenObject> _screenObjects = new HashSet<IScreenObject>();

    public void ShowScreen(string screenId, bool flag) {
      SwitchView(screenId, flag);
    }

    public void ShowScreen(string screenId) {
      SwitchView(screenId, true);
    }

    public void HideScreen(string screenId) {
      SwitchView(screenId, false);
    }

    public void InitScreenManager(ISceneStarter sceneStarter) {
      var screens = GetComponentsInChildren<WScreen>(true);
      // instantiate the tweener
      _tweener = new CTweenChainer(transitionType);
      // setup the components
      for (int i = 0; i < screens.Length; ++i) {
        WScreen screen = screens[i];
        screen.InitScreen(sceneStarter, this);
        // add to the dictionary
        _screens.Add(screen.id, screen);
      }
      // init screen objects
      var screenObjs = GetComponentsInChildren<IScreenObject>(true);
      foreach (IScreenObject so in screenObjs) {
        so.InitScreenObject(this);
        _screenObjects.Add(so);
      }
      // start the views afterwards
      foreach (KeyValuePair<string, WScreen> kvp in _screens) {
        WScreen view = kvp.Value;
        // show if default, hide immediately otherwise
        if (view.isDefault) {
          ShowView(view, true);
        } else {
          view.Visible = false;
        }
      }
    }

    public bool SwitchView(string viewId, bool flag) {
      WScreen view = null;
      // if the view exists, try showing it
      if (_screens.TryGetValue(viewId, out view)) {
        return TryShow(view, flag);
      }
      return false;
    }

    public void UpdateViewManager(float dt) {
      _tweener.Update(dt);

      foreach (IScreenObject so in _screenObjects) {
        so.UpdateScreenObject(dt);
      }
    }

    bool TryShow(WScreen view, bool flag) {
      // dont process if the flag is same as the visibility
      if (view.Visible == flag) {
        return false;
      }
      // or if the tweener is currently playing and it's not stackable
      if (IsTransitioning && !isStackable) {
        return false;
      }
      // check whether the view manager is not stackable
      if (!isStackable && flag) {
        // hide the current showing if so
        if (_showings.Count > 0) {
          ShowView(_showings.Peek(), false);
        }
      }
      // finally, show time
      ShowView(view, flag);
      return true;
    }

    void ShowView(WScreen view, bool flag) {
      // get the tween
      HashSet<ITween> tweens = new HashSet<ITween>();
      string tweenId = flag ? onShowTweenId : onHideTweenId;
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
        OnSwitchedView(view.id, flag ? ViewState.WillAppear : ViewState.WillDisappear);
        // add to the chainer and play the hide/show tween
        _tweener.PlayOnly(tweens, () => {
          OnSwitchedView(view.id, flag ? ViewState.DidAppear : ViewState.DidDisappear);
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
        _showings.Push(view);
      } else {
        _showings.Pop();
      }
    }

    void OnSwitchedView(string viewId, ViewState state) {
      OnChangeVisibility?.Invoke(new VisibilityEv { ScreenId = viewId, State = state });
    }
  }
}