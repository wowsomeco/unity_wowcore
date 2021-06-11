using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wowsome.Generic;

namespace Wowsome {
  namespace Core {
    /// <summary>
    /// Wowsome Core Engine
    /// </summary>
    /// <description>
    /// A Singleton that doesn't get destroyed whenever scene changes consists of one or many ISystem.
    /// ISystem in this case could be anything that you expect to be available globally and needs to be only instatiated once,
    /// such as AudioManager, LocalizationManager, etc.    
    /// </description>
    public class CavEngine : MonoBehaviour {
      public struct ChangeSceneEv {
        public bool IsInitial { get; private set; }
        public Scene Scene { get; private set; }

        public ChangeSceneEv(bool isInit, Scene scene) {
          IsInitial = isInit;
          Scene = scene;
        }
      }

      internal static CavEngine Instance { get; private set; }

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

      public List<GameObject> m_systemPrefabs = new List<GameObject>();

      List<ISystem> _systems = new List<ISystem>();
      WConditional<int> _screenSizeChecker = null;
      bool _isInitialScene = true;

      void OnSceneLoaded(Scene scene, LoadSceneMode m) {
        OnChangeScene?.Invoke(new ChangeSceneEv(_isInitialScene, scene));
        if (_isInitialScene) _isInitialScene = false;
      }

      void Awake() {
        if (!Instance) {
          Instance = this;
          // INIT SYSTEMS HERE
          InitSystems();
          StartSystem();
          // observer scene changes
          SceneManager.sceneLoaded += OnSceneLoaded;
          // observe screen size change
          _screenSizeChecker = new WConditional<int>(
            new List<WObservable<int>> {
              new WObservable<int>(Screen.width),
              new WObservable<int>(Screen.height),
            }
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
        _screenSizeChecker.Check(new int[] { Screen.width, Screen.height });
#endif
      }

      void InitSystem(GameObject go) {
        ISystem system = go.GetComponent<ISystem>();
        Debug.Assert(null != system, "gameobject doesnt have any ISystem, name =" + go.name);
        system.InitSystem();
        _systems.Add(system);
      }

      void InitSystems() {
        //instantiate the prefabs
        for (int i = 0; i < m_systemPrefabs.Count; ++i) {
          GameObject systemGO = Instantiate(m_systemPrefabs[i]) as GameObject;
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

        if (assertIfNull) Assert.Null<T>(typeof(T));

        return null;
      }
    }
  }
}

