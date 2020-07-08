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
      public Sprite[] Sprites;
      public float MaxSize;
    }

    /// <summary>
    /// Acts as a container for multiple animations in a Gameobject.
    /// </summary>
    public class CAnimations {
      Dictionary<string, AnimData> m_anims = new Dictionary<string, AnimData>();
      AnimData m_curAnim = null;
      int m_curIdx = 0;
      Timer m_timer;
      float m_duration;
      Image m_image;

      public float Duration {
        get { return m_duration; }
        set {
          m_duration = value;
          m_timer = new Timer(m_duration);
          m_curIdx = 0;
        }
      }

      public CAnimations(AnimData[] anims, Image image, float duration = 0.1f) {
        for (int i = 0; i < anims.Length; ++i) {
          m_anims.Add(anims[i].Id, anims[i]);
        }
        //cache the img
        m_image = image;
        //set speed
        Duration = duration;
      }

      public void SetAnim(string id) {
        if (m_anims.TryGetValue(id, out m_curAnim)) {
          m_curIdx = 0;
        }
      }

      public void Play(float dt) {
        if (null != m_curAnim) {
          if (!m_timer.UpdateTimer(dt)) {
            m_timer.Reset();
            ++m_curIdx;
            if (m_curIdx >= m_curAnim.Sprites.Length) {
              m_curIdx = 0;
            }
          }
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