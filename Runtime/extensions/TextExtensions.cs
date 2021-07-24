using UnityEngine;
using UnityEngine.UI;

namespace Wowsome {
  public static class TextExt {
    public static void SetColor(this Text text, float[] rgba) {
      Color color = text.color;
      for (int i = 0; i < rgba.Length; ++i) {
        color[i] = rgba[i];
      }
      text.color = color;
    }

    public static void SetAlpha(this Text txt, float alpha) {
      Color color = txt.color;
      color.a = alpha;
      txt.color = color;
    }

    public static float Alpha(this Text txt) {
      return txt.color.a;
    }
  }
}