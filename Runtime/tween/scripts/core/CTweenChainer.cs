using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome {
  namespace Tween {
    /// @enum   TweenerType
    ///
    /// @brief  Values that represent tweener types.
    [Serializable]
    public enum TweenerType {
      Simultaneous,
      StepByStep
    }

    public class CTweenChainer {
      TweenPlayer m_tweener;
      HashSet<ITween> m_tweens = new HashSet<ITween>();

      public bool IsPlaying {
        get { return m_tweener.Count > 0; }
      }

      #region Constructors
      public CTweenChainer() {
        m_tweener = new TweenPlayer(TweenerType.Simultaneous);
      }

      public CTweenChainer(TweenerType type) {
        m_tweener = new TweenPlayer(type);
      }
      #endregion

      public CTweenChainer Add(GameObject[] gameObjects) {
        //get all the tween components for each of the game object and set them up
        for (int i = 0; i < gameObjects.Length; ++i) {
          Add(gameObjects[i].GetComponents<ITween>());
        }
        return this;
      }

      public CTweenChainer Add(GameObject gameObject, bool includeChildren) {
        if (includeChildren) {
          Add(gameObject.GetComponentsInChildren<ITween>(true));
        } else {
          Add(gameObject.GetComponents<ITween>());
        }
        return this;
      }

      public CTweenChainer Add(ITween node) {
        node.Setup();
        m_tweens.Add(node);
        return this;
      }

      public CTweenChainer Add(ITween[] nodes) {
        for (int i = 0; i < nodes.Length; ++i) {
          Add(nodes[i]);
        }
        return this;
      }

      public void PlayOnly(IEnumerable<ITween> tweens, OnCompleteCallback callback = null) {
        m_tweener.Add(new TweenChunk(tweens, callback));
      }

      public bool PlayExistingTween(ICollection<string> tweenIds, OnCompleteCallback callback = null) {
        if (tweenIds.Count > 0) {
          //find all the tweens with this id
          HashSet<ITween> tweens = new HashSet<ITween>();
          foreach (string tweenId in tweenIds) {
            foreach (ITween existingTween in m_tweens) {
              //dont add to tween player if it's already playing
              if (existingTween.TweenId.IsEqual(tweenId) && !existingTween.IsPlaying) {
                tweens.Add(existingTween);
              }
            }
          }
          //add to player
          if (tweens.Count > 0) {
            m_tweener.Add(new TweenChunk(tweens, callback));
            return true;
          }
        }
        return false;
      }

      public void PlayExistingAll(OnCompleteCallback callback = null) {
        m_tweener.Add(new TweenChunk(m_tweens, callback));
      }

      public bool Update(float dt) {
        return m_tweener.UpdateContainer(dt);
      }

      public void FastForward() {
        m_tweener.FastForward();
      }

      public void Stop() {
        m_tweener.Stop();
      }

      public void Clear() {
        m_tweener.Clear();
      }

      public bool IsTweenPlaying(string id) {
        foreach (ITween tween in m_tweens) {
          if (tween.IsPlaying && tween.TweenId.IsEqual(id)) {
            return true;
          }
        }
        return false;
      }

      public bool IsTweensPlaying(string[] ids) {
        for (int i = 0; i < ids.Length; ++i) {
          if (IsTweenPlaying(ids[i])) {
            return true;
          }
        }
        return false;
      }

      public void StopTween(string id) {
        m_tweener.Remove(id);
      }

      public ITween GetExistingTween(string id) {
        foreach (ITween tween in m_tweens) {
          if (tween.TweenId.IsEqual(id)) {
            return tween;
          }
        }
        return null;
      }

      public HashSet<ITween> GetExistingTweens(string id) {
        HashSet<ITween> tweens = new HashSet<ITween>();
        foreach (ITween tween in m_tweens) {
          if (tween.TweenId.IsEqual(id)) {
            tweens.Add(tween);
          }
        }
        return tweens;
      }
    }

    class TweenChunk {
      HashSet<ITween> m_tweens;
      OnCompleteCallback m_callback;

      public TweenChunk(IEnumerable<ITween> tweens, OnCompleteCallback callback = null) {
        m_tweens = new HashSet<ITween>(tweens);
        m_callback = callback;
      }

      public void Start() {
        foreach (ITween tween in m_tweens) {
          tween.Play();
        }
      }

      public void FastForward() {
        foreach (ITween tween in m_tweens) {
          tween.FastForward();
        }
      }

      public void Stop() {
        foreach (ITween tween in m_tweens) {
          tween.Stop();
        }
      }

      public void Stop(string tweenId) {
        HashSet<ITween> tweens = new HashSet<ITween>();
        foreach (ITween tween in m_tweens) {
          if (tween.TweenId.IsEqual(tweenId)) {
            tweens.Add(tween);
          }
        }
        //remove them
        foreach (ITween tween in tweens) {
          tween.Stop();
          m_tweens.Remove(tween);
        }
      }

      public bool UpdateChunk(float dt) {
        bool isUpdating = false;
        foreach (ITween tween in m_tweens) {
          if (tween.UpdateTween(dt)) {
            isUpdating = true;
          }
        }
        //invoke the callback when done
        if (!isUpdating && null != m_callback) {
          m_callback.Invoke();
        }
        //return
        return isUpdating;
      }
    }

    /// @class  ContainerParallel
    ///
    /// @brief  Tweener container that will be used for playing tweens simultaneously
    class TweenPlayer {
      List<TweenChunk> m_chunks;
      TweenerType m_type;

      public TweenPlayer(TweenerType type) {
        m_chunks = new List<TweenChunk>();
        m_type = type;
      }

      public int Count { get { return m_chunks.Count; } }

      public void Add(TweenChunk chunk) {
        chunk.Start();
        m_chunks.Add(chunk);
      }

      public void FastForward() {
        while (m_chunks.Count > 0) {
          m_chunks[0].FastForward();
          m_chunks.Remove(m_chunks[0]);
        }
      }

      public void Stop() {
        while (m_chunks.Count > 0) {
          m_chunks[0].Stop();
          m_chunks.Remove(m_chunks[0]);
        }
      }

      public void Remove(string tweenId) {
        for (int i = 0; i < m_chunks.Count; ++i) {
          m_chunks[i].Stop(tweenId);
        }
      }

      public void Clear() {
        m_chunks.Clear();
      }

      public bool UpdateContainer(float dt) {
        int i = 0;
        while (i < m_chunks.Count) {
          TweenChunk cur = m_chunks[i];
          //if it's no longer updating, remove the nodes
          if (!cur.UpdateChunk(dt)) {
            if (m_chunks.Count > 0) {
              m_chunks.RemoveAt(i);
            } else {
              break;
            }
          }
          //if it's not done yet, check if should set next index
          else {
            //only increment the index if it should be simultaneous
            if (m_type == TweenerType.Simultaneous) {
              ++i;
            } else {
              //for the stepbystep type we play the first index only, get out of the loop
              break;
            }
          }
        }
        return m_chunks.Count > 0;
      }
    }
  }
}