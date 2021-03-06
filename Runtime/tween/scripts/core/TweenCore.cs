﻿using System;
using UnityEngine;

namespace Wowsome {
  namespace Tween {
    public delegate void OnCompleteCallback();

    public interface ITween {
      string TweenId { get; }
      bool IsPlaying { get; }
      void Setup();
      void Play();
      void Stop();
      void FastForward();
      bool UpdateTween(float dt);
    }

    #region TweenData
    [Serializable]
    public class TweenCommonData {
      public string m_id;
      [Tooltip("if null then it will use the gameobject where the tween component is attached to")]
      public GameObject m_otherTarget;

      public GameObject GetTweenObject(GameObject go) {
        return m_otherTarget != null ? m_otherTarget : go;
      }
    }

    [Serializable]
    public enum Loop {
      Restart,
      Yoyo
    }

    [Serializable]
    public struct LoopData {
      public int m_loopCount;
      public Loop m_loopType;

      public LoopData(int count) {
        m_loopCount = count;
        m_loopType = Loop.Restart;
      }

      public LoopData(int count, Loop loopType) {
        m_loopCount = count;
        m_loopType = loopType;
      }
    }

    [Serializable]
    public enum TweenType {
      To,
      By
    }

    [Serializable]
    public struct TweenData {
      public TweenType m_tweenType;
      public Easing m_easeType;
      public TimeData m_timeData;

      public TweenData(TweenType tweenType, Easing easeType, TimeData timeData) {
        m_tweenType = tweenType;
        m_easeType = easeType;
        m_timeData = timeData;
      }

      public TweenData(Easing easeType, TimeData timeData) {
        m_tweenType = TweenType.To;
        m_easeType = easeType;
        m_timeData = timeData;
      }

      public TweenData(TimeData timeData) {
        m_tweenType = TweenType.To;
        m_easeType = Easing.Linear;
        m_timeData = timeData;
      }
    }

    [Serializable]
    public struct MoveData {
      public Vector2 m_pos;
      public TweenData m_tween;

      public MoveData(Vector2 pos, TweenData tween) {
        m_pos = pos;
        m_tween = tween;
      }

      public MoveData(Vector2 pos, float duration, TweenType tweenType = TweenType.To, Easing ease = Easing.Linear, float delay = 0f) {
        m_pos = pos;
        m_tween = new TweenData(tweenType, ease, new TimeData(duration, delay));
      }

      public MoveData(Vector2 pos, RangeData range, TweenType tweenType = TweenType.To, Easing ease = Easing.Linear) {
        m_pos = pos;
        m_tween = new TweenData(tweenType, ease, new TimeData(range));
      }
    }

    [Serializable]
    public struct FadeData {
      public float m_alpha;
      public TweenData m_tween;

      public FadeData(float alpha, TweenData tween) {
        m_alpha = alpha;
        m_tween = tween;
      }

      public FadeData(float alpha, float duration, Easing ease = Easing.Linear, float delay = 0f) {
        m_alpha = alpha;
        m_tween = new TweenData(ease, new TimeData(duration, delay));
      }
    }

    [Serializable]
    public struct ScaleData {
      public Vector2 m_scale;
      public TweenData m_tween;

      public ScaleData(Vector2 scale, TweenData tween) {
        m_scale = scale;
        m_tween = tween;
      }

      public ScaleData(Vector2 scale, float duration, Easing ease = Easing.Linear, float delay = 0f) {
        m_scale = scale;
        m_tween = new TweenData(ease, new TimeData(duration, delay));
      }
    }

    [Serializable]
    public struct RotationData {
      public float m_rotation;
      public TweenData m_tween;

      public RotationData(float rot, TweenData tween) {
        m_rotation = rot;
        m_tween = tween;
      }

      public RotationData(float rot, float duration, Easing ease = Easing.Linear, float delay = 0f) {
        m_rotation = rot;
        m_tween = new TweenData(ease, new TimeData(duration, delay));
      }
    }

    [Serializable]
    public struct ColorData {
      public Color m_color;
      public TweenData m_tween;

      public ColorData(Color color, TweenData tween) {
        m_color = color;
        m_tween = tween;
      }
    }

