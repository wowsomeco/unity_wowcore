using System;
using UnityEngine;

namespace Wowsome {
  public class Print {
    public class Txt {
      public string Text { get; private set; }
      public string Color { get; private set; }

      public string GetText {
        get { return string.Format("<color={0}>{1}</color>", Color, Text); }
      }

      public Txt(object t, string c) {
        Text = t.ToString();
        Color = c ?? "white";
      }

      public Txt(object t) : this(t, "white") { }
    }

    public static void Log(Func<string> color, params object[] msg) {
#if UNITY_EDITOR
      string str = msg.Map(x => x.ToString()).Flatten();
      Txt t = new Txt(str, color?.Invoke());
      Debug.Log(t.GetText);
#endif
    }

    public static void Log(params object[] msg) {
#if UNITY_EDITOR
      Log(() => "white", msg);
#endif
    }

    public static void Log(params Txt[] texts) {
#if UNITY_EDITOR
      string t = texts.Fold(string.Empty, (p, c) => p += (c.GetText + " "));
      Debug.Log(t);
#endif
    }

    public static void Info(params object[] msg) {
      Log(() => "cyan", msg);
    }

    public static void Warn(params object[] msg) {
      Log(() => "yellow", msg);
    }

    public static void Error(params object[] msg) {
      Log(() => "red", msg);
    }
  }
}