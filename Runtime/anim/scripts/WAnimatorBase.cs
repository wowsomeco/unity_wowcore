using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  /// <summary>
  /// Base class for animatable object.
  /// </summary>  
  public abstract class WAnimatorBase : MonoBehaviour, IAnimatable {
    public delegate Vector2 GetCur();

    public string id;
    public List<WAnimation> animations = new List<WAnimation>();

    List<AnimStepController> _controllers = new List<AnimStepController>();

    #region IAnimatable

    public abstract Vector2 GetCurrentValue(FrameType type);
    public abstract void OnLerp(FrameType type, Vector2 cur);

    #endregion

    public abstract void InitAnimator();

    public abstract void SetInitialValue();

    public void Play() {
      Stop();
      animations.ForEach(a => {
        a.clips.ForEach(step => {
          var c = new AnimStepController(this, step);
          c.Start();
          _controllers.Add(c);
        });
      });
    }

    public void Stop() {
      _controllers.Clear();
      SetInitialValue();
    }

    public bool Animate(float dt) {
      bool isAnimating = false;
      foreach (AnimStepController c in _controllers) {
        bool animating = c.Animate(dt);
        // if any of the steps is still animating, return true
        if (animating) {
          isAnimating = true;
        }
      }

      return isAnimating;
    }
  }
}

