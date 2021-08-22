using UnityEngine;

namespace Wowsome.Anim {
  public abstract class WAnimatorStaticBase : MonoBehaviour, IAnimatable {
    public string Id => id;

    public string id;

    public bool Animate(float dt) => false;

    public virtual void InitAnimator(WAnimController controller) { }

    public abstract void Play();
  }
}

