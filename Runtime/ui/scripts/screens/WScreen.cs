using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Core;
using Wowsome.Tween;

namespace Wowsome.UI {
  [DisallowMultipleComponent]
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
      _screenManager.OnShow += ev => {
        if (ev.ScreenId.CompareStandard(id)) {
          if (ev.State == WScreenManager.ShowState.WillShow)
            OnWillShow?.Invoke();
          else
            OnDidShow?.Invoke();
        }
      };
      _screenManager.OnHide += ev => {
        if (ev.ScreenId.CompareStandard(id)) {
          if (ev.State == WScreenManager.HideState.WillHide)
            OnWillHide?.Invoke();
          else
            OnDidHide?.Invoke();

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

