using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Core;

namespace Wowsome.UI {
  [DisallowMultipleComponent]
  public class WScreen : MonoBehaviour {
    public interface IScreenComponent {
      void InitScreenComponent(WScreen screen);
      void UpdateScreenComponent(float dt);
    }

    public bool IsTransitioning { get; protected set; }
    public ISceneStarter SceneStarter { get; private set; }
    public bool IsVisible {
      get => gameObject.activeSelf;
      set {
        gameObject.SetActive(value);
      }
    }

    public string id;
    public bool isDefault;

    protected WScreenManager _screenManager;

    List<IScreenComponent> _components = new List<IScreenComponent>();

    /// <summary>
    /// Gets called by the controller whenever it's about to show / hide.
    /// 
    /// Subclass this accordingly to create your own transition animations
    /// </summary>
    public virtual void SetShow(bool flag, Action onDone) {
      gameObject.SetActive(flag);

      onDone();
    }

    public virtual void InitScreen(ISceneStarter sceneStarter, WScreenManager controller) {
      SceneStarter = sceneStarter;
      // cache the listener
      _screenManager = controller;
      // get all IScreenComponent(s)
      _components = GetComponentsInChildren<IScreenComponent>(true).ToList();
      foreach (IScreenComponent c in _components) {
        c.InitScreenComponent(this);
      }
    }

    public virtual void UpdateScreen(float dt) {
      for (int i = 0; i < _components.Count; ++i) {
        _components[i].UpdateScreenComponent(dt);
      }
    }
  }
}

