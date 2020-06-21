using UnityEngine;

namespace Wowsome {
  namespace Tween {
    public class TweenFade : MonoBehaviour, ITween {
      public TweenCommonData m_tweenData;
      public TargetType m_type;
      public FadeData[] m_fadeData;
      public LoopData m_loopData;
      public bool m_shouldFlipOnStart;

      CTweenFade m_tween;

      #region ITransition implementation
      public string TweenId {
        get { return m_tweenData.m_id; }
      }

      public bool IsPlaying {
        get { return m_tween.IsPlaying; }
      }

      public void Setup() {
        m_tween = new CTweenFade(TweenId, m_type, m_tweenData.GetTweenObject(gameObject), m_fadeData, m_loopData, m_shouldFlipOnStart);
        m_tween.Setup();
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