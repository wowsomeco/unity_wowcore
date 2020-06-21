using System.Collections.Generic;
using UnityEngine;

namespace Wowsome {
  namespace Core {
    public sealed class SceneStarter : MonoBehaviour, ISceneStarter {
      public GameObject[] m_sceneControllerObjs;

      HashSet<ISceneController> m_controllers = new HashSet<ISceneController>();

      #region ISceneStarter
      public CavEngine Engine { get; private set; }

      public T GetController<T>() where T : class, ISceneController {
        foreach (ISceneController controller in m_controllers) {
          T t = controller as T;
          if (null != t) {
            return t;
          }
        }
        return null;
      }
      #endregion

      void Start() {
        Engine = CavEngine.Instance;

        for (int i = 0; i < m_sceneControllerObjs.Length; ++i) {
          ISceneController sceneController = m_sceneControllerObjs[i].GetComponent<ISceneController>();
          if (null != sceneController) {
            //only add if it doesnt exist yet
            if (!m_controllers.Contains(sceneController)) {
              m_controllers.Add(sceneController);
            }
          } else {
            Debug.LogError("scene controller objects " + m_sceneControllerObjs[i] + " is not a scene controller");
          }
        }

        foreach (ISceneController controller in m_controllers) {
          controller.InitSceneController(this);
        }

        foreach (ISceneController controller in m_controllers) {
          controller.StartSceneController(this);
        }
      }

      void Update() {
        foreach (ISceneController controller in m_controllers) {
          controller.UpdateSceneController(Time.deltaTime);
        }
      }
    }
  }
}
