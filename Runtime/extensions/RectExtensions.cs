using UnityEngine;

namespace Wowsome {
  public static class RectExt {
    /// <summary>
    /// Checks the whether these 2 rects are currently intersecting
    /// based on https://stackoverflow.com/questions/306316/determine-if-two-rectangles-overlap-each-other
    /// </summary>
    public static bool Intersects(this Rect r1, Rect r2) {
      if (r1.xMin < r2.xMax && r1.xMax > r2.xMin &&
        r1.yMax > r2.yMin && r1.yMin < r2.yMax) {
        return true;
      }

      return false;
    }

    public static Bounds ToBounds(this Rect r) {
      return new Bounds(r.center, r.size);
    }
  }
}
