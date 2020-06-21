using UnityEngine;

namespace Wowsome {
  namespace Tween {
    public class TweenChainer : MonoBehaviour {
      public TweenerType m_tweenerType;
      public string[] m_playOnAwake;

      CTweenChainer m_tweener;

      public void Play(string tweenId) {
        m_tweener.PlayExistingTween(new string[] { tweenId });
      }

      public void Stop() {
        m_tweener.Stop();
      }

      public void FastForward() {
        m_tweener.FastForward();
      }

      public void Replay() {
        FastForward();
        m_tweener.PlayExistingTween(m_playOnAwake);
      }

      void Start() {
        m_tweener = new CTweenChainer(m_tweenerType);
        m_tweener.Add(gameObject, true);
        Replay();
      }

      void Update() {
        m_tweener.Update(Time.deltaTime);
      }
    }
  }
}
