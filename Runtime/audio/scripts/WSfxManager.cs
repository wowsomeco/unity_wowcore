using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wowsome.Audio {
  public class WSfxManager : MonoBehaviour, IAudioManager {
    public string path = "audio/sfx";
    public WSound prefabSound;
    public int sfxChannel = 16;

    List<WSound> _sources = new List<WSound>();
    List<WSound> _currentPlaying = new List<WSound>();
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    float _volume;

    #region IAudioManager

    public float Volume {
      get { return _volume; }
      set {
        _volume = value;
        foreach (WSound soundFX in _sources) {
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
        WSound sound = prefabSound.Clone<WSound>(transform);
        sound.InitSound();
        sound.OnDeactivated += () => _currentPlaying.RemoveAll(x => x.GetInstanceID() == sound.GetInstanceID());

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
      ReleaseCurrentPlaying(audioClipName);
      PlaySound(audioClipName);
    }

    public void PlayAfterStop(string audioClipName) {
      StopCurrentPlaying(audioClipName);
      PlaySound(audioClipName);
    }

    /// <summary>
    /// Stops the currently playing sound if no sounds are available in the pool
    /// it will try to stop the one that has same audioClipName first.
    /// if it's not found then it will stop the first one from the _currentPlaying list instead 
    /// </summary>
    public void ReleaseCurrentPlaying(string audioClipName) {
      if (GetAvailableSound() == null) {
        StopCurrentPlaying(audioClipName);
      }
    }

    public void StopCurrentPlaying(string audioClipName) {
      WSound curPlaying = GetPlayingSound(audioClipName) ?? _currentPlaying.First();
      curPlaying?.StopSound();
    }

    public void PlaySound(string audioClipName, WSound.PlayOptions options = null, Action onStopCallback = null) {
      bool containsAudioClip = _audioClips.ContainsKey(audioClipName);
      if (containsAudioClip) {
        WSound soundFX = GetAvailableSound();
        if (null != soundFX) {
          soundFX.PlaySound(_audioClips[audioClipName], options, onStopCallback);
          AddToCurPlaying(soundFX);
        }
      }
    }

    public void PlaySound(string audioClipName, bool shouldRecycle, Action onStopCallback) {
      if (shouldRecycle) ReleaseCurrentPlaying(audioClipName);

      PlaySound(audioClipName, null, onStopCallback);
    }

    public void StopAllAudio() {
      foreach (var sfx in _sources) {
        sfx.StopSound();
      }
    }

    public void StopAudioByName(string audioClipName) {
      List<WSound> bSounds = _sources.FindAll(x => x.AudioName == audioClipName);

      foreach (WSound bSound in bSounds) {
        bSound.StopSound();
      }
    }

    public bool IsSoundPlaying(string audioClipName) {
      var source = _sources.Find(x => x.IsPlaying && x.AudioName == audioClipName);
      return source != null;
    }

    public WSound GetPlayingSound(string audioClipName) {
      for (int i = 0; i < _currentPlaying.Count; ++i) {
        WSound sound = _currentPlaying[i];
        if (sound.AudioName == audioClipName) {
          return sound;
        }
      }

      return null;
    }

    WSound GetAvailableSound() {
      foreach (var soundFX in _sources) {
        if (!soundFX.IsPlaying) {
          return soundFX;
        }
      }
      return null;
    }

    void AddToCurPlaying(WSound sound) {
      if (_currentPlaying.Exists(x => x.GetInstanceID() == sound.GetInstanceID())) return;

      _currentPlaying.Add(sound);
    }
  }
}

