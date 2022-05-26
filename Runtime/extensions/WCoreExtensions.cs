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

    public static void RestartScene(this WTaskQueue taskLoader, Action act = null) {
      taskLoader.LoadScene(SceneManager.GetActiveScene().name, act);
    }

    public static WSfxManager GetSfxManager(this WEngine engine) {
      return engine.GetSystem<WAudioSystem>().GetManager<WSfxManager>();
    }
  }
}