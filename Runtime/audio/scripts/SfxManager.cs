using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wowsome.Audio {
  public class SfxManager : MonoBehaviour, IAudioManager {
    public string path = "audio/sfx";
    public CSound prefabSound;
    public int sfxChannel = 16;

    List<CSound> _sources = new List<CSound>();
    List<CSound> _currentPlaying = new List<CSound>();
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    float _volume;

    #region IAudioManager

    public float Volume {
      get { return _volume; }
      set {
        _volume = value;
        foreach (CSound soundFX in _sources) {
          soundFX.Volume = _volume;
        }
      }
    }

    public void InitAudioManager() {
      AudioClip[] audioClipsFromResources = Resources.LoadAll<AudioClip>(path);
      for (int i = 0; i < audioClipsFromResources.Length; ++i) {
        if (!_audioClips.ContainsKey(audioClipsFromResources[i].name)) {
          _audioClips.Add(audioClipsFromResources[i].name, audioClipsFromResources[i]);
        }
      }

      for (int i = 0; i < sfxChannel; ++i) {
        CSound sound = prefabSound.Clone<CSound>(transform);
        sound.InitSound();
        sound.OnDeactivated += () => _currentPlaying.Remove(sound);

        _sources.Add(sound);
      }

      Volume = 1f;
    }

    public void OnChangeScene(Scene scene) { }

    public void UpdateAudio(float dt) {
      for (int i = 0; i < _currentPlaying.Count; ++i) {
        _currentPlaying[i].UpdateSound(dt);
      }
    }

    #endregion

    /// <summary>
    /// Stops a current playing sounds first if no available sounds to be found
    ///  before playing a new one.
    /// </summary>
    public void PlayRecycleSound(string audioClipName) {
      StopCurrentPlaying(audioClipName);
      PlaySound(audioClipName);
    }

    /// <summary>
    /// Stops the currently playing sound if no sounds are available in the pool
    /// it will try to stop the one that has same audioClipName first.
    /// if it's not found then it will stop the first one from the _currentPlaying list instead 
    /// </summary>
    public void StopCurrentPlaying(string audioClipName) {
      if (GetAvailableSound() == null) {
        CSound curPlaying = GetPlayingSound(audioClipName) ?? _currentPlaying.First();
        curPlaying.StopSoundImmediate();
      }
    }

    public void PlaySound(string audioClipName, int loopCount = 1, float delay = 0f, bool isFade = false, Action onStopCallback = null) {
      bool containsAudioClip = _audioClips.ContainsKey(audioClipName);
      if (containsAudioClip) {
        CSound soundFX = GetAvailableSound();
        if (null != soundFX) {
          soundFX.PlaySound(_audioClips[audioClipName], loopCount, isFade, delay, 0.5f, onStopCallback);
          _currentPlaying.Add(soundFX);
        }
      }
    }

    public void PlaySound(string audioClipName, Action onStopCallback) {
      PlaySound(audioClipName, 1, 0f, false, onStopCallback);
    }

    public void PlaySound(SfxData sfxData) {
      // bail if no sfx name defined
      if (string.IsNullOrEmpty(sfxData.sfxName)) {
        return;
      }
      // if it should stop, stop and bail
      if (sfxData.shouldStop) {
        StopAudioByName(sfxData.sfxName);
        return;
      }
      // also bail if there's a sound for the name playing where it should be unique
      if (sfxData.shouldUnique && IsSoundPlaying(sfxData.sfxName)) {
        return;
      }
      // finally play the sound
      PlaySound(
        sfxData.sfxName
        , sfxData.loopCount > -1 ? (sfxData.loopCount + 1) : -1
        , sfxData.delay
        , sfxData.isFadeOnPlay
      );
    }

    public void PlaySound(SfxData[] sfxData) {
      for (int i = 0; i < sfxData.Length; ++i) {
        PlaySound(sfxData[i]);
      }
    }

    public void StopAllAudio() {
      foreach (var sfx in _sources) {
        sfx.StopSoundImmediate();
      }
    }

    public void StopAudioByName(string audioClipName, bool isFade = false, float delay = 1f) {
      List<CSound> bSounds = _sources.FindAll(x => x.AudioName == audioClipName);

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
      var source = _sources.Find(x => x.IsPlaying && x.AudioName == audioClipName);
      return source != null;
    }

    public CSound GetPlayingSound(string audioClipName) {
      for (int i = 0; i < _currentPlaying.Count; ++i) {
        CSound sound = _currentPlaying[i];
        if (sound.IsPlaying && sound.AudioName == audioClipName) {
          return sound;
        }
      }

      return null;
    }

    CSound GetAvailableSound() {
      foreach (var soundFX in _sources) {
        if (!soundFX.IsPlaying) {
          soundFX.gameObject.SetActive(true);
          return soundFX;
        }
      }
      return null;
    }
  }
}

