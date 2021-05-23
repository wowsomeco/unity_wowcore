using UnityEngine;

namespace Wowsome {
  public static class Utils {
    public static float AspectRatioScaler(Vector2 referenceResolution) {
      return (referenceResolution.x / referenceResolution.y) / ((float)Screen.width / Screen.height);
    }
  }
}