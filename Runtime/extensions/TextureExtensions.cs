using System.IO;
using UnityEngine;

namespace Wowsome {
  public static class TextureExt {
    public static Sprite ToSprite(this Texture2D texture) {
      Sprite sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.one * 0.5f);
      return sprite;
    }

    public static Sprite ToSprite(this string filePath) {
      return filePath.ToTexture2D().ToSprite();
    }

    public static Texture2D ToTexture2D(this string filePath) {
      Texture2D t = new Texture2D(1, 1);
      t.LoadImage(File.ReadAllBytes(filePath));
      return t;
    }
  }

  public static class ColorExt {
    public static string ColorToHex(Color32 color) {
      string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
      return hex;
    }

    public static Color HexToColor(string hex) {
      // in case the string is formatted 0xFFFFFF
      hex = hex.Replace("0x", "");
      // in case the string is formatted #FFFFFF
      hex = hex.Replace("#", "");
      // assume fully visible unless specified in hex
      byte a = 255;
      byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
      byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
      byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
      // Only use alpha if the string has enough characters
      if (hex.Length == 8) {
        a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
      }
      return new Color32(r, g, b, a);
    }

    public static float[] ToFloats(this Color col) {
      return new float[] { col.r, col.g, col.b, col.a };
    }

    public static Color SetAlpha(this Color col, float a) {
      return new Color(col.r, col.g, col.b, a);
    }
  }
}