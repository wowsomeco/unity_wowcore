using UnityEngine;
using UnityEngine.UI;

namespace Wowsome {
  namespace Tween {
    public class TweenSpriteAnim : MonoBehaviour, ITween, ITweenTarget<TweenAnimData> {
      public TweenCommonData m_tweenData;
      public SpriteAnimData[] m_clips;
      public Vector2[] m_clipPositions;
      public LoopData m_loopData;

      CTween<TweenAnimData> m_tween;
      Image m_image;

      #region ITransition implementation
      public string TweenId {
        get { return m_tweenData.m_id; }
      }

      public bool IsPlaying {
        get { return m_tween.IsPlaying; }
      }

      public void Setup() {
        //get the image
        m_image = m_tweenData.GetTweenObject(gameObject).GetComponent<Image>();
        Debug.Assert(null != m_image, "cant find any image component!");
        //setup the ctween
        m_tween = new CTween<TweenAnimData>(this, TweenId);
      }

      public void Play() {
        TweenLerpData[] nodes = new TweenLerpData[m_clips.Length];
        for (int i = 0; i < m_clips.Length; ++i) {
          nodes[i] = new TweenLerpData(0f, 1f, m_clips[i].m_duration);
        }
        m_tween.Play(nodes, m_loopData);
      }

      public void Stop() {
        m_tween.Stop();
      }

      public void FastForward() {
        m_tween.FastForward();
      }

      public void Replay() {
        Stop();
        Play();
      }

      public bool UpdateTween(float dt) {
        return m_tween.UpdateTween(dt);
      }
      #endregion

      public TweenAnimData GetTargetData(TweenAnimData data) {
        data.TargetData = new float[] { 0f };
        return data;
      }

      public void SetTargetData(TweenAnimData data) {
        SpriteAnimData clip = m_clips[data.CurIdx];
        //set the sprite
        m_image.sprite = clip.m_sprite;
        //change max size if necessary
        if (clip.m_maxSize > 0f) {
          m_image.SetMaxSize(clip.m_maxSize);
        }
        //set pos
        if (data.CurIdx < m_clipPositions.Length) {
          m_image.rectTransform.SetPos(m_clipPositions[data.CurIdx]);
        }
      }
    }
  }
}