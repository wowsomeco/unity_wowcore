using System;
using UnityEngine.SceneManagement;

namespace Wowsome.Audio {
  [Serializable]
  public enum AudioFadeType { In, Out, None };

  public interface IAudioManager {
    float Volume { get; set; }
    void InitAudioManager();
    void OnChangeScene(Scene scene);
    void UpdateAudio(float dt);
  }
}
