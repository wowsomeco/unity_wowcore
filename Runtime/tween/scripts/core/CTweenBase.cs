using UnityEngine;

namespace Wowsome {
  namespace Tween {
    #region Base
    public class CTweenBase<T> : ITween where T : ITweenTargetData, new() {
      protected CTween<T> m_tween;
      protected LoopData m_loop;

      OnCompleteCallback m_callback;

      public CTweenBase(string id, TargetType type, GameObject go) {
        m_tween = new CTween<T>(type, go, id);
        m_loop = new LoopData(0);
      }

      public CTweenBase(string id, TargetType type, GameObject go, LoopData loop) {
        m_tween = new CTween<T>(type, go, id);
        m_loop = loop;
      }

      public bool IsPlaying {
        get { return m_tween.IsPlaying; }
      }

      public string TweenId {
        get { return m_tween.Id; }
      }

      public void FastForward() {
        m_tween.FastForward();
      }

      public virtual void Play() { }

      public void Setup() { }

      public void Stop() {
        m_tween.Stop();
      }

      public bool UpdateTween(float dt) {
        bool isUpdating = m_tween.UpdateTween(dt);
        if (!isUpdating && null != m_callback) {
          m_callback.Invoke();
        }
        return isUpdating;
      }

      #region Chaining Methods
      public CTweenBase<T> SetLoop(LoopData data) {
        m_loop = data;
        return this;
      }

      public CTweenBase<T> SetLoop(int count, Loop loop = Loop.Restart) {
        return SetLoop(new LoopData(count, loop));
      }

      public CTweenBase<T> SetCompleteCallback(OnCompleteCallback callback) {
        m_callback = callback;
        return this;
      }

      public CTweenBase<T> Start() {
        Play();
        return this;
      }
      #endregion
    }
    #endregion

    #region Tween Fade
    public class CTweenFade : CTweenBase<TweenFadeData> {
      FadeData[] m_data;
      bool m_shouldFlipOnStart;

      public CTweenFade(GameObject go, float alpha, float duration, TargetType type = TargetType.Image, bool shouldFlipOnStart = false)
          : base(go.name, type, go, new LoopData(0)) {
        m_data = new FadeData[] { new FadeData(alpha, new TweenData(new TimeData(duration))) };
        m_shouldFlipOnStart = shouldFlipOnStart;
      }

      public CTweenFade(GameObject go, float alpha, float duration, float delay = 0f, Easing ease = Easing.Linear, TargetType type = TargetType.Image)
          : base(go.name, type, go, new LoopData(0)) {
        m_data = new FadeData[] { new FadeData(alpha, new TweenData(ease, new TimeData(duration, delay))) };
        m_shouldFlipOnStart = false;
      }

      public CTweenFade(TargetType type, GameObject go, FadeData fadeData) : base(go.name, type, go) {
        m_data = new FadeData[] { fadeData };
        m_shouldFlipOnStart = false;
      }

      public CTweenFade(string id, TargetType type, GameObject go, FadeData[] fadeData, bool shouldFlipOnStart = false) : base(id, type, go) {
        Init(fadeData, new LoopData(0), shouldFlipOnStart);
      }

      public CTweenFade(TargetType type, GameObject go, FadeData[] fadeData, bool shouldFlipOnStart = false) : base(go.name, type, go) {
        Init(fadeData, new LoopData(0), shouldFlipOnStart);
      }

      public CTweenFade(string id, TargetType type, GameObject go, FadeData[] fadeData, LoopData loopData, bool shouldFlipOnStart = false) : base(id, type, go) {
        Init(fadeData, loopData, shouldFlipOnStart);
      }

      public override void Play() {
        float frAlpha = 0f;
        float toAlpha = 0f;
        TweenLerpData[] nodes = new TweenLerpData[m_data.Length];

        for (int i = 0; i < m_data.Length; ++i) {
          if (i == 0) {
            if (m_shouldFlipOnStart) {
              m_tween.Values = new float[] { m_data[m_data.Length - 1].m_alpha >= 1f ? 0f : 1f };
            }
            frAlpha = m_tween.Values[0];
          } else {
            frAlpha = toAlpha;
          }
          //set to alpha based on the tween type
          toAlpha = m_data[i].m_tween.m_tweenType == TweenType.To ? m_data[i].m_alpha : frAlpha + m_data[i].m_alpha;
          //instantiate the lerp data node
          nodes[i] = new TweenLerpData(frAlpha, toAlpha, m_data[i].m_tween);
        }
        //finally, play the tween
        m_tween.Play(nodes, m_loop);
      }

