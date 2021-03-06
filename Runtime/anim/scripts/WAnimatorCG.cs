﻿using UnityEngine;

namespace Wowsome.Anim {
  public sealed class WAnimatorCG : WAnimatorBase {
    public CanvasGroup CanvasGroup { get; private set; }

    float _initAlpha;

    public override void InitAnimator() {
      CanvasGroup = GetTarget<CanvasGroup>();

      _initAlpha = CanvasGroup.alpha;

      _setters[FrameType.Alpha] = v => CanvasGroup.alpha = v[0];

      _getters[FrameType.Alpha] = () => CanvasGroup.alpha.ToVector2();
    }

    public override void SetInitialValue() {
      CanvasGroup.alpha = _initAlpha;
    }
  }
}

