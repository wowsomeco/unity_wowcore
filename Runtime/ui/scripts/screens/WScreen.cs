using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Core;
using Wowsome.Tween;

namespace Wowsome.UI {
  public class WScreen : MonoBehaviour {
    public Action OnWillShow { get; set; }
    public Action OnDidShow { get; set; }
    public Action OnWillHide { get; set; }
    public Action OnDidHide { get; set; }

    public bool Visible {
      get { return gameObject.activeSelf; }
      set {
        gameObject.SetActive(value);
      }
    }

    public ICollection<ITween> Tweens { get; private set; }

    public string id;
    public bool isDefault;

    protected WScreenManager _screenManager;

    public void InitScreen(ISceneStarter sceneStarter, WScreenManager controller) {
      // cache the listener
      _screenManager = controller;
      _screenManager.OnChangeVisibility += (VisibilityEv vis) => {
        if (vis.ScreenId.CompareStandard(id)) {
          switch (vis.State) {
            case ViewState.WillAppear:
              OnWillShow?.Invoke();
              break;
            case ViewState.DidAppear:
              OnDidShow?.Invoke();
              break;
            case ViewState.WillDisappear:
              OnWillHide?.Invoke();
              break;
            case ViewState.DidDisappear:
              OnDidHide?.Invoke();
              break;
            default:
              break;
          }
        }
      };
      // get all ITween component(s) in this CScreen
      List<ITween> allTweens = GetComponentsInChildren<ITween>(true).ToList();
      // just grab the ones with onshow / onhide id defined in the view manager
      Tweens = allTweens.FindAll(x =>
        x.TweenId.CompareStandard(_screenManager.onShowTweenId) || x.TweenId.CompareStandard(_screenManager.onHideTweenId)
      );
      // init tweens
      foreach (ITween tween in Tweens) {
        tween.Setup();
      }
    }
  }
}

