using System;
using UnityEngine.SceneManagement;

namespace Wowsome {
  namespace Audio {
    [Serializable]
    public enum AudioFadeType {
      IN
    , OUT
    , NONE
    };

    [Serializable]
    public struct SfxData {
      public string m_sfxName;
      public int m_loopCount;
      public bool m_isFadeOnPlay;
      public float m_delay;
      public bool m_shouldUnique;
      public bool m_shouldStop;

      public SfxData(string sfxName) {
        m_sfxName = sfxName;
        m_delay = 0f;
        m_loopCount = 0;
        m_isFadeOnPlay = false;
        m_shouldUnique = false;
        m_shouldStop = false;
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
