using UnityEngine;

namespace Wowsome {
  public static class ScreenExtensions {
    public static float AspectRatio() {
      return (float)Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
    }
  }
}