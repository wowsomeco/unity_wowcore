using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  /// <summary>
  /// Base class for animatable object.
  /// </summary>  
  public abstract class WAnimatorBase : MonoBehaviour, IAnimatable {
    public delegate Vector2 GetCur();

    public string id;
    [Tooltip("leave it to null if the target is self. when it's defined, the target will be this gameobject instead of self")]
    public GameObject otherTarget;
    public List<WAnimation> animations = new List<WAnimation>();

    protected Dictionary<FrameType, Action<Vector2>> _setters = new Dictionary<FrameType, Action<Vector2>>();
    protected Dictionary<FrameType, GetCur> _getters = new Dictionary<FrameType, GetCur>();

    List<AnimStepController> _controllers = new List<AnimStepController>();

    #region IAnimatable

    public virtual Vector2 GetCurrentValue(FrameType type) {
      if (_getters.ContainsKey(type)) {
        return _getters[type]();
      }

      return Vector2.zero;
    }

    public virtual void OnLerp(FrameType type, Vector2 cur) {
      if (_setters.ContainsKey(type)) {
        _setters[type](cur);
      }
    }

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

    protected T GetTarget<T>() where T : Component {
      T component = null;

      if (null != otherTarget) {
        component = otherTarget.GetComponent<T>();
      } else {
        component = GetComponent<T>();
      }

      Assert.Null<T, WAnimatorBase>(component, gameObject);

      return GetComponent<T>();
    }
  }
}

