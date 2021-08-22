using UnityEngine;
using Wowsome.Chrono;

namespace Wowsome.Anim {
  public sealed class WAnimatorTimer : MonoBehaviour, IAnimatable {
    public string Id => id;

    public string id;
    public float duration;
    public string nextAnimId;

    WAnimController _controller;
    ObservableTimer _timer = null;

    public bool Animate(float dt) {
      if (null == _timer) return false;

      return _timer.UpdateTimer(dt);
    }

    public void Play() {
      _timer = new ObservableTimer(duration);
      _timer.OnDone += () => {
        _timer = null;
        _controller.PlayAnim(nextAnimId);
      };
    }

    public void InitAnimator(WAnimController controller) {
      _controller = controller;
    }
  }
}

