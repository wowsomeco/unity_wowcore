using UnityEngine;

namespace Wowsome {
  public static class BoundsExtensions {
    public static bool IsVisibleFrom(this Bounds bounds, Camera camera) {
      Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
      return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
  }
}