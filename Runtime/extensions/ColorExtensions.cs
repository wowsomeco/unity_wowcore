using UnityEngine;

namespace Wowsome {
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

    public static bool IsTransparent(this Color32 c) {
      return c.a == 0;
    }

    public static Color32 WithAlpha(this Color32 color, byte a) {
      return new Color32(color.r, color.g, color.b, a);
    }

    public static Color CombineColors(params Color[] colors) {
      Color result = new Color(0, 0, 0, 0);
      foreach (Color c in colors) {
        result += c;
      }

      result /= colors.Length;
      return result;
    }

    public static Color Darken(this Color col, float factor) {
      return Color.Lerp(col, Color.black, factor);
    }

    public static Color Lighten(this Color col, float factor) {
      return Color.Lerp(col, Color.white, factor);
    }
  }
}
