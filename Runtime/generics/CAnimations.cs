using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wowsome.Chrono;

namespace Wowsome {
  namespace Generic {
    [Serializable]
    public class AnimData {
      public string Id;
      public List<Sprite> Sprites = new List<Sprite>();
      public float Duration;
      [Tooltip("The number of time the anim needs to loop e.g. 0 will play 1 time, -1 = infinitely")]
      public int Loop;
      public float MaxSize;
      public int SpriteCount;
    }

    /// <summary>
    /// Acts as a container for multiple animations in a Gameobject.
    /// </summary>
    public class CAnimations {
      public delegate void Done();

      Dictionary<string, AnimData> m_anims = new Dictionary<string, AnimData>();
      Image m_image;
      AnimData m_curAnim = null;
      int m_curIdx = 0;
      int m_counter = 0;
      Timer m_timer;
      Done m_onDone;

      public CAnimations(IList<AnimData> anims, Image image) {
        for (int i = 0; i < anims.Count; ++i) {
          m_anims.Add(anims[i].Id, anims[i]);
        }
        //cache the img
        m_image = image;
      }

      public void Play(string id, Done onDone) {
        if (m_anims.TryGetValue(id, out m_curAnim)) {
          m_timer = new Timer(m_curAnim.Duration);
          m_counter = m_curIdx = 0;
          m_onDone = onDone;
        } else {
          Debug.LogError("Can't find anim with id=" + id);
        }
      }

      public void Update(float dt) {
        if (null != m_curAnim) {
          // update timer here
          if (!m_timer.UpdateTimer(dt)) {
            m_timer.Reset();

            ++m_curIdx;
            if (m_curIdx >= m_curAnim.Sprites.Count) {
              m_curIdx = 0;
              ++m_counter;
            }
          }
          // check loop count here
          if (m_curAnim.Loop > -1 && m_counter > m_curAnim.Loop) {
            m_onDone();
            m_curAnim = null;
          } else {
            m_image.sprite = m_curAnim.Sprites[m_curIdx];
            float maxSize = m_curAnim.MaxSize;
            if (maxSize > 0f) {
              m_image.SetMaxSize(maxSize);
            }
          }
        }
      }
    }
  }
}