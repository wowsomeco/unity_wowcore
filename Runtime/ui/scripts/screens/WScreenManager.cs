using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Core;

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
    public bool IsTransitioning {
      get {
        foreach (var kv in _screens) {
          if (kv.Value.IsTransitioning) return true;
        }

        return false;
      }
    }

    [Tooltip("true = the screens are stackable, false = if it's showing one at a time")]
    public bool isStackable;
    public string onShowTweenId = "onshow";
    public string onHideTweenId = "onhide";

    Dictionary<string, WScreen> _screens = new Dictionary<string, WScreen>();
    List<WScreen> _showings = new List<WScreen>();
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
          view.IsVisible = false;
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
      foreach (IScreenObject so in _screenObjects) {
        so.UpdateScreenObject(dt);
      }

      foreach (KeyValuePair<string, WScreen> kvp in _screens) {
        kvp.Value.UpdateScreen(dt);
      }
    }

    bool TryShow(WScreen view, bool flag) {
      // dont process if the flag is same as the visibility
      if (view.IsVisible == flag) return false;
      // or if the screen is currently transitioning and it's not stackable
      if (IsTransitioning && !isStackable) return false;
      // check whether the view manager is not stackable
      if (!isStackable && flag) {
        // hide the last showing
        if (_showings.Count > 0) {
          ShowView(_showings.Last(), false);
        }
      }
      // finally, show time
      ShowView(view, flag);

      return true;
    }

    void ShowView(WScreen screen, bool flag) {
      // set visibly on appear
      if (flag) {
        OnShowScreen(screen.id, ShowState.WillShow);
      } else {
        OnHideScreen(screen.id, HideState.WillHide);
      }
      // show the screen
      screen.SetShow(flag, () => {
        // hide on did disappear
        if (!flag) {
          OnHideScreen(screen.id, HideState.DidHide);
        } else {
          OnShowScreen(screen.id, ShowState.DidShow);
        }
      });
      // add the view to the showing list if flag is true
      if (flag)
        _showings.Add(screen);
      else
        _showings.Remove(screen);
    }

    void OnShowScreen(string screenId, ShowState state) {
      OnShow?.Invoke(new ShowEv { ScreenId = screenId, State = state });
    }

    void OnHideScreen(string screenId, HideState state) {
      OnHide?.Invoke(new HideEv { ScreenId = screenId, State = state });
    }
  }
}