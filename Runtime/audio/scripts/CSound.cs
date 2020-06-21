using System;
using UnityEngine;
using Wowsome.Chrono;

namespace Wowsome {
  namespace Audio {
    public class CSound : MonoBehaviour {
      public AudioSource m_audioSource;

      Timer m_delayTimer;
      int m_loopCount;
      float m_fadeSpeed;
      AudioFadeType m_fadeState;
      float m_maxVolume;
      bool m_isFadeInOnPlay;
      Action m_onDoneFadeOutCallback;
      Action m_onStopCallback;

      public void PlaySound(AudioClip audioClip, int loopCount, bool isFadeInOnPlay, float delay = 0f, float fadeSpeed = 1f, Action onStopCallback = null) {
        m_audioSource.clip = audioClip;
        //instantiate the delay counter
        m_delayTimer = new Timer(delay);
        m_loopCount = loopCount;
        m_fadeSpeed = fadeSpeed;
        m_isFadeInOnPlay = isFadeInOnPlay;
        //check if fade in on play
        SetFadeInOnPlay();
        //set the callback on stop, if any
        if (null != onStopCallback) {
          m_onStopCallback = onStopCallback;
        }
      }

      public void ResetSound() {
        m_audioSource.Stop();
        m_audioSource.clip = null;
        m_isFadeInOnPlay = false;
        m_audioSource.volume = m_maxVolume;
        m_fadeState = AudioFadeType.NONE;
        m_loopCount = 0;
        m_onDoneFadeOutCallback = null;
        m_onStopCallback = null;
        gameObject.SetActive(false);
      }

      public void UpdateSound(float dt) {
        if (null != m_delayTimer) {
          //return if there's delay
          if (m_delayTimer.UpdateTimer(dt)) {
            return;
          } else {
            m_delayTimer = null;
          }
        }

        if (m_fadeState == AudioFadeType.OUT) {
          if (m_audioSource.volume > 0f) {
            m_audioSource.volume -= m_fadeSpeed * dt;
          } else {
            m_fadeState = AudioFadeType.NONE;
            if (m_onDoneFadeOutCallback != null) {
              m_onDoneFadeOutCallback();
              m_onDoneFadeOutCallback = null;
            }
          }
        } else if (m_fadeState == AudioFadeType.IN) {
          if (m_audioSource.volume < m_maxVolume) {
            m_audioSource.volume += m_fadeSpeed * dt;
          } else {
            m_fadeState = AudioFadeType.NONE;
          }
        }

        if (!m_audioSource.isPlaying) {
          if (m_loopCount > 0) {
            //play again when it has stopped
            --m_loopCount;
            ReplaySound();
          } else if (m_loopCount == 0) {
            //do when loop count has been 0 and audio is not playing anymore, stop, reset and deactivated sound
            if (m_onStopCallback != null) {
              m_onStopCallback();
              m_onStopCallback = null;
            }

            ResetSound();
          } else if (m_loopCount < 0) {
            //only replay when it's not fading out, otherwise will be looping forever a.k.a stupid bug
            if (m_fadeState != AudioFadeType.OUT) {
              //loop forever
              ReplaySound();
            }
          }
        }
      }

      public float Volume {
        get { return m_audioSource.volume; }
        set { m_audioSource.volume = m_maxVolume = value; }
      }

      public void StopSoundImmediate() {
        ResetSound();
      }

      public void StopFadeSound(float fadeSpeed, System.Action onFadeOutCallback = null) {
        m_fadeState = AudioFadeType.OUT;
        m_fadeSpeed = fadeSpeed;
        m_onDoneFadeOutCallback = onFadeOutCallback;
      }

      public void FadeSound(AudioFadeType fadeType, float fadeSpeed) {
        m_fadeState = fadeType;
        m_fadeSpeed = fadeSpeed;
      }

      public bool IsPlaying() {
        return m_audioSource.isPlaying;
      }

      public string GetAudioName() {
        if (m_audioSource.clip != null) {
          return m_audioSource.clip.name;
        }
        return string.Empty;
      }

      void ReplaySound() {
        m_audioSource.Play();
        SetFadeInOnPlay();
      }

      void SetFadeInOnPlay() {
        if (m_isFadeInOnPlay) {
          m_audioSource.volume = 0f;
          m_fadeState = AudioFadeType.IN;
        }
      }
    }
  }
}

