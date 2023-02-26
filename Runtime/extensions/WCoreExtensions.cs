using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Wowsome.Audio;
using Wowsome.Core;
using Wowsome.Tasks;

namespace Wowsome {
  public static class WCoreExt {
    public static void LoadScene(this WTaskQueue taskLoader, string sceneName, Action act = null) {
      var tasks = new List<WTaskQueue.Task>();
      if (null != act) tasks.Add(new WTaskQueue.Task(act));
      tasks.Add(new WTaskQueue.Task(() => SceneManager.LoadScene(sceneName)));

      taskLoader.Load(tasks);
    }

    public static void LoadPrevScene(this WTaskQueue taskLoader, WEngine engine, Action act = null) {
      string prevScene = engine.PrevSceneName;

      LoadScene(taskLoader, prevScene, act);
    }

    public static void RestartScene(this WTaskQueue taskLoader, Action act = null) {
      taskLoader.LoadScene(SceneManager.GetActiveScene().name, act);
    }

    public static WAudioSystem GetAudioSystem(this WEngine engine) {
      return engine.GetSystem<WAudioSystem>();
    }

    public static WSfxManager GetSfxManager(this WEngine engine) {
      return engine.GetAudioSystem().GetManager<WSfxManager>();
    }

    public static WBgmManager GetBgmManager(this WEngine engine) {
      return engine.GetAudioSystem().GetManager<WBgmManager>();
    }
  }
}