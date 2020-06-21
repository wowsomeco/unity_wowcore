using UnityEngine;

namespace Wowsome {
  namespace Tween {
    public class TweenColor : MonoBehaviour, ITween {
      public TweenCommonData m_tweenData;
      public TargetType m_type;
      public ColorData[] m_colorData;
      public LoopData m_loopData;

      CTweenColor m_tween;

      #region ITween implementation
      public string TweenId {
        get { return m_tweenData.m_id; }
      }

      public bool IsPlaying {
        get { return m_tween.IsPlaying; }
      }

      public void Setup() {
        m_tween = new CTweenColor(TweenId, m_type, m_tweenData.GetTweenObject(gameObject), m_colorData, m_loopData);
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
