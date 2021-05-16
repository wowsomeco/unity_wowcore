using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Anim;
using Wowsome.Chrono;
using Wowsome.Core;
using Wowsome.Generic;

namespace Wowsome.Tasks {
  /// <summary>
  /// Singleton for running tasks subsequently.
  /// 
  /// How to use:
  /// 
  /// 1. Attach this script to a prefab and 
  /// 2. Add the prefab as one of the System Prefabs in [[CavEngine]] via Editor
  /// 
  /// You might want to subclass according to your needs
  /// Can be used for fake loading OR
  /// if you want to break down the performance heavy object creations into multiple tasks,
  /// so that you can show the progress bar accordingly
  /// </summary>
  public abstract class WTaskQueue : MonoBehaviour, ISystem {
    public class Task {
      public float StartDelay { get; private set; }
      public Action Callback { get; private set; }
      public float EndDelay { get; private set; }

      public Task(Action c, float startDelay = 0f, float endDelay = 0f) {
        Callback = c;
        StartDelay = startDelay;
        EndDelay = endDelay;
      }
    }

    public class TaskTimer {
      float _endDelay;
      Timer _timer;
      bool _isEnd = false;

      public TaskTimer(float start, float end) {
        _endDelay = end;
        _timer = new Timer(start);
      }

      public bool Update(float dt) {
        bool updating = false;

        if (null != _timer) {
          updating = _timer.UpdateTimer(dt);
          if (!updating) {
            if (!_isEnd) {
              _isEnd = true;
              _timer = new Timer(_endDelay);
              updating = true;
            } else {
              _timer = null;
            }
          }
        }

        return updating;
      }
    }

    public bool IsBusy { get; protected set; } = false;
    /// <summary>
    /// Progress observable value
    /// </summary>
    public WObservable<CapacityData> Progress { get; private set; } = new WObservable<CapacityData>(null);

    Queue<Task> _tasks = new Queue<Task>();
    TaskTimer _timer = null;

    public abstract void Load(IEnumerable<Task> tasks, Timing lerpTime = null);

    public void AddTasks(IEnumerable<Task> tasks) {
      // only can perform 1 bulk of tasks at a time
      if (_tasks.Count > 0 || null == tasks) return;

      foreach (Task t in tasks) {
        _tasks.Enqueue(t);
      }
      // update progress and set busy
      Progress.Next(new CapacityData(_tasks.Count));
      IsBusy = true;
    }

    #region ISystem

    public virtual void InitSystem() { }

    public virtual void StartSystem(CavEngine gameEngine) { }

    public virtual void UpdateSystem(float dt) {
      int taskCount = _tasks.Count;
      if (taskCount == 0) return;

      if (taskCount > 0) {
        Task t = _tasks.Peek();
        if (null != _timer) {
          // exec callback on done and dequeue
          if (!_timer.Update(dt)) {
            t.Callback();
            _timer = null;
            _tasks.Dequeue();

            Progress.Value.Add();
            Progress.Broadcast();

            if (_tasks.Count == 0) {
              IsBusy = false;
            }
          }
        } else {
          _timer = new TaskTimer(t.StartDelay, t.EndDelay);
        }
      }
    }

    #endregion
  }
}

