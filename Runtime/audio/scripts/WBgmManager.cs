using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wowsome.Audio {
  public class WBgmManager : MonoBehaviour, IAudioManager {
    [Serializable]
    public struct BgmData {
      /// <summary>
      /// The name of the sound file
      /// </summary>
      public string name;
      /// <summary>
      /// How many times the bgm needs to loop
      /// -1 = infinitely, 1 = 1 time, 2 = 2 times, etc.
      /// </summary>
      public int loopCount;
    }

    [Serializable]
    public class BgmsData {
      /// <summary>
      /// when set to true, the bgm will keep playing regardless of the scene.
      /// </summary>
      public bool isGlobal;
      /// <summary>
      /// the unity scene where the bgm needs to play at.
      /// </summary>
      public string sceneName;
      public List<BgmData> bgms;

      public bool Matches(string sceneName) {
        return this.sceneName == sceneName;
      }
    }

    public float Volume {
      get { return soundSource.Volume; }
      set { soundSource.Volume = value; }
    }

    public WSound soundSource;
    public List<BgmsData> bgms = new List<BgmsData>();
    public string path = "audio/bgm";

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    int _bgmCount = 0;
    BgmsData _curPlaying;

    #region IAudioManager
    public void InitAudioManager() {
      // on init, load all the sfx in the path defined
      AudioClip[] audioClipsFromResources = Resources.LoadAll<AudioClip>(path);
      for (int i = 0; i < audioClipsFromResources.Length; ++i) {
        _audioClips.Add(audioClipsFromResources[i].name, audioClipsFromResources[i]);
      }

      // init sound
      soundSource.InitSound();

      Debug.Assert(null != soundSource);
    }

    public void OnChangeScene(Scene scene) {
      BgmsData bgm = bgms.Find(x => x.Matches(scene.name) || x.isGlobal);
      if (null != bgm) {
        // dont replay if it is on the same scene OR the current playing is of global type. 
        if (null != _curPlaying && (_curPlaying.isGlobal || _curPlaying.Matches(scene.name))) {
          return;
        }
        // reset          
        _curPlaying = bgm;
        _bgmCount = 0;
        // play the sound
        Play(_curPlaying.bgms[_bgmCount]);
      }
    }

    public void UpdateAudio(float dt) {
      // check whenever the current source sound has finished playing
      if (!soundSource.gameObject.activeSelf) {
        // set next                  
        ++_bgmCount;
        // if exceeds, reset the count to 0
        if (_bgmCount >= _curPlaying.bgms.Count) {
          _bgmCount = 0;
        }
        // re activate the source sound
        soundSource.gameObject.SetActive(true);
        // play the next bgm
        Play(_curPlaying.bgms[_bgmCount]);
      } else {
        // update the sound
        soundSource.UpdateSound(dt);
      }
    }
    #endregion

    public void Play(BgmData bgmData) {
      AudioClip audioClip = null;
      // find the bgm name in the audio clips dictionary        
      if (_audioClips.TryGetValue(bgmData.name, out audioClip)) {
        soundSource.StopSound();
        soundSource.PlaySound(audioClip);
      }
    }
  }
}