    [Serializable]
    public struct NumberData {
      public float m_from;
      public float m_to;
      public Easing m_easeType;
      public TimeData m_timeData;

      public NumberData(float fr, float to, Easing easeType, TimeData time) {
        m_from = fr;
        m_to = to;
        m_easeType = easeType;
        m_timeData = time;
      }
    }

    [Serializable]
    public class SpriteAnimData {
      public Sprite m_sprite;
      public float m_duration;
      public float m_maxSize;
    }
    #endregion

    #region ITweenTarget
    public abstract class ITweenTargetData {
      public float[] TargetData { get; set; }
      public int CurIdx { get; set; }
    }

    public class TweenMoveData : ITweenTargetData { }

    public class TweenRotationData : ITweenTargetData { }

    public class TweenScaleData : ITweenTargetData { }

    public class TweenFadeData : ITweenTargetData { }

    public class TweenColorData : ITweenTargetData { }

    public class TweenAnimData : ITweenTargetData { }

    public class TweenNumberData : ITweenTargetData { }

    public interface ITweenTarget<T> where T : ITweenTargetData {
      T GetTargetData(T data);
      void SetTargetData(T data);
    }
    #endregion

    /// <summary>
    /// Can use it to simplify the tween job.
    /// </summary>
    public class Tweener {
      /// <summary>
      /// Scale up once then scale down again to the origin scale
      /// </summary>
      /// <param name="go">The target object</param>
      /// <param name="targetScale">Vector2 of the up scale</param>
      /// <param name="duration">Duration of pulse</param>    
      public static ITween Pulse(GameObject go, float targetScale = 1.05f, float duration = 0.1f, int loop = 1) {
        return new CTweenScale(
          TargetType.RectTransform, go,
          new ScaleData(targetScale.ToVector2(), duration, Easing.OutQuad), new LoopData(loop, Loop.Yoyo)
        );
      }

      /// <summary>
      /// Fades a gameobject to the alpha defined
      /// </summary>
      public static ITween Fade(GameObject go, float alpha, TargetType type = TargetType.Image, float duration = 0.3f) {
        return new CTweenFade(go, alpha, duration, type, true);
      }

      /// <summary>
      /// Fade alpha in to 1f, forces it to begin at 0f 
      /// </summary>
      public static ITween FadeIn(GameObject go, TargetType type = TargetType.Image, float duration = 0.3f) {
        return Fade(go, 1f, type, duration);
      }

      /// <summary>
      /// Fade alpha out to 0f, forces it to begin at 1f 
      /// </summary>
      public static ITween FadeOut(GameObject go, TargetType type = TargetType.Image, float duration = 0.3f) {
        return Fade(go, 0f, type, duration);
      }

      public static ITween FadeInOut(GameObject go, float duration = 0.2f, TargetType type = TargetType.CanvasGroup) {
        return new CTweenFade(
          go.name,
          type,
          go,
          new FadeData[] { new FadeData(1f, new TweenData(Easing.OutQuad, new TimeData(duration, 0f))) },
          new LoopData(1, Loop.Yoyo),
          true
        );
      }

      public static ITween Slide(RectTransform rt, Vector2 from, float duration = 0.1f) {
        Vector2 curPos = rt.Pos();
        rt.SetPos(curPos - from);
        return new CTweenMove(rt.gameObject, curPos, duration, Easing.OutQuad);
      }

      public static ITween SlideFromBottom(RectTransform rt, float duration = 0.1f) {
        return Slide(rt, new Vector2(0f, 10f));
      }

      public static ITween SlideFromRight(RectTransform rt, float duration = 0.1f) {
        return Slide(rt, new Vector2(-10f, 0f));
      }

      ITween _tween;
      Action _callback;
      bool _playing = false;

      public bool IsPlaying {
        get { return _tween.IsPlaying; }
      }

      public Tweener(ITween tw) {
        _tween = tw;
      }

      public void Play(Action callback = null) {
        _tween.FastForward();
        _tween.Play();

        _callback = callback;
        _playing = true;
      }

      public bool Update(float dt) {
        if (_playing) {
          if (!_tween.UpdateTween(dt)) {
            _playing = false;
            if (null != _callback) _callback();
          }
          return true;
        }
        return false;
      }
    }
  }
}