      void Init(FadeData[] fadeData, LoopData loopData, bool flipOnStart = false) {
        m_data = fadeData;
        m_loop = loopData;
        m_shouldFlipOnStart = flipOnStart;
      }
    }
    #endregion

    #region Tween Color
    public class CTweenColor : CTweenBase<TweenColorData> {
      ColorData[] m_data;

      public CTweenColor(GameObject go, Color color, float duration, float delay = 0f, Easing ease = Easing.Linear, TargetType type = TargetType.Image)
          : base(go.name, type, go, new LoopData(0)) {
        m_data = new ColorData[] { new ColorData(color, new TweenData(ease, new TimeData(duration, delay))) };
      }

      public CTweenColor(string id, TargetType type, GameObject go, ColorData[] data)
          : base(id, type, go) {
        m_data = data;
      }

      public CTweenColor(string id, TargetType type, GameObject go, ColorData[] data, LoopData loop)
          : base(id, type, go, loop) {
        m_data = data;
      }

      public override void Play() {
        Color frCol = Color.white;
        Color toCol = Color.white;
        TweenLerpData[] lerps = new TweenLerpData[m_data.Length];

        for (int i = 0; i < m_data.Length; ++i) {
          frCol = i == 0 ? m_tween.Values.ToColor() : toCol;
          toCol = m_data[i].m_tween.m_tweenType == TweenType.To ? m_data[i].m_color : frCol + m_data[i].m_color;

          lerps[i] = new TweenLerpData(frCol.ToFloats(), toCol.ToFloats(), m_data[i].m_tween);
        }

        m_tween.Play(lerps, m_loop);
      }
    }
    #endregion

    #region Tween Move
    public class CTweenMove : CTweenBase<TweenMoveData> {
      MoveData[] m_data;

      public CTweenMove(string id, GameObject go, Vector2 pos, float duration, Easing ease = Easing.Linear, TargetType type = TargetType.RectTransform)
          : base(id, type, go, new LoopData(0)) {
        m_data = new MoveData[] { new MoveData(pos, duration, TweenType.To, ease) };
      }

      public CTweenMove(GameObject go, Vector2 pos, RangeData range, Easing ease = Easing.Linear, TargetType type = TargetType.RectTransform)
          : base(go.name, type, go, new LoopData(0)) {
        m_data = new MoveData[] { new MoveData(pos, range, TweenType.To, ease) };
      }

      public CTweenMove(GameObject go, Vector2 pos, float duration, Easing ease = Easing.Linear, TargetType type = TargetType.RectTransform)
          : base(go.name, type, go, new LoopData(0)) {
        m_data = new MoveData[] { new MoveData(pos, duration, TweenType.To, ease) };
      }

      public CTweenMove(GameObject go, MoveData[] moves, TargetType type = TargetType.RectTransform)
          : base(go.name, type, go) {
        m_data = moves;
      }

      public CTweenMove(TargetType type, GameObject go, MoveData moveData)
          : base(go.name, type, go) {
        m_data = new MoveData[] { moveData };
      }

      public CTweenMove(string id, TargetType type, GameObject go, MoveData[] moveData)
          : base(id, type, go) {
        m_data = moveData;
      }

      public CTweenMove(string id, TargetType type, GameObject go, MoveData[] moveData, LoopData loop)
          : base(id, type, go, loop) {
        m_data = moveData;
      }

      public override void Play() {
        Vector2 fromPos = Vector2.zero;
        Vector2 toPos = Vector2.zero;
        TweenLerpData[] nodes = new TweenLerpData[m_data.Length];

        for (int i = 0; i < m_data.Length; ++i) {
          if (i == 0) {
            fromPos = m_tween.Values.ToVector2();
          } else {
            //simply set from pos as to pos if it's not the first index
            fromPos = toPos;
          }
          toPos = m_data[i].m_tween.m_tweenType == TweenType.To ? m_data[i].m_pos : fromPos + m_data[i].m_pos;

          nodes[i] = new TweenLerpData(fromPos.ToFloats(), toPos.ToFloats(), m_data[i].m_tween);
        }

        m_tween.Play(nodes, m_loop);
      }
    }
    #endregion

    #region Tween Rotation
    public class CTweenRotation : CTweenBase<TweenRotationData> {
      RotationData[] m_data;

      public CTweenRotation(TargetType type, GameObject go, RotationData data)
          : base(go.name, type, go, new LoopData(0)) {
        m_data = new RotationData[] { data };
      }

