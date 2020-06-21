using UnityEngine;
using System.Collections.Generic;

namespace Wowsome {
  namespace Audio {
    public class BgmManager : MonoBehaviour, IAudioManager {
      public CSound m_sourceSound;
      public BgmLevelsData[] m_bgms;
      public string m_path;

      Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();
      int m_bgmIndex = -1;
      int m_bgmCount = 0;
      bool m_isMute;

      #region IAudioManager
      public void InitAudioManager() {
        AudioClip[] audioClipsFromResources = Resources.LoadAll<AudioClip>(m_path);
        for (int i = 0; i < audioClipsFromResources.Length; ++i) {
          m_audioClips.Add(audioClipsFromResources[i].name, audioClipsFromResources[i]);
        }
      }

      public void OnChangeScene(int level) {
        if (level < m_bgms.Length) {
          if (m_bgmIndex != level) {
            m_isMute = false;
            m_bgmIndex = level;
            m_bgmCount = 0;
            Play(m_bgms[m_bgmIndex].m_bgmLevels[m_bgmCount]);
          }
        }
      }

      public void UpdateAudio(float dt) {
        //while the current index is less than the length                       
        if (m_bgmIndex != -1 && m_bgmIndex < m_bgms.Length) {
          //check whenever the current source sound has finished playing
          if (!m_sourceSound.gameObject.activeSelf && !m_isMute) {
            //set next                  
            ++m_bgmCount;
            //if exceeds, reset the count to 0
            if (m_bgmCount >= m_bgms[m_bgmIndex].m_bgmLevels.Length) {
              m_bgmCount = 0;
            }
            //re activate the source sound
            m_sourceSound.gameObject.SetActive(true);
            //play the next bgm
            Play(m_bgms[m_bgmIndex].m_bgmLevels[m_bgmCount]);
          } else {
            //update the sound
            m_sourceSound.UpdateSound(dt);
          }
        }
      }

      public float Volume {
        get { return m_sourceSound.Volume; }
        set { m_sourceSound.Volume = value; }
      }
      #endregion

      public void Play(BgmLevelData bgmData) {
        bool isContainAudioClip = m_audioClips.ContainsKey(bgmData.m_name);
        if (isContainAudioClip) {
          if (null != m_sourceSound) {
            if (m_sourceSound.IsPlaying()) {
              if (m_sourceSound.Volume <= 0f) {
                m_sourceSound.PlaySound(m_audioClips[bgmData.m_name], bgmData.m_loopCount, bgmData.m_isFadeOnStart, bgmData.m_fadeSpeed);
              } else {
                m_sourceSound.StopFadeSound(1f, () => {
                  m_sourceSound.PlaySound(m_audioClips[bgmData.m_name], bgmData.m_loopCount, bgmData.m_isFadeOnStart, bgmData.m_fadeSpeed);
                });
              }
            } else {
              m_sourceSound.PlaySound(m_audioClips[bgmData.m_name], bgmData.m_loopCount, bgmData.m_isFadeOnStart, bgmData.m_fadeSpeed);
            }
          }
        }
      }

      public void Fade(AudioFadeType fadeType, float fadeSpeed = 1f) {
        m_sourceSound.FadeSound(fadeType, fadeSpeed);
      }

      public void Stop() {
        m_isMute = true;
        m_sourceSound.ResetSound();
      }
    }
  }
}

