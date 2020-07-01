using UnityEngine;

namespace Wowsome {
  namespace Tween {
    /// <summary>
    /// TweenChainer is a Unity Component that animates a Gameobject on Start() where this script is attached to
    /// </summary>
    /// <description>
    /// It will find all the ITween(s) e.g. TweenFade, TweenScale, etc that gets attached to the same Gameobject or its Children and
    /// play them by the IDs defined in m_playOnAwake
    /// </description>
    public class TweenChainer : MonoBehaviour {
      /// <summary>
      /// Either Simultaneous or Step By Step
      /// Simultaneous will play all the m_playOnAwake tween(s)
      /// StepByStep will play it one at a time.
      /// </summary>
      public TweenerType m_tweenerType;
      /// <summary>
      /// The Tween IDs of the existing ITween(s) attached to the Gameobject or its children.
      /// </summary>
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
