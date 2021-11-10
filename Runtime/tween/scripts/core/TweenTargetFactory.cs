using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wowsome.Tween {
  public enum TargetType {
    Image,
    CanvasGroup,
    RectTransform,
    Text,
    Transform,
    SpriteRenderer
  }

  public class TweenTargetFactory<T> where T : ITweenTargetData {
    private delegate ITweenTarget<T> CreateTweenTarget(GameObject go);

    private ITweenTarget<T> CreateTargetImage(GameObject go) {
      return new TargetImage(go) as ITweenTarget<T>;
    }

    private ITweenTarget<T> CreateTargetCanvasGroup(GameObject go) {
      return new TargetCanvasGroup(go) as ITweenTarget<T>;
    }

    private ITweenTarget<T> CreateTargetRectTransform(GameObject go) {
      return new TargetRectTransform(go) as ITweenTarget<T>;
    }

    private ITweenTarget<T> CreateTargetText(GameObject go) {
      return new TargetText(go) as ITweenTarget<T>;
    }

    private ITweenTarget<T> CreateTargetTransform(GameObject go) {
      return new TargetTransform(go) as ITweenTarget<T>;
    }

    private ITweenTarget<T> CreateTargetSpriteRenderer(GameObject go) {
      return new TargetSpriteRenderer(go) as ITweenTarget<T>;
    }

    private Dictionary<TargetType, CreateTweenTarget> m_targets = new Dictionary<TargetType, CreateTweenTarget>();

    public TweenTargetFactory() {
      m_targets[TargetType.Image] = CreateTargetImage;
      m_targets[TargetType.CanvasGroup] = CreateTargetCanvasGroup;
      m_targets[TargetType.RectTransform] = CreateTargetRectTransform;
      m_targets[TargetType.Text] = CreateTargetText;
      m_targets[TargetType.Transform] = CreateTargetTransform;
      m_targets[TargetType.SpriteRenderer] = CreateTargetSpriteRenderer;
    }

    public ITweenTarget<T> Create(TargetType targetType, GameObject go) {
      ITweenTarget<T> target = m_targets[targetType](go);
      Debug.Assert(null != target, "can not cast to ITweenTarget<" + typeof(T).ToString() + "> for gameobject = " + go.name + " with type = " + targetType.ToString() + ", use another tween target type!");
      return m_targets[targetType](go);
    }
  }

  public class TargetImage : ITweenTarget<TweenMoveData>, ITweenTarget<TweenRotationData>, ITweenTarget<TweenScaleData>, ITweenTarget<TweenFadeData>, ITweenTarget<TweenColorData> {
    private Image m_image;

    public TargetImage(GameObject go) {
      m_image = go.GetComponent<Image>();
      Debug.Assert(null != m_image, "cant find any image component in this gameobject = " + go.name);
    }

    #region ITweenMove implementation
    public TweenMoveData GetTargetData(TweenMoveData data) {
      data.TargetData = m_image.rectTransform.Pos().ToFloats();
      return data;
    }

    public void SetTargetData(TweenMoveData data) {
      m_image.rectTransform.SetPos(data.TargetData);
    }
    #endregion

    #region ITweenRotateable implementation
    public TweenRotationData GetTargetData(TweenRotationData data) {
      data.TargetData = new float[] { m_image.rectTransform.Rotation() };
      return data;
    }

    public void SetTargetData(TweenRotationData data) {
      m_image.rectTransform.SetRotation(data.TargetData[0]);
    }
    #endregion

    #region ITweenScaleable implementation
    public TweenScaleData GetTargetData(TweenScaleData data) {
      data.TargetData = m_image.rectTransform.Scale().ToFloats();
      return data;
    }

    public void SetTargetData(TweenScaleData data) {
      m_image.rectTransform.SetScale(data.TargetData);
    }
    #endregion

    #region ITweenFade implementation
    public TweenFadeData GetTargetData(TweenFadeData data) {
      data.TargetData = new float[] { m_image.Alpha() };
      return data;
    }

    public void SetTargetData(TweenFadeData data) {
      m_image.SetAlpha(data.TargetData[0]);
    }
    #endregion

    #region ITweenColorable implementation
    public TweenColorData GetTargetData(TweenColorData data) {
      data.TargetData = m_image.color.ToFloats();
      return data;
    }

    public void SetTargetData(TweenColorData data) {
      m_image.SetColor(data.TargetData);
    }
    #endregion
  }

  public class TargetRectTransform : ITweenTarget<TweenMoveData>, ITweenTarget<TweenRotationData>, ITweenTarget<TweenScaleData> {
    private RectTransform m_rectTransform;

    public TargetRectTransform(GameObject go) {
      m_rectTransform = go.GetComponent<RectTransform>();
      Debug.Assert(null != m_rectTransform, "cant find any rect transform component in this gameobject = " + go.name);
    }

    #region ITweenTarget implementation
    public TweenMoveData GetTargetData(TweenMoveData data) {
      data.TargetData = m_rectTransform.Pos().ToFloats();
      return data;
    }

    public void SetTargetData(TweenMoveData data) {
      m_rectTransform.SetPos(data.TargetData);
    }
    #endregion

    #region ITweenRotateable implementation
    public TweenRotationData GetTargetData(TweenRotationData data) {
      data.TargetData = new float[] { m_rectTransform.Rotation() };
      return data;
    }

    public void SetTargetData(TweenRotationData data) {
      m_rectTransform.SetRotation(data.TargetData[0]);
    }
    #endregion

    #region ITweenScaleable implementation
    public TweenScaleData GetTargetData(TweenScaleData data) {
      data.TargetData = m_rectTransform.Scale().ToFloats();
      return data;
    }

    public void SetTargetData(TweenScaleData data) {
      m_rectTransform.SetScale(data.TargetData);
    }
    #endregion
  }

  public class TargetCanvasGroup : ITweenTarget<TweenFadeData> {
    CanvasGroup m_canvasGroup;

    public TargetCanvasGroup(GameObject go) {
      m_canvasGroup = go.GetComponent<CanvasGroup>();
      if (null == m_canvasGroup) {
        m_canvasGroup = go.AddComponent<CanvasGroup>();
        Debug.Assert(null != "cant find any canvas group component in this gameobject = " + go.name);
      }
    }

    #region ITweenTarget implementation
    public TweenFadeData GetTargetData(TweenFadeData data) {
      data.TargetData = new float[] { m_canvasGroup.alpha };
      return data;
    }

    public void SetTargetData(TweenFadeData data) {
      float alpha = data.TargetData[0];
      m_canvasGroup.interactable = alpha >= 1f;
      m_canvasGroup.blocksRaycasts = alpha >= 1f;
      m_canvasGroup.alpha = alpha;
    }
    #endregion
  }

  public class TargetText : ITweenTarget<TweenNumberData>, ITweenTarget<TweenColorData>, ITweenTarget<TweenFadeData> {
    Text m_text;

    public TargetText(GameObject go) {
      m_text = go.GetComponent<Text>();
      Debug.Assert(null != "cant find any text component in this gameobject = " + go.name);
    }

    #region ITweenNumber implementation
    public TweenNumberData GetTargetData(TweenNumberData data) {
      return data;
    }

    public void SetTargetData(TweenNumberData data) {
      float num = data.TargetData[0];
      m_text.text = num.ToString("0");
    }
    #endregion

    #region ITweenColorable implementation
    public TweenColorData GetTargetData(TweenColorData data) {
      data.TargetData = m_text.color.ToFloats();
      return data;
    }

    public void SetTargetData(TweenColorData data) {
      m_text.SetColor(data.TargetData);
    }
    #endregion

    #region ITweenFade implementation
    public TweenFadeData GetTargetData(TweenFadeData data) {
      data.TargetData = new float[] { m_text.Alpha() };
      return data;
    }

    public void SetTargetData(TweenFadeData data) {
      float alpha = data.TargetData[0];
      m_text.SetAlpha(alpha);
    }
    #endregion
  }

  public class TargetTransform : ITweenTarget<TweenMoveData>, ITweenTarget<TweenScaleData> {
    Transform m_transform;

    public TargetTransform(GameObject go) {
      m_transform = go.GetComponent<Transform>();
      Debug.Assert(null != m_transform, "cant find any transform component in this gameobject = " + go.name);
    }

    #region ITweenTarget implementation
    public TweenMoveData GetTargetData(TweenMoveData data) {
      data.TargetData = new Vector2(m_transform.position.x, m_transform.position.y).ToFloats();
      return data;
    }

    public void SetTargetData(TweenMoveData data) {
      m_transform.position = new Vector2(data.TargetData[0], data.TargetData[1]);
    }
    #endregion

    #region ITweenScaleable implementation
    public TweenScaleData GetTargetData(TweenScaleData data) {
      data.TargetData = new Vector2(m_transform.localScale.x, m_transform.localScale.y).ToFloats();
      return data;
    }

    public void SetTargetData(TweenScaleData data) {
      m_transform.localScale = new Vector2(data.TargetData[0], data.TargetData[1]);
    }
    #endregion
  }

  public class TargetSpriteRenderer : ITweenTarget<TweenColorData>, ITweenTarget<TweenFadeData> {
    SpriteRenderer m_sprite;

    public TargetSpriteRenderer(GameObject go) {
      m_sprite = go.GetComponent<SpriteRenderer>();
      Debug.Assert(null != "cant find any sprite renderer component in this gameobject = " + go.name);
    }

    #region ITweenColorable implementation
    public TweenColorData GetTargetData(TweenColorData data) {
      data.TargetData = m_sprite.color.ToFloats();
      return data;
    }

    public void SetTargetData(TweenColorData data) {
      Color color = m_sprite.color;
      for (int i = 0; i < data.TargetData.Length; ++i) {
        color[i] = data.TargetData[i];
      }
      m_sprite.color = color;
    }
    #endregion

    #region ITweenFade implementation
    public TweenFadeData GetTargetData(TweenFadeData data) {
      data.TargetData = new float[] { m_sprite.color.a };
      return data;
    }

    public void SetTargetData(TweenFadeData data) {
      float alpha = data.TargetData[0];
      Color color = m_sprite.color;
      color.a = alpha;
      m_sprite.color = color;
    }
    #endregion
  }
}
