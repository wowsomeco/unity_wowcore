using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wowsome.Generic;

namespace Wowsome.Core {
  /// <summary>
  /// Wowsome Core Engine
  /// </summary>
  /// <description>
  /// A Singleton that doesn't get destroyed whenever scene changes consists of one or many ISystem.
  /// ISystem in this case could be anything that you expect to be available globally and needs to be only instatiated once,
  /// such as AudioManager, LocalizationManager, etc.    
  /// </description>
  public class WEngine : MonoBehaviour {
    public struct ChangeSceneEv {
      public bool IsInitial => PrevScene.IsEmpty();
      public Scene Scene { get; private set; }
      public string SceneName => Scene.name;
      public string PrevScene { get; private set; }

      public ChangeSceneEv(Scene scene, string prevScene) {
        Scene = scene;
        PrevScene = prevScene;
      }
    }

    internal static WEngine Instance { get; private set; }

    /// <summary>
    /// Callback that gets called whenever the scene changes.
    /// This gets called initially too, Use IsInitial to differentiate it      
    /// </summary>
    public Action<ChangeSceneEv> OnChangeScene { get; set; }
    public Action OnStarted { get; set; }
    /// <summary>
    /// Callback that gets called whenever screen size changes.
    /// Right now it's editor only for performance reason
    /// </summary>            
    public Action OnScreenSizeChanged { get; set; }
    public string PrevSceneName { get; private set; } = string.Empty;
    public string CurSceneName { get; private set; } = string.Empty;

    public List<GameObject> m_systemPrefabs = new List<GameObject>();

    List<ISystem> _systems = new List<ISystem>();
    WConditional<Vector2Int> _screenSizeChecker = null;

    void OnSceneLoaded(Scene scene, LoadSceneMode m) {
      PrevSceneName = string.Copy(CurSceneName);

      OnChangeScene?.Invoke(new ChangeSceneEv(scene, PrevSceneName));

      CurSceneName = scene.name;
    }

    void Awake() {
      if (!Instance) {
        Instance = this;
        // init all
        InitSystems();
        // then call start
        StartSystem();
        // observer scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
        // observe screen size change
        _screenSizeChecker = new WConditional<Vector2Int>(
          new WObservable<Vector2Int>(new Vector2Int(Screen.width, Screen.height))
        );
        _screenSizeChecker.OnChange += () => OnScreenSizeChanged?.Invoke();
      } else {
        Destroy(gameObject);
      }
      DontDestroyOnLoad(gameObject);
    }

    void Update() {
      for (int i = 0; i < _systems.Count; ++i) {
        _systems[i].UpdateSystem(Time.deltaTime);
      }

#if UNITY_EDITOR
      _screenSizeChecker.Check(new Vector2Int(Screen.width, Screen.height));
#endif
    }

    void InitSystem(GameObject go) {
      ISystem system = go.GetComponent<ISystem>();
      Debug.Assert(null != system, "gameobject doesnt have any ISystem, name =" + go.name);
      system.InitSystem();
      _systems.Add(system);
    }

    void InitSystems() {
      // instantiate the prefabs
      for (int i = 0; i < m_systemPrefabs.Count; ++i) {
        GameObject prefab = m_systemPrefabs[i];
        GameObject systemGO = Instantiate(prefab) as GameObject;
        systemGO.name = prefab.name;
        systemGO.transform.SetParent(transform, false);
        InitSystem(systemGO);
      }
    }

    void StartSystem() {
      for (int i = 0; i < _systems.Count; ++i) {
        _systems[i].StartSystem(this);
      }

      OnStarted?.Invoke();
    }

    public T GetSystem<T>(bool assertIfNull = true) where T : class, ISystem {
      foreach (ISystem system in _systems) {
        T t = system as T;
        if (null != t) {
          return t;
        }
      }

      if (assertIfNull) Assert.Null<T>(null);

      return null;
    }
  }
}

