using UnityEngine;
using UnityEngine.UI;

namespace Wowsome {
  public static class ImageExtensions {
    public static void SetMaxSize(this Image img, float maxSize) {
      img.SetNativeSize();
      img.rectTransform.SetMaxSize(maxSize);
    }

    public static void SetColor(this Image img, float[] rgba) {
      Color color = img.color;
      for (int i = 0; i < rgba.Length; ++i) {
        color[i] = rgba[i];
      }
      img.color = color;
    }

    public static void SetColor(this Image img, Color color) {
      img.color = color;
    }

    public static void SetAlpha(this Image img, float alpha) {
      Color color = img.color;
      color.a = Mathf.Clamp(alpha, 0f, 1f);
      img.color = color;
    }

    public static void AddAlpha(this Image img, float delta) {
      float cur = img.Alpha();
      img.SetAlpha(cur + delta);
    }

    public static float Alpha(this Image img) {
      return img.color.a;
    }

    public static Vector2 Pos(this Image img) {
      return img.rectTransform.Pos();
    }

    public static void SetPos(this Image img, Vector2 pos) {
      img.rectTransform.SetPos(pos);
    }

    public static void SetScale(this Image img, Vector2 scale) {
      img.rectTransform.SetScale(scale);
    }

    public static void SetScale(this Image img, float scale) {
      img.rectTransform.SetScale(new Vector2(scale, scale));
    }

    public static void SetRotation(this Image img, float rot) {
      img.rectTransform.SetRotation(rot);
    }

    public static void SetParent(this Image img, RectTransform parent) {
      img.rectTransform.SetParent(parent);
    }

    public static void SetWidth(this Image img, float w) {
      img.rectTransform.SetWidth(w);
    }

    public static void SetHeight(this Image img, float h) {
      img.rectTransform.SetHeight(h);
    }
  }
}