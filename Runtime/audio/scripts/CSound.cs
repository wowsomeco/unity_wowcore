using System;
using UnityEngine;
using Wowsome.Chrono;

namespace Wowsome.Audio {
  [RequireComponent(typeof(AudioSource))]
  [DisallowMultipleComponent]
  public class CSound : MonoBehaviour {
    public Action OnDeactivated { get; set; }

    public bool IsPlaying {
      get { return _audioSource.isPlaying; }
    }

    public float Volume {
      get { return _audioSource.volume; }
      set { _audioSource.volume = _maxVolume = value; }
    }

    public string AudioName {
      get {
        if (_audioSource.clip != null) {
          return _audioSource.clip.name;
        }
        return string.Empty;
      }
    }

    AudioSource _audioSource;
    Timer _delayTimer;
    int _loopCount;
    float _fadeSpeed;
    AudioFadeType _fadeState;
    float _maxVolume;
    bool _isFadeInOnPlay;
    Action _onDoneFadeOutCallback;
    Action _onStopCallback;

    public void InitSound() {
      _audioSource = GetComponent<AudioSource>();
      gameObject.SetActive(false);
    }

    public void PlaySound(AudioClip audioClip, int loopCount, bool isFadeInOnPlay, float delay = 0f, float fadeSpeed = 1f, Action onStopCallback = null) {
      _audioSource.clip = audioClip;
      // instantiate the delay counter
      _delayTimer = new Timer(delay);
      _loopCount = loopCount;
      _fadeSpeed = fadeSpeed;
      _isFadeInOnPlay = isFadeInOnPlay;
      // check if fade in on play
      SetFadeInOnPlay();
      // set the callback on stop, if any
      if (null != onStopCallback) {
        _onStopCallback = onStopCallback;
      }
    }

    public void ResetSound() {
      _audioSource.Stop();
      _audioSource.clip = null;
      _isFadeInOnPlay = false;
      _audioSource.volume = _maxVolume;
      _fadeState = AudioFadeType.None;
      _loopCount = 0;
      _onDoneFadeOutCallback = null;
      _onStopCallback = null;
      gameObject.SetActive(false);

      OnDeactivated?.Invoke();
    }

    // TODO: refactor the fading logic, it's quite messy already
    public void UpdateSound(float dt) {
      if (null != _delayTimer) {
        // return if there's delay
        if (_delayTimer.UpdateTimer(dt)) {
          return;
        } else {
          _delayTimer = null;
        }
      }

      if (_fadeState == AudioFadeType.Out) {
        if (_audioSource.volume > 0f) {
          _audioSource.volume -= _fadeSpeed * dt;
        } else {
          _fadeState = AudioFadeType.None;
          if (_onDoneFadeOutCallback != null) {
            _onDoneFadeOutCallback();
            _onDoneFadeOutCallback = null;
          }
        }
      } else if (_fadeState == AudioFadeType.In) {
        if (_audioSource.volume < _maxVolume) {
          _audioSource.volume += _fadeSpeed * dt;
        } else {
          _fadeState = AudioFadeType.None;
        }
      }

      if (!_audioSource.isPlaying) {
        if (_loopCount > 0) {
          // play again when it has stopped
          --_loopCount;
          ReplaySound();
        } else if (_loopCount == 0) {
          // do when loop count has been 0 and audio is not playing anymore, stop, reset and deactivated sound
          if (_onStopCallback != null) {
            _onStopCallback();
            _onStopCallback = null;
          }

          ResetSound();
        } else if (_loopCount < 0) {
          // only replay when it's not fading out, otherwise will be looping forever a.k.a stupid bug
          if (_fadeState != AudioFadeType.Out) {
            // loop forever
            ReplaySound();
          }
        }
      }
    }

    public void StopSoundImmediate() {
      ResetSound();
    }

    public void StopFadeSound(float fadeSpeed, Action onFadeOutCallback = null) {
      _fadeState = AudioFadeType.Out;
      _fadeSpeed = fadeSpeed;
      _onDoneFadeOutCallback = onFadeOutCallback;
    }

    public void FadeSound(AudioFadeType fadeType, float fadeSpeed) {
      _fadeState = fadeType;
      _fadeSpeed = fadeSpeed;
    }

    void ReplaySound() {
      _audioSource.Play();
      SetFadeInOnPlay();
    }

    void SetFadeInOnPlay() {
      if (_isFadeInOnPlay) {
        _audioSource.volume = 0f;
        _fadeState = AudioFadeType.In;
      }
    }
  }
}

