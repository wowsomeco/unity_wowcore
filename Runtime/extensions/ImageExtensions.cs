using UnityEngine;
using UnityEngine.UI;

namespace Wowsome {
  public static class ImageExtensions {
    public static Image SetNativeSizeWithMul(this Image img, float multiplier) {
      img.SetNativeSize();
      img.rectTransform.MultiplySize(multiplier);

      return img;
    }

    public static Image SetMaxSize(this Image img, float maxSize) {
      img.SetNativeSize();
      img.rectTransform.SetMaxSize(maxSize);

      return img;
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

    public static Image AddAlpha(this Image img, float delta) {
      float cur = img.Alpha();
      img.SetAlpha(cur + delta);

      return img;
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

    public static Image SetParent(this Image img, RectTransform parent) {
      img.rectTransform.SetParent(parent);
      return img;
    }

    public static float Width(this Image img) {
      return img.rectTransform.Width();
    }

    public static Image SetWidth(this Image img, float w) {
      img.rectTransform.SetWidth(w);
      return img;
    }

    public static float Height(this Image img) {
      return img.rectTransform.Height();
    }

    public static Image SetHeight(this Image img, float h) {
      img.rectTransform.SetHeight(h);
      return img;
    }

    public static Vector2 Size(this Image img) {
      return img.rectTransform.Size();
    }

    public static Image SetSize(this Image img, Vector2 size) {
      img.rectTransform.SetSize(size);
      return img;
    }

    public static Image SetSprite(this Image img, Sprite spr) {
      img.sprite = spr;
      return img;
    }

    public static Image Normalize(this Image img) {
      return img.SetAlpha(1f).SetRotation(0f).SetScale(1f);
    }
  }
}