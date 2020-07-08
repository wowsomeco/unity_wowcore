using UnityEngine;
using Wowsome.Core;

namespace Wowsome {
  namespace Audio {
    public class AudioSystem : MonoBehaviour, ISystem {
      IAudioManager[] m_audioManagers;

      public T GetManager<T>() where T : class, IAudioManager {
        foreach (IAudioManager manager in m_audioManagers) {
          T t = manager as T;
          if (null != t) {
            return t;
          }
        }
        return null;
      }

      #region ISystem implementation
      public void InitSystem() {
        m_audioManagers = GetComponentsInChildren<IAudioManager>(true);
        for (int i = 0; i < m_audioManagers.Length; ++i) {
          m_audioManagers[i].InitAudioManager();
        }
      }

      public void StartSystem(CavEngine gameEngine) {
        gameEngine.OnChangeScene += index => {
          for (int i = 0; i < m_audioManagers.Length; ++i) {
            m_audioManagers[i].OnChangeScene(index);
          }
        };
      }

      public void UpdateSystem(float dt) {
        for (int i = 0; i < m_audioManagers.Length; ++i) {
          m_audioManagers[i].UpdateAudio(dt);
        }
      }
      #endregion
    }
  }
}
