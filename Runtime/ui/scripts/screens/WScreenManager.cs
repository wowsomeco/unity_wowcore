using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Core;
using Wowsome.Tween;

namespace Wowsome.UI {
  public class WScreenManager : MonoBehaviour {
    public enum ShowState { WillShow, DidShow }

    public enum HideState { WillHide, DidHide }

    public struct ShowEv {
      public bool IsWillShow => State == ShowState.WillShow;
      public bool IsDidShow => State == ShowState.DidShow;
      public string ScreenId { get; set; }
      public ShowState State { get; set; }
    }

    public struct HideEv {
      public bool IsWillHide => State == HideState.WillHide;
      public bool IsDidHide => State == HideState.DidHide;
      public string ScreenId { get; set; }
      public HideState State { get; set; }
    }

    public interface IScreenObject {
      void InitScreenObject(WScreenManager controller);
      void UpdateScreenObject(float dt);
    }

    public Action<ShowEv> OnShow { get; set; }
    public Action<HideEv> OnHide { get; set; }
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
      if (IsTransitioning) return false;

      WScreen view = null;
      // if the view exists, try showing it
      if (_screens.TryGetValue(viewId, out view)) {
        return TryShow(view, flag);
      }

      return false;
    }

    public void UpdateScreenManager(float dt) {
      _tweener.Update(dt);

      foreach (IScreenObject so in _screenObjects) {
        so.UpdateScreenObject(dt);
      }
    }

    bool TryShow(WScreen view, bool flag) {
      // dont process if the flag is same as the visibility
      if (view.Visible == flag) return false;
      // or if the tweener is currently playing and it's not stackable
      if (IsTransitioning && !isStackable) return false;
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

    void ShowView(WScreen screen, bool flag) {
      // get the tween
      HashSet<ITween> tweens = new HashSet<ITween>();
      string tweenId = flag ? onShowTweenId : onHideTweenId;
      foreach (ITween viewTween in screen.Tweens) {
        if (viewTween.TweenId.CompareStandard(tweenId)) tweens.Add(viewTween);
      }
      // check if the tween exists
      if (tweens.Count > 0) {
        // set visibly on appear
        if (flag) {
          screen.Visible = true;
          OnShowScreen(screen.id, ShowState.WillShow);
        } else {
          OnHideScreen(screen.id, HideState.WillHide);
        }
        // add to the chainer and play the hide/show tween
        _tweener.PlayOnly(tweens, () => {
          // hide on did disappear
          if (!flag) {
            screen.Visible = false;
            OnHideScreen(screen.id, HideState.DidHide);
          } else {
            OnShowScreen(screen.id, ShowState.DidShow);
          }
        });
      }
      // otherwise if no tweens, just set the visibility directly
      else {
        screen.Visible = flag;
        // broadcast event
        if (flag)
          OnShowScreen(screen.id);
        else
          OnHideScreen(screen.id);
      }
      // add the view to the showing stack if flag is true
      if (flag)
        _showings.Push(screen);
      else
        _showings.Pop();
    }

    void OnHideScreen(string screenId) {
      OnHideScreen(screenId, HideState.WillHide);
      OnHideScreen(screenId, HideState.DidHide);
    }

    void OnShowScreen(string screenId) {
      OnShowScreen(screenId, ShowState.WillShow);
      OnShowScreen(screenId, ShowState.DidShow);
    }

    void OnShowScreen(string screenId, ShowState state) {
      OnShow?.Invoke(new ShowEv { ScreenId = screenId, State = state });
    }

    void OnHideScreen(string screenId, HideState state) {
      OnHide?.Invoke(new HideEv { ScreenId = screenId, State = state });
    }
  }
}