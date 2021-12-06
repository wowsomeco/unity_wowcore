using System;
using UnityEngine;
using Wowsome.Chrono;

namespace Wowsome.Audio {
  [RequireComponent(typeof(AudioSource))]
  [DisallowMultipleComponent]
  public class WSound : MonoBehaviour {
    [Serializable]
    public class PlayOptions {
      public int loopCount;
      public float startDelay;
    }

    public Action OnDeactivated { get; set; }

    public bool IsPlaying {
      get { return gameObject.activeSelf; }
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
    Timer _delayTimer = null;
    float _maxVolume;
    int _loopCount = 0;
    Action _onStopCallback;

    public void InitSound() {
      _audioSource = GetComponent<AudioSource>();
      gameObject.SetActive(false);
    }

    public void PlaySound(AudioClip audioClip, PlayOptions options = null, Action onStopCallback = null) {
      gameObject.SetActive(true);
      _audioSource.clip = audioClip;
      // instantiate the delay counter
      if (null != options) {
        _loopCount = options.loopCount;
        _delayTimer = new Timer(options.startDelay);
      } else {
        PlaySound();
      }
      // set the callback on stop, if any
      _onStopCallback = onStopCallback;
    }

    public void ResetSound() {
      _audioSource.volume = 0f;
      _audioSource.clip = null;
      _onStopCallback = null;
      _loopCount = 0;
      _delayTimer = null;

      gameObject.SetActive(false);

      OnDeactivated?.Invoke();
    }

    public void UpdateSound(float dt) {
      if (null != _delayTimer) {
        // return if there's delay
        if (_delayTimer.UpdateTimer(dt)) {
          return;
        } else {
          _delayTimer = null;
        }
      }
      // this gets triggered once, when audio source is done playing
      if (!_audioSource.isPlaying) {
        if (_loopCount > 0) {
          // play again when it has stopped
          --_loopCount;
          PlaySound();
        } else if (_loopCount == 0) {
          // do when loop count has been 0 and audio is not playing anymore, stop, reset and deactivated sound
          _onStopCallback?.Invoke();
          _onStopCallback = null;
          ResetSound();
        } else if (_loopCount < 0) {
          // loop forever
          PlaySound();
        }
      }
    }

    public void StopSound() {
      ResetSound();
      // TODO: handle stop fade sound
    }

    void PlaySound() {
      _audioSource.volume = _maxVolume;
      _audioSource.Play();
    }
  }
}