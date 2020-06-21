using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wowsome.Generic;

namespace Wowsome {
  namespace Core {
    /// @class  CavEngine
    ///
    /// @brief
    /// A game engine that consists of all the associated systems in a form of components.
    /// e.g. LocaleSystem, ParticleSystem, TextureSystem, etc.
    public class CavEngine : MonoBehaviour {
      internal static CavEngine Instance { get; private set; }
      public List<GameObject> m_systemPrefabs = new List<GameObject>();

      List<ISystem> m_systems = new List<ISystem>();
      CMessenger m_globalMessenger = new CMessenger();

      void OnSceneLoaded(Scene scene, LoadSceneMode m) {
        for (int i = 0; i < m_systems.Count; ++i) {
          m_systems[i].OnChangeScene(scene.buildIndex);
        }
      }

      void Awake() {
        if (!Instance) {
          Instance = this;

          // INIT SYSTEMS HERE
          InitSystems();
          StartSystem();

          SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
          Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
      }

      void Update() {
        for (int i = 0; i < m_systems.Count; ++i) {
          m_systems[i].UpdateSystem(Time.deltaTime);
        }
      }

      void InitSystem(GameObject go) {
        ISystem system = go.GetComponent<ISystem>();
        Debug.Assert(null != system, "gameobject doesnt have any ISystem, name =" + go.name);
        system.InitSystem();
        m_systems.Add(system);
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
        for (int i = 0; i < m_systems.Count; ++i) {
          m_systems[i].StartSystem(this);
        }
      }

      public T GetSystem<T>() where T : class, ISystem {
        foreach (ISystem system in m_systems) {
          T t = system as T;
          if (null != t) {
            return t;
          }
        }
        return null;
      }

      #region GLOBAL MESSENGER
      public void AddGlobalObserver(IObserver observer) {
        m_globalMessenger.AddObserver(observer);
      }

      public void RemoveGlobalObserver(IObserver observer) {
        m_globalMessenger.RemoveObserver(observer);
      }

      public void BroadcastGlobalEvent<T>(T ev) {
        m_globalMessenger.BroadcastMessage(ev);
      }
      #endregion
    }
  }
}

