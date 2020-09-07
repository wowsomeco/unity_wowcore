using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wowsome {
  namespace Audio {
    public class BgmManager : MonoBehaviour, IAudioManager {
      [Serializable]
      public struct BgmData {
        /// <summary>
        /// The name of the sound file
        /// </summary>
        public string Name;
        /// <summary>
        /// How many times the bgm needs to loop
        /// -1 = infinitely, 1 = 1 time, 2 = 2 times, etc.
        /// </summary>
        public int LoopCount;
      }

      [Serializable]
      public class BgmsData {
        /// <summary>
        /// when set to true, the bgm will keep playing regardless of the scene.
        /// </summary>
        public bool IsGlobal;
        /// <summary>
        /// the unity scene where the bgm needs to play at.
        /// </summary>
        public string SceneName;
        public List<BgmData> Bgms;
        public bool FadeOnStart;
        public float FadeDuration;

        public bool Matches(string sceneName) {
          return SceneName == sceneName;
        }
      }

      public CSound m_sourceSound;
      public List<BgmsData> m_bgms = new List<BgmsData>();
      public string m_path;

      Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();
      int m_bgmCount = 0;
      BgmsData m_curPlaying;

      #region IAudioManager
      public void InitAudioManager() {
        // on init, load all the sfx in the path defined
        AudioClip[] audioClipsFromResources = Resources.LoadAll<AudioClip>(m_path);
        for (int i = 0; i < audioClipsFromResources.Length; ++i) {
          m_audioClips.Add(audioClipsFromResources[i].name, audioClipsFromResources[i]);
        }

        Debug.Assert(null != m_sourceSound);
      }

      public void OnChangeScene(Scene scene) {
        BgmsData bgm = m_bgms.Find(x => x.Matches(scene.name) || x.IsGlobal);
        if (null != bgm) {
          // dont replay if it is on the same scene OR the current playing is of global type. 
          if (null != m_curPlaying && (m_curPlaying.IsGlobal || m_curPlaying.Matches(scene.name))) {
            return;
          }
          // reset          
          m_curPlaying = bgm;
          m_bgmCount = 0;
          // play the sound
          Play(m_curPlaying.Bgms[m_bgmCount]);
        }
      }

      public void UpdateAudio(float dt) {
        // check whenever the current source sound has finished playing
        if (!m_sourceSound.gameObject.activeSelf) {
          // set next                  
          ++m_bgmCount;
          // if exceeds, reset the count to 0
          if (m_bgmCount >= m_curPlaying.Bgms.Count) {
            m_bgmCount = 0;
          }
          // re activate the source sound
          m_sourceSound.gameObject.SetActive(true);
          // play the next bgm
          Play(m_curPlaying.Bgms[m_bgmCount]);
        } else {
          // update the sound
          m_sourceSound.UpdateSound(dt);
        }
      }

      public float Volume {
        get { return m_sourceSound.Volume; }
        set { m_sourceSound.Volume = value; }
      }
      #endregion

      public void Play(BgmData bgmData) {
        AudioClip audioClip = null;
        // find the bgm name in the audio clips dictionary        
        if (m_audioClips.TryGetValue(bgmData.Name, out audioClip)) {
          bool shouldPlay = true;
          System.Action playAction = () => m_sourceSound.PlaySound(audioClip, bgmData.LoopCount, m_curPlaying.FadeOnStart, m_curPlaying.FadeDuration);
          // check if the sound source is currently playing,
          // if so, stop it first
          if (m_sourceSound.IsPlaying()) {
            if (m_sourceSound.Volume > 0f) {
              shouldPlay = false;
              // then on done stopping, play the sound
              m_sourceSound.StopFadeSound(1f, playAction);
            }
          }
          // we reach here when the source sound is currently not playing
          if (shouldPlay) playAction();
        }
      }
    }
  }
}
