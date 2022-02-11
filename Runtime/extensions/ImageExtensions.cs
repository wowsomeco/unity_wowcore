using UnityEngine;
using UnityEngine.UI;

namespace Wowsome {
  public static class ImageExtensions {
    public static void SetNativeSizeWithMul(this Image img, float multiplier) {
      img.SetNativeSize();
      img.rectTransform.MultiplySize(multiplier);
    }

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

    public static Image SetColor(this Image img, Color color) {
      img.color = color;
      return img;
    }

    public static Image SetAlpha(this Image img, float alpha) {
      Color color = img.color;
      color.a = Mathf.Clamp(alpha, 0f, 1f);
      img.color = color;

      return img;
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

    public static Image SetPos(this Image img, Vector2 pos) {
      img.rectTransform.SetPos(pos);
      return img;
    }

    public static Image SetScale(this Image img, Vector2 scale) {
      img.rectTransform.SetScale(scale);
      return img;
    }

    public static Image SetScale(this Image img, float scale) {
      img.rectTransform.SetScale(new Vector2(scale, scale));
      return img;
    }

    public static float Rotation(this Image img) {
      return img.rectTransform.Rotation();
    }

    public static Image SetRotation(this Image img, float rot) {
      img.rectTransform.SetRotation(rot);
      return img;
    }

    public static void SetParent(this Image img, RectTransform parent) {
      img.rectTransform.SetParent(parent);
    }

    public static float Width(this Image img) {
      return img.rectTransform.Width();
    }

    public static void SetWidth(this Image img, float w) {
      img.rectTransform.SetWidth(w);
    }

    public static float Height(this Image img) {
      return img.rectTransform.Height();
    }

    public static void SetHeight(this Image img, float h) {
      img.rectTransform.SetHeight(h);
    }

    public static Image Normalize(this Image img) {
      img.SetAlpha(1f);
      img.SetRotation(0f);
      img.SetScale(1f);

      return img;
    }
  }
}