      public CTweenRotation(GameObject go, float rotation, float duration, Easing ease = Easing.Linear, TargetType type = TargetType.RectTransform)
          : base(go.name, type, go, new LoopData(0)) {
        m_data = new RotationData[] { new RotationData(rotation, duration, ease) };
      }

      public CTweenRotation(string id, TargetType type, GameObject go, RotationData data)
          : base(id, type, go, new LoopData(0)) {
        m_data = new RotationData[] { data };
      }

      public CTweenRotation(string id, TargetType type, GameObject go, RotationData[] data)
          : base(id, type, go) {
        m_data = data;
      }

      public CTweenRotation(string id, TargetType type, GameObject go, RotationData[] data, LoopData loop)
          : base(id, type, go, loop) {
        m_data = data;
      }

      public override void Play() {
        float fr = 0f;
        float to = 0f;
        TweenLerpData[] nodes = new TweenLerpData[m_data.Length];

        for (int i = 0; i < m_data.Length; ++i) {
          fr = i == 0 ? m_tween.Values[0] : to;
          to = m_data[i].m_tween.m_tweenType == TweenType.To ?
              m_data[i].m_rotation : fr + m_data[i].m_rotation;
          // this needs to be done because minus angle will be incremented with 360 degrees by the unity euler angle
          // i.e. -30 degrees will be forced and returned as 330 degrees instead, hence below          
          if (fr > 180f) {
            fr -= 360f;
          }
          nodes[i] = new TweenLerpData(fr, to, m_data[i].m_tween);
        }

        m_tween.Play(nodes, m_loop);
      }
    }
    #endregion

    #region Tween Scale
    public class CTweenScale : CTweenBase<TweenScaleData> {
      ScaleData[] m_data;

      public CTweenScale(string id, GameObject go, Vector2 scale, float duration, float delay = 0f, Easing ease = Easing.Linear, TargetType type = TargetType.RectTransform)
          : base(id, type, go, new LoopData(0)) {
        m_data = new ScaleData[] { new ScaleData(scale, duration, ease, delay) };
      }

      public CTweenScale(GameObject go, Vector2 scale, float duration, float delay = 0f, Easing ease = Easing.Linear, TargetType type = TargetType.RectTransform)
          : base(go.name, type, go, new LoopData(0)) {
        m_data = new ScaleData[] { new ScaleData(scale, duration, ease, delay) };
      }

      public CTweenScale(TargetType type, GameObject go, ScaleData data)
          : base(go.name, type, go, new LoopData(0)) {
        m_data = new ScaleData[] { data };
      }

      public CTweenScale(TargetType type, GameObject go, ScaleData data, LoopData loop)
          : base(go.name, type, go, loop) {
        m_data = new ScaleData[] { data };
      }

      public CTweenScale(string id, TargetType type, GameObject go, ScaleData[] data)
          : base(id, type, go) {
        m_data = data;
      }

      public CTweenScale(string id, TargetType type, GameObject go, ScaleData[] data, LoopData loop)
          : base(id, type, go, loop) {
        m_data = data;
      }

      public override void Play() {
        Vector2 fr = Vector2.zero;
        Vector2 to = Vector2.zero;
        TweenLerpData[] nodes = new TweenLerpData[m_data.Length];
        for (int i = 0; i < m_data.Length; ++i) {
          fr = i == 0 ? m_tween.Values.ToVector2() : to;
          to = m_data[i].m_tween.m_tweenType == TweenType.To ? m_data[i].m_scale : fr + m_data[i].m_scale;
          nodes[i] = new TweenLerpData(fr.ToFloats(), to.ToFloats(), m_data[i].m_tween);
        }

        m_tween.Play(nodes, m_loop);
      }
    }
    #endregion

    #region Tween Number
    public class CTweenNumber : CTweenBase<TweenNumberData> {
      NumberData m_data;

      public CTweenNumber(string id, TargetType type, GameObject go, NumberData data)
          : base(id, type, go) {
        m_data = data;
      }

      public CTweenNumber(string id, TargetType type, GameObject go, NumberData data, LoopData loop)
          : base(id, type, go, loop) {
        m_data = data;
      }

      public override void Play() {
        TweenLerpData node = new TweenLerpData(
            m_data.m_from
            , m_data.m_to
            , new TweenData(m_data.m_easeType, m_data.m_timeData)
            );
        //play
        m_tween.Play(node, m_loop);
      }
    }
    #endregion
  }
}
