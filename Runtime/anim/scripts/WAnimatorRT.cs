using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  [RequireComponent(typeof(RectTransform))]
  public class WAnimatorRT : WAnimatorBase {
    public sealed class RTInitValueHandler {
      RectTransform _rt;
      Vector2 _pos;
      Vector2 _scale;
      Vector2 _size;
      float _rotation;

      public RTInitValueHandler(RectTransform rt) {
        _rt = rt;
        _pos = _rt.Pos();
        _size = _rt.Size();
        _scale = _rt.Scale();
        _rotation = _rt.Rotation();
      }

      public void Reset() {
        _rt.SetPos(_pos);
        _rt.SetSize(_size);
        _rt.SetScale(_scale);
        _rt.SetRotation(_rotation);
      }
    }

    public RectTransform RectTransform { get; private set; }

    protected Dictionary<FrameType, Action<Vector2>> _setters = new Dictionary<FrameType, Action<Vector2>>();
    protected Dictionary<FrameType, GetCur> _getters = new Dictionary<FrameType, GetCur>();

    RTInitValueHandler _initValue;

    public override void InitAnimator() {
      RectTransform = GetComponent<RectTransform>();
      _initValue = new RTInitValueHandler(RectTransform);
      // setters
      _setters[FrameType.Position] = v => RectTransform.SetPos(v);
      _setters[FrameType.Scale] = v => RectTransform.SetScale(v);
      _setters[FrameType.Rotation] = v => RectTransform.SetRotation(v[0]);
      // getters
      _getters[FrameType.Position] = () => RectTransform.Pos();
      _getters[FrameType.Scale] = () => RectTransform.Scale();
      _getters[FrameType.Rotation] = () => new Vector2(RectTransform.Rotation(), 0f);
    }

    public override void SetInitialValue() {
      _initValue.Reset();
    }

    #region IAnimatable

    public override Vector2 GetCurrentValue(FrameType type) {
      if (_getters.ContainsKey(type)) {
        return _getters[type]();
      }

      return Vector2.zero;
    }

    public override void OnLerp(FrameType type, Vector2 cur) {
      if (_setters.ContainsKey(type)) {
        _setters[type](cur);
      }
    }

    #endregion
  }
}

