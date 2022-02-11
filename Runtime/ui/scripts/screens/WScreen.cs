using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Core;
using Wowsome.Tween;

namespace Wowsome.UI {
  using HideState = WScreenManager.HideState;
  using ShowState = WScreenManager.ShowState;

  [DisallowMultipleComponent]
  public class WScreen : MonoBehaviour {
    public interface IScreenComponent {
      void InitScreenComponent(WScreen screen);
      void UpdateScreenComponent(float dt);
    }

    public Action OnWillShow { get; set; }
    public Action OnDidShow { get; set; }
    public Action<ShowState> OnShow { get; set; }
    public Action OnWillHide { get; set; }
    public Action OnDidHide { get; set; }
    public Action<HideState> OnHide { get; set; }
    public ISceneStarter SceneStarter { get; private set; }
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

    List<IScreenComponent> _components = new List<IScreenComponent>();

    public virtual void InitScreen(ISceneStarter sceneStarter, WScreenManager controller) {
      SceneStarter = sceneStarter;
      // cache the listener
      _screenManager = controller;
      _screenManager.OnShow += ev => {
        if (ev.ScreenId.CompareStandard(id)) {
          OnShow?.Invoke(ev.State);

          if (ev.State == WScreenManager.ShowState.WillShow)
            OnWillShow?.Invoke();
          else
            OnDidShow?.Invoke();
        }
      };
      _screenManager.OnHide += ev => {
        if (ev.ScreenId.CompareStandard(id)) {
          OnHide?.Invoke(ev.State);

          if (ev.State == WScreenManager.HideState.WillHide)
            OnWillHide?.Invoke();
          else
            OnDidHide?.Invoke();
        }
      };
      // get all IScreenComponent(s)
      _components = GetComponentsInChildren<IScreenComponent>(true).ToList();
      foreach (IScreenComponent c in _components) {
        c.InitScreenComponent(this);
      }
      // get all ITween component(s) in this CScreen
      // TODO: change ITween with WAnimController instead as we'd like to deprecate the tween stuffs later.
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

    public virtual void UpdateScreen(float dt) {
      for (int i = 0; i < _components.Count; ++i) {
        _components[i].UpdateScreenComponent(dt);
      }
    }
  }
}

