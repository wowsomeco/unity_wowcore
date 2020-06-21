using UnityEngine;
using System.Collections.Generic;

namespace Wowsome {
  namespace Audio {
    public class SfxManager : MonoBehaviour, IAudioManager {
      public CSound[] m_sources;
      public string m_path;

      Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();
      float m_volume;
      float m_BGMVolume;
      string m_menuBGM = "";
      string m_gameBGM = "";

      #region IAudioManager
      public float Volume {
        get { return m_volume; }
        set {
          m_volume = value;
          foreach (CSound soundFX in m_sources) {
            if ((soundFX.GetAudioName() != m_gameBGM) && (soundFX.GetAudioName() != m_menuBGM)) {
              soundFX.Volume = m_volume;
            }
          }
        }
      }

      public void InitAudioManager() {
        AudioClip[] audioClipsFromResources = Resources.LoadAll<AudioClip>(m_path);
        for (int i = 0; i < audioClipsFromResources.Length; ++i) {
          if (!m_audioClips.ContainsKey(audioClipsFromResources[i].name)) {
            m_audioClips.Add(audioClipsFromResources[i].name, audioClipsFromResources[i]);
          }
        }
        foreach (CSound soundFX in m_sources) {
          soundFX.gameObject.SetActive(false);
        }
      }

      public void OnChangeScene(int idx) { }

      public void UpdateAudio(float dt) {
        for (int i = 0; i < m_sources.Length; ++i) {
          m_sources[i].UpdateSound(dt);
        }
      }
      #endregion

      public float BGMVolume {
        get { return m_BGMVolume; }
        set {
          m_BGMVolume = value;
          foreach (CSound soundFX in m_sources) {
            if ((soundFX.GetAudioName() == m_gameBGM) || (soundFX.GetAudioName() == m_menuBGM)) {
              soundFX.Volume = m_BGMVolume;
            }
          }
        }
      }

      public void SetBGMName(string menuBGM, string gameBGM) {
        m_menuBGM = menuBGM;
        m_gameBGM = gameBGM;
      }


      public void PlaySound(string audioClipName, int loopCount = 1, float delay = 0f, bool isFade = false, System.Action onStopCallback = null) {
        bool isContainAudioClip = m_audioClips.ContainsKey(audioClipName);
        if (isContainAudioClip) {
          CSound soundFX = GetAvailableSound();
          if (null != soundFX) {
            soundFX.PlaySound(m_audioClips[audioClipName], loopCount, isFade, delay, 0.5f, onStopCallback);
          }
        }
      }

      public void PlaySound(SfxData sfxData) {
        //bail if no sfx name defined
        if (string.IsNullOrEmpty(sfxData.m_sfxName)) {
          return;
        }
        //if it should stop, stop and bail
        if (sfxData.m_shouldStop) {
          StopAudioByName(sfxData.m_sfxName);
          return;
        }
        //also bail if there's a sound for the name playing where it should be unique
        if (sfxData.m_shouldUnique && IsSoundPlaying(sfxData.m_sfxName)) {
          return;
        }
        //finally play the sound
        PlaySound(sfxData.m_sfxName
            , sfxData.m_loopCount > -1 ? (sfxData.m_loopCount + 1) : -1
            , sfxData.m_delay
            , sfxData.m_isFadeOnPlay);
      }

      public void PlaySound(SfxData[] sfxData) {
        for (int i = 0; i < sfxData.Length; ++i) {
          PlaySound(sfxData[i]);
        }
      }

      public void StopAllAudio() {
        foreach (var sfx in m_sources) {
          sfx.StopSoundImmediate();
        }
      }

      public void StopAudioByName(string audioClipName, bool isFade = false, float delay = 1f) {
        List<CSound> bSounds = new List<CSound>(); //to prevent from same sound that is still playing in the background
        foreach (var soundFX in m_sources) {
          if (/*soundFX.IsPlaying() &&*/  soundFX.GetAudioName() == audioClipName) {
            bSounds.Add(soundFX);
          }
        }

        foreach (CSound bSound in bSounds) {
          if (isFade) {
            bSound.StopFadeSound(delay, () => {
              bSound.StopSoundImmediate();
            });
          } else {
            bSound.StopSoundImmediate();
          }
        }
      }

      public bool IsSoundPlaying(string audioClipName) {
        foreach (var soundFX in m_sources) {
          if (soundFX.IsPlaying() && soundFX.GetAudioName() == audioClipName) {
            return true;
          }
        }
        return false;
      }

      CSound GetAvailableSound() {
        foreach (var soundFX in m_sources) {
          if (!soundFX.gameObject.activeSelf) {
            soundFX.gameObject.SetActive(true);
            return soundFX;
          }
        }
        return null;
      }
    }
  }
}

