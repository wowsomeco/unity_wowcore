using UnityEngine;
using Wowsome.Core;

namespace Wowsome.Audio {
  public class WAudioSystem : MonoBehaviour, ISystem {
    IAudioManager[] _audioManagers;

    public T GetManager<T>() where T : class, IAudioManager {
      foreach (IAudioManager manager in _audioManagers) {
        T t = manager as T;
        if (null != t) {
          return t;
        }
      }
      return null;
    }

    #region ISystem implementation
    public void InitSystem() {
      _audioManagers = GetComponentsInChildren<IAudioManager>(true);
      for (int i = 0; i < _audioManagers.Length; ++i) {
        _audioManagers[i].InitAudioManager();
      }
    }

    public void StartSystem(WEngine gameEngine) {
      gameEngine.OnChangeScene += ev => {
        for (int i = 0; i < _audioManagers.Length; ++i) {
          _audioManagers[i].OnChangeScene(ev.Scene);
        }
      };
    }

    public void UpdateSystem(float dt) {
      for (int i = 0; i < _audioManagers.Length; ++i) {
        _audioManagers[i].UpdateAudio(dt);
      }
    }
    #endregion
  }
}
