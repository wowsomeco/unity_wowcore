using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Core {
  /// <summary>
  /// The Scene starter that acts as the starting point whenever you start a Unity Scene. 
  /// </summary>
  /// <description>
  /// Attach this script to any empty Gameobject.
  /// It listens to Unity methods e.g. Start() as well as Update() whenever the Scene loaded.
  /// Those methods only get called here and all the methods related to them (InitSceneController and StartSceneController respectively) in ISceneController will get called accordingly.
  /// In Start() method, it caches the WEngine, followed by
  /// retrieving all the ISceneController from m_sceneControllerObjs, then they might want to Init, Start, or Update their associated Components accordingly.        
  /// This ensures that you have the flexibility to call which class that needs to be updated first before the others
  /// so you won't need to touch the Unity Execution Order Script at all that might get very messy easily.       
  /// </description>
  public sealed class SceneStarter : MonoBehaviour, ISceneStarter {
    public GameObject[] m_sceneControllerObjs;

    HashSet<ISceneController> _controllers = new HashSet<ISceneController>();

    #region ISceneStarter
    public WEngine Engine { get; private set; }

    public StartSceneController OnStartSceneController { get; set; }

    public T GetController<T>(bool assertIfNull = true) where T : class, ISceneController {
      foreach (ISceneController controller in _controllers) {
        T t = controller as T;
        if (null != t) {
          return t;
        }
      }

      if (assertIfNull) Assert.Null<T>(null);

      return null;
    }
    #endregion

    void Start() {
      Engine = WEngine.Instance;

      for (int i = 0; i < m_sceneControllerObjs.Length; ++i) {
        ISceneController sceneController = m_sceneControllerObjs[i].GetComponent<ISceneController>();
        if (null != sceneController) {
          //only add if it doesnt exist yet
          if (!_controllers.Contains(sceneController)) {
            _controllers.Add(sceneController);
          }
        } else {
          Debug.LogError("scene controller objects " + m_sceneControllerObjs[i] + " is not a scene controller");
        }
      }

      foreach (ISceneController controller in _controllers) {
        controller.InitSceneController(this);
      }

      // broadcast on start scene controller
      OnStartSceneController?.Invoke(this);
    }
  }
}
