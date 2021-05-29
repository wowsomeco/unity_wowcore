using UnityEngine;

namespace Wowsome.Anim {
  public sealed class WAnimatorCG : WAnimatorBase {
    public CanvasGroup CanvasGroup { get; private set; }

    float _initAlpha;

    public override void InitAnimator() {
      CanvasGroup = otherTarget?.GetComponent<CanvasGroup>() ?? GetComponent<CanvasGroup>();
      Assert.Null<CanvasGroup, WAnimatorCG>(CanvasGroup, gameObject);

      _initAlpha = CanvasGroup.alpha;

      _setters[FrameType.Alpha] = v => CanvasGroup.alpha = v[0];

      _getters[FrameType.Alpha] = () => CanvasGroup.alpha.ToVector2();
    }

    public override void SetInitialValue() {
      CanvasGroup.alpha = _initAlpha;
    }
  }
}

