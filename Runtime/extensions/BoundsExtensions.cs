using UnityEngine;

namespace Wowsome {
  public static class BoundsExtensions {
    public static bool IsVisibleFrom(this Bounds bounds, Camera camera) {
      Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
      return GeometryUtility.TestPlanesAABB(planes, bounds);
    }

    public static bool IsFullyVisibleFrom(this Bounds bounds, Camera camera) {
      Bounds cameraBounds = camera.OrthoBounds();

      return cameraBounds.Contains(bounds.center.SetZ(cameraBounds.center.z));
    }
  }
}