using System;
using UnityEngine.SceneManagement;

namespace Wowsome {
  namespace Audio {
    [Serializable]
    public enum AudioFadeType { In, Out, None };

    [Serializable]
    public struct SfxData {
      public string sfxName;
      public int loopCount;
      public bool isFadeOnPlay;
      public float delay;
      public bool shouldUnique;
      public bool shouldStop;

      public SfxData(string sfxName) {
        this.sfxName = sfxName;
        delay = 0f;
        loopCount = 0;
        isFadeOnPlay = false;
        shouldUnique = false;
        shouldStop = false;
      }
    }

    public interface IAudioManager {
      float Volume { get; set; }
      void InitAudioManager();
      void OnChangeScene(Scene scene);
      void UpdateAudio(float dt);
    }
  }
}
