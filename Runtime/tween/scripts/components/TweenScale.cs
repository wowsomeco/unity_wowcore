﻿using UnityEngine;

namespace Wowsome {
  namespace Tween {
    public class TweenScale : MonoBehaviour, ITween {
      public TweenCommonData m_tweenData;
      public TargetType m_type;
      public ScaleData[] m_scaleData;
      public LoopData m_loopData;

      CTweenScale m_tween;

      #region ITransition implementation
      public string TweenId {
        get { return m_tweenData.m_id; }
      }

      public bool IsPlaying {
        get { return m_tween.IsPlaying; }
      }

      public void Setup() {
        m_tween = new CTweenScale(TweenId, m_type, m_tweenData.GetTweenObject(gameObject), m_scaleData, m_loopData);
      }

      public void Play() {
        m_tween.Play();
      }

      public void Stop() {
        m_tween.Stop();
      }

      public void FastForward() {
        m_tween.FastForward();
      }

      public bool UpdateTween(float dt) {
        return m_tween.UpdateTween(dt);
      }
      #endregion
    }
  }
}
